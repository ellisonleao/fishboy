#region File Description
//-----------------------------------------------------------------------------
// BackgroundScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;
#endregion

namespace fishboy
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class BackgroundScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        Texture2D backgroundTexture;
        Texture2D cloud1Texture;
        Texture2D cloud2Texture;
        Texture2D logoTexture;
        Texture2D bubbleTexture;
        

        Vector2 cloud1Pos;
        Vector2 cloud2Pos;

        Random rand;

        Song music;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        
        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            backgroundTexture = content.Load<Texture2D>("background");
            cloud1Texture = content.Load<Texture2D>("cloud1");
            cloud2Texture = content.Load<Texture2D>("cloud2");
            logoTexture = content.Load<Texture2D>("logo");
            

            bubbleTexture = content.Load<Texture2D>("bubblesky");

            music = content.Load<Song>("menu");

            cloud1Pos = new Vector2(400, 100);
            cloud2Pos = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 200, 100);

            rand = new Random();

            if (!IsolatedStorageSettings.ApplicationSettings.Contains("sound"))
            {
                IsolatedStorageSettings.ApplicationSettings["sound"] = true;
            }


            if ((bool)IsolatedStorageSettings.ApplicationSettings["sound"])
            {
                MediaPlayer.Play(music);
                MediaPlayer.IsRepeating = true;
            }
            else 
            {
                MediaPlayer.Stop();
            }

            if (!IsolatedStorageSettings.ApplicationSettings.Contains("soundFx"))
            {
                IsolatedStorageSettings.ApplicationSettings["soundFx"] = true;
            }

        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            
            //atualiza posicao das nuvens
            var vel1 = new Vector2(0.04f * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0.0f);
            var vel2 = new Vector2(0.07f * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0.0f);
            cloud1Pos -= vel1;
            cloud2Pos -= vel2;
            
            if (cloud1Pos.X < -cloud1Texture.Width)
                cloud1Pos.X = ScreenManager.GraphicsDevice.Viewport.Width + cloud1Texture.Width;
 
            if (cloud2Pos.X < -cloud2Texture.Width)
                cloud2Pos.X = ScreenManager.GraphicsDevice.Viewport.Width + cloud2Texture.Width + cloud1Texture.Width;


            base.Update(gameTime, otherScreenHasFocus, false);
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, fullscreen,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            spriteBatch.Draw(cloud1Texture, cloud1Pos, Color.White);
            spriteBatch.Draw(cloud2Texture, cloud2Pos, Color.White);

            //bolhas
            if (rand.NextDouble() > 0.5)
            {
                spriteBatch.Draw(bubbleTexture,
                    new Vector2(rand.Next(0, ScreenManager.GraphicsDevice.Viewport.Width), 
                        rand.Next(0, 85)), Color.White);
            }
            //logo
            spriteBatch.Draw(logoTexture, 
                new Vector2(ScreenManager.GraphicsDevice.Viewport.Width/2 - 180, 10), 
                Color.White);

            spriteBatch.End();
        }


        #endregion
    }
}
