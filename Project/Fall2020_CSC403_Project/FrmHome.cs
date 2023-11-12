using Fall2020_CSC403_Project.ProjectRedPill;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fall2020_CSC403_Project
{
    public partial class FrmHome : Form
    {
        private List<Image> images = new List<Image>();
        private List<string> label1Text = new List<string>();
        private List<string> label2Text = new List<string>();
        private int currentImageIndex = 0;
        public static FrmLevel gameplayForm = null;
        public static TheAwakening frmAwakening = null;
        public FrmHome()
        {
            InitializeComponent();
        }

        private void FrmHome_Load(object sender, EventArgs e)
        {
            // Set the background color of the form to a specific color
            //this.BackColor = Color.YellowGreen;
            label1.Font = new Font(label1.Font.FontFamily, 16, label1.Font.Style); // Replace 16 with your desired font size
            label2.Font = new Font(label1.Font.FontFamily, 12, label1.Font.Style); // Replace 16 with your desired font size


            images.Add(Properties.Resources.player); // Load images from resources
            images.Add(Properties.Resources.enemy_koolaid);
            images.Add(Properties.Resources.enemy_cheetos);
            images.Add(Properties.Resources.enemy_poisonpacket);

            // Add more images as needed

            label1Text.Add("Main Player");
            label1Text.Add("enemy_koolaid");
            label1Text.Add("enemy_cheetos");
            label1Text.Add("enemy_poison");
            // Add more text as needed

            label2Text.Add("Player Character (Your Avatar): \n A young hero with the ability to harness the power of Air. \n You are known for your agility and problem-solving skills.");
            label2Text.Add("Koolaid (Enemy - representing the corrupted Water Element): \n Koolaid is a devious and manipulative enchantress who \n wants to flood Eldorania with her dark Water magic.");
            label2Text.Add("Cheetos (Enemy - representing the corrupted Fire Element):\n Cheetos is a cunning and power-hungry sorcerer who \n seeks to unleash the destructive force of Fire to rule the realm.");
            label2Text.Add("Poisonpacket (Enemy - representing the corrupted Earth Element): \n Poisonpacket is a treacherous rogue who plans \n to spread chaos by infecting the land with poisonous Earth.");
            // Add more text as needed
        }

        public void playBtn_Click(object sender, EventArgs e)
        {
            string selectedTheme = themeSelect.SelectedItem.ToString();
            if (selectedTheme == "Classic Theme")
            {
                gameplayForm = new FrmLevel();
                gameplayForm.Show();
                this.Hide();
            }
            if (selectedTheme == "New Theme")
            {
                gameplayForm = new FrmLevel();
                gameplayForm.Show();
                gameplayForm.applyTheme1();
                this.Hide();
            }
            if (selectedTheme == "Invisible Theme")
            {
                gameplayForm = new FrmLevel();
                gameplayForm.Show();
                gameplayForm.applyTheme2();
                this.Hide();
            }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FrmHome_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                    break;
                case Keys.S:
                    FrmLevel gameplayForm = new FrmLevel();
                    gameplayForm.Show();
                    this.Hide();
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (currentImageIndex < images.Count)
            {
                pictureBox1.Image = images[currentImageIndex];
                label1.Text = label1Text[currentImageIndex];
                label2.Text = label2Text[currentImageIndex];
                currentImageIndex++;

                if (currentImageIndex >= images.Count)
                {
                    currentImageIndex = 0; // Reset the image index
                    timer1.Enabled = true; // Start the timer
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmHelp frmHelp = new FrmHelp(); // Create an instance of the instruction form
            frmHelp.ShowDialog(); // Show the instruction form as a modal dialog

            // Environment.Exit(0);
        }

        private void themeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
