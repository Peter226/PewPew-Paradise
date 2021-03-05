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

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += KeyPress;
            Thickness thickness = new Thickness();
            GameWindow.Margin = thickness;

            DrawingBrush dw = new DrawingBrush();
        }



        private void KeyPress(object sender, KeyEventArgs e)
        {

        }

        private void KeyLift(object sender, KeyEventArgs e)
        {

        }


        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {




            LinearGradientBrush gb = new LinearGradientBrush();

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

            this.Resources["BackgroundBrush"] = gb;

            if (this.SizeToContent != SizeToContent.Manual)
            {
                windowDifferenceX = this.ActualWidth - this.Width;
                windowDifferenceY = this.ActualHeight - this.Height;
            }

            double minSize = Math.Min(RealWidth, RealHeight);
            UwU.FontSize = minSize * 0.2f;

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


    }
}
