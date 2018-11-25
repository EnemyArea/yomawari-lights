#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace TestEyeGame
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D lichtRund;
        private Texture2D testeye;
        private RenderTarget2D blackpixel;
        private RenderTarget2D renderTarget;
        private Texture2D background;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.graphics.PreferredBackBufferWidth = 650;
            this.graphics.PreferredBackBufferHeight = 764;
        }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.lichtRund = this.Content.Load<Texture2D>("LichtRund2");
            this.testeye = this.Content.Load<Texture2D>("testeye");
            this.background = this.Content.Load<Texture2D>("Background");

            this.blackpixel = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            this.renderTarget = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var blend = new BlendState
            {
                ColorBlendFunction = BlendFunction.Add,
                ColorSourceBlend = Blend.DestinationColor,
                ColorDestinationBlend = Blend.Zero
            };

            var lightBlendState = new BlendState
            {
                ColorSourceBlend = Blend.Zero,
                ColorDestinationBlend = Blend.SourceColor
            };

            var mousePosition = Mouse.GetState().Position;

            // Draw lights
            this.GraphicsDevice.SetRenderTarget(this.blackpixel);
            this.GraphicsDevice.Clear(Color.DarkGray);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            this.spriteBatch.Draw(this.lichtRund, new Vector2(mousePosition.X, mousePosition.Y), null, Color.White, 0f, new Vector2(this.lichtRund.Width * 0.5f, this.lichtRund.Height * 0.5f), 1f, SpriteEffects.None, 0);
            this.spriteBatch.End();
            this.GraphicsDevice.SetRenderTarget(null);

            // Draw hidden
            this.GraphicsDevice.SetRenderTarget(this.renderTarget);
            this.GraphicsDevice.Clear(Color.White);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, blend);
            this.spriteBatch.Draw(this.testeye, new Vector2(this.GraphicsDevice.Viewport.Width / 2f, this.GraphicsDevice.Viewport.Height / 2f), null, Color.White, 0f, new Vector2(this.testeye.Width * 0.5f, this.testeye.Height * 0.5f), 1f, SpriteEffects.None, 0);
            this.spriteBatch.Draw(this.blackpixel, Vector2.Zero, Color.White);
            this.spriteBatch.End();
            this.GraphicsDevice.SetRenderTarget(null);

            // Combine all
            this.GraphicsDevice.Clear(Color.Black);

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            this.spriteBatch.Draw(this.background, Vector2.Zero, Color.White);
            this.spriteBatch.End();

            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            this.spriteBatch.Draw(this.renderTarget, Vector2.Zero, Color.White);
            this.spriteBatch.End();

            this.spriteBatch.Begin(SpriteSortMode.Deferred, lightBlendState);
            this.spriteBatch.Draw(this.blackpixel, Vector2.Zero, Color.White);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}