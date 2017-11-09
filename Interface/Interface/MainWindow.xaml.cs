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
            public string typeColumn { get; set; }
            public string srcAdrColumn { get; set; }
            public string dstAdrColumn { get; set; }
            public string srcPortColumn { get; set; }
            public string dstPortColumn { get; set; }
		}

        private void BtnParse_OnClick(object sender, RoutedEventArgs e)
        {
	        if (txtEditor.Text == "")
	        {
		        MessageBox.Show("Device path error", "Error");
				return;
	        }

	        dgTable.ColumnWidth = 243;
	        dgTable.CanUserResizeColumns = false;
	        dgTable.AutoGenerateColumns = false;

			PcapParser.PacketManipulation parser = new PacketManipulation();
	        parser.Parse(txtEditor.Text);
	        var table = parser.pcapTable;

			DataGridTextColumn typeColumn = new DataGridTextColumn();
	        typeColumn.Header = "Protocol type";
            typeColumn.Binding = new Binding("typeColumn");
			dgTable.Columns.Add(typeColumn);

			DataGridTextColumn srcAdrColumn = new DataGridTextColumn();
			srcAdrColumn.Header = "Source address";
            srcAdrColumn.Binding = new Binding("srcAdrColumn");
			dgTable.Columns.Add(srcAdrColumn);

			DataGridTextColumn dstAdrColumn = new DataGridTextColumn();
			dstAdrColumn.Header = "Destination address";
            dstAdrColumn.Binding = new Binding("dstAdrColumn");
			dgTable.Columns.Add(dstAdrColumn);

            DataGridTextColumn srcPortColumn = new DataGridTextColumn();
            srcPortColumn.Header = "Source Port";
            srcPortColumn.Binding = new Binding("srcPortColumn");
            dgTable.Columns.Add(srcPortColumn);

            DataGridTextColumn dstPortColumn = new DataGridTextColumn();
            dstPortColumn.Header = "Destination Port";
            dstPortColumn.Binding = new Binding("dstPortColumn");
            dgTable.Columns.Add(dstPortColumn);

			foreach (var raw in table)
	        {
                dgTable.Items.Add(new Item() { typeColumn = raw[0], srcAdrColumn = raw[1], dstAdrColumn = raw[2], srcPortColumn = raw[3], dstPortColumn = raw[4] });
	        }
        }
    }
}
