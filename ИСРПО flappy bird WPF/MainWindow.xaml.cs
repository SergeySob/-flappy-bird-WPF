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

namespace ИСРПО_flappy_bird_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
            gameTimer.Tick += gameEngine; 
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); 
            startGame();
        }

        DispatcherTimer gameTimer = new DispatcherTimer();
        int gravity = 8;
        double score;
        Rect FlappyRect;
        bool gameover = false;

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            flappyBird.RenderTransform = new RotateTransform(5, flappyBird.Width / 2, flappyBird.Height / 2);
            gravity = 8;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                flappyBird.RenderTransform = new RotateTransform(-20, flappyBird.Width / 2, flappyBird.Height / 2);
                gravity = -8;
            }
            if (e.Key == Key.R && gameover == true)
            {
                startGame();
            }
        }

        private void startGame()
        {
            int temp = 200;
            score = 0;
            Canvas.SetTop(flappyBird, 100);
            gameover = false;
            foreach (var x in canvas.Children.OfType<Image>())
            {
                if (x is Image && (string)x.Tag == "obs1")
                {
                    Canvas.SetLeft(x, 500);
                }
                if (x is Image && (string)x.Tag == "obs2")
                {
                    Canvas.SetLeft(x, 800);
                }
                if (x is Image && (string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, 1000);
                }
                if (x is Image && (string)x.Tag == "clouds")
                {
                    Canvas.SetLeft(x, (300 + temp));
                    temp = 800;
                }
            }
            gameTimer.Start();
        }

        private void gameEngine(object sender, EventArgs e)
        {
            scoreText.Content = "Счёт: " + score;
            FlappyRect = new Rect(Canvas.GetLeft(flappyBird), Canvas.GetTop(flappyBird), flappyBird.Width - 12, flappyBird.Height - 6);
            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + gravity);
            if (Canvas.GetTop(flappyBird) + flappyBird.Height > 490 || Canvas.GetTop(flappyBird) < -30)
            {
                gameTimer.Stop();
                gameover = true;
                scoreText.Content += "   Нажмите R чтобы попробовать ещё раз";
            }
            foreach (var x in canvas.Children.OfType<Image>())
            {
                if ((string)x.Tag == "obs1" || (string)x.Tag == "obs2" || (string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);
                    Rect pillars = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (FlappyRect.IntersectsWith(pillars))
                    {
                        gameTimer.Stop();
                        gameover = true;
                        scoreText.Content += "   Нажмите R чтобы попробовать ещё раз";
                    }
                }
                if ((string)x.Tag == "obs1" && Canvas.GetLeft(x) < -100)
                {
                    Canvas.SetLeft(x, 800);
                    score = score + .5;
                }
                if ((string)x.Tag == "obs2" && Canvas.GetLeft(x) < -200)
                {
                    Canvas.SetLeft(x, 800);
                    score = score + .5;
                }
                if ((string)x.Tag == "obs3" && Canvas.GetLeft(x) < -250)
                {
                    Canvas.SetLeft(x, 800);
                    score = score + .5;
                }
                if ((string)x.Tag == "clouds")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - .6);
                    if (Canvas.GetLeft(x) < -220)
                    {
                        Canvas.SetLeft(x, 550);
                    }
                }
            }
        }
    }
}
