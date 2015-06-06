using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace V
{
    public class RGBShiftManager
    {
        public Texture2D texture;
        Vector2 textureSize;

        Effect effect;
        public bool effectActivated = true;


        float maxDeviation =20;
        float scanBarSpeed = 10;
        float speed = 0.0005f;
        
        Color transparentBlack30;
        Color transparentBlack40;
        

        #region parameters
        Random random = new Random();

        int yloc;

        float yRSpeed;
        float yR;

        float yGSpeed;
        float yG;

        float yBSpeed;
        float yB;


        float goalR;
        float goalG;
        float goalB;

        #endregion

        public RGBShiftManager(ContentManager Content, Texture2D texture)
        {

            this.texture = texture;
            textureSize = new Vector2(texture.Width, texture.Height);

            effect = Content.Load<Effect>("Effect1");

            
            transparentBlack30 = Color.Black;
            transparentBlack40 = Color.Black;
            transparentBlack30.A = 30;
            transparentBlack40.A = 40;
        }

        public  void Update()
        {
            SetRGBGoals();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (effectActivated)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, effect);
                spriteBatch.Draw(texture, Vector2.Zero, transparentBlack30);
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                DrawScanLines(spriteBatch);
                //DrawRollingBar(spriteBatch);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.Draw(texture, Vector2.Zero, Color.White);
                spriteBatch.End();
            }
        }

        void SetRGBGoals()
        {

            if (yR <= goalR)
                yR += yRSpeed;
            else
                yR -= yRSpeed;
            if (Math.Abs(yR) >= Math.Abs(goalR))
            {
                goalR = maxDeviation * random.Next(-100, 100) / 100000;
                yRSpeed = speed + speed * random.Next(-90, 100) / 100;
            }



            if (yG <= goalG)
                yG += yGSpeed;
            else
                yG -= yGSpeed;
            if (Math.Abs(yG) >= Math.Abs(goalG))
            {
                goalG = maxDeviation * random.Next(-100, 100) / 100000;
                yGSpeed = speed + speed * random.Next(-90, 100) / 100;
            }



            if (yB <= goalB)
                yB += yBSpeed;
            else
                yB -= yBSpeed;
            if (Math.Abs(yB) >= Math.Abs(goalB))
            {
                goalB = maxDeviation * random.Next(-100, 100) / 100000;
                yBSpeed = speed + speed * random.Next(-90, 100) / 100;
            }



            effect.Parameters["yOffsetR"].SetValue(yR);
            effect.Parameters["yOffsetG"].SetValue(yG);
            effect.Parameters["yOffsetB"].SetValue(yB);

            float transparency = random.Next(0, 20) / (float)100;
            effect.Parameters["transparency"].SetValue(transparency + 0.8f);
        }

        void DrawScanLines(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textureSize.Y / 2; i++)
                Primitive.DrawLineSegment(spriteBatch, new Vector2(0, 2 * i), new Vector2(textureSize.X, 2 * i), transparentBlack40, 1);
        }

        void DrawRollingBar(SpriteBatch spriteBatch)
        {
            yloc++;
            if (yloc > textureSize.Y / 10 + 50)
            {
                yloc = 0;
                scanBarSpeed = random.Next(2, 20);
            }

            Primitive.DrawLineSegment(spriteBatch, new Vector2(0, yloc * scanBarSpeed), new Vector2(textureSize.X, yloc * scanBarSpeed), transparentBlack30, 40);
        }
    }
}
