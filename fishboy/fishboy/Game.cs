#region File Description
//-----------------------------------------------------------------------------
// Game.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.GamerServices;
using System;
#endregion

namespace fishboy
{
    /// <summary>
    /// Sample showing how to manage different game states, with transitions
    /// between menu screens, a loading screen, the game itself, and a pause
    /// menu. This main game class is extremely simple: all the interesting
    /// stuff happens in the ScreenManager component.
    /// </summary>
    public class GameStateManagementGame : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        #endregion

        #region Initialization

        /// <summary>
        /// The main game constructor.
        /// </summary>
        public GameStateManagementGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;

            // you can choose whether you want a landscape or portait
            // game by using one of the two helper functions.
            //InitializePortraitGraphics();
            InitializeLandscapeGraphics();

            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            // attempt to deserialize the screen manager from disk. if that
            // fails, we add our default screens.
            if (!screenManager.DeserializeState())
            {
                //verifica som externo tocando
                IsMediaPlayerBusy();

                if (!IsolatedStorageSettings.ApplicationSettings.Contains("soundFx"))
                {
                    SetIsolatedStorageSettings("soundFx", true);
                }

                if (!IsolatedStorageSettings.ApplicationSettings.Contains("hiscore"))
                {
                    SetIsolatedStorageSettings("hiscore", 0);
                }
                // Activate the first screens.
                screenManager.AddScreen(new BackgroundScreen(), null);
                screenManager.AddScreen(new MainMenuScreen(), null);
            }
        }

        public bool IsMediaPlayerBusy()
        {
            if (MediaPlayer.GameHasControl)
            {
                IAsyncResult result = Guide.BeginShowMessageBox(
                    "The media player is currently playing.  Do you wish to stop it and continue?",
                    "Please answer YES or NO",
                    new string[] { "YES", "NO" },
                    0,
                    MessageBoxIcon.None,
                    null,
                    null
                );

                result.AsyncWaitHandle.WaitOne();

                int? choice = Guide.EndShowMessageBox(result);
                if (choice.HasValue)
                {
                    if(choice.Value == 0)
                    {
                        MediaPlayer.Stop();
                        SetIsolatedStorageSettings("playGameSound", true);
                        SetIsolatedStorageSettings("sound", true);
                        return true;
                    }
                    else
                    {
                        SetIsolatedStorageSettings("playGameSound", false);
                        SetIsolatedStorageSettings("sound", false);
                        return false;
                    }
                }
            }

            SetIsolatedStorageSettings("playGameSound", false);
            return false;
        }

        public static void SetIsolatedStorageSettings(string key, object value)
        {
            IsolatedStorageSettings isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            isolatedStore[key] = value;
            isolatedStore.Save();
        }



        protected override void OnExiting(object sender, System.EventArgs args)
        {
            // serialize the screen manager whenever the game exits
            screenManager.SerializeState();

            base.OnExiting(sender, args);
        }

        /// <summary>
        /// Helper method to the initialize the game to be a portrait game.
        /// </summary>
        private void InitializePortraitGraphics()
        {
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
        }

        /// <summary>
        /// Helper method to initialize the game to be a landscape game.
        /// </summary>
        private void InitializeLandscapeGraphics()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }

        #endregion
    }

    #region Entry Point

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static class Program
    {
        static void Main()
        {
            using (GameStateManagementGame game = new GameStateManagementGame())
            {
                game.Run();
            }
        }
    }

    #endregion
}
