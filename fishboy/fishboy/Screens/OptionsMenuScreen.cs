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

        MenuEntry soundMenuEntry;
        MenuEntry soundFxMenuEntry;
        private bool sound = true;
        private bool soundFx = true;
        

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
            SetMenuEntryText();
            // Hook up menu event handlers.
            soundMenuEntry.Selected += SoundMenuEntrySelected;
            soundFxMenuEntry.Selected += SoundFxMenuEntrySelected;

            
            // Add entries to the menu.
            MenuEntries.Add(soundMenuEntry);
            MenuEntries.Add(soundFxMenuEntry);

        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            //music
            if (IsolatedStorageSettings.ApplicationSettings.Contains("sound"))
            {
                sound = (bool)IsolatedStorageSettings.ApplicationSettings["sound"];
            }
            else 
            {
                IsolatedStorageSettings.ApplicationSettings["sound"] = sound;
            }
            soundMenuEntry.Text = "Sound: " + (sound ? "on" : "off");


            //effects
            if (IsolatedStorageSettings.ApplicationSettings.Contains("soundFx"))
            {
                soundFx = (bool)IsolatedStorageSettings.ApplicationSettings["soundFx"];
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings["soundFx"] = sound;
            }
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
