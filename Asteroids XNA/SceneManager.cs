using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Asteroids_XNA
{
    public static class SceneManager
    {
        public static ContentManager content;
        public static GraphicsDeviceManager graphics;
        public static GraphicsDevice graphicsDevice;

        public static Dictionary<int, Entity> loadedEntities = new Dictionary<int, Entity>();
        static int entityIdCounter = 0;

        public static int screenWidth = 1600;
        public static int screenHeight = 900;

        public static bool debugMode = false;

        public static Texture2D pixel;
        public static Texture2D asteroid1Texture;
        public static SpriteFont pixelFont;

        public static int score;
        public static bool isDead = false;

        public static void ApplyScreenModifications()
        {
            //screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
        }

        public static void LoadScene(Scene scene)
        {
            score = 0;
            isDead = false;
            pixel = LoadTexture("pixel");
            asteroid1Texture = LoadTexture("asteroid1");
            pixelFont = LoadFont("PixelFont");
            loadedEntities.Clear();
            entityIdCounter = 0;
            scene.Load();
        }

        public static Texture2D LoadTexture(string textureName)
        {
            try
            {
                Texture2D t = content.Load<Texture2D>(textureName);
                Logger.LogInfo($"Loaded texture \"{textureName}\".");
                return t;
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to load texture \"{textureName}\".", e.ToString());
                return null;
            }
        }

        public static SpriteFont LoadFont(string fontName)
        {
            try
            {
                SpriteFont f = content.Load<SpriteFont>(fontName);
                Logger.LogInfo($"Loaded font \"{fontName}\".");
                return f;
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to load font \"{fontName}\".", e.ToString());
                return null;
            }
        }

        public static Entity SpawnEntity(Entity entity)
        {
            loadedEntities.Add(entityIdCounter, entity);
            entity._id = entityIdCounter;
            entityIdCounter++;
            Logger.LogInfo($"Spawned entity with ID of {entity._id}.");
            return entity;
        }

        public static void DestroyEntity(int id)
        {
            if (loadedEntities.ContainsKey(id))
            {
                loadedEntities.Remove(id);
                Logger.LogInfo($"Destroyed entity of ID {id}.");
            }
            else
            {
                Logger.LogError($"Failed to destroy entity of ID {id}, entity not found.");
            }
        }

        public static void Start()
        {
            ApplyScreenModifications();
            LoadScene(new Scene1());
        }

        public static void Update()
        {
            InputHelper.UpdateState();

            if (InputHelper.GetKeyPressed(Keys.L))
                debugMode = !debugMode;
            if (InputHelper.GetKeyPressed(Keys.R))
                LoadScene(new Scene1());

            List<Entity> entitiesToUpdate = new List<Entity>(loadedEntities.Values);

            foreach(Entity e in entitiesToUpdate)
                e.Update();

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            List<Entity> entitiesToDraw = new List<Entity>(loadedEntities.Values);

            foreach (Entity e in entitiesToDraw)
            {
                e.Draw(spriteBatch);
                foreach (Script s in e.attachedScripts)
                    s.Draw(spriteBatch);
            }

            spriteBatch.DrawString(pixelFont, $"Score: {score}", new Vector2(20, 20), Color.White);

            if (isDead)
                spriteBatch.DrawString(SceneManager.pixelFont, $"You Are Dead!\nPress R to Restart", new Vector2(SceneManager.screenWidth / 2 - 150, SceneManager.screenHeight / 2 - 20), Color.White);
        }
    }
}
