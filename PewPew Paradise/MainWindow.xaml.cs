﻿using System;
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
            Thickness left_arrow = new Thickness();
            Thickness right_arrow = new Thickness();
            Thickness current_button = new Thickness();
            GameWindow.Margin = thickness;
            previousHeight = GameWindow.Height;
            previousWidth = GameWindow.Width;
            gameManager = new GameManager(60);
            gameManager.Begin();
        }

        public void Test()
        {
            Timer -= 0.03f;
            Sprite mrph = GameManager.Instance.MrPlaceHolder;
            mrph.Position = new Maths.Vector2(8 + Math.Sin(Timer) * 8.0f, 8 + Math.Sin(Timer) * 8.0f);
            mrph.Size = new Maths.Vector2(Math.Sin(Timer * 0.3f) * 8.0f,Math.Cos(Timer * 0.3f) * 8.0f);

           
        }


        private void KeyPress(object sender, KeyEventArgs e)
        {
            SpriteManager.Instance.CreateSprite("MrPlaceHolder",new Maths.Vector2(8,8), new Maths.Vector2(3,3));
        }

        private void KeyLift(object sender, KeyEventArgs e)
        {

        }


        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {




           /* LinearGradientBrush gb = new LinearGradientBrush();

            GradientStop red = new GradientStop();
            red.Color = Colors.Red;
            gb.GradientStops.Add(red);

            GradientStop yellow = new GradientStop();
            yellow.Offset = 0.25;
            yellow.Color = Colors.Yellow;
            gb.GradientStops.Add(yellow);

            GradientStop green = new GradientStop();
            green.Offset = 0.5;
            green.Color = Colors.Green;
            gb.GradientStops.Add(green);

            GradientStop blue = new GradientStop();
            blue.Offset = 0.75;
            blue.Color = Colors.Blue;
            gb.GradientStops.Add(blue);

            GradientStop purple = new GradientStop();
            purple.Offset = 1.0;
            purple.Color = Colors.Purple;
            gb.GradientStops.Add(purple);

            this.Resources["BackgroundBrush"] = gb;*/

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

        private void ResizeObjectsCascade(UIElement child, double width, double height)
        {
            if (typeof(FrameworkElement).IsAssignableFrom(child.GetType()))
            {
                FrameworkElement frameworkElement = (FrameworkElement)child;
                frameworkElement.Width *= width;
                frameworkElement.Height *= height;
                Thickness margin = frameworkElement.Margin;
                margin.Left *= width;
                margin.Right *= width;
                margin.Top *= height;
                margin.Bottom *= height;

                if (typeof(Control).IsAssignableFrom(child.GetType()))
                {
                    Control control = (Control)child;
                    control.FontSize *= width;
                }
                frameworkElement.Margin = margin;
            }
            if (typeof(Panel).IsAssignableFrom(child.GetType()))
            {
                Panel panel = (Panel)child;
                foreach (UIElement uIElement in panel.Children)
                {
                    ResizeObjectsCascade(uIElement, width, height);
                }
            }
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
            Thickness current_button = bt_options.Margin;
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

        private void bt_options_MouseLeave(object sender, MouseEventArgs e)
        {
            img_left_arrow.Visibility = Visibility.Collapsed;
            img_right_arrow.Visibility = Visibility.Collapsed;
        }
        private void bt_help_MouseEnter(object sender, MouseEventArgs e)
        {
            Thickness current_button = bt_help.Margin;
            current_button.Left -= img_left_arrow.Width;
            Thickness left_arrow = current_button;
            Double Left_arrow_place = bt_help.Width + img_right_arrow.Width;
            current_button.Left += Left_arrow_place;
            img_left_arrow.Margin = left_arrow;
            Thickness right_arrow = current_button;
            img_right_arrow.Margin = right_arrow;
            img_left_arrow.Visibility = Visibility.Visible;
            img_right_arrow.Visibility = Visibility.Visible;

        }
        private void bt_help_MouseLeave(object sender, MouseEventArgs e)
        {
            img_left_arrow.Visibility = Visibility.Collapsed;
            img_right_arrow.Visibility = Visibility.Collapsed;
        }
        private void bt_exit_MouseEnter(object sender, MouseEventArgs e)
        {
            Thickness current_button = bt_exit.Margin;
            current_button.Left -= img_left_arrow.Width;
            Thickness left_arrow = current_button;
            Double Left_arrow_place = bt_exit.Width + img_right_arrow.Width;
            current_button.Left += Left_arrow_place;
            img_left_arrow.Margin = left_arrow;
            Thickness right_arrow = current_button;
            img_right_arrow.Margin = right_arrow;
            img_left_arrow.Visibility = Visibility.Visible;
            img_right_arrow.Visibility = Visibility.Visible;

        }
        private void bt_exit_MouseLeave(object sender, MouseEventArgs e)
        {
            img_left_arrow.Visibility = Visibility.Collapsed;
            img_right_arrow.Visibility = Visibility.Collapsed;
        }
    }
}
