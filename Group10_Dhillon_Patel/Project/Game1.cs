using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Project.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont gameFont;
        private SpriteFont infoFont;
        private SpriteFont bodyFont;
        private SpriteFont bodyFont2;
        private SpriteFont titleFont;
        private Texture2D healthBar;
        private Texture2D infoPageBackground;
        private Texture2D AboutPageBackground;
        private Texture2D coder1;
        private Texture2D coder2;
        private Texture2D arrowUp;
        private Texture2D arrowLeft;
        private Texture2D arrowRight;
        private Texture2D arrowDown;
        private Texture2D hKey;
        private Texture2D mKey;
        private Texture2D escKey;
        private Texture2D enterKey;
        private Texture2D spaceBar;
        private MainMenu _mainMenu;
        public bool _isPlaying;
        private int _currentLevel = 0;
        private string _currentState = "Menu";
        private int selectedLevel =1;
        private SoundEffect MainMenuBackgroundSound;
        private SoundEffect GameOverMenuBackgroundSound;

        private Texture2D _playerTank;
        private Vector2 _playerTankPosition;
        private Vector2 _playerTankVelocity = Vector2.Zero;
        private Texture2D _background;
        private float _playerTankRotation = 0f; // The rotation angle of the tank (in radians)
        private float _playerTankRotationSpeed = 0.05f; // Speed of rotation
        private float _playerTankSpeed = 2.0f;     // Speed of movement
        private Vector2 _tankPosition;
        bool hasRotated = false; //checking for rotation
        Vector2 tankOrigin;
        Song _moveSound;
        public int playerTankHealth = 5;
        bool isDead = false;
        private DateTime lastHitTime = DateTime.MinValue; // Tracks the last time health was reduced
        private readonly TimeSpan cooldownPeriod = TimeSpan.FromSeconds(2);

        //For Bullet Class
        private List<Bullet> bullets;
        private Texture2D _bullet;
        private float bulletRotation = 0f; //Roation angle for bullet
        private KeyboardState previousKeyboardState; //setting the keyboard state before and after an event (spawning bullet) will help control
                                                     //that evvent so only one bullet gets spawned with one space press.
        SoundEffect bulletFireSound;

        //For Wall Class
        List<Wall> walls = new List<Wall>();
        SoundEffect explosionSound;
        private Texture2D gameWall;

        
        private Texture2D powerUpTexture;

        //For EnemyTank Class
        private List<EnemyTank> enemyTanks = new List<EnemyTank>();
        private Texture2D _enemyTank;
        public float Rotation;      // Rotation angle
        int enemyTankCount;
        int wallCount;
        private bool _isMusicPlaying = false;
        int playerScore = 0;
        int currentLevel = 0;
        private bool _isNameEntered = false;
        private string _nameInput = "";
        private string _playerName;

        private Texture2D[] explosionFrames; // Array to store explosion frames
        private int currentFrame; // Current frame of the explosion animation
        private float frameTime; // Time between frames
        private float timeSinceLastFrame; // Timer to track the time elapsed
        private bool isExplosionActive; // Flag to track if explosion is active
        private Vector2 explosionPosition;

        private Texture2D[] destructionFrames; // Array to store destruction frames
        private int currentDestructionFrame; // Current frame of the destruction animation
        private float destructionFrameTime; // Time between frames for destruction animation
        private float destructionTimeSinceLastFrame; // Timer to track time for the next frame
        private bool isDestructionActive; // Flag to track if destruction animation is active
        private Vector2 destructionPosition; // Position where the destruction occurs

        private int _playerTurnsLeft = 3;
        private bool level2Yes = false;
        private bool level3Yes = false;

        private float activeTime = 5f; // Object is visible for 5 seconds
        private float inactiveTime = 3f; // Object is invisible for 3 seconds
        private float timer = 0f; // Tracks time
        private bool isVisible = true;
        private Point powerUpPosition = new Point(50, 50);

        private Random random = new Random();


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1300,
                PreferredBackBufferHeight = 900,
                IsFullScreen = false
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            bullets = new List<Bullet>();
            previousKeyboardState = Keyboard.GetState();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _mainMenu = new MainMenu();
            _currentLevel=0;
            _isPlaying = false;
            _playerTankPosition = new Vector2(Random.Shared.Next(100, 1200), Random.Shared.Next(100, 800));
            _playerTankRotation = 0f;
            bulletRotation = 0f;
            base.Initialize();
            level2Yes = false;
            level3Yes = false;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _mainMenu.LoadContent(Content);

            // TODO: use this.Content to load your game content here
            gameFont = Content.Load<SpriteFont>("Fonts/GameFont");
            infoFont = Content.Load<SpriteFont>("Fonts/HelpPageFont");
            bodyFont = Content.Load<SpriteFont>("Fonts/BodyFont");
            bodyFont2 = Content.Load<SpriteFont>("Fonts/BodyFont2");
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");
            _playerTank = Content.Load<Texture2D>("Photos/Tank1");
            _background = Content.Load<Texture2D>("Photos/Background");
            infoPageBackground = Content.Load<Texture2D>("Photos/InfoPageBackground");
            AboutPageBackground = Content.Load<Texture2D>("Photos/AboutPage2");
            coder1 = Content.Load<Texture2D>("Photos/Coder1");
            coder2 = Content.Load<Texture2D>("Photos/Coder2");
            arrowUp = Content.Load<Texture2D>("Photos/upArrow");
            arrowLeft = Content.Load<Texture2D>("Photos/leftArrow");
            arrowRight = Content.Load<Texture2D>("Photos/rightArrow");
            arrowDown = Content.Load<Texture2D>("Photos/downArrow");
            mKey = Content.Load<Texture2D>("Photos/mKey");
            hKey = Content.Load<Texture2D>("Photos/hKey");
            escKey = Content.Load<Texture2D>("Photos/escKey");
            enterKey = Content.Load<Texture2D>("Photos/enterKey");
            spaceBar = Content.Load<Texture2D>("Photos/spaceBar");
            _bullet = Content.Load<Texture2D>("Photos/Bullet");
            healthBar = Content.Load<Texture2D>("Photos/HealthBar");
            _moveSound = Content.Load<Song>("Audio/EngineRunning");
            MediaPlayer.IsRepeating = true;
            bulletFireSound = Content.Load<SoundEffect>("Audio/BulletFireSound");
            MainMenuBackgroundSound = Content.Load<SoundEffect>("Audio/MainBackgorundMusic");
            GameOverMenuBackgroundSound = Content.Load<SoundEffect>("Audio/GameOverBackground");
            explosionSound = Content.Load<SoundEffect>("Audio/TankFireSound");
            gameWall = Content.Load<Texture2D>("Photos/Wall1");
            powerUpTexture = Content.Load<Texture2D>("Photos/PowerUp");

            _enemyTank = Content.Load<Texture2D>("Photos/Tank1");
            foreach (var enemyTank in enemyTanks)
            {
                enemyTank.Texture = Content.Load<Texture2D>("Photos/Tank1");
                
            }
            explosionFrames = new Texture2D[]
          {
               Content.Load<Texture2D>("Photos/explosion1"),
               Content.Load<Texture2D>("Photos/explosion2"),
               Content.Load<Texture2D>("Photos/explosion3"),
               Content.Load<Texture2D>("Photos/explosion4"),
               Content.Load<Texture2D>("Photos/explosion5"),
               Content.Load<Texture2D>("Photos/explosion6"),
               Content.Load<Texture2D>("Photos/explosion7"),
          };

            currentFrame = 0;
            frameTime = 0.07f; // Set the time per frame (adjust this value for faster/slower animation)
            timeSinceLastFrame = 0f;
            isExplosionActive = false;

            destructionFrames = new Texture2D[]
            {
                Content.Load<Texture2D>("Photos/TankExplosion1"),
                Content.Load<Texture2D>("Photos/TankExplosion2"),
                Content.Load<Texture2D>("Photos/TankExplosion3"),
                Content.Load<Texture2D>("Photos/TankExplosion4"),
                Content.Load<Texture2D>("Photos/TankExplosion5"),
                Content.Load<Texture2D>("Photos/TankExplosion6"),
                Content.Load<Texture2D>("Photos/TankExplosion7"),
                Content.Load<Texture2D>("Photos/TankExplosion8"),
                Content.Load<Texture2D>("Photos/TankExplosion9"),

            };

            currentDestructionFrame = 0;
            destructionFrameTime = 0.07f; // Set time between frames (adjust as necessary)
            destructionTimeSinceLastFrame = 0f;
            isDestructionActive = false;
        }
        private Rectangle GetTankBounds(Vector2 position, Texture2D tankTexture)
        {
            return new Rectangle((int)(position.X - tankTexture.Width / 2), (int)(position.Y - tankTexture.Height / 2),
                                  tankTexture.Width, tankTexture.Height);
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            level2Yes = false;

            KeyboardState currentKeyboardState = Keyboard.GetState();
            if (_currentState == "Menu")
            {
                // Update the main menu (e.g., level selection logic)
                _mainMenu.Update(gameTime, currentKeyboardState, ref _currentState, ref _currentLevel);
                if (!_isMusicPlaying)
                {
                    MainMenuBackgroundSound.Play();
                    _isMusicPlaying = true;

                }

            }
            else if (_currentState == "Playing")
            {
                if (_isMusicPlaying)
                {
                    _isMusicPlaying = false;
                }
                if (!_isPlaying)
                {
                    // Handle the Welcome Menu
                    _mainMenu.Update(gameTime, currentKeyboardState, ref _currentState, ref _currentLevel);
                    if (_mainMenu.IsLevelSelected)
                    {
                        // Load the selected level
                        _currentLevel = _mainMenu.SelectedLevel;
                        currentLevel = _currentLevel;
                        InitializeEnemyTanks(_currentLevel);
                        InitializeWalls(_currentLevel);
                        _isPlaying = true; // Transition to gameplay
                       
                    }
                }
                else if (!isDead && enemyTankCount > 0)
                {
                    if (_currentLevel == 2)
                    {
                        level2Yes = true;
                    }
                    if (_currentLevel == 3)
                    {
                        level3Yes = true;
                    }
                    if (_isMusicPlaying)
                    {
                        _isMusicPlaying = false;
                    }
                    int height = GraphicsDevice.Viewport.Height;
                    int width = GraphicsDevice.Viewport.Width;

                    float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _playerTankVelocity = Vector2.Zero;
                    Vector2 direction = new Vector2((float)Math.Cos(_playerTankRotation), (float)Math.Sin(_playerTankRotation));
                    bool isMoving = false;
                    bool isMusicPlaying = false;
                    Vector2 previouosPosition = _playerTankPosition;
                    if (currentKeyboardState.IsKeyDown(Keys.Up))
                    {
                        _playerTankVelocity = new Vector2(0, -5);
                        isMoving = true;
                        if (!hasRotated)
                        {
                            _playerTankRotation = 0f;
                            _playerTankRotation += 600f;
                            bulletRotation = 0f;
                            bulletRotation += 600f;
                            hasRotated = true; // Mark rotation as done
                        }
                    }
                    else hasRotated = false;

                    if (currentKeyboardState.IsKeyDown(Keys.Down))
                    {
                        _playerTankVelocity = new Vector2(0, 3);
                        isMoving = true;
                        if (!hasRotated)
                        {
                            _playerTankRotation = 0f;
                            bulletRotation = 0f;
                            hasRotated = true; // Mark rotation as done
                        }
                    }
                    else hasRotated = false;

                    if (currentKeyboardState.IsKeyDown(Keys.Left))
                    {
                        _playerTankVelocity = new Vector2(-5, 0);
                        isMoving = true;
                        if (!hasRotated)
                        {
                            _playerTankRotation = 0f;
                            _playerTankRotation -= 300f;
                            bulletRotation = 0f;
                            bulletRotation -= 300f;
                            hasRotated = true; // Mark rotation as done
                        }
                    }
                    else hasRotated = false;
                    if (currentKeyboardState.IsKeyDown(Keys.Right))
                    {
                        _playerTankVelocity = new Vector2(5, 0);
                        isMoving = true;
                        if (!hasRotated)
                        {
                            _playerTankRotation = 0f;
                            _playerTankRotation += 300f;
                            bulletRotation = 0f;
                            bulletRotation += 300f;
                            hasRotated = true; // Mark rotation as done
                        }
                    }
                    else hasRotated = false;

                    if (currentKeyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
                    {
                        bullets.Add(new Bullet(_playerTankPosition.X, _playerTankPosition.Y, _playerTankRotation)); // Add new bullet to the list
                        bulletFireSound.Play();
                    }
                    _playerTankPosition += _playerTankVelocity * _playerTankSpeed;


                    if (_playerTankPosition.X < 80) _playerTankPosition.X = 80; // Left edge
                    if (_playerTankPosition.X > width - _playerTank.Width)
                        _playerTankPosition.X = width - _playerTank.Width; // Right edge

                    if (_playerTankPosition.Y < 80) _playerTankPosition.Y = 80; // Top edge
                    if (_playerTankPosition.Y > height - _playerTank.Height)
                        _playerTankPosition.Y = (height - _playerTank.Height); // Bottom edge



                    if (isMoving && MediaPlayer.State == MediaState.Stopped)
                    {
                        MediaPlayer.Play(_moveSound); // Stops the music when not moving
                    }

                    if (!isMoving && MediaPlayer.State == MediaState.Playing)
                    {
                        MediaPlayer.Stop(); // Stops the music when not moving
                    }

                    //Update all active bullets
                    foreach (var bullet in bullets)
                    {
                        bullet.Update();
                    }

                    Rectangle playerTankBounds = GetTankBounds(_playerTankPosition, _playerTank);
                    for (int j = walls.Count - 1; j >= 0; j--)
                    {
                        Rectangle wallBounds = new Rectangle((int)(walls[j].Position.X),
                                                           (int)(walls[j].Position.Y), gameWall.Width, gameWall.Height);

                        if (playerTankBounds.Intersects(wallBounds))
                        {
                            _playerTankPosition = previouosPosition;
                            break;
                        }
                    }


                    for (int k = enemyTanks.Count - 1; k >= 0; k--)
                    {
                        Rectangle enemyTankBounds = GetTankBounds(enemyTanks[k].Position, _enemyTank);
                        if (playerTankBounds.Intersects(enemyTankBounds))
                        {
                            if (DateTime.Now - lastHitTime >= cooldownPeriod) // Only decrease health if not already processed
                            {
                                lastHitTime = DateTime.Now;
                                _playerTankPosition = previouosPosition;
                                playerTankHealth--;
                            }
                            break;
                        }

                    }

                    for (int i = bullets.Count - 1; i >= 0; i--)
                    {
                        bullets[i].Update(); // Move the bullet
                                             // Remove the bullet if it goes off-screen
                        if (Bullet.KeepBulletsInBounds(bullets, i, width, height))
                        {
                            bullets.RemoveAt(i); // Remove bullet from list
                        }
                        else
                        {
                            // Check if the bullet intersects the wall
                            Rectangle bulletBounds = new Rectangle((int)bullets[i].Position.X,
                                                                    (int)bullets[i].Position.Y, _bullet.Width, _bullet.Height);
                            for (int j = walls.Count - 1; j >= 0; j--)
                            {
                                Rectangle wallBounds = new Rectangle((int)(walls[j].Position.X),
                                                                   (int)(walls[j].Position.Y), gameWall.Width, gameWall.Height);

                                if (bulletBounds.Intersects(wallBounds))
                                {
                                    explosionSound.Play();
                                    bullets.RemoveAt(i); // Remove bullet
                                    walls.RemoveAt(j);   // Remove wall
                                    playerScore++;
                                    explosionPosition = bulletBounds.Center.ToVector2(); // Or use another position as needed
                                    isExplosionActive = true;
                                    break; // Exit the loop as the bullet is removed
                                }

                            }
                            for (int k = enemyTanks.Count - 1; k >= 0; k--)
                            {
                                Rectangle enemyTankBounds = GetTankBounds(enemyTanks[k].Position, _enemyTank);
                                if (bulletBounds.Intersects(enemyTankBounds))
                                {
                                    enemyTanks[k].Health--;
                                    explosionSound.Play();
                                    bullets.RemoveAt(i);
                                    if (enemyTanks[k].IsDead())
                                    {
                                        destructionPosition = enemyTankBounds.Center.ToVector2(); // Or use a different position
                                        isDestructionActive = true;
                                        playerScore += 2;
                                        enemyTankCount--;
                                        enemyTanks.RemoveAt(k);
                                    }
                                    break;
                                }
                            }

                        }
                    }

                    EnemyTank.checkCollision(enemyTanks, walls);
                    EnemyTank.KeepEnemyInBounds(enemyTanks, width, height); //to check and keep enemy tanks within screen 
                    previousKeyboardState = currentKeyboardState;
                    foreach (var enemyTank in enemyTanks)
                    {
                        enemyTank.Update();
                    }

                }
                if (enemyTankCount <= 0)
                {
                    isDead = true;
                    _currentState = "GameOver";
                }
                else if (playerTankHealth <= 0)
                {
                    if (level2Yes && _playerTurnsLeft > 0)
                    {
                        playerTankHealth = 5;
                        _playerTurnsLeft--;

                    } else if (level3Yes && _playerTurnsLeft > 0)
                    {
                        PowerUp powerUp1 = new PowerUp(new Vector2(50, 50), powerUpTexture);
                        playerTankHealth = 5;
                        _playerTurnsLeft--;

                    }
                    else
                    {
                        isDead = true;
                        _currentState = "GameOver";
                    }
                    
                }

            }
            else if (_currentState == "GameOver")
            {
                if (!_isMusicPlaying)
                {
                    GameOverMenuBackgroundSound.Play();
                    _isMusicPlaying = true;

                }

                KeyboardState _currentKeyboardState = Keyboard.GetState();

                if (!_isNameEntered)
                {

                    // Name input logic
                    foreach (var key in currentKeyboardState.GetPressedKeys())
                    {
                        if (previousKeyboardState == null || !previousKeyboardState.IsKeyDown(key))
                        {
                            if (key == Keys.Enter && _nameInput.Length > 0)
                            {
                                _playerName = _nameInput;  // Store the name globally
                                _isNameEntered = true;    // Mark name entry as complete
                            }
                            else if (key == Keys.Back && _nameInput.Length > 0)
                            {
                                _nameInput = _nameInput.Substring(0, _nameInput.Length - 1); // Delete last character
                            }
                            else if (_nameInput.Length < 20 && key >= Keys.A && key <= Keys.Z)
                            {
                                _nameInput += key.ToString(); // Add character
                            }
                        }
                    }
                }
                else
                {
                    UpdateGameOver(currentKeyboardState);
                }
                previousKeyboardState = _currentKeyboardState;
            }
            else if (_currentState == "Help" || _currentState == "About")
            {
                UpdateGameOver(currentKeyboardState);
            }
            if (isExplosionActive)
            {
                timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timeSinceLastFrame >= frameTime)
                {
                    currentFrame++;
                    timeSinceLastFrame = 0f;

                    // If all frames are displayed, reset the animation
                    if (currentFrame >= explosionFrames.Length)
                    {
                        currentFrame = 0; // Reset or stop the animation
                        isExplosionActive = false; // Stop the animation
                    }
                }
            }
            if (isDestructionActive)
            {
                destructionTimeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (destructionTimeSinceLastFrame >= destructionFrameTime)
                {
                    currentDestructionFrame++;
                    destructionTimeSinceLastFrame = 0f;

                    // If all frames are displayed, reset the animation
                    if (currentDestructionFrame >= destructionFrames.Length)
                    {
                        currentDestructionFrame = 0; // Reset or stop the animation
                        isDestructionActive = false; // Stop the animation
                    }
                }
            }
            Rectangle playerTankBounds1 = GetTankBounds(_playerTankPosition, _playerTank);
            // Update power-up bounds
            Rectangle powerUpBounds = new Rectangle(powerUpPosition.X, powerUpPosition.Y, 50, 50); 

            if (isVisible && playerTankBounds1.Intersects(powerUpBounds))
            {
                // Player collected the power-up
                playerTankHealth = 5; // Replenish health to full
                isVisible = false; // Make power-up disappear
                timer = 0f; // Reset timer for the next appearance
            }
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update the timer
            timer += elapsedTime;

            if (isVisible && timer >= activeTime)
            {
                // Switch to inactive state
                isVisible = false;
                timer = 0f; // Reset the timer
            }
            else if (!isVisible && timer >= inactiveTime)
            {
                // Switch back to active state
                isVisible = true;
                timer = 0f; // Reset the timer
                int randomX = random.Next(0, GraphicsDevice.Viewport.Width - 50); // Assuming 50 is the object's width
                int randomY = random.Next(0, GraphicsDevice.Viewport.Height - 50); // Assuming 50 is the object's height
                powerUpPosition = new Point(randomX, randomY);
            }


            base.Update(gameTime);

        }

        private void UpdateGameOver(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.N))
            {
                ResetCurrentGame();   
            }
            if (keyboardState.IsKeyDown(Keys.R))
            {
                ResetLevel();
            }
            if (keyboardState.IsKeyDown(Keys.M))
            {
                ResetGame();
            }
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                SaveScoreToFile();
            }
        }

        private void ResetLevel()
        {
                _currentState = "Playing";
                // Reset the player's health
                playerTankHealth = 5;

                // Reset the player's position
                _playerTankPosition = new Vector2(Random.Shared.Next(100, 1200), Random.Shared.Next(100, 800));
                playerScore = playerScore;

                // Mark the player as alive
                isDead = false;
                _currentLevel = 1;
                bullets.Clear();
                //enemyTanks.Clear(); 
                InitializeEnemyTanks(_currentLevel);

        }
        private void ResetGame()
        {
            _currentState = "Menu";
            // Reset the player's health
            playerTankHealth = 5;

            // Reset the player's position
            _playerTankPosition = new Vector2(Random.Shared.Next(100, 1200), Random.Shared.Next(100, 800));
            // Mark the player as alive
            isDead = false;
            _isPlaying = false;
            _currentLevel = 1;
            playerScore = 0;
            bullets.Clear();
            //enemyTanks.Clear();
            InitializeEnemyTanks(_currentLevel);
        }
        private void ResetCurrentGame()
        {
            _currentState = "Menu";
            // Reset the player's health
            playerTankHealth = 5;

            // Reset the player's position
            _playerTankPosition = new Vector2(Random.Shared.Next(100, 1200), Random.Shared.Next(100, 800));
            // Mark the player as alive
            isDead = false;
            _isPlaying = false;
            _currentLevel = 1;
            playerScore = playerScore;
            bullets.Clear();
            //enemyTanks.Clear();
            InitializeEnemyTanks(_currentLevel);
        }
        private void SaveScoreToFile()
        {
            string filePath = "leaderboard.txt";
            string scoreEntry = $"{_playerName}: {playerScore}";

            // Append the score to the file
            File.AppendAllText(filePath, scoreEntry + Environment.NewLine);
        }
        private List<Tuple<string, int>> LoadLeaderboard()
        {
            string filePath = "leaderboard.txt";
            List<Tuple<string, int>> leaderboard = new List<Tuple<string, int>>();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                    {
                        leaderboard.Add(new Tuple<string, int>(parts[0].Trim(), score));
                    }
                }
            }

            // Sort the leaderboard by score (descending)
            leaderboard = leaderboard.OrderByDescending(x => x.Item2).ToList();

            return leaderboard;
        }


        private bool IsOverlapping(Vector2 position, int buffer = 5)
        {
            foreach (var wall in walls)
            {
                Rectangle newWallBounds = new Rectangle(
                    (int)(position.X - gameWall.Width / 2),
                    (int)(position.Y - gameWall.Height / 2),
                    gameWall.Width,
                    gameWall.Height
                );
                Rectangle playerTankBounds = GetTankBounds(_playerTankPosition, _playerTank);
                if ((newWallBounds.Intersects(wall.Bounds))|| (newWallBounds.Intersects(playerTankBounds)))
                {
                    return true;
                }
            }
            return false;
        }

        private void InitializeWalls(int level)
        {
            walls.Clear(); 
            if (level == 1)
            {
                int startX = 200; // Starting x position
                int endX = 1000;   // Ending x position
                int increment = 200; // Increment value for each wall

                for (int x = startX; x <= endX; x += increment)
                {
                    walls.Add(new Wall(new Vector2(x, 0), gameWall));
                }
                int startY = 200; // Starting x position
                int endY = 1000;   // Ending x position
                for (int y = startY; y <= endY; y += increment)
                {
                    walls.Add(new Wall(new Vector2(5, y), gameWall));
                }


            }

            else if (level == 2)
            {
                int startX = 200; // Starting x position
                int endX = 1000;   // Ending x position
                int increment = 150; // Increment value for each wall

                for (int x = startX; x <= endX; x += increment)
                {
                    walls.Add(new Wall(new Vector2(x, 0), gameWall));
                }
                int startY = 200; // Starting x position
                int endY = 1000;   // Ending x position
                for (int y = startY; y <= endY; y += increment)
                {
                    walls.Add(new Wall(new Vector2(5, y), gameWall));
                }
                int startY2 = 200; // Starting x position
                int endY2 = 1000;   // Ending x position

                for (int y = startY2; y <= endY2; y += increment)
                {
                    walls.Add(new Wall(new Vector2(1250, y), gameWall));
                }
            }


            else if (level == 3)
            {
                int startX = 200; // Starting x position
                int endX = 1000;   // Ending x position
                int increment = 150; // Increment value for each wall

                for (int x = startX; x <= endX; x += increment)
                {
                    walls.Add(new Wall(new Vector2(x, 0), gameWall));
                }
                int startX2 = 200; // Starting x position
                int endX2 = 1000;   // Ending x position

                for (int x = startX2; x <= endX2; x += increment)
                {
                    walls.Add(new Wall(new Vector2(x, 500), gameWall));
                }
                int startY = 200; // Starting x position
                int endY = 600;   // Ending x position
                for (int y = startY; y <= endY; y += increment)
                {
                    walls.Add(new Wall(new Vector2(5, y), gameWall));
                }
                int startY2 = 200; // Starting x position
                int endY2 = 600;   // Ending x position

                for (int y = startY2; y <= endY2; y += increment)
                {
                    walls.Add(new Wall(new Vector2(1250, y), gameWall));
                }
            }
        }

        private void InitializeEnemyTanks(int level)
        {
            enemyTanks.Clear();
            if (level == 1)
            {
                enemyTankCount = 3;
                for (int i = 0; i < 3; i++)
                {
                    Vector2 enemyPosition = new Vector2(Random.Shared.Next(100, 1200), Random.Shared.Next(100, 800));
                    Vector2 enemyDirection = new Vector2(Random.Shared.Next(100, 1200), Random.Shared.Next(100, 800));
                    enemyTanks.Add(new EnemyTank(enemyPosition, enemyDirection, _enemyTank));
                }
            }
            else if (level == 2)
            {
                enemyTankCount = 5;
                for (int i = 0; i < 5; i++)
                {
                    Vector2 enemyPosition = new Vector2(Random.Shared.Next(100, 1200), Random.Shared.Next(100, 800));
                    Vector2 enemyDirection = new Vector2(Random.Shared.Next(100, 1200), Random.Shared.Next(100, 800));
                    enemyTanks.Add(new EnemyTank(enemyPosition, enemyDirection, _enemyTank));
                }
            }
            else if (level == 3)
            {
                enemyTankCount = 7;
                for (int i = 0; i < 7; i++)
                {
                    Vector2 enemyPosition = new Vector2(Random.Shared.Next(100, 1200), Random.Shared.Next(100, 800));
                    Vector2 enemyDirection = new Vector2(Random.Shared.Next(100, 1200), Random.Shared.Next(100, 800));
                    enemyTanks.Add(new EnemyTank(enemyPosition, enemyDirection, _enemyTank));
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(_background,
                                    new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), // Full screen
                                    Color.White);
            if (isExplosionActive)
            {
                _spriteBatch.Draw(explosionFrames[currentFrame], explosionPosition, null, Color.White, 0f, new Vector2(explosionFrames[currentFrame].Width / 2, explosionFrames[currentFrame].Height / 2), 1f, SpriteEffects.None, 0f);
            }
            if (isDestructionActive)
            {
                _spriteBatch.Draw(destructionFrames[currentDestructionFrame], destructionPosition, null, Color.White, 0f, new Vector2(destructionFrames[currentDestructionFrame].Width / 2, destructionFrames[currentDestructionFrame].Height / 2), 1f, SpriteEffects.None, 0f);
            }

            if (!_isPlaying)
            {

                if (_currentState == "Help")
                {
                    _spriteBatch.Draw(infoPageBackground,
                                    new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), // Full screen
                                    Color.White);
                    _spriteBatch.DrawString(titleFont, "Tank Wars!!", new Vector2(520, 100), Color.Black);
                    _spriteBatch.DrawString(bodyFont, "DON'T WORRY!! We've Got You!", new Vector2(450, 200), Color.Orange);
                    _spriteBatch.Draw(arrowUp, new Rectangle(540, 250, 35, 35), Color.White);
                    _spriteBatch.DrawString(bodyFont, "-Move Upwards", new Vector2(580, 250), Color.Turquoise);
                    _spriteBatch.Draw(arrowLeft, new Rectangle(255, 300, 35, 35), Color.White);
                    _spriteBatch.DrawString(bodyFont, "-Move Towards Left", new Vector2(295, 300), Color.Turquoise);
                    _spriteBatch.Draw(arrowRight, new Rectangle(725, 300, 35, 35), Color.White);
                    _spriteBatch.DrawString(bodyFont, "-Move Towards Right", new Vector2(770, 300), Color.Turquoise);
                    _spriteBatch.Draw(arrowDown, new Rectangle(540, 350, 35, 35), Color.White);
                    _spriteBatch.DrawString(bodyFont, "-Move Downwards", new Vector2(590, 350), Color.Turquoise);
                    _spriteBatch.Draw(spaceBar, new Rectangle(510, 430, 85, 35), Color.White);
                    _spriteBatch.DrawString(bodyFont, "-Fire At Enemy", new Vector2(600, 430), Color.Yellow);
                    _spriteBatch.Draw(escKey, new Rectangle(815, 510, 45, 35), Color.White);
                    _spriteBatch.DrawString(bodyFont, "Not that we like it but you can press        anytime to Exit!", new Vector2(365, 510), Color.YellowGreen);
                    _spriteBatch.Draw(mKey, new Rectangle(915, 560, 35, 35), Color.White);
                    _spriteBatch.DrawString(bodyFont, "Now that you are ready to fight and concquer the enemy, press       to go to the Main Menu!", new Vector2(145, 560), Color.YellowGreen);
                    
                }
                else if (_currentState == "About")
                {
                    _spriteBatch.Draw(AboutPageBackground,
                                   new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), // Full screen
                                   Color.White);
                    _spriteBatch.DrawString(titleFont, "Tank Wars!!", new Vector2(480, 100), Color.Black);
                    _spriteBatch.DrawString(bodyFont2, "Meet the two developers who scaled mountains (figuratively, or perhaps literally?)\n" +
                        " to bring this game to life! Our group, #10, proudly presents this game as part of our\n" +
                        " final project. While our previous projects included  a Q game, a calculator app, a\n" +
                        " mobile app, and, of course, the classic HelloWorld app that simply printed \n" +
                        " 'Hello, World', this project represents our biggest achievement yet.", new Vector2(100, 250), Color.AliceBlue);
                    _spriteBatch.Draw(coder1, new Rectangle(400, 500, 200, 200), Color.White);
                    _spriteBatch.Draw(coder2, new Rectangle(700, 500, 200, 200), Color.White);
                    _spriteBatch.Draw(mKey, new Rectangle(530, 750, 35, 35), Color.White);
                    _spriteBatch.DrawString(bodyFont, "Press       to go to the Main Menu!", new Vector2(450, 750), Color.AliceBlue);
                }
                else
                // Draw the Welcome Menu
                _mainMenu.Draw(_spriteBatch);
            }
            else
            {
                if (_currentState == "Menu")
                {
                    _mainMenu.Draw(_spriteBatch);
                }
                else if (_currentState == "Playing")
                {
                    DrawGame();
                }
                else if (_currentState == "GameOver")
                {
                    var leaderboard = LoadLeaderboard();
                    _spriteBatch.DrawString(gameFont, "GAME OVER!!", new Vector2(550, 300), Color.Orange);
                    _spriteBatch.DrawString(gameFont, "Leaderboard", new Vector2(50, 10), Color.Yellow);

                    int yPosition = 50;
                    foreach (var entry in leaderboard)
                    {
                        _spriteBatch.DrawString(gameFont, $"{entry.Item1}: {entry.Item2}", new Vector2(50, yPosition), Color.White);
                        yPosition += 40;  // Adjust spacing between leaderboard entries
                    }
                    if (!_isNameEntered)
                    {
                        // Display name entry prompt
                        _spriteBatch.DrawString(gameFont, "Enter your name and press Enter:", new Vector2(490, 350), Color.Yellow);
                        _spriteBatch.DrawString(gameFont, _nameInput, new Vector2(540, 400), Color.White);
                    }
                    else
                    {
                        // Show navigation instructions after name is entered
                        _spriteBatch.DrawString(gameFont, $"{_playerName}'s Score: {playerScore}", new Vector2(530, 350), Color.Turquoise);
                        _spriteBatch.DrawString(gameFont, "Press R to re-start this level.", new Vector2(490, 400), Color.Turquoise);
                        _spriteBatch.DrawString(gameFont, "Press N to start a new level.", new Vector2(490, 450), Color.Turquoise);
                        _spriteBatch.DrawString(gameFont, "Press Esc to exit.", new Vector2(540, 500), Color.Turquoise);
                    }
                }
               


            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
       
        private void DrawGame()
        {
            if (level2Yes)
            {
                _spriteBatch.DrawString(bodyFont, "Player Turns Left: " + _playerTurnsLeft, new Vector2(20, 50), Color.Turquoise);
            }
            if (level3Yes)
            {
                _spriteBatch.DrawString(bodyFont, "Player Turns Left: " + _playerTurnsLeft, new Vector2(50, 50), Color.Turquoise);
                if (isVisible)
                {
                    _spriteBatch.Draw(powerUpTexture, new Rectangle(powerUpPosition.X, powerUpPosition.Y, 50, 50), Color.White); // Draw power-up
                }

            }
            _spriteBatch.DrawString(bodyFont, "Score: " + playerScore, new Vector2(20, 20), Color.Turquoise);
            _spriteBatch.Draw(  _playerTank,
                                _playerTankPosition,
                                null,
                                Color.White,
                                _playerTankRotation,
                                new Vector2(_playerTank.Width / 2, _playerTank.Height / 2), 
                                1.0f,
                                SpriteEffects.None,
                                0f);

           
            // Display health above the player tank
            Vector2 healthPosition1 = new Vector2(
                _playerTankPosition.X + 10,  // Offset to center the health display
                _playerTankPosition.Y - (_playerTank.Height / 2 + 20) // Above the tank
            );

            Rectangle healthBarBackground1 = new Rectangle(
                (int)(_playerTankPosition.X - 20),
                (int)(_playerTankPosition.Y - (_playerTank.Height / 2 + 20)), // Slightly higher for the bar
                40,
                10
            );

            Rectangle healthBarForeground1 = new Rectangle(
                healthBarBackground1.X,
                healthBarBackground1.Y,
                (int)(40 * (playerTankHealth / 5f)), // Scale width by health percentage
                healthBarBackground1.Height
            );

            // Draw health bar background
            _spriteBatch.Draw(
                healthBar,
                healthBarBackground1,
                Color.Red
            );

            // Draw health bar foreground
            _spriteBatch.Draw(
                healthBar,
                healthBarForeground1,
                Color.Orange
            );
            foreach (var bullet in bullets)
            {
                _spriteBatch.Draw(
                    _bullet,
                    bullet.Position,
                    null,
                    Color.White,
                    bulletRotation,
                    new Vector2(3, 3), // Bullet size
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }
            foreach (var wall in walls)
            {
                _spriteBatch.Draw(
                   gameWall,
                   wall.Position,
                   null,
                   Color.White,
                        0f,
                   Vector2.Zero,
                   1f,
                   SpriteEffects.None,
                   0f
                  );
            }
            foreach (var enemyTank in enemyTanks)
            {
                _spriteBatch.Draw(
                    _enemyTank, // The texture for the enemy tank
                    enemyTank.Position,
                    null,
                    Color.Red, // Color tint for the enemy tank
                    enemyTank.Rotation,
                    new Vector2(_enemyTank.Width / 2, _enemyTank.Height / 2), // Origin
                    1.0f, // Scale
                    SpriteEffects.None,
                    0f // Layer depth
                );
                // Display health above the enemy tank
                Vector2 healthPosition = new Vector2(
                    enemyTank.Position.X + 10,  // Offset to center the health display
                    enemyTank.Position.Y - (_enemyTank.Height / 2 + 20) // Above the tank
                );

                Rectangle healthBarBackground = new Rectangle(
                    (int)(enemyTank.Position.X - 20),
                    (int)(enemyTank.Position.Y - (_enemyTank.Height / 2 + 20)), // Slightly higher for the bar
                    40,
                    10
                );

                Rectangle healthBarForeground = new Rectangle(
                    healthBarBackground.X,
                    healthBarBackground.Y,
                    (int)(40 * (enemyTank.Health / 5f)), // Scale width by health percentage
                    healthBarBackground.Height
                );

                // Draw health bar background
                _spriteBatch.Draw(
                    healthBar,
                    healthBarBackground,
                    Color.Red
                );

                // Draw health bar foreground
                _spriteBatch.Draw(
                    healthBar,
                    healthBarForeground,
                    Color.Orange
                );
            }
        }
    }
}
