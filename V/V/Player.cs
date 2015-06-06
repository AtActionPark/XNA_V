using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace V
{
    class Player
    {
        Texture2D texture;
        public Vector2 position;
        Color color = Color.White;
        V v;
        public bool isMoving = false;
        bool steep = false;
        int speed { get { return steep ? 10 : 12; } }
        int gravSpeed { get { return steep ? 7 : 5; } }
        public float scale = 1;
        bool growing = true;
        private static Player instance;
        public static Player Instance
        {
            get
            { 
                if(instance == null)
                    return new Player();
                return instance;
            }
        }
        public bool gravity = false;
        public float mod = 0.6f;
        public int inverse = 1;
        public float radius = 9;
        public bool growth = false;

        public bool IsAlive = true;

        public Player()
        {
            texture = Art.Balltexture;
            v = V.Instance;
            position = v.point3;
            instance = this;
        }

        public void Update(KeyboardState keyboardState)
        {
            UpdatePosition(keyboardState);
            isMoving = keyboardState.IsKeyDown(Keys.Left)  || keyboardState.IsKeyDown(Keys.Right);
            if (growth)
                Grow();
            //Pulse();
        }

        void UpdatePosition(KeyboardState keyboardState)
        {
            int dir;
            if (!IsAlive)
            {
                position = v.point3;
                return;
            }
            if (v.point3.Y < v.point2.Y && position.X > v.point2.X && position.X < v.point4.X )
                dir = -1;
            else
                dir = 1;
            if (gravity)
            {
                if (position.Y < Game1.size.Y - 2)
                {
                    if (position.X < v.point3.X)
                        position.X += gravSpeed * dir;
                    else if (position.X > v.point3.X)
                        position.X -= gravSpeed * dir;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Left))
                position.X -= speed*mod*inverse;
            if (keyboardState.IsKeyDown(Keys.Right))
                position.X += speed*mod*inverse;

            if (position.X <= v.point2.X)
            {
                position.Y = v.point1.Y + (position.X - v.point1.X) * (v.point1.Y - v.point2.Y) / (v.point1.X - v.point2.X);
                steep = true;
            }

            else if (position.X <= v.point3.X)
            {
                position.Y = v.point2.Y + (position.X - v.point2.X) * (v.point2.Y - v.point3.Y) / (v.point2.X - v.point3.X);
                steep = false;
            }

            else if (position.X <= v.point4.X)
            {
                position.Y = v.point3.Y - (position.X - v.point3.X) * -(v.point3.Y - v.point4.Y) / (v.point3.X - v.point4.X);
                steep = false;
            }

            else if (position.X <= v.point5.X)
            {
                position.Y = v.point4.Y - (position.X - v.point4.X) * -(v.point4.Y - v.point5.Y) / (v.point4.X - v.point5.X);
                steep = true;
            }

            if (position.X < 0)
                position.X = 0;
            if (position.X > Game1.size.X)
                position.X = Game1.size.X;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(IsAlive)
                spriteBatch.Draw(texture, position - new Vector2(scale*texture.Width/2, scale*texture.Height/2), null, color, 0f,Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        void Pulse()
        {
            if (growing)
            {
                scale += .05f;
            }
            if (!growing)
            {
                scale -= .05f;
            }
            if (scale > 2)
                growing = false;
            if (scale < 0.5f)
                growing = true;
        }

        public void Reset()
        {
            IsAlive = true;
            gravity = false;
            mod = 0.6f;
            inverse = 1;
            scale = 1;
            radius = 9;
            growth = false;
        }

        public void Death()
        {
            IsAlive = false;
            ObstaclesManager.toClear = true;
            PlayerStatus.SaveHighScore();
        }

        public void Grow()
        {
            if(scale<=2)
                scale += 0.01f;
            if(radius<=18)
                radius += 0.1f;
        }
    }
}
