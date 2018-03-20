using System.Drawing;

namespace Asteroids
{
    // Ship laser bullet
    class Bullet : BaseObject
    {
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        //Draw bullet
        public override void Draw()
        {           
            Game.Buffer.Graphics.FillRectangle(Brushes.OrangeRed, Pos.X, Pos.Y, Size.Width, Size.Height);           
        }

        // Change bullet position
        public override void Update()
        {
            Pos.X = Pos.X + 5;
        }
    }
}
