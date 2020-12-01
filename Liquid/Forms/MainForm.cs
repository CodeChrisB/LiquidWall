using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;
using Liquid.MemoryManagers;
using Liquid.Objects;
using Newtonsoft.Json;
using System.Net;
using SlimDX.DirectInput;
using System.Threading;
using Liquid.Misc;
using Liquid.Objects.Structs;
using AqHaxCSGO.Forms.SubForms;

namespace Liquid 
{
    public partial class MainForm : Form 
    {
        private bool IsWaitingForInput = false;
        private int currentKey = 16;
        KeysConverter keyConverter = new KeysConverter();

        System.Timers.Timer timer = new System.Timers.Timer();




        public MainForm() 
        {
            InitializeComponent();
            AllocConsole();

            #region VERSION CHECK
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            int[] versionParts = { fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart };
            int[] latestVersion = GetVersion();

            if (latestVersion[0] != 0) {
                if (latestVersion[0] > versionParts[0]) {
                    DialogResult dr = MessageBox.Show("New version of AqHax-CSGO is available. Would you like to update ?", "New Version !", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes) {
                        Process.Start("https://github.com/krxdev-kaan/AqHax-CSGO/releases");
                    }
                } else if (latestVersion[0] == versionParts[0]) {
                    if (latestVersion[1] > versionParts[1]) {
                        DialogResult dr = MessageBox.Show("New version of AqHax-CSGO is available. Would you like to update ?", "New Version !", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes) {
                            Process.Start("https://github.com/krxdev-kaan/AqHax-CSGO/releases");
                        }
                    } else if (latestVersion[1] == versionParts[1]) {
                        if (latestVersion[2] > versionParts[2]) {
                            DialogResult dr = MessageBox.Show("New version of AqHax-CSGO is available. Would you like to update ?", "New Version !", MessageBoxButtons.YesNo);
                            if (dr == DialogResult.Yes) {
                                Process.Start("https://github.com/krxdev-kaan/AqHax-CSGO/releases");
                            }
                        }
                    }
                }
            }
            #endregion

            
            #region Visuals Data
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string programDataPath = Path.Combine(appDataPath, "Liquid");
            string fullSavePath = Path.Combine(programDataPath, "CSG.dat");
            try {
                string[] lines = File.ReadAllLines(fullSavePath);
                if (lines.Length == 0) {
                    File.WriteAllLines(fullSavePath, new string[] { "00", "255000000", "000255000", "255000000" });
                    lines = File.ReadAllLines(fullSavePath);
                }
                Color colorCT = new Color();
                Color colorT = new Color();
                Color colorR = new Color();
                int p = 0;
                foreach (string line in lines) {
                    if (p == 1) {
                        colorCT = Color.FromArgb(Convert.ToInt32(line.Substring(0, 3)), Convert.ToInt32(line.Substring(3, 3)), Convert.ToInt32(line.Substring(6, 3)));
                    }
                    if (p == 2) {
                        colorT = Color.FromArgb(Convert.ToInt32(line.Substring(0, 3)), Convert.ToInt32(line.Substring(3, 3)), Convert.ToInt32(line.Substring(6, 3)));
                    }
                    if (p == 3) {
                        colorR = Color.FromArgb(Convert.ToInt32(line.Substring(0, 3)), Convert.ToInt32(line.Substring(3, 3)), Convert.ToInt32(line.Substring(6, 3)));
                    }
                    p++;
                }

                //ctColor.BackColor = colorCT;
                //tColor.BackColor = colorT;
                //rColor.BackColor = colorR;
                Globals.WallHackEnemy = colorCT;
                Globals.WallHackTeammate = colorT;
                Globals.RenderColor = colorR;
            } catch {
                try {
                    try {
                        File.Create(fullSavePath);
                        File.WriteAllLines(fullSavePath, new string[] { "00", "255000000", "000255000", "255000000" });
                    } catch {
                        Directory.CreateDirectory(programDataPath);
                        File.Create(fullSavePath);
                        File.WriteAllLines(fullSavePath, new string[] { "00", "255000000", "000255000", "255000000" });
                    }
                } catch {
                    MessageBox.Show("IO Error. \nApp save file cannot be initialized. \nRunning the program again should shortly fix th issue.",
                                    "FATAL ERROR");
                    Environment.Exit(1);
                }
            }
            #endregion

            #region Settings
            SaveManager.SettingsScheme settings = SaveManager.LoadSettings();
            Globals.BunnyHopAccuracy = Math.Abs(settings.BunnyAccuracy - 5);
            Globals.IdleWait = Math.Abs(settings.IdlePowerConsumption - 50) / 10;
            Globals.UsageDelay = Math.Abs(settings.UsagePowerConsumption - 5);
            Globals.TriggerKey = settings.TriggerKey;
            currentKey = settings.TriggerKey;
            #endregion
 
            #region SETUP
            if (!Memory.Init()) {
                timer.Stop();
                timer.Dispose();
                timer = null;
                if (Program.entryForm.InvokeRequired) {
                    Program.entryForm.BeginInvoke((MethodInvoker)delegate () {
                        Program.entryForm.Visible = true;
                    });
                }
                this.Close();
            } 
            #endregion

            #region EVENT REGISTER
            timer.Elapsed += new ElapsedEventHandler(UpdateHandle);
            timer.Interval = 90000;
            timer.Start();
            #endregion


            
        }

        private int[] GetVersion() 
        {
            try 
            {
                using (var webC = new WebClient()) 
                {
                    webC.Headers.Add("User-Agent", "request");
                    string json = webC.DownloadString("https://api.github.com/repos/krxdev-kaan/AqHax-CSGO/releases");
                    JsonTextReader reader = new JsonTextReader(new StringReader(json));
                    while (reader.Read()) 
                    {
                        if (reader.Value != null) 
                        {
                            if (reader.TokenType == JsonToken.PropertyName) 
                            {
                                if (reader.Value.ToString() == "tag_name") 
                                {
                                    reader.Read();
                                    string version = reader.Value.ToString();
                                    string int_ified = version.Substring(1);
                                    string[] splitted = int_ified.Split('.');
                                    int[] result = new int[3];
                                    for (int i = 0; i < splitted.Length; i++) 
                                    {
                                        result[i] = Convert.ToInt32(splitted[i]);
                                    }
                                    return result;
                                }
                            }
                        }
                    }
                    return new int[] { 0, 0, 0 };
                }
            } 
            catch 
            {
                return new int[] { 0, 0, 0 };
            }
        }
 

        #region Events

        private void KeyEvent(object sender, KeyEventArgs e)
        {
            if (IsWaitingForInput) 
            {
                currentKey = e.KeyValue;
                //keyButton.Text = e.KeyCode.ToString();
                IsWaitingForInput = false;
            }
        }

        private void UpdateHandle(object source, ElapsedEventArgs e)
        {
            if (!(Process.GetProcessesByName("csgo").Length > 0)) 
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
                if (this.InvokeRequired) {
                    this.BeginInvoke((MethodInvoker)delegate () 
                    {
                        this.Hide();
                        Program.entryForm.Visible = true;
                        this.Close();
                        });
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e) 
        {
            NetvarManager.LoadOffsets();
            OffsetManager.ScanOffsets();
            Threads.InitAll();
            FreeConsole();
            NetvarManager.netvarList.Clear();
        }
        #endregion

        #region User Events
        private void AimButton_Click(object sender, EventArgs e)
        {
            openChildForm(new AimForm());
        }

        private void VisualButton_Click(object sender, EventArgs e)
        {
            openChildForm(new VisualForm());
        }
        #endregion

        #region Some Shit For Loading State
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();


        #endregion

        #region ChangeChild

        private Form activeForm = null;
        private void openChildForm(Form child)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = child;
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;

            child.BringToFront();
            child.Show();
        }


        #endregion

        private void repoLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
            System.Diagnostics.Process.Start("https://github.com/codechrisb/liquid");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult result =MessageBox.Show(this, "This Hack Client was created by @CodeChrisB, do you want to go to the Repository of the hack client?.",
                                   "Help Caption", MessageBoxButtons.OK,
                                   MessageBoxIcon.Question,
                                   MessageBoxDefaultButton.Button1, 0,
                                   "mspaint.chm",
                                   "mspaint.chm::/paint_brush.htm");

            if(result == DialogResult.OK)
                System.Diagnostics.Process.Start("https://github.com/codechrisb/liquid");

        }



        private void button1_Click(object sender, EventArgs e)
        {
            Globals.RadarEnabled = !Globals.RadarEnabled;
            button1.BackColor = Globals.RadarEnabled ? Color.Green : Color.Red;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Globals.WallHackEnabled = !Globals.WallHackEnabled;
            button2.BackColor = Globals.WallHackEnabled ? Color.Green : Color.Red;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Globals.AntiFlashEnabled = !Globals.AntiFlashEnabled;
            button3.BackColor = Globals.AntiFlashEnabled ? Color.Green : Color.Red;

        }
    }
}
