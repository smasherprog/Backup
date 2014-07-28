namespace BackUp_Presentation.Volume
{
    partial class Volume_Mapper
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
            if(disposing && (components != null))
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.conSaveCred = new System.Windows.Forms.CheckBox();
            this.conPromptForCred = new System.Windows.Forms.CheckBox();
            this.conForce = new System.Windows.Forms.CheckBox();
            this.conPersistant = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDrive = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.conStatusBar = new System.Windows.Forms.StatusBar();
            this.conStatusBar_Panel_Action = new System.Windows.Forms.StatusBarPanel();
            this.conMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.conMenu_Dialog1 = new System.Windows.Forms.MenuItem();
            this.conMenu_Dialog2 = new System.Windows.Forms.MenuItem();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.conStatusBar_Panel_Action)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.conSaveCred);
            this.groupBox1.Controls.Add(this.conPromptForCred);
            this.groupBox1.Controls.Add(this.conForce);
            this.groupBox1.Controls.Add(this.conPersistant);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtDrive);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Controls.Add(this.txtAddress);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Location = new System.Drawing.Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 176);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map Drive Settings";
            // 
            // conSaveCred
            // 
            this.conSaveCred.Location = new System.Drawing.Point(264, 144);
            this.conSaveCred.Name = "conSaveCred";
            this.conSaveCred.Size = new System.Drawing.Size(136, 24);
            this.conSaveCred.TabIndex = 29;
            this.conSaveCred.Text = "Save credentials";
            // 
            // conPromptForCred
            // 
            this.conPromptForCred.Location = new System.Drawing.Point(112, 144);
            this.conPromptForCred.Name = "conPromptForCred";
            this.conPromptForCred.Size = new System.Drawing.Size(136, 24);
            this.conPromptForCred.TabIndex = 28;
            this.conPromptForCred.Text = "Prompt for credentials";
            // 
            // conForce
            // 
            this.conForce.Location = new System.Drawing.Point(264, 120);
            this.conForce.Name = "conForce";
            this.conForce.Size = new System.Drawing.Size(136, 24);
            this.conForce.TabIndex = 25;
            this.conForce.Text = "Force dis/connetion";
            // 
            // conPersistant
            // 
            this.conPersistant.Checked = true;
            this.conPersistant.CheckState = System.Windows.Forms.CheckState.Checked;
            this.conPersistant.Location = new System.Drawing.Point(112, 120);
            this.conPersistant.Name = "conPersistant";
            this.conPersistant.Size = new System.Drawing.Size(136, 24);
            this.conPersistant.TabIndex = 24;
            this.conPersistant.Text = "Persistant connection";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 21);
            this.label5.TabIndex = 23;
            this.label5.Text = "Options:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 21);
            this.label4.TabIndex = 21;
            this.label4.Text = "Password:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 21);
            this.label3.TabIndex = 20;
            this.label3.Text = "Username:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 21);
            this.label2.TabIndex = 19;
            this.label2.Text = "Map to Drive:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 21);
            this.label1.TabIndex = 18;
            this.label1.Text = "Share Address:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDrive
            // 
            this.txtDrive.Location = new System.Drawing.Point(112, 48);
            this.txtDrive.Name = "txtDrive";
            this.txtDrive.Size = new System.Drawing.Size(176, 21);
            this.txtDrive.TabIndex = 17;
            this.txtDrive.Text = "Z:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(112, 96);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(176, 21);
            this.txtPassword.TabIndex = 16;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(112, 72);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(176, 21);
            this.txtUsername.TabIndex = 15;
            // 
            // txtAddress
            // 
            this.txtAddress.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtAddress.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtAddress.Location = new System.Drawing.Point(112, 24);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(176, 21);
            this.txtAddress.TabIndex = 14;
            this.txtAddress.Text = "\\\\ComputerName\\Share";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(296, 24);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 24);
            this.button3.TabIndex = 12;
            this.button3.Text = "Map Drive";
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // conStatusBar
            // 
            this.conStatusBar.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.conStatusBar.Location = new System.Drawing.Point(0, 179);
            this.conStatusBar.Name = "conStatusBar";
            this.conStatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.conStatusBar_Panel_Action});
            this.conStatusBar.ShowPanels = true;
            this.conStatusBar.Size = new System.Drawing.Size(440, 24);
            this.conStatusBar.SizingGrip = false;
            this.conStatusBar.TabIndex = 14;
            // 
            // conStatusBar_Panel_Action
            // 
            this.conStatusBar_Panel_Action.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.conStatusBar_Panel_Action.Name = "conStatusBar_Panel_Action";
            this.conStatusBar_Panel_Action.Text = "...";
            this.conStatusBar_Panel_Action.Width = 440;
            // 
            // conMenu
            // 
            this.conMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2});
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.conMenu_Dialog1,
            this.conMenu_Dialog2});
            this.menuItem2.Text = "Dialogs";
            // 
            // conMenu_Dialog1
            // 
            this.conMenu_Dialog1.Index = 0;
            this.conMenu_Dialog1.Text = "Show \'Drive connection\'";
            this.conMenu_Dialog1.Click += new System.EventHandler(this.conMenu_Dialog1_Click);
            // 
            // conMenu_Dialog2
            // 
            this.conMenu_Dialog2.Index = 1;
            this.conMenu_Dialog2.Text = "Show \'Drive Disconnection\'";
            this.conMenu_Dialog2.Click += new System.EventHandler(this.conMenu_Dialog2_Click);
            // 
            // Volume_Mapper
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(440, 203);
            this.Controls.Add(this.conStatusBar);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Menu = this.conMenu;
            this.Name = "Volume_Mapper";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Drive Mapper";
            this.Load += new System.EventHandler(this.frmTest_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.conStatusBar_Panel_Action)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDrive;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtAddress;

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.StatusBar conStatusBar;
        private System.Windows.Forms.StatusBarPanel conStatusBar_Panel_Action;
        private System.Windows.Forms.CheckBox conForce;
        private System.Windows.Forms.CheckBox conPersistant;
        private System.Windows.Forms.MainMenu conMenu;
        private System.Windows.Forms.CheckBox conPromptForCred;
        private System.Windows.Forms.CheckBox conSaveCred;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem conMenu_Dialog1;
        private System.Windows.Forms.MenuItem conMenu_Dialog2;


    }
}