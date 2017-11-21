using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using PcapParser;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

		/// <summary>
		/// "btnOpenFile" button click handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void BtnOpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog {Filter = "PCAP-files (*.pcap)|*.pcap"};

            if (openFileDialog.ShowDialog() == true)
                txtEditor.Text = openFileDialog.FileName;
        }

		/// <summary>
		/// Columns class
		/// </summary>
	    private class Item
		{
			public string protocolColumn { get; set; }
			public string timeColumn { get; set; }
			public string srcColumn { get; set; }
			public string dstColumn { get; set; }
            public string lengthColumn { get; set; }
			public string infoColumn { get; set; }
		}

		/// <summary>
		/// "btnParse" button handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void BtnParse_OnClick(object sender, RoutedEventArgs e)
        {
	        if (txtEditor.Text == "")
	        {
		        MessageBox.Show("Device path error", "Error");
				return;
	        }

			dgTable.Columns.Clear();

	        dgTable.ColumnWidth = 203;
	        dgTable.CanUserResizeColumns = false;
	        dgTable.AutoGenerateColumns = false;

			var parser = new PacketManipulation();
	        parser.Parse(txtEditor.Text);
	        var table = parser.pcapTable;
			
			var protocolColumn = new DataGridTextColumn
			{
				Header = "Protocol type",
				Binding = new Binding("protocolColumn")
			};
			dgTable.Columns.Add(protocolColumn);

			var timeColumn = new DataGridTextColumn
			{
				Header = "Time",
				Binding = new Binding("timeColumn")
			};
			dgTable.Columns.Add(timeColumn);

			var srcColumn = new DataGridTextColumn
			{
				Header = "Source IP",
				Binding = new Binding("srcColumn")
			};
			dgTable.Columns.Add(srcColumn);

			var dstColumn = new DataGridTextColumn
			{
				Header = "Destination IP",
				Binding = new Binding("dstColumn")
			};
			dgTable.Columns.Add(dstColumn);

			var lengthColumn = new DataGridTextColumn
			{
				Header = "Length",
				Binding = new Binding("lengthColumn")
			};
			dgTable.Columns.Add(lengthColumn);

	        var infoColumn = new DataGridTextColumn
	        {
				Header = "Info",
				Binding = new Binding("infoColumn")
	        };
			dgTable.Columns.Add(infoColumn);

			foreach (var raw in table)
	        {
                dgTable.Items.Add(new Item() { protocolColumn = raw[0],
												timeColumn  = raw[1],
												srcColumn = raw[2],
												dstColumn = raw[3],
												lengthColumn = raw[4],
												infoColumn = raw[5]
												});
	        }
        }

		/// <summary>
		/// "btnLog" button handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
	    private void BtnLog_OnClick(object sender, RoutedEventArgs e)
	    {
			System.Diagnostics.Process.Start(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"../../../logs/logs.txt"));
		}
    }
}
