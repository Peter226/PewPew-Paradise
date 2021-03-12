﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using PewPew_Paradise.GameLogic;
using PewPew_Paradise.Maths;
using System.ComponentModel;
using PewPew_Paradise.Editor;
namespace PewPew_Paradise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow instance;

        double windowDifferenceX;
        double windowDifferenceY;

        double previousWidth;
        double previousHeight;

        float Timer = 0;

        List<Sprite> MrPlaceHolders = new List<Sprite>();

        MediaPlayer mp = new MediaPlayer();

        public MapLoad load;
        public CharacterSelect chars;

        /// <summary>
        /// Get instance
        /// </summary>
        public static MainWindow Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public MainWindow()
        {
            instance = this;
            InitializeComponent();
            Thickness thickness = new Thickness();
            GameWindow.Margin = thickness;
            previousHeight = GameWindow.Height;
            previousWidth = GameWindow.Width;
            
            GameManager.Init(60);
            GameManager.OnUpdate += Update;
            GameManager.Begin();
            CollisionEditor.Init(collisionEditor);



            load = new MapLoad();
            chars = new CharacterSelect();


            string l = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string p = System.IO.Path.GetDirectoryName(l);
            p = System.IO.Path.Combine(p, "PewPew_Paradise_Assets/Music/MainMenu.mp3");
            Console.WriteLine(p);
            mp.Open(new Uri(p));
            mp.MediaOpened += new EventHandler(PlayMedia);


            

            AnimationCollection playerAnimations = new AnimationCollection("Player",Vector2.One,Vector2.One * 4,1);
            SpriteAnimation jumpAnimation = new SpriteAnimation(150, false, 2);
            playerAnimations.animations.Add(jumpAnimation);
            jumpAnimation.keyFrames.Add(new Vector2(0, 0));
            jumpAnimation.keyFrames.Add(new Vector2(1, 0));
            jumpAnimation.keyFrames.Add(new Vector2(2, 0));
            jumpAnimation.keyFrames.Add(new Vector2(3, 0));

            SpriteAnimation walkAnimation = new SpriteAnimation(100, true);
            playerAnimations.animations.Add(walkAnimation);
            walkAnimation.keyFrames.Add(new Vector2(0, 1));
            walkAnimation.keyFrames.Add(new Vector2(1, 1));
            walkAnimation.keyFrames.Add(new Vector2(2, 1));
            walkAnimation.keyFrames.Add(new Vector2(1, 1));
            SpriteManager.AddAnimationCollection(playerAnimations,"Player");


        }

        /// <summary>
        /// Music
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        public void PlayMedia(object sender, EventArgs a)
        {
            Console.WriteLine("Opened Media");
            mp.Play();
            double k = (double)sl_music.Value;
            mp.Volume = k / 100;

        }


        /// <summary>
        /// Animation loop
        /// </summary>
        public void Update()
        {
            Timer -= (float)(GameManager.DeltaTime * 0.00075);
            foreach (SpriteAnimated sprite in MrPlaceHolders)
            {
                Vector2 newpos = sprite.Position;
                newpos.x += Math.Sin(Timer * 2.2 + Math.Cos(newpos.y) * 0.1f) * 0.1f;
                newpos.y += Math.Sin(Timer * 3.3 + Math.Cos(newpos.x) * 0.1f) * 0.1f;
                sprite.Position = newpos;
            }
            // Moving arrows by sin timer
            MatrixTransform mt_left= new MatrixTransform();
            Matrix matrix_left = mt_left.Matrix;
            matrix_left.Translate(10 * Math.Sin(Timer * 10), 0);
            mt_left.Matrix = matrix_left;
            img_left_arrow.RenderTransform = mt_left;

            MatrixTransform mt_right = new MatrixTransform();
            Matrix matrix_right = mt_right.Matrix;
            matrix_right.Translate(-10 * Math.Sin(Timer * 10), 0);
            mt_right.Matrix = matrix_right;
            img_right_arrow.RenderTransform = mt_right;

        }

     

        /// <summary>
        /// Detect key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyPress(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.P) {
                SpriteAnimated mrph = new SpriteAnimated("MrPlaceHolder", "Player", new Maths.Vector2(8, 8), new Maths.Vector2(4, 4));
                MrPlaceHolders.Add(mrph);
                mrph.AddComponent<GameLogic.SpriteComponents.SpriteRandomizer>();
            }
            if (e.Key == Key.Space)
            {
                foreach (SpriteAnimated spriteAnimated in MrPlaceHolders)
                {
                    spriteAnimated.PlayAnimation(0);
                }
            }
            if (e.Key == Key.Delete)
            {
                for (int i = MrPlaceHolders.Count - 1; i >= 0;i--)
                {
                    MrPlaceHolders[i].Destroy();
                    MrPlaceHolders.RemoveAt(i);
                }
            }
            //This is for just testing now
            if(e.Key == Key.M)
            {
                load.NextMap(2);
            }
            if (e.Key == Key.N)
            {
                load.NextMap(1);
            }
            if(e.Key == Key.A)
            {
               chars.MoveLeft(1);
            }
            if(e.Key == Key.D)
            {
                chars.MoveRight(1);
            }
            if(e.Key == Key.Left)
            {
                chars.MoveLeft(2);
            }
            if(e.Key == Key.Right)
            {
                chars.MoveRight(2);
            }
        }

        /// <summary>
        /// Detect Key lift
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyLift(object sender, KeyEventArgs e)
        {

        }

        /// <summary>
        /// Resize GameWindow when MainWindow changes it's size
        /// </summary>
        /// <param name="sizeInfo"></param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {

            if (this.SizeToContent != SizeToContent.Manual)
            {
                windowDifferenceX = this.ActualWidth - this.Width;
                windowDifferenceY = this.ActualHeight - this.Height;
            }

            double minSize = Math.Min(RealWidth, RealHeight);

            double spaceX = RealWidth - minSize;
            double spaceY = RealHeight - minSize;
            GameWindow.Width = minSize;
            GameWindow.Height = minSize;
            Thickness thickness = new Thickness(spaceX * 0.5, spaceY * 0.5, 0, 0);
            GameWindow.Margin = thickness;
        }

        public double RealWidth
        {
            get { return this.ActualWidth - windowDifferenceX; }
        }
        public double RealHeight
        {
            get { return this.ActualHeight - windowDifferenceY; }
        }

        /// <summary>
        /// Resize every UIElement in window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            double ratioW = GameWindow.Width / previousWidth;
            double ratioH = GameWindow.Height / previousHeight;


            foreach (UIElement uIElement in GameWindow.Children)
            {
                ResizeObjectsCascade(uIElement, ratioW, ratioH);
            }
            previousHeight = GameWindow.Height;
            previousWidth = GameWindow.Width;
        }

        /// <summary>
        /// Resize every child of every element
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void ResizeObjectsCascade(UIElement parent, double width, double height)
        {
            if (typeof(FrameworkElement).IsAssignableFrom(parent.GetType()))
            {
                FrameworkElement frameworkElement = (FrameworkElement)parent;
                frameworkElement.Width *= width;
                frameworkElement.Height *= height;
                Thickness margin = frameworkElement.Margin;
                margin.Left *= width;
                margin.Right *= width;
                margin.Top *= height;
                margin.Bottom *= height;

                if (typeof(Control).IsAssignableFrom(parent.GetType()))
                {
                    Control control = (Control)parent;
                    control.FontSize *= width;
                }
                frameworkElement.Margin = margin;


                int childcount = VisualTreeHelper.GetChildrenCount(parent);
                for (int c = 0;c < childcount;c++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent,c);
                    if (typeof(UIElement).IsAssignableFrom(child.GetType()))
                    {
                        ResizeObjectsCascade((UIElement)child, width, height);
                    }
                }
            }
        }
        
        /// <summary>
        /// Replacing selection arrows with window size changing
        /// </summary>
        /// <param name="button"></param>
        private void  Arrow_placing(Thickness button)
        {   
            Thickness current_button = button;
            current_button.Left -= img_left_arrow.Width;
            Thickness left_arrow = current_button;
            double Left_arrow_place = bt_options.Width + img_right_arrow.Width;
            current_button.Left += Left_arrow_place;
            img_left_arrow.Margin = left_arrow;
            Thickness right_arrow = current_button;
            img_right_arrow.Margin = right_arrow;
            img_left_arrow.Visibility = Visibility.Visible;
            img_right_arrow.Visibility = Visibility.Visible;
            
            

        }

        /// <summary>
        /// Main Menu button events
        /// </summary>
        private void Arrow_clear()
        {
            img_left_arrow.Visibility = Visibility.Collapsed;
            img_right_arrow.Visibility = Visibility.Collapsed;
      
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            GameManager.Stop();
        }

        private void bt_exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void bt_help_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Help.Visibility = Visibility.Visible;

        }

        private void bt_back_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Visible;
            Help.Visibility = Visibility.Collapsed;
            Options.Visibility = Visibility.Collapsed;
            Leaderboard.Visibility = Visibility.Collapsed;
            chars.UnLoadChar(1);
            chars.UnLoadChar(2);
        }

        private void bt_options_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Options.Visibility = Visibility.Visible;
            chars.LoadChar();
            
        }
        private void bt_singleplay_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            PlayingField.Visibility = Visibility.Visible;
            load.LoadMap(1);
        }
        private void bt_multiplay_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            PlayingField.Visibility = Visibility.Visible;
            load.LoadMap(2);
        }
        private void bt_leaderboard_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Leaderboard.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Selection arrow moving events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_options_MouseEnter(object sender, MouseEventArgs e)
        {
            Arrow_placing(bt_options.Margin);
        }

        private void bt_options_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();   
        }
        private void bt_help_MouseEnter(object sender, MouseEventArgs e)
        {
            Arrow_placing(bt_help.Margin);
        }
        private void bt_help_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();
        }
        private void bt_exit_MouseEnter(object sender, MouseEventArgs e)
        {
            Arrow_placing(bt_exit.Margin);
        }
        private void bt_exit_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();
        }

        private void bt_multiplay_MouseEnter(object sender, MouseEventArgs e)
        {
            Arrow_placing(bt_multiplay.Margin);
        }

        private void bt_multiplay_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();
        }

        private void bt_singleplay_MouseEnter(object sender, MouseEventArgs e)
        {
            Arrow_placing(bt_singleplay.Margin);
        }

        private void bt_singleplay_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();
        }

        /// <summary>
        /// Options button events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void sl_music_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double k = (double)sl_music.Value;
            mp.Volume = k/100;
        }
        private void bt_mainmenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Visible;
            EndGame.Visibility = Visibility.Collapsed;
        }
        private void bt_charsselect1_r_Click(object sender, RoutedEventArgs e)
        {
            chars.NextChar(1);
        }

        private void bt_charsselect2_r_Click(object sender, RoutedEventArgs e)
        {
            chars.NextChar(2);
        }

        private void bt_charsselect1_l_Click(object sender, RoutedEventArgs e)
        {
            chars.PreChar(1);
        }

        private void bt_charsselect2_l_Click(object sender, RoutedEventArgs e)
        {
            chars.PreChar(2);
        }
        /// <summary>
        /// Mouse events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CollisionEditor.StartDrawing(sender,e);
        }
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CollisionEditor.StopDrawing(sender, e);
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            CollisionEditor.Draw(sender, e);
        }


    }
}
