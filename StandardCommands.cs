using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using Renci.SshNet;
using System.IO;

namespace DiscordBot
{
    public class StandardCommands
    {
        [Command("hi"), Description("This is a test command for debugging to ensure bot is working!")]
        public async Task Hi(CommandContext ctx)
        {
            Console.WriteLine("Command: hi");
            await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}!");
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Command("getsql"), Description("Testing SQL")]
        public async Task GetSQL(CommandContext ctx, string table = "DiscordBot")
        {
            // When i want to work on the formatting for the table output,
            //https://stackoverflow.com/questions/26223980/display-results-in-a-table-format-in-c-sharp
            
            string Returnstring = "``` \n";
            string connectionString;
            MySqlConnection cnn;
            connectionString = @$"Server=x.x.x.x;Port=xxxx;Database={table};UID=Username;Pwd=Redacted";
            cnn = new MySqlConnection(connectionString);
            try
            {
                cnn.Open();
                Console.WriteLine("Connection Open !");
                string sql = "Select ID,UserName,DiscordID,Notification_Preference,IsAdmin FROM Discord_Users";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                int columnCount = rdr.FieldCount;

                List<List<string>> output = new List<List<string>>();
                bool showHeader = true;

                #region Database Output Formatting
                if (showHeader)
                {
                    List<string> values = new List<string>();
                    for (int count = 0; count < columnCount; ++count)
                    {
                        values.Add(string.Format("{0}", rdr.GetName(count)));
                    }
                    output.Add(values);
                }

                while (rdr.Read())
                {
                    List<string> values = new List<string>();
                    for (int count = 0; count < columnCount; ++count)
                    {
                        values.Add(string.Format("{0}", rdr[count]));
                    }
                    output.Add(values);
                }
                rdr.Close();

                List<int> widths = new List<int>();
                for (int count = 0; count < columnCount; ++count)
                {
                    widths.Add(0);
                }

                foreach (List<string> row in output)
                {
                    for (int count = 0; count < columnCount; ++count)
                    {
                        widths[count] = Math.Max(widths[count], row[count].Length);
                        string temp = widths[count].ToString();
                        Console.WriteLine(temp+","+row[count].Length);
                    }
                }

                //int totalWidth = widths.Sum() + (widths.Count * 1) * 3;
                //Console.SetWindowSize(Math.Max(Console.WindowWidth, totalWidth), Console.WindowHeight);
                foreach (List<string> row in output)
                {
                    StringBuilder outputLine = new StringBuilder();

                    for (int count = 0; count < columnCount; ++count)
                    {
                        if (count > 0)
                        {
                                outputLine.Append(" ");
                        }
                        else
                        {
                            outputLine.Append("| ");
                        }
                        string value = row[count];
                        outputLine.Append(value.PadLeft(widths[count]));
                        outputLine.Append(" |");
                    }
                    Returnstring += outputLine.ToString();
                    Returnstring += "\n";
                    //Console.WriteLine("{0}", outputLine.ToString());
                }
                #endregion

            }
            catch (Exception ex)
            {
                Returnstring = ex.ToString();
            }

            Returnstring += "```";
            await ctx.RespondAsync(Returnstring);
            cnn.Close();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Command("addsql"), Description("Adds a user to the SQL Table"), RequireOwner]
        public async Task AddSql(CommandContext ctx, [Description("DiscordName#xxxx, DiscordID, NotificationDefault, IsAdmin")] String DiscordName, String DiscordID, bool NotificationDefault = false,bool IsAdmin = false,string table = "DiscordBot")
        {
            string Returnstring = "";
            string connectionString;MySqlConnection cnn;
            connectionString = @$"Server=x.x.x.x;Port=xxxx;Database={table};UID=username;Pwd=redacted";
            cnn = new MySqlConnection(connectionString);
            try
            {
                cnn.Open();
                Console.WriteLine("Connection Open !");
                string sql = "Select ID,UserName,DiscordID,Notification_Preference,IsAdmin FROM Discord_Users";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Returnstring += ("\n" + rdr[0] + " \t " + rdr[1] + " \t " + rdr[2] + " \t " + rdr[3] + " \t " + rdr[4]);
                }
                rdr.Close();
                Returnstring = "Successfully added uservariables to database!";
            }
            catch (Exception ex)
            {
                Returnstring = "Failed to write uservariables to string! Check Console for exception.";
                Console.WriteLine("AddSQL Failed. Exception: \t" + ex);
            }

            await ctx.RespondAsync();
            cnn.Close();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Command("balance"), Description("Testing SQL")]
        public async Task Balance(CommandContext ctx, [Description("InGame Name or nothing, for baltop")] string Username = "all")
        {
            
            string Returnstring = "``` \n";
            string connectionString;
            MySqlConnection cnn;
            connectionString = @$"Server=x.x.x.x;Port=xxxx;Database=survsqlbridge;UID=username;Pwd=redacted";
            cnn = new MySqlConnection(connectionString);
            bool boolusername = int.TryParse(Username, out int intusername);
            try
            {
                cnn.Open();
                Console.WriteLine("Connection Open !");
                if (Username == "all")
                {
                    string sql = "SELECT player_name,money FROM mpdb_economy WHERE (money > 1) ORDER BY money DESC";
                    MySqlCommand cmd = new MySqlCommand(sql, cnn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    int columnCount = rdr.FieldCount;

                    List<List<string>> output = new List<List<string>>();
                    bool showHeader = true;

                    #region Database Output Formatting
                    if (showHeader)
                    {
                        List<string> values = new List<string>();
                        for (int count = 0; count < columnCount; ++count)
                        {
                            values.Add(string.Format("{0}", rdr.GetName(count)));
                        }
                        output.Add(values);
                    }

                    while (rdr.Read())
                    {
                        List<string> values = new List<string>();
                        for (int count = 0; count < columnCount; ++count)
                        {
                            values.Add(string.Format("{0}", rdr[count]));
                        }
                        output.Add(values);
                    }
                    rdr.Close();

                    List<int> widths = new List<int>();
                    for (int count = 0; count < columnCount; ++count)
                    {
                        widths.Add(0);
                    }

                    foreach (List<string> row in output)
                    {
                        for (int count = 0; count < columnCount; ++count)
                        {
                            widths[count] = Math.Max(widths[count], row[count].Length);
                            string temp = widths[count].ToString();
                        }
                    }

                    //int totalWidth = widths.Sum() + (widths.Count * 1) * 3;
                    //Console.SetWindowSize(Math.Max(Console.WindowWidth, totalWidth), Console.WindowHeight);
                    int rowcount = 0;
                    foreach (List<string> row in output)
                    {
                        StringBuilder outputLine = new StringBuilder();

                        for (int count = 0; count < columnCount; ++count)
                        {
                            if (count > 0)
                            {
                                outputLine.Append(" ");
                            }
                            else
                            {
                                outputLine.Append("| ");
                            }
                            string value = row[count];
                            outputLine.Append(value.PadLeft(widths[count]));
                            outputLine.Append(" |");
                        }
                        rowcount++;
                        Returnstring += outputLine.ToString();
                        Returnstring += "\n";
                        if (rowcount >= 50)
                        {
                            Returnstring += "<";
                            rowcount = 0;
                        }
                        //Console.WriteLine("{0}", outputLine.ToString());
                    }
                    #endregion
                }
                else
                {
                    //begin else//////////////////////////////////////////////////////////////////////////////////////////////////////
                    string sql = $"SELECT player_name,money FROM mpdb_economy WHERE player_name LIKE '{Username}' ORDER BY money DESC";
                    MySqlCommand cmd = new MySqlCommand(sql, cnn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    int columnCount = rdr.FieldCount;

                    List<List<string>> output = new List<List<string>>();
                    bool showHeader = true;

                    #region Database Output Formatting
                    if (showHeader)
                    {
                        List<string> values = new List<string>();
                        for (int count = 0; count < columnCount; ++count)
                        {
                            values.Add(string.Format("{0}", rdr.GetName(count)));
                        }
                        output.Add(values);
                    }

                    while (rdr.Read())
                    {
                        List<string> values = new List<string>();
                        for (int count = 0; count < columnCount; ++count)
                        {
                            values.Add(string.Format("{0}", rdr[count]));
                        }
                        output.Add(values);
                    }
                    rdr.Close();

                    List<int> widths = new List<int>();
                    for (int count = 0; count < columnCount; ++count)
                    {
                        widths.Add(0);
                    }

                    foreach (List<string> row in output)
                    {
                        for (int count = 0; count < columnCount; ++count)
                        {
                            widths[count] = Math.Max(widths[count], row[count].Length);
                            string temp = widths[count].ToString();
                        }
                    }

                    //int totalWidth = widths.Sum() + (widths.Count * 1) * 3;
                    //Console.SetWindowSize(Math.Max(Console.WindowWidth, totalWidth), Console.WindowHeight);
                    int rowcount = 0;
                    foreach (List<string> row in output)
                    {
                        StringBuilder outputLine = new StringBuilder();
                        
                        for (int count = 0; count < columnCount; ++count)
                        {
                            if (count > 0)
                            {
                                outputLine.Append(" ");
                            }
                            else
                            {
                                outputLine.Append("| ");
                            }
                            string value = row[count];
                            outputLine.Append(value.PadLeft(widths[count]));
                            outputLine.Append(" |");
                        }
                        rowcount++;
                        Returnstring += outputLine.ToString();
                        Returnstring += "\n";
                        //Console.WriteLine("{0}", outputLine.ToString());
                    }
                    #endregion
                }
                
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                Returnstring = ex.ToString();
            }
            /////////////////////////////////////////////////////write output
            Returnstring += "```";
            Console.WriteLine("finished statement");
            Console.WriteLine(Returnstring);
            char delimiter = '<';
            int occurences = Returnstring.Count(f => (f == delimiter));string responsevar = "";
            for (int i = 0; i <= occurences; i++)
            {
                if (Returnstring.Contains(delimiter))
                {
                    responsevar = Returnstring.Substring(0, Returnstring.IndexOf(delimiter))+"```";
                    Returnstring = Returnstring.Substring(Returnstring.IndexOf(delimiter) + 1);
                    Returnstring = "``` \n" + Returnstring;
                    await ctx.Channel.SendMessageAsync(responsevar);
                }
                else
                {
                    await ctx.Channel.SendMessageAsync(Returnstring);
                }

            }
            
            await ctx.RespondAsync(null);
            cnn.Close();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Command("status"), Description("Reports statistics for all attached servers!")]
        public async Task PlayerStatus(CommandContext ctx, [Description("Name of game server to display if any")] String ServerName = "")
        {
            bool debugging = false;
            await ctx.Channel.SendMessageAsync("Working on that now...!");

            Console.WriteLine($"Status - '{ServerName}'");
            string IP = null;ushort Port = 0;
            MineStat ms = new MineStat(IP,Port);
            var currentplayers = ms.GetCurrentPlayers(); var maxplayers = ms.GetMaximumPlayers(); var Version = ms.GetVersion();
            //eventually move this to a config file of some kind, or a secondary .cs for looping purposes?
            // > would make the code look a lot cleaner, but is it worth it...?

            //Ports must be CONNECTION PORTS!
            string NetworkIP = "x.x.x.x";       ushort NetworkPort = 0;
            string HubIP = "x.x.x.x";           ushort HubPort = 0;
            string SurvivalIP = "x.x.x.x";      ushort SurvivalPort = 0;
            string FactionsIP = "x.x.x.x";      ushort FactionsPort = 0;
            string SkyBlockIP = "x.x.x.x";      ushort SkyBlockPort = 0;
            string SurvivalHubIP = "x.x.x.x";   ushort SurvivalHubPort = 0;
            string StaffTestIP = "x.x.x.x";     ushort StaffTestPort = 0;
            string CreativeIP = "x.x.x.x";      ushort CreativePort = 0;


            int PortDifference = 100; //The STANDARD difference between all ports. (will brake script if incorrect!) Active: 25565 Query 25665 (difference of 100, Standard+100=query)


            bool ServerOffline = false;

            #region Network
            if (debugging)
            { Console.WriteLine($"Working on Network"); }
            string Network = "";
            IP = NetworkIP; Port = NetworkPort; ms = new MineStat(IP, Port);
            if(ms.ServerUp)
            {
                currentplayers = ms.GetCurrentPlayers(); maxplayers = ms.GetMaximumPlayers(); Version = ms.GetVersion();
                if ((Version.Contains("Waterfall")) || (Version.Contains("Paper")))
                    Version = Version.Substring(Version.IndexOf(" " + 1));

                Network += $"Network Status: Online \n Supporting McVersion(s):{Version} \n Online Players: {currentplayers} out of {maxplayers} \n \t";
            }
            else
            {
                ServerOffline = true;
                Global.ImportantServers += "\t __NETWORK/BUNGEE__";
                Global.OfflineServers += "\t NETWORK/BUNGEE";
                Network = "[Network may be offline!]";
            }
            #endregion

            var status = McQuery.GetStatus(HubIP, (Convert.ToUInt16((Convert.ToInt32(HubPort)) + PortDifference))); status = null;

            if (debugging)
            { Console.WriteLine("[Debugging]: Entering Debugging mode, Debugging flag: True"); }
            
            #region Hub
            if (debugging)
            { Console.WriteLine($"Working on Hub"); }
            string Hub = "";
            IP = HubIP; Port = HubPort; ms = new MineStat(IP, Port);
            if (ms.ServerUp)
            {
                currentplayers = ms.GetCurrentPlayers(); maxplayers = ms.GetMaximumPlayers();Version = ms.GetVersion();
                if ((Version.Contains("Waterfall")) || (Version.Contains("Paper")))
                    Version = Version.Substring(Version.IndexOf(" " + 1));
                Hub += $"Server Status: Online \n Supporting McVersion(s):{Version} \n Online Players: {currentplayers} out of {maxplayers} \n \t";
                Port = Convert.ToUInt16((Convert.ToInt32(Port))+PortDifference);
                status = McQuery.GetStatus(IP, Port);
                foreach (var player in status.Players) Hub += player + "\t";
                if (Convert.ToInt16(currentplayers) > 0) Hub += "\n";
            }
            else
            {
                ServerOffline = true;
                Global.ImportantServers += "\t __Hub__";
                Global.OfflineServers += "\t Hub";
                Hub = "[Server Status: Offline!] \n";
            }
            status = null;
            if (debugging)
            { Console.WriteLine($"Finished Hub"); }
            #endregion
            #region Survival
            if (debugging)
            { Console.WriteLine($"Working on Survival"); }
            string Survival = "";
            IP = SurvivalIP; Port = SurvivalPort; ms = new MineStat(IP, Port);

            if (ms.ServerUp)
            {
                currentplayers = ms.GetCurrentPlayers(); maxplayers = ms.GetMaximumPlayers(); Version = ms.GetVersion();
                if ((Version.Contains("Waterfall")) || (Version.Contains("Paper")))
                    Version = Version.Substring(Version.IndexOf(" " + 1)); 
                Survival += $"Server Status: Online \n Supported McVersion(s): {Version} \n Online Players: {currentplayers} out of {maxplayers} \n \t";
                Port = Convert.ToUInt16((Convert.ToInt32(Port)) + PortDifference);
                status = McQuery.GetStatus(IP, Port);
                foreach (var player in status.Players) Survival += player + "\t";
                if (Convert.ToInt16(currentplayers) > 0) Survival += "\n";
            }
            else
            {
                ServerOffline = true;
                Global.ImportantServers += "\t Survival";
                Global.OfflineServers += "\t Survival";
                Survival = "Server Status: Offline! \n";
            }
            /*    
            try
                {
                    UdpClient.Connect(SurvivalIP, SurvivalPort);
                    status = McQuery.GetStatus(SurvivalIP, SurvivalPort);
                    Survival += "Number of Players: " + status.NumPlayers + "\n";
                    foreach (var player in status.Players) Survival += player + "\t";
                }
                catch (Exception)
                {
                    ServerOffline = true;
                    Global.OfflineServers += "\t Survival";
                    Survival = $"Survival is Offline!";
                }
                */
            status = null;
            if (debugging)
            { Console.WriteLine($"Finished Survival"); }
            #endregion
            #region Factions
            if (debugging)
            { Console.WriteLine($"Working on Factions"); }
            string Factions = "";
            IP = FactionsIP;Port = FactionsPort; ms = new MineStat(IP, Port);
            if (ms.ServerUp)
            {
                currentplayers = ms.GetCurrentPlayers(); maxplayers = ms.GetMaximumPlayers(); Version = ms.GetVersion();
                if ((Version.Contains("Waterfall")) || (Version.Contains("Paper")))
                    Version = Version.Substring(Version.IndexOf(" " + 1));
                Factions += $"Server Status: Online \n Supported McVersion(s): {Version} \n Online Players: {currentplayers} out of {maxplayers} \n \t";
                Port = Convert.ToUInt16((Convert.ToInt32(Port)) + PortDifference);
                status = McQuery.GetStatus(IP, Port);
                foreach (var player in status.Players) Factions += player + "\t";
                if (Convert.ToInt16(currentplayers) > 0) Factions += "\n";
            }
            else
            {
                ServerOffline = true;
                Global.OfflineServers += "\t Factions";
                Factions = "[Server Status: Offline!] \n";
            }
            /*try
            {
            var task = Task.Run(() =>
            {
                return McQuery.GetStatus(FactionsIP, FactionsPort);
            });
            bool isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(3000));
            if(isCompletedSuccessfully)
            { status = task.Result; }
            else
            { ServerOffline = true;}

            if (ServerOffline == true)
            {
                Console.WriteLine("Start Factions");
                UdpClient.Connect(FactionsIP, FactionsPort);
                Console.WriteLine("Udp Check success");
                // status = McQuery.GetStatus(FactionsIP, FactionsPort); //still failing when the server is offline... need to find a timeout.
                Console.WriteLine("status block achieved");
                Factions += "Number of Players: " + status.NumPlayers + "\n";
                foreach (var player in status.Players) Factions += player + "\t";
            }
            else
            {
                Console.WriteLine("Catch exception for Factions achieved");
                ServerOffline = true;
                Global.OfflineServers += "\t Factions";
                Factions = $"Factions is Offline!";
            }
            }
            catch (Exception)
            {
                Console.WriteLine("Catch exception for Factions achieved");
                ServerOffline = true;
                Global.OfflineServers += "\t Factions";
                Factions = $"Factions is Offline!";
            }
            Console.WriteLine("finished factions");
            ServerOffline = false;

            */
            status = null;
            if (debugging)
            { Console.WriteLine($"Finished Factinos"); }
            #endregion
            #region SkyBlock
            if (debugging)
            { Console.WriteLine($"Working on SkyBlock"); }
            string SkyBlock = "";
            IP = SkyBlockIP;Port = SkyBlockPort; ms = new MineStat(IP, Port);
            if (ms.ServerUp)
            {
                currentplayers = ms.GetCurrentPlayers(); maxplayers = ms.GetMaximumPlayers();Version = ms.GetVersion();
                if ((Version.Contains("Waterfall")) || (Version.Contains("Paper")))
                    Version = Version.Substring(Version.IndexOf(" " + 1));
                SkyBlock += $"Server Status: Online \n Supported McVersion(s): {Version} \n Online Players: {currentplayers} out of {maxplayers} \n \t";
                Port = Convert.ToUInt16((Convert.ToInt32(Port)) + PortDifference);
                status = McQuery.GetStatus(IP, Port);
                foreach (var player in status.Players) SkyBlock += player + "\t";
                if (Convert.ToInt16(currentplayers) > 0) SkyBlock += "\n";
            }
            else
            {
                ServerOffline = true;
                Global.ImportantServers += "\t __SkyBlock__";
                Global.OfflineServers += "\t SkyBlock";
                SkyBlock = "[Server Status: Offline!] \n";
            }
            /*
                try
                {
                var task = Task.Run(() =>
                {
                    return McQuery.GetStatus(FactionsIP, FactionsPort);
                });
                bool isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(3000));
                if (isCompletedSuccessfully)
                { status = task.Result; }
                else
                { ServerOffline = true;Console.WriteLine("Throw"); throw new TimeoutException("Server offline"); }
                Console.WriteLine("Post Throw");
                UdpClient.Connect(SkyBlockIP, SkyBlockPort);
                //status = McQuery.GetStatus(SkyBlockIP, SkyBlockPort);
                if (ServerOffline == false)
                {
                    SkyBlock += "Number of Players: " + status.NumPlayers + "\n";
                    foreach (var player in status.Players) SkyBlock += player + "\t";
                }
                else
                {
                    SkyBlock = "Server Offline";
                }
                }
                catch (Exception)
                {
                    ServerOffline = true;
                    Global.OfflineServers += "\t SkyBlock";
                    SkyBlock = $"SkyBlock is Offline!";
                }
            */
            status = null;
            if (debugging)
            { Console.WriteLine($"Finished SkyBlock"); }
            #endregion
            #region SurvivalHub
            if (debugging)
            { Console.WriteLine($"Working on SurvivalHub"); }
            string SurvivalHub = "";
            IP = SurvivalHubIP; Port = SurvivalHubPort; ms = new MineStat(IP, Port);
            if (ms.ServerUp)
            {
                currentplayers = ms.GetCurrentPlayers(); maxplayers = ms.GetMaximumPlayers(); Version = ms.GetVersion();
                if ((Version.Contains("Waterfall")) || (Version.Contains("Paper")))
                    Version = Version.Substring(Version.IndexOf(" " + 1));
                SurvivalHub += $"Server Status: Online \n Supported McVersion(s): {Version} \n Online Players: {currentplayers} out of {maxplayers} \n \t";
                Port = Convert.ToUInt16((Convert.ToInt32(Port)) + PortDifference);
                status = McQuery.GetStatus(IP, Port);
                foreach (var player in status.Players) SurvivalHub += player + "\t";
                if (Convert.ToInt16(currentplayers) > 0) SurvivalHub += "\n";
            }
            else
            {
                ServerOffline = true;
                Global.ImportantServers += "\t __SurvivalHub__";
                Global.OfflineServers += "\t SurvivalHub";
                SurvivalHub = "[Server Status: Offline!] \n";
            }
            /*
            try
                {
                    UdpClient.Connect(SurvivalHubIP, SurvivalHubPort);
                    status = McQuery.GetStatus(SurvivalHubIP, SurvivalHubPort);
                    SurvivalHub += "Number of Players: " + status.NumPlayers + "\n";
                    foreach (var player in status.Players) SurvivalHub += player + "\t";
                }
                catch (Exception)
                {
                    ServerOffline = true;
                Global.OfflineServers += "\t SurvivalHub";
                SurvivalHub = $"SurvivalHub is Offline!";
                }
            */
            status = null;
            if (debugging)
            { Console.WriteLine($"Finished SurvivalHub"); }
            #endregion
            #region Creative
            if (debugging)
            { Console.WriteLine($"Working on Creative"); }
            string Creative = "";
            IP = CreativeIP; Port = CreativePort; ms = new MineStat(IP, Port);
            if (ms.ServerUp)
            {
                currentplayers = ms.GetCurrentPlayers(); maxplayers = ms.GetMaximumPlayers(); Version = ms.GetVersion();
                if ((Version.Contains("Waterfall")) || (Version.Contains("Paper")))
                    Version = Version.Substring(Version.IndexOf(" " + 1));
                Creative += $"Server Status: Online \n Supported McVersion(s): {Version} \n Online Players: {currentplayers} out of {maxplayers} \n \t";
                Port = Convert.ToUInt16((Convert.ToInt32(Port)) + PortDifference);
                status = McQuery.GetStatus(IP, Port);
                foreach (var player in status.Players) Creative += player + "\t";
                if (Convert.ToInt16(currentplayers) > 0) Creative += "\n";
            }
            else
            {
                ServerOffline = true;
                Global.ImportantServers += "\t __Creative__";
                Global.OfflineServers += "\t Creative";
                Creative = "[Server Status: Offline!] \n";
            }
            /*
            try
                {
                    UdpClient.Connect(SurvivalHubIP, SurvivalHubPort);
                    status = McQuery.GetStatus(SurvivalHubIP, SurvivalHubPort);
                    SurvivalHub += "Number of Players: " + status.NumPlayers + "\n";
                    foreach (var player in status.Players) SurvivalHub += player + "\t";
                }
                catch (Exception)
                {
                    ServerOffline = true;
                Global.OfflineServers += "\t SurvivalHub";
                SurvivalHub = $"SurvivalHub is Offline!";
                }
            */
            status = null;
            if (debugging)
            { Console.WriteLine($"Finished SurvivalHub"); }
            #endregion
            #region StaffTest
            if (debugging)
            { Console.WriteLine($"Working on StaffTest"); }
            string StaffTest = "";
            IP = StaffTestIP; Port = StaffTestPort; ms = new MineStat(IP, Port);
            if (ms.ServerUp)
            {
                currentplayers = ms.GetCurrentPlayers(); maxplayers = ms.GetMaximumPlayers(); Version = ms.GetVersion();
                if ((Version.Contains("Waterfall")) || (Version.Contains("Paper")))
                    Version = Version.Substring(Version.IndexOf(" " + 1));
                StaffTest += $"Server Status: Online \n Supported McVersion(s): {Version} \n Online Players: {currentplayers} out of {maxplayers} \n \t";
                Port = Convert.ToUInt16((Convert.ToInt32(Port)) + PortDifference);
                status = McQuery.GetStatus(IP, Port);
                foreach (var player in status.Players) StaffTest += player + "\t";
                if (Convert.ToInt16(currentplayers) > 0) StaffTest += "\n";
            }
            else
            {
                ServerOffline = true;
                if (debugging == true)
                    Global.ImportantServers += "\t __StaffTest__";
                Global.OfflineServers += "\t StaffTest";
                StaffTest = "[Server Status: Offline!] \n";
            }
            /*
                try
                {
                var task = Task.Run(() =>
                {
                    return McQuery.GetStatus(FactionsIP, FactionsPort);
                });
                bool isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(3000));
                if (isCompletedSuccessfully)
                { status = task.Result; }
                else
                { ServerOffline = true;  }
                UdpClient.Connect(StaffTestIP, StaffTestPort);
                //status = McQuery.GetStatus(StaffTestIP, StaffTestPort);
                    if (ServerOffline == false)
                    {
                        StaffTest += "Number of Players: " + status.NumPlayers + "\n";
                        foreach (var player in status.Players) StaffTest += player + "\t";
                    }
                    else
                    {
                        StaffTest = "Server Offline";
                    }
                }
                catch (Exception)
                {
                    ServerOffline = true;
                    Global.OfflineServers += "\t StaffTest";
                    StaffTest = $"StaffTest is Offline!";
                }
            */
            status = null;
            if (debugging)
            { Console.WriteLine($"Finished StaffTest"); }
            #endregion
            string FiveM = "[PlaceHolder; not yet completed.]";
            string Rust = "[PlaceHolder; not yet completed.]";
            string McFull = $"Network: {Network} \n Hub: {Hub} \n Survival: {Survival} \n SurvivalHub: {SurvivalHub} \n SkyBlock: {SkyBlock} \n Creative: {Creative} \n Factions: {Factions} \n StaffTest: {StaffTest}";
            string AllServerStatus = $"Network: {Network} \n Hub: {Hub} \n Survival: {Survival} \n SurvivalHub: {SurvivalHub} \n SkyBlock: {SkyBlock} \n Creative: {Creative} \n Factions: {Factions} \n StaffTest: {StaffTest} \n FiveM: {FiveM} \n Rust: {Rust}";
            string valuestatus = null;
            switch(ServerName.ToLower())
            {
                case "minecraft":
                case "mc":
                    valuestatus = McFull;
                    break;
                case "hub":
                    valuestatus = "Hub: "+ Hub;
                    break;
                case "network":
                    valuestatus = "Network: "+ Network;
                    break;
                case "survival":
                    valuestatus = "Survival: "+ Survival;
                    break;
                case "creative":
                    valuestatus = "Creative: " + Creative;
                    break;
                case "factions":
                    valuestatus = "Factions: "+ Factions;
                    break;
                case "fivem":
                    valuestatus = "FiveM: "+ FiveM;
                    break;
                case "rust":
                    valuestatus = "Rust: "+ Rust;
                    break;
                case "stafftest":
                    valuestatus = "Staff Test: " + StaffTest;
                    break;
                default:
                    valuestatus = AllServerStatus;
                    break;

            }
            var currenttime = DateTime.Now;
            string OutputText = $"```css\n   ~~~Current Status as of [{currenttime} (Eastern Time, -4:00 GMT)]: ~~~ \n { valuestatus} \n Having issues? Try running no1/Fixconnection!```";
            string UsersToNotify = "";
            
            if((ServerOffline == true) && (Global.ImportantServers.Length > 2))
            {
                if(Global.NotifyList.Length >0)
                {
                    if (debugging) { Console.WriteLine("Global NotifyList: " +Global.NotifyList); }
                    OutputText += "The following servers are offline Unexpectedly! \n Triggered by the following Critical Server(s): \n" +
                            $"**{Global.ImportantServers}** \n being offline! \n Additionally, the following servers are offline: \n {Global.OfflineServers}";
                    foreach (string UserID in Global.NotifyList.Split(","))
                    {
                        if (UserID.Length > 1)
                        {
                            UsersToNotify += $"<@{UserID}> \t";
                        }
                        if (debugging) { Console.WriteLine("Iteration, UserID: "+UserID); }
                    }
                    OutputText += "\n Notifying users...: " + UsersToNotify;
                }
                else
                {
                    OutputText += "\n \n Nobody to notify, Is the database online?";
                }
            }
            Global.OfflineServers = "";
            Global.ImportantServers = "";
            //Global.OfflineServers = "";
            await ctx.RespondAsync(OutputText);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Command("notify"), Description("Notification command for offline servers!")]
        public async Task Notify(CommandContext ctx, [Description("Parameters accepted: Status, True/Enable, False/Disable.")] string stringnotifyIsSet)
        {
            bool debugging = false; string UserID = ctx.User.Id.ToString();
            //public static DBQuery SetDBQuery(string UserID, bool NotificationPreference = false)
            string Response = "";
            bool notifyIsSet = false;
            try
            {
                if (stringnotifyIsSet.ToLower() == "status")
                {
                    bool? CurrentPreference = DBQuery.GetNotificationPreference(ctx.User.Id.ToString());
                    if (Boolean.TryParse(CurrentPreference.ToString(), out _))
                    {
                        bool tempvalue = Convert.ToBoolean(CurrentPreference);
                        if (tempvalue)
                        {
                            Response = "Your current have notifications Enabled for online servers!";
                        }
                        else
                        {
                            Response = "You currently have notifications Disabled for offline servers!";
                        }
                    }
                    else
                    {
                        Response = "Error, User does not exist in database! Try no1/notifyme to be added!";
                    }
                    //respond with current
                }
                else if (stringnotifyIsSet.ToLower() == "enable*") { stringnotifyIsSet = "true"; }
                else if (stringnotifyIsSet.ToLower() == "disable*") { stringnotifyIsSet = "false"; }
                else if (Boolean.TryParse(stringnotifyIsSet.ToString(), out _))
                {
                    // continue and parse to bool
                    notifyIsSet = Convert.ToBoolean(stringnotifyIsSet);
                    if (debugging) { Console.WriteLine("Try block"); }
                    bool? CurrentPreference = DBQuery.GetNotificationPreference(ctx.User.Id.ToString());
                    if (Boolean.TryParse(CurrentPreference.ToString(), out _))
                    {
                        bool CurrentDBPreference = false;
                        if (CurrentPreference == true)
                        { CurrentDBPreference = true; }
                        else
                        { CurrentDBPreference = false; }

                        if ((CurrentDBPreference == true) && (notifyIsSet == true))
                        {
                            Response = "Notification Preference is set to True!";
                            if (debugging)
                            {
                                Console.WriteLine("A table update was not required.");
                                Console.WriteLine("~~~");
                                Console.WriteLine("CurrentDBPreference = " + CurrentDBPreference.ToString());
                                Console.WriteLine("~~~");
                            }
                        }
                        else if ((CurrentDBPreference == false) && (notifyIsSet == false))
                        {
                            Response = "Notification Preference is set to False!";
                            if (debugging)
                            {
                                Console.WriteLine("A table update was not required.");
                                Console.WriteLine("~~~");
                                Console.WriteLine("CurrentDBPreference = " + CurrentDBPreference.ToString());
                                Console.WriteLine("~~~");

                            }
                        }
                        else
                        {
                            if (debugging) { Console.WriteLine("A table update IS required."); }
                            if (notifyIsSet == true)
                            {
                                DBQuery.SetDBQuery(UserID, true);
                                Response = $"<@{UserID}>, You will now be notified about Offline Servers!";
                                Console.WriteLine("~~~");
                                Console.WriteLine("CurrentDBPreference = " + CurrentDBPreference.ToString());
                                Console.WriteLine("~~~");

                            }
                            else
                            {
                                DBQuery.SetDBQuery(UserID, false);
                                Response = $"<@{UserID}>, You will NO LONGER be notified about Offline Servers!";
                                Console.WriteLine("~~~");
                                Console.WriteLine("CurrentDBPreference = " + CurrentDBPreference.ToString());
                                Console.WriteLine("~~~");

                            }
                        }
                    }
                    else if (DBQuery.IsDBAvailable())
                    {
                        //user not found
                        if (debugging)
                        {
                            Console.WriteLine("[Error] start");
                            Console.WriteLine("else block");
                            Console.WriteLine(CurrentPreference.ToString());
                            Console.WriteLine("[Error] end");
                        }

                        Response = "User was not found in Database! If you would like to be notified, you can do no1/NotifyMe to be added to the table!";
                    }
                    else
                    {
                        Response = "I regret to inform you, my Database is currently Unavailable. Please Contact my creator.";
                    }
                }
                else
                {
                    Response = "Invalid Syntax! Try: no1/notify True/False/Status";
                }
            }
            catch
            {
                Response = "Unexpected Error, Please contact creator.";
            }

            await ctx.RespondAsync(Response);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Command("notifyme"), Description("Get added to our database and alerted when servers status change to Online!")]
        public async Task NotifyMe(CommandContext ctx, [Description("Parameters accepted: true/false for notification status (default false)")] bool Notifyset = false)
        {
            bool debugging = true; string UserID = ctx.User.Id.ToString();
            //public static DBQuery SetDBQuery(string UserID, bool NotificationPreference = false)
            string Response = "";bool exists = false;
            try
            {
                    if (debugging) { Console.WriteLine("Try block"); }
                DBQuery.GetUserIDsDBQuery();
                if (debugging) { Console.WriteLine("Current value of Global.DBQueryResult: " + Global.DBQueryResult); }
                    if (debugging) { Console.WriteLine("Finished GetDBQuery"); }



                foreach (String tempuserID in Global.DBQueryResult.Split(","))
                {
                    if (debugging) { Console.WriteLine("if(global.dbquery contains)"); }
                    if (tempuserID == ctx.User.Id.ToString())
                    {
                        exists = true;
                        await ctx.RespondAsync("Error, Your UserID already exists in our Database! Try running no1/notify <parameter> instead!");
                    }
                }
                if (exists == false)
                {
                    if (debugging) { Console.WriteLine("exists == false"); }
                    //add user to db with var Notifyset 
                    DBQuery.AddDBUser(ctx.User.Id.ToString(), ctx.User.Username.ToString(), Notifyset);
                    Console.WriteLine("Finished AddDBUser");
                    Response = $"User {ctx.User.Username.ToString()} added to Database!";
                }
            }
            catch
            {
                if (debugging) { Console.WriteLine("Caught in catch."); }
                Response = "Unexpected Error, Please contact creator. User not added to Database";
            }
            await ctx.RespondAsync(Response);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Command("version"), Description("Write version and patch notes!")]
        public async Task Version(CommandContext ctx)
        {
            string Response = "```css";
            Response += "\n Current Version: " + Global.Version;
            Response += "\n Last Updated: "+ Global.LastUpdated;
            Response += "\n Last Updated By: " + Global.LastUpdatedBy;
            Response += "\n Features Added: " + Global.FeaturesAdded;
            Response += "\n Features Removed: " + Global.FeaturesRemoved;
            Response += "```";
            await ctx.RespondAsync(Response);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Command("fixconnection"), Description("Having some connection issues? Run this command!")]
        public async Task Fixconnection(CommandContext ctx)
        {
            string Response = "```css";
            using (var client = new SshClient("x.x.x.x", "redacted", "redacted"))
            {
                client.Connect();
                var cmd = client.CreateCommand("/etc/UPNPC/UPNPC-Update.sh");
                var result = cmd.Execute();
                Console.WriteLine(result);
                client.Disconnect();
            }
            Response += "\n Port update script has completed running... Please try connecting again!";
            Response += "\n Please note, this is not a fix all, but can happen sometimes due to Host ISP Issues.";
            Response += "\n \n If this command was instant, either there was no need to run the command, or it may be processing in the background! More logic to come...";

            Response += "```";
            await ctx.RespondAsync(Response);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Command("connect"), Description("Test command to ensure backend is working properly")]
        public async Task Connect(CommandContext ctx)
        {
            string Response = "";
            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/bin/bash", Arguments = "upnpc -l",};
            Process proc = new Process() { StartInfo = startInfo, };
            proc.Start();
            await ctx.RespondAsync(Response);
        }
    }
}

