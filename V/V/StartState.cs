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
    class StartState
    {
        string diff;
        string highScore;

        public void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState oldState)
        {
            if(keyboardState.IsKeyDown(Keys.Left) && oldState.IsKeyUp(Keys.Left))
            {
                if(Game1.difficulty == Difficulty.MEDIUM)
                    Game1.difficulty = Difficulty.EASY;
                else if(Game1.difficulty == Difficulty.HARD)
                    Game1.difficulty = Difficulty.MEDIUM;
            }

            if (keyboardState.IsKeyDown(Keys.Right) && oldState.IsKeyUp(Keys.Right))
            {
                if (Game1.difficulty == Difficulty.MEDIUM)
                    Game1.difficulty = Difficulty.HARD;
                else if (Game1.difficulty == Difficulty.EASY)
                    Game1.difficulty = Difficulty.MEDIUM;
            }

            diff = Game1.difficulty.ToString();
            if (Game1.difficulty == Difficulty.EASY)
                highScore = PlayerStatus.bestSec + ":" + PlayerStatus.bestMil;

            if (keyboardState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                Game1.state = State.Gameplay;
                Game1.gamePlayState = new GamePlayState(Game1.difficulty);
            }  
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            string message = "V";
            var textWidth = Art.FontBig.MeasureString(message).X;
            V.Instance.Draw(spriteBatch);
            spriteBatch.DrawString(Art.FontBig, message, new Vector2((Game1.size.X - textWidth) / 2, 100), Color.White);

            message = diff;
            textWidth = Art.FontSmall.MeasureString(message).X;
            spriteBatch.DrawString(Art.FontSmall, message, new Vector2((Game1.size.X - textWidth) / 2, 500), Color.White);

            if(Game1.difficulty == Difficulty.EASY || Game1.difficulty == Difficulty.MEDIUM)
                spriteBatch.Draw(Art.RightArrow, new Vector2((Game1.size.X - textWidth) / 2 + textWidth +10, 506), Color.White);
            if (Game1.difficulty == Difficulty.HARD || Game1.difficulty == Difficulty.MEDIUM)
                spriteBatch.Draw(Art.LeftArrow, new Vector2((Game1.size.X - textWidth) / 2 - 25, 506), Color.White);
        }

    }
}
