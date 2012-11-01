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
using Microsoft.Xna.Framework.Input;
#endregion

namespace fishboy
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class GameOverScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        Texture2D backgroundTexture;
        Texture2D cloud1Texture;
        Texture2D cloud2Texture;
        Texture2D logoTexture;

        Vector2 cloud1Pos;
        Vector2 cloud2Pos;

        Random rand;

        Song music;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameOverScreen()
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

            backgroundTexture = content.Load<Texture2D>("bggameover");
            cloud1Texture = content.Load<Texture2D>("cloud1");
            cloud2Texture = content.Load<Texture2D>("cloud2");
            logoTexture = content.Load<Texture2D>("gameover");

            //bubbleTexture = content.Load<Texture2D>("bubblesky");
            //music = content.Load<Song>("menu");

            cloud1Pos = new Vector2(400, 100);
            cloud2Pos = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 200, 100);

            rand = new Random();

            MediaPlayer.Stop();
            //MediaPlayer.Play(music);
            //MediaPlayer.IsRepeating = true;
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
            cloud1Pos.X -= 0.04f * gameTime.ElapsedGameTime.Milliseconds;
            cloud2Pos.X -= 0.07f * gameTime.ElapsedGameTime.Milliseconds;

            if (cloud1Pos.X < 0)
                cloud1Pos.X = ScreenManager.GraphicsDevice.Viewport.Width + cloud1Texture.Width;
 
            if (cloud2Pos.X < 0)
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


            spriteBatch.Draw(logoTexture,
                new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 150, ScreenManager.GraphicsDevice.Viewport.Height / 2 - 150), 
                Color.White);

            spriteBatch.End();
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");


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


        #endregion
    }
}
