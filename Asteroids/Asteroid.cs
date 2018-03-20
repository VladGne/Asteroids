using System.Drawing;

namespace Asteroids
{
    class Asteroid : BaseObject
    {       
        private Image img = Properties.Resources.asteroid;

        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        // Draw asteroid
        public override void Draw()
        {                     
            Game.Buffer.Graphics.DrawImage(img, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
    }
}
