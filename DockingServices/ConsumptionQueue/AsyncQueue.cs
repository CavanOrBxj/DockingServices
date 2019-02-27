using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ConsumptionQueue
{

    [Serializable]
    public class EventArgs<T> : System.EventArgs
    {
        public T Argument;

        public EventArgs() : this(default(T))
        {
        }

        public EventArgs(T argument)
        {
            Argument = argument;
        }
    }
    public  class AsyncQueue<T>
    {
        //队列是否正在处理数据
        private int isProcessing;
        //有线程正在处理数据
        private const int Processing = 1;
        //没有线程处理数据
        private const int UnProcessing = 0;
        //队列是否可用 单线程下用while来判断，多线程下用if来判断，随后用while来循环队列的数量
        private volatile bool enabled = true;
        // 消费者线程
        private Task currentTask;
        // 消费者线程处理事件
        public event Action<T> ProcessItemFunction;

        public delegate bool UpdateIdDelegate(ref T item);
        public delegate bool QueryQueueDelegate(T t,T Newobject);
        //修改当前ID
        public event UpdateIdDelegate UpdateIdEvent;
        // 查询队列ID值是否存在
        public event QueryQueueDelegate QueryQueueExistsEvent;
        //消费者线程处理事件
        public event EventHandler<EventArgs<Exception>> ProcessException;
        // 并发队列
        private ConcurrentQueue<T> queue;//高效的线程安全的队列  修改于20180813
        // 消费者的数量
        private int _internalTaskCount;
        // 存储消费者队列
        List<Task> tasks = new List<Task>();

        public AsyncQueue()
        {
            _internalTaskCount = 10;
            queue = new ConcurrentQueue<T>();
            Start();
        }

        public bool UpdateId(ref T t)
        {
           return  UpdateIdEvent(ref t);
        }
        public bool QueryQueue(T t)
        {

            for (int i = 0; i < queue.Count; i++)
            {

                try
                {
                    T publishFrame;
                    if (queue.TryPeek(out publishFrame))
                    {
                
                       bool flag = QueryQueueExistsEvent(publishFrame,t);
                        if (flag)
                            return true;
                    }
                }
                catch (Exception ex)
                {
                    OnProcessException(ex);
                }
            }
            return false;
        }
        public int Count
        {
            get
            {
                return queue.Count;
            }
        }
        // 开启监听线程
        private void Start()
        {
            Thread process_Thread = new Thread(PorcessItem);
            process_Thread.IsBackground = true;
            process_Thread.Start();
        }

        // 生产者生产
        public void Enqueue(T items)
        {
            if (items == null)
            {
                throw new ArgumentException("items");
            }
            if (QueryQueue(items))
            {
                UpdateId(ref items);
            }
            queue.Enqueue(items);
            DataAdded();
        }

        //数据添加完成后通知消费者线程处理
        private void DataAdded()
        {
            if (enabled)
            {
                if (!IsProcessingItem())
                {
                    // 开启消费者消费队列
                    ProcessRangeItem();
                }
            }
        }

        //判断是否队列有线程正在处理 
        private bool IsProcessingItem()
        {
            return !(Interlocked.CompareExchange(ref isProcessing, Processing, UnProcessing) == 0);
        }

        private void ProcessRangeItem()
        {
            for (int i = 0; i < _internalTaskCount; i++)
            {
                currentTask = Task.Factory.StartNew(() => ProcessItemLoop());
                tasks.Add(currentTask);
            }
        }
        // 消费者处理事件
        private void ProcessItemLoop()
        {
            Console.WriteLine("正在执行的Task的Id: {0}", Task.CurrentId);
            // 队列为空，并且队列不可用
            if (!enabled && queue.IsEmpty)
            {
                Interlocked.Exchange(ref isProcessing, 0);
                return;
            }
            //处理的线程数 是否小于当前最大任务数
            //if (Thread.VolatileRead(ref runingCore) <= this.MaxTaskCount)
            //{
            T publishFrame;

            while (enabled)
            {
                if (queue.TryDequeue(out publishFrame))
                {
                    try
                    {
                        // 消费者处理事件
                        ProcessItemFunction(publishFrame);
                    }
                    catch (Exception ex)
                    {
                        OnProcessException(ex);
                    }
                }
                else
                {
                    Console.WriteLine("线程Id{0}取队列失败，跳出循环", Task.CurrentId);
                    break;
                }
            }
        }

        /// <summary>
        ///定时处理线程调用函数  
        ///主要是监视入队的时候线程 没有来的及处理的情况
        /// </summary>
        private void PorcessItem(object state)
        {
            int sleepCount = 0;
            int sleepTime = 1000;
            while (enabled)
            {
                //如果队列为空则根据循环的次数确定睡眠的时间
                if (queue.IsEmpty)
                {
                    // Task消费者消费完了队列中的数据....注销掉消费者线程
                    if (tasks.Count == _internalTaskCount)
                    {
                        Flush();
                    }
                    if (sleepCount == 0)
                    {
                        sleepTime = 1000;
                    }
                    else if (sleepCount <= 3)
                    {
                        sleepTime = 1000 * 3;
                    }
           
                    sleepCount++;
                    Thread.Sleep(sleepTime);
                }
                else
                {
                    //判断是否队列有线程正在处理 
                    if (enabled && Interlocked.CompareExchange(ref isProcessing, Processing, UnProcessing) == 0)
                    {
                        if (!queue.IsEmpty)
                        {
                            currentTask = Task.Factory.StartNew(ProcessItemLoop);
                            tasks.Add(currentTask);
                        }
                        else
                        {
                            //队列为空，已经取完了
                            Interlocked.Exchange(ref isProcessing, 0);
                        }
                        sleepCount = 0;
                        sleepTime = 1000;
                    }
            
                }
            }
        }

        //更新并关闭消费者
        public void Flush()
        {
         //   Stop();
            foreach (var t in tasks)
            {
                if (t != null)
                {
                    t.Wait();
                    Console.WriteLine("Task已经完成");
                }
            }

            // 消费者未消费完
            while (!queue.IsEmpty)
            {
                try
                {
                    T publishFrame;
                    if (queue.TryDequeue(out publishFrame))
                    {
                        ProcessItemFunction(publishFrame);
                    }
                }
                catch (Exception ex)
                {
                    OnProcessException(ex);
                }
            }
            currentTask = null;
            tasks.Clear();
            Interlocked.Exchange(ref isProcessing, 0);
        }

        public void Stop()
        {
            this.enabled = false;
        }

        private void OnProcessException(System.Exception ex)
        {
            var tempException = ProcessException;
            Interlocked.CompareExchange(ref ProcessException, null, null);

            if (tempException != null)
            {
                ProcessException(ex, new EventArgs<Exception>(ex));
            }
        }
    }
}
