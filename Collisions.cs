using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace pristi
{
    public static class Collisions{
        static float margin = 0.1f;
        public static (Vector2 intersect,bool collide) LineLine(Vector2 p1,Vector2 p2,Vector2 p3,Vector2 p4)
        {
            float linelen = (p2 - p1).Length();
            

            float uA = ((p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X)) / ((p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y));

            float uB = ((p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X)) / ((p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y));
            float intersectionX = p1.X + (uA * (p2.X - p1.X));
            float intersectionY = p1.Y + (uA * (p2.Y - p1.Y));
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
                return (new Vector2(intersectionX, intersectionY), true);
            else return (new Vector2(float.NaN, float.NaN), false);
        }

        public static (Vector2 intersect1,Vector2 intersect, bool collide) RectLine(Vector2 rectPos,Vector2 rectSize,Vector2 p1,Vector2 p2) {
            (Vector2 intersect, bool collide)[] lineLines =
            {
                LineLine(rectPos,rectPos + new Vector2(rectSize.X,0),p1,p2),
                LineLine(rectPos,rectPos + new Vector2(0,rectSize.Y),p1,p2),
                LineLine(rectPos + new Vector2(rectSize.X,0),rectPos + rectSize,p1,p2),
                LineLine(rectPos  + new Vector2(0,rectSize.Y),rectPos + rectSize,p1,p2)
            };
            bool collided = false;
            Vector2[] intersects = new Vector2[] { new Vector2(float.NaN, float.NaN), new Vector2(float.NaN, float.NaN) };
            Vector2 i1 = new Vector2(float.NaN,float.NaN);
            Vector2 i2 = new Vector2(float.NaN, float.NaN);
            int j = 0;
            for (int i = 0; i < lineLines.Length; i++){
                if (lineLines[i].collide)
                {
                    collided = true;
                    intersects[j%2] = lineLines[i].intersect;
                    j++;
                }

            }
          
            return (intersects[0], intersects[1], collided);
            
        }
    }
}
