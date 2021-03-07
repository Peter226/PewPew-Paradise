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
            previousHeight = GameWindow.Height;
            previousWidth = GameWindow.Width;
            gameManager = new GameManager(60);
            gameManager.Begin();
            gameManager.OnUpdate += Update;
        }



        public void Update()
        {
            Timer -= 0.03f;
            foreach (Sprite sprite in MrPlaceHolders)
            {
                Vector2 newpos = sprite.Position;
                newpos.x += Math.Sin(Timer) * 0.1f;
                newpos.y += Math.Sin(Timer * 0.2f) * 0.1f;
                sprite.Position = newpos;
            }
        }

        private void KeyPress(object sender, KeyEventArgs e)
        {
            Sprite mrph = SpriteManager.instance.CreateSprite("MrPlaceHolder",new Maths.Vector2(8,8), new Maths.Vector2(1,1));
            MrPlaceHolders.Add(mrph);


        }

        private void KeyLift(object sender, KeyEventArgs e)
        {

        }


        /// <summary>
        /// Called by the Window's event when it's size changes. Used for resizing the canvas
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
        /// Called when the canvas changes it's size, resizes all child objects
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
        /// Function that resizes the parent and calls itself with the children
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="width">width difference ratio</param>
        /// <param name="height">heigth difference ratio</param>
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
        private void Arrow_placing(Thickness button)
        {   
            Thickness current_button = button;
            current_button.Left -= img_left_arrow.Width;
            Thickness left_arrow = current_button;
            Double Left_arrow_place = bt_options.Width + img_right_arrow.Width;
            current_button.Left += Left_arrow_place;
            img_left_arrow.Margin = left_arrow;
            Thickness right_arrow = current_button;
            img_right_arrow.Margin = right_arrow;
            img_left_arrow.Visibility = Visibility.Visible;
            img_right_arrow.Visibility = Visibility.Visible;
        }
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


    }
}
