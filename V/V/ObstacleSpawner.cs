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
    public enum Direction { left, right, down, none }

    public static class ObstacleSpawner
    {
        public static float inverseSpawnChance = 1500;
        static Random random = new Random();
        public static bool spawn = true;
        static float time;
        static float obstPerTickTimer;
        public static float timeBetweenObstacles = 2000;
        static float specialTimer;
        public static int obstPerTick = 1;
        public static int poolNb = 1;
        public static bool attack = false;

        public static void Update(GameTime gameTime)
        {
            if (!spawn)
                return;

            if (attack)
                if (random.Next((int)inverseSpawnChance) == 0)
                {
                    CreateRandom();
                    CreateSeeker();
                }

                time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                obstPerTickTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                specialTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (time > timeBetweenObstacles)
                {
                    time = 0;
                    for (int i = 0; i < obstPerTick; i++)
                        ChooseObstacle();

                    if (poolNb <= 18)
                        poolNb++;

                    if (timeBetweenObstacles > 500)
                        timeBetweenObstacles-=GamePlayState.timeBetweenObstaclesDecrement;
                }

                if (obstPerTickTimer > GamePlayState.obstPerTickTimer && obstPerTick <GamePlayState.maxObsPerTick)
                {
                    obstPerTick++;
                    obstPerTickTimer = 0;
                }

                if (specialTimer > GamePlayState.specialTimer)
                {
                    SpecialMove();
                    specialTimer = random.Next(8000);
                }
        }

        public static void Reset()
        {
            obstPerTick = 1;
            timeBetweenObstacles = 2000;
            specialTimer = 0;
            poolNb = 1;
            inverseSpawnChance = 60;
            spawn = true;
            attack = false;
            specialTimer = random.Next(8000);
        }

        private static Vector2 GetPosition()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(random.Next((int)Game1.size.X), random.Next((int)Game1.size.Y));
            }
            while (Vector2.DistanceSquared(pos, Player.Instance.position) < 250 * 250);
            return pos;
        }

        private static Vector2 GetRandomAcceleration()
        {
            Vector2 acc = new Vector2(random.Next(-10, 10), random.Next(-10, 10));
            acc.Normalize();
            acc /= 10;
            return acc;
        }

        private static void ChooseObstacle()
        {

            int r = random.Next(0, poolNb);
            if (r == 0)
                CreateVertical();
            else if (r == 1)
                CreateHorizontal(Direction.right);
            else if (r == 2)
                CreateHorizontal(Direction.left);
            else if (r == 3)
                CreateHeart();
            else if (r == 4)
                CreateTunnel1();
            else if (r == 5)
                CreateTunnel2();
            else if (r == 6)
                CreateRain();
            else if (r == 7)
                CreateArrows(Direction.right);
            else if (r == 8)
                CreateArrows(Direction.left);
            else if (r == 9)
                CreateDoubleTunnel();
            else if (r == 10)
                CreateRain(Direction.left);
            else if (r == 11)
                CreateRain(Direction.right);
            else if (r == 12)
                CreateChange(Direction.left);
            else if (r == 13)
                CreateChange(Direction.right);
            else if (r == 14)
                CreateDoubleRain();
            else if (r == 15)
                CreateChange(Direction.down);
            else if (r == 16)
                CreateFollowLine(Direction.left);
            else if (r == 17)
                CreateChange(Direction.right);
        }

        #region Obstacles type
        private static void CreateRandom()
        {
           Obstacle obs = new Obstacle(GetPosition(), GetRandomAcceleration(), new Vector2(40,40));
           obs.color = Color.White;
           ObstaclesManager.AddObstacle(obs);
        }

        private static void CreateSeeker()
        {
            Vector2 pos = GetPosition();
            Vector2 acc = Player.Instance.position - pos;
            acc.Normalize();
            acc /= 10;
            Obstacle obs = new Obstacle(pos, acc, new Vector2(20, 20));
            obs.color = Color.Red;
            ObstaclesManager.AddObstacle(obs);
        }

        private static void CreateTunnel1()
        {
            Vector2 pos = new Vector2 (0,0);
            Vector2 acc = new Vector2(1,1);
            acc.Normalize();
            acc /= 10;
            Obstacle obs = new Obstacle(pos,acc, new Vector2(100,300));
            obs.color = Color.Blue;
            obs.angle = MathHelper.ToRadians(-45);
            ObstaclesManager.AddObstacle(obs);
            pos += new Vector2(105, -105);
            Obstacle obs2 = new Obstacle(pos, acc,new Vector2(100, 300));
            obs2.color = Color.Blue;
            obs2.angle = MathHelper.ToRadians(-45);
            ObstaclesManager.AddObstacle(obs2);

        }

        private static void CreateTunnel2()
        {
            Vector2 pos = new Vector2(Game1.size.X, 0);
            Vector2 acc = new Vector2(-1, 1);
            acc.Normalize();
            acc /= 10;
            Obstacle obs = new Obstacle(pos, acc, new Vector2(100, 300));
            obs.color = Color.Blue;
            obs.angle = MathHelper.ToRadians(45);
            ObstaclesManager.AddObstacle(obs);
            pos += new Vector2(-105, -105);
            Obstacle obs2 = new Obstacle(pos, acc, new Vector2(100, 300));
            obs2.color = Color.Blue;
            obs2.angle = MathHelper.ToRadians(45);
            ObstaclesManager.AddObstacle(obs2);

        }

        private static void CreateDoubleTunnel()
        {
            CreateTunnel1();
            CreateTunnel2();
        }

        private static void CreateVertical()
        {
            Vector2 pos = new Vector2(random.Next(0, (int)Game1.size.X), 0);
            Vector2 acc = new Vector2(0, 1);
            acc.Normalize();
            acc /= 5;
            Obstacle obs = new Obstacle(pos,acc, new Vector2(50,200));
            obs.color = Color.Green;
            obs.angle = MathHelper.ToRadians(0);
            ObstaclesManager.AddObstacle(obs);

            Obstacle obs2 = new Obstacle(pos, acc, new Vector2(44, 194));
            obs2.color = Color.LawnGreen;
            obs2.angle = MathHelper.ToRadians(0);
            ObstaclesManager.AddObstacle(obs2);
        }

        private static void CreateHorizontal(Direction direction)
        {
            Vector2 pos ;
            Vector2 acc;
            if (direction == Direction.left)
            {
                pos = new Vector2(0, random.Next(0, (int)Game1.size.Y));
                acc = new Vector2(1, 0);
            }

            else
            {
                pos = new Vector2(Game1.size.X, random.Next(0, (int)Game1.size.Y));
                acc = new Vector2(-1, 0);
            }
                
            acc.Normalize();
            acc /= 5;
            Obstacle obs = new Obstacle(pos, acc, new Vector2(200, 50));
            obs.color = Color.LawnGreen;
            obs.angle = MathHelper.ToRadians(0);
            ObstaclesManager.AddObstacle(obs);

            Obstacle obs2 = new Obstacle(pos, acc, new Vector2(194, 44));
            obs2.color = Color.Green;
            obs2.angle = MathHelper.ToRadians(0);
            ObstaclesManager.AddObstacle(obs2);
        }

        private static void CreateHeart()
        {
            Vector2 pos = new Vector2(Game1.size.X/2, 10);
            Vector2 acc = new Vector2(0, 1);
            acc.Normalize();
            acc /= 5;
            Obstacle obs = new Obstacle(pos, acc, new Vector2(50, 50));
            obs.color = Color.Red;
            obs.angle = MathHelper.ToRadians(45);
            ObstaclesManager.AddObstacle(obs);

            pos += new Vector2(-37, -37);
            Obstacle obs2 = new Obstacle(pos, acc, new Vector2(50, 50));
            obs2.color = Color.Red;
            obs2.angle = MathHelper.ToRadians(45);
            ObstaclesManager.AddObstacle(obs2);

            pos += new Vector2(74, 0);
            Obstacle obs3 = new Obstacle(pos, acc, new Vector2(50, 50));
            obs3.color = Color.Red;
            obs3.angle = MathHelper.ToRadians(45);
            ObstaclesManager.AddObstacle(obs3);


        }

        public static void CreateStaticHeart()
        {
            Vector2 pos = new Vector2(Game1.size.X / 2, Game1.size.Y/2);
            Vector2 acc = new Vector2(0, 0);
            Obstacle obs = new Obstacle(pos, acc, new Vector2(50, 50));
            obs.color = Color.White;
            obs.angle = MathHelper.ToRadians(45);
            obs.isSpecial = true;
            obs.specialDirection = Direction.down;
            ObstaclesManager.AddObstacle(obs);

            pos += new Vector2(-37, -37);
            Obstacle obs2 = new Obstacle(pos, acc, new Vector2(50, 50));
            obs2.color = Color.White;
            obs2.angle = MathHelper.ToRadians(45);
            obs2.isSpecial = true;
            obs2.specialDirection = Direction.left;
            ObstaclesManager.AddObstacle(obs2);

            pos += new Vector2(74, 0);
            Obstacle obs3 = new Obstacle(pos, acc, new Vector2(50, 50));
            obs3.color = Color.White;
            obs3.angle = MathHelper.ToRadians(45);
            obs3.isSpecial = true;
            obs3.specialDirection = Direction.right;
            ObstaclesManager.AddObstacle(obs3);
        }

        private static void CreateRain(Direction direction)
        {
            int r = random.Next(0, 100);
                for (int i = 0; i < Game1.size.X; i += 100)
                {
                    Vector2 pos;
                    if(direction == Direction.left)
                        pos = new Vector2(i, 0);
                    else
                        pos = new Vector2(Game1.size.X -i, 0);
                    Vector2 acc = new Vector2(0, 1);
                    acc.Normalize();
                    acc /= 10;
                    Obstacle obs = new Obstacle(pos, acc, new Vector2(30, 30));
                    obs.color = Color.White;
                    obs.angle = MathHelper.ToRadians(45);
                    obs.framesToStart = i / 10;
                    ObstaclesManager.AddObstacle(obs);
                }
        }

        private static void CreateRain()
        {
            int r = random.Next(0, 100);
            for (int i = 0; i < Game1.size.X; i += 100)
            {
                Vector2 pos= new Vector2(i, 0);
                Vector2 acc = new Vector2(0, 1);
                acc.Normalize();
                acc /= 10;
                Obstacle obs = new Obstacle(pos, acc, new Vector2(30, 30));
                obs.color = Color.White;
                obs.angle = MathHelper.ToRadians(45);
                ObstaclesManager.AddObstacle(obs);
            }
        }

        private static void CreateDoubleRain()
        {
            for (int i = 0; i < Game1.size.X; i += 100)
            {
                Vector2 pos = new Vector2(i, 0);
                Vector2 acc = new Vector2(0, 1);
                acc.Normalize();
                acc /= 10;
                Obstacle obs = new Obstacle(pos, acc, new Vector2(30, 30));
                obs.color = Color.White;
                obs.angle = MathHelper.ToRadians(45);
                obs.framesToStart = i / 10;
                ObstaclesManager.AddObstacle(obs);
            }

            for (int i = 0; i < Game1.size.X; i += 100)
            {
                Vector2 pos = pos = new Vector2(Game1.size.X - i, 0);
                Vector2 acc = new Vector2(0, 1);
                acc.Normalize();
                acc /= 10;
                Obstacle obs = new Obstacle(pos, acc, new Vector2(30, 30));
                obs.color = Color.White;
                obs.angle = MathHelper.ToRadians(45);
                obs.framesToStart = i / 10;
                ObstaclesManager.AddObstacle(obs);
            }
        }

        private static void CreateArrows(Direction direction)
        {
            int nb = random.Next(3, 6);
            for (int i = 1; i < nb; i++)
            {
                Vector2 pos;
                Vector2 acc;
                if (direction == Direction.left)
                {
                    pos = new Vector2(0, random.Next(0, (int)Game1.size.Y));
                    acc = new Vector2(1, 0);
                }
                else
                {
                    pos = new Vector2(Game1.size.X, random.Next(0, (int)Game1.size.Y));
                    acc = new Vector2(-1, 0);
                }
                acc.Normalize();
                acc /= 2;
                Obstacle obs = new Obstacle(pos, acc, new Vector2(200, 8));
                obs.color = Color.Yellow;
                obs.angle = MathHelper.ToRadians(0);
                obs.framesToStart = 50+(10*i);
                ObstaclesManager.AddObstacle(obs);

                pos += new Vector2(0, -5);
                Obstacle obs2 = new Obstacle(pos, acc, new Vector2(50, 8));
                obs2.color = Color.Yellow;
                obs2.angle = MathHelper.ToRadians(0);
                obs2.framesToStart = 50 + (10 * i);
                ObstaclesManager.AddObstacle(obs2);

                pos += new Vector2(0, 10);
                Obstacle obs3 = new Obstacle(pos, acc, new Vector2(50, 8));
                obs3.color = Color.Yellow;
                obs3.angle = MathHelper.ToRadians(0);
                obs3.framesToStart = 50 + (10 * i);
                ObstaclesManager.AddObstacle(obs3);
            }
            
        }

        private static void CreateChange(Direction direction)
        {
            Vector2 pos, acc;
            if (direction == Direction.left)
            {
                pos = new Vector2(0, Game1.size.Y / 2);
                acc = new Vector2(1, 0);
            }

            else if (direction == Direction.right)
            {
                pos = new Vector2(Game1.size.X, Game1.size.Y / 2);
                acc = new Vector2(-1, 0);
            }
            else
            {
                pos = new Vector2(Game1.size.X/2, 0);
                acc = new Vector2(0, 1);
            }
            
            acc.Normalize();
            acc /= 15;
            Obstacle obs = new Obstacle(pos, acc, new Vector2(20, Game1.size.Y + 10));
            if (direction == Direction.down)
            {
                obs = new Obstacle(pos, acc, new Vector2(10, 10));
                obs.specialDirection = Direction.down;
            }
            obs.color = Color.Violet;
            obs.angle = MathHelper.ToRadians(0);
            obs.changeSize = true;
            ObstaclesManager.AddObstacle(obs);
        }

        private static void CreateFollowLine(Direction direction)
        {
            Vector2 pos;
            Vector2 acc;
            if (direction == Direction.left)
            {
                pos = new Vector2(0, V.Instance.point1.Y);
                acc = new Vector2(V.Instance.point2.X - pos.X, V.Instance.point2.Y - pos.Y);
            }

            else
            {
                pos = new Vector2(Game1.size.X, V.Instance.point5.Y);
                acc = new Vector2(V.Instance.point4.X - pos.X, V.Instance.point4.Y - pos.Y);
            }

            acc.Normalize();
            acc /= 100;
            Obstacle obs = new Obstacle(pos, acc, new Vector2(20, 40));
            obs.color = Color.Pink;
            obs.specialDirection = direction;
            obs.followLine = true;
            ObstaclesManager.AddObstacle(obs);
        }

        #endregion

        public static void SpecialMove()
        {
            foreach (Obstacle obstacle in ObstaclesManager.obstacles)
                if (obstacle.isSpecial)
                    obstacle.move = true;
        }
    }
}
