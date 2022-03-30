using System;
using GLFW;
using OpenGL;
using GlmSharp;

namespace RenderSharp
{
    public class Window
    {
        private PositionCallback windowPositionCallback;
        private SizeCallback windowSizeCallback, framebufferSizeCallback;
        private FocusCallback windowFocusCallback;
        private WindowCallback closeCallback, windowRefreshCallback;
        private FileDropCallback dropCallback;
        private MouseCallback cursorPositionCallback, scrollCallback;
        private MouseEnterCallback cursorEnterCallback;
        private MouseButtonCallback mouseButtonCallback;
        private CharModsCallback charModsCallback;
        private KeyCallback keyCallback;
        private WindowMaximizedCallback windowMaximizeCallback;
        private WindowContentsScaleCallback windowContentScaleCallback;

        GLFW.Window window;

        Mesh mesh;

        Shader shader;
        Texture texture0;
        Texture texture1;

        Camera camera;

        public Window()
        {
            PrepareContext();

            GLFW.Window window = CreateWindow();
            BindCallbacks(window);

            Start();
            Glfw.SetInputMode(window, InputMode.Cursor, (int)CursorMode.Disabled);

            #region Load Textures

            texture0 = new Texture(@"Resources\Textures\container.jpg");
            texture1 = new Texture(@"Resources\Textures\awesomeface.png");
            shader.Use();
            shader.SetInt("texture0", 0);
            shader.SetInt("texture1", 1);

            #endregion Load Textures

            while (!Glfw.WindowShouldClose(window))
            {
                Update();
                Render();

                Glfw.SwapBuffers(window);
                Glfw.PollEvents();
            }
        }

        public void PrepareContext()
        {
            Glfw.Init();
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.Doublebuffer, true);
            Glfw.WindowHint(Hint.Decorated, true);
        }

        public GLFW.Window CreateWindow()
        {
            window = Glfw.CreateWindow(800, 600, "Test", Monitor.None, GLFW.Window.None);
            if (window == null)
            {
                Console.WriteLine("Failed to create GLFW window.");
                Glfw.Terminate();
            }

            Glfw.MakeContextCurrent(window);
            GL.Import(Glfw.GetProcAddress); // Important. Don't know why, but events don't work without this. ??? Link GL with GLFW window ???

            GL.glClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            return window;
        }

        private void BindCallbacks(GLFW.Window window)
        {
            //windowPositionCallback = (_, x, y) => OnPositionChanged(x, y);
            //windowSizeCallback = (_, w, h) => OnSizeChanged(window, w, h);
            //windowFocusCallback = (_, focusing) => OnFocusChanged(focusing);
            //closeCallback = _ => OnClosing();
            //dropCallback = (_, count, arrayPtr) => OnFileDrop(count, arrayPtr);
            cursorPositionCallback = (_, x, y) => OnMouseMove(window, x, y);
            //cursorEnterCallback = (_, entering) => OnMouseEnter(entering);
            //mouseButtonCallback = (_, button, state, mod) => OnMouseButton(button, state, mod);
            scrollCallback = (_, x, y) => OnMouseScroll(window, x, y);
            //charModsCallback = (_, cp, mods) => OnCharacterInput(cp, mods);
            framebufferSizeCallback = (_, w, h) => OnFramebufferSizeChanged(window, w, h);
            //windowRefreshCallback = _ => Refreshed?.Invoke(this, EventArgs.Empty);
            keyCallback = (_, key, code, state, mods) => OnKey(window, key, code, state, mods);
            //windowMaximizeCallback = (_, maximized) => OnMaximizeChanged(maximized);
            //windowContentScaleCallback = (_, x, y) => OnContentScaleChanged(x, y);

            //Glfw.SetWindowPositionCallback(window, windowPositionCallback);
            //Glfw.SetWindowSizeCallback(window, windowSizeCallback);
            //Glfw.SetWindowFocusCallback(window, windowFocusCallback);
            //Glfw.SetCloseCallback(window, closeCallback);
            //Glfw.SetDropCallback(window, dropCallback);
            Glfw.SetCursorPositionCallback(window, cursorPositionCallback);
            //Glfw.SetCursorEnterCallback(window, cursorEnterCallback);
            //Glfw.SetMouseButtonCallback(window, mouseButtonCallback);
            Glfw.SetScrollCallback(window, scrollCallback);
            //Glfw.SetCharModsCallback(window, charModsCallback);
            Glfw.SetFramebufferSizeCallback(window, windowSizeCallback);
            //Glfw.SetWindowRefreshCallback(window, windowRefreshCallback);
            Glfw.SetKeyCallback(window, keyCallback);
            //Glfw.SetWindowMaximizeCallback(window, windowMaximizeCallback);
            //Glfw.SetWindowContentScaleCallback(window, windowContentScaleCallback);

        }

        private void Start()
        {
            this.camera = new Camera();
            this.shader = new Shader();
            this.mesh = Mesh.Cube();
        }

        private void Update()
        {
            process_input(window);

            float currentFrame = (float)Glfw.Time;
            Time.deltaTime = currentFrame - Time.lastFrame;
            Time.lastFrame = currentFrame;

            camera.Update();
        }

        private void Render()
        {
            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);

            shader.Use();
            texture0.Use(GL.GL_TEXTURE0);
            texture1.Use(GL.GL_TEXTURE1);

            shader.SetMatrix4("model_matrix", mesh.Transform.modelMatrix);
            shader.SetMatrix4("view_matrix", camera.view_matrix);
            shader.SetMatrix4("projection_matrix", camera.projection_matrix);

            GL.glEnable(GL.GL_DEPTH_TEST);

            GL.glBindVertexArray(this.mesh.VAO);
            unsafe
            {
                GL.glDrawElements(GL.GL_TRIANGLES, mesh.Indices.Length, GL.GL_UNSIGNED_INT, (void*)0);
            }
        }

        private void process_input(GLFW.Window window)
        {
            if (Glfw.GetKey(window, Keys.Escape) == InputState.Press)
                Glfw.SetWindowShouldClose(window, true);

            if (Glfw.GetKey(window, Keys.W) == InputState.Press)
                camera.Position -= new vec3(0.0f, 0.0f, 1.0f) * Time.deltaTime;

            if (Glfw.GetKey(window, Keys.S) == InputState.Press)
                camera.Position += new vec3(0.0f, 0.0f, 1.0f) * Time.deltaTime;

            if (Glfw.GetKey(window, Keys.A) == InputState.Press)
                camera.Position -= new vec3(1.0f, 0.0f, 0.0f) * Time.deltaTime;

            if (Glfw.GetKey(window, Keys.D) == InputState.Press)
                camera.Position += new vec3(1.0f, 0.0f, 0.0f) * Time.deltaTime;

            if (Glfw.GetKey(window, Keys.LeftShift) == InputState.Press)
                camera.Position += new vec3(0.0f, 1.0f, 0.0f) * Time.deltaTime;

            if (Glfw.GetKey(window, Keys.Space) == InputState.Press)
                camera.Position -= new vec3(0.0f, 1.0f, 0.0f) * Time.deltaTime;
        }

        protected virtual void OnKey(GLFW.Window window, Keys key, int scanCode, InputState state, ModifierKeys mods)
        {
            
        }

        float lastX = 400;
        float lastY = 300;
        protected virtual void OnMouseMove(GLFW.Window window, double xPos, double yPos)
        {
            float xOffset = (float)xPos - lastX;
            float yOffset = lastY - (float)yPos;
            lastX = (float)xPos;
            lastY = (float)yPos;

            float sensitivity = 0.1f;
            xOffset *= sensitivity;
            yOffset *= sensitivity;

            this.camera.Yaw += xOffset;
            this.camera.Pitch += yOffset;
        }

        private void OnMouseScroll(GLFW.Window window, double x, double y)
        {
            Console.WriteLine(y);

            this.camera.FOV -= (float)y;
        }

        public void OnFramebufferSizeChanged(GLFW.Window window, int width, int height)
        {
            GL.glViewport(0, 0, width, height);
        }

        ~Window()
        {
            Glfw.Terminate();
        }
    }
}
