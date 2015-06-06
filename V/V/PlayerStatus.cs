using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace V
{
    public static class PlayerStatus
    {
        public static float time;
        static int sec, mil;
        public static int bestSec, bestMil;
        static float handicapTimer;
        static int handicapNb = 10;
        static List<int> handicaps = new List<int>();
        static string message;
        static bool drawString = false;
        static float drawTime;
        public static string highScoreFileNameEasy = "highscoreEasy.txt";
        public static string highScoreFileNameMedium = "highscoreMedium.txt";
        public static string highScoreFileNameHard = "highscoreHard.txt";

        public static void Update(GameTime gameTime)
        {
            if (!Player.Instance.IsAlive)
            {
                drawString = false;
                return;
            }
            
            if(Player.Instance.isMoving)
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            sec = (int)time;
            mil = (int)Math.Round(100*(time - (int)time),0);

            if (sec >= bestSec)
            {
                bestSec = sec;
                bestMil = mil;
            }

            handicapTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (handicapTimer >GamePlayState.timeBetweenHandicaps)
            {
                ChooseHandicap();
                handicapTimer = 0;
            }

            if (drawString == true)
            {
                drawTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (drawTime > 2000)
            {
                drawTime = 0;
                drawString = false;
            }
        }

        public static void Start()
        {
            Player.Instance.Reset();
            ObstaclesManager.Reset();
            ObstacleSpawner.Reset();
            V.Instance.ResetPosition();

            Game1.pulse = false;
            Game1.bloomIntensity = 2;
            Game1.rgbManager.effectActivated = false;

            handicapTimer = 0;
            handicaps.Clear();
            for (int i = 1; i <= handicapNb; i++)
                handicaps.Add(i);
            time = 0;

            drawString = false;
            drawTime = 0;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if(sec>=bestSec)
                color = Color.Red;
            spriteBatch.DrawString(Art.FontSmall, "Time: " + sec + ":" + mil, new Vector2(Game1.size.X - 120, Game1.size.Y - 60), color);
            spriteBatch.DrawString(Art.FontSmall, "Best: " + bestSec + ":" + bestMil, new Vector2(Game1.size.X - 120, Game1.size.Y - 30), Color.White);
            spriteBatch.DrawString(Art.FontSmall, "" + Game1.difficulty, new Vector2(Game1.size.X - 120, Game1.size.Y - 90), Color.White);
            if (!Player.Instance.IsAlive)
            { 
                spriteBatch.DrawString(Art.FontBig, "DEATH", new Vector2(80, 100), Color.White);
                spriteBatch.DrawString(Art.FontSmall, "SPACE : RESTART", new Vector2(58, Game1.size.Y - 50), Color.White);
                spriteBatch.DrawString(Art.FontSmall, "ESC : MENU", new Vector2(80, Game1.size.Y - 30), Color.White);
            }
            if (drawString)
            {
                var textWidth = Art.FontMedium.MeasureString(message).X;
                spriteBatch.DrawString(Art.FontMedium, message, new Vector2((Game1.size.X - textWidth)/2, 200), Color.White);
            }
            if (GamePlayState.pause)
            {
                var textWidth = Art.FontMedium.MeasureString("PAUSE").X;
                spriteBatch.DrawString(Art.FontMedium, "PAUSE", new Vector2((Game1.size.X - textWidth) / 2, 200), Color.White);
            }
        }

        public static void ChooseHandicap()
        {
            if (handicaps.Count == 0)
                return;
            Random random = new Random();
            int r = random.Next(0, handicaps.Count());
            handicaps.ToArray();
            if (handicaps[r] == 1)
            {
                V.Instance.moveP3 = true;
                handicaps.Remove(1);
                message = "VELOCITY1";
                drawString = true;
            }
            else if (handicaps[r] == 2)
            {
                V.Instance.moveP2 = true;
                V.Instance.moveP4 = true;
                handicaps.Remove(2);
                message = "VELOCITY2";
                drawString = true;
            }
            else if (handicaps[r] == 3)
            {
                Game1.pulse = true;
                handicaps.Remove(3);
                message = "PRECISION";
                drawString = true;
            }
            else if (handicaps[r] == 4)
            {
                Player.Instance.gravity = true;
                Player.Instance.mod *=2;
                handicaps.Remove(4);
                message = "GRAVITY";
                drawString = true;
            }
            else if (handicaps[r] == 5)
            {
                ObstacleSpawner.attack = true;
                handicaps.Remove(5);
                message = "ATTACK";
                drawString = true;
            }
            else if (handicaps[r] == 6)
            {
                Player.Instance.inverse = -1;
                handicaps.Remove(6);
                message = "INVERSE";
                drawString = true;
            }
            else if (handicaps[r] == 7)
            {
                Player.Instance.mod*=.7f;
                handicaps.Remove(7);
                message = "SLOW";
                drawString = true;
            }
            else if (handicaps[r] ==8)
            {
                Game1.rgbManager.effectActivated = true;
                handicaps.Remove(8);
                message = "SHIFT";
                drawString = true;
            }
            else if (handicaps[r] == 9)
            {
                Player.Instance.growth = true;
                handicaps.Remove(9);
                message = "GROWTH";
                drawString = true;
            }
            else if (handicaps[r] == 10)
            {
                V.Instance.moveP1 = true;
                V.Instance.moveP5 = true;
                handicaps.Remove(10);
                message = "VELOCITY3";
                drawString = true;
            }
        }

       public static void loadHighScore()
        {
           if(Game1.difficulty  == Difficulty.EASY)
            {
                if (!File.Exists(highScoreFileNameEasy))
                {
                    File.WriteAllText(highScoreFileNameEasy, "" + 0 + "\r\n" + 0);
                    return;
                }
                IEnumerable<string> list = File.ReadLines(highScoreFileNameEasy);
                string[] array = new string[2];
                array = list.ToArray();
                bestSec = int.Parse(array[0]);
                bestMil = int.Parse(array[1]);
            }

           else if (Game1.difficulty == Difficulty.MEDIUM)
           {
               if (!File.Exists(highScoreFileNameMedium))
               {
                   File.WriteAllText(highScoreFileNameEasy, "" + 0 + "\r\n" + 0);
                   return;
               }
               IEnumerable<string> list = File.ReadLines(highScoreFileNameMedium);
               string[] array = new string[2];
               array = list.ToArray();
               bestSec = int.Parse(array[0]);
               bestMil = int.Parse(array[1]);
           }

           else if (Game1.difficulty == Difficulty.HARD)
           {
               if (!File.Exists(highScoreFileNameHard))
               {
                   File.WriteAllText(highScoreFileNameHard, "" + 0 + "\r\n" + 0);
                   return;
               }
               IEnumerable<string> list = File.ReadLines(highScoreFileNameHard);
               string[] array = new string[2];
               array = list.ToArray();
               bestSec = int.Parse(array[0]);
               bestMil = int.Parse(array[1]);
           }
        }

        public static void SaveHighScore()
        {
            if (Game1.difficulty == Difficulty.EASY)
                File.WriteAllText(highScoreFileNameEasy, "" + bestSec + "\r\n" + bestMil);
            else if (Game1.difficulty == Difficulty.MEDIUM)
                File.WriteAllText(highScoreFileNameMedium, "" + bestSec + "\r\n" + bestMil);
            else if (Game1.difficulty == Difficulty.HARD)
                File.WriteAllText(highScoreFileNameHard, "" + bestSec + "\r\n" + bestMil);
        }

    }
}
