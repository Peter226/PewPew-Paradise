using PewPew_Paradise.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PewPew_Paradise
{
    /// <summary>
    /// Interaction logic for Confirm.xaml
    /// </summary>
    public partial class Confirm : Window
    {
        private static Confirm inst;
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        public bool IsForeground()
        {
            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            IntPtr foregroundWindow = GetForegroundWindow();
            return windowHandle == foregroundWindow;
        }
        public static Confirm Inst
        {
            get
            {
                return inst;
            }
        }
        public Confirm()
        {
            inst = this;
            InitializeComponent();

        }

        private void bt_yes_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.score.ClearDB();
            Close();
        }

        private void bt_no_Click(object sender, RoutedEventArgs e)
        {
            
            Close();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            Close();
        }
    }
}
