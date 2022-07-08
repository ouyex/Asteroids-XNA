using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Asteroids_XNA
{
    class BulletScript : Script
    {
        public Vector2 direction;
        public float speed;
        
        public BulletScript(Vector2 dir, float spd)
        {
            direction = dir;
            speed = spd;
        }

        public override void Start()
        {
            attachedEntity._collisionRectanglePadding = new Vector2(10);
        }

        public override void Update()
        {
            attachedEntity._position += direction * speed;

            Dictionary<int, Entity> hitAsteroids = attachedEntity.GetCollisionsTag("asteroid");
            if (hitAsteroids.Count > 0)
            {
                SceneManager.DestroyEntity(attachedEntity._id);

                Entity hitAsteroid = hitAsteroids.Values.ToList()[0];
                AsteroidScript hitAsteroidScript = null;

                foreach (Script s in hitAsteroid.attachedScripts)
                    if (s.GetType().ToString() == "Asteroids_XNA.AsteroidScript")
                        hitAsteroidScript = s as AsteroidScript;
                if (hitAsteroidScript != null)
                    hitAsteroidScript.Destroy();
                SceneManager.score++;
            }
        }
    }
}
