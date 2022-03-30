using System;
using System.Collections.Generic;
using System.Text;

namespace RenderSharp.EntityComponentSystem
{
    public abstract class Component
    {
        public string ComponentName { get; set; }
        public Entity Entity { get; set; }

        public abstract void Start();
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}
