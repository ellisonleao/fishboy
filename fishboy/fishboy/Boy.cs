using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Devices.Sensors;

namespace fishboy
{
    class Boy
    {
        public Vector2 position { get; set; }
        private bool left;
        int viewPort;
        Texture2D texture;

        public Boy(Vector2 pos, int viewPort, Texture2D texture)
        {
            this.position = pos;
            this.left = false;
            this.viewPort = viewPort;
            this.texture = texture;
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
                    var velX = new Vector2(0.6f *(float)gametime.ElapsedGameTime.TotalMilliseconds, 0.0f);
                    if (tl.Position.X <= this.viewPort/2 )
                    {
                        if (this.position.X < 50) this.position = new Vector2(50, this.position.Y);
                        //esquerda
                        this.left = true;
                        this.position -= velX;
                    }


                    if (tl.Position.X > this.viewPort / 2)
                    {
                        if (this.position.X > 720) this.position = new Vector2(720, this.position.Y);
                        //direita
                        this.left = false;
                        this.position += velX;
                    }
                   
                }
            }
            
        }

        public void draw(SpriteBatch sbatch) 
        {
            if (left)
                sbatch.Draw(texture, this.position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally,0);
            else
                sbatch.Draw(texture, this.position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

        }


        public Rectangle getBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y,
                        texture.Width + 30, texture.Height);
        }


    }
}
