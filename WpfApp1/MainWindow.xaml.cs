using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {

        bool goLeft = false; // boolean which will control players going left
        bool goRight = false; // boolean which will control players going right
        bool jumping = false; // boolean to check if player is jumping or not
        bool hasKey = false; // default value of whether the player has the key

        int jumpSpeed = 10; // integer to set jump speed
        int force = 8; // force of the jump in an integer
        int score = 0; // default score integer set to 0

        int playerSpeed = 18; //this integer will set players speed to 18

        DispatcherTimer gameTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            myCanvas.Focus();

            gameTimer.Tick += MainGameTimer;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
        }


        private void MainGameTimer(object sender, EventArgs e)
        {
            Canvas.SetTop(player, Canvas.GetTop(player) + jumpSpeed);

            // if jumping is true and force is less than 0
            // then change jumping to false
            if (jumping && force < 0)
            {
                jumping = false;
            }

            // if jumping is true
            // then change jump speed to -12 
            // reduce force by 1
            if (jumping)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                // else change the jump speed to 12
                jumpSpeed = 12;
            }

            // if go left is true and players left is greater than 100 pixels
            // only then move player towards left of the 
            if (goLeft && Canvas.GetLeft(player) > 30)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            // by doing the if statement above, the player picture will stop on the forms left


            // if go right Boolean is true
            // player left plus players width plus 100 is less than the forms width
            // then we move the player towards the right by adding to the players left
            if (goRight && Canvas.GetLeft(player) + (player.Width + 30) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);

            }
            // by doing the if statement above, the player picture will stop on the forms right


            Rect playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            var children = myCanvas.Children.OfType<Image>().ToList();

            foreach (var k in children)
            {
                // if the picture box found has a tag of coin
                if ((string)k.Tag == "coin")
                {
                    Rect coinHitBox = new Rect(Canvas.GetLeft(k), Canvas.GetTop(k), k.Width, k.Height);

                    // now if the player collides with the coin picture box
                    if (playerHitBox.IntersectsWith(coinHitBox))
                    {
                        myCanvas.Children.Remove(k); // then we are going to remove the coin image
                        score++; // add 1 to the score
                        txtScore.Content = "Coins: " + score;
                    }
                }
            }

            // below if the for loop thats checking for all of the controls in this form
            foreach (var x in myCanvas.Children.OfType<Image>())
            {
                

                // is X is a picture box and it has a tag of platform
                if ((string)x.Tag == "platform")
                {
                    Rect platformHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    // then we are checking if the player is colliding with the platform
                    // and jumping is set to false
                    if (playerHitBox.IntersectsWith(platformHitBox) && !jumping)
                    {
                        // then we do the following
                        force = 8; // set the force to 8
                        Canvas.SetTop(player, Canvas.GetTop(x) - player.Height); // also we place the player on top of the picture box
                        jumpSpeed = 0; // set the jump speed to 0
                    }
                }
            }

            var bgleft = Canvas.GetLeft(background);
            var plleft = Canvas.GetLeft(player);
            txtScore1.Content = "bgleft: " + plleft;


            // if the player collides with the door and has key boolean is true
            Rect doorHitBox = new Rect(Canvas.GetLeft(door), Canvas.GetTop(door), door.Width, door.Height);

            if (playerHitBox.IntersectsWith(doorHitBox) && hasKey)
            {
                // then we change the image of the door to open
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!door.Image = Properties.Resources.door_open;
                // and we stop the timer
                gameTimer.Stop();
                MessageBox.Show("You Completed the level!!"); // show the message box
            }

            // if the player collides with the key picture box
            Rect keyHitBox = new Rect(Canvas.GetLeft(key), Canvas.GetTop(key), key.Width, key.Height);

            if (playerHitBox.IntersectsWith(keyHitBox))
            {
                // then we remove the key from the game
                myCanvas.Children.Remove(key);
                // change the has key boolean to true
                hasKey = true;
            }


            // this is where the player dies
            // if the player goes below the forms height then we will end the game
            if (Canvas.GetTop(player) + player.Height > Application.Current.MainWindow.Height + 60)
            {
                gameTimer.Stop(); // stop the timer
                MessageBox.Show("You Died!!!"); // show the message box
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            //if the player pressed the left key AND the player is inside the panel
            // then we set the car left boolean to true
            if (e.Key == Key.A)
            {
                goLeft = true;
            }
            // if player pressed the right key and the player left plus player width is less then the panel1 width          

            if (e.Key == Key.D)
            {
                // then we set the player right to true
                goRight = true;
            }

            //if the player pressed the space key and jumping boolean is false

            if (e.Key == Key.Space && !jumping)
            {
                // then we set jumping to true
                jumping = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            // if the LEFT key is up we set the car left to false
            if (e.Key == Key.A)
            {
                goLeft = false;
            }
            // if the RIGHT key is up we set the car right to false
            if (e.Key == Key.D)
            {
                goRight = false;
            }
            //when the keys are released we check if jumping is true
            // if so we need to set it back to false so the player can jump again
            if (jumping)
            {
                jumping = false;
            }
        }
    }
}