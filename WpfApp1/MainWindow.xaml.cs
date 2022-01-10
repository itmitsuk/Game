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
        bool goLeft = false; //контроль шага влево
        bool goRight = false; // контроль шага вправо
        bool jumping = false; //контроль пражка
        bool hasKey = false; //проверка на взятие ключа
        bool fall = false;

        int jumpSpeed = 10; //скорость прыжка
        int force = 8; //сила прыжка
        int score = 0; //начальное кол-во очков
        int keys = 0; // начальное кол-во ключей

        int playerSpeed = 11; //скорость персонажа

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

            // условия для прыжка
            if (jumping && force < 0)
            {
                jumping = false;
            }

            // если прыжок то уменьшаем скорость и силу
            if (jumping)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            // условия для шага влево
            if (goLeft && Canvas.GetLeft(player) > 25)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }

            // условие для шага вправо
            if (goRight && Canvas.GetLeft(player) + (player.Width + 25) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);

            }


            Rect playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            var children = myCanvas.Children.OfType<Image>().ToList();

            foreach (Image k in children)
            {
                //если картинка - монета
                if ((string)k.Tag == "coin")
                {
                    Rect coinHitBox = new Rect(Canvas.GetLeft(k), Canvas.GetTop(k), k.Width, k.Height);

                    // игрок взаимодейтсвует с монетой
                    if (playerHitBox.IntersectsWith(coinHitBox))
                    {
                        myCanvas.Children.Remove(k); // убираем изображение
                        score++; //добавляем счет
                        txtScore.Content = "Монеты: " + score;
                    }
                }
            }

            foreach (Image x in myCanvas.Children.OfType<Image>())
            {

                // если картинка - платформа
                if ((string)x.Tag == "platform")
                {
                    Rect platformHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    // если игрок прыгает на платформу
                    if (playerHitBox.IntersectsWith(platformHitBox) && !jumping)
                    {
                        force = 4;
                        Canvas.SetTop(player, Canvas.GetTop(x) - player.Height); // ставим персонажа на платформу
                        jumpSpeed = 0; // завершаем прыжок
                    }

                    if (playerHitBox.IntersectsWith(platformHitBox) == false)
                    {
                        fall = true;
                    }
                    else
                    {
                        fall = false;
                    }
                }
            }

            //если игрок взаимодействует с дверью и имеет ключ
            Rect doorHitBox = new Rect(Canvas.GetLeft(door), Canvas.GetTop(door), door.Width, door.Height);

            if (playerHitBox.IntersectsWith(doorHitBox) && hasKey)
            {

                // останавливаем таймер
                gameTimer.Stop();
                MessageBox.Show("Уровень пройден!");
            }

            // если игрок взаимодействует с ключом
            Rect keyHitBox = new Rect(Canvas.GetLeft(key), Canvas.GetTop(key), key.Width, key.Height);

            if (playerHitBox.IntersectsWith(keyHitBox))
            {
                //удаляем ключ
                myCanvas.Children.Remove(key);
                keys = 1;
                txtKey.Content = "Ключи: " + keys;
                hasKey = true;
            }


            // если игрок ниже высоты окна то конец игры
            if (Canvas.GetTop(player) + player.Height > Application.Current.MainWindow.Height + 60)
            {
                gameTimer.Stop(); // остановка таймера
                MessageBox.Show("Игра закончена!");
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            //если нажимаем кнопка А то шагаем влево
            if (e.Key == Key.A)
            {
                goLeft = true;
            }
            // если нажимаем кнопка D то шаг вправо
            if (e.Key == Key.D)
            {
                goRight = true;
            }

            //если нажимаем кнопка W то прыжок
            if (e.Key == Key.W && !jumping && fall)
            {
                jumping = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            //если зажат кнопка А то перестаем шагать влево
            if (e.Key == Key.A)
            {
                goLeft = false;
            }
            // если зажата кнопка D то перестаем шагать вправо
            if (e.Key == Key.D)
            {
                goRight = false;
            }
            //если зажата кнопка W то перестаем прыгать
            if (jumping)
            {
                jumping = false;
            }
        }
    }
}