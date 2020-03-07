using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BruhSnakeTimeBois
{




    public partial class Game : Form
    {
        private int verVelocity = 0;
        private int horVelocity = 0;
        private int tempVer = 0;
        private int tempHor = 0;
        private int Score = 0;

        private Rectangle CropRectangle = new Rectangle(10, 10, 60, 60);

        private int[] AllowedValues = new int[] {  18, 20, 23 };

        private string headDirection = "R";
        private string tempHeadDirection;

        private bool GameStart = false;

        private int snakeSpeed = 20;

        private Random rand = new Random();

        private GameArea Area;
        private List<SnakePixel> Snake = new List<SnakePixel>();
        private Timer GameTimer;
        private Food Food;
        private Label Scoreboard;
        private Tile Tile;

        public Game()
        {
            InitializeComponent();
            InitializeGameArea();
            InitializeTiles();
            InitializeGame();
            InitializeSnake();
            InitializeFood();
            InitializeGameTimer();
            SnakeHeadSprite();
            InitializeScoreBoard();
        }

        private int GetRandomNumber()
        {
            return AllowedValues[rand.Next(AllowedValues.Length)];
        }

        private Image CropImage(Image img, Rectangle rect)
        {
            return ((Bitmap)img).Clone(rect, img.PixelFormat);
        }
   
        private void InitializeTiles()
        {
            for(int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {

                    int GetNumber = GetRandomNumber();

                    Tile = new Tile();

                    Tile.Image = (Image)Properties.Resources.ResourceManager.GetObject("tile0" + GetNumber.ToString());
                    Tile.Image = CropImage(Tile.Image, CropRectangle);

                    Tile.Top = 100 + (20 * x);
                    Tile.Left = 100 + (20 * y);
                    
                    Tile.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.Controls.Add(Tile);
                    Tile.BringToFront();
                }
            }
        }

        private void InitializeScoreBoard()
        {
            Scoreboard = new Label();
            Scoreboard.Width = 500;
            Scoreboard.Height = 40;
            Scoreboard.Top = 20;
            Scoreboard.Left = 20;
            Scoreboard.Font = new Font("Comic Sans MS", 20, FontStyle.Bold);
            Scoreboard.Text = "Your Score is: " + Score.ToString();
            this.Controls.Add(Scoreboard);
        }

        private void InitializeFood()
        {
            Food = new Food();
            Food.Left = 100 + 20 * RandGenerate(0, 19);
            Food.Top = 100 + 20 * RandGenerate(0, 19);
            for (int i = Snake.Count - 1; i > -1; i--)
            {
                if (Snake[i].Bounds.IntersectsWith(Food.Bounds))
                {
                    do
                    {
                        Food.Left = 100 + 20 * RandGenerate(0, 19);
                        Food.Top = 100 + 20 * RandGenerate(0, 19);
                    } while (Snake[i].Bounds.IntersectsWith(Food.Bounds));

                }
            }
            FoodSprite();
            this.Controls.Add(Food);
            Food.BringToFront();
        }

        private void InitializeGameTimer()
        {
            GameTimer = new Timer();
            GameTimer.Interval = 500;
            GameTimer.Tick += new EventHandler(GameTimer_Tick);
            GameTimer.Start();

        }

        private void RegenerateFood()
        {
            Food.Left = 100 + 20 * RandGenerate(0, 19);
            Food.Top = 100 + 20 * RandGenerate(0, 19);
            for (int i = Snake.Count - 1; i > -1; i--)
            {
                if (Snake[i].Bounds.IntersectsWith(Food.Bounds))
                {
                    do
                    {
                        Food.Left = 100 + 20 * RandGenerate(0, 19);
                        Food.Top = 100 + 20 * RandGenerate(0, 19);
                    } while (Snake[i].Bounds.IntersectsWith(Food.Bounds));

                }
            }

            FoodSprite();
        }

        private void FoodSprite()
        {
            string foodSprite = "snake_food_" + RandGenerate(1, 2).ToString();
            Food.Image = (Image)Properties.Resources.ResourceManager.GetObject(foodSprite);
            Food.Image = CropImage(Food.Image, CropRectangle);
            Food.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private int RandGenerate(int a, int b)
        {
            return rand.Next(a, b + 1);
        }

        private void ScoreCount()
        {
            Score++;
            Scoreboard.Text = "Your Score is: " + Score.ToString();
        }

        private void AddSnakePixel()
        {
            Snake.Add(new SnakePixel());
            Snake[Snake.Count - 1].Top = Snake[Snake.Count - 2].Top;
            Snake[Snake.Count - 1].Left = Snake[Snake.Count - 2].Left;
            this.Controls.Add(Snake[Snake.Count - 1]);
            Snake[Snake.Count - 1].BringToFront();
        }

        private void SnakeFoodCollision()
        {
            if(Snake[0].Bounds.Contains(Food.Bounds))
            {
                RegenerateFood();
                AddSnakePixel();
                SpriteBringToFront();
                ScoreCount();
                IncreaseSpeed();
            }
        }

        private void IncreaseSpeed()
        {
            if(GameTimer.Interval > 60)
            {
                GameTimer.Interval -= 20;
            }
        }

        private void GameOver()
        {
            for (int i = Snake.Count - 1; i > 0; i--)
            {
                this.Controls.Remove(Snake[i]);
                Snake.Remove(Snake[i]);
            }
            Snake[0].Left = 200;
            Snake[0].Top = 200;
            horVelocity = 0;
            verVelocity = 0;
            GameTimer.Stop();
            GameTimer.Interval = 500;
            MessageBox.Show("Game Over!");
            Snake[0].BringToFront();
            GameStart = false;
            Score = 0;
            Scoreboard.Text = "Game Over!";


        }

        private void SnakeBodyCollision()
        {
            for(int i = Snake.Count - 1; i > 2; i--)
            {
                if (Snake[0].Bounds.IntersectsWith(Snake[i].Bounds))
                {
                    GameOver();
                    return;
                }
            }
        }
        
        private void SnakeAreaCollision()
        {
            if(!Area.Bounds.Contains(Snake[0].Bounds))
            {
                GameOver();
            }
        }

        private void SetHeadDirection()
        {
            if (Snake.Count > 1)
            {
                if (tempHeadDirection == "R" && headDirection == "L" ||
                    tempHeadDirection == "U" && headDirection == "D" ||
                    tempHeadDirection == "L" && headDirection == "R" ||
                    tempHeadDirection == "D" && headDirection == "U")
                    return;
            }
            headDirection = tempHeadDirection;
        }

        private void SetDirection()
        {
          if (Snake.Count > 1)
          {
             if (tempHor == -horVelocity || tempVer == -verVelocity)
             return;
          }

          horVelocity = tempHor;
          verVelocity = tempVer;
        }

        private void SnakeTailSprite()
        {
            if(Snake[Snake.Count - 1].Top > Snake[Snake.Count - 2].Top && Snake[Snake.Count - 1].Left == Snake[Snake.Count - 2].Left)
            {
                Snake[Snake.Count - 1].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_tail_270");
                Snake[Snake.Count - 1].Image = CropImage(Snake[Snake.Count - 1].Image, CropRectangle);
            }
            else if (Snake[Snake.Count - 1].Top < Snake[Snake.Count - 2].Top && Snake[Snake.Count - 1].Left == Snake[Snake.Count - 2].Left)
            {
                Snake[Snake.Count - 1].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_tail_90");
                Snake[Snake.Count - 1].Image = CropImage(Snake[Snake.Count - 1].Image, CropRectangle);
            }
            else if (Snake[Snake.Count - 1].Left > Snake[Snake.Count - 2].Left && Snake[Snake.Count - 1].Top == Snake[Snake.Count - 2].Top)
            {
                Snake[Snake.Count - 1].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_tail_180");
                Snake[Snake.Count - 1].Image = CropImage(Snake[Snake.Count - 1].Image, CropRectangle);
            }
            else if (Snake[Snake.Count - 1].Left < Snake[Snake.Count - 2].Left && Snake[Snake.Count - 1].Top == Snake[Snake.Count - 2].Top)
            {
                Snake[Snake.Count - 1].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_tail_0");
                Snake[Snake.Count - 1].Image = CropImage(Snake[Snake.Count - 1].Image, CropRectangle);
            }
            Snake[Snake.Count -1].SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void SnakeHeadSprite()
        {
            switch(headDirection)
            {
                case "L":
                    Snake[0].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_head_180");
                    Snake[0].Image = CropImage(Snake[0].Image, CropRectangle);
                    break;
                case "R":
                    Snake[0].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_head_0");
                    Snake[0].Image = CropImage(Snake[0].Image, CropRectangle);
                    break;
                case "D":
                    Snake[0].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_head_90");
                    Snake[0].Image = CropImage(Snake[0].Image, CropRectangle);
                    break;
                case "U":
                    Snake[0].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_head_270");
                    Snake[0].Image = CropImage(Snake[0].Image, CropRectangle);
                    break;
              
            }
            Snake[0].SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void SnakeBodySprite()
        {
            for (int i = Snake.Count - 2; i > 0; i--)
            {
                if(Snake[i + 1].Top == Snake[i - 1].Top)
                {
                    Snake[i].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_body_0");
                    Snake[i].Image = CropImage(Snake[i].Image, CropRectangle);
                }
                else if(Snake[i + 1].Left == Snake[i - 1].Left)
                {
                    Snake[i].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_body_90");
                    Snake[i].Image = CropImage(Snake[i].Image, CropRectangle);
                }
                else if(Snake[i].Top > Snake[i + 1].Top || Snake[i].Top > Snake[i - 1].Top)
                {
                    if(Snake[i].Left > Snake[i + 1].Left || Snake[i].Left > Snake[i - 1].Left)
                    {
                        Snake[i].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_corner_ccw_270");
                        Snake[i].Image = CropImage(Snake[i].Image, CropRectangle);
                    }
                    else if(Snake[i].Left < Snake[i + 1].Left || Snake[i].Left < Snake[i - 1].Left)
                    {
                        Snake[i].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_corner_ccw_0");
                        Snake[i].Image = CropImage(Snake[i].Image, CropRectangle);
                    }
                }
                else if(Snake[i].Top < Snake[i + 1].Top || Snake[i].Top < Snake[i - 1].Top)
                {
                    if (Snake[i].Left > Snake[i + 1].Left || Snake[i].Left > Snake[i - 1].Left)
                    {
                        Snake[i].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_corner_ccw_180");
                        Snake[i].Image = CropImage(Snake[i].Image, CropRectangle);
                    }
                    else if (Snake[i].Left < Snake[i + 1].Left || Snake[i].Left < Snake[i - 1].Left)
                    {
                        Snake[i].Image = (Image)Properties.Resources.ResourceManager.GetObject("snake_corner_ccw_90");
                        Snake[i].Image = CropImage(Snake[i].Image, CropRectangle);
                    }
                }               
                Snake[i].SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void SpriteBringToFront()
        {
            for (int i = Snake.Count - 1; i > 0; i--)
            {
                Snake[i].BringToFront();
            }
            Snake[0].BringToFront();
        }

        private void SnakeSprite()
        {
            SetHeadDirection();
            SnakeHeadSprite();
            if (Snake.Count > 2)
            {
                SnakeTailSprite();
                SnakeBodySprite();
            }
            else if (Snake.Count > 1)
            {
                SnakeTailSprite();
            }
            
        }

        private void MoveSnake()
        {
            SetDirection();

            for(int i = Snake.Count - 1; i > 0; i--)
            {
                Snake[i].Left = Snake[i - 1].Left;
                Snake[i].Top = Snake[i - 1].Top;
            }
            Snake[0].Left += horVelocity;
            Snake[0].Top += verVelocity;
            SnakeSprite();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
            SnakeFoodCollision();
            SnakeAreaCollision();
            SnakeBodyCollision();
        }

        private void InitializeGame()
        {
            this.Width = 600;
            this.Height = 600;
            this.KeyDown += new KeyEventHandler(Game_KeyDown);

        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            if (GameStart == false)
            {
                GameStart = true;
                GameTimer.Start();
                Scoreboard.Text = "Your Score is: " + Score.ToString();
            }
            switch (e.KeyCode)
            {
                case Keys.Right:
                    tempVer = 0;
                    tempHor = snakeSpeed;
                    tempHeadDirection = "R";
                    break;
                case Keys.Left:
                    tempVer = 0;
                    tempHor = -snakeSpeed;
                    tempHeadDirection = "L";
                    break;
                case Keys.Down:
                    tempVer = snakeSpeed;
                    tempHor = 0;
                    tempHeadDirection = "D";
                    break;
                case Keys.Up:
                    tempVer = -snakeSpeed;
                    tempHor = 0;
                    tempHeadDirection = "U";
                    break;

            }
        }



        private void InitializeGameArea()
        {
            Area = new GameArea();
            Area.Left = 100;
            Area.Top = 100;
            
            this.Controls.Add(Area); 

        }

        
        private void InitializeSnake()
        {
            
            Snake.Add(new SnakePixel());
            this.Controls.Add(Snake[0]);
            Snake[0].BringToFront();
            Snake[0].Left = 200;
            Snake[0].Top = 200;

        }
    }
}
