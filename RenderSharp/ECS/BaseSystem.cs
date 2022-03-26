using System.Collections.Generic;
using RenderSharp.Components;

namespace RenderSharp.ECS
{
    public class BaseSystem<T> where T : Component
    {
        protected static List<T> Components = new List<T>();

        public static void Register(T Component)
        {
            Components.Add(Component);
        }

        public static void Start()
        {
            foreach (T component in Components)
            {
                component.Start();
            }
        }

        public static void Update()
        {
            foreach (T component in Components)
            {
                component.Update();
            }
        }

        public static void FixedUpdate()
        {
            foreach (T component in Components)
            {
                component.FixedUpdate();
            }
        }
    }

    public class TransformSystem : BaseSystem<TransformComponent> { }
    public class MeshSystem : BaseSystem<MeshComponent> { }
    public class CameraSystem : BaseSystem<CameraComponent> { }
}
