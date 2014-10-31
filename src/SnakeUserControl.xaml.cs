using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Leboeuf.SnakeExtension
{
    /// <summary>
    /// Interaction logic for SnakeUserControl.xaml
    /// </summary>
    public partial class SnakeUserControl : UserControl
    {
        public SnakeUserControl()
        {
            InitializeComponent();
            this.btnSettings.Focus(); // Set focus to any control in this window to grab keyboard input

            this.KeyDown += OnKeyDown;
            SnakeGame.Initialize(this.canvas);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.S:
                case Key.Down:
                    SnakeGame.ChangeDirection(MovingDirection.Down);
                    break;

                case Key.W:
                case Key.Up:
                    SnakeGame.ChangeDirection(MovingDirection.Up);
                    break;

                case Key.A:
                case Key.Left:
                    SnakeGame.ChangeDirection(MovingDirection.Left);
                    break;

                case Key.D:
                case Key.Right:
                    SnakeGame.ChangeDirection(MovingDirection.Right);
                    break;
            }
        }


        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Settings");
        }

        private void btnSettings_MouseEnter(object sender, MouseEventArgs e)
        {
            //(sender as Button).Opacity = 1;
        }

        private void btnSettings_MouseLeave(object sender, MouseEventArgs e)
        {
            //(sender as Button).Opacity = 0.2;
        }
    }
}