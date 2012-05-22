using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using MySql.Data.MySqlClient;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class WebonaryMysqlDatabaseTransfer
    {
        public PublicationInformation projInfo;

        public bool RunScriptForCreateDatabase(string fileName, string userName, string password, string hostAddress, string port, string databaseName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                MySqlCommand command;
                // Create a connection string without passing a database 
                string ConnectionString = string.Format("Uid={0};Pwd={1};Server={2};Port={3}",
                userName, password, hostAddress, port);

                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                Connection.Open();
                try
                {
                    string line = "";
                    string l;
                    while (true)
                    {
                        // Clear line if its been used already 
                        if (line.EndsWith(";"))
                            line = "";

                        l = reader.ReadLine();

                        // is this eof? 
                        if (l == null) break;
                        // Trim off rubbish at end 
                        l = l.TrimEnd();
                        // Don't call if it's empty 
                        if (l == "") continue;
                        // If it's a comment dont call 
                        if (l.StartsWith("--")) continue;

                        // Add l to line 
                        line += l;
                        // Test for end of line character and continue reading if needed 
                        if (!line.EndsWith(";")) continue;

                        // mysql generated files start and end with series of commented out 
                        // lines. 
                        // These are not true comment out but are parsed by mysql to determine 
                        // the version number. If version higher than current version then they're 
                        // not run. So we need to remove the comment marks and pass the command 
                        // in. Im using mysql 5, so all valid 
                        if (line.StartsWith("/*!"))
                        {
                            if (line.EndsWith("*/;"))
                            {
                                int start = line.IndexOf(" ");
                                if (start == -1)
                                    continue;
                                line = line.Substring(start + 1);
                                line = line.Remove(line.Length - 4) + ";";

                            }
                            else continue;
                        }//if (line.StartsWith("/*!")) 
                        
                        // Now we have a full line, run it in mysql 
                        command = new MySqlCommand(line, Connection);
                        command.ExecuteNonQuery();
                    }// while true 
                }
                catch (MySqlException ex)
                {

                }
                finally
                {
                    Connection.Close();
                }
            }// using 

            return true;
        }// function


        public bool RunScript(string fileName, string userName, string password, string hostAddress, string port, string databaseName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                MySqlCommand command;
                // Create a connection string without passing a database 
                string ConnectionString = string.Format("Uid={0};Pwd={1};Server={2};Port={3}",
                userName, password, hostAddress, port);

                MySqlConnection Connection = new MySqlConnection(ConnectionString);
                Connection.Open();
                try
                {
                    string line = "";
                    string l;
                    while (true)
                    {
                        // Clear line if its been used already 
                        if (line.EndsWith(";"))
                            line = "";

                        l = reader.ReadLine();

                        // is this eof? 
                        if (l == null) break;
                        // Trim off rubbish at end 
                        l = l.TrimEnd();
                        // Don't call if it's empty 
                        if (l == "") continue;
                        // If it's a comment dont call 
                        if (l.StartsWith("--")) continue;

                        // Add l to line 
                        line += l;
                        // Test for end of line character and continue reading if needed 
                        if (!line.EndsWith(";")) continue;

                        // mysql generated files start and end with series of commented out 
                        // lines. 
                        // These are not true comment out but are parsed by mysql to determine 
                        // the version number. If version higher than current version then they're 
                        // not run. So we need to remove the comment marks and pass the command 
                        // in. Im using mysql 5, so all valid 
                        if (line.StartsWith("/*!"))
                        {
                            if (line.EndsWith("*/;"))
                            {
                                int start = line.IndexOf(" ");
                                if (start == -1)
                                    continue;
                                line = line.Substring(start + 1);
                                line = line.Remove(line.Length - 4) + ";";

                            }
                            else continue;
                        }//if (line.StartsWith("/*!")) 

                        line = "USE " + databaseName + "; " + line;
                        // Now we have a full line, run it in mysql 
                        command = new MySqlCommand(line, Connection);
                        command.ExecuteNonQuery();
                    }// while true 
                }
                catch (MySqlException ex)
                {

                }
                finally
                {
                    Connection.Close();
                }
            }// using 

            return true;
        }// function

        public string InstallWordPressPHPPage(string webPageUrl, string webFolder, string weblogTitle, string userName, string adminPassword, string adminEmail, string blogPublic)
        {
            string uri = string.Empty;
            // This is where we will send it
            uri = webPageUrl + "/" + webFolder + "/wp-admin/setup_wp.php" + "?weblog_title=" +
                             weblogTitle + "&user_name=" + userName + "&admin_password=" + adminPassword +
                             "&admin_email=" + adminEmail + "&blog_public=" + blogPublic + "";
            try
            {
                // This is what we are sending sample, don't remove or change
                string post_data = "&FrameworkVersionsss=sdf";
                // create a request
                HttpWebRequest request = (HttpWebRequest)
                                         WebRequest.Create(uri);
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";

                // turn our request string into a byte stream
                byte[] postBytes = Encoding.ASCII.GetBytes(post_data);

                // this is important - make sure you specify type this way
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postBytes.Length;
                Stream requestStream = request.GetRequestStream();

                // now send it
                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Close();

                // grab te response and print it out to the console along with the status code
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //Console.WriteLine(new StreamReader(response.GetResponseStream()).ReadToEnd());
                //Console.WriteLine(response.StatusCode);

                var consoleString = (new StreamReader(response.GetResponseStream()).ReadToEnd());
                var consoleStatusString = (response.StatusCode);
            }
            catch { }

            return uri;
        }


        public void CreateDatabase(string fileName, string userName, string password, string hostAddress, string port, string databaseName)
        {
            string getCreateDatabaseFile = Common.GetApplicationPath();
            getCreateDatabaseFile = Common.PathCombine(getCreateDatabaseFile, "wordpress");
            getCreateDatabaseFile = Common.PathCombine(getCreateDatabaseFile, fileName);
            string mysqlScriptFileName = Common.PathCombine(projInfo.ProjectPath, fileName);
            File.Copy(getCreateDatabaseFile, mysqlScriptFileName, true);
            ModifyDatabasenameInCreateDbSqlFile(mysqlScriptFileName, databaseName);
            mysqlScriptFileName = mysqlScriptFileName.Replace(".sql", "1.sql");
            RunScriptForCreateDatabase(mysqlScriptFileName, userName, password, hostAddress, port, databaseName);
        }

        public void Data(string fileName, string userName, string password, string hostAddress, string port, string databaseName, string websiteAddress, string directoryName)
        {
            //string getCreateDatabaseFile = Common.GetApplicationPath();
            //getCreateDatabaseFile = Common.PathCombine(getCreateDatabaseFile, "wordpress");
            //getCreateDatabaseFile = Common.PathCombine(getCreateDatabaseFile, fileName);
            string mysqlScriptFileName = Common.PathCombine(projInfo.ProjectPath, fileName);
            //File.Copy(getCreateDatabaseFile, mysqlScriptFileName, true);
            ModifyDatabasenameInDataDbSqlFile(mysqlScriptFileName, databaseName, websiteAddress, directoryName);
            mysqlScriptFileName = mysqlScriptFileName.Replace(".sql", "1.sql");
            RunScript(mysqlScriptFileName, userName, password, hostAddress, port, databaseName);
        }

        public void Drop2reset(string fileName, string userName, string password, string hostAddress, string port, string databaseName)
        {
            string getCreateDatabaseFile = Common.GetApplicationPath();
            getCreateDatabaseFile = Common.PathCombine(getCreateDatabaseFile, "wordpress");
            getCreateDatabaseFile = Common.PathCombine(getCreateDatabaseFile, fileName);
            string mysqlScriptFileName = Common.PathCombine(projInfo.ProjectPath, fileName);
            File.Copy(getCreateDatabaseFile, mysqlScriptFileName, true);
            RunScript(mysqlScriptFileName, userName, password, hostAddress, port, databaseName);
        }

        public void EmptyWebonary(string fileName, string userName, string password, string hostAddress, string port, string databaseName, string websiteAddress, string directoryName, string websiteTitle)
        {
            string getCreateDatabaseFile = Common.GetApplicationPath();
            getCreateDatabaseFile = Common.PathCombine(getCreateDatabaseFile, "wordpress");
            getCreateDatabaseFile = Common.PathCombine(getCreateDatabaseFile, fileName);
            string mysqlScriptFileName = Common.PathCombine(projInfo.ProjectPath, fileName);
            File.Copy(getCreateDatabaseFile, mysqlScriptFileName, true);
            ModifyDatabasenameInEmptyWebonaryDbSqlFile(mysqlScriptFileName, databaseName, websiteAddress, directoryName, websiteTitle);
            mysqlScriptFileName = mysqlScriptFileName.Replace(".sql", "1.sql");
            RunScript(mysqlScriptFileName, userName, password, hostAddress, port, databaseName);
        }

        private void ModifyDatabasenameInCreateDbSqlFile(string fileLocation, string databaseName)
        {
            string configFile = fileLocation;
            if (!File.Exists(configFile))
            {
                return;
            }

            FileStream fs = new FileStream(configFile, FileMode.Open);
            StreamReader stream = new StreamReader(fs);
            string configFile2 = configFile.Replace(".sql", "1.sql");

            if (File.Exists(configFile2))
            {
                File.Delete(configFile2);
            }

            var fs2 = new FileStream(configFile2, FileMode.Create, FileAccess.Write);
            var sw2 = new StreamWriter(fs2);
            string line;
            while ((line = stream.ReadLine()) != null)
            {

                if (line.IndexOf("CREATE USER 'webonary'@'localhost'") >= 0)
                {
                    line = line.Replace("CREATE USER 'webonary'@'localhost'", "CREATE USER '" + databaseName + "'@'localhost'");
                }

                if (line.IndexOf("GRANT USAGE ON * . * TO 'webonary'@'localhost'") >= 0)
                {
                    line = line.Replace("GRANT USAGE ON * . * TO 'webonary'@'localhost'", "GRANT USAGE ON * . * TO '" + databaseName + "'@'localhost'");
                }

                if (line.IndexOf("CREATE DATABASE IF NOT EXISTS `webonary`") >= 0)
                {
                    line = line.Replace("CREATE DATABASE IF NOT EXISTS `webonary`", "CREATE DATABASE IF NOT EXISTS `" + databaseName + "`");
                }

                if (line.IndexOf("GRANT ALL PRIVILEGES ON `webonary` . * TO 'webonary'@'localhost';") >= 0)
                {
                    line = line.Replace("GRANT ALL PRIVILEGES ON `webonary` . * TO 'webonary'@'localhost';", "GRANT ALL PRIVILEGES ON `" + databaseName + "` . * TO '" + databaseName + "'@'localhost';");
                }

                sw2.WriteLine(line);
            }
            sw2.Close();
            fs.Close();
            fs2.Close();
        }


        private void ModifyDatabasenameInEmptyWebonaryDbSqlFile(string fileLocation, string databaseName, string websiteAddress, string directoryName, string websiteTitle)
        {
            string configFile = fileLocation;
            if (!File.Exists(configFile))
            {
                return;
            }

            FileStream fs = new FileStream(configFile, FileMode.Open);
            StreamReader stream = new StreamReader(fs);

            string configFile2 = configFile.Replace(".sql", "1.sql");
            if (File.Exists(configFile2))
            {
                File.Delete(configFile2);
            }

            var fs2 = new FileStream(configFile2, FileMode.Create, FileAccess.Write);
            var sw2 = new StreamWriter(fs2);
            string line;
            while ((line = stream.ReadLine()) != null)
            {

                if (line.IndexOf("http://localhost/wordpress") >= 0)
                {
                    //if(websiteAddress.Substring(websiteAddress.Length-2, websiteAddress.Length-1) == "/")
                    //{
                    line = line.Replace("http://localhost/wordpress", websiteAddress + "/" + directoryName);
                    //}
                    //else
                    //{
                    //    line = line.Replace("http://localhost/wordpress", websiteAddress + directoryName);
                    //}
                }
                if (line.IndexOf("FROM `wordpress`.") >= 0)
                {
                    line = line.Replace("FROM `wordpress`.", "FROM `" + databaseName + "`.");
                }

                if (line.IndexOf("'blogname', 'Webonary', 'yes')") >= 0)
                {
                    line = line.Replace("'blogname', 'Webonary', 'yes')", "'blogname', '" + websiteTitle + "', 'yes')");
                }

                if (line.IndexOf("'Online Dictionary', 'site-title', 0)") >= 0)
                {
                    line = line.Replace("'Online Dictionary', 'site-title', 0)", "'Online Dictionary', '" + websiteTitle + "', 0)");
                }


                sw2.WriteLine(line);
            }
            sw2.Close();
            fs.Close();
            fs2.Close();
        }

        private void ModifyDatabasenameInDataDbSqlFile(string fileLocation, string databaseName, string websiteAddress, string directoryName)
        {
            string configFile = fileLocation;
            if (!File.Exists(configFile))
            {
                return;
            }

            FileStream fs = new FileStream(configFile, FileMode.Open);
            StreamReader stream = new StreamReader(fs);

            string configFile2 = configFile.Replace(".sql", "1.sql");

            if (File.Exists(configFile2))
            {
                File.Delete(configFile2);
            }

            var fs2 = new FileStream(configFile2, FileMode.Create, FileAccess.Write);
            var sw2 = new StreamWriter(fs2);
            string line;
            while ((line = stream.ReadLine()) != null)
            {

                if (line.IndexOf("http://localhost/wordpress") >= 0)
                {
                    //if (websiteAddress.Substring(websiteAddress.Length - 1, websiteAddress.Length) == "/")
                    //{
                    line = line.Replace("http://localhost/wordpress", websiteAddress + "/" + directoryName);
                    //}
                    //else
                    //{
                    //    line = line.Replace("http://localhost/wordpress", websiteAddress + directoryName);
                    //}
                }
                if (line.IndexOf("FROM `wordpress`.") >= 0)
                {
                    line = line.Replace("FROM `wordpress`.", "FROM `" + databaseName + "`.");
                }

                if (line.IndexOf("AudioVisual/") >= 0)
                {
                    //  line = line.Replace("AudioVisual/", websiteAddress + "/" + directoryName + "/" + "AudioVisual/");
                }

                sw2.WriteLine(line);
            }
            sw2.Close();
            fs.Close();
            fs2.Close();
        }
    }
}
