using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Contexts;
using PewPew_Paradise.Maths;
using System.Diagnostics;
namespace PewPew_Paradise.GameLogic
{
    public class GameManager
    {
        public static GameManager Instance;
        public const double GameUnitSize = 16.0;
        public const double GameResolution = 256.0;

        private int _minimumDelta;
        private Action<object> _updateAction;
        Thread UpdateThread;
        public SpriteManager spriteManager;


        public delegate void UpdateDelegate();
        public event UpdateDelegate OnUpdate;

        private static double _lastTime;
        private static double _deltaTime;
        private static Stopwatch _stopWatch = new Stopwatch();


        public static double DeltaTime
        {
            get
            {
                return _deltaTime;
            }
        }


        public GameManager(int frameRate)
        {
            Instance = this;
            _updateAction = new Action<object>(delegate (object param) { Update(); });
            this.FrameRate = frameRate;
            spriteManager = new SpriteManager();
            spriteManager.LoadImage("Images/Sprites/Characters/MrPlaceHolder.png","MrPlaceHolder");
        }



        int FrameRate
        {
            get
            {
                return 1000 / (_minimumDelta + 1);
            }
            set
            {
                _minimumDelta = 1000 / value - 1;
            }
        }

        private void UpdateCall()
        {
            if (_minimumDelta < 1)_minimumDelta = 1;
            MainWindow.Instance.Dispatcher.Invoke(_updateAction,new object[] { 0 });
            Thread.Sleep((int)Math.Max(0,_minimumDelta - (_stopWatch.ElapsedMilliseconds - _lastTime)));
            UpdateCall();
        }

        protected void Update()
        {
            _deltaTime = _stopWatch.Elapsed.TotalMilliseconds - _lastTime;
            OnUpdate.Invoke();
            _lastTime = _stopWatch.ElapsedMilliseconds;
        }

        

        public void Begin()
        {
            _stopWatch.Start();
            UpdateThread = new Thread(UpdateCall);
            UpdateThread.Priority = ThreadPriority.Lowest;
            UpdateThread.Start();
        }

        public void Stop()
        {
            _stopWatch.Stop();
            if (UpdateThread != null)
            {
                UpdateThread.Abort();
            }
        }


    }
}
