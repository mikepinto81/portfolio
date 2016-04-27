using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace VolumeHotKeyApp
{
    public partial class Form1 : Form
    {
        KeyHandler upkey;
        KeyHandler downkey;
        KeyHandler mutekey;
        SystemVolumeControl volControl;
        //used to store info not in the KeyHandlers
        Settings extraSettingObject = new Settings();
        bool loading = false;

        public Form1()
        {
            InitializeComponent();
                        
            //Methods object to access windows volume controls
            volControl = new SystemVolumeControl(this);
            
            //add a default modifier
            List<Constants.Modifiers> modList = new List<Constants.Modifiers>();            
            modList.Add(Constants.Modifiers.CTRL);

            //select the CTRL item by default in the selection list
            modselectbox.SelectedIndex = 1;
                        
            //fill select box with Key names
            foreach(string keyName in Enum.GetNames(typeof(Keys)))
            {
                volumeupkey.Items.Add(keyName);
                volumedownkey.Items.Add(keyName);
                mutecombobox.Items.Add(keyName);
            }          

            //create the Keyhandlers
            upkey = new KeyHandler(Keys.Up, modList, this);
            downkey = new KeyHandler(Keys.Down, modList, this);
            mutekey = new KeyHandler(Keys.M, modList, this);       

            //try and load vars if not just turn on the defaults
            if (!SaveLoad.Load(upkey, downkey, mutekey ,extraSettingObject))                
                TurnOn(); //turn on volume control
      
            SyncVisuals();
        }

        void SyncVisuals()
        {
            //set loading so we don't get callbacks after changing form items
            loading = true;

            //set default key selections
            volumedownkey.SelectedIndex = volumedownkey.Items.IndexOf(Enum.GetName(typeof(Keys), downkey.getKey));
            volumeupkey.SelectedIndex = volumeupkey.Items.IndexOf(Enum.GetName(typeof(Keys), upkey.getKey));
            mutecombobox.SelectedIndex = mutecombobox.Items.IndexOf(Enum.GetName(typeof(Keys), mutekey.getKey));

            //go through list of mods on any of the keys (since they all share the same mods) and set the display
            modselectbox.ClearSelected();
            for (int i = 0; i < upkey.getModifiers.Count; i++)
            {
                modselectbox.SetSelected(modselectbox.Items.IndexOf(Constants.ModToReadableString(upkey.getModifiers[i])), true);
            }

            if (upkey.isRegistered && downkey.isRegistered)
                TurnOn();
            else
                TurnOff();

            sendtotraycheckbox.Checked = extraSettingObject.sendToTray;
            notifyIcon1.Visible = false; //hide tray icon since windows is visible
            loading = false;
        }

        //resets to defaults
        void Reset()
        {
            //set keys
            upkey.SetKey(Keys.Up);
            downkey.SetKey(Keys.Down);
            mutekey.SetKey(Keys.M);

            //set default modifier
            List<string> modList = new List<string>() { "CTRL"};
            upkey.SetModifiers(modList);
            downkey.SetModifiers(modList);

            //minimize to tray 
            sendtotraycheckbox.Checked = false;
            extraSettingObject.SetSendToTray(false);

            SyncVisuals();

            SaveLoad.Save(upkey, downkey, mutekey, extraSettingObject);

        }

        private void HandleHotKey(ref Message m)
        {
            if(m.WParam.ToInt32() == upkey.getID)
            {               
                volControl.VolUp();
            }
            else if(m.WParam.ToInt32() == downkey.getID)
            {             
                volControl.VolDown();
            } 
            else if(m.WParam.ToInt32() == mutekey.getID)
            {
                volControl.Mute();
            }
        }        

        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotKey(ref m);
            base.DefWndProc(ref m);
        }
       
        
        private void turnonbutton_Click_1(object sender, EventArgs e)
        {            
            TurnOn();
        }

        private void turnoffbutton_Click_1(object sender, EventArgs e)
        {
            TurnOff();
        }

        void TurnOn()
        {
            upkey.Register();
            downkey.Register();
            mutekey.Register();
            turnonbutton.BackColor = Color.Green;
            turnoffbutton.BackColor = Color.LightGray;
        }        
        
        void TurnOff()
        {
            upkey.Unregister();
            downkey.Unregister();
            mutekey.Unregister();
            turnonbutton.BackColor = Color.LightGray;
            turnoffbutton.BackColor = Color.Red;
        }


        //Modifier Selection
        private void modselectbox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (upkey == null || downkey == null || loading)
                return;

            //convert to List
            List<string> convertedList = new List<string>();
            foreach (string item in modselectbox.SelectedItems)
                convertedList.Add(item);

            //send list to keyhandlers
            upkey.SetModifiers(convertedList);
            downkey.SetModifiers(convertedList);
            mutekey.SetModifiers(convertedList);          

      
        }

        //*******************
        //Key Selection
        private void volumeupkey_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (upkey == null)
                return;

            upkey.SetKey(volumeupkey.Items[volumeupkey.SelectedIndex].ToString());                    
        }

        private void volumedownkey_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (downkey == null)
                return;
                        
            downkey.SetKey(volumedownkey.Items[volumedownkey.SelectedIndex].ToString());
        }

        private void mutecombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mutekey == null)
                return;

            mutekey.SetKey(mutecombobox.Items[mutecombobox.SelectedIndex].ToString());
        }

        //click on apply button
        private void applybutton_Click(object sender, EventArgs e)
        {
            //deselect this control so the up and down arrows don't cause a reselection
            this.ActiveControl = null;

            upkey.ResetRegistration();
            downkey.ResetRegistration();
            mutekey.ResetRegistration();

            SaveLoad.Save(upkey, downkey, mutekey, extraSettingObject);
        }

        //click on reset button
        private void resetbutton_Click(object sender, EventArgs e)
        {
            Reset();
        }

        //click on website
        private void label1_Click(object sender, EventArgs e)
        {

            ProcessStartInfo sInfo = new ProcessStartInfo("http://zotnip.com/");
            Process.Start(sInfo);
        }

        //****************
        //minimize to tray
        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (!sendtotraycheckbox.Checked)
                return;

            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(250);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                //notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void sendtotraycheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!sendtotraycheckbox.Checked)                
                notifyIcon1.Visible = false;

            extraSettingObject.SetSendToTray(sendtotraycheckbox.Checked);
        }

        //click on donate button
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://zotnip.com/donate");
            Process.Start(sInfo);
        }

       
    }
}
