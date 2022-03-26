using System;
using System.Collections.Generic;
using System.Text;

namespace RenderSharp
{
    public class Time
    {
        public static float lastFrame { get; set; } = 0.0f;
        public static float deltaTime { get; set; }
    }
}
