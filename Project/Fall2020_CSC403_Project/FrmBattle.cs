﻿using Fall2020_CSC403_Project.code;
using Fall2020_CSC403_Project.Properties;
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Fall2020_CSC403_Project 
{
    public partial class FrmBattle : Form 
    {
        // This is the variable that controls our instance of the class FrmBattle. The active/inactive elements are tied to it.
        public static FrmBattle instance = null;
        public static SoundPlayer simpleSound;
        public static SoundPlayer simpleSFX;

        private Enemy enemy;
        private Player player;
        private FrmLevel frmLevel;
        

        public FrmBattle(FrmLevel frmLevelFromLvl) 
        {
            InitializeComponent();
            player = Game.player;
            // TODO: use mplayer and pause game music while i attack, play sfx, then resume
            simpleSFX = new SoundPlayer(Resources.attack1SFX);
            frmLevel = frmLevelFromLvl;
            FrmLevel.levelMusic.Stop();

        }

        public void FormBattle_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            try 
            {
                // If the player's health reached 0 and the form closed, the player died
                //  so exit the game.
                if (player.Health <= 0)
                {
                    System.Windows.Forms.MessageBox.Show("Game Over");

                    Application.Exit();
                }


                // If player/enemy health>0 and form closes, the user closed the window
                // You cannot run from battle by closing it!
                if (player.Health > 0 && enemy.Health > 0)
                {
                    Application.Exit();
                }

                // If the battle closes normally , release the instance for future fights
                //  so it does not lead to disposed exception.
                if (enemy.Health <= 0)
                {
                    instance = null;
                    //frmLevel.picPoisonPacket
                }
            }
            catch 
            {
                // enemy or player was likely null-ed
                //System.Windows.Forms.MessageBox.Show("Error: Enemy or Player null?");
            }

        }
        public async void Setup() 
        {
            // update for this enemy
            picEnemy.BackgroundImage = enemy.Img;
            picEnemy.Refresh();
            BackColor = enemy.Color;
            picEpicBossBattle.Visible = false;
            label3.Visible = false;

            // Observer pattern
            enemy.AttackEvent += PlayerDamage;
            enemy.HealEvent += EnemyDamage;
            player.AttackEvent += EnemyDamage;

            // show health
            UpdateHealthBars();

            // Update text box the boss's intro statement
            CGPT cgpt = new CGPT();
            label3.Text = await cgpt.GetBossIntroStatement();

        }

        public void SetupForBossBattle() 
        {
            picEpicBossBattle.Location = Point.Empty;
            picEpicBossBattle.Size = ClientSize;

            picEpicBossBattle.Visible = true;
            label3.Visible = true;

            //simpleSound.Stop();
            simpleSound = new SoundPlayer(Resources.cgptBossRapTakeoverwav);
            simpleSound.PlayLooping();

            tmrFinalBattle.Enabled = true;
        }

        // In order to make the instance nonnull, we use this. if its already non null, we can just use FrmBattle.instance to access it. 
        // Or FrmBattle.attribute to access tha attribute if its public.
        // Although, the instance of FrmBattle is tied to the enemy thats fighting the player in the said frmbattle.
        // and everytime the instance is called, the frmbattle map is setup.
        public static FrmBattle GetInstance(Enemy enemy, FrmLevel frmLevel) 
        {

            if (instance == null) 
            {
                instance = new FrmBattle(frmLevel);
                instance.enemy = enemy;
                instance.Setup();
            }
            return instance;
        }

        private void UpdateHealthBars() 
        {

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
            player.OnAttack(-3);

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
                        enemy.OnAttack(-2);
                        label3.Text = $"You do 3 damage, and CGPT Attacks for 2 damage";
                    }
                    if (result.ToString() == "heal")
                    {
                        enemy.OnHeal(4);
                        label3.Text = $"You do 3 damage, but CGPT Heals 4 health back!";
                    }
                }
                else
                {
                    enemy.OnAttack(-2);
                    simpleSFX.PlaySync();
                }

            }
            

            UpdateHealthBars();

            if (enemy.Health <= 0)
            {
                if (enemy.Name == "BossChatgpt")
                {
                    simpleSound = new SoundPlayer(Resources.congrats);
                    simpleSound.Play();
                    System.Windows.Forms.MessageBox.Show("~~~ You Win! ~~~");

                    // Close the window and send to formclosed event
                    instance = null;
                    frmLevel.picBossChatgpt.Visible = false;
                    frmLevel.bossChatgpt = null;
                    Close();
                }
                else
                {
                    // for all other enemies, after battle play another sound

                    // Close the window and send to formclosed event
                    instance = null;

                    if (frmLevel.enemyCheeto.Name == enemy.Name) 
                    {
                        frmLevel.picEnemyCheeto.Visible = false;
                        frmLevel.enemyCheeto = null;
                    }
                    else if (frmLevel.enemyPoisonPacket.Name == enemy.Name) 
                    {
                        frmLevel.picEnemyPoisonPacket.Visible = false;
                        frmLevel.enemyPoisonPacket = null;
                    }
                    Close();
                }
            }

        }

        private void EnemyDamage(int amount) 
        {
            enemy.AlterHealth(amount);
        }

        private void PlayerDamage(int amount) 
        {

            player.AlterHealth(amount);
        }

        private void tmrFinalBattle_Tick(object sender, EventArgs e) 
        {
            picEpicBossBattle.Visible = false;
            tmrFinalBattle.Enabled = false;
        }

    }
}
