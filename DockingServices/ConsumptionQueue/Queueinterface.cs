using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsumptionQueue
{
 
    public class Queueinterface<T>
    {

            public static AsyncQueue<T> queue = new AsyncQueue<T>();
            public static AsyncQueue<T> SendQueue = new AsyncQueue<T>();

    }
}
