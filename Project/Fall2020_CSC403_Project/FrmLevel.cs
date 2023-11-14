using Fall2020_CSC403_Project.code;
using System;
using System.Drawing;
using System.Windows.Forms;
using Fall2020_CSC403_Project.item_system;
using Fall2020_CSC403_Project.item_system.interfaces;
using Fall2020_CSC403_Project.Properties;
using System.Media;

namespace Fall2020_CSC403_Project {

    public partial class FrmLevel : Form {
        private Player player;
        public bool playerHasKey = false;
        public bool playerHasHeal = false;

        public Enemy enemyPoisonPacket;
        public Enemy bossChatgpt;
        public Enemy enemyCheeto;
        public Character[] walls, fences, dialog, key, healing_potion;
        public static SoundPlayer levelMusic; // background music for the level

        private DateTime timeBegin;
        public FrmBattle frmBattle;
        private Random random; // Random number generator for item system
        public IItem rpot; //rpot is always the handle regardless of the item called because its a random item, thus rpot
        private int keyHitCount = 0;
        private int healHitCount = 0;

    public string theme = "Classic";

    public FrmLevel() {
      InitializeComponent();
    }


        private void FrmLevel_Load(object sender, EventArgs e) {
            const int PADDING = 4;
            const int NUM_WALLS = 13;
            const int NUM_FENCES = 1;
            const int NUM_DIALOG = 1;
            const int NUM_KEY = 1;
            const int NUM_HEAL = 1;


            player = new Player(CreatePosition(picPlayer), CreateCollider(picPlayer, PADDING));
            //bossChatgpt = new Enemy(CreatePosition(picBossChatgpt), CreateCollider(picBossChatgpt, PADDING));
            bossChatgpt = new Enemy(CreatePosition(picBossChatgpt), CreateCollider(picBossChatgpt, PADDING));
            enemyPoisonPacket = new Enemy(CreatePosition(picEnemyPoisonPacket), CreateCollider(picEnemyPoisonPacket, PADDING));
            enemyCheeto = new Enemy(CreatePosition(picEnemyCheeto), CreateCollider(picEnemyCheeto, PADDING));


            bossChatgpt.Img = picBossChatgpt.BackgroundImage;
            enemyPoisonPacket.Img = picEnemyPoisonPacket.BackgroundImage;
            enemyCheeto.Img = picEnemyCheeto.BackgroundImage;

            bossChatgpt.Color = Color.Green;
            enemyPoisonPacket.Color = Color.Green;
            enemyCheeto.Color = Color.FromArgb(255, 245, 161);

            bossChatgpt.Name = "BossChatgpt";
            enemyPoisonPacket.Name = "PoisonPacket";
            enemyCheeto.Name = "Cheeto";

            walls = new Character[NUM_WALLS];
            for (int w = 0; w < NUM_WALLS; w++) {
                PictureBox pic = Controls.Find("picWall" + w.ToString(), true)[0] as PictureBox;
                walls[w] = new Character(CreatePosition(pic), CreateCollider(pic, PADDING));
            }

            fences = new Character[NUM_FENCES];
            for (int w = 0; w < NUM_FENCES; w++) {
                PictureBox pic = Controls.Find("picFence" + w.ToString(), true)[0] as PictureBox;
                fences[w] = new Character(CreatePosition(pic), CreateCollider(pic, PADDING));
            }

            dialog = new Character[NUM_DIALOG];
            for (int w = 0; w < NUM_DIALOG; w++) {
                PictureBox pic = Controls.Find("picDialog" + w.ToString(), true)[0] as PictureBox;
                dialog[w] = new Character(CreatePosition(pic), CreateCollider(pic, PADDING));
            }

            key = new Character[NUM_KEY];
            for (int w = 0; w < NUM_KEY; w++) {
                PictureBox pic = Controls.Find("picKey" + w.ToString(), true)[0] as PictureBox;
                key[w] = new Character(CreatePosition(pic), CreateCollider(pic, PADDING));
            }

            healing_potion = new Character[NUM_HEAL];
            for (int w = 0; w < NUM_KEY; w++)
            {
                PictureBox pic = Controls.Find("picHealthPotion" + w.ToString(), true)[0] as PictureBox;
                healing_potion[w] = new Character(CreatePosition(pic), CreateCollider(pic, PADDING));
            }

            picDialog0.Visible = false;
            picKey0.Visible = false;
            picHealthPotion0.Visible = false;
            picHealthPotion0.Enabled = false;

            Game.player = player;
            timeBegin = DateTime.Now;
            // Initialize the looping level music
            levelMusic = new SoundPlayer(Resources.gamemusic);
            levelMusic.PlayLooping();

            // Get a random number generator
            random = new Random();
        }

        private Vector2 CreatePosition(PictureBox pic) {
            return new Vector2(pic.Location.X, pic.Location.Y);
        }

        private Collider CreateCollider(PictureBox pic, int padding) {
            Rectangle rect = new Rectangle(pic.Location, new Size(pic.Size.Width - padding, pic.Size.Height - padding));
            return new Collider(rect);
        }

        private void FrmLevel_KeyUp(object sender, KeyEventArgs e) {
            player.ResetMoveSpeed();
        }

        private void tmrUpdateInGameTime_Tick(object sender, EventArgs e) {
            TimeSpan span = DateTime.Now - timeBegin;
            string time = span.ToString(@"hh\:mm\:ss");
            lblInGameTime.Text = "Time: " + time.ToString();
        }

        private void UpdatePlayerBar(object sender, EventArgs e)
        {
            float playerHealthPer = player.Health / (float)player.MaxHealth;
            const int MAX_HEALTHBAR_WIDTH = 200;
            lblPlayerHealthFull.Width = (int)(MAX_HEALTHBAR_WIDTH * playerHealthPer);
            lblPlayerHealthFull.Text = player.Health.ToString();
            if (player.Health <= 16 && player.Health >= 13)
            {
                lblPlayerHealthFull.BackColor = Color.GreenYellow;
            }
            if (player.Health <= 12 && player.Health >= 9)
            {
                lblPlayerHealthFull.BackColor = Color.Yellow;
            }
            if (player.Health <= 8 && player.Health >= 5)
            {
                lblPlayerHealthFull.BackColor = Color.DarkOrange;
            }
            if (player.Health <= 4 && player.Health >= 1)
            {
                lblPlayerHealthFull.BackColor = Color.DarkRed;
            }

        }


        private void tmrPlayerMove_Tick(object sender, EventArgs e) {
            // move player
            player.Move();

            // check collision with walls
            try {
                if (HitAWall(player)) {
                    player.MoveBack();
                }
            }
            catch {
                // walls dont exist to collide with!
            }

            // check collision with fences
            if (fences != null) { 
                if (HitAFence(player))
                {
                    player.MoveBack();
                }
            }
            

            // check collision with key
            if (HitAKey(player)) {
                player.MoveBack();    
            }

            if (HitAHeal(player)){
                player.MoveBack();
            }

            // check collision with enemies

            if (enemyPoisonPacket != null)
            {
                if (HitAChar(player, enemyPoisonPacket))
                {
                    if (IsEnemyDead(enemyPoisonPacket) == true)
                    {
                        Fight(enemyPoisonPacket);
                        // Generate a random number to get a random effect from the RandomPotion (first 3 potions will be random potion candidates)

                        // In the current state, the potion cannot be generated under improper conditions, BUT
                        // if you do something new like FLEE from battle, the potion will generate like this! Need a condition to fix this.
                        // Wait to see how Nis implements flee
                        //if (this.enemyPoisonPacket == null)
                        //{
                            // If, specifically, enemyPoisonPacket died within the battle form, after the battle generate the potion
                            //  the other option to do this is to move rpot to inside the frmBattle instance code, but thats much heavier lifting than this
                            //rpot = InstantiateItem(random.Next(1, 3), this, enemyPoisonPacket.Position.x, enemyPoisonPacket.Position.y);
                        //}

                    }
                }
            }
            //else if (enemyPoisonPacket == null) 
            //{
                // enemyPoisonPacket died and was thus nulled after killing him
                //rpot = InstantiateItem(random.Next(1, 3), this, enemyPoisonPacket.Position.x, enemyPoisonPacket.Position.y);
            //}

            if (enemyCheeto != null) {
                if (HitAChar(player, enemyCheeto)){
                    if (IsEnemyDead(enemyCheeto) == true) {
                        Fight(enemyCheeto);
                    }
                }
            }
            
            if (bossChatgpt != null) {
                if (HitAChar(player, bossChatgpt)) {
                    // not triggering
                    if (IsEnemyDead(bossChatgpt) == true)
                    {
                        Fight(bossChatgpt);
                    }
                }
                    
            }
        
            if (HitAnItem(player)) {
                player.MoveBack();
                rpot.ExecuteEffect(this); //rpot is always the handle regardless of the item called because its a random item, thus rpot
            }

            // update player's picture box
            picPlayer.Location = new Point((int)player.Position.x, (int)player.Position.y);
        }
        private void AllSideEnemyDied(Character c) {
            //bool allSideEnemyDied = false;
            bool keyHit = HitAKey(player);
            bool healHit = HitAHeal(player);

            if (enemyCheeto != null && enemyPoisonPacket != null)
            {
                // This only works if neither enemy has been killed (nulled) by the bomb and their instance disposed of
                //  so we need to check to see if the enemy is null before checking to see if their health is 0
                if (IsEnemyDead(enemyPoisonPacket) == false && IsEnemyDead(enemyCheeto) == false)
                {
                    if (keyHit == true)
                    {
                        picKey0.Visible = false;

                    }
                    if (keyHit == false && keyHitCount == 0)
                    {
                        picKey0.Visible = true;
                    }

                    if (healHit == true)
                    {
                        picHealthPotion0.Visible = false;

                    }
                    if (healHit == false && healHitCount == 0)
                    {
                        picHealthPotion0.Enabled = true;
                        picHealthPotion0.Visible = true;
                    }
                }
            }
            else if (enemyCheeto == null && enemyPoisonPacket == null) 
            {
                // But, as before, we need this to trigger if both of them are killed (nulled) when the boom triggers
                if (keyHit == true)
                {
                    picKey0.Visible = false;

                }
                if (keyHit == false && keyHitCount == 0)
                {
                    picKey0.Visible = true;
                }

                if (healHit == true)
                {
                    picHealthPotion0.Visible = false;

                }
                if (healHit == false && healHitCount == 0)
                {
                    picHealthPotion0.Enabled = true;
                    picHealthPotion0.Visible = true;
                }
            }
        }

        private bool HitAnItem(Character player) {

            bool hitAnItem = false;
            try {
                if (rpot != null) {
                    if (player.Collider.Intersects(rpot.collider)) {
                        hitAnItem = true;
                    }
                }
            }
            catch {
                //Error: null item?
            }

            return hitAnItem;
        }
        private bool HitAWall(Character c) {

            bool hitAWall = false;
            try 
            {
                if (walls[0] != null) 
                {
                    for (int w = 0; w < walls.Length; w++)
                    {
                        if (c.Collider.Intersects(walls[w].Collider))
                        {
                            hitAWall = true;
                            break;
                        }
                    }
                }
                
            }
            catch 
            {
                // walls dont exist to collide with!
            }
      
            return hitAWall;
        }
        private bool HitAKey(Character c) {
            bool hitAKey = false;
            if (enemyPoisonPacket != null && enemyCheeto != null) 
            {
                // neither of them have been nulled, so we can check to see if they are dead
                if (playerHasKey == false && IsEnemyDead(enemyPoisonPacket) == false && IsEnemyDead(enemyCheeto) == false)
                {
                    for (int w = 0; w < key.Length; w++)
                    {
                        if (c.Collider.Intersects(key[w].Collider))
                        {
                            hitAKey = true;
                            playerHasKey = true;
                            keyHitCount++;
                            picKey0.Visible = false;
                            break;
                        }
                    }
                }
            }
            else if (enemyPoisonPacket == null && enemyCheeto == null)
            {
                // The explosion killed both cheeto and poisonpacket, so they are both null (how ive set them to be "dead" ..
                //  if they have been killed there is no purpose to keep their instance around unless we intend to revive them or something, so I dispose of them with null instead of health=0/invisible)
                // the reason we include this else if... is because we need to handle the situation where the bomb killed both of them, but we need the key to drop
                for (int w = 0; w < key.Length; w++)
                {
                    if (c.Collider.Intersects(key[w].Collider))
                    {
                        hitAKey = true;
                        playerHasKey = true;
                        keyHitCount++;
                        picKey0.Visible = false;
                        break;
                    }
                }
            }
            
            return hitAKey;
        }

        private bool HitAHeal(Character c)
        {
            bool hitAHeal = false;
            // if both enemies dead via boom item or fighting, healing potion hasnt been nulled yet
            if (enemyPoisonPacket == null && enemyCheeto == null && healing_potion != null) 
            {

                for (int w = 0; w < healing_potion.Length; w++)
                {
                    if (c.Collider.Intersects(healing_potion[w].Collider))
                    {
                        hitAHeal = true;
                        playerHasHeal = true;
                        healHitCount++;
                        picHealthPotion0.Visible = false;
                        player.Health = player.Health + 10;
                        healing_potion = null;
                        break;
                    }
                }
                
            }

            return hitAHeal;
        }

        private bool HitAFence(Character c) {
            bool hitAFence = false;
            for (int w = 0; w < fences.Length; w++) {
                if(playerHasKey == false) { 
                    if (c.Collider.Intersects(fences[w].Collider)) {
                        hitAFence = true;
                        picDialog0.Visible = true;
                        Timer timer = new Timer();
                        timer.Interval = 1500;
                        timer.Tick += (sender, e) => {
                            picDialog0.Visible = false;
                            timer.Stop();
                            timer.Dispose();
                        };
                        timer.Start();
       
                        break;
                    }
                }
                else {
                    picDialog0.Visible = false;
                    picFence0.Visible = false;
                }
            }
            return hitAFence;
        }

        private bool HitAChar(Character you, Character other) {
            AllSideEnemyDied(other);
            return you.Collider.Intersects(other.Collider);
        }

        public void Fight(Enemy enemy) {
            player.ResetMoveSpeed();
            player.MoveBack();
            frmBattle = FrmBattle.GetInstance(enemy, this);
            if (enemy.Name == "BossChatgpt") {
                frmBattle.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.Psychedelic;
            }
            else {
                frmBattle.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.enemyFightBG;
            }
            frmBattle.Show();
      
            if (enemy == bossChatgpt) {
                frmBattle.SetupForBossBattle();
            }
        }



        private void FrmLevel_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Left:
                    player.GoLeft();
                    break;

                case Keys.Right:
                    player.GoRight();
                    break;

                case Keys.Up:
                    player.GoUp();
                    break;

                case Keys.Down:
                    player.GoDown();
                    break;

                case Keys.Escape:
                    DialogResult exitDialogue = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (exitDialogue == DialogResult.Yes) {
                        Application.Exit();
                    }
                    break;

                case Keys.Q:
                    DialogResult gotoHomeDialogue = MessageBox.Show("Are you sure you want to go to home and lose all progress?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (gotoHomeDialogue == DialogResult.Yes) {
                        Program.frmHome.Show();
                        FrmHome.gameplayForm = null;
                        FrmLevel.levelMusic.Stop();
                        this.Close();
                        
                        
                    }
                    break;

                default:
                    player.ResetMoveSpeed();
                    break;
            } 
        }
        private void lblInGameTime_Click(object sender, EventArgs e) {

        }

        private void goToHome_Click(object sender, EventArgs e) {
            DialogResult gotoHomeDialogue = MessageBox.Show("Are you sure you want to go to home and lose all progress?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (gotoHomeDialogue == DialogResult.Yes) {
                Program.frmHome.Show();
                FrmHome.gameplayForm = null;
                FrmLevel.levelMusic.Stop();
                this.Close();
            }
        }
        public bool IsEnemyDead(Enemy enemy) { 
            if (enemy.Health <= 0) {



                if (enemy == enemyPoisonPacket ) {
                    picEnemyPoisonPacket.Visible = false;       
                }
              
                if (enemy == bossChatgpt ) {
                    picBossChatgpt.Visible = false;  
                }
             
                if (enemy == enemyCheeto ) {
                    picEnemyCheeto.Visible = false;    
                }
            
            return false;
                }
             else
                {
                    return true;
                }
            
   }

        public void applyTheme1()
        {
            theme = "New";
            this.BackColor = Color.Black;

            this.picPlayer.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.peanut;

            this.picEnemyPoisonPacket.Location = new System.Drawing.Point(620, 260);
            this.picEnemyCheeto.Location = new System.Drawing.Point(120, 200);

            this.picBossChatgpt.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.iamchatgpt2;
            this.picEnemyPoisonPacket.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.enemy_poisonpacket2;
            this.picEnemyCheeto.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.enemy_cheetos2;

            bossChatgpt = new Enemy(CreatePosition(picBossChatgpt), CreateCollider(picBossChatgpt, 7));
            enemyPoisonPacket = new Enemy(CreatePosition(picEnemyPoisonPacket), CreateCollider(picEnemyPoisonPacket, 7));
            enemyCheeto = new Enemy(CreatePosition(picEnemyCheeto), CreateCollider(picEnemyCheeto, 7));

            bossChatgpt.Img = global::Fall2020_CSC403_Project.Properties.Resources.iamchatgpt2;
            enemyPoisonPacket.Img = global::Fall2020_CSC403_Project.Properties.Resources.enemy_poisonpacket2;
            enemyCheeto.Img = global::Fall2020_CSC403_Project.Properties.Resources.enemy_cheetos2;

            bossChatgpt.Color = Color.Black;
            enemyPoisonPacket.Color = Color.Lavender;
            enemyCheeto.Color = Color.Orange;

            this.picEnemyPoisonPacket.Size = new System.Drawing.Size(144, 148);

            this.picWall3.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall2;
            this.picWall5.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall2;
            this.picWall4.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall2;
            this.picWall12.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall2;
            this.picWall6.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall2;
            this.picWall9.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall2;
            this.picWall10.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall2;
            this.picWall0.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall2;
            this.picWall7.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall2;
            this.picWall8.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall2;
            this.picWall1.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall2;
            this.picWall2.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall2;
            this.picWall11.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall2;
        }

        public void applyTheme2()
        {
            theme = "Invisible";
            this.BackColor = Color.Black;

            this.picPlayer.Size = new System.Drawing.Size(10, 10);
            player = new Player(CreatePosition(picPlayer), CreateCollider(picPlayer, 7));
            this.picPlayer.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.dot;

            this.picEnemyPoisonPacket.Location = new System.Drawing.Point(612, 68);
            this.picEnemyCheeto.Location = new System.Drawing.Point(120, 200);

            this.picEnemyPoisonPacket.Size = new System.Drawing.Size(40, 40);
            this.picEnemyCheeto.Size = new System.Drawing.Size(40,40);

            enemyPoisonPacket = new Enemy(CreatePosition(picEnemyPoisonPacket), CreateCollider(picEnemyPoisonPacket, 7));
            enemyCheeto = new Enemy(CreatePosition(picEnemyCheeto), CreateCollider(picEnemyCheeto, 7));

            enemyPoisonPacket.Img = global::Fall2020_CSC403_Project.Properties.Resources.enemy_poisonpacket;
            enemyCheeto.Img = global::Fall2020_CSC403_Project.Properties.Resources.enemy_cheetos;

            bossChatgpt.Color = Color.Black;
            enemyPoisonPacket.Color = Color.Black;
            enemyCheeto.Color = Color.Black;

            this.picEnemyPoisonPacket.Visible = false;
            this.picEnemyCheeto.Visible = false;
            this.picBossChatgpt.Visible = false;
            this.picFence0.Visible = false;

            this.picWall3.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall3;
            this.picWall5.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall3;
            this.picWall4.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall3;
            this.picWall12.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall3;
            this.picWall6.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall3;
            this.picWall9.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall3;
            this.picWall10.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall3;
            this.picWall0.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall3;
            this.picWall7.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall3;
            this.picWall8.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall3;
            this.picWall1.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall3;
            this.picWall2.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall3;
            this.picWall11.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.wall3;
        }

    }
}
