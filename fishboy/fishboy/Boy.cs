using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace fishboy
{
    class Boy
    {
        public Vector2 position { get; set; }
        private bool left, right;


        public Boy(Vector2 pos)
        {
            this.position = pos;
            this.left = false;
            this.right = false;
        }

        public void update(GameTime gametime)
        {
            // Process touch events
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                if ((tl.State == TouchLocationState.Pressed)
                        || (tl.State == TouchLocationState.Moved))
                {
                    float velX = 0.6f *(float)gametime.ElapsedGameTime.TotalMilliseconds;
                    if (tl.Position.X < 90)
                    {
                        if (this.position.X < 50) this.position = new Vector2(50, this.position.Y);
                        //esquerda
                        left = true;
                        right = false;
                        this.position = new Vector2(this.position.X - velX, this.position.Y);
                    }


                    if (tl.Position.X >  650)
                    {
                        if (this.position.X > 720) this.position = new Vector2(720, this.position.Y);
                        //direita
                        left = false;
                        right = true;
                        this.position = new Vector2(this.position.X + velX, this.position.Y);
                    }
                   
                }
            }
            
        }

        public void draw(SpriteBatch sbatch, Texture2D texture) 
        {
            if (left)
                sbatch.Draw(texture, this.position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally,0);
            else
                sbatch.Draw(texture, this.position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

        }


    }
}
