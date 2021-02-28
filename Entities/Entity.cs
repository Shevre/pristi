using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;

namespace pristi.Entities
{
    public class Entity
    {
        public Vector2 Position { get; protected set; }
        protected Vector2 PrevPos;
        public Vector2 Size { get; protected set; }
        public string Name { get; protected set; }
        public bool Moving { get; protected set; }
        public bool Collidable { get; protected set; }
        public bool Grounded { get; protected set; } = false;
        public (int polyId, int vertId)[] LastCollidedVerts { get; protected set; }

        public Entity(Vector2 position,Vector2 size,string name) {
            Position = position;
            Size = size;
            Name = name;
            if (Moving) Collidable = true;
        }

        public void Update(GameTime gameTime,List<List<Vector2>> collisionVerts = null){
            if (Moving)
            {
                if(collisionVerts != null)
                {
                    LastCollidedVerts = DoCollision(collisionVerts);
                }
            }
            PrevPos = Position;
        }
        private string CollisionInfos = "";
        protected (int polyId,int vertId)[] DoCollision(List<List<Vector2>> collisionVerts){
            List<(int i1, int i2)> collidedLines = new List<(int i1, int i2)>();
            CollisionInfos = "";
            for (int i = 0; i < collisionVerts.Count; i++){
                for (int j = 0; j < collisionVerts[i].Count; j++){
                    (Vector2 i1, Vector2 i2, bool collide) a = Collisions.RectLine(Position, Size, collisionVerts[i][j], collisionVerts[i][(j + 1) % collisionVerts[i].Count]);
                    if (a.collide)
                    {
                        CollisionInfos += a.ToString() + "\n";
                        //Position = a.i1 - new Vector2((PrevPos.X > a.i1.X)?0:Size.X,(PrevPos.Y < a.i1.Y)?Size.Y:0f);
                        collidedLines.Add((i, j));
                    }
                        
                }
            }

            return collidedLines.ToArray();
        }

        public void Draw(SpriteBatch spriteBatch){
            if (GlobalInput.Manager.Debug.IsToggled){
                spriteBatch.DrawRectangle(Position, Size, Color.Red);
                
            }

        }

        public void DrawNative(SpriteBatch spriteBatch){
            if (GlobalInput.Manager.Debug.IsToggled) {
                spriteBatch.FillRectangle((Position * Game1.ScreenScale) + Game1.ScreenOffset, Game1.DebugFont.MeasureString(Name), Color.Red);
                spriteBatch.DrawString(Game1.DebugFont, Name, (Position * Game1.ScreenScale) + Game1.ScreenOffset, Color.Black);
                spriteBatch.DrawString(Game1.DebugFont, CollisionInfos, new Vector2(Game1.Settings.GetScreenWidth() - Game1.DebugFont.MeasureString(CollisionInfos).X, 0), Color.Magenta);
            }
        }

       

    }

    public class TexturedEntity : Entity
    {
        protected Texture2D Texture;
        public TexturedEntity(Vector2 position, Vector2 size,Texture2D texture, string name) : base(position, size, name){
            Texture = texture;
        }

        public new void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(Texture, Position, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
