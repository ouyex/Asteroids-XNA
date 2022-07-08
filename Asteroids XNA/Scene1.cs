using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_XNA
{
    class Scene1 : Scene
    {
        public override void Load()
        {
            Texture2D shipTexture = SceneManager.LoadTexture("ship");

            Entity rocket = SceneManager.SpawnEntity(new Entity(shipTexture, new Vector2(SceneManager.screenWidth / 2, SceneManager.screenHeight / 2)));
            rocket._scale = new Vector2(2f);
            rocket._tag = "rocket";
            rocket.AttachScript(new RocketScript());

            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                Entity asteroid = SceneManager.SpawnEntity(new Entity(SceneManager.asteroid1Texture, new Vector2(rand.Next(SceneManager.screenWidth, SceneManager.screenWidth + 1000), rand.Next(SceneManager.screenHeight, SceneManager.screenHeight + 1000))));
                asteroid._scale = new Vector2((float)rand.NextDouble() * 3.25f + 2);
                asteroid._angle = rand.Next(360);
                asteroid._tag = "asteroid";
                asteroid.AttachScript(new AsteroidScript());
            }

        }
    }
}
