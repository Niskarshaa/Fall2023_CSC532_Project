using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Fall2020_CSC403_Project.code;
using Fall2020_CSC403_Project.item_system.interfaces;

namespace Fall2020_CSC403_Project.item_system
{

    public class WallBoom : AbstractItem, IItem
    {
        public string itemName => "Wall Boom";
        private SoundPlayer effectEventPlayer;
        


        public WallBoom(FrmLevel frmLevel, int numItems, float locXatLvl, float locYatLvl)
        {
            // Non unique methods(implemented the same for all items) are called from the AbsractItem class
            InitializeComponent(frmLevel, numItems, global::Fall2020_CSC403_Project.Properties.Resources.RandomPotion, locXatLvl, locYatLvl, "RandomItem", 50, 50, 18);

        }

        // Unique Implementations for each Item are required by the interface.
        //public void ExecuteEffect(FrmLevel frmLevel, Player player, Enemy enemy)
        public void ExecuteEffect(FrmLevel frmLevel)
        {
            /* ExecuteEffect: Triggers on collision of player with item object
             * EFFECT #1 : WallBoom ...maybe implement as separate item -- TODO: move to itemID 2 and make this a separate effect */

            EffectEvent(frmLevel, 18);

            // Remove all walls from map (or make them all non enabled or something
            // also execute a battle_screen type popup with a boom that disappears shortly after
            for (int w = 0; w < 13; w++)
            {
                // Get a handle for each wall picture
                PictureBox pic = frmLevel.Controls.Find("picWall" + w.ToString(), true)[0] as PictureBox;
                
                // Remove pictures from being seen
                pic.Visible = false;
                // Remove colliders ... each wall in walls that was constructed in frmLevel has a collider. if we null the class contained in wall, all is removed
                frmLevel.walls[w] = null; // In order to null this, checks for the walls need to have try catch in case the walls no longer exist!
                
            }

            RemoveItemFromMap(this);
            
        }
        

        public async void EffectEvent(FrmLevel frmLevel, int tabIndex) 
        {

            effectEventPlayer = new SoundPlayer(global::Fall2020_CSC403_Project.Properties.Resources.wallBoomSound1);
            effectEventPlayer.Play();

            picEffectEvent = new System.Windows.Forms.PictureBox();

            ((System.ComponentModel.ISupportInitialize)(picEffectEvent)).BeginInit();
            frmLevel.SuspendLayout();

            this.picEffectEvent.Image = global::Fall2020_CSC403_Project.Properties.Resources.wallBoom1;
            this.picEffectEvent.Location = new System.Drawing.Point(150,50);
            this.picEffectEvent.Name = "WallBoom";
            this.picEffectEvent.Size = new System.Drawing.Size(650,650);
            this.picEffectEvent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEffectEvent.TabIndex = tabIndex;
            this.picEffectEvent.TabStop = false;

            frmLevel.Controls.Add(picEffectEvent);
            ((System.ComponentModel.ISupportInitialize)(picEffectEvent)).EndInit();
            frmLevel.ResumeLayout(false);

            // Give the gif time to play
            await Task.Delay(3000);

            // Remove the image after the gif plays for however many seconds
            picEffectEvent.Visible = false;

        }

    }

    public class CheetoBoom : AbstractItem, IItem
    {
        public string itemName => "Wall Boom";
        private SoundPlayer effectEventPlayer;


        public CheetoBoom(FrmLevel frmLevel, int numItems, float locXatLvl, float locYatLvl)
        {
            InitializeComponent(frmLevel, numItems, global::Fall2020_CSC403_Project.Properties.Resources.RandomPotion, locXatLvl, locYatLvl, "RandomItem", 50, 50, 18);
        }


        public void ExecuteEffect(FrmLevel frmLevel)
        {
            EffectEvent(frmLevel, global::Fall2020_CSC403_Project.Properties.Resources.wallBoom1, 700, 400, "CheetoBoom", 300, 300, 18);

            // Remove enemy cheeto from map due to this CheetoBoom effect of Random Potion
            
            frmLevel.enemyCheeto = null;
            frmLevel.picEnemyCheeto.Visible =  false;

            RemoveItemFromMap(this);

        }

        public async void EffectEvent(FrmLevel frmLevel, Image img, int locationX, int locationY, string name, int sizeX, int sizeY, int tabIndex) 
        {
            effectEventPlayer = new SoundPlayer(global::Fall2020_CSC403_Project.Properties.Resources.wallBoomSound1);
            effectEventPlayer.Play();

            picEffectEvent = new System.Windows.Forms.PictureBox();

            ((System.ComponentModel.ISupportInitialize)(picEffectEvent)).BeginInit();
            frmLevel.SuspendLayout();

            this.picEffectEvent.Image = img;
            this.picEffectEvent.Location = new System.Drawing.Point(locationX, locationY);
            this.picEffectEvent.Name = name;
            this.picEffectEvent.Size = new System.Drawing.Size(sizeX, sizeY);
            this.picEffectEvent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEffectEvent.TabIndex = tabIndex;
            this.picEffectEvent.TabStop = false;

            frmLevel.Controls.Add(picEffectEvent);
            ((System.ComponentModel.ISupportInitialize)(picEffectEvent)).EndInit();
            frmLevel.ResumeLayout(false);

            // Give the gif time to play
            await Task.Delay(3000);

            // Remove the image after the gif plays for however many seconds
            picEffectEvent.Visible = false;

            // Remove the image of the fence after gif plays for however many seconds
            frmLevel.picFence0.Visible = false;
            frmLevel.fences = null;
            //frmLevel
            //frmLevel.picDialog0.Visible = false;
            //frmLevel.dialog[0].Collider = null;
        }

    }

    public class RedPill : AbstractItem, IItem
    {
        public string itemName => "Red Pill";

        public RedPill(FrmLevel frmLevel, int numItems, float locXatLvl, float locYatLvl)
        {
            InitializeComponent(frmLevel, numItems, global::Fall2020_CSC403_Project.Properties.Resources.redcap, locXatLvl, locYatLvl, "RedPill", 50, 50, 18);
            //picItem.BackColor = Color.Gray;
        }
        public void ExecuteEffect(FrmLevel frmLevel)
        {
            RemoveItemFromMap(this);
        }
    }

}
