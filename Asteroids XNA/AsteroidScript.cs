using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Asteroids_XNA
{
    class AsteroidScript : Script
    {
        Vector2 moveDir;
        float moveSpd;

        public override void Start()
        {
            Random rand = new Random();
            moveDir.X = rand.Next(-100, 100);
            moveDir.Y = rand.Next(-100, 100);

            if (moveDir != Vector2.Zero)
                moveDir.Normalize();

            moveSpd = (float)rand.NextDouble() * 3 + 0.5f;
            attachedEntity._loopOverScreenEdge = true;
            attachedEntity._loopOverScreenEdgeMargin = 40;
        }

        public override void Update()
        {
            attachedEntity._position += moveSpd * moveDir;
        }

        public void Destroy()
        {
            SceneManager.DestroyEntity(attachedEntity._id);

            if (attachedEntity._scale.Length() > 2.5f)
            {
                Random rand = new Random();
                

                if (rand.NextDouble() >= 0.7f)
                {
                    Entity asteroid = SceneManager.SpawnEntity(new Entity(SceneManager.asteroid1Texture, new Vector2(rand.Next(SceneManager.screenWidth, SceneManager.screenWidth + 1000), rand.Next(SceneManager.screenHeight, SceneManager.screenHeight + 1000))));
                    asteroid._scale = new Vector2((float)rand.NextDouble() * 3.25f + 2);
                    asteroid._angle = rand.Next(360);
                    asteroid._tag = "asteroid";
                    asteroid.AttachScript(new AsteroidScript());
                }

                for (int i = 0; i < 2; i++)
                {
                    Entity asteroidSmall = SceneManager.SpawnEntity(new Entity(SceneManager.asteroid1Texture, attachedEntity._position));
                    asteroidSmall._scale = attachedEntity._scale / 1.5f;
                    asteroidSmall._angle = rand.Next(360);
                    asteroidSmall._tag = "asteroid";
                    asteroidSmall.AttachScript(new AsteroidScript());
                }
            }
        }
    }
}
