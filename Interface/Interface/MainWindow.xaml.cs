using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FEButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnOpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog {Filter = "PCAP-files (*.pcap)|*.pcap"};

            if (openFileDialog.ShowDialog() == true)
                txtEditor.Text = openFileDialog.FileName;
        }

        private void BtnParce_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Developing", "Coming soon");
        }
    }
}
