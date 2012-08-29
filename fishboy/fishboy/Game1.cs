using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;



namespace fishboy
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            fishTexture = Content.Load<Texture2D>("fish");
            bubbleTexture = Content.Load<Texture2D>("bubble");
            backgroundTexture = Content.Load<Texture2D>("bg");
            scoreFont = Content.Load<SpriteFont>("score");
            boyTexture = Content.Load<Texture2D>("fishboy");
            heartTexture = Content.Load<Texture2D>("heart");
            theme = Content.Load<Song>("theme");
            hit = Content.Load<SoundEffect>("hit");

            fishes = new List<Fish>();
            boy = new Boy(new Vector2(GraphicsDevice.Viewport.Width/2,100));

            MediaPlayer.Play(theme);
            // Coloca a música de fundo em loop infinito
            MediaPlayer.IsRepeating = true;
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            fishes.Clear();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (lifes == 0)
                gameOver = true;

            //TODO: Formula para level
            //level = ?


            //boy
            boy.update(gameTime);
            var fishBoyRect = new Rectangle((int)boy.position.X, (int)boy.position.Y,
                    boyTexture.Width, boyTexture.Height);

            //cria peixes
            if (rand.NextDouble() > 0.99/level)
            {
                Vector2 pos = new Vector2(
                        GraphicsDevice.Viewport.Width/2, 
                        GraphicsDevice.Viewport.Height
                );
                fishes.Add(new Fish(pos, fishTexture));
            }



            for(int i=0 ; i < fishes.Count ; i++ )
            {
                fishes[i].update(gameTime, 0.10f * level);
                
                if (fishes[i].hit(fishBoyRect,hit))
                {
                    fishes.Remove(fishes[i]);
                    score += 10;
                  
                }

            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //if (gameOver)
            spriteBatch.Begin();
            //bg
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            //hearts
            var lifePos = new Vector2(GraphicsDevice.Viewport.Width / 2 - 50, heartTexture.Width + 10);
            for (int i = 0; i < lifes; i++ )
            {
                spriteBatch.Draw(heartTexture, lifePos, Color.White);
                lifePos.X += heartTexture.Width;
            }


            //score
            spriteBatch.DrawString(scoreFont, "SCORE", new Vector2(30, GraphicsDevice.Viewport.Height - 60), Color.White);
            spriteBatch.DrawString(scoreFont, score.ToString() , new Vector2(150, GraphicsDevice.Viewport.Height - 60), Color.Red);


            spriteBatch.DrawString(scoreFont, gameTime.ElapsedGameTime.Seconds.ToString(), 
                new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height - 60), 
                Color.Red);
            
            //boy
            boy.draw(spriteBatch, boyTexture);

            //peixes
            foreach(Fish fish in fishes){
                fish.draw(spriteBatch);
            }

            //bolhas
            if (rand.NextDouble() > 0.5)
            {
                spriteBatch.Draw(bubbleTexture, new Rectangle(
                 rand.Next(0, GraphicsDevice.Viewport.Width),
                 rand.Next(GraphicsDevice.Viewport.Height - 120, GraphicsDevice.Viewport.Height), 20, 20),
                 Color.White);
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
