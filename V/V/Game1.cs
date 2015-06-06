using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BloomPostprocess;


namespace V
{
    public enum State { Gameplay, Start }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Texture2D whitePixel;
        public static Vector2 size = new Vector2(960, 640);
        KeyboardState oldState;
        BloomComponent bloom;
        public static float bloomIntensity = 2;
        public static RGBShiftManager rgbManager;
        RenderTarget2D renderTarget;
        Texture2D renderTexture;
        bool pause = false;
        bool growing = true;

        public static GamePlayState gamePlayState;
        StartState startState;
        public static State state = State.Start;
        public static Difficulty difficulty;
        public static bool pulse = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = (int)size.X;
            graphics.PreferredBackBufferHeight = (int)size.Y;
            IsMouseVisible = true;
            graphics.ApplyChanges();

            bloom = new BloomComponent(this);
            Components.Add(bloom);
            bloom.Settings = new BloomSettings(null, 0.25f, 4,bloomIntensity, 1, 1.5f, 1);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            whitePixel = Content.Load<Texture2D>("WhitePixel");
            
            renderTarget = new RenderTarget2D(graphics.GraphicsDevice,
                graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                graphics.GraphicsDevice.PresentationParameters.BackBufferHeight,
                true, SurfaceFormat.Color, DepthFormat.None);
            rgbManager = new RGBShiftManager(Content, renderTarget);
            rgbManager.effectActivated = false;
            Art.Load(Content);

            //gamePlayState = new GamePlayState(Difficulty.easy);
            startState = new StartState();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            
            if (state == State.Start)
                startState.Update(gameTime, keyboardState, oldState);
            else if (state == State.Gameplay)
                gamePlayState.Update(gameTime, keyboardState, oldState, rgbManager, bloom, bloomIntensity);
            Pulse();
            oldState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if (state == State.Start)
                startState.Draw(spriteBatch);
            else if (state == State.Gameplay)
                gamePlayState.Draw(spriteBatch);
            spriteBatch.End();


            GraphicsDevice.SetRenderTarget(null);
            renderTexture = (Texture2D)renderTarget;

            GraphicsDevice.Clear(ClearOptions.Target, Color.White, 1, 0);

            bloom.BeginDraw();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            spriteBatch.Draw(renderTexture, Vector2.Zero, Color.White);
            spriteBatch.End();

            rgbManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private void HandleInput(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Space) && !Player.Instance.IsAlive)
                PlayerStatus.Start();
        }

        public void Pulse()
        {
            if (!pulse)
            {
                bloom.Settings = new BloomSettings(null, 0.25f, 4, 2, 1, 1.5f, 1);
                return;
            }

            if (growing)
                bloomIntensity += .05f;
            else
                bloomIntensity -= .05f;
            if (bloomIntensity > 10)
                growing = false;
            if (bloomIntensity < 1)
                growing = true;

            bloom.Settings = new BloomSettings(null, 0.25f, 4, bloomIntensity, 1, 1.5f, 1);
        }
    }
}
