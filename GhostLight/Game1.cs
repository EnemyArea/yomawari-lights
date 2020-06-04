#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace GhostLight
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Effect ghostLightShader;
        private Texture2D background;
        private Texture2D lightmap;
        private Texture2D ghost;
        private RenderTarget2D sceneLightLayer;
        private RenderTarget2D ghostLayer;
        private RenderTarget2D ghostLightLayer;
        private BlendState lightBlend;
        private Effect waveShader;
        private Texture2D jan;


        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.graphics.GraphicsProfile = GraphicsProfile.HiDef;
            this.graphics.PreferredBackBufferWidth = 650;
            this.graphics.PreferredBackBufferHeight = 764;
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.background = this.Content.Load<Texture2D>("Background");
            this.ghost = this.Content.Load<Texture2D>("geist");
            this.lightmap = this.Content.Load<Texture2D>("LightCone");
            this.ghostLightShader = this.Content.Load<Effect>("Shader");
            this.waveShader = this.Content.Load<Effect>("WaveShader");
            this.jan = this.Content.Load<Texture2D>("jan");

            this.lightBlend = new BlendState
            {
                ColorSourceBlend = Blend.Zero,
                ColorDestinationBlend = Blend.SourceColor
            };

            this.ghostLightLayer = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            this.ghostLayer = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            this.sceneLightLayer = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            var mousePosition = Mouse.GetState().Position;

            // Draw lights for scene
            var clearColor = new Color(140, 140, 140);
            this.GraphicsDevice.SetRenderTarget(this.sceneLightLayer);
            this.GraphicsDevice.Clear(clearColor);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            this.spriteBatch.Draw(this.lightmap, new Vector2(mousePosition.X, mousePosition.Y), null, Color.White, 0f, new Vector2(this.lightmap.Width * 0.5f, this.lightmap.Height * 0.5f), 1f, SpriteEffects.None, 0);
            this.spriteBatch.End();
            this.GraphicsDevice.SetRenderTarget(null);

            // Draw lights for GhostLayer
            this.GraphicsDevice.SetRenderTarget(this.ghostLayer);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.ghost, new Vector2(this.GraphicsDevice.Viewport.Width / 2f, this.GraphicsDevice.Viewport.Height / 2f), null, Color.White, 0f, new Vector2(this.ghost.Width * 0.5f, this.ghost.Height * 0.5f), 1f, SpriteEffects.None, 0);
            this.spriteBatch.End();
            this.GraphicsDevice.SetRenderTarget(null);

            this.GraphicsDevice.SetRenderTarget(this.ghostLightLayer);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.ghostLightShader.Parameters["LightTexture"].SetValue(this.sceneLightLayer);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, this.ghostLightShader);
            this.spriteBatch.Draw(this.ghostLayer, Vector2.Zero, Color.White);
            this.spriteBatch.End();
            this.GraphicsDevice.SetRenderTarget(null);

            // Render scene
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.background, Vector2.Zero, Color.White);
            this.spriteBatch.Draw(this.ghostLightLayer, Vector2.Zero, Color.White);
            this.spriteBatch.End();

            this.spriteBatch.Begin(SpriteSortMode.Deferred, this.lightBlend);
            this.spriteBatch.Draw(this.sceneLightLayer, Vector2.Zero, Color.White);
            this.spriteBatch.End();

            this.waveShader.Parameters["textureSize"].SetValue(new Vector2(this.jan.Width, this.jan.Height));
            this.waveShader.Parameters["shiftTime"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            this.waveShader.Parameters["intensity"].SetValue(50f);
            this.waveShader.Parameters["moveAmtX"].SetValue(5f);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, this.waveShader);
            this.spriteBatch.Draw(this.jan, new Vector2(this.GraphicsDevice.Viewport.Width / 2f, this.GraphicsDevice.Viewport.Height / 2f), null, Color.White, 0f, new Vector2(this.jan.Width * 0.5f, this.jan.Height * 0.5f), 1f, SpriteEffects.None, 0);
            this.spriteBatch.End();

            // Alternative (use ShaderAlternative.fx)
            //// Draw lights
            //this.GraphicsDevice.SetRenderTarget(this.SceneLightLayer);
            //this.GraphicsDevice.Clear(clearColor);
            //this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            //this.spriteBatch.Draw(this.Lightmap, new Vector2(mousePosition.X, mousePosition.Y), null, Color.White, 0f, new Vector2(this.Lightmap.Width * 0.5f, this.Lightmap.Height * 0.5f), 1f, SpriteEffects.None, 0);
            //this.spriteBatch.End();
            //this.GraphicsDevice.SetRenderTarget(null);

            //this.GraphicsDevice.SetRenderTarget(this.GhostLayer);
            //this.GraphicsDevice.Clear(clearColor);
            //this.spriteBatch.Begin();
            //this.spriteBatch.Draw(this.Ghost, new Vector2(100, this.GraphicsDevice.Viewport.Height / 2f), null, Color.White, 0f, new Vector2(this.Ghost.Width * 0.5f, this.Ghost.Height * 0.5f), 1f, SpriteEffects.None, 0);
            //this.spriteBatch.Draw(this.Ghost, new Vector2(this.GraphicsDevice.Viewport.Width / 2f, this.GraphicsDevice.Viewport.Height / 2f), null, Color.White, 0f, new Vector2(this.Ghost.Width * 0.5f, this.Ghost.Height * 0.5f), 1f, SpriteEffects.None, 0);
            //this.spriteBatch.Draw(this.Ghost, new Vector2(this.GraphicsDevice.Viewport.Width / 2f, 700), null, Color.White, 0f, new Vector2(this.Ghost.Width * 0.5f, this.Ghost.Height * 0.5f), 1f, SpriteEffects.None, 0);
            //this.spriteBatch.End();
            //this.GraphicsDevice.SetRenderTarget(null);

            //this.GraphicsDevice.SetRenderTarget(this.GhostLightLayer);
            //this.GraphicsDevice.Clear(Color.Transparent);
            //this.ghostLightShader.Parameters["LightTexture"].SetValue(this.SceneLightLayer);
            //this.ghostLightShader.Parameters["ColorKey"].SetValue(clearColor.ToVector4());
            //this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, this.ghostLightShader);
            //this.spriteBatch.Draw(this.GhostLayer, Vector2.Zero, Color.White);
            //this.spriteBatch.End();
            //this.GraphicsDevice.SetRenderTarget(null);

            //this.GraphicsDevice.Clear(Color.CornflowerBlue);

            //this.spriteBatch.Begin();
            //this.spriteBatch.Draw(this.Background, Vector2.Zero, Color.White);
            //this.spriteBatch.Draw(this.GhostLightLayer, Vector2.Zero, Color.White);
            //this.spriteBatch.End();

            //this.spriteBatch.Begin(SpriteSortMode.Deferred, this.LightBlend);
            //this.spriteBatch.Draw(this.SceneLightLayer, Vector2.Zero, Color.White);
            //this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}