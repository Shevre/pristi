using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace pristi.Graphics
{
    public class TilesetManager{
        private List<Tileset> m_Tilesets = new List<Tileset>();
        private string m_TilesetLocation;
        private ContentManager m_ContentManager;
        public TilesetManager(string tilesetLocation,ContentManager contentManager){
            m_TilesetLocation = tilesetLocation;
            m_ContentManager = contentManager;
        }

       
    }
}
