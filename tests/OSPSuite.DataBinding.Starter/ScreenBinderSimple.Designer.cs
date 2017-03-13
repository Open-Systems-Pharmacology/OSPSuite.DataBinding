namespace OSPSuite.DataBinding.Starter
{
    partial class ScreenBinderSimple
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
                _screenBinder.Dispose();

            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
         this.components = new System.ComponentModel.Container();
         this.grpCaption = new System.Windows.Forms.GroupBox();
         this.label2 = new System.Windows.Forms.Label();
         this.cbComboBox = new System.Windows.Forms.ComboBox();
         this.btnDump = new System.Windows.Forms.Button();
         this.cmdReset = new System.Windows.Forms.Button();
         this.tbAnotherFirstName = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.btnApplyLocalChange = new System.Windows.Forms.Button();
         this.tbValue = new System.Windows.Forms.TextBox();
         this.label7 = new System.Windows.Forms.Label();
         this.rtbDump = new System.Windows.Forms.RichTextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.tbFirstName = new System.Windows.Forms.TextBox();
         this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
         this.grpCaption.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         this.SuspendLayout();
         // 
         // grpCaption
         // 
         this.grpCaption.Controls.Add(this.label2);
         this.grpCaption.Controls.Add(this.cbComboBox);
         this.grpCaption.Controls.Add(this.btnDump);
         this.grpCaption.Controls.Add(this.cmdReset);
         this.grpCaption.Controls.Add(this.tbAnotherFirstName);
         this.grpCaption.Controls.Add(this.label3);
         this.grpCaption.Controls.Add(this.btnApplyLocalChange);
         this.grpCaption.Controls.Add(this.tbValue);
         this.grpCaption.Controls.Add(this.label7);
         this.grpCaption.Controls.Add(this.rtbDump);
         this.grpCaption.Controls.Add(this.label1);
         this.grpCaption.Controls.Add(this.tbFirstName);
         this.grpCaption.Location = new System.Drawing.Point(3, 3);
         this.grpCaption.Name = "grpCaption";
         this.grpCaption.Size = new System.Drawing.Size(349, 411);
         this.grpCaption.TabIndex = 7;
         this.grpCaption.TabStop = false;
         this.grpCaption.Text = "Data Binding Mode = ";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(8, 80);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(151, 13);
         this.label2.TabIndex = 24;
         this.label2.Text = "And that\'s a bound combo box";
         // 
         // cbComboBox
         // 
         this.cbComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cbComboBox.FormattingEnabled = true;
         this.cbComboBox.Location = new System.Drawing.Point(200, 72);
         this.cbComboBox.Name = "cbComboBox";
         this.cbComboBox.Size = new System.Drawing.Size(121, 21);
         this.cbComboBox.TabIndex = 23;
         // 
         // btnDump
         // 
         this.btnDump.Location = new System.Drawing.Point(5, 370);
         this.btnDump.Name = "btnDump";
         this.btnDump.Size = new System.Drawing.Size(122, 23);
         this.btnDump.TabIndex = 22;
         this.btnDump.Text = "Dump Source Values";
         this.btnDump.UseVisualStyleBackColor = true;
         this.btnDump.Click += new System.EventHandler(this.btnDumpClick);
         // 
         // cmdReset
         // 
         this.cmdReset.Location = new System.Drawing.Point(258, 370);
         this.cmdReset.Name = "cmdReset";
         this.cmdReset.Size = new System.Drawing.Size(75, 23);
         this.cmdReset.TabIndex = 21;
         this.cmdReset.Text = "Reset";
         this.cmdReset.UseVisualStyleBackColor = true;
         // 
         // tbAnotherFirstName
         // 
         this.tbAnotherFirstName.Location = new System.Drawing.Point(235, 37);
         this.tbAnotherFirstName.Name = "tbAnotherFirstName";
         this.tbAnotherFirstName.Size = new System.Drawing.Size(86, 20);
         this.tbAnotherFirstName.TabIndex = 19;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(2, 40);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(216, 13);
         this.label3.TabIndex = 18;
         this.label3.Text = "Another bound text box to the same property";
         // 
         // btnApplyLocalChange
         // 
         this.btnApplyLocalChange.Location = new System.Drawing.Point(247, 338);
         this.btnApplyLocalChange.Name = "btnApplyLocalChange";
         this.btnApplyLocalChange.Size = new System.Drawing.Size(86, 23);
         this.btnApplyLocalChange.TabIndex = 17;
         this.btnApplyLocalChange.Text = "Change Value!";
         this.btnApplyLocalChange.UseVisualStyleBackColor = true;
         // 
         // tbValue
         // 
         this.tbValue.Location = new System.Drawing.Point(160, 338);
         this.tbValue.Name = "tbValue";
         this.tbValue.Size = new System.Drawing.Size(78, 20);
         this.tbValue.TabIndex = 16;
         this.tbValue.Text = "toto";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(2, 343);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(155, 13);
         this.label7.TabIndex = 15;
         this.label7.Text = "Change Source Value by hand:\r\n";
         // 
         // rtbDump
         // 
         this.rtbDump.Location = new System.Drawing.Point(0, 150);
         this.rtbDump.Name = "rtbDump";
         this.rtbDump.Size = new System.Drawing.Size(327, 177);
         this.rtbDump.TabIndex = 14;
         this.rtbDump.Text = "";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(5, 16);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(122, 13);
         this.label1.TabIndex = 13;
         this.label1.Text = "This is a bound text box:";
         // 
         // tbFirstName
         // 
         this.tbFirstName.Location = new System.Drawing.Point(147, 13);
         this.tbFirstName.Name = "tbFirstName";
         this.tbFirstName.Size = new System.Drawing.Size(174, 20);
         this.tbFirstName.TabIndex = 12;
         // 
         // errorProvider
         // 
         this.errorProvider.ContainerControl = this;
         
         // 
         // ScreenBinderSimple
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.grpCaption);
         this.Name = "ScreenBinderSimple";
         this.Size = new System.Drawing.Size(361, 422);
         this.grpCaption.ResumeLayout(false);
         this.grpCaption.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCaption;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbComboBox;
        private System.Windows.Forms.Button btnDump;
        private System.Windows.Forms.Button cmdReset;
        private System.Windows.Forms.TextBox tbAnotherFirstName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnApplyLocalChange;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox rtbDump;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbFirstName;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
