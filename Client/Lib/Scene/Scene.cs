﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCreep.Client.Lib.Input;

namespace SpaceCreep.Client.Lib.Scene
{
    public abstract class Scene
    {
        protected Game Game;
        protected SpriteBatch SpriteBatch;

        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);

        public virtual void MouseDrag(MouseButton button)
        {
        }

        public virtual void MouseClick(MouseButton button)
        {
        }

        public virtual void MouseDoubleClick(MouseButton button)
        {
        }

        public virtual void UpdateKeyboardInput()
        {
        }

        public virtual void UpdateMouseInput()
        {
        }

        public virtual void MouseUp(MouseButton mouseButton)
        {
        }

        public virtual void MouseScroll()
        {
        }

        public virtual void Load()
        {
        }

        public virtual void MouseDown(MouseButton button)
        {
        }
    }
}