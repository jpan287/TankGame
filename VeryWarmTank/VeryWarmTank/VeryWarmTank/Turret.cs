using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VeryWarmTank
{
    public class Turret : Sprite
    {
        private Keys turnButton1;
        public Keys TurnButton1
        {
            get { return turnButton1; }
            set { turnButton1 = value; }
        }
        private Keys turnButton2;
        public Keys TurnButton2
        {
            get { return turnButton2; }
            set { turnButton2 = value; }
        }
        private Keys shootButton;
        public Keys ShootButton
        {
            get { return shootButton; }
            set { shootButton = value; }
        }
        TimeSpan shootTime = TimeSpan.Zero;
        TimeSpan coolDown = TimeSpan.FromMilliseconds(400);
        bool canShoot = true;

        private TimeSpan burn = TimeSpan.Zero;
        private TimeSpan freeze = TimeSpan.FromMilliseconds(5000);
        public TimeSpan Burn
        {
            get { return burn; }
            set { burn = value; }
        }
        public TimeSpan Freeze
        {
            get { return freeze; }
            set { freeze = value; }
        }
        private int _remaining;
        public int Remaining
        {
            get { return _remaining; }
            set { _remaining = value; }
        }


        private List<Bullet> bullets = new List<Bullet>();
        public List<Bullet>  Bullets
        {
            get { return bullets; }
            set { bullets = value; }
        }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private bool freezingHot = true;
        public bool FreezingHot
        {
            get { return freezingHot; }
            set { freezingHot = value; }
        }


        public Turret(Texture2D texture, Vector2 position, Color color, float rotation)
            : base(texture, position, color)
        {
            this._origin = new Vector2(16, 48);
        }
        public void Update(GameTime gametime, KeyboardState keystate)
        {
            shootTime += gametime.ElapsedGameTime;
            burn += gametime.ElapsedGameTime;
            if (keystate.IsKeyDown(turnButton1))
            {
                _rotation -= 1.5f;
            }
            if (keystate.IsKeyDown(turnButton2))
            {
                _rotation += 1.5f;
            }
            if (keystate.IsKeyDown(shootButton) && canShoot)
            {
                Bullet temp = new Bullet(this._position + 45f * new Vector2((float)Math.Cos(MathHelper.ToRadians(this._rotation - 90)), (float)Math.Sin(MathHelper.ToRadians(this._rotation - 90))), Color.White, 5);

                temp.Speed = 4 * new Vector2((float)Math.Cos(MathHelper.ToRadians(this._rotation - 90)), (float)Math.Sin(MathHelper.ToRadians(this._rotation - 90)));

                bullets.Add(temp);
            }
            if (burn > freeze)
            {
                freezingHot = false;
            }
   
            if (shootTime >= coolDown)
            {
                canShoot = true;
                shootTime = TimeSpan.Zero;
            }
            else
            {
                canShoot = false;
            }
            if (burn.Seconds <= 5)
            {
                _remaining = freeze.Seconds - burn.Seconds;
            }
            
            base.Update(gametime);
        }
        public void UpdateBullets(GameTime gameTime, bool opponentFreezingHot, Tank opponentTank, Color[,] pixelMap, List<Bullet> opponentBullets)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (opponentFreezingHot == false && freezingHot == false)
                {
                    bullets[i].Update(gameTime, pixelMap);
                }
                if (bullets[i].HitBox.Intersects(opponentTank.HitBox) && freezingHot == false && opponentFreezingHot == false)
                {
                    opponentTank.Health -= bullets[i].Damage;
                    bullets.Remove(bullets[i]);
                    continue;
                }
                else if (bullets[i].Bounces >= bullets[i].MaxBounces)
                {
                    bullets.Remove(bullets[i]);
                    continue;
                }
                else if (bullets[i].Position.X > 1900 || bullets[i].Position.Y > 900 || bullets[i].Position.X < 0 || bullets[i].Position.Y < 0)
                {
                    bullets.Remove(bullets[i]);
                    continue;
                }
                for (int j = 0; j < opponentBullets.Count; j++)
                {
                    if (opponentBullets[j].HitBox.Intersects(bullets[i].HitBox))
                    {
                        opponentBullets.Remove(opponentBullets[j]);
                        bullets.Remove(bullets[i]);
                        continue;
                    }
                }
            }
        }
        public override void Draw(SpriteBatch spritebatch)
        {
            
            base.Draw(spritebatch);
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(spritebatch);
            }
        }
    }
}
