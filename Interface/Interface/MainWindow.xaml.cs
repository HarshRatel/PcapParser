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
using PcapParser;

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

        private void BtnOpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog {Filter = "PCAP-files (*.pcap)|*.pcap"};

            if (openFileDialog.ShowDialog() == true)
                txtEditor.Text = openFileDialog.FileName;
        }

	    public class Item
	    {
		    public string ColOne { get; set; }
			public string ColTwo { get; set; }
			public string ColThree { get; set; }
		}

        private void BtnParse_OnClick(object sender, RoutedEventArgs e)
        {
	        if (txtEditor.Text == "")
	        {
		        MessageBox.Show("Device path error", "Error");
				return;
	        }

	        dgTable.ColumnWidth = 406;
	        dgTable.CanUserResizeColumns = false;
	        dgTable.AutoGenerateColumns = false;

			PcapParser.PacketManipulation parser = new PacketManipulation();
	        parser.Parse(txtEditor.Text);
	        var table = parser.pcapTable;

			DataGridTextColumn columnOne = new DataGridTextColumn();
	        columnOne.Header = "Protocol type";
	        columnOne.Binding = new Binding("ColOne");
			dgTable.Columns.Add(columnOne);

			DataGridTextColumn columnTwo = new DataGridTextColumn();
			columnTwo.Header = "Source address";
			columnTwo.Binding = new Binding("ColTwo");
			dgTable.Columns.Add(columnTwo);

			DataGridTextColumn columnThree = new DataGridTextColumn();
			columnThree.Header = "Destination address";
			columnThree.Binding = new Binding("ColThree");
			dgTable.Columns.Add(columnThree);

			foreach (var raw in table)
	        {
		        dgTable.Items.Add(new Item() {ColOne = raw[0], ColTwo = raw[1], ColThree = raw[2]});
	        }
        }
    }
}
