using System.Drawing;

namespace Asteroids
{
    interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
    }

    // Base class for all objects
    abstract class BaseObject : ICollision
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;
        public delegate void Message();

        // Parametrs
        public BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }

        public abstract void Draw();

        // Change object position
        public virtual void Update()
        {            
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0)

            Pos.X = Game.Width + Size.Width;
        }

        public bool Collision(ICollision o) => o.Rect.IntersectsWith(Rect);
        public Rectangle Rect => new Rectangle(Pos, Size);
    }
}
