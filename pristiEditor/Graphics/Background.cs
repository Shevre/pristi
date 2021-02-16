using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml;

namespace pristi.Graphics
{
    public interface IBackground
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }

    public class BasicBackground : IBackground{
        private Texture2D m_Texture;
        private (bool x,bool y) m_Tiled = (false,false);
        private float m_Ang;
        private float m_MoveSpeed;
        private Vector2 m_Offset = new Vector2();
        private Vector2 m_AngCalc;

        public BasicBackground(Texture2D texture,float ang = 0f,bool tiledX = false,bool tiledY = false,float movementSpeed = 0f){
            m_Texture = texture;
            m_Ang = ang;
            m_Tiled = (tiledX, tiledY);
            m_MoveSpeed = movementSpeed;
            m_AngCalc = new Vector2(MathF.Cos(MathHelper.ToRadians(m_Ang)), MathF.Sin(MathHelper.ToRadians(m_Ang)));
        }

        public BasicBackground(XmlNode bgNode,ContentManager content){
            //XmlDocument doc = new XmlDocument();
            //doc.Load(@"Content\Data\World\Rooms\TestRoom.xml");
            //XmlNode bgNode = doc.SelectSingleNode("//Room/Background");
            //Console.WriteLine( bgNode.SelectSingleNode("Name").InnerText);

            m_Texture = content.Load<Texture2D>(bgNode.SelectSingleNode("Name").InnerText);
            m_Ang = int.Parse(bgNode.SelectSingleNode("Angle").InnerText);
            m_MoveSpeed = int.Parse(bgNode.SelectSingleNode("MoveSpeed").InnerText);
            m_Tiled = (bool.Parse(bgNode.SelectSingleNode("TiledX").InnerText), bool.Parse(bgNode.SelectSingleNode("TiledY").InnerText));
            m_AngCalc = new Vector2(MathF.Cos(MathHelper.ToRadians(m_Ang)), MathF.Sin(MathHelper.ToRadians(m_Ang)));
            Console.WriteLine(m_AngCalc);
        }

        public void ChangeAngle(float newAng){
            m_Ang = newAng;
            m_AngCalc = new Vector2(MathF.Cos(MathHelper.ToRadians(m_Ang)), MathF.Sin(MathHelper.ToRadians(m_Ang)));
        }

        public void Update(GameTime gameTime){
            m_Offset += m_AngCalc * m_MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (m_Offset.X > m_Texture.Width)
                m_Offset.X = 0;
            if (m_Offset.Y > m_Texture.Height)
                m_Offset.Y = 0;
        }

        public void Draw(SpriteBatch spriteBatch){
            
            for (int x = -1; (m_Tiled.x)?x*m_Texture.Width <= spriteBatch.GraphicsDevice.Viewport.Width: x < 1 ; x++){
                for (int y = -1; (m_Tiled.y)?y*m_Texture.Height <= spriteBatch.GraphicsDevice.Viewport.Height : y < 1; y++){
                    spriteBatch.Draw(m_Texture, new Vector2(x*m_Texture.Width, y*m_Texture.Height) + m_Offset, Color.White);
                }
            }   
           
            
        }
    }

    public class StarBackground : IBackground
    {
        private Texture2D m_Texture;
        private List<Vector2> m_StarPositions = new List<Vector2>();
        private List<float> m_StarProgress = new List<float>();
        private Random PeterGriffin = new Random();
        private float m_LifeSpan = 3f;
        private float m_SpawnTime = 0.1f;
        private int m_Chance = 100;
        private float m_Rotation = 30f;
        private float m_Movespeed = 0.001f;
        public StarBackground(ContentManager content){
            m_Texture = content.Load<Texture2D>("Sprites/Star");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //RealValue = (prog > 1.5f)? 3f - prog:prog;
            for (int i = 0; i < m_StarPositions.Count; i++){
                float currentScale = (m_StarProgress[i] < (m_LifeSpan / 2)) ? (m_StarProgress[i] / (m_LifeSpan / 2)) : ((m_LifeSpan - m_StarProgress[i]) / (m_LifeSpan / 2));
                spriteBatch.Draw(m_Texture, new Vector2((m_StarPositions[i].X * spriteBatch.GraphicsDevice.Viewport.Width) - ((m_Texture.Width*currentScale)/2),(m_StarPositions[i].Y * spriteBatch.GraphicsDevice.Viewport.Height)-((m_Texture.Height*currentScale)/2)),null, Color.White,MathHelper.ToRadians( m_Rotation),new Vector2(m_Texture.Width/2,m_Texture.Height/2),currentScale,SpriteEffects.None,0f);
            }
        }

        public void Update(GameTime gameTime){
            if((int)gameTime.TotalGameTime.TotalMilliseconds%(int)(m_SpawnTime*1000) == 0){
                if (m_Chance >= 100)
                    GenerateStar();
                else if (PeterGriffin.Next(100) < m_Chance)
                    GenerateStar();
            }
            

            for (int i = 0; i < m_StarPositions.Count; i++){
                m_StarProgress[i] += (float)gameTime.ElapsedGameTime.TotalSeconds;
                m_StarPositions[i] += new Vector2((float)gameTime.ElapsedGameTime.TotalSeconds * m_Movespeed * MathF.Cos(MathHelper.ToRadians(m_Rotation)), (float)gameTime.ElapsedGameTime.TotalSeconds * m_Movespeed * MathF.Sin(MathHelper.ToRadians(m_Rotation)));
                m_Rotation += 0.1f;
                if(m_StarProgress[i] > m_LifeSpan)
                {
                    m_StarPositions.RemoveAt(i);
                    m_StarProgress.RemoveAt(i);
                }
                    
            }
            
        }

        private void GenerateStar()
        {
            float x = PeterGriffin.Next(100) / 100f;
            float y = PeterGriffin.Next(100) / 100f;
            m_StarPositions.Add(new Vector2(x, y));
            m_StarProgress.Add(0f);
        }
    }


}
