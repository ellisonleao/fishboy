using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace fishboy
{
    class Fish
    {
        public Vector2 position { get; set; }
        public float scale { get; set; }
        public Texture2D texture;
        public bool isDead;
        private Random rand = new Random();
        private int[] directions = {-1,1};


        public Fish(Vector2 pos,Texture2D text)
        {
            this.position = pos;
            this.texture = text;
            this.isDead = false;
        }

        public void update(GameTime gametime,float vel)
        {
            float velX = 0.55f *(float)gametime.ElapsedGameTime.TotalMilliseconds;
            float velY = vel * (float)gametime.ElapsedGameTime.TotalMilliseconds;
            
            if (this.position.Y < 240)
            {
                this.isDead = true;
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
            sbatch.Draw(this.texture, this.position, Color.White);
        }


    }
}
