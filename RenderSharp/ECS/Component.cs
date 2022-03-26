using RenderSharp.Components;

namespace RenderSharp.ECS
{
    public class Component
    {
        public Entity Entity { get; set; }
        public TransformComponent Transform 
        { 
            get { return this.Entity.GetComponent<TransformComponent>().Transform; }
            set { this.Transform = value; }
        }

        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}
