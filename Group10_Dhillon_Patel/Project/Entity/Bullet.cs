using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity
{
    public class Bullet
    {
        public Vector2 Position;
        public float Speed = 8f;
        public float Rotation;
        //private float bulletRotation = 0f;
        private bool hasRotated = false;
        public Vector2 Velocity;
        private Random random = new Random();

        public Bullet(float x, float y, float tankRotation) //to calculate bullet spawing position relative to tank
        {
            if (tankRotation == 0f) //tank facing down
            {
                Position = new Vector2(x, y + 48);
                Velocity = new Vector2(0, 8f);

            }
            if (tankRotation == 600f) // Tank facing up
            {
                Position = new Vector2(x, y - 48);
                Velocity = new Vector2(0, -8f);
            }
            if (tankRotation == 300f) // Tank facing right
            {
                Position = new Vector2(x + 48, y);
                Velocity = new Vector2(8f, 0);
            }
            if (tankRotation == -300f) //Tank facing left
            {
                Position = new Vector2(x - 48, y);
                Velocity = new Vector2(-8f, 0);
            }

        }

        public void Update()
        {
            Position += Velocity;
        }

        public static bool KeepBulletsInBounds(List<Bullet> bullets, int idx, int width, int height)
        {
            if (bullets[idx].Position.X < 0 || bullets[idx].Position.X > width ||
                    bullets[idx].Position.Y < 0 || bullets[idx].Position.Y > height)
            {
                return true; // Remove bullet from list
            }
            return false;
        }
    }
}
