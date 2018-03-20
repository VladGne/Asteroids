using System;
using System.Drawing;
using System.Windows.Forms;

namespace Asteroids
{
    class Ship : BaseObject
    {
        private int _health = 3;
        public int Health => _health;
        public static event Message MessageDie;
        public bool _immune = false;
        Image img = Properties.Resources.shp;
        Timer ImmuneTimer = new Timer { Interval = 3000 };
        
        // Take damage
        public void HealthLow()
        {
            ImmuneTimer.Tick += new EventHandler(ImmuneTimer_Tick);
            ImmuneTimer.Start();
            _health--;
            _immune = true;                             // Ship immune for 3 sec
            img = Properties.Resources.damagedship;           
        }

        // Stopping ship immune
        private void ImmuneTimer_Tick(object sender, EventArgs e)
        {
            _immune = false;
            ImmuneTimer.Stop();
            img = Properties.Resources.shp;
        }

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {           
        }

        // Draw ship
        public override void Draw()
        {                               
            Game.Buffer.Graphics.DrawImage(img, Pos.X, Pos.Y, Size.Width, Size.Height);          
        }

        public override void Update()
        {
        }

        // Move UP
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }

        // Move DOWN
        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }
              
        // Death when zero health
        public void Die()
        {
            _health = 3;
            MessageDie?.Invoke();
            if(Game.score> Program.BestScore)
                Program.BestScore = Game.score;          
        }
    }
}
