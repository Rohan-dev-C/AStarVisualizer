using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Text;

namespace AStarVisualizer
{
    public class Sprite
    {
        public Vector2 position;
        public Texture2D texture;
        public Color color;
        public float rotation;
        public Vector2 origin;
        public Vector2 scale;
        public SpriteEffects effects;

        public Vector2 Size
        {
            get
            {
                return new Vector2(texture.Width * scale.X, texture.Height * scale.Y);
            }
        }

        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        public Sprite(Vector2 position, Texture2D texture, Color color, Vector2 scale)
        {
            this.texture = texture;
            this.color = color;
            this.position = position;
            this.scale = scale;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, effects, 0);
        }

    }
}
