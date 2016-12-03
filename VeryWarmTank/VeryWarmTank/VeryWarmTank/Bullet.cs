using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VeryWarmTank
{
    public class Bullet : Sprite
    {
        Rectangle topHitbox;
        Rectangle leftHitbox;
        Rectangle bottomHitbox;
        Rectangle rightHitbox;
        private int _damage;
        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }
        private Vector2 speed;
        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        private int bounces;
        public int Bounces
        {
            get { return bounces; }
            set { bounces = value; }
        }
        private int maxBounces = 10;
        public int MaxBounces
        {
            get { return maxBounces; }
        }
        private bool dontdothis = true;




        public Bullet(Vector2 position, Color color, int damage)
            : base(Game1.bulletTexture, position, color)
        {
            _damage = damage;
            _origin = new Vector2(Game1.bulletTexture.Width / 2, Game1.bulletTexture.Height / 2);
            topHitbox = new Rectangle((int)position.X, (int)position.Y, Game1.bulletTexture.Width, 1);
            leftHitbox = new Rectangle((int)position.X, (int)position.Y, 1, Game1.bulletTexture.Height);
            rightHitbox = new Rectangle((int)position.X, (int)position.Y + Game1.bulletTexture.Width, 1, Game1.bulletTexture.Height);
            bottomHitbox = new Rectangle((int)position.X, (int)position.Y + Game1.bulletTexture.Height, Game1.bulletTexture.Width, 1);
        }
        public void Update(GameTime gametime, Color[,] pixelMap)
        {
            BulletCollisionCheck(pixelMap);
            this._position += this.speed;
            base.Update(gametime);
        }
        private void BulletCollisionCheck(Color[,] pixelMap)
        {
            Vector2 nextPosition = Position + speed;
            /*
            nextPosition.X = this._position.X + 12 * (float)Math.Cos(MathHelper.ToRadians(this._rotation - 90));
            nextPosition.Y = this._position.Y + 12 * (float)Math.Sin(MathHelper.ToRadians(this._rotation - 90));

            bool collision = false;
            */
            /*
            for (int i = -90; i < 90; i += 1)
            {
                nextPosition.X = this._position.X + 10 * (float)Math.Cos(MathHelper.ToRadians(this._rotation - 90 + i));
                nextPosition.Y = this._position.Y + 10 * (float)Math.Sin(MathHelper.ToRadians(this._rotation - 90 + i));
                #region

                //if (nextPosition.X < 0 || nextPosition.Y < 0)
                //{
                //    collision = true;
                //}
                //else if (nextPosition.X >= pixelMap.GetLength(0) || nextPosition.Y >= pixelMap.GetLength(1))
                //{
                //    collision = true;
                //}

                //hot garbage that works for now
                #endregion
                */

            if (pixelMap[(int)MathHelper.Clamp(nextPosition.X, 0, pixelMap.GetLength(0)-1), (int)MathHelper.Clamp(nextPosition.Y, 0, pixelMap.GetLength(1)-1)] == Color.Red)
            {
                Vector2 daNormal = new Vector2(-1, 0); //this is the vector away from the surface of the wall
                speed = Vector2.Reflect(speed, daNormal);
                bounces++;
            }
            if (pixelMap[(int)MathHelper.Clamp(nextPosition.X, 0, pixelMap.GetLength(0) - 1), (int)MathHelper.Clamp(nextPosition.Y, 0, pixelMap.GetLength(1) - 1)] == Color.Yellow)
            {
                Vector2 daNormal = new Vector2(1, 0); //this is the vector away from the surface of the wall
                speed = Vector2.Reflect(speed, daNormal);
                bounces++;
            }


            if (pixelMap[(int)MathHelper.Clamp(nextPosition.X, 0, pixelMap.GetLength(0) - 1), (int)MathHelper.Clamp(nextPosition.Y, 0, pixelMap.GetLength(1) - 1)] == Color.Blue)
            {
                Vector2 daNormal = new Vector2(0, 1); //this is the vector away from the surface of the wall
                speed = Vector2.Reflect(speed, daNormal);
                bounces++;
            }
            if (pixelMap[(int)MathHelper.Clamp(nextPosition.X, 0, pixelMap.GetLength(0) - 1), (int)MathHelper.Clamp(nextPosition.Y, 0, pixelMap.GetLength(1) - 1)] == Color.Black)
            {
                Vector2 daNormal = new Vector2(0, -1); //this is the vector away from the surface of the wall
                speed = Vector2.Reflect(speed, daNormal);
                bounces++;
            }


            /*
            if (pixelMap[(int)MathHelper.Clamp(nextPosition.X + 1, 0, 1899), (int)MathHelper.Clamp(nextPosition.Y, 0, 899)] != Color.White
                || pixelMap[(int)MathHelper.Clamp(nextPosition.X - 1, 0, 1899), (int)MathHelper.Clamp(nextPosition.Y, 0, 899)] != Color.White
                || pixelMap[(int)MathHelper.Clamp(nextPosition.X, 0, 1899), (int)MathHelper.Clamp(nextPosition.Y + 1, 0, 899)] != Color.White
                || pixelMap[(int)MathHelper.Clamp(nextPosition.X, 0, 1899), (int)MathHelper.Clamp(nextPosition.Y - 1, 0, 899)] != Color.White)
            {
                collision = true;
            }

            if (collision == true && (pixelMap[(int)MathHelper.Clamp(nextPosition.X + 12, 0, 1899), (int)MathHelper.Clamp(nextPosition.Y, 0, 899)] != Color.White))
            {
                speed.X = -Math.Abs(speed.X);
                bounces++;
            }
            else if (collision == true && (pixelMap[(int)MathHelper.Clamp(nextPosition.X - 12, 0, 1899), (int)MathHelper.Clamp(nextPosition.Y, 0, 899)] != Color.White))
            {
                speed.X = Math.Abs(speed.X);
                bounces++;
            }

            if (collision == true && (pixelMap[(int)MathHelper.Clamp(nextPosition.X, 0, 1899), (int)MathHelper.Clamp(nextPosition.Y - 12, 0, 899)] != Color.White))
            {
                speed.Y = Math.Abs(speed.Y);
                bounces++;
            }

            else if (collision == true && (pixelMap[(int)MathHelper.Clamp(nextPosition.X, 0, 1899), (int)MathHelper.Clamp(nextPosition.Y + 12, 0, 899)] != Color.White))
            {
                speed.Y = -Math.Abs(speed.Y);
                bounces++;
            }

            //}
            */

        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
        }
    }
}
