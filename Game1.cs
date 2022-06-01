using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace biomatria_proj
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        SpriteFont _font;
        Texture2D texture;
        Texture2D texture2;
        Vector2 targetPosition;
        Vector2 targetPosition2;
        Sobel sobel;
        RenderTarget2D target;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Sobel";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            _graphics.IsFullScreen = true;
            targetPosition = new Vector2(GraphicsDevice.DisplayMode.Width / 2, GraphicsDevice.DisplayMode.Height / 2);
            targetPosition2 = new Vector2(GraphicsDevice.DisplayMode.Width / 2, GraphicsDevice.DisplayMode.Height / 2);
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            texture = Content.Load<Texture2D>("kot");
            texture2 = Content.Load<Texture2D>("kot2");
            _font = Content.Load<SpriteFont>("galleryFont");

            BoundingRenderer.Initialize(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            target = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height,
                false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            sobel = new Sobel(this);
            if (sobel != null) sobel.Load();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if(Keyboard.GetState().IsKeyDown(Keys.Up))
                targetPosition.Y -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                targetPosition.Y += 10;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                targetPosition.X -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                targetPosition.X += 10;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                targetPosition2.Y -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                targetPosition2.Y += 10;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                targetPosition2.X -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                targetPosition2.X += 10;

            if (sobel != null) sobel.Update();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            if (sobel != null) GraphicsDevice.SetRenderTarget(target);
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // TODO: Add your drawing code here

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteBatch.Draw(texture2, targetPosition2, null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(texture, targetPosition, Color.White);
            _spriteBatch.DrawString(_font, "Boltowicz, Wiszowaty lokalny filtr", new Vector2(GraphicsDevice.DisplayMode.Width / 2, GraphicsDevice.DisplayMode.Height / 2), Color.White);
            _spriteBatch.End();

            if (sobel != null)
            {
                GraphicsDevice.SetRenderTarget(null);
                sobel.Draw(target);
            }
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _spriteBatch.Draw(texture, new Vector2(GraphicsDevice.DisplayMode.Width - texture.Width * 0.6F, 0), null, Color.White, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(_font, "Boltowicz, Wiszowaty lokalny filtr", new Vector2(GraphicsDevice.DisplayMode.Width - 600, GraphicsDevice.DisplayMode.Height-40), Color.White);

            _spriteBatch.Draw(texture2, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0f);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
