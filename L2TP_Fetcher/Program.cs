using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace L2TP_Fetcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Fetching VPN's...");
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load("https://freevpn4you.net/l2tp-ipsec.php");

            FreeVPN4YouCell rawCells = htmlDocument.DocumentNode.GetEncapsulatedData<FreeVPN4YouCell>();

            List<FreeVPN4You_VPN> vpns = new List<FreeVPN4You_VPN>();

            for (int i = 0; i < rawCells.Cells.Count; i = i + 4)
            {
                FreeVPN4You_VPN vpn = new FreeVPN4You_VPN();

                vpn.Country = rawCells.Cells[i].Replace("\n", string.Empty).Trim();
                vpn.IP_Address = rawCells.Cells[i + 1].Replace("\n", string.Empty).Trim();
                vpn.Uptime = rawCells.Cells[i + 2].Replace("\n", string.Empty).Trim(); ;

                if (vpn.Uptime.Contains("mins"))
                {
                    vpn.Uptime_Minute = int.Parse(vpn.Uptime.Replace("mins", string.Empty).Trim());
                }

                if (vpn.Uptime.Contains("hours"))
                {
                    vpn.Uptime_Minute = int.Parse(vpn.Uptime.Replace("hours", string.Empty).Trim()) * 60;
                }

                if (vpn.Uptime.Contains("days"))
                {
                    vpn.Uptime_Minute = int.Parse(vpn.Uptime.Replace("days", string.Empty).Trim()) * 60 * 24;
                }

                vpn.Ping = int.Parse(rawCells.Cells[i + 3].Replace("ms", string.Empty).Trim());

                vpns.Add(vpn);

            }

            string vpns_json = JsonSerializer.Serialize(vpns);

            Console.WriteLine("Saving...");

            System.IO.File.WriteAllText(@"..\..\..\..\L2TP_List.json", vpns_json);

            Console.ReadKey();

        }

    }



    [HasXPath]
    public class FreeVPN4YouCell
    {
        [XPath("//div[@class='lst']")]
        public List<string> Cells { get; set; }
    }

    public class FreeVPN4You_VPN
    {
        public string Country { get; set; }
        public string IP_Address { get; set; }
        public string Uptime { get; set; }
        public int Uptime_Minute { get; set; }
        public int Ping { get; set; }

    }
}
