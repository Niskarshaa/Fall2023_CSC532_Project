using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fall2020_CSC403_Project.ProjectRedPill
{
    public partial class TheAwakening: Form
    {
        bool isBlinking = false;
        bool isClickable = true;
        int clickCount = 0;
        public TheAwakening()
        {
            InitializeComponent();
        }

        private async void TheAwakening_Load(object sender, EventArgs e)
        {
            // Initially, make the button invisible and unworkable
            button1.Visible = false;

            // Do the gif event
            pictureBox1.Visible = false;
            await Task.Delay(3000);
            pictureBox1.Visible = true;
            await Task.Delay(6000);
            pictureBox1.Visible = false;


            // Write the first line of text
            await TypeWriterEffect("Your entire life until this point has been a simulation...", label1, 200);
            // Now that its written, make the button visible, focus on it so it can be either clicked or enter can be pressed, then make it blink
            //  to show the player that they can proceed in the text
            button1.Visible = true;
            button1.Focus();
            Blink(true);



            // Use these to make a form fadeIn
            //Opacity = 0;      //first the opacity is 0
            //t1.Interval = 100;  //we'll increase the opacity every 10ms
            //t1.Tick += new EventHandler(FadeIn);  //this calls the function that changes opacity 
            //t1.Start();
        }

        public async void Blink(bool isBlinking)
        {
            // This function sets button1 to blink if isBlinking is true
            while (isBlinking)
            {
                await Task.Delay(500);
                button1.ForeColor = button1.ForeColor == Color.Blue ? Color.Gray : Color.Blue;
            }
        }

        public Task TypeWriterEffect(string txt, Label lbl, int delay)
        {
            // This function executes the typewriter effect required for the event
            return Task.Run(() =>
            {
                for (int i = 0; i <= txt.Length; i++)
                {
                    lbl.Invoke((MethodInvoker)delegate
                    {
                        lbl.Text = txt.Substring(0, i);
                    });
                    System.Threading.Thread.Sleep(delay); ;
                }
            });
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            // This function moves the event/text along

            // Handle different events/texts with each new click
            clickCount += 1;

            // Before presenting new text, stop the blinking of the button and make it invisible
            Blink(false);
            button1.Visible = false;

            // If the button can be clicked,
            if (isClickable)
            {
                // Fire the event/text according to which clickCount we are on
                if (clickCount == 1)
                {

                    // typewriter effect writes the text out
                    await TypeWriterEffect("First line of text...", label1, 100);
                    // Then the button to move text forward is shown
                    button1.Visible = true;
                    //button1.Enabled = true;
                    // Button blinks again to show user it can be clicked
                    Blink(true);

                }
                else if (clickCount == 2)
                {
                    // And so on...
                    await TypeWriterEffect("the second line of text...", label1, 200);
                    button1.Visible = true;
                    //button1.Enabled = true;
                    Blink(true);
                }
                else if (clickCount == 3)
                {
                    await TypeWriterEffect("etc...", label1, 200);
                    //button1.Visible = true;
                    //button1.Enabled = true;
                    //Blink(true);
                }
            }

        }
    }
}
