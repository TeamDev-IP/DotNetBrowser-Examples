using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Demo
{
    internal class WinFormsUIContext
    {
        public SynchronizationContext Value
        {
            get;
            set;
        }

        private static WinFormsUIContext instance;
        public static WinFormsUIContext Instance
        {
            get 
            { 
                if(instance == null)
                {
                    instance =  new WinFormsUIContext();
                }
                return  instance; 
            }
        }

        public void Send(SendOrPostCallback callback, object state)
        {
            if (this.Value == null)
            {
                return;
            }
            this.Value.Send(callback, state);
        }

        public void Post(SendOrPostCallback callback, object state)
        {
            if (this.Value == null)
            {
                return;
            }
            this.Value.Post(callback, state);
        }

    }
}
