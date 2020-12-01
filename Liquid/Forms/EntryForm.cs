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
            InitializeComponent();
        }


        private void launcherButton_Click(object sender, EventArgs e) 
        {
            Process.Start("steam://rungameid/730");
        }

        private void initButton_Click(object sender, EventArgs e) 
        {
            if ((Process.GetProcessesByName("csgo").Length > 0)) 
            {
               
                new MainForm().Show();    
                this.Visible = false;
            }
        }


    }
}
