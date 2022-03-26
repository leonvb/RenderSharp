using GlmSharp;
using RenderSharp.ECS;

namespace RenderSharp.Components
{
    public class CameraComponent : Component
    {
        private float _pitch;
        private float _fov = 45.0f;

        private vec3 _position;

        public vec3 Front { get; set; }
        public vec3 Up { get; set; }
        public vec3 Right { get; set; }

        public vec3 WorldUp { get; set; }


        public float Yaw { get; set; }

        public float Pitch
        {
            get { return this._pitch; }
            set
            {
                this._pitch = value;

                if (this._pitch > 89.0f)
                    this._pitch = 89.0f;
                else if (this._pitch < -89.0f)
                    this._pitch = -89.0f;
            }
        }

        public float FOV { 
            get { return this._fov; }
            set
            {
                this._fov = value;

                if (this._fov < 1.0f)
                    this._fov = 1.0f;

                if (this._fov > 45.0f)
                    this._fov = 45.0f;
            }
        }

        public mat4 view_matrix { get; set; }
        public mat4 projection_matrix { get; set; }

        public CameraComponent()
        {
            this.projection_matrix = mat4.Perspective(glm.Radians(FOV), 800.0f / 600.0f, 0.1f, 100.0f);

            this.WorldUp = vec3.UnitY;

            TransformComponent transform = this.Entity.GetComponent<TransformComponent>().Transform;
            this._position = transform.Position;

            Update();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            this.projection_matrix = mat4.Perspective(glm.Radians(FOV), 800.0f / 600.0f, 0.1f, 100.0f);

            vec3 front = new vec3();
            front.x = glm.Cos(glm.Radians(this.Yaw)) * glm.Cos(glm.Radians(this.Pitch));
            front.y = glm.Sin(glm.Radians(this.Pitch));
            front.z = glm.Sin(glm.Radians(this.Yaw)) * glm.Cos(glm.Radians(this.Pitch));

            this.Front = glm.Normalized(front);
            this.Right = glm.Normalized(glm.Cross(this.Front, WorldUp));
            this.Up = glm.Normalized(glm.Cross(this.Right, this.Front));


            this.view_matrix = mat4.LookAt(this._position, this._position + this.Front, WorldUp);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
    }
}
