using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BloomPostprocess;

namespace V
{
    public enum Difficulty { EASY, MEDIUM, HARD }

    public class GamePlayState
    {
        bool growing = true;
        public static float velocityMultiplier = 1f;
        public static int timeBetweenHandicaps = 10000;
        public static int obstPerTickTimer = 6000;
        public static int maxObsPerTick = 5;
        public static int minTimeBetweenObstacle = 500;
        public static int timeBetweenObstaclesDecrement =20;
        public static int specialTimer = 12000;
        public static bool pause = false;

        public GamePlayState(Difficulty difficulty)
        {
            ObstacleSpawner.CreateStaticHeart();
            PlayerStatus.Start();
            
            if (difficulty == Difficulty.EASY)
            {
                velocityMultiplier = 0.5f;
                timeBetweenHandicaps = 15000;
                obstPerTickTimer = 10000;
                maxObsPerTick = 3;
                minTimeBetweenObstacle = 1000;
                timeBetweenObstaclesDecrement = 10;
                specialTimer = 16000;
            }
            else if (difficulty == Difficulty.MEDIUM)
            {
                velocityMultiplier = 1f;
                timeBetweenHandicaps = 10000;
                obstPerTickTimer = 7000;
                maxObsPerTick = 5;
                minTimeBetweenObstacle = 500;
                timeBetweenObstaclesDecrement = 20;
                specialTimer = 12000;
            }
            else if (difficulty == Difficulty.HARD)
            {
                velocityMultiplier = 2f;
                timeBetweenHandicaps = 7000;
                obstPerTickTimer = 5000;
                maxObsPerTick = 7;
                minTimeBetweenObstacle = 500;
                timeBetweenObstaclesDecrement = 40;
                specialTimer = 9000;
            }
            PlayerStatus.loadHighScore();
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState oldState, RGBShiftManager rgbManager, BloomComponent bloom, float bloomIntensity)
        {
            HandleInput(keyboardState, oldState);

            if (pause)
                return;

            Player.Instance.Update(keyboardState);
            ObstaclesManager.Update();
            ObstacleSpawner.Update(gameTime);
            PlayerStatus.Update(gameTime);
            V.Instance.Update();
            
            rgbManager.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            V.Instance.Draw(spriteBatch);
            ObstaclesManager.Draw(spriteBatch);
            Player.Instance.Draw(spriteBatch);
            PlayerStatus.Draw(spriteBatch);
        }

        private void HandleInput(KeyboardState keyboardState, KeyboardState oldState)
        {
            if (keyboardState.IsKeyDown(Keys.Space) && !Player.Instance.IsAlive)
                PlayerStatus.Start();
            if (keyboardState.IsKeyDown(Keys.Escape) && (!Player.Instance.IsAlive || GamePlayState.pause))
            {
                PlayerStatus.Start();
                Game1.state = State.Start;
                GamePlayState.pause = false;
            }
            if (keyboardState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
                pause = !pause;
        }

    }
}
