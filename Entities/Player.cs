using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace pristi.Entities
{
    public class Player : TexturedEntity
    {
        protected Vector2 Velocity = new Vector2(0, 0);
        float MaxMoveSpeed = 96f;
        float AccelSpeed = 64f;
        float FallSpeed = 96f;
        
        public Player(Vector2 position, Vector2 size, Texture2D texture, string name) : base(position, size, texture, name){
            Moving = true;
        }

        public new void Update(GameTime gameTime,List<List<Vector2>> collisionVerts = null)
        {
            if (GlobalInput.Manager.MoveLeft.IsDown)
                Velocity.X = -MaxMoveSpeed;
            else if (GlobalInput.Manager.MoveRight.IsDown)
                Velocity.X = MaxMoveSpeed;
            else
                Velocity.X = 0;

            //Velocity.Y += FallSpeed*(float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity.Y = FallSpeed;
            if (GlobalInput.Manager.Jump.IsDown)
                Velocity.Y = -FallSpeed;

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime,collisionVerts);
        }
    }
}
