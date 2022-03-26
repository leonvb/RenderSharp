using System.Collections.Generic;
using RenderSharp.Components;
using GlmSharp;

namespace RenderSharp.ECS
{
    public class Scene
    {

        public Dictionary<int, Entity> Entities;
        public int EntityCounter;

        public Scene()
        {
            this.Entities = new Dictionary<int, Entity>();

            // Create an Enitity. Set the position. Create a Mesh Component. Add the Mesh Component to the entity
            Entity test_object = new Entity(0, "Test Object", true, "None", "Default", "");
            test_object.GetComponent<TransformComponent>().Transform = new TransformComponent(new vec3(0, 0, 0), new vec3(0, 0, 0), new vec3(1, 1, 1));
            test_object.Scene = this;
            MeshComponent meshComponent = MeshComponent.Cube();
            test_object.AddComponent(meshComponent);

            // Create an Enitity. Set the transform. Create a Camera component. Add the Camera Component to the entity
            Entity camera = new Entity(1, "Main Camera", true, "MainCamera", "Default", "");
            camera.GetComponent<TransformComponent>().Transform = new TransformComponent(new vec3(0.0f, 0.0f, 3.0f), new vec3(0, 0, 0), new vec3(0, 0, 0));
            camera.Scene = this;
            CameraComponent cameraComponent = new CameraComponent();
            camera.AddComponent(cameraComponent);

            this.Entities.Add(0, test_object);
            this.Entities.Add(1, camera);

            MeshSystem.Register(meshComponent);
            CameraSystem.Register(cameraComponent);
        }

        public void CreateEntity()
        {

        }

        public void GetEntity()
        {

        }

        public void DeleteEntity()
        {

        }

        public void GetRegistry()
        {

        }

        public static void Update()
        {
            TransformSystem.Update();
            MeshSystem.Update();
            CameraSystem.Update();
        }
    }
}
