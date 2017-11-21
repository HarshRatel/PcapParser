﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			_parcedList = new List<List<string>>();
			_myCollection = new ObservableCollection<Item>();
			dgTable.ItemsSource = _myCollection;
		}

		private static List<List<string>> _parcedList;
		private ObservableCollection<Item> _myCollection;

		/// <summary>
		/// "btnOpenFile" button click handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnOpenFile_OnClick(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog {Filter = "PCAP-files (*.pcap)|*.pcap"};

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
			if (txtEditor.Text == "" || txtEditor.Text == "Device path")
			{
				MessageBox.Show("Device path error", "Error");
				return;
			}

			var parser = new PacketManipulation();
			parser.Parse(txtEditor.Text);
			_parcedList.Clear();
			_parcedList = parser.pcapTable;

			DrawTable(_parcedList);
		}

		/// <summary>
		/// Displays table from list
		/// </summary>
		/// <param name="table">List of table</param>
		private void DrawTable(List<List<string>> table)
		{
			if (table.Count == 0)
			{
				return;
			}

			_myCollection.Clear();

			/*var protocolColumn = new DataGridTextColumn
			{
				Header = "Protocol type",
				Binding = new Binding("protocolColumn"),
				Width = 140
			};
			dgTable.Columns.Add(protocolColumn);

			var timeColumn = new DataGridTextColumn
			{
				Header = "Time",
				Binding = new Binding("timeColumn"),
				Width = 125
			};
			dgTable.Columns.Add(timeColumn);

			var srcColumn = new DataGridTextColumn
			{
				Header = "Source IP",
				Binding = new Binding("srcColumn"),
				Width = 125
			};
			dgTable.Columns.Add(srcColumn);

			var dstColumn = new DataGridTextColumn
			{
				Header = "Destination IP",
				Binding = new Binding("dstColumn"),
				Width = 125
			};
			dgTable.Columns.Add(dstColumn);

			var lengthColumn = new DataGridTextColumn
			{
				Header = "Length",
				Binding = new Binding("lengthColumn"),
				Width = 60
			};
			dgTable.Columns.Add(lengthColumn);

			var infoColumn = new DataGridTextColumn
			{
				Header = "Info",
				Binding = new Binding("infoColumn"),
				Width = 644
			};
			dgTable.Columns.Add(infoColumn);*/

			foreach (var raw in table)
			{
				_myCollection.Add(new Item()
				{
					protocolColumn = raw[0],
					timeColumn = raw[1],
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

		/// <summary>
		/// "dgTable" editing stub
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DgTable_OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
		{
			e.Cancel = true;
		}

		/// <summary>
		/// Filter handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TbFilter_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			if (tbFilter.Text.Length == 0)
			{
				DrawTable(_parcedList);
				return;
			}
			var filteredList = _parcedList.Where(raw => raw[0].Contains(tbFilter.Text)).ToList();
			DrawTable(filteredList);
		}
	}
}