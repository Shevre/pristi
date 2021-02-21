using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pristi.Graphics
{
    public class Tileset{
        private Texture2D m_TilesetTexture;
        private int m_TileWidth, m_TileHeight,m_TileCount,m_Width,m_Height;




        public int GetTileHeight() => m_TileHeight;
        public int GetTileWidth() => m_TileWidth;

        public Tileset(Texture2D texture,int tileWidth,int tileHeight){
            m_TilesetTexture = texture;
            m_TileWidth = tileWidth;
            m_TileHeight = tileHeight;
            m_Width = (m_TilesetTexture.Width / tileWidth);
            m_Height = (m_TilesetTexture.Height / tileHeight);
            m_TileCount = m_Width * m_Height;
        }

        

       
       
        

        //public void ReplaceTile(int tileID,Texture2D tex){
        //    m_Tiles[tileID] = tex;
        //}

        public void DrawTileset(SpriteBatch spriteBatch,Vector2 pos,int width = 5){
            for (int i = 0; i < m_TileCount; i++){
                DrawTile(spriteBatch, i+1, new Vector2(pos.X + (m_TileWidth * (i % width)), pos.Y + (m_TileHeight * (i / width))));
            }
        }

        private Rectangle GetSrcRect(int tileID){
            int tileid = tileID - 1;
            int x = (tileid%m_Width)*m_TileWidth;
            int y = (tileid/m_Width)*m_TileHeight;
            return new Rectangle(x, y, m_TileWidth, m_TileHeight);
        }

        public void DrawTile(SpriteBatch spritebatch,int tileID,Vector2 pos ,float scale = 1f){
            if (tileID != 0) spritebatch.Draw(m_TilesetTexture,pos,GetSrcRect(tileID),Color.White,0f,new Vector2(0,0),scale,SpriteEffects.None,0f);
        }
        public void DrawTile(SpriteBatch spritebatch, int tileID, Vector2 pos,Color color){
            if (tileID != 0) spritebatch.Draw(m_TilesetTexture, pos, GetSrcRect(tileID), color);
        }
        
    }
}
