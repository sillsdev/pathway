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
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class WebonaryFileTransfer : Form
    {

        #region Private Variables

        private int _totalFiles = 1;
        private int _filesCount = 1;

        #endregion

        #region Constructor

        public WebonaryFileTransfer()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Event(s)

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            StartedFileTransferToFTPLocation();
        }

        private void btnPickFile_Click(object sender, EventArgs e)
        {
            DialogResult result = directoryDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.txtSourceFileLocation.Text = directoryDialog.SelectedPath;
            }
        }

        #endregion

        #region Private Method(s)

        private void StartBackgroundWork()
        {
            if (Application.RenderWithVisualStyles)
                progressBar.Style = ProgressBarStyle.Continuous;
            else
            {
                progressBar.Style = ProgressBarStyle.Marquee;
                progressBar.Maximum = 100;
                progressBar.Value = 0;
            }
        }

        private void GetDirectoryFileCount(string p)
        {
            string[] directoryLocalfiles;
            directoryLocalfiles = Directory.GetFiles(txtSourceFileLocation.Text);
            foreach (string fileName in directoryLocalfiles)
            {
                _totalFiles++;
            }

            //Directory Creating in FTP
            string[] directories = Directory.GetDirectories(txtSourceFileLocation.Text, "*.*", SearchOption.AllDirectories);
            foreach (string directoryName in directories)
            {
                string subDirectory = Common.LeftRemove(directoryName, txtSourceFileLocation.Text);
                string filePath = txtSourceFileLocation.Text + subDirectory;
                string[] files;
                files = Directory.GetFiles(filePath);
                foreach (string fileName in files)
                {
                    _totalFiles++;
                }
            }
        }

        private void ftpUpload()
        {

            string[] directoryLocalfiles;
            directoryLocalfiles = Directory.GetFiles(txtSourceFileLocation.Text);

            if (directoryLocalfiles.Length > 0)
            {
                foreach (string fileName in directoryLocalfiles)
                {
                    FileInfo f2 = new FileInfo(fileName);
                    Int64 fileLength = f2.Length;
                    progressBar.Value = Convert.ToInt32(_filesCount++ * 100 / _totalFiles);
                    fileupload(fileName, txtTargetFileLocation.Text, txtUsername.Text, txtPassword.Text);
                }
            }

            //Directory Creating in FTP
            string[] directories = Directory.GetDirectories(txtSourceFileLocation.Text, "*.*",
                                                            SearchOption.AllDirectories);
            foreach (string directoryName in directories)
            {
                string subDirectory = Common.LeftRemove(directoryName, txtSourceFileLocation.Text);

                //DirectoryInfo di = new DirectoryInfo(directoryName + "\\");

                createDirectoryToUpload(txtTargetFileLocation.Text + subDirectory, txtUsername.Text,
                                        txtPassword.Text);

                string filePath = txtSourceFileLocation.Text + subDirectory;
                string[] files;
                files = Directory.GetFiles(filePath);
                foreach (string fileName in files)
                {
                    //File Creating in FTP Directory
                    FileInfo f2 = new FileInfo(fileName);
                    Int64 fileLength = f2.Length;
                    progressBar.Value = Convert.ToInt32(_filesCount++ * 100 / _totalFiles);
                    fileupload(fileName, txtTargetFileLocation.Text + subDirectory, txtUsername.Text,
                               txtPassword.Text);
                }
            }
        }

        private void createDirectoryToUpload(string ftpLocation, string userName, string password)
        {
            WebRequest request = WebRequest.Create(ftpLocation);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential(userName, password);
            using (var resp = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine(resp.StatusCode);
            }
        }

        private void fileupload(string uploadDirectoryFiles, string ftpLocation, string userName, string password)
        {
            //Get a FileInfo object for the file that will
            // be uploaded.
            FileInfo toUpload = new FileInfo(uploadDirectoryFiles);

            //Get a new FtpWebRequest object.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpLocation + "/" + toUpload.Name);

            //Method will be UploadFile.
            request.Method = WebRequestMethods.Ftp.UploadFile;

            //Set our credentials.
            request.Credentials = new NetworkCredential(userName, password);

            //Setup a stream for the request and a stream for
            // the file we'll be uploading.
            Stream ftpStream = request.GetRequestStream();
            FileStream file = System.IO.File.OpenRead(uploadDirectoryFiles);

            //Setup variables we'll use to read the file.
            int length = 1024;
            byte[] buffer = new byte[length];
            int bytesRead = 0;

            //Write the file to the request stream.
            do
            {
                bytesRead = file.Read(buffer, 0, length);
                ftpStream.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);

            //Close the streams.
            file.Close();
            ftpStream.Close();

            //  MessageBox.Show("Upload complete");
        }

        #endregion

        private void WebonaryFileTransfer_Load(object sender, EventArgs e)
        {
            GetFtpandMysql();
            ProceedFileTransfer();
        }

        private void GetFtpandMysql()
        {
            Param.LoadSettings();
            XmlNodeList baseNode1 = Param.GetItems("//styles/web/style[@name='" + "OneWeb" + "']/styleProperty");
            HashUtilities hashUtil = new HashUtilities();
            hashUtil.Key = "%:#@?,*&";

            // show/hide web UI controls based on the input type

            foreach (XmlNode VARIABLE in baseNode1)
            {
                string attribName = VARIABLE.Attributes["name"].Value.ToLower();
                string attribValue = VARIABLE.Attributes["value"].Value;
                switch (attribName)
                {
                    case "ftpaddress":
                        txtTargetFileLocation.Text = attribValue;
                        break;
                    case "ftpuserid":
                        txtUsername.Text = attribValue;
                        break;
                    case "ftppwd":
                        txtPassword.Text = hashUtil.Decrypt(attribValue);
                        break;
                    case "dbservername":
                        txtSqlServerName.Text = attribValue;
                        break;
                    case "dbname":
                        txtSqlDBName.Text = attribValue;
                        break;
                    case "dbuserid":
                        txtSqlUsername.Text = attribValue;
                        break;
                    case "dbpwd":
                        txtSqlPassword.Text = hashUtil.Decrypt(attribValue);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ProceedFileTransfer()
        {
            try
            {
                PublicationInformation projInfo = new PublicationInformation();
                projInfo.ProjectInputType = "Dictionary";

                string dictionaryDirectoryPath = Path.Combine(Path.Combine(Path.Combine(Common.GetAllUserAppPath(), "SIL"), "Pathway"), projInfo.ProjectInputType);
                string[] filePaths = Directory.GetFiles(dictionaryDirectoryPath, "*.zip");
                string webonaryZipFile = Path.Combine(dictionaryDirectoryPath, "PathwayWebonary.zip");
                txtSourceFileLocation.Text = Path.Combine(dictionaryDirectoryPath, "Wordpress\\");
                if (filePaths.Length > 0)
                {
                    ZipUtil.UnZipFiles(webonaryZipFile, Path.Combine(dictionaryDirectoryPath, "Wordpress"), "", false);
                    StartedFileTransferToFTPLocation();
                }
                else
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    webClient.DownloadFileAsync(new Uri("http://pathway.sil.org/wp-content/sprint/PathwayWebonary-0.5.zip"), webonaryZipFile);
                }

            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == 530)
                {

                }
            }
            catch (WebException ex)
            {
                if (ex.Message == "The remote server returned an error: (530) Not logged in.")
                {
                    MessageBox.Show("FTP Username and Password are invalid, Please verify in the configuration tool in web property tab.", "Pathway", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("FTP settings are invalid, Please verify in the configuration tool in web property tab.", "Pathway", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                this.Close();
            }
        }

        private void StartedFileTransferToFTPLocation()
        {
            GetDirectoryFileCount(txtSourceFileLocation.Text);
            SetPHPConfigFile(Path.Combine(txtSourceFileLocation.Text, "Webonary"));
            StartBackgroundWork();
            progressBar.Value = 2;
            ftpUpload();
            progressBar.Value = 100;
            ////MessageBox.Show("Transfer Completed");
            string address = txtTargetFileLocation.Text;
            address = address.Replace("ftp", "http");
            using (Process.Start(address + "Webonary/" + "index.php"))
            {

            }
        }

        private void SetPHPConfigFile(string fileLocation)
        {
            string tempConfigFile = Common.PathCombine(fileLocation, "wp-config-sample.php");
            string configFile = Common.PathCombine(fileLocation, "wp-config.php");
            if (!File.Exists(tempConfigFile))
            {
                return;
            }
            
            FileStream fs = new FileStream(tempConfigFile, FileMode.Open);
            StreamReader stream = new StreamReader(fs);

            var fs2 = new FileStream(configFile, FileMode.Create, FileAccess.Write);
            var sw2 = new StreamWriter(fs2);
            string line;
            string splitDefine = string.Empty;
            splitDefine = GetAuthenticationUniqueKeysandSalts();
            string[] stringSeparators = new string[] { "\n" };
            string[] keyAndSalt = splitDefine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            while ((line = stream.ReadLine()) != null)
            {

                if (line.IndexOf("define('DB_NAME', 'database_name_here');") >= 0)
                {
                    line = line.Replace("define('DB_NAME', 'database_name_here');", "define('DB_NAME', '"+ txtSqlDBName.Text + "');");
                }
                if (line.IndexOf("define('DB_USER', 'username_here');") >= 0)
                {
                    line = line.Replace("define('DB_USER', 'username_here');", "define('DB_USER', '" + txtSqlUsername.Text + "');");
                }
                if (line.IndexOf("define('DB_PASSWORD', 'password_here');") >= 0)
                {
                    line = line.Replace("define('DB_PASSWORD', 'password_here');", "define('DB_PASSWORD', '" + txtSqlPassword.Text + "');");
                }
                if (line.IndexOf("define('DB_HOST', 'localhost');") >= 0)
                {
                    line = line.Replace("define('DB_HOST', 'localhost');", "define('DB_HOST', '" + txtSqlServerName.Text + "');");
                }
                if (line.IndexOf("define('AUTH_KEY',         'put your unique phrase here');") >= 0)
                {
                    line = line.Replace("define('AUTH_KEY',         'put your unique phrase here');", keyAndSalt[0]);
                }
                if (line.IndexOf("define('SECURE_AUTH_KEY',  'put your unique phrase here');") >= 0)
                {
                    line = line.Replace("define('SECURE_AUTH_KEY',  'put your unique phrase here');", keyAndSalt[1]);
                }
                if (line.IndexOf("define('LOGGED_IN_KEY',    'put your unique phrase here');") >= 0)
                {
                    line = line.Replace("define('LOGGED_IN_KEY',    'put your unique phrase here');", keyAndSalt[2]);
                }
                if (line.IndexOf("define('NONCE_KEY',        'put your unique phrase here');") >= 0)
                {
                    line = line.Replace("define('NONCE_KEY',        'put your unique phrase here');", keyAndSalt[3]);
                }
                if (line.IndexOf("define('AUTH_SALT',        'put your unique phrase here');") >= 0)
                {
                    line = line.Replace("define('AUTH_SALT',        'put your unique phrase here');", keyAndSalt[4]);
                }
                if (line.IndexOf("define('SECURE_AUTH_SALT', 'put your unique phrase here');") >= 0)
                {
                    line = line.Replace("define('SECURE_AUTH_SALT', 'put your unique phrase here');", keyAndSalt[5]);
                }
                if (line.IndexOf("define('LOGGED_IN_SALT',   'put your unique phrase here');") >= 0)
                {
                    line = line.Replace("define('LOGGED_IN_SALT',   'put your unique phrase here');", keyAndSalt[6]);
                } 
                if (line.IndexOf("define('NONCE_SALT',       'put your unique phrase here');") >= 0)
                {
                    line = line.Replace("define('NONCE_SALT',       'put your unique phrase here');", keyAndSalt[7]);
                }

                sw2.WriteLine(line);
            }
            sw2.Close();
            fs.Close();
            fs2.Close();
        }


        private string GetAuthenticationUniqueKeysandSalts()
        {
            string getUserData = string.Empty;
            string uri = string.Empty;
            // This is where we will send it
            uri = "https://api.wordpress.org/secret-key/1.1/salt/";
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

                var consoleString = (new StreamReader(response.GetResponseStream()).ReadToEnd());
                var consoleStatusString = (response.StatusCode);
                getUserData = consoleString;
                return getUserData;
            }
            catch { }

            return getUserData;
        }


        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                PublicationInformation projInfo = new PublicationInformation();
                projInfo.ProjectInputType = "Dictionary";
                string dictionaryDirectoryPath = Path.Combine(Path.Combine(Path.Combine(Common.GetAllUserAppPath(), "SIL"), "Pathway"), projInfo.ProjectInputType);
                string webonaryZipFile = Path.Combine(dictionaryDirectoryPath, "PathwayWebonary.zip");
                ZipUtil.UnZipFiles(webonaryZipFile, Path.Combine(dictionaryDirectoryPath, "Wordpress"), "", false);
                StartedFileTransferToFTPLocation();

            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == 530)
                {

                }
            }
            catch (WebException ex)
            {
                if (ex.Message == "The remote server returned an error: (530) Not logged in.")
                {
                    MessageBox.Show("FTP Username and Password are invalid, Please verify in the configuration tool in web property tab.", "Pathway", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("FTP settings are invalid, Please verify in the configuration tool in web property tab.", "Pathway", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                this.Close();
            }
        }
    }
}
