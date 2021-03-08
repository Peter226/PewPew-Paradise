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

    /// <summary>
    /// class that implements the core game functions
    /// </summary>
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

        /// <summary>
        /// Get the elapsed time in milliseconds between two Update() calls
        /// </summary>
        public static double DeltaTime
        {
            get
            {
                return _deltaTime;
            }
        }

        /// <summary>
        /// Create a new game instance
        /// </summary>
        /// <param name="frameRate"></param>
        public GameManager(int frameRate)
        {
            Instance = this;
            _updateAction = new Action<object>(delegate (object param) { Update(); });
            this.FrameRate = frameRate;
            spriteManager = new SpriteManager();
            spriteManager.LoadImage("Images/Sprites/Characters/MrPlaceHolder.png","MrPlaceHolder");
        }


        /// <summary>
        /// Get or set the frame rate of the game
        /// </summary>
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

        /// <summary>
        /// Update function thread call that invokes a callback in the main thread [Do not call]
        /// </summary>
        private void UpdateCall()
        {
            if (_minimumDelta < 1)_minimumDelta = 1;
            MainWindow.Instance.Dispatcher.Invoke(_updateAction,new object[] { 0 });
            Thread.Sleep((int)Math.Max(0,_minimumDelta - (_stopWatch.ElapsedMilliseconds - _lastTime)));
            UpdateCall();
        }

        /// <summary>
        /// Function that calls the OnUpdate event [Do not call]
        /// </summary>
        protected void Update()
        {
            _deltaTime = _stopWatch.Elapsed.TotalMilliseconds - _lastTime;
            OnUpdate.Invoke();
            _lastTime = _stopWatch.ElapsedMilliseconds;
        }

        
        /// <summary>
        /// Start the game Update loop
        /// </summary>
        public void Begin()
        {
            _stopWatch.Start();
            UpdateThread = new Thread(UpdateCall);
            UpdateThread.Priority = ThreadPriority.Lowest;
            UpdateThread.Start();
        }

        /// <summary>
        /// Stop the game Update loop
        /// </summary>
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
