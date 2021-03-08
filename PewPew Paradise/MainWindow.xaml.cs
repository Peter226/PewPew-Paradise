using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using PewPew_Paradise.GameLogic;
using PewPew_Paradise.Maths;
using System.ComponentModel;

namespace PewPew_Paradise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow instance;
        public GameManager gameManager { get; }

        double windowDifferenceX;
        double windowDifferenceY;

        double previousWidth;
        double previousHeight;

        float Timer = 0;

        List<Sprite> MrPlaceHolders = new List<Sprite>();



        MediaPlayer mp = new MediaPlayer();




        public static MainWindow Instance
        {
            get
            {
                return instance;
            }
        }


        public MainWindow()
        {
            instance = this;
            InitializeComponent();
            this.KeyDown += KeyPress;
            Thickness thickness = new Thickness();
            GameWindow.Margin = thickness;
            previousHeight = GameWindow.Height;
            previousWidth = GameWindow.Width;
            gameManager = new GameManager(60);
            gameManager.Begin();

            GameManager.Instance.OnUpdate += Update;
            
            string l = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string p = System.IO.Path.GetDirectoryName(l);
            p = System.IO.Path.Combine(p, "PewPew_Paradise_Assets/Music/MainMenu.mp3");
            Console.WriteLine(p);
            mp.Open(new Uri(p));
            mp.MediaOpened += new EventHandler(PlayMedia);

            SpriteManager.Instance.LoadImage("Images/Sprites/forest_map.png", "forest_map");
            SpriteManager.Instance.LoadImage("Images/Sprites/sky2.png", "sky_map");

            SpriteAnimation animation = new SpriteAnimation(150,true);
            AnimationCollection playerAnimations = new AnimationCollection("Player",Vector2.One,Vector2.One * 4);
            playerAnimations.animations.Add(animation);
            animation.keyFrames.Add(new Vector2(0, 0));
            animation.keyFrames.Add(new Vector2(1, 0));
            animation.keyFrames.Add(new Vector2(2, 0));
            animation.keyFrames.Add(new Vector2(3, 0));
            SpriteManager.Instance.AddAnimationCollection(playerAnimations,"Player");


        }

        public void PlayMedia(object sender, EventArgs a)
        {
            Console.WriteLine("Opened Media");
            mp.Play();
            double k = (double)sl_music.Value;
            mp.Volume = k / 100;

        }



        public void Update()
        {
            Timer -= (float)(GameManager.DeltaTime * 0.00075);
            foreach (Sprite sprite in MrPlaceHolders)
            {
                Vector2 newpos = sprite.Position;
                newpos.x += Math.Sin(Timer * 2.2 + Math.Cos(newpos.y) * 0.1f) * 0.1f;
                newpos.y += Math.Sin(Timer * 3.3 + Math.Cos(newpos.x) * 0.1f) * 0.1f;
                sprite.Position = newpos;

               /* if (Math.Sin(Timer * 10.0f + newpos.x) < -0.8)
                {
                    sprite.IsActive = false;
                }
                else
                {
                    sprite.IsActive = true;
                }*/
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


        private void KeyPress(object sender, KeyEventArgs e)
        {
            SpriteAnimated mrph = new SpriteAnimated("MrPlaceHolder","Player",new Maths.Vector2(8,8), new Maths.Vector2(4,4));
            MrPlaceHolders.Add(mrph);

            
        }

        private void KeyLift(object sender, KeyEventArgs e)
        {
            var vals = LogicalTreeHelper.GetChildren(sl_music);
            foreach (DependencyObject dep in vals) {

                Console.WriteLine(dep);
            }
        }


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
            gameManager.Stop();
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
        }

        private void bt_options_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Options.Visibility = Visibility.Visible;
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

        private void bt_singleplay_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            SinglePlayer.Visibility = Visibility.Visible;
            Vector2 map_position = new Vector2(8,8);
            Vector2 map_size = new Vector2(16, 16);
            Sprite forest_map = new Sprite("sky_map",  map_position, map_size, true);
        }

        private void sl_music_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double k = (double)sl_music.Value;
            mp.Volume = k/100;
        }
    }
}
