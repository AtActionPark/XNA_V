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
    public static class ObstaclesManager
    {
        public static List<Obstacle> obstacles = new List<Obstacle>();
        public static bool toClear = false;

        public static void Update()
        {
            foreach (Obstacle obstacle in obstacles)
                if (obstacle.alive)
                    obstacle.Update();

            obstacles = obstacles.Where(x => x.alive).ToList();
            
            ChechCollision();
            if (toClear)
                Reset();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Obstacle obstacle in obstacles)
                if (obstacle.alive)
                    obstacle.Draw(spriteBatch);
        }

        public static void AddObstacle(Obstacle obstacle)
        {
            obstacles.Add(obstacle);
        }

        private static void ChechCollision()
        {
            foreach (Obstacle obstacle in obstacles)
                if(IsColliding(obstacle))
                    Player.Instance.Death();
        }

        public static void Reset()
        {
            obstacles.Clear();
            toClear = false;
            ObstacleSpawner.CreateStaticHeart();
        }

        private static bool IsColliding(Obstacle obs)
        {
            if (!Player.Instance.IsAlive)
                return false;
            Vector2 P = Player.Instance.position;
            return PointInRectangle(P, obs.p1, obs.p2, obs.p3, obs.p4)
                || IntersectsCircle(P, obs.p1, obs.p2)
                || IntersectsCircle(P, obs.p2, obs.p3)
                || IntersectsCircle(P, obs.p3, obs.p4)
                || IntersectsCircle(P, obs.p4, obs.p1);
        }

        private static bool PointInRectangle(Vector2 P, Vector2 p1, Vector2 p2,Vector2 p3, Vector2 p4)
        {
            //to implement
            return false;
        }

        private static bool IntersectsCircle2(Vector2 P, Vector2 p1, Vector2 p2)
        { 
            float dist = Vector2.Distance(p1,p2);
            float d = Math.Abs((p2.X - p1.X) * (p1.Y - P.Y) - (p1.X - P.X) * (p2.Y - p1.Y)) / dist;
            bool betweenPoint = (P.X >= p1.X && P.X <= p2.X || P.X <= p1.X && P.X >= p2.X) || (P.Y >= p1.Y && P.Y <= p2.Y || P.Y <= p1.Y && P.Y >= p2.Y);
            return d < Player.Instance.radius && betweenPoint;  
        }

        private static bool IntersectsCircle(Vector2 c, Vector2 a, Vector2 b)
        {
            float A = (b.X - a.X)*(b.X - a.X) + (b.Y - a.Y)*(b.Y - a.Y);
            float B = 2 * ((b.X - a.X) * (a.X - c.X) + (b.Y - a.Y) * (a.Y - c.Y));
            float C = a.X * a.X + a.Y * a.Y + c.X * c.X + c.Y * c.Y - 2 * (a.X * c.X + a.Y * c.Y) - Player.Instance.radius*Player.Instance.radius;
            float delta = B * B - 4 * A * C;

            if (delta >= 0)
            {
                float test = ((c.X - a.X) * (b.X - a.X) + (c.Y - a.Y) * (b.Y - a.Y)) / ((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));
                if (test <= 1 && test >= 0)
                    return true;
            }
            return false;
            
        }
    }
}
