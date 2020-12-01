using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Liquid.Objects;
using Liquid.Objects.Structs;
using LiveCharts;
using Liquid.Hacks;

namespace AqHaxCSGO.Forms.SubForms
{
    public partial class AimForm : Form
    {
        public AimForm()
        {
            InitializeComponent();
            progressBarBodyPercentage.RightToLeftLayout = true;
            SetBars();
        }

        private void SetBars()
        {
            progressBarHeadPercentage.Value = trackBarHeadPercentage.Value;
            progressBarBodyPercentage.Value = 100-trackBarHeadPercentage.Value;
        }

        private void ChangeButton(Button b, bool global)
        {
            b.BackColor = global ? Color.Green : Color.DarkRed;
        }

        private void btnRageBot_Click(object sender, EventArgs e)
        {
            Globals.AimEnabled = !Globals.AimEnabled;

            ChangeButton(btnRageBot,Globals.AimEnabled);
            if (Globals.AimEnabled)
            {
                Globals.TriggerEnabled = false;
                ChangeButton(btnTrigger, Globals.TriggerEnabled);
            }
        }

        Func<ChartPoint, string> label = charpoint => string.Format("{0} {1}",charpoint.Y,charpoint.Participation);



        private void btnFocus_Click(object sender, EventArgs e)
        {
            Globals.AimShootOnCollide = !Globals.AimShootOnCollide;
            ChangeButton(btnFocus, Globals.AimShootOnCollide);
        }

        private void btnRecoil_Click(object sender, EventArgs e)
        {
            Globals.AimRecoil = !Globals.AimRecoil;
            ChangeButton(btnRecoil, Globals.AimRecoil);
        }

        private void btnTrigger_Click(object sender, EventArgs e)
        {
            Globals.TriggerEnabled = !Globals.TriggerEnabled;
            ChangeButton(btnTrigger, Globals.TriggerEnabled);

            if (Globals.AimEnabled)
            {
                Globals.AimEnabled = false;
                ChangeButton(btnRageBot, Globals.AimEnabled);
            }

        }

        private void btnTriggerClick_Click(object sender, EventArgs e)
        {
            Globals.TriggerPressOnlyEnabled = !Globals.TriggerPressOnlyEnabled;
            ChangeButton(btnTriggerClick, Globals.TriggerPressOnlyEnabled);
        }

        private void btnAssist_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("This will be implemented one Day");
            Globals.AimAssist = !Globals.AimAssist;
            ChangeButton(btnAssist, Globals.AimAssist);
        }

        private void trackBarHeadPercentage_Scroll(object sender, EventArgs e)
        {
            SetBars();
            Globals.HeadShotPercentage = trackBarHeadPercentage.Value%100; //just to be safe
        }

        private void friendlyFire_Click(object sender, EventArgs e)
        {
            Globals.FriendlyFire = !Globals.FriendlyFire;
            ChangeButton(friendlyFire, Globals.FriendlyFire);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Globals.searchFriend = !Globals.searchFriend;
            ChangeButton(button1, Globals.searchFriend);

        }
    }
}
