using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace pristi
{
    public partial class Game1 : Game
    {
        public static Vector2 ScreenScale { get; private set; } = new Vector2(0, 0);
        public static Vector2 ScreenOffset { get; private set; } = new Vector2(0, 0);
        public void AdjustScreen(int newWidth,int newHeight){
            int outWidth = newWidth, outHeight = newHeight,xOffset = 0,yOffset = 0;
            int scaleX = (newWidth / Settings.GetInteralScreenWidth()), scaleY = (newHeight / Settings.GetInternalScreenHeight());
            scaleX = (scaleX > scaleY) ? scaleY : scaleX;
            scaleY = (scaleY > scaleX) ? scaleX : scaleY;

            outWidth = scaleX * Settings.GetInteralScreenWidth();
            xOffset = (newWidth - outWidth)/2;

            outHeight = scaleY * Settings.GetInternalScreenHeight();
            yOffset = (newHeight - outHeight) / 2;

            ScreenScale = new Vector2(scaleX, scaleY);
            ScreenOffset = new Vector2(xOffset, yOffset);
            m_TargetRect.X = xOffset;
            m_TargetRect.Y = yOffset;
            m_TargetRect.Width = outWidth;
            m_TargetRect.Height = outHeight;
        }
    }
}
