#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace fishboy
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;
        Texture2D fishTexture;
        Texture2D boyTexture;
        Texture2D bubbleTexture;
        Texture2D backgroundTexture;
        Texture2D heartTexture;
        List<Fish> fishes;
        Random rand = new Random();

        SpriteFont scoreFont;
        SpriteFont lifeFont;
        SpriteFont levelFont;
        int lifes = 4;
        int score;
        int level = 1;
        bool gameOver = false;

        Song theme;
        SoundEffect hit;

        Boy boy;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            
            fishTexture = content.Load<Texture2D>("fish");
            bubbleTexture = content.Load<Texture2D>("bubble");
            backgroundTexture = content.Load<Texture2D>("bg");
            scoreFont = content.Load<SpriteFont>("score");
            boyTexture = content.Load<Texture2D>("fishboy");
            heartTexture = content.Load<Texture2D>("heart");
            theme = content.Load<Song>("theme");
            hit = content.Load<SoundEffect>("hit");

            fishes = new List<Fish>();
            boy = new Boy(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, 100));

            MediaPlayer.Play(theme);
            // Coloca a música de fundo em loop infinito
            MediaPlayer.IsRepeating = true;
            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            fishes.Clear();
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive)
            {
                if (lifes == 0)
                    gameOver = true;

                //TODO: Formula para level
                //level = ?

                //boy
                boy.update(gameTime);
                var fishBoyRect = new Rectangle((int)boy.position.X, (int)boy.position.Y,
                        boyTexture.Width, boyTexture.Height);

                //cria peixes
                if (rand.NextDouble() > 0.99 / level)
                {
                    Vector2 pos = new Vector2(
                            ScreenManager.GraphicsDevice.Viewport.Width / 2,
                            ScreenManager.GraphicsDevice.Viewport.Height
                    );
                    fishes.Add(new Fish(pos, fishTexture));
                }



                for (int i = 0; i < fishes.Count; i++)
                {
                    fishes[i].update(gameTime, 0.10f * level);

                    if (fishes[i].hit(fishBoyRect, hit))
                    {
                        fishes.Remove(fishes[i]);
                        score += 10;

                    }

                }




            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // if the user pressed the back button, we return to the main menu
            PlayerIndex player;
            if (input.IsNewButtonPress(Buttons.Back, ControllingPlayer, out player))
            {
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new BackgroundScreen(), new MainMenuScreen());
            }
            else
            {
                // Otherwise move the player position.
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            //if (gameOver)
            spriteBatch.Begin();
            //bg
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, 
                ScreenManager.GraphicsDevice.Viewport.Height), Color.White);

            //hearts
            var lifePos = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 50, heartTexture.Width + 10);
            for (int i = 0; i < lifes; i++)
            {
                spriteBatch.Draw(heartTexture, lifePos, Color.White);
                lifePos.X += heartTexture.Width;
            }


            //score
            spriteBatch.DrawString(scoreFont, "SCORE", new Vector2(30, ScreenManager.GraphicsDevice.Viewport.Height - 60), Color.White);
            spriteBatch.DrawString(scoreFont, score.ToString(), new Vector2(150, ScreenManager.GraphicsDevice.Viewport.Height - 60), Color.Red);


            spriteBatch.DrawString(scoreFont, gameTime.ElapsedGameTime.Seconds.ToString(),
                new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height - 60),
                Color.Red);

            //boy
            boy.draw(spriteBatch, boyTexture);

            //peixes
            foreach (Fish fish in fishes)
            {
                fish.draw(spriteBatch);
            }

            //bolhas
            if (rand.NextDouble() > 0.5)
            {
                spriteBatch.Draw(bubbleTexture, new Rectangle(
                 rand.Next(0, ScreenManager.GraphicsDevice.Viewport.Width),
                 rand.Next(ScreenManager.GraphicsDevice.Viewport.Height - 120, ScreenManager.GraphicsDevice.Viewport.Height), 20, 20),
                 Color.White);
            }
            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(1f - TransitionAlpha);
        }


        #endregion
    }
}
