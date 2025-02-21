using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity
{
    public class PowerUp
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture;
        public float Timer { get; set; } // Timer to track how long the power-up is active
        public bool IsActive { get; set; } // Flag to check if the power-up is still active

        public Rectangle Bounds
        {
            get
            {
                if (Texture == null)
                {
                    // Handle the error (e.g., return an empty rectangle or log a message)
                    return new Rectangle(0, 0, 0, 0);
                }

                return new Rectangle(
                    (int)(Position.X - Texture.Width / 2),
                    (int)(Position.Y - Texture.Height / 2),
                    Texture.Width,
                    Texture.Height
                );
            }
        }
        public PowerUp(Vector2 position, Texture2D texture)
        {
            Position = position;
            Texture = texture;

        }

    }

}