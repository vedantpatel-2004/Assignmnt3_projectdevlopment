using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entity
{
    public class EnemyTank
    {
        public Vector2 Position;    // Current position of the enemy tank
        public float Rotation;      // Rotation angle
        public float Speed;         // Movement speed
        public Vector2 Velocity;    // Current velocity
        public Texture2D Texture;
        public Vector2 Direction;
        private Random _random;
        public int Health;


        public int CollisionCooldown { get; set; } = 0;

        public EnemyTank(Vector2 startPosition, Vector2 direction, Texture2D texture)
        {
            Position = startPosition;
            //Rotation = 0f;
            Texture = texture;
            Speed = 2f;
            Direction = direction;
            _random = new Random();
            SetRandomDirection();
            Health = 5;
        }
        public bool IsDead()
        {
            return Health <= 0;
        }
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)(Position.X - Texture.Width / 2),
                    (int)(Position.Y - Texture.Height / 2),
                    Texture.Width,
                    Texture.Height
                );
            }
        }
        public void Update()
        {
            // Move the tank in its current direction
            Position += Velocity;

            if (Velocity != Vector2.Zero)
            {
                Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) + 250f;
            }
        }

        private void SetRandomDirection()
        {
            // Generate a random angle for movement
            Rotation = (float)(_random.NextDouble() * 2); // Random angle in radians
            Velocity = new Vector2((float)Math.Cos(Rotation) * Speed, (float)Math.Sin(Rotation) * Speed);

        }
        public static void KeepEnemyInBounds(List<EnemyTank> enemyTanks, int width, int height)
        {
            for (int k = 0; k < enemyTanks.Count; k++)
            {
                if (enemyTanks[k] == null)
                {
                    Console.WriteLine($"EnemyTank at index {k} is null!");
                    continue;
                }

                // Check if the tank is out of bounds
                if (enemyTanks[k].Position.X < 0)
                {
                    enemyTanks[k].Position = new Vector2(0, enemyTanks[k].Position.Y); // Reset X position to 0                                                   
                }
                else if (enemyTanks[k].Position.X > width)
                {
                    enemyTanks[k].Position = new Vector2(width, enemyTanks[k].Position.Y); // Reset X position to width
                }

                if (enemyTanks[k].Position.Y < 0)
                {
                    enemyTanks[k].Position = new Vector2(enemyTanks[k].Position.X, 0); // Reset Y position to 0
                }
                else if (enemyTanks[k].Position.Y > height)
                {
                    enemyTanks[k].Position = new Vector2(enemyTanks[k].Position.X, height); // Reset Y position to height
                }

                if (enemyTanks[k].Position.X == 0 || enemyTanks[k].Position.X == width)
                {
                    enemyTanks[k].Velocity = new Vector2(-enemyTanks[k].Velocity.X, enemyTanks[k].Velocity.Y); // Reverse X velocity
                }

                if (enemyTanks[k].Position.Y == 0 || enemyTanks[k].Position.Y == height)
                {
                    enemyTanks[k].Velocity = new Vector2(enemyTanks[k].Velocity.X, -enemyTanks[k].Velocity.Y); // Reverse Y velocity
                }
            }
        }

        public static void checkCollision(List<EnemyTank> enemyTanks, List<Wall> walls)
        {
            for (int i = 0; i < enemyTanks.Count; i++)
            {
                for (int j = i + 1; j < enemyTanks.Count; j++)
                {
                    if (enemyTanks[i].Bounds.Intersects(enemyTanks[j].Bounds))
                    {
                        // Reverse directions of both tanks to avoid overlap
                        enemyTanks[i].Direction = new Vector2(-enemyTanks[i].Direction.X, -enemyTanks[i].Direction.Y);
                        enemyTanks[j].Direction = new Vector2(-enemyTanks[j].Direction.X, -enemyTanks[j].Direction.Y);

                        // Calculate separation vector to move tanks away from each other
                        Vector2 separationVector = enemyTanks[i].Position - enemyTanks[j].Position;
                        separationVector.Normalize(); // Ensure it's a unit vector

                        // Move tanks slightly away from each other based on their separation
                        enemyTanks[i].Position += separationVector * 10f; // Move 10 units away
                        enemyTanks[j].Position -= separationVector * 10f; // Move 10 units away
                    }
                }
                for (int j = walls.Count - 1; j >= 0; j--)
                {
                    //Rectangle wallBounds = new Rectangle((int)(walls[j].Position.X),
                    //                                   (int)(walls[j].Position.Y),  gameWall.Width, gameWall.Height);
                    Random _random = new Random();
                    if (enemyTanks[i].Bounds.Intersects(walls[j].Bounds))
                    {
                        if (enemyTanks[i].CollisionCooldown <= 0 && enemyTanks[i].Bounds.Intersects(walls[j].Bounds))
                        {
                            enemyTanks[i].Rotation = (float)(_random.NextDouble() * 2); // Random angle in radians
                            enemyTanks[i].Velocity = new Vector2((float)Math.Cos(enemyTanks[i].Rotation) * enemyTanks[i].Speed, (float)Math.Sin(enemyTanks[i].Rotation) * enemyTanks[i].Speed);
                            enemyTanks[i].CollisionCooldown = 10; // Set cooldown (e.g., 10 frames)
                        }
                        break;
                    }
                }

                if (enemyTanks[i].CollisionCooldown > 0)
                {
                    enemyTanks[i].CollisionCooldown--;
                }

            }
        }

    }
}
