using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BruhSnakeTimeBois
{
    class Food: PictureBox
    {
        public Food()
        {
            InitializeFood();
        }

        private void InitializeFood()
        {
            this.BackColor = Color.Transparent;
            this.Height = 20;
            this.Width = 20;
        }

    }
}
