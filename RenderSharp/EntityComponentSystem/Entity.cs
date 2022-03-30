using System;
using System.Collections.Generic;

namespace RenderSharp.EntityComponentSystem
{
    public class Entity
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }

        public Entity Parent { get; set; }
        public List<Entity> Children { get; set; }

        public Transform LocalTransform { get; set; }
        public Transform WorldTransform { get; set; }

        public List<Component> Components { get; set; }

        public Entity() { }

        public Entity(string Name, Transform Transform)
        {
            this.Children = new List<Entity>();
            this.Components = new List<Component>();
            this.WorldTransform = new Transform();

            this.Name = Name;
            this.Enabled = true;
            this.Parent = null;
            this.LocalTransform = Transform;

            CalculateWorldTransform();
        }

        public void CalculateWorldTransform()
        {
            if (this.Parent is null)
            {
                this.WorldTransform.Position = this.LocalTransform.Position;
                this.WorldTransform.Rotation = this.LocalTransform.Rotation;
                this.WorldTransform.Scale = this.LocalTransform.Scale;
            }
            else
            {
                this.WorldTransform.Position = this.Parent.WorldTransform.Position + this.LocalTransform.Position;
                this.WorldTransform.Rotation = this.Parent.WorldTransform.Rotation + this.LocalTransform.Rotation;
                this.WorldTransform.Scale = this.Parent.WorldTransform.Scale + this.LocalTransform.Scale;
            }

            foreach (Entity child in this.Children)
                child.CalculateWorldTransform();
        }

        // Children
        public void SetParent(Entity parent)
        {
            if (parent is null)
                return;

            if (!(this.Parent is null))
                this.Parent.RemoveChild(this);

            if (!(parent is null))
            {
                this.Parent = parent;
                parent.AddChild(this);
                CalculateWorldTransform();
            }
        }

        public void AddChild(Entity child)
        {
            child.Parent = this;
            this.Children.Add(child);
        }

        public void RemoveChild(Entity child)
        {
            if (this.Children.Contains(child))
                this.Children.Remove(child);
        }

        // Components
        public void AddComponent(Component component)
        {
            if (!(component is null))
            {
                component.Entity = this;
                this.Components.Add(component);
            }
        }

        public T? GetComponent<T>() where T : class
        {
            foreach (Component component in this.Components)
            {
                if (component.GetType().Equals(typeof(T)))
                {
                    return (T)Convert.ChangeType(component, typeof(T));
                }
            }

            return null;
        }

        public void RemoveComponent(Component component)
        {
            if (!(component is null))
            {
                for (int i = 0; i < this.Components.Count; i++)
                {
                    Component c = this.Components[i];

                    if (c.Equals(component))
                        this.Components.RemoveAt(i);
                }
            }
        }

        // Used to call component and child methods
        public void Start()
        {
            foreach (Component component in this.Components)
                component.Start();

            foreach (Entity child in this.Children)
                child.Start();
        }

        public void Update()
        {
            foreach (Component component in this.Components)
                component.Update();

            foreach (Entity child in this.Children)
                child.Update();
        }

        public void FixedUpdate()
        {
            foreach (Component component in this.Components)
                component.FixedUpdate();

            foreach (Entity child in this.Children)
                child.FixedUpdate();
        }
    }
}
