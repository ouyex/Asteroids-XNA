using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Diagnostics;

namespace Asteroids_XNA
{
    class RocketScript : Script
    {
        // Trackers
        Vector2 forwardVelocity;
        float rotationVelocity;
        bool hasStarted = false;
        Stopwatch startWatch = new Stopwatch();
        int timerInt;

        // Speeds
        float forwardSpeed = 0.25f;
        float rotationSpeed = 0.35f;
        float forwardFriction = 1.01f;
        float rotationFriction = 1.05f;
        float brakingFriction = 1.05f;

        // Textures
        Texture2D bulletTexture;

        public override void Start()
        {
            bulletTexture = SceneManager.LoadTexture("bullet");
            attachedEntity._loopOverScreenEdge = true;
            attachedEntity._loopOverScreenEdgeMargin = 20;
            attachedEntity._collisionRectanglePadding = new Vector2(-5);
            startWatch.Start();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!hasStarted)
                spriteBatch.DrawString(SceneManager.pixelFont, $"{timerInt}", new Vector2(SceneManager.screenWidth / 2 - 10, SceneManager.screenHeight / 2 - 100), Color.White);
            
        }

        public override void Update()
        {
            if (!hasStarted)
            {
                if (startWatch.ElapsedMilliseconds < 1000)
                    timerInt = 3;
                else if (startWatch.ElapsedMilliseconds < 2000)
                    timerInt = 2;
                else
                    timerInt = 1;
            }

            if (startWatch.ElapsedMilliseconds < 3000f)
                return;
            else
                hasStarted = true;

            // Input
            int rotInput = (InputHelper.GetKey(Keys.D) ? 1 : 0) - (InputHelper.GetKey(Keys.A) ? 1 : 0);
            int forwardInput = (InputHelper.GetKey(Keys.W) ? 1 : 0);

            // Update velocities
            Vector2 forwardDirection = new Vector2((float)Math.Cos(attachedEntity._angle),
                                                   (float)Math.Sin(attachedEntity._angle));
            forwardVelocity += forwardDirection * forwardInput * forwardSpeed;
            rotationVelocity += rotInput * rotationSpeed;

            // Friciton
            rotationVelocity /= rotationFriction;
            forwardVelocity /= forwardFriction;

            if (InputHelper.GetKey(Keys.S))
            {
                forwardVelocity /= brakingFriction;
                rotationVelocity /= brakingFriction;
            }
            

            // Apply
            attachedEntity._position += forwardVelocity;
            attachedEntity._angle += rotationVelocity * MathF.PI/180;

            // Shooting
            if (InputHelper.GetKeyPressed(Keys.Space))
            {
                Entity bulletEntity = new Entity(bulletTexture, attachedEntity._position + forwardDirection);
                bulletEntity._scale = new Vector2(0.1f);
                bulletEntity._destroyOnScreenExit = true;

                if (forwardDirection != Vector2.Zero)
                    forwardDirection.Normalize();

                bulletEntity.AttachScript(new BulletScript(forwardDirection, 25));
                SceneManager.SpawnEntity(bulletEntity);
            }

            // Death
            Dictionary<int, Entity> hitAsteroids = attachedEntity.GetCollisionsTag("asteroid");
            if (hitAsteroids.Count > 0 && !SceneManager.debugMode)
            {
                SceneManager.DestroyEntity(attachedEntity._id);

                Entity asteroid = hitAsteroids.Values.ToList()[0];
                AsteroidScript asteroidScript = null;
                foreach (Script s in asteroid.attachedScripts)
                    if (s.GetType().ToString() == "Asteroids_XNA.AsteroidScript")
                        asteroidScript = s as AsteroidScript;

                asteroidScript.Destroy();
                SceneManager.isDead = true;
            }
        }
    }
}
