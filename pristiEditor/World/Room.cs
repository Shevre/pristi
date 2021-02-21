using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using pristi.Graphics;
using System;
using Microsoft.Xna.Framework;

namespace pristi.World
{
    public class Room{
        private List<Tilemap> m_PreEntityTilemaps = new List<Tilemap>();
        private List<Tilemap> m_PostEntityTilemaps = new List<Tilemap>();
        private IBackground m_Background;
        private int m_EntityLayer;

        public int GetWidth() => m_PreEntityTilemaps[0].GetWidth();
        public int GetHeight() => m_PreEntityTilemaps[0].GetHeight();


        public Room(string xmlLocation,ContentManager content,int tileWidth = 16,int tileHeight = 16){
            LoadFromXml(xmlLocation,content,tileWidth,tileHeight);

        }

        

        private void LoadFromXml(string xmlLocation,ContentManager content,int tileWidth,int tileHeight){
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlLocation);
            //m_Background = new BasicBackground(xmlDoc.SelectSingleNode("//Room/Background"), content);
            m_Background = new StarBackground(content);
            m_EntityLayer = int.Parse(xmlDoc.SelectSingleNode("//Room/EntityLayer").InnerText);
            XmlNodeList tilemapNodes = xmlDoc.SelectNodes("//Room/Tilemaps/Tilemap");
            Console.WriteLine(tilemapNodes.Count);
            for (int i = 0; i < tilemapNodes.Count; i++){
                if(i < m_EntityLayer){
                    m_PreEntityTilemaps.Add(new Tilemap(tilemapNodes[i], content, tileWidth, tileHeight));
                }
                else
                {
                    m_PostEntityTilemaps.Add(new Tilemap(tilemapNodes[i], content, tileWidth, tileHeight));
                }
            }
        }

        public void Update(GameTime gameTime){
            m_Background.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch,Vector2 pos,float scale = 1f, int highlightedLayer = -1)
        {
            
            if (highlightedLayer == -1)
            {
                foreach (Tilemap tilemap in m_PreEntityTilemaps)
                {
                    tilemap.Draw(spriteBatch,pos,scale);
                }
                foreach (Tilemap tilemap in m_PostEntityTilemaps)
                {
                    tilemap.Draw(spriteBatch,pos,scale);
                }
            }
            else
            {

            }
        }

        //public void DrawPreEntities(SpriteBatch spriteBatch){
        //    
        //    foreach (Tilemap tilemap in m_PreEntityTilemaps){
        //        tilemap.Draw(spriteBatch);
        //    }
        //}

        //public void DrawPostEntities(SpriteBatch spriteBatch){
        //    foreach (Tilemap tilemap in m_PostEntityTilemaps){
                
        //        tilemap.Draw(spriteBatch);
        //    }
        //}
    }
}
