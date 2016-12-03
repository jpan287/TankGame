using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VeryWarmTank
{
    public class Sprite
    {
        protected Texture2D _texture;
        protected Vector2 _position;
        protected Color _color;
        protected float _rotation;
        protected Vector2 _origin;
        protected Rectangle _hitbox;
        protected Rectangle _sourceRectangle;
        public Rectangle HitBox
        {
            get { return _hitbox; }
            set { _hitbox = value; }
        }
        public int Width
        {
            get { return _texture.Width; }
        }
        public int Height
        {
            get { return _texture.Height; }
        }
        public Sprite(Texture2D texture, Vector2 position, Color color)
        {
            _texture = texture;
            _position = position;
            _color = color;
            _rotation = 0;
            _origin = Vector2.Zero;
            _sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            _hitbox = new Rectangle((int)_position.X, (int)_position.Y, texture.Width, texture.Height);
        }
        public virtual void Update(GameTime gametime)
        {
            _hitbox.X = (int)_position.X;
            _hitbox.Y = (int)_position.Y;
            _hitbox.Width = _sourceRectangle.Width;
            _hitbox.Height = _sourceRectangle.Height;
        }
        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_texture, _position, null, _color, MathHelper.ToRadians(_rotation), _origin, 1, SpriteEffects.None, 0);
        }
    }
}
