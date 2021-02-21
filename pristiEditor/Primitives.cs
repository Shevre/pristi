using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pristiEditor
{
    public static class Primitives{
        public static void DrawRectangle(this SpriteBatch spriteBatch,Texture2D whitePixel,Rectangle rect,Color color,int thickness = 1,float scale = 1f){
            spriteBatch.Draw(whitePixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            spriteBatch.Draw(whitePixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            spriteBatch.Draw(whitePixel, new Rectangle(rect.X + rect.Width, rect.Y, thickness, rect.Height), color);
            spriteBatch.Draw(whitePixel, new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, thickness), color);
        }

        public static void FillRectangle(this SpriteBatch spriteBatch,Texture2D whitePixel,Rectangle rect,Color color){
            spriteBatch.Draw(whitePixel, rect, color);
        }


    }
}
