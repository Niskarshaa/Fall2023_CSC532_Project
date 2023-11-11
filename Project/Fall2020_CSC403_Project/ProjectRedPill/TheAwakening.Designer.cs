using System;
using System.Drawing;
using System.Windows.Forms;

namespace Fall2020_CSC403_Project.ProjectRedPill
{
    partial class TheAwakening
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
            pictureBox1 = new PictureBox();
            label1 = new Label();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.flicker_gif;
            pictureBox1.Location = new Point(652, 349);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(700, 415);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.Lime;
            label1.Location = new Point(358, 164);
            label1.MaximumSize = new Size(700, 50);
            label1.MinimumSize = new Size(700, 50);
            label1.Name = "label1";
            label1.Size = new Size(700, 50);
            label1.TabIndex = 1;
            // 
            // button1
            // 
            button1.BackColor = Color.Black;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.LightGray;
            button1.Location = new Point(1026, 217);
            button1.Name = "button1";
            button1.Size = new Size(32, 27);
            button1.TabIndex = 2;
            button1.Text = "V";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click_1;
            // 
            // FormSequenceOne
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(1800, 950);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            MaximumSize = new Size(1818, 997);
            MinimumSize = new Size(1818, 997);
            Name = "FormSequenceOne";
            Text = "Sequence One";
            WindowState = FormWindowState.Maximized;
            Load += TheAwakening_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private async void button1_Click(object sender, EventArgs e)
        {
            label1.Enabled = false;

            await TypeWriterEffect("This is some text to be `typed`...", label1, 100);

            label1.Enabled = true;
        }

        /*private async void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;

            await TypeWriterEffect("Look mom, we're running at the same time!!!", label2, 200);

            button2.Enabled = true;
        }*/


        private PictureBox pictureBox1;
        private Label label1;
        private Button button1;
    }
}