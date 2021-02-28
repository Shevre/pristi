using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using pristi.Graphics;
using System;
using Microsoft.Xna.Framework;
using MonoGame;
using pristi.Entities;

namespace pristi.World
{
    public class Room{
        private List<Tilemap> m_PreEntityTilemaps = new List<Tilemap>();
        private List<Tilemap> m_PostEntityTilemaps = new List<Tilemap>();
        List<List<Vector2>> m_CollisionVerts = new List<List<Vector2>>();
        private Player Player;
        //private IBackground m_Background;
        private int m_EntityLayer;


        public Room(string xmlLocation,ContentManager content,Player player){
            LoadFromXml(xmlLocation,content);
            Player = player;
        }

        private void LoadFromXml(string xmlLocation,ContentManager content){
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlLocation);
            //m_Background = new BasicBackground(xmlDoc.SelectSingleNode("//Room/Background"), content);
            //m_Background = new StarBackground(content);
            m_EntityLayer = int.Parse(xmlDoc.SelectSingleNode("//Room/EntityLayer").InnerText);
            XmlNodeList tilemapNodes = xmlDoc.SelectNodes("//Room/Tilemaps/Tilemap");
            Console.WriteLine(tilemapNodes.Count);
            for (int i = 0; i < tilemapNodes.Count; i++){
                if(i < m_EntityLayer){
                    m_PreEntityTilemaps.Add(new Tilemap(tilemapNodes[i], content, StaticSettings.GetTileWidth(), StaticSettings.GetTileHeight()));
                }
                else
                {
                    m_PostEntityTilemaps.Add(new Tilemap(tilemapNodes[i], content, StaticSettings.GetTileWidth(), StaticSettings.GetTileHeight()));
                }
            }

            XmlNodeList collisionPolygonNodes = xmlDoc.SelectNodes("//Room/CollisionData/Polygon");
            foreach (XmlNode polygonNode in collisionPolygonNodes){
                string[] posStrings = polygonNode.SelectSingleNode("Position").InnerText.Split(',');
                Vector2 position = new Vector2(float.Parse(posStrings[0]), float.Parse(posStrings[1]));
                m_CollisionVerts.Add(new List<Vector2>());
                string[] vertStrings = polygonNode.SelectSingleNode("Verts").InnerText.Split('|');
                foreach (string s in vertStrings){
                    m_CollisionVerts[m_CollisionVerts.Count - 1].Add(new Vector2(float.Parse(s.Split(',')[0]) + position.X, float.Parse(s.Split(',')[1]) + position.Y));
                }

            }
        }

        public void Update(GameTime gameTime){
            //m_Background.Update(gameTime);
            Player.Update(gameTime, m_CollisionVerts);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawPreEntities(spriteBatch);
            Player.Draw(spriteBatch);
            DrawPostEntities(spriteBatch);
        }

        public void DrawNative(SpriteBatch spriteBatch)
        {
            Player.DrawNative(spriteBatch);
        }


        public void DrawPreEntities(SpriteBatch spriteBatch){
            //m_Background.Draw(spriteBatch);
            foreach (Tilemap tilemap in m_PreEntityTilemaps){
                tilemap.Draw(spriteBatch);
            }
        }

        public void DrawPostEntities(SpriteBatch spriteBatch){
            foreach (Tilemap tilemap in m_PostEntityTilemaps){
                
                tilemap.Draw(spriteBatch);
                
            }
            if(GlobalInput.Manager.Debug.IsToggled) DrawHitboxes(spriteBatch);
        }

        private void DrawHitboxes(SpriteBatch spriteBatch){
            for (int i = 0; i < m_CollisionVerts.Count; i++){
                for (int j = 0; j < m_CollisionVerts[i].Count; j++){
                    bool collided = false;
                    foreach ((int i1,int i2) item in Player.LastCollidedVerts){
                        if (item.i1 == i && item.i2 == j)
                            collided = true;
                    }
                    spriteBatch.DrawLine(m_CollisionVerts[i][j], m_CollisionVerts[i][(j + 1) % m_CollisionVerts[i].Count],(collided)? Color.DarkOrange:Color.Cyan);
                }
            }
        }
    }
}
