using GlmSharp;
using RenderSharp.ECS;

namespace RenderSharp.Components
{
    public class TransformComponent : Component
    {
        public vec3 Position;
        public vec3 Rotation;
        public vec3 Scale;

        mat4 mat_position { get { return mat4.Translate(this.Position.x, this.Position.y, this.Position.z); } }
        mat4 mat_rotation { get { return mat4.RotateY(this.Rotation.y) * mat4.RotateX(this.Rotation.x) *  mat4.RotateZ(this.Rotation.z); } }
        mat4 mat_scale { get { return mat4.Scale(this.Scale.x, this.Scale.y, this.Scale.z); } }
        
        public mat4 model_matrix { get { return this.mat_scale * this.mat_rotation * this.mat_position; } }

        public TransformComponent()
        {
            this.Position = new vec3(0, 0, 0);
            this.Rotation = new vec3(0, 0, 0);
            this.Scale = new vec3(1, 1, 1);
        }

        public TransformComponent(vec3 Position, vec3 Rotation, vec3 Scale)
        {
            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;
        }
    }
}
