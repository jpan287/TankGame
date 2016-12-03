using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VeryWarmTank
{
    public class Tank : Sprite
    {
        KeyboardState ks = new KeyboardState();
        private int _health;
        List<Vector2> blackPixels;
        Vector2 _radius;
        bool canMoveForward = true;
        bool canMoveBackward = true;
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        private Keys moveButton1;
        public Keys MoveButton1
        {
            get { return moveButton1; }
            set { moveButton1 = value; }
        }
        private Keys moveButton2;
        public Keys MoveButton2
        {
            get { return moveButton2; }
            set { moveButton2 = value; }
        }
        private Keys moveButton3;
        public Keys MoveButton3
        {
            get { return moveButton3; }
            set { moveButton3 = value; }
        }
        private Keys moveButton4;
        public Keys MoveButton4
        {
            get { return moveButton4; }
            set { moveButton4 = value; }
        }

        public Turret turret;
        
        public Tank(Texture2D texture, Vector2 position, Color color, int health, float rotation)
            :base(texture, position, color)
        {
            _health = health;
            _rotation = rotation;
            _origin = new Vector2(texture.Width / 2, texture.Height / 2);
            blackPixels = new List<Vector2>();
            _radius = new Vector2(0, texture.Height);
        }
        public void Update(GameTime gametime, Color[,] pixelMap, KeyboardState keyState)
        {
            this.ks = keyState;

            CollisionCheck(pixelMap);

            if (ks.IsKeyDown(moveButton2))
            {
                _rotation -= 1f;
            }
            if (ks.IsKeyDown(moveButton4))
            {
                _rotation += 1f;
            }
            if (ks.IsKeyDown(moveButton1) && canMoveForward == true)
            {
                this._position.X += 2 * (float)Math.Cos(MathHelper.ToRadians(this._rotation - 90));
                this._position.Y += 2 * (float)Math.Sin(MathHelper.ToRadians(this._rotation - 90));
            }
            if (ks.IsKeyDown(moveButton3) && canMoveBackward == true)
            {
                this._position.X -= 1.5f * (float)Math.Cos(MathHelper.ToRadians(this._rotation - 90));
                this._position.Y -= 1.5f *(float)Math.Sin(MathHelper.ToRadians(this._rotation - 90));
            }
            if (ks.IsKeyDown(moveButton1) || ks.IsKeyDown(moveButton2) || ks.IsKeyDown(moveButton3) || ks.IsKeyDown(moveButton4))
            {
                this.turret.FreezingHot = false;
                this.turret.Burn = TimeSpan.Zero;
            }
            else
            {
                this.turret.FreezingHot = true;
            }
            this.turret.Update(gametime, keyState);
            this.turret.Position = this._position;
            base.Update(gametime);
            //UpdateChampion(gametime, pixelMap);
        }

        private void CollisionCheck(Color[,] pixelMap)
        {
            //forward check
            Vector2 nextForwardPosition;
            nextForwardPosition.X = this._position.X + 29 * (float)Math.Cos(MathHelper.ToRadians(this._rotation - 90));
            nextForwardPosition.Y = this._position.Y + 29 * (float)Math.Sin(MathHelper.ToRadians(this._rotation - 90));


            bool collision = false;
            for (int i = -90; i < 90; i += 1)
            {

                nextForwardPosition.X = this._position.X + 28 * (float)Math.Cos(MathHelper.ToRadians(this._rotation - 90 + i));
                nextForwardPosition.Y = this._position.Y + 28 * (float)Math.Sin(MathHelper.ToRadians(this._rotation - 90 + i));

                if (nextForwardPosition.X < 0 || nextForwardPosition.Y < 0)
                {
                    collision = true;
                }
                else if (nextForwardPosition.X >= pixelMap.GetLength(0) || nextForwardPosition.Y >= pixelMap.GetLength(1))
                {
                    collision = true;
                }
                else if (pixelMap[(int)nextForwardPosition.X, (int)nextForwardPosition.Y] != Color.White)
                {
                    collision = true;
                }
            }
            if (collision)
            {
                canMoveForward = false;
            }
            else
            {
                canMoveForward = true;
            }

            Vector2 nextBackwardPosition;
            collision = false;
            for (int i = -90; i < 90; i += 1)
            {

                nextBackwardPosition.X = this._position.X - 28 * (float)Math.Cos(MathHelper.ToRadians(this._rotation - 90 + i));
                nextBackwardPosition.Y = this._position.Y - 28 * (float)Math.Sin(MathHelper.ToRadians(this._rotation - 90 + i));


                if (nextBackwardPosition.X < 0 || nextBackwardPosition.Y < 0)
                {
                    collision = true;
                }
                else if (nextBackwardPosition.X >= pixelMap.GetLength(0) || nextBackwardPosition.Y >= pixelMap.GetLength(1))
                {
                    collision = true;
                }
                else if (pixelMap[(int)nextBackwardPosition.X, (int)nextBackwardPosition.Y] != Color.White)
                {
                    collision = true;
                }
            }

            if (collision)
            {
                canMoveBackward = false;
            }
            else
            {
                canMoveBackward = true;
            }

        }

        public void UpdateChampion(GameTime gametime, Color[] colors)
        {

            for (int y = 0; y < _hitbox.Width; y++)
            {
                for (int x = 0; x < _hitbox.Height; x++)
                {
                    if (colors[x + y * _hitbox.Height] == Color.Black)
                    {
                        if (CheckRadius(new Vector2(_position.X + x, _position.Y + y)))
                        {
                            //int collisionAngle = (int)MathHelper.ToDegrees(CollisionAngle(new Vector2(_position.X + x, _position.Y + y)));
                            //int speedAngle = (int)MathHelper.ToDegrees((float)Math.Atan2(speed.Y , speed.X));
                            blackPixels.Add(new Vector2(_position.X + x, _position.Y + y));
                        }
                    }
                }
            }



            for (int i = 0; i < blackPixels.Count; i++)
            {
                for (int j = 0; j < blackPixels.Count; j++)
                {
                    if (i != j && j > i && blackPixels[i].X > blackPixels[j].X)
                    {
                        Vector2 temp = blackPixels[i];
                        blackPixels[i] = blackPixels[j];
                        blackPixels[j] = temp;
                    }
                }
            }



            if (blackPixels.Count > 0)
            {
                int collisionAngle = (int)MathHelper.ToDegrees(CollisionAngle(new Vector2(blackPixels[blackPixels.Count / 2].X, blackPixels[blackPixels.Count / 2].Y)));
                int speedAngle = (int)MathHelper.ToDegrees((float)Math.Atan2(Math.Sin(MathHelper.ToRadians(this._rotation - 90)), Math.Cos(MathHelper.ToRadians(this._rotation - 90))));
                if (speedAngle >= collisionAngle - 80 && speedAngle <= collisionAngle + 80)
                {
                    canMoveForward = false;
                }
                else
                {
                    canMoveForward = true;
                }
                blackPixels.Clear();
            }
            
        }

        public bool CheckRadius(Vector2 pixelLocation)
        {
            Vector2 distance = new Vector2(_position.X - pixelLocation.X, _position.Y - pixelLocation.Y);
            return distance.Length() <= _radius.Length();

        }
        public float CollisionAngle(Vector2 pixelLocation)
        {
            Vector2 distance = new Vector2(pixelLocation.X - (_position.X + Width / 2), pixelLocation.Y - (_position.Y + Height / 2));
            return (float)Math.Atan2(distance.Y, distance.X);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            //Vector2 nextForwardPosition;

            //for (int i = -90; i < 90; i += 1)
            //{
            //    nextForwardPosition.X = this._position.X + 28 * (float)Math.Cos(MathHelper.ToRadians(this._rotation - 90 + i));
            //    nextForwardPosition.Y = this._position.Y + 28 * (float)Math.Sin(MathHelper.ToRadians(this._rotation - 90 + i));

            //    spritebatch.Draw(Game1.pixel, nextForwardPosition, new Rectangle(0, 0, 4, 4), Color.Black);
            //}

            this.turret.Draw(spritebatch);
            
        }
    }
}
