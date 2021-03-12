using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Contexts;
using PewPew_Paradise.Maths;
using System.Diagnostics;
using System.Windows.Threading;
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

        //Update Event
        public delegate void UpdateDelegate();
        public static event UpdateDelegate OnPreUpdate;
        public static event UpdateDelegate OnUpdate;
        public static event UpdateDelegate OnPostUpdate;

        //Timer for update call
        private static DispatcherTimer _dispatherTimer = new DispatcherTimer();

        //variables used to calculate DeltaTime
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
        /// Initialize GameManager
        /// </summary>
        /// <param name="frameRate"></param>
        /// 
        public static void Init(int frameRate)
        {
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
                _minimumDelta = Math.Max(1,1000 / value - 1);
            }
        }

       

        /// <summary>
        /// Function that calls the OnUpdate event [Do not call]
        /// </summary>
        protected static void Update(object state, EventArgs e)
        {
            _deltaTime = _stopWatch.Elapsed.TotalMilliseconds - _lastTime;
            Console.WriteLine(_deltaTime);
            if (OnPreUpdate != null)
            {
                OnPreUpdate.Invoke();
            }
            if (OnUpdate != null) {
                OnUpdate.Invoke();
            }
            if (OnPostUpdate != null)
            {
                OnPostUpdate.Invoke();
            }
            _lastTime = _stopWatch.ElapsedMilliseconds;
        }


        /// <summary>
        /// Start the game Update loop
        /// </summary>
        public static void Begin()
        {
            
            _stopWatch.Start();
            _dispatherTimer.Interval = TimeSpan.FromMilliseconds(_minimumDelta);
            _dispatherTimer.Tick += Update;
            _dispatherTimer.Start();
        }

        /// <summary>
        /// Stop the game Update loop
        /// </summary>
        public static void Stop()
        {
            _stopWatch.Stop();
            _dispatherTimer.Stop();
        }


    }
}
