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
        /// <summary>
        /// Checks if this window in foreground
        /// Return true if the window is foreground
        /// </summary>
        /// <returns></returns>
        public bool IsForeground()
        {
            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            IntPtr foregroundWindow = GetForegroundWindow();
            return windowHandle == foregroundWindow;
        }
        /// <summary>
        /// Giving instance to this window
        /// </summary>
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
        /// <summary>
        /// Clearing database if the user clicks on yes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_yes_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.scoreManager.ClearDB();
            Close();
        }
        /// <summary>
        /// Close this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_no_Click(object sender, RoutedEventArgs e)
        {
            
            Close();
        }
        /// <summary>
        /// If Windowstate change or the user clicks on the window anywhere expect yes button it closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
