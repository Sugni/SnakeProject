﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BruhSnakeTimeBois
{


    class GameArea: PictureBox
    {
        public GameArea()
        {
            InitializeGameArea();
        }



        private void InitializeGameArea()
        {
            this.BackColor = Color.Transparent;
            this.Width = 400;
            this.Height = 400;
        }

    }
}
