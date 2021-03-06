using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;

namespace fishboy
{
    class Fish
    {
        public Vector2 position { get; set; }
        public float scale { get; set; }
        public Texture2D texture;
        public bool isOnTheBeach { get; set; }
        public bool isCaptured { get; set; }
        public bool isDead { get; set; }
        public Vector2 vel { get; set; }

        private Random rand = new Random();
        
        float timeToKill;


        public Fish(Vector2 pos,Texture2D text)
        {
            this.position = pos;
            this.texture = text;
            this.isCaptured = false;
            this.isOnTheBeach = false;
            this.isDead = false;
            
        }

        public void update(GameTime gametime, float vel, SoundEffect dead)
        {
            
            float velY = vel;

            this.vel = new Vector2(0, velY * (float)gametime.ElapsedGameTime.TotalMilliseconds);

            if (this.isOnTheBeach)
            {
                if (this.timeToKill > 0)
                    this.timeToKill -= (float)gametime.ElapsedGameTime.TotalSeconds;
                else
                {
                    if ((bool)IsolatedStorageSettings.ApplicationSettings["sound"])
                        dead.Play();
                    this.isDead = true;
                }
                    
            }


            if (this.position.Y < 210)
            {
                if (!this.isOnTheBeach)
                {
                    this.isOnTheBeach = true;
                    this.timeToKill = 2.0f;
                }
                    
            }
            else
            {
                this.position -=  this.vel;
            }
        }

        public void draw(SpriteBatch sbatch)
        {
            if (!this.isDead || !this.isCaptured)
            { 
                sbatch.Draw(this.texture, this.position, null, Color.White, MathHelper.ToRadians(90), Vector2.Zero, 1, SpriteEffects.None, 0);
            }

        }
        public Boolean hit(Rectangle playerRect, SoundEffect hit)
        {
            var bbox = new Rectangle((int)this.position.X, (int)this.position.Y, this.texture.Height, this.texture.Width); //peixe rotacionado
            if (playerRect.Intersects(bbox)) 
            {
                if ((bool) IsolatedStorageSettings.ApplicationSettings["soundFx"]) 
                    hit.Play();
                this.isCaptured = true;
                return true;
            }

            return false;
        }

        public void reboot(int height, int width) 
        { 
            int newX = rand.Next(this.texture.Width, width - this.texture.Width);
            this.position = new Vector2((float) newX, (float) height);
            this.isDead = false;
            this.isOnTheBeach = false;
        }


    }
}
