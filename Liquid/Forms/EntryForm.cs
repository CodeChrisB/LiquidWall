using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using Liquid.MemoryManagers;
using System.IO;
using Liquid.Objects;
using System.Runtime.InteropServices;

namespace Liquid 
{
    public partial class EntryForm : Form 
    {
        public EntryForm() 
        {
            if ((Process.GetProcessesByName("csgo").Length > 0))
            {
                this.Shown += new EventHandler(InstantConnect);
                new MainForm().Show();
                this.Visible = false;

            }
            InitializeComponent();
        }

        private void InstantConnect(object sender, EventArgs e)
        {
            this.Visible = false;
        }


        private void initButton_Click(object sender, EventArgs e) 
        {
            if ((Process.GetProcessesByName("csgo").Length > 0)) 
            {
                new MainForm().Show();    
                this.Visible = false;
            }
        }

        private void launcherButton_Click_1(object sender, EventArgs e)
        {
            Process.Start("steam://rungameid/730");

        }
    }
}
