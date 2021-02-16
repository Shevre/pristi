using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using pristi.World;
using System;
using System.Xml;

namespace pristi
{
    public partial class Game1 : Game
    {
        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_Spritebatch;

        private Settings m_Settings;

        private RenderTarget2D m_InternalScreen;
        private Rectangle m_TargetRect;

        private Room testRoom;
        private Texture2D AMOGUS;
        private Vector2 AMOGUSPos = new Vector2(32,7*16);

        public Game1(){
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize(){
            // TODO: Add your initialization logic here

            m_Settings = new Settings();
           

            ApplyGraphicSettings();
            base.Initialize();
        }

        private void ApplyGraphicSettings(){
            m_Graphics.PreferredBackBufferWidth = m_Settings.GetScreenWidth();
            m_Graphics.PreferredBackBufferHeight = m_Settings.GetScreenHeight();
            m_Graphics.IsFullScreen = m_Settings.IsFullscreen();
            m_Graphics.ApplyChanges();
            m_TargetRect = new Rectangle(0, 0, m_Settings.GetScreenWidth(), m_Settings.GetScreenHeight());
            AdjustScreen(m_TargetRect.Width, m_TargetRect.Height);
        }

        protected override void LoadContent(){
            m_Spritebatch = new SpriteBatch(GraphicsDevice);
            m_InternalScreen = new RenderTarget2D(GraphicsDevice, m_Settings.GetInteralScreenWidth(), m_Settings.GetInternalScreenHeight());
            testRoom = new Room("Content/Data/World/Rooms/TestRoom.xml", Content);

            AMOGUS = Content.Load<Texture2D>("Sprites/Amogus");
            
            // TODO: use this.Content to load your game content here
        }
        protected override void Dispose(bool disposing)
        {
            m_InternalScreen.Dispose();
            base.Dispose(disposing);
        }
        float amogusdir = 1f;
        protected override void Update(GameTime gameTime){
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            AMOGUSPos.X += amogusdir * 48f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (AMOGUSPos.X > m_Settings.GetInteralScreenWidth() - 56)
                amogusdir = -1f;
            if (AMOGUSPos.X < 32)
                amogusdir = 1f;
            // TODO: Add your update logic here
            testRoom.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime){


            // TODO: Add your drawing code here
           
            GraphicsDevice.SetRenderTarget(m_InternalScreen);
            GraphicsDevice.Clear(Color.Black);
            m_Spritebatch.Begin();
            testRoom.DrawPreEntities(m_Spritebatch);
            //m_Spritebatch.Draw(AMOGUS, AMOGUSPos,null, Color.White,0f,new Vector2(0,0),0f,(amogusdir < 0)? SpriteEffects.FlipHorizontally : SpriteEffects.None,1f);
            m_Spritebatch.Draw(AMOGUS, AMOGUSPos, Color.White);
            testRoom.DrawPostEntities(m_Spritebatch);
            //m_Tileset.DrawTileset(m_Spritebatch, new Vector2(0, 0),6);
            m_Spritebatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.SkyBlue);
            m_Spritebatch.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp);
            m_Spritebatch.Draw(m_InternalScreen, m_TargetRect, Color.White);
            m_Spritebatch.End();
            base.Draw(gameTime);
        }
    }
}
