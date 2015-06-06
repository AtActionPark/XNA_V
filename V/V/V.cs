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
    class V
    {
        int size = 3;
        Color color = Color.White;
        bool growingP1;
        bool growingP2;
        bool growingP3;
        bool growingP4;
        bool growingP5;
        public Vector2 point1 = new Vector2(0, 0);
        public Vector2 point2 = new Vector2(Game1.size.X / 3.0f, Game1.size.Y * 3.0f / 4.0f);
        public Vector2 point3 = new Vector2(Game1.size.X / 2.0f, Game1.size.Y);
        public Vector2 point4 = new Vector2(Game1.size.X * 2.0f / 3.0f, Game1.size.Y * 3.0f / 4.0f);
        public Vector2 point5 = new Vector2(Game1.size.X, 0);
        public bool moveP1 = false;
        public bool moveP2 = false;
        public bool moveP3 = false;
        public bool moveP4 = false;
        public bool moveP5 = false;

        private static V instance;
        public static V Instance
        {
            get
            {
                if (instance == null)
                    return new V();
                return instance;
            }
        }

        public V()
        {
            instance = this;
        }

        public void Update()
        {
            if (moveP1)
                MoveP1(0, Game1.size.Y/2, 2);
            if (moveP5)
                MoveP5(0, Game1.size.Y/2, 2);
            if (moveP3)
                MoveP3(Game1.size.Y / 2 + 30, Game1.size.Y, 1);
            if(moveP2)
                MoveP2(100, Game1.size.X / 2 - 100, 1);
            if (moveP4)
                MoveP4(Game1.size.X / 2 + 100, Game1.size.X - 100, 1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Primitive.DrawLineSegment(spriteBatch, point1, point2, color, size);
            Primitive.DrawLineSegment(spriteBatch, point2, point3, color, size);
            Primitive.DrawLineSegment(spriteBatch, point3, point4, color, size);
            Primitive.DrawLineSegment(spriteBatch, point4, point5, color, size);
        }

        public void MoveP3(float min, float max, int speed)
        {
            if (growingP3)
                point3.Y-=speed;
            else
                point3.Y+=speed;
            if (point3.Y > max)
                growingP3 = true;
            if (point3.Y < min)
                growingP3 = false;
        }

        public void MoveP1(float min, float max, int speed)
        {
            if (growingP1)
                point1.Y -= speed;
            else
                point1.Y += speed;
            if (point1.Y > max)
                growingP1 = true;
            if (point1.Y < min)
                growingP1 = false;
        }

        public void MoveP5(float min, float max, int speed)
        {
            if (growingP5)
                point5.Y -= speed;
            else
                point5.Y += speed;
            if (point5.Y > max)
                growingP5 = true;
            if (point5.Y < min)
                growingP5 = false;
        }

        public void MoveP2(float min, float max, int speed)
        {
            if (growingP2)
                point2.X+=speed;
            else
                point2.X-=speed;
            if (point2.X < 100)
                growingP2 = true;
            if (point2.X > Game1.size.X / 2 - 100)
                growingP2 = false;
        }

        public void MoveP4(float min, float max, int speed)
        {
            if (growingP4)
                point4.X-=speed;
            else
                point4.X+=speed;
            if (point4.X > max)
                growingP4 = true;
            if (point4.X < min)
                growingP4 = false;
        }

        public void ResetPosition()
        {
            moveP3 = false;
            moveP2 = false;
            moveP4 = false;
            moveP1 = false;
            moveP5 = false;
            point1 = new Vector2(0, 0);
            point2 = new Vector2(Game1.size.X / 3.0f, Game1.size.Y * 3.0f / 4.0f);
            point3 = new Vector2(Game1.size.X / 2.0f, Game1.size.Y);
            point4 = new Vector2(Game1.size.X * 2.0f / 3.0f, Game1.size.Y * 3.0f / 4.0f);
            point5 = new Vector2(Game1.size.X, 0);
        }

    }
}
