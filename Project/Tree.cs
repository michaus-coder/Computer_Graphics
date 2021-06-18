using System;
using System.Collections.Generic;
using System.Text;
using LearnOpenTK.Common;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System.IO;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CG
{

    class tree
    {

        protected List<Vector3> vertices = new List<Vector3>();
        protected List<Vector3> texture = new List<Vector3>();
        protected List<Vector3> normals = new List<Vector3>();
        protected List<uint> indeces = new List<uint>();

        protected int _VBO;
        protected int _VAO;
        protected int _EBO;

        protected Shader _shader;
        protected Matrix4 _transform;
        protected Matrix4 _transform_tmp;

        protected Matrix4 _projection;
        protected Matrix4 _view;

        float radius = 10.0f;
        float camx;
        float camz;

        Vector3 worldUp = new Vector3(0.0f, 0.1f, 0.0f);
        Vector3 cameraPos;
        Vector3 cameraFront = new Vector3(0.0f, 0.0f, 0.0f);

        float fov = 10.0f;
        float nearPlane = 0.1f;
        float farPlane = 10000.0f; //draw distance

        uint width = 800;
        uint height = 800;

        public List<mesh> child = new List<mesh>();
        public tree()
        { }

        public void set_transform()
        {
            _transform = Matrix4.Identity;

        }

        public void set_VAO()
        {
            _VAO = GL.GenVertexArray();
            GL.BindVertexArray(_VAO);
        }
        public void set_VBO()
        {
            _VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }



        public void set_EBO()
        {
            _EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indeces.Count * sizeof(uint), indeces.ToArray(), BufferUsageHint.StaticDraw);

        }
        public void set_view()
        {
            _view = Matrix4.Identity;
            //camx = (float)Math.Sin(GLFW.GetTime()) * radius;
            //camz = (float)Math.Cos(GLFW.GetTime()) * radius;
            //cameraPos = new Vector3(camx, 0.0f, camz);
            //_view = Matrix4.LookAt(cameraPos, cameraFront, worldUp);
            _shader.SetMatrix4("view", _view);
        }

        public void set_projection()
        {
            _projection = Matrix4.Identity;
            //_projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), (float)width / (float)height, nearPlane, farPlane);
            _shader.SetMatrix4("projection", _projection);
        }

        public void set_shader(string p1)
        {

            _shader = new Shader("M:/Documents/Petra/Grafika Komputer/Iverson/Tugas_Project_GrafKom_klmpk9/Tugas_Project_GrafKom_klmpk9/Project/Shaders/shader.vert", "M:/Documents/Petra/Grafika Komputer/Iverson/Tugas_Project_GrafKom_klmpk9/Tugas_Project_GrafKom_klmpk9/Project/Shaders/" + p1 + ".frag");

            set_projection();
            _shader.Use();
        }
        public void set_shader_sunflower(string p1)
        {

            _shader = new Shader("D:/Visual Studio source/Computer Graphic/Tugas_Project_GrafKom_klmpk9/Project/Shaders_sunflower/shader.vert", "D:/Visual Studio source/Computer Graphic/Tugas_Project_GrafKom_klmpk9/Project/Shaders_sunflower/" + p1 + ".frag");

            set_projection();
            _shader.Use();
        }
        public void set_shader_mushroom(string p1)
        {

            _shader = new Shader("D:/Visual Studio source/Computer Graphic/Tugas_Project_GrafKom_klmpk9/Project/Shaders_mushroom/shader.vert", "D:/Visual Studio source/Computer Graphic/Tugas_Project_GrafKom_klmpk9/Project/Shaders_mushroom/" + p1 + ".frag");

            set_projection();
            _shader.Use();
        }
        public void set_shader_environment(string p1)
        {

            _shader = new Shader("D:/Visual Studio source/Computer Graphic/Tugas_Project_GrafKom_klmpk9/Project/Shaders_environment/shader.vert", "D:/Visual Studio source/Computer Graphic/Tugas_Project_GrafKom_klmpk9/Project/Shaders_environment/" + p1 + ".frag");

            set_projection();
            _shader.Use();
        }
        public void set_shader_lawnmower(string p1)
        {

            _shader = new Shader("M:/Documents/Petra/Grafika Komputer/Project_Grafkom/Project_Grafkom/Shaders/shader.vert",
                "M:/Documents/Petra/Grafika Komputer/Project_Grafkom/Project_Grafkom/Shaders/shader.frag");

            set_projection();
            _shader.Use();
        }

        public void initialize(string p1)
        {
            set_transform();
            LoadObjFile(p1);
            set_VAO();
            set_VBO();
            set_EBO();

        }

        public void Render(Camera _camera, Vector3 _color)
        {
            _shader.Use();
            _shader.SetVector3("my_color", _color);
            _shader.SetMatrix4("transform", _transform);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            GL.BindVertexArray(_VAO);
            GL.DrawElements(PrimitiveType.TriangleFan, indeces.Count, DrawElementsType.UnsignedInt, 0);
            //foreach (var meshobj in child)
            //{
            //    meshobj.Render(_camera, );
            //}
        }
        public void Render_Arrays(Camera _camera, Vector3 _color)
        {
            _shader.Use();
            _shader.SetVector3("my_color", _color);
            _shader.SetMatrix4("transform", _transform);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            //rotate();
            GL.BindVertexArray(_VAO);
            //perlu diganti di parameter 2

            GL.DrawArrays(PrimitiveType.TriangleFan,
               0, vertices.Count);
        }

        public void save()
        {
            _transform_tmp = _transform;
        }
        public void reset()
        {
            _transform = _transform_tmp;
        }

        public void rotate(float dr)
        {
            _transform = _transform * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(dr));

        }
        public void scale(float r)
        {
            _transform = _transform * Matrix4.CreateScale(r);
        }

        public void translate(float dx, float dy, float dz)
        {
            _transform = _transform * Matrix4.CreateTranslation(dx, dy, dz);

        }




        public void LoadObjFile(string path)
        {

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Unable to open \"" + path + "\", does not exist.");
            }

            using (StreamReader streamReader = new StreamReader(path))
            {
                while (!streamReader.EndOfStream)
                {

                    List<string> words = new List<string>(streamReader.ReadLine().ToLower().Split(' '));

                    words.RemoveAll(s => s == string.Empty);

                    if (words.Count == 0)
                    {
                        continue;
                    }

                    string type = words[0];
                    words.RemoveAt(0);

                    switch (type)
                    {

                        case "v":
                            vertices.Add(new Vector3(float.Parse(words[0]) / 10, float.Parse(words[1]) / 10, float.Parse(words[2]) / 10));
                            break;

                        case "vt":
                            texture.Add(new Vector3(float.Parse(words[0]), float.Parse(words[1]),
                                                            words.Count < 3 ? 0 : float.Parse(words[2])));
                            break;

                        case "vn":
                            normals.Add(new Vector3(float.Parse(words[0]), float.Parse(words[1]), float.Parse(words[2])));
                            break;

                        case "f":
                            foreach (string w in words)
                            {
                                if (w.Length == 0)
                                    continue;

                                string[] comps = w.Split('/');

                                indeces.Add(uint.Parse(comps[0]) - 1);

                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}
