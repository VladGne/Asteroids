﻿using System.Drawing;

namespace Asteroids
{
    class Star : BaseObject
    {
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size) { }

        // Draw star
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X, Pos.Y, Pos.X + Size.Width, Pos.Y + Size.Height);
            Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width, Pos.Y, Pos.X, Pos.Y + Size.Height);
        }

        // Change star position
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X; // change position

            //when left the window
            if (Pos.X < 0)
                Pos.X = Game.Width + Size.Width; // spawn on the right side of the window
        }
    }
}
