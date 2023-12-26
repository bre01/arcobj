
namespace EX3
{
    partial class frmLayerRender
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
            this.renderMethodList = new System.Windows.Forms.ListBox();
            this.renderMethodTab = new System.Windows.Forms.TabControl();
            this.simpleTabPage = new System.Windows.Forms.TabPage();
            this.btnBmp = new System.Windows.Forms.Button();
            this.uniqueValueTabPage = new System.Windows.Forms.TabPage();
            this.classBreaksTabPage = new System.Windows.Forms.TabPage();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.renderMethodTab.SuspendLayout();
            this.simpleTabPage.SuspendLayout();
            this.uniqueValueTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // renderMethodList
            // 
            this.renderMethodList.FormattingEnabled = true;
            this.renderMethodList.ItemHeight = 21;
            this.renderMethodList.Items.AddRange(new object[] {
            "单一符号",
            "唯一值符号",
            "分级符号"});
            this.renderMethodList.Location = new System.Drawing.Point(35, 43);
            this.renderMethodList.Name = "renderMethodList";
            this.renderMethodList.Size = new System.Drawing.Size(161, 172);
            this.renderMethodList.TabIndex = 0;
            this.renderMethodList.SelectedIndexChanged += new System.EventHandler(this.renderMethodList_SelectedIndexChanged);
            // 
            // renderMethodTab
            // 
            this.renderMethodTab.Controls.Add(this.simpleTabPage);
            this.renderMethodTab.Controls.Add(this.uniqueValueTabPage);
            this.renderMethodTab.Controls.Add(this.classBreaksTabPage);
            this.renderMethodTab.Location = new System.Drawing.Point(238, 25);
            this.renderMethodTab.Name = "renderMethodTab";
            this.renderMethodTab.SelectedIndex = 0;
            this.renderMethodTab.Size = new System.Drawing.Size(697, 530);
            this.renderMethodTab.TabIndex = 1;
            this.renderMethodTab.SelectedIndexChanged += new System.EventHandler(this.renderMethodTab_SelectedIndexChanged);
            // 
            // simpleTabPage
            // 
            this.simpleTabPage.Controls.Add(this.btnBmp);
            this.simpleTabPage.Location = new System.Drawing.Point(4, 31);
            this.simpleTabPage.Name = "simpleTabPage";
            this.simpleTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.simpleTabPage.Size = new System.Drawing.Size(689, 495);
            this.simpleTabPage.TabIndex = 0;
            this.simpleTabPage.Text = "单一符号";
            this.simpleTabPage.UseVisualStyleBackColor = true;
            // 
            // btnBmp
            // 
            this.btnBmp.Location = new System.Drawing.Point(269, 167);
            this.btnBmp.Name = "btnBmp";
            this.btnBmp.Size = new System.Drawing.Size(139, 139);
            this.btnBmp.TabIndex = 0;
            this.btnBmp.UseVisualStyleBackColor = true;
            this.btnBmp.Click += new System.EventHandler(this.btnBmp_Click);
            // 
            // uniqueValueTabPage
            // 
            this.uniqueValueTabPage.Controls.Add(this.listView1);
            this.uniqueValueTabPage.Controls.Add(this.comboBox1);
            this.uniqueValueTabPage.Controls.Add(this.label1);
            this.uniqueValueTabPage.Location = new System.Drawing.Point(4, 31);
            this.uniqueValueTabPage.Name = "uniqueValueTabPage";
            this.uniqueValueTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.uniqueValueTabPage.Size = new System.Drawing.Size(689, 495);
            this.uniqueValueTabPage.TabIndex = 1;
            this.uniqueValueTabPage.Text = "唯一值符号";
            this.uniqueValueTabPage.UseVisualStyleBackColor = true;
            // 
            // classBreaksTabPage
            // 
            this.classBreaksTabPage.Location = new System.Drawing.Point(4, 31);
            this.classBreaksTabPage.Name = "classBreaksTabPage";
            this.classBreaksTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.classBreaksTabPage.Size = new System.Drawing.Size(689, 495);
            this.classBreaksTabPage.TabIndex = 2;
            this.classBreaksTabPage.Text = "分级符号";
            this.classBreaksTabPage.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(527, 582);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(123, 49);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "确定";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(656, 582);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(123, 49);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "取消";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(785, 582);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(123, 49);
            this.applyButton.TabIndex = 4;
            this.applyButton.Text = "应用";
            this.applyButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "字段：";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(144, 31);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(264, 29);
            this.comboBox1.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(6, 75);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(677, 414);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // frmLayerRender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 658);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.renderMethodTab);
            this.Controls.Add(this.renderMethodList);
            this.Name = "frmLayerRender";
            this.Text = "frmLayerRender";
            this.renderMethodTab.ResumeLayout(false);
            this.simpleTabPage.ResumeLayout(false);
            this.uniqueValueTabPage.ResumeLayout(false);
            this.uniqueValueTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox renderMethodList;
        private System.Windows.Forms.TabControl renderMethodTab;
        private System.Windows.Forms.TabPage simpleTabPage;
        private System.Windows.Forms.TabPage uniqueValueTabPage;
        private System.Windows.Forms.TabPage classBreaksTabPage;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button btnBmp;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
    }
}