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

            moveSpd = (float)rand.NextDouble() * 3;
            attachedEntity._loopOverScreenEdge = true;
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
                for (int i = 0; i < 2; i++)
                {
                    Random rand = new Random();
                    Entity asteroid = SceneManager.SpawnEntity(new Entity(SceneManager.asteroid1Texture, attachedEntity._position));
                    asteroid._scale = attachedEntity._scale / 1.5f;
                    asteroid._angle = rand.Next(360);
                    asteroid._tag = "asteroid";
                    asteroid.AttachScript(new AsteroidScript());
                }
            }
        }
    }
}
