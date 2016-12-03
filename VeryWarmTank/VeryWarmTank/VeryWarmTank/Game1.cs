using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using FarseerPhysics.Dynamics;
//using FarseerPhysics.DebugView;
//using FarseerPhysics.Factories;
//using FarseerPhysics;

namespace VeryWarmTank
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //World world;
        //DebugViewXNA debugView;
        public static Texture2D bulletTexture;

        Tank veryWarmTank;
        Tank evenWarmerTank;
        Texture2D background;
        Texture2D keymap;
        Color[,] pixelMap;

        public static Texture2D pixel;

        Sprite arena;
        Sprite test;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1900;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            veryWarmTank = new Tank(Content.Load<Texture2D>("tankbody"), new Vector2(100, 50), Color.White, 50, 0f);
            veryWarmTank.turret = new Turret(Content.Load<Texture2D>("tankturret"), new Vector2(100, 50), Color.White, 0f);
            evenWarmerTank = new Tank(Content.Load<Texture2D>("tankbody"), new Vector2(1800, 50), Color.Red, 50, 0f);
            evenWarmerTank.turret = new Turret(Content.Load<Texture2D>("tankturret"), new Vector2(1800, 50), Color.Red, 0f);
            bulletTexture = Content.Load<Texture2D>("bullet");
            background = Content.Load<Texture2D>("Arena");
            keymap = Content.Load<Texture2D>("KeyMap");

            Color[] pixels = new Color[keymap.Width * keymap.Height];
            keymap.GetData<Color>(pixels);
            pixelMap = new Color[keymap.Width, keymap.Height];
            for (int y = 0; y < keymap.Height; y++)
            {
                for (int x = 0; x < keymap.Width; x++)
                {
                    pixelMap[x, y] = pixels[x + y * keymap.Width];
                }
            }

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            arena = new Sprite(background, new Vector2(0, 0), Color.White);
            test = new Sprite(keymap, new Vector2(0, 0), Color.White);
            #region moveButtons
            veryWarmTank.MoveButton1 = Keys.W;
            veryWarmTank.MoveButton2 = Keys.A;
            veryWarmTank.MoveButton3 = Keys.S;
            veryWarmTank.MoveButton4 = Keys.D;
            veryWarmTank.turret.TurnButton1 = Keys.O;
            veryWarmTank.turret.TurnButton2 = Keys.P;
            veryWarmTank.turret.ShootButton = Keys.I;
            evenWarmerTank.MoveButton1 = Keys.Up;
            evenWarmerTank.MoveButton2 = Keys.Left;
            evenWarmerTank.MoveButton3 = Keys.Down;
            evenWarmerTank.MoveButton4 = Keys.Right;
            evenWarmerTank.turret.TurnButton1 = Keys.NumPad2;
            evenWarmerTank.turret.TurnButton2 = Keys.NumPad3;
            evenWarmerTank.turret.ShootButton = Keys.NumPad1;
            #endregion
            #region FarseerPhysics
            //world = new World(new Vector2(0, 9.81f));

            //debugView = new DebugViewXNA(world);
            //debugView.LoadContent(GraphicsDevice, Content);

            //Body body = BodyFactory.CreateBody(world, new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) / 2);
            //body.BodyType = BodyType.Dynamic;
            //FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(100), ConvertUnits.ToSimUnits(100), 1, Vector2.Zero, body);
            // body.ApplyForce(Vector2.UnitY * 10);

            //Body floorBody = BodyFactory.CreateBody(world, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height/2 + 4));
            //FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(1000), ConvertUnits.ToSimUnits(1), 100, Vector2.Zero, floorBody);
            // TODO: use this.Content to load your game content here
            #endregion
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        
        protected override void Update(GameTime gameTime)
        {
            //world.Step(1/60f);

            //Color[] colors = new Color[veryWarmTank.Width * veryWarmTank.Height];
            //keymap.GetData<Color>(0, veryWarmTank.HitBox, colors, 0, colors.Length);

            veryWarmTank.Update(gameTime, pixelMap, Keyboard.GetState());
            veryWarmTank.turret.UpdateBullets(gameTime, evenWarmerTank.turret.FreezingHot, evenWarmerTank, pixelMap, evenWarmerTank.turret.Bullets);

            evenWarmerTank.Update(gameTime, pixelMap, Keyboard.GetState());
            evenWarmerTank.turret.UpdateBullets(gameTime, veryWarmTank.turret.FreezingHot, veryWarmTank, pixelMap, veryWarmTank.turret.Bullets);

            // OVER HERE. UH JUST USE THE UPDATEBULLETS TO SEE THE FREEZINGHOT OF THE OTHER TANK LOL
            // TODO: Add your update logic here
            
           

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //debugView.RenderDebugData(  Matrix.CreateOrthographic(ConvertUnits.ToSimUnits(GraphicsDevice.Viewport.Width), ConvertUnits.ToSimUnits(GraphicsDevice.Viewport.Height), 0, 10), 
                                        //Matrix.CreateLookAt(new Vector3(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -10)/2, new Vector3(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0) / 2, Vector3.Down));

            spriteBatch.Begin();
            //arena.Draw(spriteBatch);
            test.Draw(spriteBatch);
            veryWarmTank.Draw(spriteBatch);
            evenWarmerTank.Draw(spriteBatch);
            //spriteBatch.DrawString(Content.Load<SpriteFont>("SpriteFont1"), string.Format("EvenWarmerTank: {0} --- VeryWarmTank: {1}", evenWarmerTank.turret.FreezingHot, veryWarmTank.turret.FreezingHot), new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(Content.Load<SpriteFont>("SpriteFont1"), string.Format("Health: {0}", veryWarmTank.Health), new Vector2(0, 875), Color.Blue);
            spriteBatch.DrawString(Content.Load<SpriteFont>("SpriteFont1"), string.Format("{0}", veryWarmTank.turret.Remaining), new Vector2(veryWarmTank.turret.Position.X + 50, veryWarmTank.turret.Position.Y), Color.Blue);
            spriteBatch.DrawString(Content.Load<SpriteFont>("SpriteFont1"), string.Format("Health: {0}", evenWarmerTank.Health), new Vector2(1775, 875), Color.Red);
            spriteBatch.DrawString(Content.Load<SpriteFont>("SpriteFont1"), string.Format("{0}", evenWarmerTank.turret.Remaining), new Vector2(evenWarmerTank.turret.Position.X + 50, evenWarmerTank.turret.Position.Y), Color.Blue);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
