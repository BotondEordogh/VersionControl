using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Week8KWKTJ6.Entitites
{
    class Present : Toy
    {
        public Color ribbon { get; set; }
        public Color box { get; set; }

        public SolidBrush PresentColor { get; private set; }

        public Present(Color color)
        {
            PresentColor = new SolidBrush(color);
        }

        protected override void DrawImage(Graphics g)
        {
            g.FillRectangle(Color.Yellow(),0,0, Width, Height);
            g.FillRectangle(Color.Red(), 3, 0, Width / 5, Height / 5);
            g.FillRectangle(Color.Red(), 0, 3, Width / 5, Height / 5);
        }
    }
}
