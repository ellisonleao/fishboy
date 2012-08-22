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
        private Random rand = new Random();
        private int[] directions = {-1,1};
        public Fish(Vector2 pos,Texture2D text)
        {
            this.position = pos;
            this.texture = text;
        }

        public Vector2 update(GameTime gametime,float vel)
        {
        
            float velX = 0.9f *(float)gametime.ElapsedGameTime.TotalMilliseconds;
            float velY = vel * (float)gametime.ElapsedGameTime.TotalMilliseconds;
            this.position = new Vector2(
                    this.position.X , 
                    this.position.Y - velY
                    );   
            return position;
        }

        public void draw(SpriteBatch sbatch) 
        {
            /*
            float rotation = MathHelper.ToRadians(180);
            sbatch.Draw(this.texture, this.position, 
                new Rectangle((int)this.position.X,(int)this.position.Y,this.texture.Width,this.texture.Height),
                Color.White, rotation, Vector2.Zero, 0, SpriteEffects.None, 0f);
            */
            sbatch.Draw(this.texture, this.position, Color.White);
        }


    }
}
