using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MemeGame
{
    enum Screen
    {
        Play,
        Build,
        Menu
    }

    enum Heros
    {
        Basic,
        Chungus,
        Doge
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
        Menus mainMenu;

        WallCollection walls;
        PlayerCollection players;
        WeaponCollection weapons;

        int screenWidth, screenHeight;

        Texture2D fill;
        SpriteFont sans;
        
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

            screen = Screen.Menu;
            camera = new Camera(0,0,1f,screenWidth,screenHeight);

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
            sans = Content.Load<SpriteFont>("sans");

            // load in the textures for heros
            List<Texture2D> heroTextures = new List<Texture2D>();

            // load textures into the list
            Texture2D heroTex = Content.Load<Texture2D>("newBasic");
            heroTextures.Add(heroTex);
            Texture2D chungTex = Content.Load<Texture2D>("chungus");
            heroTextures.Add(chungTex);
            Texture2D dogeTex = Content.Load<Texture2D>("doge");
            heroTextures.Add(dogeTex);
            Texture2D blockTex = Content.Load<Texture2D>("blocks");
            Texture2D weaponsTex = Content.Load<Texture2D>("weapons");
            players = new PlayerCollection(heroTextures, UNIT_SIZE / 3 * 2, UNIT_SIZE,sans,fill);

            Texture2D wallTexture = loadColorTexture(Color.DarkGreen);
            walls = new WallCollection(wallTexture, TILE_SIZE);
            //walls.createBlock(0, 200, screenWidth, screenHeight);

            builder = new Builder(walls,sans,fill,heroTex,UNIT_SIZE/3*2,UNIT_SIZE,weaponsTex,UNIT_SIZE,UNIT_SIZE/2);

            builder.loadMap("last.gmd");

            // delete this sometime
            players.AddPlayer(new Point(100, 100), Heros.Chungus,"player 1",Color.Red, Keys.Left, Keys.Right, Keys.Up,Keys.Space);
            players.AddPlayer(new Point(200, 100), Heros.Doge, "Player 2", Color.Blue, Keys.A, Keys.D, Keys.W, Keys.LeftShift);


            weapons = new WeaponCollection();

            Gun gun = new Gun(new Rectangle(0, 0, UNIT_SIZE, UNIT_SIZE / 2), weaponsTex, fill, 2);
            weapons.Add(gun);

            mainMenu = new Menus(sans, fill);
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
        
        const int GRAV = 2;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (screen == Screen.Menu)
            {
                screen = mainMenu.Update(Mouse.GetState());
            }

            if (screen == Screen.Play)
            {
                screen = players.Update(GRAV, walls, players, weapons,Mouse.GetState());
                camera.trackTo(players.GetPlayerMid(), players.GetDifference(),walls.GetMid(), walls.GetDifference());

                if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
                {
                    screen = Screen.Build;
                }
            }
            else if (screen == Screen.Build)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    builder.saveMap("last.gmd");
                    screen = Screen.Menu;
                }

                screen = builder.building(Mouse.GetState(),camera,"last");
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
            players.Draw(spriteBatch);
            walls.Draw(spriteBatch);
            players.DrawNames(spriteBatch, sans);
            weapons.Draw(spriteBatch);
            spriteBatch.End();

            // Camera test
            spriteBatch.Begin();
            //  spriteBatch.DrawString(sans, "" + camera.scale, new Vector2(100, 100), Color.Yellow);

            if (screen == Screen.Menu)
            {
                mainMenu.Draw(spriteBatch);
            }
            if (screen == Screen.Play)
            {
                players.Drawhud(spriteBatch);
            }
            if (screen == Screen.Build)
            {
                builder.Drawhud(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
