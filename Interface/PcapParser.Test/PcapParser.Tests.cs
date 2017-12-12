using System;
using System.IO;
using System.Linq;

namespace PcapParser.Test
{
	using NUnit.Framework;
	using PcapParser;

	[TestFixture]
	public class PcapParserTests
	{
		[Test]
		public void CorrectStartTest()
		{
			var parser = new PacketManipulation();
			parser.Parse(Path.Combine(Directory.GetCurrentDirectory(), @"../../../http.pcap"));
			Assert.That(parser.pcapTable.Any(), Is.True);

			parser.Parse(Path.Combine(Directory.GetCurrentDirectory(), @"../../../icmp.pcap"));
			Assert.That(parser.pcapTable.Any(), Is.True);
		}

		[Test]
		public void WrongStartTest()
		{
			var parser = new PacketManipulation();

			try
			{
				parser.Parse(Path.Combine(Directory.GetCurrentDirectory(), @"../../../wrongpath.pcap"));
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}