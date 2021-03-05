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
namespace PewPew_Paradise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        double windowDifferenceX;
        double windowDifferenceY;

        double previousWidth;
        double previousHeight;

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += KeyPress;
            Thickness thickness = new Thickness();
            GameWindow.Margin = thickness;
            previousHeight = GameWindow.Height;
            previousWidth = GameWindow.Width;
        }



        private void KeyPress(object sender, KeyEventArgs e)
        {

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



    }
}
