using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bachelin_Merchant
{
    class SplashThread
    {
        private Thread thread;
        private LOADING form;
        private EventWaitHandle loaded;

        public SplashThread()
        {
            thread = new Thread(new ThreadStart(RunSplash));
            loaded = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        public void Open()
        {
            thread.Start();
        }

        public void CloseEnd()
        {
            Close();
            Join();
        }

        public void Close()
        {
            loaded.WaitOne();

            form.Invoke(new CloseCallback(form.Close));
        }

        public void Join()
        {
            thread.Join();
        }

        private void RunSplash()
        {
            form = new LOADING();
            form.Load += new EventHandler(OnLoad);
            form.ShowDialog();
        }

        void OnLoad(object sender, EventArgs e)
        {
            loaded.Set();
        }

        private delegate void CloseCallback();
    }
}
