using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace pristi
{
    public partial class Game1 : Game
    {
        public void AdjustScreen(int newWidth,int newHeight){
            int outWidth = newWidth, outHeight = newHeight,xOffset = 0,yOffset = 0;
            int scaleX = (newWidth / m_Settings.GetInteralScreenWidth()), scaleY = (newHeight / m_Settings.GetInternalScreenHeight());
            scaleX = (scaleX > scaleY) ? scaleY : scaleX;
            scaleY = (scaleY > scaleX) ? scaleX : scaleY;

            outWidth = scaleX * m_Settings.GetInteralScreenWidth();
            xOffset = (newWidth - outWidth)/2;

            outHeight = scaleY * m_Settings.GetInternalScreenHeight();
            yOffset = (newHeight - outHeight) / 2;

            m_TargetRect.X = xOffset;
            m_TargetRect.Y = yOffset;
            m_TargetRect.Width = outWidth;
            m_TargetRect.Height = outHeight;
        }
    }
}
