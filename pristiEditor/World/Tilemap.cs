using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using pristi.Graphics;
using System.Xml;

namespace pristi.World
{
    class Tilemap{
        private int m_Width, m_Height;
        public int GetWidth() => m_Width;
        public int GetHeight() => m_Height;
        private int[,] m_TileArray;
        private Tileset m_Tileset;

        public Tilemap(int[,] tileArray,Tileset tileset){
            m_TileArray = tileArray;
            m_Width = m_TileArray.GetLength(1);
            m_Height = m_TileArray.GetLength(0);
            m_Tileset = tileset;
            
        }

        public Tilemap(XmlNode tilemapNode, ContentManager content,int tileWidth,int tileHeight){
            m_Tileset = new Tileset(content.Load<Texture2D>(tilemapNode.SelectSingleNode("Tileset").InnerText), tileWidth, tileHeight);

            XmlNodeList rowNodes = tilemapNode.SelectNodes("TileArray/Row");
            m_Height = rowNodes.Count;
            m_Width = rowNodes[0].InnerText.Split(',').Length;

            m_TileArray = new int[m_Height, m_Width];
            
            for(int y = 0; y < rowNodes.Count; y++){
                string[] tileStrings = rowNodes[y].InnerText.Split(',');

                for (int x = 0; x < tileStrings.Length; x++){
                    m_TileArray[y, x] = int.Parse(tileStrings[x]);
                }
            }


            Console.WriteLine("tilemap loaded.");
        }


        public void Draw(SpriteBatch spriteBatch,Vector2 pos,float scale = 1f){
            for (int y = 0; y < m_Height; y++) {
                for (int x = 0; x < m_Width; x++){
                    m_Tileset.DrawTile(spriteBatch, m_TileArray[y, x],pos + new Vector2(x*m_Tileset.GetTileWidth()*scale,y*m_Tileset.GetTileHeight()*scale),scale);
                }
            }
        }

    }
}
