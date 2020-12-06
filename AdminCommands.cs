using System;
using System.Collections.Generic;
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
using DiscordBot.Models.Client;
using DiscordBot.Models.Node;
using DiscordBot.Models.User;
 
namespace DiscordBot
{
    public class AdminCommands
    {
        public bool debugging = true;
        public PClient client = new PClient("URL", "token");
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Command("admintest"), Description("Admin Test Command")]
        public async Task admintest(CommandContext ctx)
        {
            string Response = ""; string UserID = ctx.User.Id.ToString();
            DBQuery.UpdateBotAdminList();


            if (Global.BotAdmins.Split(",").Contains(UserID))
            {
                Response = "You have access to this command, but it does nothing just yet... sorry...";
            }
            else
            {
                Response = $"Sorry {ctx.User.Username}, This command is for Admins only. " +
                    "Additional permissions will be added for all users in the future but you do not currently have rights to this command." +
                    $"If you believe this is in error, Please contact {Global.Creator}";
            }

            await ctx.RespondAsync(Response);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Command("listservers"), Description("List all of the servers we have.")]
        public async Task ListServers(CommandContext ctx)
        {
            string Response = "```css\n"; string UserID = ctx.User.Id.ToString();
            DBQuery.UpdateBotAdminList();

            Console.WriteLine("test");
            Console.WriteLine(client.Admin_GetServers());
            if (Global.BotAdmins.Split(",").Contains(UserID))
            {
                if (debugging) { Console.WriteLine("[Debugging] List Servers"); }
                foreach (ServerDatum srv in client.Admin_GetServers())
                {
                    if (debugging) { Console.WriteLine("[Debugging] Foreach client.Admin_GetServers"); }
                    if (debugging) { Console.WriteLine("[Debugging] Working on Identifier: "+srv.Attributes.Identifier); }
                    if (debugging) { Console.WriteLine("[Debugging] Passed If/else Statement and set response. "); }
                    Response += (srv.Attributes.Name + "@" + srv.Attributes.Identifier + " Current State:" + srv.Attributes.Status +"\n");
                }
                
                Response += "```";
            }
            else
            {
                Response = $"Sorry {ctx.User.Username}, This command is for Admins only. " +
                    "Additional permissions will be added for all users in the future but you do not currently have rights to this command." +
                    $"If you believe this is in error, Please contact {Global.Creator}";
            }

            await ctx.RespondAsync(Response);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*[Command("admintest"), Description("Admin Test Command")]
        public async Task admintest(CommandContext ctx)
        {
            string Response = ""; string UserID = ctx.User.Id.ToString();
            DBQuery.UpdateBotAdminList();


            if (Global.BotAdmins.Split(",").Contains(UserID))
            {
                Response = "You have access to this command, but it does nothing just yet... sorry...";
            }
            else
            {
                Response = $"Sorry {ctx.User.Username}, This command is for Admins only. " +
                    "Additional permissions will be added for all users in the future but you do not currently have rights to this command." +
                    $"If you believe this is in error, Please contact {Global.Creator}";
            }

            await ctx.RespondAsync(Response);
        }*/
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*[Command("admintest"), Description("Admin Test Command")]
        public async Task admintest(CommandContext ctx)
        {
            string Response = ""; string UserID = ctx.User.Id.ToString();
            DBQuery.UpdateBotAdminList();


            if (Global.BotAdmins.Split(",").Contains(UserID))
            {
                Response = "You have access to this command, but it does nothing just yet... sorry...";
            }
            else
            {
                Response = $"Sorry {ctx.User.Username}, This command is for Admins only. " +
                    "Additional permissions will be added for all users in the future but you do not currently have rights to this command." +
                    $"If you believe this is in error, Please contact {Global.Creator}";
            }

            await ctx.RespondAsync(Response);
        }*/
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
