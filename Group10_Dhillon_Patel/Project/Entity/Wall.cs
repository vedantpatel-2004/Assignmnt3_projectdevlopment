using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Project.Entity
{
    public class Wall
    {
        public Vector2 Position; // Wall's position
        public Texture2D Texture; // Wall's texture



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

        public Wall(Vector2 position, Texture2D texture)
        {
            Position = position;
            Texture = texture;

            // Define the collision bounds based on the texture size
            //Bounds = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
        }
    }
}
