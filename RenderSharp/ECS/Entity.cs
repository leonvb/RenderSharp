using System.Collections.Generic;
using RenderSharp.Components;

namespace RenderSharp.ECS
{
    public class Entity
    {
        public int UUID;
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string Tag { get; set; }
        public string Layer { get; set; }
        public string EditorIcon { get; set; }

        public Scene Scene;
        public List<Component> Components;

        public Entity()
        {
            this.UUID = 0; // Set to unique ID
            this.Components = new List<Component>();

            TransformComponent transform = new TransformComponent();

            AddComponent(new TransformComponent());

            TransformSystem.Register(transform);
        }

        public Entity(int UUID, string Name, bool Enabled, string Tag, string Layer, string EditorIcon)
        {
            this.UUID = UUID; // Set to unique ID
            this.Name = Name;
            this.Enabled = Enabled;
            this.Tag = Tag;
            this.Layer = Layer;
            this.EditorIcon = EditorIcon;

            this.Components = new List<Component>();

            TransformComponent transform = new TransformComponent();

            AddComponent(new TransformComponent());

            TransformSystem.Register(transform);
        }

        public bool HasComponent<T>()
        {
            return false;
        }

        public void AddComponent(Component Component)
        {
            Component.Entity = this;
            this.Components.Add(Component);
        }

        public void RemoveComponent<T>()
        {

        }

        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in this.Components)
            {
                if (component.GetType().Equals(typeof(T)))
                {
                    return (T)component;
                }
            }

            return null;
        }
    }
}
