using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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

        private Random rand = new Random();
        private int[] directions = {-1,1};



        public Fish(Vector2 pos,Texture2D text)
        {
            this.position = pos;
            this.texture = text;
            this.isCaptured = false;
            this.isOnTheBeach = false;
            this.isDead = false;
        }

        public void update(GameTime gametime,float vel)
        {
            float velX = 0.55f *(float)gametime.ElapsedGameTime.TotalMilliseconds;
            float velY = vel * (float)gametime.ElapsedGameTime.TotalMilliseconds;
            

            if (this.position.Y < 210)
            {
                this.isOnTheBeach = true;
            }
            else
            {
                this.position = new Vector2(
                    this.position.X ,
                    this.position.Y - velY
                );
            }

         

        }

        public void draw(SpriteBatch sbatch)
        {
            if (!this.isDead)
            {
                sbatch.Draw(this.texture, this.position, null, Color.White, MathHelper.ToRadians(90.0f), Vector2.Zero, 1, SpriteEffects.None, 0);
            }

        }
        public Boolean hit(Rectangle playerRect,SoundEffect hit)
        {
            var bbox = new Rectangle((int)this.position.X,(int)this.position.Y,this.texture.Width,this.texture.Height);
            if (playerRect.Intersects(bbox)) 
            {
                hit.Play();
                this.isDead = true;
                return true;
            }

            return false;
        }


    }
}
