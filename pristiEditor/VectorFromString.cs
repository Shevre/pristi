using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace pristiEditor
{
    public static class VectorFromString{
        public static Vector2 ToVector2(this string s)
        {
            
            return new Vector2(float.Parse(s.Split(',')[0]),float.Parse(s.Split(',')[1]));
        }
    }
}
