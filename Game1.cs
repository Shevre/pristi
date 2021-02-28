using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using pristi.World;
using pristi.Entities;
using System;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace pristi
{
    public partial class Game1 : Game
    {
        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_Spritebatch;

        public static Settings Settings;

        private RenderTarget2D m_InternalScreen;
        private Rectangle m_TargetRect;

        private Room testRoom;
        private Texture2D AMOGUS;
        private Vector2 AMOGUSPos = new Vector2(32,7*16);

        InputManager m_InputManager = new InputManager();

        Entity testEntity = new Entity(new Vector2(2 * 16, 5 * 16), new Vector2(16, 32), "TestEntity");
        Player testPlayer;
        

        public static SpriteFont DebugFont;
        public static SpriteFont DebugFontSmall;


        public Game1(){
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize(){
            // TODO: Add your initialization logic here

            Settings = new Settings();
    
            

            GlobalInput.SetManager(m_InputManager);
         
            ApplyGraphicSettings();
            base.Initialize();
        }

        private void ApplyGraphicSettings(){
            m_Graphics.PreferredBackBufferWidth = Settings.GetScreenWidth();
            m_Graphics.PreferredBackBufferHeight = Settings.GetScreenHeight();
            m_Graphics.IsFullScreen = Settings.IsFullscreen();
            m_Graphics.ApplyChanges();
            m_TargetRect = new Rectangle(0, 0, Settings.GetScreenWidth(), Settings.GetScreenHeight());
            AdjustScreen(m_TargetRect.Width, m_TargetRect.Height);
        }

        protected override void LoadContent(){
            m_Spritebatch = new SpriteBatch(GraphicsDevice);
            m_InternalScreen = new RenderTarget2D(GraphicsDevice, Settings.GetInteralScreenWidth(), Settings.GetInternalScreenHeight());
            testPlayer = new Player(new Vector2(50,50), new Vector2(12,24), Content.Load<Texture2D>("Sprites/placerholder"), "Player");
            testRoom = new Room("Content/Data/TestWorld/Rooms/testmap.xml", Content,testPlayer);

            AMOGUS = Content.Load<Texture2D>("Sprites/Amogus");

            DebugFont = Content.Load<SpriteFont>("Consolas");
            DebugFontSmall = Content.Load<SpriteFont>("ConsolasSmall");
           

            // TODO: use this.Content to load your game content here
        }
        protected override void Dispose(bool disposing)
        {
            m_InternalScreen.Dispose();
            base.Dispose(disposing);
        }
        protected override void Update(GameTime gameTime){
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            m_InputManager.Update(gameTime,Keyboard.GetState(),GamePad.GetState(PlayerIndex.One));
            //AMOGUSPos.X += amogusdir * 48f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if (AMOGUSPos.X > m_Settings.GetInteralScreenWidth() - 56)
            //    amogusdir = -1f;
            //if (AMOGUSPos.X < 32)
            //    amogusdir = 1f;
            // TODO: Add your update logic here
            if (m_InputManager.Start.IsToggled)
            {
                testRoom.Update(gameTime);
            }
           
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime){


            // TODO: Add your drawing code here
           
            GraphicsDevice.SetRenderTarget(m_InternalScreen);
            GraphicsDevice.Clear(Color.Black);
            m_Spritebatch.Begin(samplerState:SamplerState.PointClamp);

            testRoom.Draw(m_Spritebatch);
            //m_Tileset.DrawTileset(m_Spritebatch, new Vector2(0, 0),6);
            m_Spritebatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.SkyBlue);
            m_Spritebatch.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp);
            m_Spritebatch.Draw(m_InternalScreen, m_TargetRect, Color.White);
            testRoom.DrawNative(m_Spritebatch);
            if(GlobalInput.Manager.Control.IsToggled) GlobalInput.Manager.DrawDebug(m_Spritebatch);
            m_Spritebatch.End();
            base.Draw(gameTime);
        }
    }




}
