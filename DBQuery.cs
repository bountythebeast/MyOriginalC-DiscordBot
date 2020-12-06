using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace DiscordBot
{
    class DBQuery
    {
        public static DBQuery GetDBQuery()
        {
            string Returnstring = "";
            string connectionString;
            MySqlConnection cnn;
            connectionString = @$"Server=x.x.x.x;Port=xxxx;Database=TableName;UID=Username;Pwd=Redacted";
            cnn = new MySqlConnection(connectionString);
            try
            {
                cnn.Open();
                string sql = "Select ID,UserName,DiscordID,Notification_Preference,IsAdmin FROM Discord_Users";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                Returnstring += ("\n" + rdr[0] + " \t " + rdr[1] + " \t " + rdr[2] + " \t " + rdr[3] + " \t " + rdr[4]);
                Console.WriteLine(Returnstring);

            }
            catch (Exception ex)
            {
                Returnstring = ex.ToString();
            }            
            Global.DBQueryResult = Returnstring;
            cnn.Close();
            return null;

        }
        public static DBQuery GetUserIDsDBQuery()
        {
            string Returnstring = "";
            string connectionString = @$"Server=x.x.x.x;Port=xxxx;Database=TableName;UID=Username;Pwd=Redacted";
            MySqlConnection cnn = new MySqlConnection(connectionString);
            try
            {
                cnn.Open();
                string sql = "Select DiscordID FROM Discord_Users";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                { Returnstring += (rdr[0].ToString() + ","); }
                Console.WriteLine("NotifyList = " + Returnstring);
            }
            catch (Exception ex)
            {
                Returnstring = ex.ToString();
            }
            Global.DBQueryResult = Returnstring;
            cnn.Close();
            return null;

        }
        public static bool? GetNotificationPreference(string DiscordID)
        {
            string connectionString = @$"Server=x.x.x.x;Port=xxxx;Database=TableName;UID=Username;Pwd=Redacted";
            MySqlConnection cnn = new MySqlConnection(connectionString);
            try
            {
                cnn.Open();
                string sql = "Select Notification_Preference FROM Discord_Users where DiscordID='"+DiscordID+"';";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if(Convert.ToBoolean(rdr[0]))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetNotificationPreference! :"+ex.ToString());
            }
            cnn.Close();

            return null; //break case
        }
        public static DBQuery SetDBQuery(string UserID, bool NotificationPreference)
        {
            string Returnstring = "";
            string connectionString;
            MySqlConnection cnn;
            connectionString = @$"Server=x.x.x.x;Port=xxxx;Database=TableName;UID=Username;Pwd=Redacted";
            cnn = new MySqlConnection(connectionString);
            try
            {
                cnn.Open();
                Console.WriteLine("Connection Open !");
                string sql = "update Discord_Users set Notification_Preference='" + NotificationPreference + "'where DiscordID='"+UserID+"';";
                //string sql = "Select ID,UserName,DiscordID,Notification_Preference,IsAdmin FROM Discord_Users";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                Returnstring += $"Updated Record for: <@{UserID}> to {NotificationPreference}";
                Console.WriteLine(Returnstring);
                DBQuery.UpdateNotificationList();
            }
            catch (Exception ex)
            {
                Returnstring = ex.ToString() + "\n NotificationPreference was not updated!";
            }
            Global.DBQueryResult = Returnstring;
            cnn.Close();
            return null;
        }
        public static DBQuery UpdateNotificationList()
        {
            string Returnstring = "";
            string connectionString = @$"Server=x.x.x.x;Port=xxxx;Database=TableName;UID=Username;Pwd=Redacted";
            MySqlConnection cnn = new MySqlConnection(connectionString);
            try
            {
                cnn.Open();
                string sql = "Select DiscordID FROM Discord_Users where Notification_Preference='True'";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                { Returnstring += (rdr[0].ToString() + ","); }
                Console.WriteLine("NotifyList = "+Returnstring);

            }
            catch (Exception ex)
            {
                Returnstring = ex.ToString();
            }
            Global.NotifyList = Returnstring;
            cnn.Close();
            return null;
        }
        public static DBQuery UpdateBotAdminList()
        {
            string Returnstring = "";
            string connectionString = @$"Server=x.x.x.x;Port=xxxx;Database=TableName;UID=Username;Pwd=Redacted";
            MySqlConnection cnn = new MySqlConnection(connectionString);
            try
            {
                cnn.Open();
                string sql = "Select DiscordID FROM Discord_Users where IsAdmin='True'";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                { Returnstring += (rdr[0].ToString() + ","); }
                Console.WriteLine("BotAdmins ="+Returnstring);

            }
            catch (Exception ex)
            {
                Returnstring = ex.ToString();
            }
            Global.BotAdmins = Returnstring;
            return null;
        }
        public static DBQuery AddDBUser(string UserID,string DiscordName,bool NotificationPreference)
        {
            string Returnstring = ""; bool debugging = true;
            string connectionString = @$"Server=x.x.x.x;Port=xxxx;Database=TableName;UID=Username;Pwd=Redacted";
            MySqlConnection cnn = new MySqlConnection(connectionString);
            try
            {
                cnn.Open();
                if (debugging) { Console.WriteLine("AddDBUser try opened"); }
                if (DiscordName.Contains("'")) { DiscordName = DiscordName.Replace("'", "`"); }
                string sql = $"insert into Discord_Users (UserName, DiscordID, Notification_Preference, IsAdmin) VALUES('"+DiscordName+"','"+UserID+"','"+NotificationPreference+"','False');";
                //string sql = "Select ID,UserName,DiscordID,Notification_Preference,IsAdmin FROM Discord_Users";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (debugging) { Console.WriteLine("AddDBUser try attempting add."); }
                Returnstring += $"Added Record for: <@{UserID}> with Notification Preference: {NotificationPreference}";
                Console.WriteLine(Returnstring);
                if (debugging) { Console.WriteLine("Updating Notification List"); }
                DBQuery.UpdateNotificationList();
            }
            catch (Exception ex)
            {
                if (debugging) { Console.WriteLine("Broke into catch statement \n "+ex.ToString()); }
                Returnstring = ex.ToString() + "\n User was not added to Database! Error: "+ex;
            }
            Global.DBQueryResult = Returnstring;
            cnn.Close();
            return null;
        }
        public static bool IsDBAvailable()
        {

            string connectionString = @$"Server=x.x.x.x;Port=xxxx;Database=TableName;UID=Username;Pwd=Redacted";
            MySqlConnection cnn = new MySqlConnection(connectionString);
            try
            {
                cnn.Open();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }
    }
}
/*

        string MyConnection2 = "datasource=localhost;port=xxxx;username=redacted;password=redacted";  
        //This is my update query in which i am taking input from the user through windows forms and update the record.  
        
        //This is  MySqlConnection here i have created the object and pass my connection string.  
        MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);  
        MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);  
        MySqlDataReader MyReader2;  
        MyConn2.Open();  
        MyReader2 = MyCommand2.ExecuteReader();  
        MessageBox.Show("Data Updated");  
        while (MyReader2.Read())  
        {  
        
        }  
        MyConn2.Close();//Connection closed here  
        }  
        catch (Exception ex)  
        {   
            MessageBox.Show(ex.Message);  
        }  
    }  

 */
