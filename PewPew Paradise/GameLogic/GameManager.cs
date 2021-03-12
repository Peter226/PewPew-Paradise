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
        public static string GameAssetPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "PewPew_Paradise_Assets");

        /// <summary>
        /// Screen in game units
        /// </summary>
        public const double GameUnitSize = 16.0;

        /// <summary>
        /// Resolution of screen art (map) in pixels
        /// </summary>
        public const double GameResolution = 256.0;

        //Update Thread
        private static int _minimumDelta;
        private static Action<object> _updateAction;
        static Thread UpdateThread;

        //Update Event
        public delegate void UpdateDelegate();
        public static event UpdateDelegate OnUpdate;

        //variables used to calculate DeltaTime
        private static double _lastTime;
        private static double _deltaTime;
        private static Stopwatch _stopWatch = new Stopwatch();

        //fliplock
        private static bool _threadFlipLock;
        private static bool _threadLastLock;

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
        /// Initialize GameManager
        /// </summary>
        /// <param name="frameRate"></param>
        /// 
        public static void Init(int frameRate)
        {
            _updateAction = new Action<object>(delegate (object param) { Update(); });
            FrameRate = frameRate;
            SpriteManager.LoadImage("Images/Sprites/Characters/MrPlaceHolder.png", "MrPlaceHolder");
            OnUpdate += SpriteManager.UpdateSprites;
        }

        

        /// <summary>
        /// Get or set the frame rate of the game
        /// </summary>
        public static int FrameRate
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
        private static void UpdateCall()
        {
            if (_threadFlipLock != _threadLastLock) {
                _threadLastLock = _threadFlipLock;
                if (_minimumDelta < 1) _minimumDelta = 1;
                MainWindow.Instance.Dispatcher.Invoke(_updateAction, new object[] { 0 });
            }
            Thread.Sleep((int)Math.Max(0, _minimumDelta - (_stopWatch.ElapsedMilliseconds - _lastTime)));
            UpdateCall();
           
        }

        /// <summary>
        /// Function that calls the OnUpdate event [Do not call]
        /// </summary>
        protected static void Update()
        {
            _threadFlipLock = !_threadFlipLock;
            _deltaTime = _stopWatch.Elapsed.TotalMilliseconds - _lastTime;
            OnUpdate.Invoke();
            _lastTime = _stopWatch.ElapsedMilliseconds;
        }


        /// <summary>
        /// Start the game Update loop
        /// </summary>
        public static void Begin()
        {
            _threadFlipLock = false;
            _threadLastLock = true;
            _stopWatch.Start();
            UpdateThread = new Thread(UpdateCall);
            UpdateThread.Priority = ThreadPriority.Lowest;
            UpdateThread.Start();
        }

        /// <summary>
        /// Stop the game Update loop
        /// </summary>
        public static void Stop()
        {
            _stopWatch.Stop();
            if (UpdateThread != null)
            {
                UpdateThread.Abort();
            }
        }


    }
}
