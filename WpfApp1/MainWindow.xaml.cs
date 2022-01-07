using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {

        bool goleft = false; // boolean which will control players going left
        bool goright = false; // boolean which will control players going right
        bool jumping = false; // boolean to check if player is jumping or not
        bool hasKey = false; // default value of whether the player has the key

        int jumpSpeed = 10; // integer to set jump speed
        int force = 8; // force of the jump in an integer
        int score = 0; // default score integer set to 0

        int playSpeed = 18; //this integer will set players speed to 18
        int backLeft = 8; // this integer will set the background moving speed to 8

        DispatcherTimer gameTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            myCanvas.Focus();

            gameTimer.Tick += mainGameTimer;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
        }


        private void mainGameTimer(object sender, EventArgs e)
        {
            // linking the jumpspeed integer with the player picture boxes to location
            Canvas.SetLeft(player, Canvas.GetLeft(player) + jumpSpeed);

            // refresh the player picture box consistently
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!player.Refresh();

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
            if (goleft && Canvas.GetLeft(player) > 100)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playSpeed);
            }
            // by doing the if statement above, the player picture will stop on the forms left


            // if go right Boolean is true
            // player left plus players width plus 100 is less than the forms width
            // then we move the player towards the right by adding to the players left
            if (goright && Canvas.GetLeft(player) + (player.Width + 100) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playSpeed);

            }
            // by doing the if statement above, the player picture will stop on the forms right


            // if go right is true and the background picture left is greater 1352
            // then we move the background picture towards the left
            if (goright &&  Canvas.GetLeft(background) > -1353)        /*backgroud.left*/
            {
                Canvas.SetLeft(background, Canvas.GetLeft(background) - backLeft); 

                // the for loop below is checking to see the platforms and coins in the level
                // when they are found it will move them towards the left
                foreach (Image x in myCanvas.Children.OfType<Image>())
                {
                    if (x is Image && x.Name == "platform" || x is Image && x.Name == "coin" || x is Image && x.Name == "door" || x is Image && x.Name == "key")
                    {
                        Canvas.SetLeft(x, - backLeft);
                    }
                }

            }

            // if go left is true and the background pictures left is less than 2
            // then we move the background picture towards the right
            if (goleft && Canvas.GetLeft(background) < 2)
            {
                Canvas.SetLeft(background, Canvas.GetLeft(background) + backLeft);

                // below the is the for loop thats checking to see the platforms and coins in the level
                // when they are found in the level it will move them all towards the right with the background
                //foreach (Control x in this.Controls)
                foreach (var x in myCanvas.Children.OfType<Image>())
                {
                    if (x is Image && x.Name == "platform" || x is Image && x.Name == "coin" || x is Image && x.Name == "door" || x is Image && x.Name == "key")
                    {
                        Canvas.SetLeft(x, + backLeft);
                    }
                }
            }

            Rect playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            // below if the for loop thats checking for all of the controls in this form
            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {

                // is X is a picture box and it has a tag of platform
                if (x.Name == "platform")
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
                // if the picture box found has a tag of coin
                if (x.Name == "coin")
                {
                    Rect coinHitBox = new Rect(Canvas.GetLeft(coin), Canvas.GetTop(coin), coin.Width, coin.Height);

                    // now if the player collides with the coin picture box
                    if (playerHitBox.IntersectsWith(coinHitBox))
                    {
                        myCanvas.Children.Remove(x); // then we are going to remove the coin image
                        score++; // add 1 to the score
                    }
                }
            }

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

        private void keyisdown(object sender, KeyEventArgs e)
        {
            //if the player pressed the left key AND the player is inside the panel
            // then we set the car left boolean to true
            if (e.Key == Key.A)
            {
                goleft = true;
            }
            // if player pressed the right key and the player left plus player width is less then the panel1 width          

            if (e.Key == Key.D)
            {
                // then we set the player right to true
                goright = true;
            }

            //if the player pressed the space key and jumping boolean is false

            if (e.Key == Key.Space && !jumping)
            {
                // then we set jumping to true
                jumping = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            // if the LEFT key is up we set the car left to false
            if (e.Key == Key.A)
            {
                goleft = false;
            }
            // if the RIGHT key is up we set the car right to false
            if (e.Key == Key.D)
            {
                goright = false;
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
