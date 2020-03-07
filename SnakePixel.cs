using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BruhSnakeTimeBois
{
    class SnakePixel: PictureBox
    {
        public SnakePixel()
        {
            InitializeSnakePixel();
        }

        private void InitializeSnakePixel()
        {
            this.BackColor = Color.Green;
            this.Width = 20;
            this.Height = 20;
        }

    }
}
