#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
#endregion

namespace fishboy
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields
        ContentManager content;
        MenuEntry soundMenuEntry;
        MenuEntry soundFxMenuEntry;
        private bool sound;
        private bool soundFx;
        Song music;
        

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("")
        {
            // Create our menu entries.
            soundMenuEntry = new MenuEntry(string.Empty);
            soundFxMenuEntry = new MenuEntry(string.Empty);
            // Hook up menu event handlers.
            soundMenuEntry.Selected += SoundMenuEntrySelected;
            soundFxMenuEntry.Selected += SoundFxMenuEntrySelected;

            
            // Add entries to the menu.
            MenuEntries.Add(soundMenuEntry);
            MenuEntries.Add(soundFxMenuEntry);

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
            base.LoadContent();
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            music = content.Load<Song>("menu");
            SetMenuEntryText();

        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }



        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            //music

            sound = (bool)IsolatedStorageSettings.ApplicationSettings["sound"];
            soundMenuEntry.Text = "Sound: " + (sound ? "on" : "off");


            if (sound)
            {
                if (!MediaPlayer.IsRepeating)
                {
                    MediaPlayer.Play(music);
                    MediaPlayer.IsRepeating = true;
                }
            }
            else 
            {
                MediaPlayer.Stop();
                MediaPlayer.IsRepeating = false;
            }

            //effects
            soundFx = (bool)IsolatedStorageSettings.ApplicationSettings["soundFx"];
            soundFxMenuEntry.Text = "Effects: " + (soundFx ? "on" : "off");

            IsolatedStorageSettings.ApplicationSettings.Save();
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void SoundMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            sound = !(bool)IsolatedStorageSettings.ApplicationSettings["sound"];
            IsolatedStorageSettings.ApplicationSettings["sound"] = sound;
            IsolatedStorageSettings.ApplicationSettings.Save();
            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void SoundFxMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            soundFx = !(bool)IsolatedStorageSettings.ApplicationSettings["soundFx"];
            IsolatedStorageSettings.ApplicationSettings["soundFx"] = soundFx;
            IsolatedStorageSettings.ApplicationSettings.Save();
            SetMenuEntryText();
        }

        #endregion
    }
}
