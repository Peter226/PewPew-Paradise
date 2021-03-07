using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Contexts;
using PewPew_Paradise.Maths;
namespace PewPew_Paradise.GameLogic
{
    public class GameManager
    {
        /// <summary>
        /// Current active GameManager
        /// </summary>
        public static GameManager instance;

        /// <summary>
        /// The SpriteManager of the current GameManager Instance
        /// (Used for creating and destroying sprites, loading images for sprites to use, etc...)
        /// </summary>
        public SpriteManager spriteManager;

        /// <summary>
        /// The size of the screen in game units
        /// </summary>
        public const double GameUnitSize = 16.0;
        /// <summary>
        /// The resolution of the pixel art
        /// </summary>
        public const double GameResolution = 256.0;

        /// <summary>
        /// Delegate for OnUpdate event. Subscribe method to OnUpdate event to have it be called every frame.
        /// </summary>
        public delegate void UpdateDelegate();
        /// <summary>
        /// Subscribe a method to have it be called every frame.
        /// </summary>
        public event UpdateDelegate OnUpdate;

        /// <summary>
        /// The time between two frames (in ms)
        /// </summary>
        private int _minimumDelta;

        
        private Action<object> _updateAction;
        Thread UpdateThread;




        public GameManager(int frameRate)
        {
            instance = this;
            _updateAction = new Action<object>(delegate (object param) { Update(); });
            this.FrameRate = frameRate;
            spriteManager = new SpriteManager();
            spriteManager.LoadImage("Images/Sprites/Characters/MrPlaceHolder.png","MrPlaceHolder");
        }



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

        /// <summary>
        /// Update thread call
        /// </summary>
        private void UpdateCall()
        {
            if (_minimumDelta < 1)_minimumDelta = 1;
            MainWindow.Instance.Dispatcher.Invoke(_updateAction,TimeSpan.Zero,new object[] { 0 });
            Thread.Sleep(_minimumDelta);
            UpdateCall();
        }


        protected void Update()
        {
            OnUpdate.Invoke();
        }

        
        /// <summary>
        /// Start the game loop
        /// </summary>
        public void Begin()
        {
            UpdateThread = new Thread(UpdateCall);
            UpdateThread.Start();
        }

        /// <summary>
        /// Stop the game loop
        /// </summary>
        public void Stop()
        {
            if (UpdateThread != null)
            {
                UpdateThread.Abort();
            }
        }


    }
}
