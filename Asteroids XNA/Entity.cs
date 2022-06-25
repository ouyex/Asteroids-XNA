using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_XNA
{
    public class Entity
    {
        public int _id;
        public string _tag;
        public Texture2D _texture = null;
        public Vector2 _position = Vector2.Zero;
        public Color _color = Color.White;
        public float _angle = 0;
        public Vector2 _scale = Vector2.One;
        public Vector2 _origin = Vector2.Zero;
        public bool _destroyOnScreenExit = false;
        public bool _loopOverScreenEdge = false;
        public int _loopOverScreenEdgeMargin = 0;
        public Rectangle _collisionRectangle = Rectangle.Empty;
        public Vector2 _collisionRectanglePadding = Vector2.Zero;

        private bool ranStart = false;
        public List<Script> attachedScripts = new List<Script>();

        public Entity(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }

        

        public void CenterOrigin()
        {
            if (_texture == null)
                return;
            _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }

        public virtual void Start()
        {
            foreach (Script s in attachedScripts)
                s.Start();
            CenterOrigin();
        }

        public Dictionary<int, Entity> GetCollisionsTag(string tag)
        {
            List<Entity> entitiesToCheck = new List<Entity>(SceneManager.loadedEntities.Values);
            Dictionary<int, Entity> collidingEntities = new Dictionary<int, Entity>();

            foreach (Entity e in entitiesToCheck)
            {
                if (e._tag == tag && e._collisionRectangle != Rectangle.Empty && _collisionRectangle.Intersects(e._collisionRectangle))
                {
                    collidingEntities.Add(e._id, e);
                }
            }

            return collidingEntities;
        }

        public bool IsInScreen()
        {
            return _position.X > 0 && _position.Y > 0 && _position.X < SceneManager.screenWidth && _position.Y < SceneManager.screenHeight;
        }

        public Script AttachScript(Script script)
        {
            attachedScripts.Add(script);
            script.attachedEntity = this;
            return script;
        }

        public virtual void Update()
        {
            // Start
            if (!ranStart)
            {
                Start();
                ranStart = true;
            }

            // Update collider
            _collisionRectangle = new Rectangle(
                (int)(_position.X - _origin.X * _scale.X - _collisionRectanglePadding.X / 2),
                (int)(_position.Y - _origin.Y * _scale.Y - _collisionRectanglePadding.Y / 2),
                (int)(_texture.Width * _scale.X + _collisionRectanglePadding.X),
                (int)(_texture.Height * _scale.Y + _collisionRectanglePadding.Y));

            // Destroy on screen exit
            if (_destroyOnScreenExit)
                if (_position.X > SceneManager.screenWidth || _position.X < 0 || _position.Y > SceneManager.screenHeight || _position.Y < 0)
                    SceneManager.DestroyEntity(_id);

            // Loop over edge
            // x
            if (_position.X < -_loopOverScreenEdgeMargin)
                _position.X = SceneManager.screenWidth + _loopOverScreenEdgeMargin;
            else if (_position.X > SceneManager.screenWidth + _loopOverScreenEdgeMargin)
                _position.X = -_loopOverScreenEdgeMargin;
            // y
            if (_position.Y < -_loopOverScreenEdgeMargin)
                _position.Y = SceneManager.screenHeight + _loopOverScreenEdgeMargin;
            else if (_position.Y > SceneManager.screenHeight + _loopOverScreenEdgeMargin)
                _position.Y = -_loopOverScreenEdgeMargin;
            
            // Update Scripts
            foreach (Script s in attachedScripts)
                s.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, _position, null, _color, _angle, _origin, _scale, SpriteEffects.None, 0);
            
            if (SceneManager.debugMode)
                spriteBatch.Draw(SceneManager.pixel, _collisionRectangle, new Color(1, 0, 0, 0.4f));
        }
    }
}
