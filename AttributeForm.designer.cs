
namespace EX3
{
    partial class AttributeForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.layerNameBox = new System.Windows.Forms.TextBox();
            this.shapeTypeBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "图层名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "要素几何类型：";
            // 
            // layerNameBox
            // 
            this.layerNameBox.Location = new System.Drawing.Point(216, 65);
            this.layerNameBox.Name = "layerNameBox";
            this.layerNameBox.Size = new System.Drawing.Size(431, 31);
            this.layerNameBox.TabIndex = 2;
            // 
            // shapeTypeBox
            // 
            this.shapeTypeBox.Location = new System.Drawing.Point(216, 159);
            this.shapeTypeBox.Name = "shapeTypeBox";
            this.shapeTypeBox.Size = new System.Drawing.Size(431, 31);
            this.shapeTypeBox.TabIndex = 3;
            // 
            // AttributeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 490);
            this.Controls.Add(this.shapeTypeBox);
            this.Controls.Add(this.layerNameBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AttributeForm";
            this.Text = "AttributeForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox layerNameBox;
        private System.Windows.Forms.TextBox shapeTypeBox;
    }
}