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
    public static class Art
    {
        public static Texture2D Balltexture;
        public static Texture2D WhitePixel;
        public static Texture2D LeftArrow;
        public static Texture2D RightArrow;

        public static SpriteFont FontBig;
        public static SpriteFont FontSmall;
        public static SpriteFont FontMedium;


        public static void Load(ContentManager content)
        {
            Balltexture = content.Load<Texture2D>("whiteCircle");
            WhitePixel = content.Load<Texture2D>("WhitePixel");
            LeftArrow = content.Load<Texture2D>("LeftArrow");
            RightArrow = content.Load<Texture2D>("Rightarrow");
            FontBig = content.Load<SpriteFont>("SpriteFont1");
            FontSmall = content.Load<SpriteFont>("SpriteFont2");
            FontMedium = content.Load<SpriteFont>("SpriteFont3");
        }
        
    }
}
