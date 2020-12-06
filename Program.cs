using DSharpPlus;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Net;
using System.Collections.Generic;


namespace DiscordBot
{
    class Program
    {


        static bool DevMode = false; // SET TO FALSE WHEN PUBLISHING BOT!!!
        //x86_64
        static DiscordClient discord;
        static CommandsNextModule commands;
        static void Main(string[] args)
        {
            Console.WriteLine("Bot Starting...");

            Console.WriteLine("Testing DB Connections");
            if(DBQuery.IsDBAvailable())
            {
                Console.WriteLine("\t DiscordBot Database connection Successful; Updating DB's!");
                DBQuery.UpdateNotificationList();
                DBQuery.UpdateBotAdminList();

            }
            else
            {
                Console.WriteLine("\t [WARNING] DiscordBot Database connection FAILED!; NOTIFICATIONS DISABLED!");
            }
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            string CurrentToken; string CurrentPrefix;
            if (DevMode)
            {
                CurrentToken = "token";
                CurrentPrefix = "no1test/";
            }
            else
            {
                CurrentToken = "token";
                CurrentPrefix = "no1/";
            }
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = $"{CurrentToken}",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Error,
            });

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                CaseSensitive = false,
                StringPrefix = $"{CurrentPrefix}",
            });
            commands.RegisterCommands<StandardCommands>();
            commands.RegisterCommands<AdminCommands>();

            if (DevMode) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[WARNING]: Running in DevMode! NOT FOR PRODUCTION!");
                Console.ResetColor();
            }
            Console.ResetColor();
            Console.WriteLine("Bot is now online!");
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
    
    class Global
    {
        public static string Creator = "No1's Perfect | Server Host#6666";
        public static string CreatorId = "239152780009275392";
        public static string ChronicFull = "Chronic-Reflexes#9251";
        public static string ChronicId = "181122655104008194";
        public static string OfflineServers = "";
        public static string ImportantServers = "";
        public static string BotAdmins = "";
        public static string NotifyList = "";
        public static string DBQueryResult = "";

        public static string Version = "0.3";
        public static string LastUpdated = "10/25/2020";
        public static string LastUpdatedBy = "No1's Perfect | Server Host";
        public static string FeaturesAdded = "";
        public static string FeaturesRemoved = "";
    }
}
