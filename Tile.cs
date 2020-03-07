using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BruhSnakeTimeBois
{
    class Tile: PictureBox
    {
        public Tile()
        {
            InitializeTile();
        }
        private void InitializeTile()
        {
            this.Width = 20;
            this.Height = 20;
        }
    }
}
