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
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public BlendState MultiplicativeBlend { get; set; }

        public Texture2D Background { get; set; }

        public Texture2D Lightmap { get; set; }

        public Texture2D Lightmap2 { get; set; }

        public Texture2D Ghost { get; set; }

        public RenderTarget2D LightmapLayer { get; set; }

        public RenderTarget2D LightLayer { get; set; }

        public RenderTarget2D GhostLayer { get; set; }

        public BlendState LightBlend { get; set; }

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

            this.Ghost = this.Content.Load<Texture2D>("m461");
            this.Lightmap = this.Content.Load<Texture2D>("Ghost/lightmask");
            this.Background = this.Content.Load<Texture2D>("Background");

            // TODO: use this.Content to load your game content here
            this.MultiplicativeBlend = new BlendState
            {
                AlphaBlendFunction = BlendFunction.ReverseSubtract,
                ColorBlendFunction = BlendFunction.Add,
                ColorSourceBlend = Blend.DestinationColor,
                ColorDestinationBlend = Blend.Zero
            };

            //Another blendstate to deal with the lightmap later:
            this.LightBlend = new BlendState
            {
                ColorSourceBlend = Blend.Zero,
                ColorDestinationBlend = Blend.SourceColor
            };

            this.GhostLayer = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            this.LightmapLayer = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            this.LightLayer = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
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
            var mousePosition = new Vector2(this.GraphicsDevice.Viewport.Width / 2f, this.GraphicsDevice.Viewport.Height / 2f);// Mouse.GetState().Position;


            // Draw lights
            this.GraphicsDevice.SetRenderTarget(this.LightmapLayer);
            this.GraphicsDevice.Clear(Color.Black);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            this.spriteBatch.Draw(this.Lightmap, new Vector2(mousePosition.X, mousePosition.Y), null, Color.White, 0f, new Vector2(this.Lightmap.Width * 0.5f, this.Lightmap.Height * 0.5f), 1f, SpriteEffects.None, 0);
            this.spriteBatch.End();
            this.GraphicsDevice.SetRenderTarget(null);

            //// Draw lights
            //this.GraphicsDevice.SetRenderTarget(this.LightLayer);
            //this.GraphicsDevice.Clear(Color.DarkGray);
            //this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            //this.spriteBatch.Draw(this.Lightmap, new Vector2(mousePosition.X, mousePosition.Y), null, Color.White, 0f, new Vector2(this.Lightmap.Width * 0.5f, this.Lightmap.Height * 0.5f), 1f, SpriteEffects.None, 0);
            //this.spriteBatch.End();
            //this.GraphicsDevice.SetRenderTarget(null);


            //I created a simple rendertarget 'GhostLayer':
            this.GraphicsDevice.SetRenderTarget(this.GhostLayer);
            this.GraphicsDevice.Clear(Color.Transparent);

            //first draw the ghost in the normal way
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.Ghost, new Vector2(this.GraphicsDevice.Viewport.Width / 2f, this.GraphicsDevice.Viewport.Height / 2f), null, Color.White, 0f, new Vector2(this.Ghost.Width * 0.5f, this.Ghost.Height * 0.5f), 1f, SpriteEffects.None, 0);
            this.spriteBatch.End();

            //now draw the lightmap to mask so only the 'ghost', 
            //this uses the blendstate created above.
            this.spriteBatch.Begin(SpriteSortMode.Deferred, this.MultiplicativeBlend);
            this.spriteBatch.Draw(this.LightmapLayer, Vector2.Zero, Color.White);
            this.spriteBatch.End();


            //show the result on the screen:
            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(Color.Black);

            this.spriteBatch.Begin();
            //Draw the game scene:
            this.spriteBatch.Draw(this.Background, Vector2.Zero, Color.White);
            //Draw the (masked) ghosts:
            this.spriteBatch.Draw(this.GhostLayer, Vector2.Zero, Color.White);

            // uncomment the next 'Draw' line and comment out the 'lightblend' 
            // section to see the alternate result.
            //spriteBatch.Draw(LightmapLayer, Vector2.Zero, Color.White*0.5f);  

            this.spriteBatch.End();

            ////Lightblend section:
            ////Draw the lightbeam using the lightBlend (though a shader might be better for this part).
            //this.spriteBatch.Begin(SpriteSortMode.Deferred, this.LightBlend);
            //this.spriteBatch.Draw(this.LightLayer, Vector2.Zero, Color.White);
            //this.spriteBatch.End();
            ////End lightblend section.

            base.Draw(gameTime);


            base.Draw(gameTime);
        }
    }
}