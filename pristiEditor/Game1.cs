using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace pristiEditor
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        
        private RenderTarget2D m_EditorViewer;

        Editor m_Editor;
        private int m_ViewerWidth = 1280;
        private int m_ViewerHeight = 900;
        private int m_ScreenWidth = 1600;
        private int m_ScreenHeight = 900;
        private Vector2 m_EditorViewerPos;
        Settings m_Settings;

        public Game1()
        {
           

            _graphics = new GraphicsDeviceManager(this);
            LoadSettings();
            Content.RootDirectory = m_Settings.GameContentLocation;
            IsMouseVisible = true;
           
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
   
            _graphics.PreferredBackBufferWidth = m_ScreenWidth;
            _graphics.PreferredBackBufferHeight = m_ScreenHeight;
            _graphics.ApplyChanges();

            m_EditorViewerPos = new Vector2(m_ScreenWidth - m_ViewerWidth,0);
            base.Initialize();



            //Settings settings = new Settings();
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
            //StreamWriter myWriter = new StreamWriter("myFileName.xml");
            //xmlSerializer.Serialize(myWriter, settings);
            //myWriter.Close();



        }

        private void LoadSettings() {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            // To read the file, create a FileStream.
            FileStream fileStream = new FileStream("Settings.xml", FileMode.Open);
            // Call the Deserialize method and cast to the object type.
            m_Settings = (Settings)serializer.Deserialize(fileStream);
            Console.WriteLine(m_Settings);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            m_EditorViewer = new RenderTarget2D(GraphicsDevice, m_ViewerWidth, m_ViewerHeight);
            m_Editor = new Editor(m_Settings.worldXMLLocation,Content,GraphicsDevice,new Rectangle(m_ScreenWidth-m_ViewerWidth,0, m_EditorViewer.Bounds.Width, m_EditorViewer.Bounds.Height),m_Settings);
            // TODO: use this.Content to load your game content here

           
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            m_Editor.Update();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(m_EditorViewer);
            GraphicsDevice.Clear(Color.DarkGray);
            _spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            m_Editor.Draw(_spriteBatch);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.DimGray);
            
            _spriteBatch.Begin();
            _spriteBatch.Draw(m_EditorViewer, m_EditorViewerPos, Color.White);

            _spriteBatch.End();

  
            // TODO: Add your drawing code here
            
            base.Draw(gameTime);

        }
    }
}
