namespace VolumeHotKeyApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sendtotraycheckbox = new System.Windows.Forms.CheckBox();
            this.resetbutton = new System.Windows.Forms.Button();
            this.applybutton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.volumedownkey = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.volupkeyLabel = new System.Windows.Forms.Label();
            this.volumeupkey = new System.Windows.Forms.ComboBox();
            this.modifiersBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.modselectbox = new System.Windows.Forms.ListBox();
            this.turnoffbutton = new System.Windows.Forms.Button();
            this.turnonbutton = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.mutecombobox = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.modifiersBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label1.Location = new System.Drawing.Point(310, 352);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "by Mike Pinto - www.zotnip.com";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sendtotraycheckbox);
            this.panel1.Controls.Add(this.resetbutton);
            this.panel1.Controls.Add(this.applybutton);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.modifiersBox);
            this.panel1.Controls.Add(this.turnoffbutton);
            this.panel1.Controls.Add(this.turnonbutton);
            this.panel1.Location = new System.Drawing.Point(14, 15);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(526, 300);
            this.panel1.TabIndex = 9;
            // 
            // sendtotraycheckbox
            // 
            this.sendtotraycheckbox.AutoSize = true;
            this.sendtotraycheckbox.Location = new System.Drawing.Point(303, 24);
            this.sendtotraycheckbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sendtotraycheckbox.Name = "sendtotraycheckbox";
            this.sendtotraycheckbox.Size = new System.Drawing.Size(205, 24);
            this.sendtotraycheckbox.TabIndex = 15;
            this.sendtotraycheckbox.Text = "on minimize send to tray";
            this.sendtotraycheckbox.UseVisualStyleBackColor = true;
            this.sendtotraycheckbox.CheckedChanged += new System.EventHandler(this.sendtotraycheckbox_CheckedChanged);
            // 
            // resetbutton
            // 
            this.resetbutton.Location = new System.Drawing.Point(313, 257);
            this.resetbutton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.resetbutton.Name = "resetbutton";
            this.resetbutton.Size = new System.Drawing.Size(84, 39);
            this.resetbutton.TabIndex = 14;
            this.resetbutton.Text = "Reset";
            this.resetbutton.UseVisualStyleBackColor = true;
            this.resetbutton.Click += new System.EventHandler(this.resetbutton_Click);
            // 
            // applybutton
            // 
            this.applybutton.Location = new System.Drawing.Point(404, 257);
            this.applybutton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.applybutton.Name = "applybutton";
            this.applybutton.Size = new System.Drawing.Size(104, 39);
            this.applybutton.TabIndex = 13;
            this.applybutton.Text = "Apply";
            this.applybutton.UseVisualStyleBackColor = true;
            this.applybutton.Click += new System.EventHandler(this.applybutton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mutecombobox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.volumedownkey);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.volupkeyLabel);
            this.groupBox1.Controls.Add(this.volumeupkey);
            this.groupBox1.Location = new System.Drawing.Point(238, 68);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(270, 172);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keys";
            // 
            // volumedownkey
            // 
            this.volumedownkey.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.volumedownkey.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.volumedownkey.FormattingEnabled = true;
            this.volumedownkey.Location = new System.Drawing.Point(127, 62);
            this.volumedownkey.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.volumedownkey.Name = "volumedownkey";
            this.volumedownkey.Size = new System.Drawing.Size(136, 28);
            this.volumedownkey.TabIndex = 10;
            this.volumedownkey.SelectedIndexChanged += new System.EventHandler(this.volumedownkey_SelectedIndexChanged_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Volume Down";
            // 
            // volupkeyLabel
            // 
            this.volupkeyLabel.AutoSize = true;
            this.volupkeyLabel.Location = new System.Drawing.Point(14, 30);
            this.volupkeyLabel.Name = "volupkeyLabel";
            this.volupkeyLabel.Size = new System.Drawing.Size(88, 20);
            this.volupkeyLabel.TabIndex = 8;
            this.volupkeyLabel.Text = "Volume Up";
            // 
            // volumeupkey
            // 
            this.volumeupkey.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.volumeupkey.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.volumeupkey.FormattingEnabled = true;
            this.volumeupkey.Location = new System.Drawing.Point(127, 26);
            this.volumeupkey.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.volumeupkey.Name = "volumeupkey";
            this.volumeupkey.Size = new System.Drawing.Size(136, 28);
            this.volumeupkey.TabIndex = 7;
            this.volumeupkey.SelectedIndexChanged += new System.EventHandler(this.volumeupkey_SelectedIndexChanged_1);
            // 
            // modifiersBox
            // 
            this.modifiersBox.Controls.Add(this.label2);
            this.modifiersBox.Controls.Add(this.modselectbox);
            this.modifiersBox.Location = new System.Drawing.Point(14, 68);
            this.modifiersBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.modifiersBox.Name = "modifiersBox";
            this.modifiersBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.modifiersBox.Size = new System.Drawing.Size(218, 172);
            this.modifiersBox.TabIndex = 11;
            this.modifiersBox.TabStop = false;
            this.modifiersBox.Text = "Modifiers";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "ctrl click to multi select";
            // 
            // modselectbox
            // 
            this.modselectbox.FormattingEnabled = true;
            this.modselectbox.ItemHeight = 20;
            this.modselectbox.Items.AddRange(new object[] {
            "NONE",
            "CTRL",
            "ALT",
            "SHIFT",
            "Windows Key"});
            this.modselectbox.Location = new System.Drawing.Point(8, 26);
            this.modselectbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.modselectbox.Name = "modselectbox";
            this.modselectbox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.modselectbox.Size = new System.Drawing.Size(134, 104);
            this.modselectbox.TabIndex = 4;
            this.modselectbox.SelectedIndexChanged += new System.EventHandler(this.modselectbox_SelectedIndexChanged_1);
            // 
            // turnoffbutton
            // 
            this.turnoffbutton.BackColor = System.Drawing.Color.Red;
            this.turnoffbutton.ForeColor = System.Drawing.Color.Black;
            this.turnoffbutton.Location = new System.Drawing.Point(136, 12);
            this.turnoffbutton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.turnoffbutton.Name = "turnoffbutton";
            this.turnoffbutton.Size = new System.Drawing.Size(96, 38);
            this.turnoffbutton.TabIndex = 10;
            this.turnoffbutton.Text = "Turn Off";
            this.turnoffbutton.UseVisualStyleBackColor = false;
            this.turnoffbutton.Click += new System.EventHandler(this.turnoffbutton_Click_1);
            // 
            // turnonbutton
            // 
            this.turnonbutton.Location = new System.Drawing.Point(14, 12);
            this.turnonbutton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.turnonbutton.Name = "turnonbutton";
            this.turnonbutton.Size = new System.Drawing.Size(98, 38);
            this.turnonbutton.TabIndex = 9;
            this.turnonbutton.Text = "Turn On";
            this.turnonbutton.UseVisualStyleBackColor = true;
            this.turnonbutton.Click += new System.EventHandler(this.turnonbutton_Click_1);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "Volume Hotkey app still running!";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Volume Hotkey";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::VolumeHotKeyApp.Properties.Resources.btn_donate_pp_142x27;
            this.pictureBox1.Location = new System.Drawing.Point(15, 330);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(142, 27);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 306);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Find this useful?";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Mute";
            // 
            // mutecombobox
            // 
            this.mutecombobox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.mutecombobox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.mutecombobox.FormattingEnabled = true;
            this.mutecombobox.Location = new System.Drawing.Point(127, 98);
            this.mutecombobox.Name = "mutecombobox";
            this.mutecombobox.Size = new System.Drawing.Size(136, 28);
            this.mutecombobox.TabIndex = 12;
            this.mutecombobox.SelectedIndexChanged += new System.EventHandler(this.mutecombobox_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 382);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "System Volume Control Hotkey Util";
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.modifiersBox.ResumeLayout(false);
            this.modifiersBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox volumedownkey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label volupkeyLabel;
        private System.Windows.Forms.ComboBox volumeupkey;
        private System.Windows.Forms.GroupBox modifiersBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox modselectbox;
        private System.Windows.Forms.Button turnoffbutton;
        private System.Windows.Forms.Button turnonbutton;
        private System.Windows.Forms.Button applybutton;
        private System.Windows.Forms.Button resetbutton;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.CheckBox sendtotraycheckbox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox mutecombobox;
        private System.Windows.Forms.Label label5;
    }
}

