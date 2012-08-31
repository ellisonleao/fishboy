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

        static bool sound = true;


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
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
            soundMenuEntry.Text = "Sound: " + (sound ? "on" : "off");
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void SoundMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            sound = !sound;

            SetMenuEntryText();
        }


        #endregion
    }
}
