using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using HtmlAgilityPack;

namespace CheckUpdate
{
	class MainClass
	{
		private static bool FoundUpdate;
		private static double CurrentVersion;
		private static double LastVersion;

		static void Main(string[] args)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			HtmlWeb web = new HtmlWeb();
			HtmlDocument document = web.Load("https://github.com/KylloxStudio/Durango_V2/tags");
			HtmlNode h4Node = document.DocumentNode.SelectSingleNode("//h4[@class='flex-auto min-width-0 pr-2 pb-1 commit-title']");
			HtmlNode aNode = h4Node.SelectSingleNode("a");

			string[] version = aNode.InnerText.Split(new char[]
			{
				'v'
			});
			LastVersion = double.Parse(version[1]);

			string path;
			if (Directory.GetCurrentDirectory().IndexOf("Durango_Ver_PC_Final") > -1)
				path = Directory.GetCurrentDirectory() + "\\Durango_Korea_Data\\version.txt";
			else
				path = Directory.GetCurrentDirectory() + "\\Durango_Data\\version.txt";

			if (new FileInfo(path).Exists)
			{
				string txt = File.ReadAllText(path);
				CurrentVersion = double.Parse(txt);
			}
			else
			{
				CurrentVersion = 2147483647;
				Console.WriteLine("Error: Cannot Found Required File.");
				Console.ReadLine();
				foreach (Process process in Process.GetProcesses())
				{
					if (process.ProcessName.StartsWith("CheckUpdate_DurangoV2"))
					{
						process.Kill();
					}
				}
			}

			if (CurrentVersion < LastVersion)
				FoundUpdate = true;

			Console.WriteLine("Found Update: " + FoundUpdate.ToString());
			Console.WriteLine("Current Version: " + string.Format("{0:0.0}", CurrentVersion));
			Console.WriteLine("Last Version: " + string.Format("{0:0.0}", LastVersion));

			if (FoundUpdate)
			{
				while (true)
                {
					Console.ReadLine();
				}
			}
			else
			{
				foreach (Process process in Process.GetProcesses())
				{
					if (process.ProcessName.StartsWith("CheckUpdate_DurangoV2"))
					{
						process.Kill();
					}
				}
			}
		}
	}
}