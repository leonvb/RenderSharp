using System;
using System.Collections.Generic;
using System.Text;

namespace RenderSharp.EntityComponentSystem
{
    public class Scene
    {
        public Entity SceneRoot { get; set; }

        public Scene()
        {
            this.SceneRoot = new Entity("Root", new Transform());
        }
        public void Start()
        {
            SceneRoot.Start();
        }

        public void Update()
        {
            SceneRoot.Update();
        }

        public void FixedUpdate()
        {
            SceneRoot.FixedUpdate();
        }
    }
}
