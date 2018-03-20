using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace Asteroids
{
    // Main game engine class
    class Game
    {      
        public static BufferedGraphics Buffer;
        public static Star[] _stars;
        public static int score = 0;
        public static Random Rnd = new Random();
        public static int Width { get; set; }
        public static int Height { get; set; }

        private static BufferedGraphicsContext _context;
        private static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(75, 75));
        private static SoundPlayer LaserSound = new SoundPlayer(Properties.Resources.Laser);
        private static SoundPlayer BlastSound = new SoundPlayer(Properties.Resources.Blast);
        private static SoundPlayer HitSound = new SoundPlayer(Properties.Resources.Hit);
        private static SoundPlayer StartSound = new SoundPlayer(Properties.Resources.Start);
        private static List<Bullet> _bullets = new List<Bullet>();
        private static Asteroid[] _asteroids;
        
        static Game() { }

        // Load game window
        public static void Load()
        {
            var randomizer = new Random();

            _stars = new Star[30];
            _asteroids = new Asteroid[20];

            if (Program.SoundMode == "On")
                StartSound.Play();

            // Create stars
            for (var i = 0; i < _stars.Length; i++)
            {
                int rand = randomizer.Next(5, 50);
                _stars[i] = new Star(new Point(1000, randomizer.Next(0, Height)), new Point(-rand, rand), new Size(3, 3));
            }

            // Create asteroids
            for (var i = 0; i < _asteroids.Length; i++)
            {
                int rand = randomizer.Next(5, 50);
                _asteroids[i] = new Asteroid(new Point(1000, randomizer.Next(0, Height)), new Point(-rand / 5, rand), new Size(rand, rand));
            }
        }

        // Stop Game
        public static void Finish()
        {
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
            _ship = null;           
        }       

        // Create main parametrs
        public static void Init(Form form)
        {
            Timer timer = new Timer { Interval = 100 };
            timer.Start();
            Graphics g;
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            Width = form.Width;
            Height = form.Height;
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();
            timer.Tick += Timer_Tick;
            form.KeyDown += Form_KeyDown;
            Ship.MessageDie += Finish;
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        // Draw all objects
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            Buffer.Graphics.DrawString("Best Score:" + Program.BestScore, SystemFonts.DefaultFont, Brushes.White, 150, 0);

            foreach (Star star in _stars)
                star.Draw();

            foreach (Asteroid asteroid in _asteroids)
                asteroid?.Draw();
            
            foreach (Bullet bullet in _bullets)
                bullet.Draw();
            _ship?.Draw();

            if (_ship != null)
            {
                Buffer.Graphics.DrawString("Health:" + _ship.Health, SystemFonts.DefaultFont, Brushes.White, 0, 0);
                Buffer.Graphics.DrawString("Score:" + score, SystemFonts.DefaultFont, Brushes.White, 55, 0);
            }
            Buffer.Render();
        }

        // Move positions and check collisions
        public static void Update()
        {
            // Move all stars position
            foreach (Star star in _stars)
                star.Update();

            // Move all bullets position
            foreach (Bullet bullet in _bullets)
                bullet.Update();

            for (int i = 0; i < _asteroids.Length; i++)
            {
                if (_asteroids[i] == null)
                    continue;

                _asteroids[i].Update(); // Move all bullets position

                for (int j = 0; j < _bullets.Count; j++)
                {
                    // When bullet hit asteroid
                    if (_asteroids[i] != null && _bullets[j].Collision(_asteroids[i]))
                    {    // Remove asteroid and bullet                 
                        _asteroids[i] = null;
                        _bullets.RemoveAt(j);
                        j--;

                        // Add score
                        score += 50;
                        Buffer.Graphics.DrawString("Score:" + score, SystemFonts.DefaultFont, Brushes.White, 55, 0);
                        if (Program.SoundMode == "On")
                            BlastSound.Play();
                    }
                }
                    
                
                if (_asteroids[i] == null || !_ship.Collision(_asteroids[i]) || _ship._immune)
                    continue;

                // When take damage
                if (Program.SoundMode == "On")
                    HitSound.Play();
                _ship.HealthLow();                  
                if (_ship.Health <= 0)
                    _ship.Die();
            }
        }

        // Move or fire
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            // Fire
            if (e.KeyCode == Keys.ControlKey)
            {
                _bullets.Add(new Bullet(new Point(_ship.Rect.X + 70, _ship.Rect.Y + 36), new Point(4, 0), new Size(5, 2)));
                if (Program.SoundMode == "On")
                    LaserSound.Play();
            }
                
            // Move up and down
            if (e.KeyCode == Keys.Up)
                _ship.Up();
            if (e.KeyCode == Keys.Down)
                _ship.Down();
        }
    }
}