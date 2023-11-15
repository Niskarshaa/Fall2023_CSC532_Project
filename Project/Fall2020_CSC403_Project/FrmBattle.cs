using Fall2020_CSC403_Project.code;
using Fall2020_CSC403_Project.Properties;
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Fall2020_CSC403_Project {
    public partial class FrmBattle : Form {
        // This is the variable that controls our instance of the class FrmBattle. The active/inactive elements are tied to it.
        public static FrmBattle instance = null;
        public static SoundPlayer simpleSound;
        public static SoundPlayer simpleSFX;

        public Enemy enemy;
        private Player player;
        
        public FrmBattle()
        {
            InitializeComponent();
            player = Game.player;
            // TODO: use mplayer and pause game music while i attack, play sfx, then resume
            simpleSFX = new SoundPlayer(Resources.attack1SFX);
            FrmLevel.levelMusic.Stop();

        }

        private void FormBattle_Disposed(object sender, EventArgs e) 
        {
            this.Close();
        }
        private void FormBattle_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Not currently necessary, all scenarios are handled such as player.Health and enemy.Health
        }

            public void Setup()
        {
            // These should be called and setup with every instance of the enemy in case we switch between enemies
            // Update for this enemy
            picEnemy.BackgroundImage = enemy.Img;
            picEnemy.Refresh();
            BackColor = enemy.Color;
            picEpicBossBattle.Visible = false;
            label3.Visible = false;


            // This is the issue with flee. It should be called once ever for that enemy. Instead each time you flee and return,
            //  it stacks on extra += PlayerDamage, EnemyDamage for heal and attacks, etc.
            // Observer pattern
            /*enemy.AttackEvent += PlayerDamage;
            enemy.HealEvent += EnemyDamage;
            player.AttackEvent += EnemyDamage;*/

            // show health
            UpdateHealthBars();

        }

        public async void SetupForBossBattle()
        {
            picEpicBossBattle.Location = Point.Empty;
            picEpicBossBattle.Size = ClientSize;

            picEpicBossBattle.Visible = true;
            label3.Visible = true;

            //simpleSound.Stop();
            simpleSound = new SoundPlayer(Resources.cgptBossRapTakeoverwav);
            simpleSound.PlayLooping();

            tmrFinalBattle.Enabled = true;

            // Update text box the boss's intro statement
            CGPT cgpt = new CGPT();
            label3.Text = await cgpt.GetBossIntroStatement();
        }
        
        public static FrmBattle GetInstance(Enemy enemy)
        {

            if (instance == null)
            {
                instance = new FrmBattle();
                instance.enemy = enemy;
                instance.Setup();
            }
            return instance;
        }

        private void UpdateHealthBars() {
            float playerHealthPer = player.Health / (float)player.MaxHealth;
            float enemyHealthPer = enemy.Health / (float)enemy.MaxHealth;

            const int MAX_HEALTHBAR_WIDTH = 226;
            lblPlayerHealthFull.Width = (int)(MAX_HEALTHBAR_WIDTH * playerHealthPer);
            lblEnemyHealthFull.Width = (int)(MAX_HEALTHBAR_WIDTH * enemyHealthPer);

            lblPlayerHealthFull.Text = player.Health.ToString();
            lblEnemyHealthFull.Text = enemy.Health.ToString();
        }

        private async void btnAttack_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
            player.OnAttack(-4);
=======
            //player.OnAttack(-6);
            enemy.AlterHealth(-6);
>>>>>>> 3969f4f87fb2c3efd884b9e366d0816f620a1cc3

            if (enemy.Health > 0)
            {

                if (enemy.Name == "BossChatgpt")
                {
                    // Create the constructor
                    CGPT cgpt = new CGPT();

                    // use CGPT to get the boss's attack or heal decision!
                    var result = await cgpt.GetBossDecision(player.Health, enemy.Health, -4, -2, 3);
                    if (result.ToString() == "attack")
                    {
                        //enemy.OnAttack(-2);
                        player.AlterHealth(-2);
                        label3.Text = $"You do 3 damage, and CGPT Attacks for 2 damage";
                    }
                    if (result.ToString() == "heal")
                    {
                        //enemy.OnHeal(4);
                        enemy.AlterHealth(4);
                        label3.Text = $"You do 3 damage, but CGPT Heals 4 health back!";
                    }
                }
                else // enemy.Name == other enemies
                {
                    //enemy.OnAttack(-2);
                    player.AlterHealth(-2);
                    simpleSFX.PlaySync();
                }
            }

            UpdateHealthBars();
            if (player.Health <= 0)
            {
                instance = null;
                DialogResult gotoHomeDialogue = MessageBox.Show("You lose!!! Want to play again?", "YOU LOSE!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (gotoHomeDialogue == DialogResult.Yes)
                {
                    FrmHome.gameplayForm.Close();
                    FrmHome.gameplayForm = null;
                    Program.frmHome = new FrmHome();
                    Program.frmHome.Show();
                    this.Close();
                }
                else
                {
                    Application.Exit();
                }
            }

            if (enemy.Health <= 0) {

                if (enemy == FrmHome.gameplayForm.bossChatgpt)
                {
                    simpleSound = new SoundPlayer(Resources.congrats);
                    simpleSound.Play();

                    FrmHome.gameplayForm.picBossChatgpt.Visible = false;

                    DialogResult gotoHomeDialogue = MessageBox.Show("You win!!! Want to play again?", "YOU WIN!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (gotoHomeDialogue == DialogResult.Yes)
                    {
                        FrmHome.gameplayForm.Close();
                        FrmHome.gameplayForm = null;
                        Program.frmHome = new FrmHome();
                        Program.frmHome.Show();
                        this.Close();
                        instance = null;
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                else // for all other enemies
                {
                    if (FrmHome.gameplayForm.enemyCheeto != null) 
                    {
                        if (FrmHome.gameplayForm.enemyCheeto.Name == enemy.Name)
                        {
                            FrmHome.gameplayForm.picEnemyCheeto.Visible = false;
                            FrmHome.gameplayForm.enemyCheeto = null;
                        }
                    }

                    if (FrmHome.gameplayForm.enemyPoisonPacket != null) 
                    {
                        if (FrmHome.gameplayForm.enemyPoisonPacket.Name == enemy.Name)
                        {
                            Random random = new Random();

                            // If cheeto is dead, we cant generate the item that blows him up. In this case, get random numbers until you can generate any other item.
                            int rpotRandomNum = random.Next(1, 3);
                            if (FrmHome.gameplayForm.enemyCheeto == null)// if cheeto not dead
                            {
                                
                                while (rpotRandomNum == 2)
                                {
                                    rpotRandomNum = random.Next(1, 3);
                                }
                            }
                            
                            FrmHome.gameplayForm.rpot = FrmHome.gameplayForm.InstantiateItem(rpotRandomNum, FrmHome.gameplayForm, FrmHome.gameplayForm.enemyPoisonPacket.Position.x, FrmHome.gameplayForm.enemyPoisonPacket.Position.y);
                            FrmHome.gameplayForm.picEnemyPoisonPacket.Visible = false;
                            FrmHome.gameplayForm.enemyPoisonPacket = null;

                        }
                    }
                    
                    this.Close();
                    instance = null;
                        
                }
            
            }
        }

        private void EnemyDamage(int amount)
        {
            enemy.AlterHealth(amount);
        }

        private void PlayerDamage(int amount) {
            player.AlterHealth(amount);
        }

        private void tmrFinalBattle_Tick(object sender, EventArgs e) {
            picEpicBossBattle.Visible = false;
            tmrFinalBattle.Enabled = false;
        }

        private void btnFlee_Click(object sender, EventArgs e)
        {
            SoundPlayer simpleSound = new SoundPlayer(Resources.flee);
            simpleSound.Play();
            FrmBattle.instance.Close();
            //instance = null;
            FrmBattle.instance = null;
            //this.Hide();
    
        }

        public void playerImgChange()
        {
            picPlayer.BackgroundImage = global::Fall2020_CSC403_Project.Properties.Resources.peanut;
        }
    }
}
