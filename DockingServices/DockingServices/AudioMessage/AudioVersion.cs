using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingServices.AudioMessage
{
   public interface AudioVersion<T>
    {
        void Start();
        void Stop();

        string Install(T t);

        string Send();

    }
}
