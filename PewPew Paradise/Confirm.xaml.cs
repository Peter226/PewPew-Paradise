using PewPew_Paradise.GameLogic;
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
using System.Windows.Shapes;

namespace PewPew_Paradise
{
    /// <summary>
    /// Interaction logic for Confirm.xaml
    /// </summary>
    public partial class Confirm : Window
    {
        private static Confirm inst;
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
            GameManager.Begin();
            Close();
        }

        private void bt_no_Click(object sender, RoutedEventArgs e)
        {
            GameManager.Begin();
            Close();
        }
    }
}
