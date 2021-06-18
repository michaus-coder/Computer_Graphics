using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using LearnOpenTK.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace CG
{
    class Windows : GameWindow
    {
        //wong
        mesh pohon_daun = new mesh();
        mesh pohon_batang = new mesh();
        mesh pohon_cabang = new mesh();
        mesh pulau = new mesh();
        mesh rumput = new mesh();

        //sastra
        mesh aneh = new mesh();
        mesh bola_kanan = new mesh();
        mesh bola_kiri = new mesh();
        mesh dasar_nisan = new mesh();
        mesh dasar_tengah = new mesh();
        mesh nisan = new mesh();
        mesh nisan_depan = new mesh();
        mesh paling_dasar = new mesh();
        mesh papan_belakang = new mesh();
        mesh papan_kanan = new mesh();
        mesh papan_kiri = new mesh();
        mesh tabung_kiri = new mesh();
        mesh tabung_kanan = new mesh();


        //bryan
        mesh pucuk = new mesh();
        mesh kepala = new mesh();
        mesh mata_kiri = new mesh();
        mesh mata_kanan = new mesh();
        mesh muka = new mesh();
        mesh badan = new mesh();
        mesh kaki = new mesh();


        private Camera _camera;
        private Vector3 _objectPos;
        private Vector2 _lastMousePosition;
        private bool _firstMove;
        private Matrix4 transform;
        float startYaw;

        private Matrix4 generateArbRotationMatrix(Vector3 axis, Vector3 center, float degree)
        {
            var rads = MathHelper.DegreesToRadians(degree);

            var secretFormula = new float[4, 4] {
                { (float)Math.Cos(rads) + (float)Math.Pow(axis.X, 2) * (1 - (float)Math.Cos(rads)), axis.X* axis.Y * (1 - (float)Math.Cos(rads)) - axis.Z * (float)Math.Sin(rads),    axis.X * axis.Z * (1 - (float)Math.Cos(rads)) + axis.Y * (float)Math.Sin(rads),   0 },
                { axis.Y * axis.X * (1 - (float)Math.Cos(rads)) + axis.Z * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Y, 2) * (1 - (float)Math.Cos(rads)), axis.Y * axis.Z * (1 - (float)Math.Cos(rads)) - axis.X * (float)Math.Sin(rads),   0 },
                { axis.Z * axis.X * (1 - (float)Math.Cos(rads)) - axis.Y * (float)Math.Sin(rads),   axis.Z * axis.Y * (1 - (float)Math.Cos(rads)) + axis.X * (float)Math.Sin(rads),   (float)Math.Cos(rads) + (float)Math.Pow(axis.Z, 2) * (1 - (float)Math.Cos(rads)), 0 },
                { 0, 0, 0, 1}
            };
            var secretFormulaMatrix = new Matrix4(
                new Vector4(secretFormula[0, 0], secretFormula[0, 1], secretFormula[0, 2], secretFormula[0, 3]),
                new Vector4(secretFormula[1, 0], secretFormula[1, 1], secretFormula[1, 2], secretFormula[1, 3]),
                new Vector4(secretFormula[2, 0], secretFormula[2, 1], secretFormula[2, 2], secretFormula[2, 3]),
                new Vector4(secretFormula[3, 0], secretFormula[3, 1], secretFormula[3, 2], secretFormula[3, 3])
            );

            return secretFormulaMatrix;
        }
        public Windows(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {

        }

        protected override void OnLoad()
        {
            GL.ClearColor(0.031f, 0f, 0.301f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            //wong
            pohon_daun.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Pohon/Leaves.obj");
            pohon_daun.set_shader("shader");
            pohon_daun.scale(2f);
            pohon_daun.translate(-0.5f, 0.7f, -0.7f);

            pohon_batang.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Pohon/Tree_Trunk_Main.obj");
            pohon_batang.set_shader("shader");
            pohon_batang.scale(2f);
            pohon_batang.translate(-0.5f, 0.7f, -0.7f);

            pohon_cabang.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Pohon/Tree_Trunk_Branch.obj");
            pohon_cabang.set_shader("shader");
            pohon_cabang.scale(2f);
            pohon_cabang.translate(-0.5f, 0.7f, -0.7f);

            pulau.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Pulau/Pulau.obj");
            pulau.set_shader("shader");
            pulau.scale(0.5f);
            pulau.translate(0f, -1.5f, 0f);

            rumput.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Pulau/Rumput.obj");
            rumput.set_shader("shader");
            rumput.scale(0.5f);
            rumput.translate(0f, -1.7f, 0f);


            //sastra
            aneh.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Kuburan/aneh.obj");
            aneh.set_shader("shader");
            aneh.translate(0f, -0.34f, 0f);
            aneh.scale(0.7f);

            bola_kanan.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Kuburan/bola_kanan.obj");
            bola_kanan.set_shader("shader");
            bola_kanan.translate(0f, -0.34f, 0f);
            bola_kanan.scale(0.7f);

            bola_kiri.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Kuburan/bola_kiri.obj");
            bola_kiri.set_shader("shader");
            bola_kiri.translate(0f, -0.34f, 0f);
            bola_kiri.scale(0.7f);

            dasar_nisan.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Kuburan/dasar_nisan.obj");
            dasar_nisan.set_shader("shader");
            dasar_nisan.translate(0f, -0.34f, 0f);
            dasar_nisan.scale(0.7f);

            dasar_tengah.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Kuburan/dasar_tengah.obj");
            dasar_tengah.set_shader("shader");
            dasar_tengah.translate(0f, -0.34f, 0f);
            dasar_tengah.scale(0.7f);

            nisan.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Kuburan/nisan.obj");
            nisan.set_shader("shader");
            nisan.translate(0f, -0.34f, 0f);
            nisan.scale(0.7f);

            nisan_depan.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Kuburan/nisan_depan.obj");
            nisan_depan.set_shader("shader");
            nisan_depan.translate(0f, -0.34f, 0f);
            nisan_depan.scale(0.7f);

            paling_dasar.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Kuburan/paling_dasar.obj");
            paling_dasar.set_shader("shader");
            paling_dasar.translate(0f, -0.35f, 0f);
            paling_dasar.scale(0.7f);

            papan_belakang.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Kuburan/papan_belakang.obj");
            papan_belakang.set_shader("shader");
            papan_belakang.translate(0f, -0.34f, 0f);
            papan_belakang.scale(0.7f);

            papan_kanan.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Kuburan/papan_kanan.obj");
            papan_kanan.set_shader("shader");
            papan_kanan.translate(0f, -0.34f, 0f);
            papan_kanan.scale(0.7f);

            papan_kiri.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Kuburan/papan_kiri.obj");
            papan_kiri.set_shader("shader");
            papan_kiri.translate(0f, -0.34f, 0f);
            papan_kiri.scale(0.7f);

            tabung_kiri.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Kuburan/tabung_kiri.obj");
            tabung_kiri.set_shader("shader");
            tabung_kiri.translate(0f, -0.34f, 0f);
            tabung_kiri.scale(0.7f);

            tabung_kanan.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Kuburan/tabung_kanan.obj");
            tabung_kanan.set_shader("shader");
            tabung_kanan.translate(0f, -0.34f, 0f);
            tabung_kanan.scale(0.7f);


            //bryan
            pucuk.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Pocong/pucuk_kepala_pocong.obj");
            pucuk.set_shader("shader");
            pucuk.scale(0.2f);
            pucuk.rotate(90f);

            kepala.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Pocong/kepala_pocong.obj");
            kepala.set_shader("shader");
            kepala.scale(0.2f);
            kepala.rotate(90f);

            mata_kiri.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Pocong/mata_kiri_pocong.obj");
            mata_kiri.set_shader("shader");
            mata_kiri.scale(0.2f);
            mata_kiri.rotate(90f);

            mata_kanan.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Pocong/mata_kanan_pocong.obj");
            mata_kanan.set_shader("shader");
            mata_kanan.scale(0.2f);
            mata_kanan.rotate(90f);

            muka.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Pocong/muka_pocong.obj");
            muka.set_shader("shader");
            muka.scale(0.2f);
            muka.rotate(90f);

            badan.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Pocong/badan_pocong.obj");
            badan.set_shader("shader");
            badan.scale(0.2f);
            badan.rotate(90f);

            kaki.initialize("M:/Documents/Petra/Grafika Komputer/Model 3D/Pocong/kaki_pocong.obj");
            kaki.set_shader("shader");
            kaki.scale(0.2f);
            kaki.rotate(90f);

            transform = Matrix4.Identity;
            var _cameraPosInit = new Vector3(0, 0, 0);
            _camera = new Camera(_cameraPosInit, Size.X / (float)Size.Y);
            _camera.Yaw -= 90f;
            _camera.Position -= _camera.Up * 0.1f;
            _camera.Position -= _camera.Front * 1f;
            startYaw = _camera.Yaw;
            CursorGrabbed = true;

            base.OnLoad();
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //sastra
            aneh.Render(_camera, new Vector3(0.780f, 0.819f, 0.807f));
            bola_kanan.Render(_camera, new Vector3(0.780f, 0.819f, 0.807f));
            bola_kiri.Render(_camera, new Vector3(0.780f, 0.819f, 0.807f));
            dasar_nisan.Render(_camera, new Vector3(0.709f, 0.709f, 0.709f));
            dasar_tengah.Render(_camera, new Vector3(0.709f, 0.709f, 0.709f));
            nisan.Render(_camera, new Vector3(0.709f, 0.709f, 0.709f));
            nisan_depan.Render(_camera, new Vector3(0.709f, 0.709f, 0.709f));
            paling_dasar.Render(_camera, new Vector3(0.372f, 0.486f, 0.454f));
            papan_belakang.Render(_camera, new Vector3(0.725f, 0.756f, 0.745f));
            papan_kanan.Render(_camera, new Vector3(0.725f, 0.756f, 0.745f));
            papan_kiri.Render(_camera, new Vector3(0.725f, 0.756f, 0.745f));
            tabung_kiri.Render(_camera, new Vector3(0.780f, 0.819f, 0.807f));
            tabung_kanan.Render(_camera, new Vector3(0.780f, 0.819f, 0.807f));

            //bryan
            pucuk.Render(_camera, new Vector3(0.980f, 0.925f, 0.760f));
            kepala.Render_as_linestrip(_camera, new Vector3(0.980f, 0.925f, 0.760f));
            mata_kiri.Render(_camera, new Vector3(0.878f, 0f, 0.105f));
            mata_kanan.Render(_camera, new Vector3(0.878f, 0f, 0.105f));
            muka.Render_as_linestrip(_camera, new Vector3(0f, 0f, 0f));
            badan.Render(_camera, new Vector3(0.980f, 0.925f, 0.760f));
            kaki.Render(_camera, new Vector3(0.980f, 0.925f, 0.760f));

            //wong
            pohon_daun.Render(_camera, new Vector3(0.333f, 0.498f, 0.164f));
            pohon_batang.Render(_camera, new Vector3(0.396f, 0.274f, 0.160f));
            pohon_cabang.Render(_camera, new Vector3(0.396f, 0.274f, 0.160f));
            pulau.Render(_camera, new Vector3(0.470f, 0.380f, 0.262f));
            rumput.Render(_camera, new Vector3(0.168f, 0.325f, 0.050f));



            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            const float cameraSpeed = 1.5f;
            // Escape keyboard
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            // Zoom in
            if (KeyboardState.IsKeyDown(Keys.I))
            {
                _camera.Fov -= 0.05f;
            }
            // Zoom out
            if (KeyboardState.IsKeyDown(Keys.O))
            {
                _camera.Fov += 0.05f;
            }

            // Rotasi X di pivot Camera
            // Lihat ke atas (T)
            if (KeyboardState.IsKeyDown(Keys.T))
            {
                _camera.Pitch += 0.05f;
            }
            // Lihat ke bawah (G)
            if (KeyboardState.IsKeyDown(Keys.G))
            {
                _camera.Pitch -= 0.05f;
            }
            // Rotasi Y di pivot Camera
            // Lihat ke kiri (F)
            if (KeyboardState.IsKeyDown(Keys.F))
            {
                _camera.Yaw -= 0.05f;
            }
            // Lihat ke kanan (H)
            if (KeyboardState.IsKeyDown(Keys.H))
            {
                _camera.Yaw += 0.05f;
            }

            // Maju (W)
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)args.Time;
            }
            // Mundur (S)
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;
            }
            // Kiri (A)
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
            }
            // Kanan (D)
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
            }
            // Naik (Spasi)
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)args.Time;
            }
            // Turun (Ctrl)
            if (KeyboardState.IsKeyDown(Keys.LeftControl))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time;
            }

            const float _rotationSpeed = 0.02f;
            // K (atas -> Rotasi sumbu x)
            if (KeyboardState.IsKeyDown(Keys.K))
            {
                _objectPos *= 2;
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objectPos;
                _camera.Pitch -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objectPos, _rotationSpeed).ExtractRotation());
                _camera.Position += _objectPos;

                _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
                _objectPos /= 2;
            }
            // M (bawah -> Rotasi sumbu x)
            if (KeyboardState.IsKeyDown(Keys.M))
            {
                _objectPos *= 2;
                var axis = new Vector3(1, 0, 0);
                _camera.Position -= _objectPos;
                _camera.Pitch += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objectPos, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objectPos;

                _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
                _objectPos /= 2;
            }

            // N (kiri -> Rotasi sumbu y)
            if (KeyboardState.IsKeyDown(Keys.N))
            {
                _objectPos *= 2;
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objectPos;
                _camera.Yaw += _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objectPos, _rotationSpeed).ExtractRotation());
                _camera.Position += _objectPos;

                _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
                _objectPos /= 2;
            }
            // , (kanan -> Rotasi sumbu y)
            if (KeyboardState.IsKeyDown(Keys.Comma))
            {
                _objectPos *= 2;
                var axis = new Vector3(0, 1, 0);
                _camera.Position -= _objectPos;
                _camera.Yaw -= _rotationSpeed;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objectPos, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objectPos;

                _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
                _objectPos /= 2;
            }

            // J (putar -> Rotasi sumbu z)
            if (KeyboardState.IsKeyDown(Keys.J))
            {
                _objectPos *= 2;
                var axis = new Vector3(0, 0, 1);
                _camera.Position -= _objectPos;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objectPos, _rotationSpeed).ExtractRotation());
                _camera.Position += _objectPos;

                _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
                _objectPos /= 2;
            }
            // L (putar -> Rotasi sumbu z)
            if (KeyboardState.IsKeyDown(Keys.L))
            {
                _objectPos *= 2;
                var axis = new Vector3(0, 0, 1);
                _camera.Position -= _objectPos;
                _camera.Position = Vector3.Transform(_camera.Position,
                    generateArbRotationMatrix(axis, _objectPos, -_rotationSpeed).ExtractRotation());
                _camera.Position += _objectPos;

                _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
                _objectPos /= 2;
            }

            if (!IsFocused)
            {
                return;
            }

            const float sensitivity = 0.2f;
            if (_firstMove)
            {
                _lastMousePosition = new Vector2(MouseState.X, MouseState.Y);
                _firstMove = false;
            }
            else
            {
                // Hitung selisih mouse position
                var deltaX = MouseState.X - _lastMousePosition.X;
                var deltaY = MouseState.Y - _lastMousePosition.Y;
                _lastMousePosition = new Vector2(MouseState.X, MouseState.Y);

                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }

            base.OnUpdateFrame(args);
        }
    }

}
