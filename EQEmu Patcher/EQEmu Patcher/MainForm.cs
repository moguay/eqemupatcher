using System;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
//using System.Windows.Shell;

namespace EQEmu_Patcher
{
    
    public partial class MainForm : Form
    {

        /****
         *  EDIT THESE VARIABLES FOR EACH SERVER
         * 
         ****/
        public static string serverName = "EQ ProFusion";
        public static string filelistUrl = "http://patch.eqprofusion.com.s3.fr-par.scw.cloud/";
        public static bool defaultAutoPlay = false; //When a user runs this first time, what should Autoplay be set to?
        public static bool defaultAutoPatch = true; //When a user runs this first time, what should Autopatch be set to?
        public static bool defaultAsync = true; //When a user runs this first time, what should Async Download be set to?

        //Note that for supported versions, the 3 letter suffix is needed on the filelist_###.yml file.
        public static List<VersionTypes> supportedClients = new List<VersionTypes> { //Supported clients for patcher
            //VersionTypes.Unknown, //unk
            //VersionTypes.Titanium, //tit
            //VersionTypes.Underfoot, //und
            //VersionTypes.Secrets_Of_Feydwer, //sof
            //VersionTypes.Seeds_Of_Destruction, //sod
            VersionTypes.Rain_Of_Fear, //rof
            VersionTypes.Rain_Of_Fear_2 //rof
            //VersionTypes.Broken_Mirror, //bro
        }; 
        //*** END OF EDIT ***


        bool isLoading;
        bool isNeedingPatch;
        private Dictionary<VersionTypes, ClientVersion> clientVersions = new Dictionary<VersionTypes, ClientVersion>();

        VersionTypes currentVersion;

       // TaskbarItemInfo tii = new TaskbarItemInfo();
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (MainForm.defaultAutoPlay || MainForm.defaultAutoPatch)
            {
                Console.WriteLine("Auto default enabled");
            }

            isLoading = true;

            txtList.Visible = false;
            chkLogPatch.Checked = false;

            splashLogo.Visible = true;
            if (this.Width < 432) {
                this.Width = 432;
            }
            if (this.Height < 550)
            {
                this.Height = 550;
            }
            buildClientVersions();
            IniLibrary.Load();
            detectClientVersion();
            
            if (IniLibrary.instance.ClientVersion == VersionTypes.Unknown)
            {
                detectClientVersion();
                if (currentVersion == VersionTypes.Unknown)
                {
                    this.Close();
                }
                IniLibrary.instance.ClientVersion = currentVersion;
                IniLibrary.Save();
            }
            string suffix = "unk";
            if (currentVersion == VersionTypes.Titanium) suffix = "tit";
            if (currentVersion == VersionTypes.Underfoot) suffix = "und";
            if (currentVersion == VersionTypes.Seeds_Of_Destruction) suffix = "sod";
            if (currentVersion == VersionTypes.Broken_Mirror) suffix = "bro";
            if (currentVersion == VersionTypes.Secrets_Of_Feydwer) suffix = "sof";
            if (currentVersion == VersionTypes.Rain_Of_Fear || currentVersion == VersionTypes.Rain_Of_Fear_2) suffix = "rof";

            bool isSupported = false;
            foreach (var ver in supportedClients)
            {
                if (ver != currentVersion) continue;                
                isSupported = true;
                break;
            }
            if (!isSupported) {
                MessageBox.Show("The server " + serverName + " does not work with this copy of Everquest (" + currentVersion.ToString().Replace("_", " ") + ")", serverName);
                this.Close();
                return;
            }

            this.Text = serverName + " (Client: " + currentVersion.ToString().Replace("_", " ") + ")";

            string webUrl = filelistUrl + suffix + "/filelist_" + suffix + ".yml";
            string response = DownloadFile(webUrl, "filelist.yml");
            if (response != "")
            {
                MessageBox.Show("Failed to fetch filelist from " + webUrl + ": " + response);
                this.Close();
                return;
            }

            txtList.Visible = false;
            chkLogPatch.Checked = false;

            splashLogo.Visible = true;
            FileList filelist;            

            using (var input = File.OpenText("filelist.yml"))
            {
                var deserializerBuilder = new DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention());

                var deserializer = deserializerBuilder.Build();

                filelist = deserializer.Deserialize<FileList>(input);
            }
            
            if (filelist.version != IniLibrary.instance.LastPatchedVersion)
            {
                isNeedingPatch = true;
               btnCheck.BackColor = Color.Red;
            } else
            {                
                if ( IniLibrary.instance.AutoPlay.ToLower() == "true") PlayGame();
            }
            chkAutoPlay.Checked = (IniLibrary.instance.AutoPlay == "true");
            chkAutoPatch.Checked = (IniLibrary.instance.AutoPatch == "true");
            chkAsyncPatch.Checked = (IniLibrary.instance.Async == "true");

            isLoading = false;
            if (File.Exists("eqemupatcher.png"))
            {
                splashLogo.Load("eqemupatcher.png");
            }
        }

        System.Diagnostics.Process process;
      

        System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();

        Dictionary<string, string> WalkDirectoryTree(System.IO.DirectoryInfo root)
        {
            System.IO.FileInfo[] files = null;
            var fileMap = new Dictionary<string, string>();
            try
            {
                 files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                txtList.Text += e.Message +"\n";
                return fileMap;
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                txtList.Text += e.Message + "\r\n";
                return fileMap;
            }

            if (files != null)
            {
                
                foreach (System.IO.FileInfo fi in files)
                {
                    if (fi.Name.Contains(".ini"))
                    { //Skip INI files
                        progressBar.Value++;
                        continue;
                    }
                    if (fi.Name == System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)
                    { //Skip self EXE
                        progressBar.Value++;
                        continue;
                    }

                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    var md5 = UtilityLibrary.GetMD5(fi.FullName);
                    txtList.Text += fi.Name + ": " + md5 + "\r\n";
                    if (progressBar.Maximum > progressBar.Value) {
                        progressBar.Value++;
                    }
                    fileMap[fi.Name] = md5;
                    txtList.Refresh();
                    updateTaskbarProgress();
                    Application.DoEvents();
                    
                }
                //One final update of data
                if (progressBar.Maximum > progressBar.Value)
                {
                    progressBar.Value++;
                }
                txtList.Refresh();
                updateTaskbarProgress();
                Application.DoEvents();
            }
            return fileMap;
        }
        

        private void detectClientVersion()
        {

            try
            {

                var hash = UtilityLibrary.GetEverquestExecutableHash(AppDomain.CurrentDomain.BaseDirectory);
                if (hash == "")
                {
                    MessageBox.Show("Please run this patcher in your Everquest directory.");
                    this.Close();
                    return;
                }
                switch (hash)
                {
                    case "85218FC053D8B367F2B704BAC5E30ACC":
                        currentVersion = VersionTypes.Secrets_Of_Feydwer;
                        splashLogo.Image = Properties.Resources.sof;
                        break;
                    case "859E89987AA636D36B1007F11C2CD6E0":
                    case "EF07EE6649C9A2BA2EFFC3F346388E1E78B44B48": //one of the torrented uf clients, used by B&R too
                        currentVersion = VersionTypes.Underfoot;
                        splashLogo.Image = Properties.Resources.underfoot;
                        break;
                    case "A9DE1B8CC5C451B32084656FCACF1103": //p99 client
                    case "BB42BC3870F59B6424A56FED3289C6D4": //vanilla titanium
                        currentVersion = VersionTypes.Titanium;
                        splashLogo.Image = Properties.Resources.titanium;
                        break;
                    case "368BB9F425C8A55030A63E606D184445":
                        currentVersion = VersionTypes.Rain_Of_Fear;
                        splashLogo.Image = Properties.Resources.rof;
                        break;
                    case "240C80800112ADA825C146D7349CE85B":
                    case "A057A23F030BAA1C4910323B131407105ACAD14D": //This is a custom ROF2 from a torrent download
                        currentVersion = VersionTypes.Rain_Of_Fear_2;
                        splashLogo.Image = Properties.Resources.rof;
                        break;
                    case "6BFAE252C1A64FE8A3E176CAEE7AAE60": //This is one of the live EQ binaries.
                    case "AD970AD6DB97E5BB21141C205CAD6E68": //2016/08/27
                        currentVersion = VersionTypes.Broken_Mirror;
                        splashLogo.Image = Properties.Resources.brokenmirror;
                        break;
                    default:
                        currentVersion = VersionTypes.Unknown;
                        break;
                }
                if (currentVersion == VersionTypes.Unknown)
                {
                    if (MessageBox.Show("Unable to recognize the Everquest client in this directory, open a web page to report to devs?", "Visit", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("https://github.com/Xackery/eqemupatcher/issues/new?title=A+New+EQClient+Found&body=Hi+I+Found+A+New+Client!+Hash:+" + hash);
                    }
                    txtList.Text = "Unable to recognize the Everquest client in this directory, send to developers: " + hash;
                }
                else
                {
                    //txtList.Text = "You seem to have put me in a " + clientVersions[currentVersion].FullName + " client directory";
                }
                
                //MessageBox.Show(""+currentVersion);
                
                //txtList.Text += "\r\n\r\nIf you wish to help out, press the scan button on the bottom left and wait for it to complete, then copy paste this data as an Issue on github!";
            }
            catch (UnauthorizedAccessException err)
            {
                MessageBox.Show("You need to run this program with Administrative Privileges" + err.Message);
                return;
            }
        }

        //Build out all client version's dictionary
        private void buildClientVersions()
        {
            clientVersions.Clear();
            clientVersions.Add(VersionTypes.Titanium, new ClientVersion("Titanium", "titanium"));
            clientVersions.Add(VersionTypes.Secrets_Of_Feydwer, new ClientVersion("Secrets Of Feydwer", "sof"));
            clientVersions.Add(VersionTypes.Seeds_Of_Destruction, new ClientVersion("Seeds of Destruction", "sod"));
            clientVersions.Add(VersionTypes.Rain_Of_Fear, new ClientVersion("Rain of Fear", "rof"));
            clientVersions.Add(VersionTypes.Rain_Of_Fear_2, new ClientVersion("Rain of Fear 2", "rof2"));
            clientVersions.Add(VersionTypes.Underfoot, new ClientVersion("Underfoot", "underfoot"));
            clientVersions.Add(VersionTypes.Broken_Mirror, new ClientVersion("Broken Mirror", "brokenmirror"));
        }

        private int getFileCount(System.IO.DirectoryInfo root) {
            int count = 0;
                           
            System.IO.FileInfo[] files = null;
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                txtList.Text += e.Message + "\n";
                return 0;
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                txtList.Text += e.Message + "\r\n";
                return 0;
            }

            if (files != null)
            {
              return files.Length;
            }
            return count;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            txtList.Text = "";
            progressBar.Maximum = getFileCount(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory));
            progressBar.Maximum += getFileCount(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Resources"));
            progressBar.Maximum += getFileCount(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\sounds"));
            progressBar.Maximum += getFileCount(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\SpellEffects"));
            progressBar.Maximum += getFileCount(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\storyline"));
          //  progressBar.Maximum += getFileCount(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\uifiles"));
          //  progressBar.Maximum += getFileCount(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\atlas"));
            txtList.Text = "Max:" + progressBar.Maximum;
            PatchVersion pv = new PatchVersion();
            pv.ClientVersion = clientVersions[currentVersion].ShortName;
            //Root
            var fileMap = WalkDirectoryTree(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory));
            pv.RootFiles = fileMap;
            //Resources
            fileMap = WalkDirectoryTree(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Resources"));
            pv.ResourceFiles = fileMap;
            //Sounds
            fileMap = WalkDirectoryTree(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\sounds"));
            pv.SoundFiles = fileMap;
            //SpellEffects
            fileMap = WalkDirectoryTree(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\SpellEffects"));
            pv.SpellEffectFiles = fileMap;
            //Storyline
            fileMap = WalkDirectoryTree(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\storyline"));
            pv.StorylineFiles = fileMap;
           /*
            //UIFiles
            fileMap = WalkDirectoryTree(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\uifiles"));
            pv.UIFiles = fileMap;
            //Atlas
            fileMap = WalkDirectoryTree(new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\atlas"));
            pv.AtlasFiles = fileMap;
            */
            //txtList.Text = JsonConvert.SerializeObject(pv);
        }

        private void updateTaskbarProgress()
        {
            
            if (Environment.OSVersion.Version.Major < 6)
            { //Only works on 6 or greater
                return;
            }
            
            
           // tii.ProgressState = TaskbarItemProgressState.Normal;            
           // tii.ProgressValue = (double)progressBar.Value / progressBar.Maximum;            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            PlayGame();            
        }

        private void PlayGame()
        {
            try
            {
                process = UtilityLibrary.StartEverquest();
                if (process != null) this.Close();
                else MessageBox.Show("The process failed to start");
            }
            catch (Exception err)
            {
                MessageBox.Show("An error occured while trying to start everquest: " + err.Message);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {

        }

        bool isPatching = false;
        bool isCancelled = false;
        bool isError = false;
        bool isDone = false;
        bool isPatchForce = false;
        bool isAsync = true;
        bool isAsyncDone = true;

        FileList filelist;
        List<FileEntry> filesToDownload = new List<FileEntry>();

        public static WebClient webClient;               // Our WebClient that will be doing the downloading for us
        public static Stopwatch sw = new Stopwatch();    // The stopwatch which we will be using to calculate the download speed

        long curBytes = 0;
        long reciveBytes = 0;
        long totalBytes = 0;
        int recivePercentage = 0;

        public object Keyboard { get; private set; }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (isPatching)
            {
                if (isAsync) { cancelDownload(); }
                btnCheck.Text = "Patch";
                isPatching = false;
                isDone = false;
                return;
            }
            if (isDone)
            {
                btnCheck.Text = "Patch";
                isPatching = false;
                isDone = false;
                isPatchForce = true;
            }

            StartPatch();
        }        

        private void StartPatch()
        {

            Process[] pname = Process.GetProcessesByName("eqgame");
            if (pname.Length != 0)
            {
                MessageBox.Show("Everquest is running, please close it.");
                return;
            }

            if (isPatching) return;
            isPatching = true;
            btnCheck.Text = "Cancel";

            txtList.Text = "Patching...";

            using (var input = File.OpenText("filelist.yml"))
            {
                var deserializerBuilder = new DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention());

                var deserializer = deserializerBuilder.Build();

                filelist = deserializer.Deserialize<FileList>(input);
            }

            foreach (var entry in filelist.downloads)
            {
                Application.DoEvents();
                var path = entry.name.Replace("/", "\\");
                //See if file exists.
                if (!File.Exists(path) || isPatchForce)
                {
                    //Console.WriteLine("Downloading: "+ entry.name);
                    filesToDownload.Add(entry);
                    if (entry.size < 1) totalBytes += 1;
                    else totalBytes += entry.size;
                }
                else
                {
                    var md5 = UtilityLibrary.GetMD5(path);

                    if (md5.ToUpper() != entry.md5.ToUpper())
                    {
                        Console.WriteLine(entry.name + ": " + md5 + " vs " + entry.md5);
                        filesToDownload.Add(entry);
                        if (entry.size < 1) totalBytes += 1;
                        else totalBytes += entry.size;
                    }
                }
                Application.DoEvents();
                if (!isPatching) { 
                    LogEvent("Patching cancelled.");
                    return;
                }

            }

            if (filelist.deletes != null && filelist.deletes.Count > 0)
            {
                foreach (var entry in filelist.deletes)
                {
                    if (File.Exists(entry.name))
                    {
                        LogEvent("Deleting " + entry.name + "...");
                        File.Delete(entry.name);
                    }
                    Application.DoEvents();
                    if (!isPatching)
                    {
                        LogEvent("Patching cancelled.");
                        return;
                    }
                }
            }

            if (filesToDownload.Count == 0)
            {
                LogEvent("Up to date with patch "+filelist.version+".");
                progressBar.Maximum = progressBar.Value = 1;
                IniLibrary.instance.LastPatchedVersion = filelist.version;
                IniLibrary.Save();
                btnCheck.BackColor = SystemColors.Control;
                btnCheck.Text = "Force Full Download";
                labelPerc.Text = "Done";
                isPatching = false;
                isDone = true;
                isPatchForce = false;
                return;
            }

            LogEvent("Downloading " + totalBytes + " bytes for " + filesToDownload.Count + " files...");
            curBytes = 0;

            if (!isAsync)
            {
                //progressBar.Maximum = totalBytes;
                progressBar.Maximum = 100;
                progressBar.Value = 0;
                foreach (var entry in filesToDownload)
                {
                    //progressBar.Value = (curBytes > totalBytes) ? totalBytes : curBytes;
                    progressBar.Value = (curBytes > totalBytes) ? 100 : (int)((100d / totalBytes) * (curBytes)); ;
                    string url = filelist.downloadprefix + entry.name.Replace("\\", "/");
                    DownloadFile(url, entry.name);
                    curBytes += entry.size;
                    Application.DoEvents();
                    if (!isPatching)
                    {
                        LogEvent("Patching cancelled.");
                        return;
                    }
                }
                progressBar.Value = progressBar.Maximum;
                LogEvent("Complete! Press Play to begin.");
                IniLibrary.instance.LastPatchedVersion = filelist.version;
                IniLibrary.Save();
                btnCheck.BackColor = SystemColors.Control;
                btnCheck.Text = "Force Full Download";
                labelPerc.Text = "Done";
                isPatching = false;
                isDone = true;
                isPatchForce = false;
            }
            else
            {
                progressBar.Maximum = 100;
                progressBar.Value = 0;
                string path;

                foreach (var entry in filesToDownload)
                {
                    isAsyncDone = false;
                    path = entry.name.Replace("/", "\\");
                    string url = filelist.downloadprefix + path;

                    LogEvent(path + "...");

                    DownloadFileUrl(url, entry.name);

                    while (!isAsyncDone)
                    {
                        Application.DoEvents();
                        if (isCancelled || isError) return;
                    }
                }

                    //progressBar.Value = progressBar.Maximum;
                    LogEvent("Complete! Press Play to begin.");
                    IniLibrary.instance.LastPatchedVersion = filelist.version;
                    IniLibrary.Save();
                    btnCheck.BackColor = SystemColors.Control;
                    btnCheck.Text = "Force Full Download";
                    labelPerc.Text = "Done";
                    isPatching = false;
                    isDone = true;
                    isPatchForce = false;
            }
        }

        private string DownloadFile(string url, string path)
        {

            path = path.Replace("/", "\\");
            if (path.Contains("\\"))
            { //Make directory if needed.

                string dir = Application.StartupPath + "\\" + path.Substring(0, path.LastIndexOf("\\"));
                Directory.CreateDirectory(dir);
            }

            //Console.WriteLine(Application.StartupPath + "\\" + path);
            LogEvent(path + "...");
            string reason = UtilityLibrary.DownloadFile(url, path);
            if (reason != "")
            {
                if (reason == "404")
                {
                    LogEvent("Failed to download " + url + ", 404 error (website may be down?)");
                    //MessageBox.Show("Patch server could not be found. (404)");
                }
                else
                {
                    LogEvent("Failed to download " + url + " for untracked reason: " + reason);
                    //MessageBox.Show("Patch server failed: (" + reason + ")");
                }
                return reason;
            }
            return "";
        }

        public void DownloadFileUrl(string urlAddress, string path)
        {
            path = path.Replace("/", "\\");
            if (path.Contains("\\"))
            { //Make directory if needed.

                string dir = Application.StartupPath + "\\" + path.Substring(0, path.LastIndexOf("\\"));
                Directory.CreateDirectory(dir);
            }

            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                // The variable that will be holding the url address (making sure it starts with http://)
                Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);

                // Start the stopwatch which we will be using to calculate the download speed
                sw.Start();

                try
                {
                    // Start downloading the file
                    webClient.DownloadFileAsync(URL, path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // The event that will fire whenever the progress of the WebClient is changed
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            reciveBytes = e.BytesReceived;

            // Calculate download speed and output it to labelSpeed.
            labelSpeed.Text = string.Format("{0} kb/s", ((double)(reciveBytes) / 1024d / sw.Elapsed.TotalSeconds).ToString("0"));

            // Update the progressbar percentage only when the value is not the same.
            //curBytes += entry.size

            //progressBar.Value = e.ProgressPercentage;
            recivePercentage = (int)((100d / totalBytes) * (curBytes + reciveBytes));
            if (recivePercentage > 100) { recivePercentage = 100; }

            progressBar.Value = recivePercentage;

            // Show the percentage on our label.
            //labelPerc.Text = e.ProgressPercentage.ToString() + "%";
            labelPerc.Text = recivePercentage + "%";


            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            labelDownloaded.Text = string.Format("{0} / {1} MB",
                ((curBytes + reciveBytes) / 1024d / 1024d).ToString("0.00"),
                //(e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
                (totalBytes / 1024d / 1024d).ToString("0.00"));
        }

        // The event that will trigger when the WebClient is completed
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LogEvent("Patching file error.");
                labelPerc.Text = "Patching error";

                string error = e.Error.ToString();
                // or
                // error = "Your error message here";

                isError = true;

                MessageBox.Show(error);
                return;
            }

            // Reset the stopwatch.
            sw.Reset();

            if (e.Cancelled == true)
            {
                //MessageBox.Show("Download has been canceled.");
                LogEvent("Patching cancelled.");
                labelPerc.Text = "Patching cancelled";
                isPatching = false;
                isCancelled = true;
                MessageBox.Show("Download has been canceled.");
            }
            else
            {
                isAsyncDone = true;
                curBytes = curBytes + reciveBytes;
                // MessageBox.Show("Download completed!");

/*                if (totalBytes == curBytes)
                {
                    progressBar.Value = progressBar.Maximum;
                    LogEvent("Complete! Press Play to begin.");
                    IniLibrary.instance.LastPatchedVersion = filelist.version;
                    IniLibrary.Save();
                    btnCheck.BackColor = SystemColors.Control;
                    btnCheck.Text = "Force Full Download";
                    labelPerc.Text = "Done";
                    // isPatching = false;
                    isDone = true;
                    isPatchForce = false;
                }*/
            }
        }

        private void cancelDownload()
        {
            webClient.CancelAsync();
        }

        private void LogEvent(string text)
        {
            if (!txtList.Visible)
            {
                txtList.Visible = true;
                chkLogPatch.Checked = true;

                splashLogo.Visible = false;
            }
            Console.WriteLine(text);
            txtList.AppendText(text + "\r\n");
        }

        private void chkAutoPlay_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            IniLibrary.instance.AutoPlay = (chkAutoPlay.Checked) ? "true" : "false";
            if (chkAutoPlay.Checked) LogEvent("To disable autoplay: edit eqemupatcher.yml or wait until next patch.");
            IniLibrary.Save();
        }

        private void chkAutoPatch_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            IniLibrary.instance.AutoPatch = (chkAutoPatch.Checked) ? "true" : "false";
            IniLibrary.Save();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (isNeedingPatch && IniLibrary.instance.AutoPatch == "true")
            {
                btnCheck.BackColor = SystemColors.Control;
                StartPatch();
            }
        }

        private void chkAsyncPatch_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            isAsync = (chkAsyncPatch.Checked) ? true : false;
            IniLibrary.instance.Async = (chkAsyncPatch.Checked) ? "true" : "false";
            IniLibrary.Save();
        }

        private void chkLogPatch_CheckedChanged(object sender, EventArgs e)
        {
            //if (isLoading) return;
            txtList.Visible = (chkLogPatch.Checked) ? true : false;
        }
    }
    public class FileList
    {
        public string version { get; set; }
        
        public List<FileEntry> deletes { get; set; }
        public string downloadprefix { get; set; }
        public List<FileEntry> downloads { get; set; }
        public List<FileEntry> unpacks { get; set; }

    }
    public class FileEntry
    {
        public string name { get; set;  }
        public string md5 { get; set; }
        public string date { get; set; }
        public string zip { get; set; }
        public int size { get; set; }
    }    
}


