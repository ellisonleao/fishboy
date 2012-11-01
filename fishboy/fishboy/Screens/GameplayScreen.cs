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
using Microsoft.Devices.Sensors;
using System.Windows.Threading;
using System.IO.IsolatedStorage;
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
        const int TOTAL_FISHES = 100;
        ContentManager content;
        Texture2D fishTexture;
        Texture2D boyTexture;
        Texture2D bubbleTexture;
        Texture2D backgroundTexture;
        Texture2D heartTexture;
        Texture2D cloud1Texture;
        Texture2D cloud2Texture;
        Vector2 cloud1Pos;
        Vector2 cloud2Pos;
        List<Fish> fishes;
        List<Fish> toBeRemoved;
        Random rand = new Random();

        SpriteFont scoreFont;
        SpriteFont levelFont;

        int lifes;
        int score;
        int hiscore;
        int level;
        int fishQuantity;
        float fishVel;
        private float[] angles = { MathHelper.ToRadians(90), MathHelper.ToRadians(45), -MathHelper.ToRadians(45) };

        Song theme;
        SoundEffect hit;
        SoundEffect dead;

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

            score = 0;
            lifes = 4;
            fishVel = 0.10f;
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
            levelFont = content.Load<SpriteFont>("level");
            boyTexture = content.Load<Texture2D>("fishboy");
            heartTexture = content.Load<Texture2D>("heart");
            theme = content.Load<Song>("theme");
            hit = content.Load<SoundEffect>("hit");
            dead = content.Load<SoundEffect>("dead");
            cloud1Texture = content.Load<Texture2D>("cloud1");
            cloud2Texture = content.Load<Texture2D>("cloud2");

            fishes = new List<Fish>();
            toBeRemoved = new List<Fish>();


            boy = new Boy(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, 100), 
                          ScreenManager.GraphicsDevice.Viewport.Width, boyTexture);
            cloud1Pos = new Vector2(400, 100);
            cloud2Pos = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 200, 100);


            
            if (IsolatedStorageSettings.ApplicationSettings.Contains("sound"))
            {
                var sound = IsolatedStorageSettings.ApplicationSettings["sound"];
                if ((bool)sound)
                {
                    MediaPlayer.Play(theme);
                    MediaPlayer.IsRepeating = true;
                }
                else
                {
                    MediaPlayer.Stop();
                }

            }


            if (IsolatedStorageSettings.ApplicationSettings.Contains("hiscore"))
            {
                hiscore = (int)IsolatedStorageSettings.ApplicationSettings["hiscore"];
            }
            else 
            {
                hiscore = score;
            }
            CreateFishes();

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
        public void CreateFishes()
        {
            //cria peixes
            for (int i = 0; i <= TOTAL_FISHES; i++)
            {
                Vector2 pos = new Vector2(
                    rand.Next(fishTexture.Width, ScreenManager.GraphicsDevice.Viewport.Width - fishTexture.Width),
                    ScreenManager.GraphicsDevice.Viewport.Height);
                fishes.Add(new Fish(pos, fishTexture));
            }
        }



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
                level = score / 100 + 1;
               
                if (lifes <= 0) 
                {
                    //salva hiscore
                    if (hiscore < score)
                    {
                        IsolatedStorageSettings.ApplicationSettings["hiscore"] = score;
                        IsolatedStorageSettings.ApplicationSettings.Save();
                    }
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen());
                }
                    
                //nuvens
                var vel1 = new Vector2(0.04f * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0.0f);
                var vel2 = new Vector2(0.07f * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0.0f);
                cloud1Pos -= vel1;
                cloud2Pos -= vel2;

                if (cloud1Pos.X < -cloud1Texture.Width)
                    cloud1Pos.X = ScreenManager.GraphicsDevice.Viewport.Width + cloud1Texture.Width;

                if (cloud2Pos.X < -cloud2Texture.Width)
                    cloud2Pos.X = ScreenManager.GraphicsDevice.Viewport.Width + cloud2Texture.Width + cloud1Texture.Width;

                //boy
                boy.update(gameTime);

                if (level > 6)
                {
                    fishVel = 0.10f * (level / (level + 1));
                    fishQuantity = FibonacciFishes(6);
                }
                else
                {
                    fishQuantity = FibonacciFishes(level);
                }
                for (int i = 0; i < fishQuantity; i++)
                {
                        
                    fishes[i].update(gameTime, fishVel, dead);
                    if (fishes[i].isDead)
                    {
                        lifes--;
                        fishes[i].reboot(ScreenManager.GraphicsDevice.Viewport.Height, ScreenManager.GraphicsDevice.Viewport.Width);
                    }else if (fishes[i].hit(boy.getBounds(), hit))
                    {
                        score += 10;
                        fishes[i].reboot(ScreenManager.GraphicsDevice.Viewport.Height, ScreenManager.GraphicsDevice.Viewport.Width);
                    }
                }

            }
        }

        /// <summary>
        /// Determine the fish vel by level, using fibonnacci sequence
        /// </summary>
        public int FibonacciFishes(int level)
        {
            int[] sequence = new int[] { 1, 1, 2, 3, 5, 8 };
            int key = level - 1;
            return sequence[key];
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

            //hiscore
            spriteBatch.DrawString(scoreFont,"HISCORE " + hiscore.ToString(), new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 90, 20), Color.Red);
            

            //DEBUG
            spriteBatch.DrawString(scoreFont, fishQuantity.ToString(), new Vector2(20, 20), Color.Red);


            //hearts
            var lifePos = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 50, heartTexture.Width + 30);
            for (int i = 0; i < lifes; i++)
            {
                spriteBatch.Draw(heartTexture, lifePos, Color.White);
                lifePos.X += heartTexture.Width;
            }

            //clouds
            spriteBatch.Draw(cloud1Texture, cloud1Pos, Color.White);
            spriteBatch.Draw(cloud2Texture, cloud2Pos, Color.White);

            //score
            spriteBatch.DrawString(scoreFont, "SCORE", new Vector2(30, ScreenManager.GraphicsDevice.Viewport.Height - 60), Color.White);
            spriteBatch.DrawString(scoreFont, score.ToString(), new Vector2(150, ScreenManager.GraphicsDevice.Viewport.Height - 60), Color.Red);


            //level
            spriteBatch.DrawString(levelFont, "LEVEL " + level.ToString(), new Vector2( ScreenManager.GraphicsDevice.Viewport.Width- 350, 
                ScreenManager.GraphicsDevice.Viewport.Height - 60), Color.White);

            spriteBatch.DrawString(scoreFont, gameTime.ElapsedGameTime.Seconds.ToString(),
                new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height - 60),
                Color.Red);

            //boy
            boy.draw(spriteBatch);

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
