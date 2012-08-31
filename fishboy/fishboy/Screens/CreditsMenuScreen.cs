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
    class CreditsMenuScreen : MenuScreen
    {
        #region Fields
        private string creditsText = "Texto dos creditos aqui";
        MenuEntry creditsMenuEntry;
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public CreditsMenuScreen()
            : base("")
        {
            // Create our menu entries.
            creditsMenuEntry = new MenuEntry(creditsText);
            // Add entries to the menu.
            MenuEntries.Add(creditsMenuEntry);

        }


        #endregion

    }
}
