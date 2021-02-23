namespace CSharpAddIn
{
    partial class FrmSpotLight
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
            this.label3 = new System.Windows.Forms.Label();
            this.btnRow = new System.Windows.Forms.Button();
            this.btnCol = new System.Windows.Forms.Button();
            this.nudTransparent = new System.Windows.Forms.NumericUpDown();
            this.rdGraMask = new System.Windows.Forms.RadioButton();
            this.rdShapes = new System.Windows.Forms.RadioButton();
            this.rdGraLine = new System.Windows.Forms.RadioButton();
            this.ckLineMask = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransparent)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 102);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "行标颜色值";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 150);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "列标颜色值i";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 198);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "颜色透明度";
            // 
            // btnRow
            // 
            this.btnRow.Location = new System.Drawing.Point(186, 95);
            this.btnRow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRow.Name = "btnRow";
            this.btnRow.Size = new System.Drawing.Size(129, 35);
            this.btnRow.TabIndex = 3;
            this.btnRow.Text = "颜色设置";
            this.btnRow.UseVisualStyleBackColor = true;
            this.btnRow.Click += new System.EventHandler(this.btnRow_Click);
            // 
            // btnCol
            // 
            this.btnCol.Location = new System.Drawing.Point(186, 143);
            this.btnCol.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCol.Name = "btnCol";
            this.btnCol.Size = new System.Drawing.Size(129, 35);
            this.btnCol.TabIndex = 4;
            this.btnCol.Text = "颜色设置";
            this.btnCol.UseVisualStyleBackColor = true;
            this.btnCol.Click += new System.EventHandler(this.btnCol_Click);
            // 
            // nudTransparent
            // 
            this.nudTransparent.Location = new System.Drawing.Point(186, 193);
            this.nudTransparent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nudTransparent.Name = "nudTransparent";
            this.nudTransparent.Size = new System.Drawing.Size(129, 28);
            this.nudTransparent.TabIndex = 5;
            this.nudTransparent.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudTransparent.ValueChanged += new System.EventHandler(this.nudTransparent_ValueChanged);
            // 
            // rdGraMask
            // 
            this.rdGraMask.AutoSize = true;
            this.rdGraMask.Location = new System.Drawing.Point(235, 25);
            this.rdGraMask.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdGraMask.Name = "rdGraMask";
            this.rdGraMask.Size = new System.Drawing.Size(96, 22);
            this.rdGraMask.TabIndex = 6;
            this.rdGraMask.Text = "GraMask";
            this.rdGraMask.UseVisualStyleBackColor = true;
            // 
            // rdShapes
            // 
            this.rdShapes.AutoSize = true;
            this.rdShapes.Location = new System.Drawing.Point(143, 25);
            this.rdShapes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdShapes.Name = "rdShapes";
            this.rdShapes.Size = new System.Drawing.Size(87, 22);
            this.rdShapes.TabIndex = 7;
            this.rdShapes.Text = "Shapes";
            this.rdShapes.UseVisualStyleBackColor = true;
            // 
            // rdGraLine
            // 
            this.rdGraLine.AutoSize = true;
            this.rdGraLine.Checked = true;
            this.rdGraLine.Location = new System.Drawing.Point(42, 25);
            this.rdGraLine.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdGraLine.Name = "rdGraLine";
            this.rdGraLine.Size = new System.Drawing.Size(96, 22);
            this.rdGraLine.TabIndex = 8;
            this.rdGraLine.TabStop = true;
            this.rdGraLine.Text = "GraLine";
            this.rdGraLine.UseVisualStyleBackColor = true;
            // 
            // ckLineMask
            // 
            this.ckLineMask.AutoSize = true;
            this.ckLineMask.Checked = true;
            this.ckLineMask.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckLineMask.Location = new System.Drawing.Point(42, 59);
            this.ckLineMask.Name = "ckLineMask";
            this.ckLineMask.Size = new System.Drawing.Size(106, 22);
            this.ckLineMask.TabIndex = 9;
            this.ckLineMask.Text = "LineMask";
            this.ckLineMask.UseVisualStyleBackColor = true;
            // 
            // FrmSpotLight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 247);
            this.Controls.Add(this.ckLineMask);
            this.Controls.Add(this.rdGraLine);
            this.Controls.Add(this.rdShapes);
            this.Controls.Add(this.rdGraMask);
            this.Controls.Add(this.nudTransparent);
            this.Controls.Add(this.btnCol);
            this.Controls.Add(this.btnRow);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSpotLight";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "聚光灯设置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmSpotLight_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.nudTransparent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRow;
        private System.Windows.Forms.Button btnCol;
        private System.Windows.Forms.NumericUpDown nudTransparent;
        private System.Windows.Forms.RadioButton rdGraMask;
        private System.Windows.Forms.RadioButton rdShapes;
        private System.Windows.Forms.RadioButton rdGraLine;
        private System.Windows.Forms.CheckBox ckLineMask;
    }
}