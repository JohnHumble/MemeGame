using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MemeGame
{
    enum Screen
    {
        Play,
        Build
    }
    
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const int UNIT_SIZE = 120;
        const int TILE_SIZE = 32;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;
        Screen screen;

        Builder builder;

        WallCollection walls;
        Hero player1;

        int screenWidth, screenHeight;

        Texture2D fill;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Set screen size
            screenWidth = 1652;
            screenHeight = 944;

            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            IsMouseVisible = true;
            graphics.ApplyChanges();

            screen = Screen.Build;
            camera = new Camera(0,0,1,screenWidth,screenHeight);

            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            fill = loadColorTexture(Color.White);

            Texture2D heroTex = Content.Load<Texture2D>("newBasic");
            player1 = new Hero(new Point(300, 100), UNIT_SIZE/3 * 2, UNIT_SIZE, 20, heroTex, 0, 0);

            Texture2D wallTexture = loadColorTexture(Color.DarkGreen);
            walls = new WallCollection(wallTexture, TILE_SIZE);
            //walls.createBlock(0, 200, screenWidth, screenHeight);

            builder = new Builder(walls);

            builder.loadMap("last");
            
            // TODO: use this.Content to load your game content here
        }

        private Texture2D loadColorTexture(Color color)
        {
            Texture2D texture = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { color });
            return texture;
        }
        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        const int ACCEL = 5;
        const int JUMP = 20;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (screen == Screen.Play)
            {
                // user control
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    player1.jump(JUMP);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    player1.moveLeft(ACCEL);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    player1.moveRight(ACCEL);
                }
                if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right))
                {
                    player1.stop();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
                {
                    screen = Screen.Build;
                }

                player1.Update(2, walls);
            }
            else if (screen == Screen.Build)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    builder.saveMap("last");
                    screen = Screen.Play;
                }

                builder.building(Mouse.GetState());
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.getTransformation());
            walls.Draw(spriteBatch);
            player1.Draw(spriteBatch,fill);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
