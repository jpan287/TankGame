using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VeryWarmTank
{
    class Wall : Sprite
    {
        Rectangle topHitbox;
        Rectangle leftHitbox;
        Rectangle rightHitbox;
        Rectangle bottomHitbox;
        public Wall(Texture2D texture, Vector2 position, Color color)
            :base(texture,position,color)
        {
            topHitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, 1);
            leftHitbox = new Rectangle((int)position.X, (int)position.Y, 1, texture.Height);
            rightHitbox = new Rectangle((int)position.X, (int)position.Y + texture.Width, 1, texture.Height);
            bottomHitbox = new Rectangle((int)position.X, (int)position.Y + texture.Height, texture.Width, 1);
        }
        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
        }
        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
        }

    }
}
