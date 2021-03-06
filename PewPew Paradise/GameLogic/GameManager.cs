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
        public static GameManager Instance;
        public const double GameUnitSize = 16.0;


        private int _minimumDelta;
        private Action<object> _updateAction;
        Thread UpdateThread;
        public SpriteManager spriteManager;

        public Sprite MrPlaceHolder;



        public GameManager(int frameRate)
        {
            Instance = this;
            _updateAction = new Action<object>(delegate (object param) { Update(); });
            this.FrameRate = frameRate;
            spriteManager = new SpriteManager();
            spriteManager.LoadImage("Images/Sprites/Characters/MrPlaceHolder.png","MrPlaceHolder");
            MrPlaceHolder = spriteManager.CreateSprite("MrPlaceHolder",new Vector2(8,8),new Vector2(-4,-4));
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
