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
    public static class Primitive
    {
        public static void DrawLineSegment(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, int lineWidth)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

        spriteBatch.Draw(Game1.whitePixel, point1, null, color,
                        angle, Vector2.Zero, new Vector2(length, lineWidth),
                        SpriteEffects.None, 0f);
        }

        public static void DrawLineSegment(SpriteBatch spriteBatch, Vector3 point1, Vector3 point2, Color color, float lineWidth)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector3.Distance(point1, point2);
            Vector2 point = new Vector2(point1.X, point1.Y);

            spriteBatch.Draw(Game1.whitePixel, point, null, color,
                            angle, Vector2.Zero, new Vector2(length, lineWidth),
                            SpriteEffects.None, 0f);
        }

        public static void DrawPolygon(SpriteBatch spriteBatch, Vector2[] vertex, int count, Color color, int lineWidth)
        {
            if (count > 0)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    DrawLineSegment(spriteBatch, vertex[i], vertex[i + 1], color, lineWidth);
                }

                DrawLineSegment(spriteBatch, vertex[count - 1], vertex[0], color, lineWidth);
            }
        }

        public static void DrawCircle(SpriteBatch spritbatch, Vector2 center, float radius, Color color, int lineWidth, int segments = 16)
        {

            Vector2[] vertex = new Vector2[segments];

            double increment = Math.PI * 2.0 / segments;
            double theta = 0.0;

            for (int i = 0; i < segments; i++)
            {
                vertex[i] = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                theta += increment;
            }

            DrawPolygon(spritbatch, vertex, segments, color, lineWidth);
        }
    }
}
