using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Leboeuf.SnakeExtension
{
    public enum MovingDirection
    {
        Up,
        Down,
        Left,
        Right
    };

    public static class SnakeGame
    {
        #region Default values
        private static readonly int SnakeSectionSize = 10;
        private static readonly int StartingSnakeSections = 5;
        private static readonly Brush DefaultSnakeColor = Brushes.Green;
        private static readonly Brush DefaultFoodColor = Brushes.Red;
        private static readonly Point StartingPosition = new Point(100, 100);
        private static readonly MovingDirection DefaultDirection = MovingDirection.Right;
        private static readonly TimeSpan DefaultTimerInterval = TimeSpan.FromMilliseconds(90);
        #endregion

        #region Game state
        private static List<Point> SnakeSections = new List<Point>();
        private static Point FoodPoint;
        private static int FoodEaten;

        private static Brush SnakeColor;
        private static Brush FoodColor;
        private static Point SnakeCurrentPosition;
        private static MovingDirection SnakeCurrentDirection;
        private static MovingDirection SnakePreviousDirection;
        #endregion

        private static Canvas Canvas;
        private static DispatcherTimer timer = new DispatcherTimer();
        private static Random rnd = new Random();

        public static void Initialize(Canvas canvas)
        {
            Canvas = canvas;
            timer.Tick += Tick;
        }

        public static void ChangeDirection(MovingDirection direction)
        {
            switch (direction)
            {
                case MovingDirection.Down:
                    if (SnakePreviousDirection != MovingDirection.Up)
                        SnakeCurrentDirection = MovingDirection.Down;
                    break;

                case MovingDirection.Up:
                    if (SnakePreviousDirection != MovingDirection.Down)
                        SnakeCurrentDirection = MovingDirection.Up;
                    break;

                case MovingDirection.Left:
                    if (SnakePreviousDirection != MovingDirection.Right)
                        SnakeCurrentDirection = MovingDirection.Left;
                    break;

                case MovingDirection.Right:
                    if (SnakePreviousDirection != MovingDirection.Left)
                        SnakeCurrentDirection = MovingDirection.Right;
                    break;
            }

            SnakePreviousDirection = direction;
        }

        public static void Stop()
        {
            timer.Stop();
        }

        public static void Reset()
        {
            FoodEaten = 0;
            SnakeColor = DefaultSnakeColor;
            FoodColor = DefaultFoodColor;
            SnakeCurrentPosition = StartingPosition;
            SnakeCurrentDirection = SnakePreviousDirection = DefaultDirection;

            Canvas.Children.Clear();
            SnakeSections.Clear();

            PaintSnake();
            PaintFood();

            timer.Interval = DefaultTimerInterval;
            timer.Start();
        }

        private static void Tick(object sender, EventArgs e)
        {
            switch (SnakeCurrentDirection)
            {
                case MovingDirection.Down:
                    SnakeCurrentPosition.Y += SnakeSectionSize;
                    break;
                case MovingDirection.Up:
                    SnakeCurrentPosition.Y -= SnakeSectionSize;
                    break;
                case MovingDirection.Left:
                    SnakeCurrentPosition.X -= SnakeSectionSize;
                    break;
                case MovingDirection.Right:
                    SnakeCurrentPosition.X += SnakeSectionSize;
                    break;
            }

            PaintSnake();

            // Collision detection for walls
            if ((SnakeCurrentPosition.X + SnakeSectionSize <= 1) || (SnakeCurrentPosition.X >= 309) ||
                (SnakeCurrentPosition.Y + SnakeSectionSize <= 1) || (SnakeCurrentPosition.Y >= 309))
                GameOver();

            // Collision detection for food 
            if ((Math.Abs(FoodPoint.X - SnakeCurrentPosition.X) < SnakeSectionSize) &&
                (Math.Abs(FoodPoint.Y - SnakeCurrentPosition.Y) < SnakeSectionSize))
            {
                // Add a snake section
                FoodEaten++;

                // Erase the food object from the canvas
                Canvas.Children.RemoveAt(0);
                PaintFood();

                // Accelerate the game
                if (timer.Interval.Milliseconds > 30)
                    timer.Interval = timer.Interval.Add(-TimeSpan.FromMilliseconds(8));
            }

            // Collision detection for the snake's body
            for (int q = 0; q < (SnakeSections.Count - SnakeSectionSize * 2); q++)
            {
                Point point = new Point(SnakeSections[q].X, SnakeSections[q].Y);
                if ((Math.Abs(point.X - SnakeCurrentPosition.X) < (SnakeSectionSize)) &&
                     (Math.Abs(point.Y - SnakeCurrentPosition.Y) < (SnakeSectionSize)))
                {
                    GameOver();
                    break;
                }
            }
        }

        private static void PaintSnake()
        {
            var newEllipse = new Ellipse();
            newEllipse.Fill = SnakeColor;
            newEllipse.Width = SnakeSectionSize;
            newEllipse.Height = SnakeSectionSize;

            Canvas.SetTop(newEllipse, SnakeCurrentPosition.Y);
            Canvas.SetLeft(newEllipse, SnakeCurrentPosition.X);

            Canvas.Children.Add(newEllipse);
            SnakeSections.Add(SnakeCurrentPosition);

            // Restrict the tail of the snake
            if (Canvas.Children.Count > StartingSnakeSections + FoodEaten + 1)
            {
                SnakeSections.RemoveAt(0); // Remove tailing snake section
                Canvas.Children.RemoveAt(1); // 0 is food slot
            }
        }

        private static void PaintFood()
        {
            FoodPoint = new Point(rnd.Next(5, 300), rnd.Next(5, 300));

            Ellipse newEllipse = new Ellipse();
            newEllipse.Fill = FoodColor;
            newEllipse.Width = SnakeSectionSize;
            newEllipse.Height = SnakeSectionSize;

            Canvas.SetTop(newEllipse, FoodPoint.Y);
            Canvas.SetLeft(newEllipse, FoodPoint.X);
            Canvas.Children.Insert(0, newEllipse);
        }

        private static void GameOver()
        {
            timer.Stop();
            Reset();
        }
    }
}
