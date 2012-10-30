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
        private bool sound = true;
        
        

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
            SetMenuEntryText();
            // Hook up menu event handlers.
            soundMenuEntry.Selected += SoundMenuEntrySelected;

            
            // Add entries to the menu.
            MenuEntries.Add(soundMenuEntry);

        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("sound"))
            {
                sound = (bool)IsolatedStorageSettings.ApplicationSettings["sound"];
            }
            else 
            {
                IsolatedStorageSettings.ApplicationSettings["sound"] = sound;
            }
            soundMenuEntry.Text = "Sound: " + (sound ? "on" : "off");
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


        #endregion
    }
}
