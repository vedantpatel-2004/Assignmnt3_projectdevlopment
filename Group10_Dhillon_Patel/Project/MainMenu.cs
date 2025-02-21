using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class MainMenu
    {
        private SpriteFont _font;
        private SpriteFont _font2;
        private SpriteFont _font3;
        private int _selectedIndex = 0;
        private string[] _levels = { "Level 1", "Level 2", "Level 3" };
        private int _delayCounter = 0;
        private Texture2D mainMenuBackground;
        private Texture2D aKey;
        private Texture2D hKey;
        public bool IsLevelSelected { get; private set; }
        public int SelectedLevel { get; private set; }

        public MainMenu()
        {
            _selectedIndex = 0;
            IsLevelSelected = false;
        }
        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/MainMenuFont");
            _font2 = content.Load<SpriteFont>("Fonts/MainMenuFont2");
            _font3 = content.Load<SpriteFont>("Fonts/MainMenuFont3");
            aKey = content.Load<Texture2D>("Photos/aKey");
            hKey = content.Load<Texture2D>("Photos/hKey");
            mainMenuBackground = content.Load<Texture2D>("Photos/MainMenuBackground");
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, ref string gameState, ref int currentLevel)
        {

            // Navigate Down
            if (keyboardState.IsKeyDown(Keys.Down) && _delayCounter <= 0 && _selectedIndex < _levels.Length - 1)
            {
                _selectedIndex++; // Move to next level
                _delayCounter = 10; // Reset delay counter (10 frames delay)
            }

            // Navigate Up
            if (keyboardState.IsKeyDown(Keys.Up) && _delayCounter <= 0 && _selectedIndex > 0)
            {
                _selectedIndex--; // Move to previous level
                _delayCounter = 10; // Reset delay counter (10 frames delay)
            }

            // Reset delay and key press flags if no navigation keys are held
            if (keyboardState.IsKeyUp(Keys.Down) && keyboardState.IsKeyUp(Keys.Up))
            {
                _delayCounter = 0; // Reset delay counter
            }
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                IsLevelSelected = true;
                SelectedLevel = _selectedIndex + 1; // Convert zero-based index to 1-based level (1, 2, or 3)
                gameState = "Playing";
            }

            if (keyboardState.IsKeyDown(Keys.H))
            {
                gameState = "Help";
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                gameState = "About";
            }

        }



        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mainMenuBackground,
                                   new Rectangle(0, 0, 1300, 900), // Full screen
                                   Color.White);

                // Display the menu options
                for (int i = 0; i < _levels.Length; i++)
                {
                    Color color = (i == _selectedIndex) ? Color.YellowGreen : Color.PeachPuff;
                    spriteBatch.DrawString(_font3, "Welcome to Tank Wars!", new Vector2(310, 150), Color.Black);
                    spriteBatch.DrawString(_font2, "Please Select the Level you want to play", new Vector2(300, 250), Color.Orange);
                    spriteBatch.DrawString(_font, _levels[i], new Vector2(580, 300 + i * 30), color);
                    spriteBatch.DrawString(_font2, "Need Help with Controls?", new Vector2(450, 400), Color.Orange);
                    spriteBatch.Draw(hKey, new Rectangle(650, 450, 35, 35), Color.White);
                    spriteBatch.DrawString(_font, "Press  ", new Vector2(580, 450), Color.PeachPuff);
                    spriteBatch.DrawString(_font2, "Want to Know More About Us?", new Vector2(400, 490), Color.Orange);
                    spriteBatch.Draw(aKey, new Rectangle(650, 540, 35, 35), Color.White);
                    spriteBatch.DrawString(_font, "Press ", new Vector2(580, 540), Color.PeachPuff);
                }
            
        }
    }
}
