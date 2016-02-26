using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BloogsQuest.Models;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace BloogsQuest
{
    public class BloogsQuestGame : Game
    {
        public Map GameMap { get; set; }
        public Camera Camera { get; set; }
        public Player Player { get; set; }

        protected Song song;

        // XNA Dependencies
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<TilePrototype> tilePrototypes;

        public BloogsQuestGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Global.WindowWidth;
            graphics.PreferredBackBufferHeight = Global.WindowHeight;
            this.Content.RootDirectory = "Content";
            this.IsFixedTimeStep = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TilePrototypes
            tilePrototypes = new List<TilePrototype>();

            tilePrototypes.Add(new TilePrototype()
            {
                IsPassable = true,
                TextureFilename = "dirt",
                Sprite = new Sprite()
            });

            tilePrototypes.Add(new TilePrototype()
            {
                IsPassable = true,
                TextureFilename = "darkdirt",
                Sprite = new Sprite()
            });

            tilePrototypes.Add(new TilePrototype()
            {
                IsPassable = true,
                TextureFilename = "wetdirt",
                Sprite = new Sprite()
            });

            tilePrototypes.Add(new TilePrototype()
            {
                IsPassable = false,
                TextureFilename = "rock1",
                Sprite = new Sprite()
            });

            tilePrototypes.Add(new TilePrototype()
            {
                IsPassable = false,
                TextureFilename = "rock2",
                Sprite = new Sprite()
            });

            tilePrototypes.Add(new TilePrototype()
            {
                IsPassable = false,
                TextureFilename = "shrub",
                Sprite = new Sprite()
            });

            tilePrototypes.Add(new TilePrototype()
            {
                IsPassable = false,
                TextureFilename = "stump",
                Sprite = new Sprite()
            });

            GameMap = new Map(100, 50, 50, tilePrototypes);
            Camera = new Camera();
            Player = new Player(new Vector2(Global.WindowWidth / 2, Global.WindowHeight / 2));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(this.GraphicsDevice);

            foreach (var tilePrototype in tilePrototypes)
            {
                tilePrototype.Sprite.LoadContent(this.Content, tilePrototype.TextureFilename);
            }
            Player.Sprite.LoadContent(this.Content, Player.TextureFilename);

            song = Content.Load<Song>("Canon");  // Put the name of your song in instead of "song_title"
            MediaPlayer.Play(song);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var blockedTiles = GameMap.Tiles
                    .Where(x => !x.Prototype.IsPassable);

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (blockedTiles.Any(x => x.BoundingBox.Intersects(new Rectangle(Player.BoundingBox.X + (int)Global.PlayerSpeed, Player.BoundingBox.Y, 30, 30))))
                    return;

                if (Player.Position.X + Player.Sprite.Texture.Width + Global.PlayerSpeed > Global.MapWidth)
                    Player.Position = new Vector2(Global.MapWidth - Player.Sprite.Texture.Width, Player.Position.Y);
                else
                    Player.Position = new Vector2(Player.Position.X + Global.PlayerSpeed, Player.Position.Y);

                if (Player.Position.X > Global.WindowWidth / 2)
                {
                    if (Camera.Position.X + Global.WindowWidth + Global.PlayerSpeed > Global.MapWidth)
                        Camera.Position = new Vector2(Global.MapWidth - Global.WindowWidth, Camera.Position.Y);
                    else
                        Camera.Position = new Vector2(Camera.Position.X + Global.PlayerSpeed, Camera.Position.Y);
                }

                return;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (blockedTiles.Any(x => x.BoundingBox.Intersects(new Rectangle(Player.BoundingBox.X, Player.BoundingBox.Y + (int)Global.PlayerSpeed, 30, 30))))
                    return;

                if (Player.Position.Y + Player.Sprite.Texture.Height + Global.PlayerSpeed > Global.MapHeight)
                    Player.Position = new Vector2(Player.Position.X, Global.MapHeight - Player.Sprite.Texture.Height);
                else
                    Player.Position = new Vector2(Player.Position.X, Player.Position.Y + Global.PlayerSpeed);

                if (Player.Position.Y > Global.WindowHeight / 2)
                {
                    if (Camera.Position.Y + Global.WindowHeight + Global.PlayerSpeed > Global.MapHeight)
                        Camera.Position = new Vector2(Camera.Position.X, Global.MapHeight - Global.WindowHeight);
                    else
                        Camera.Position = new Vector2(Camera.Position.X, Camera.Position.Y + Global.PlayerSpeed);
                }

                return;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (blockedTiles.Any(x => x.BoundingBox.Intersects(new Rectangle(Player.BoundingBox.X - (int)Global.PlayerSpeed, Player.BoundingBox.Y, 30, 30))))
                    return;

                if (Player.Position.X - Global.PlayerSpeed >= 0)
                    Player.Position = new Vector2(Player.Position.X - Global.PlayerSpeed, Player.Position.Y);

                if (Player.Position.X < Global.MapWidth - (Global.WindowWidth / 2))
                {
                    if (Camera.Position.X - Global.PlayerSpeed < 0)
                        Camera.Position = new Vector2(0, Camera.Position.Y);
                    else
                        Camera.Position = new Vector2(Camera.Position.X - Global.PlayerSpeed, Camera.Position.Y);
                }

                return;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (blockedTiles.Any(x => x.BoundingBox.Intersects(new Rectangle(Player.BoundingBox.X, Player.BoundingBox.Y - (int)Global.PlayerSpeed, 30, 30))))
                    return;

                if (Player.Position.Y - Global.PlayerSpeed >= 0)
                    Player.Position = new Vector2(Player.Position.X, Player.Position.Y - Global.PlayerSpeed);

                if (Player.Position.Y < Global.MapHeight - (Global.WindowHeight / 2))
                {
                    if (Camera.Position.Y - Global.PlayerSpeed < 0)
                        Camera.Position = new Vector2(Camera.Position.X, 0);
                    else
                        Camera.Position = new Vector2(Camera.Position.X, Camera.Position.Y - Global.PlayerSpeed);
                }

                return;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Camera.GetTransform());
            GameMap.Draw(spriteBatch, Camera.Position);
            spriteBatch.Draw(Player.Sprite.Texture, new Vector2(Player.Position.X, Player.Position.Y), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
