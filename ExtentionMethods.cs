using pristi.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace pristi
{
    public static class ExtentionMethods
    {
        public static Tileset LoadTileset(this Microsoft.Xna.Framework.Content.ContentManager content, string contentName)
        {
            //tilesetname - tilesets/testRoom
            return new Tileset(content.Load<Texture2D>(contentName), StaticSettings.GetTileWidth(), StaticSettings.GetTileHeight());
        }
    }
}
