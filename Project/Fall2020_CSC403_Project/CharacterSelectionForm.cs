using Fall2020_CSC403_Project.code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Fall2020_CSC403_Project
{
    public partial class CharacterSelectionForm : Form
    {
        private List<Image> characterImages = new List<Image>();
        private List<Character> characterList = new List<Character>();

        private int currentImageIndex = 0;

        public Character SelectedCharacter { get; private set; }

        public CharacterSelectionForm()
        {
            InitializeComponent();
            LoadCharacterImages();

            this.KeyPreview = true;
            this.KeyDown += CharacterSelectionForm_KeyDown;

            this.Focus();
        }

        private void LoadCharacterImages()
        {
            // Load your character images here
            characterImages.Add(Properties.Resources.player);
            characterImages.Add(Properties.Resources.img2);
            characterImages.Add(Properties.Resources.img1);
            characterImages.Add(Properties.Resources.img31);
            characterImages.Add(Properties.Resources.img41);
            // Add more images as needed
            ChangeImage(currentImageIndex);
        }

        private void ChangeImage(int index)
        {
            if (index >= 0 && index < characterImages.Count)
            {
                pictureBox1.Image = characterImages[index];
            }
        }

        private void CharacterSelectionForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    currentImageIndex = (currentImageIndex - 1 + characterImages.Count) % characterImages.Count;
                    ChangeImage(currentImageIndex);
                    break;

                case Keys.Right:
                    currentImageIndex = (currentImageIndex + 1) % characterImages.Count;
                    ChangeImage(currentImageIndex);
                    break;

                case Keys.Escape:
                    DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                    break;
                case Keys.Q:
                    Program.frmHome.Show();
                    this.Hide();
                    break;
            }
        }

        //private Character GetCharacterFromIndex(int index)
        private Character GetCharacterFromIndex(int index)
        {
            if (index >= 0 && index < characterList.Count)
            {
                return characterList[index];
            }

            // Handle other cases or return null if needed
            return null;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SelectedCharacter = GetCharacterFromIndex(currentImageIndex);
            OpenFrmLevelWithSelectedCharacter();

        }

        private void OpenFrmLevelWithSelectedCharacter()
        {
            SelectedCharacter = GetCharacterFromIndex(currentImageIndex);
            FrmHome.gameplayForm = new FrmLevel(); // Pass the selected character

            FrmBattle.currentImageIndex = currentImageIndex;
            if (currentImageIndex == 0)
            {
                FrmHome.gameplayForm.picPlayer.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.player;
            }
            else if (currentImageIndex == 1)
            {
                FrmHome.gameplayForm.picPlayer.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.img2;
            }
            else if (currentImageIndex == 2) 
            {
                FrmHome.gameplayForm.picPlayer.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.img1;
            }
            else if (currentImageIndex == 3)
            {
                FrmHome.gameplayForm.picPlayer.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.img31;
            }
            else if (currentImageIndex == 4)
            {
                FrmHome.gameplayForm.picPlayer.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.img41;
            }
            FrmHome.gameplayForm.Show();
            this.Close();
        }
    }
}
