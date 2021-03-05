using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Contexts;
namespace PewPew_Paradise.GameLogic
{
    public class GameManager
    {
        private int _minimumDelta;
        private Action<object> _updateAction;

        Thread UpdateThread;

        int FrameRate
        {
            get
            {
                return 1000 / _minimumDelta;
            }
            set
            {
                _minimumDelta = 1000 / value;
            }
        }

        private void UpdateCall()
        {
            if (_minimumDelta < 1)_minimumDelta = 1;
            MainWindow.Instance.Dispatcher.Invoke(_updateAction,TimeSpan.Zero,new object[] { 0 });
            Thread.Sleep(_minimumDelta);
            UpdateCall();
        }

        protected void Update()
        {
            MainWindow.Instance.Test();

            


        }

        public GameManager(int frameRate)
        {
            _updateAction = new Action<object>(delegate(object param) { Update(); });
            this.FrameRate = frameRate;
        }

        public void Begin()
        {
            UpdateThread = new Thread(UpdateCall);
            UpdateThread.Start();
        }

        public void Stop()
        {
            if (UpdateThread != null)
            {
                UpdateThread.Abort();
            }
        }


    }
}
