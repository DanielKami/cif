//***********************************************************************
//                        CIF proggram
//                Created by Daniel M. Kaminski
//                        Lublin 2023
//                     Under GNU licence
//***********************************************************************

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Xamarin.Essentials;


namespace Cif
{
    public class MainProgram1 : Game
    {
        private GraphicsDeviceManager graphicsManager;
        private SpriteBatch spriteBatch;
        BasicEffect basicEffect;
        SpriteFont spriteFont;
        Texture2D texture_front;

        //foto
        RenderTarget2D renderTarget;
        Texture2D shadowMap;
        Palete P1;
        Palete P2;
        //Front page counter


        Model Small_ball;
        Model Big_ball;
        Model Small_rod;
        //Model Plane;
        //Model Big_rod;

        Roler RolX;
        Roler RolY;
        Roler RolZ;



        public static Molecule molecule;


        public MainProgram1()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }


        protected override void Initialize()
        {
            Camera.Initiaqlize(graphicsManager);

            if (molecule == null)
                molecule = new Molecule();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            basicEffect = new BasicEffect(GraphicsDevice);
            Button.spriteBatch = spriteBatch;
            Roler.spriteBatch = spriteBatch;
            Palete.spriteBatch = spriteBatch;

            GraphiscHelper.Ini(GraphicsDevice, basicEffect);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //for photo
            if (renderTarget == null)
                renderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

            Small_ball = Content.Load<Model>("ball_small");
            Big_ball = Content.Load<Model>("ball");
            Small_rod = Content.Load<Model>("rodsmall");
            //Plane = Content.Load<Model>("plane");
            texture_front = Content.Load<Texture2D>("front");

            //Font
            spriteFont = Content.Load<SpriteFont>("SpriteFont1");
            Button.spriteFont = spriteFont;
            Roler.spriteFont = spriteFont;
            Palete.spriteFont = spriteFont;

            MyButtons.LoadContent(Content, GraphicsDevice);
            Palete.LoadContent(Content);

            RolX = new Roler(new Vector2(200, 500));
            RolX.Ini("Rep. x");
            RolY = new Roler(new Vector2(500, 500));
            RolY.Ini("Rep. y");
            RolZ = new Roler(new Vector2(800, 500));
            RolZ.Ini("Rep. z");

            Palete.LoadContent(Content);
            P1 = new Palete(new Vector2(GraphicsDevice.Viewport.Width - 450, GraphicsDevice.Viewport.Height - 820), "Color 1");
            P2 = new Palete(new Vector2(GraphicsDevice.Viewport.Width - 450, GraphicsDevice.Viewport.Height - 400), "Color 2");
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Flags.flag_start)
            {
                var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Flags.remainingDelay -= timer;

                if (Flags.remainingDelay <= 0)
                {
                    Flags.flag_start = false;
                    molecule.InitializeAsync(true);
                }
            }

            Camera.Update(GraphicsDevice.Viewport);
            Camera.ApplyEffects();

            MyButtons.Update(GraphicsDevice);

            //works perfect
            if (Camera.flag_double_tap)//(mCamera.flag_select || mCamera.flag_lasso) & 
            {
                if (MyButtons.button_foto.state)
                    MyButtons.button_foto.state = false;
                else
                {
                    if (Camera.ptMouseDoubleTap.X > 100)//omit menu
                        SelectedAll();
                }
                Camera.flag_double_tap = false;//to clear all selections
            }

            if (MyButtons.button_settings.state)
            {
                RolX.CheckEntry();
                RolY.CheckEntry();
                RolZ.CheckEntry();

                Flags.repeat_x = RolX.Value;
                Flags.repeat_y = RolY.Value;
                Flags.repeat_z = RolZ.Value;

                P1.Check();
                P2.Check();

                Flags.color1 = P1.color;
                Flags.color2 = P2.color;

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Vector2 Position;
            GraphicsDevice.Clear(Color.Black);

            /////////////////
            /////////////////
            Scene();
            /////////////////
            /////////////////

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            #region top text:
            Position = new Vector2(GraphicsDevice.Viewport.Width / 2, 10);
            string Temp_string = "";
            if (Measure.Message == "")
                Temp_string = Cif.FlieName;
            else
                Temp_string = Measure.Message;

            if (Molecule.error_message != "")
                Temp_string = Molecule.error_message;


            if (Flags.flag_start)
            {
                Position = new Vector2(70, 20);
                Temp_string = Flags.version;
            }
            int lenght = Temp_string.Length * 18;
            spriteBatch.DrawString(spriteFont, Temp_string, Position, Color.DeepPink, 0, new Vector2(lenght / 2, 0), 1.2f, SpriteEffects.None, .9f);
            #endregion

            if (MyButtons.button_labels.state)
            {
                float scalecorrect = 3400 / Camera.PerspectiveAngle;
                Matrix AtomMatrix;
                for (int i = 0; i < molecule.NumberOfAtoms; i++)
                {
                    AtomMatrix = Matrix.CreateScale(1, 1, 1);
                    AtomMatrix *= Matrix.CreateTranslation(molecule.atom[i].Coordinate);
                    AtomMatrix *= Camera.Scale * scalecorrect;
                    AtomMatrix *= Camera.Rotate;
                    AtomMatrix *= Camera.View;

                    Temp_string = molecule.atom[i].name;
                    //lenght = Temp_string.Length * 18;
                    spriteBatch.DrawString(spriteFont, Temp_string, new Vector2(GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width / 2 + (1.0f + AtomMatrix.M41), GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height / 2 + (1.0f - AtomMatrix.M42)), Color.Aqua, 0, new Vector2(0, 10), 0.9f, SpriteEffects.None, .9f);
                }

                #region axis
                if (MyButtons.button_cell.state)
                {
                    // 0
                    AtomMatrix = Matrix.CreateScale(1, 1, 1);
                    AtomMatrix *= Matrix.CreateTranslation(Molecule.UnitCellFrame[1].position);
                    AtomMatrix *= Camera.Scale * scalecorrect;
                    AtomMatrix *= Camera.Rotate;
                    AtomMatrix *= Camera.View;
                    spriteBatch.DrawString(spriteFont, "0", new Vector2(GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width / 2 + (1.0f + AtomMatrix.M41), GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height / 2 + (1.0f - AtomMatrix.M42)), Color.White, 0, new Vector2(0, 10), 0.9f, SpriteEffects.None, .9f);
                    //x
                    AtomMatrix = Matrix.CreateScale(1, 1, 1);
                    AtomMatrix *= Matrix.CreateTranslation(Molecule.UnitCellFrame[3].position);
                    AtomMatrix *= Camera.Scale * scalecorrect;
                    AtomMatrix *= Camera.Rotate;
                    AtomMatrix *= Camera.View;
                    spriteBatch.DrawString(spriteFont, "x", new Vector2(GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width / 2 + (1.0f + AtomMatrix.M41), GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height / 2 + (1.0f - AtomMatrix.M42)), Color.Red, 0, new Vector2(0, 10), 0.9f, SpriteEffects.None, .9f);
                    //y
                    AtomMatrix = Matrix.CreateScale(1, 1, 1);
                    AtomMatrix *= Matrix.CreateTranslation(Molecule.UnitCellFrame[6].position);
                    AtomMatrix *= Camera.Scale * scalecorrect;
                    AtomMatrix *= Camera.Rotate;
                    AtomMatrix *= Camera.View;
                    spriteBatch.DrawString(spriteFont, "y", new Vector2(GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width / 2 + (1.0f + AtomMatrix.M41), GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height / 2 + (1.0f - AtomMatrix.M42)), Color.Green, 0, new Vector2(0, 10), 0.9f, SpriteEffects.None, .9f);
                    //z
                    AtomMatrix = Matrix.CreateScale(1, 1, 1);
                    AtomMatrix *= Matrix.CreateTranslation(Molecule.UnitCellFrame[9].position);
                    AtomMatrix *= Camera.Scale * scalecorrect;
                    AtomMatrix *= Camera.Rotate;
                    AtomMatrix *= Camera.View;
                    spriteBatch.DrawString(spriteFont, "z", new Vector2(GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width / 2 + (1.0f + AtomMatrix.M41), GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height / 2 + (1.0f - AtomMatrix.M42)), Color.Blue, 0, new Vector2(0, 10), 0.9f, SpriteEffects.None, .9f);
                }
                #endregion
            }

            if (MyButtons.button_info.state)
            {
                DrawInfo();
            }

            //Draw buttons
            if (!Flags.flag_start & !MyButtons.button_foto.state)
                MyButtons.Draw();

            #region lasso:
            if (Camera.flag_lasso)
            {
                if (Camera.ptMouseSelect.X > 0)
                {
                    //Add first point
                    if (Lasso.VertexList.Count == 0)
                    {
                        Lasso.VertexList.Add(Camera.ptMouseSelect);
                    }

                    //Add rest of points after the first one
                    if (Lasso.VertexList.Count > 0 && Camera.ptMouseSelect != Lasso.VertexList[Lasso.VertexList.Count - 1])
                        Lasso.VertexList.Add(Camera.ptMouseSelect);
                }
                //Select atoms from lasso points, end of selection
                if (Camera.ptMouseSelect.X == -2)
                {
                    Lasso.MouseLasso(molecule, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                }

                // Draw the line.
                GraphiscHelper.MultiLine(Lasso.VertexList, Color.Yellow);
            }
            #endregion

            #region settings
            if (MyButtons.button_settings.state)
            {
                Position.X = 0;
                Position.Y = 120;

                GraphiscHelper.FiledRectangle(Position, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height - 300, 0, Color.FromNonPremultiplied(new Vector4(.1f, .1f, .1f, .9f)), Color.FromNonPremultiplied(new Vector4(.1f, .1f, .1f, .9f)));

                //settings
                RolX.draw();
                RolY.draw();
                RolZ.draw();

                P1.Draw();
                P2.Draw();
            }
            #endregion

            spriteBatch.End();
            base.Draw(gameTime);
        }

        void Scene()
        {
            Matrix[] transforms;
            Matrix AtomMatrix;
            Matrix AtomElipse;

            Matrix ScaleRotate;
            Vector3 VLight;

            Order_Atoms3D(Camera.Rotate * Camera.View * Camera.Projection);

            // GraphicsDevice.BlendState = BlendState.AlphaBlend;

            //background 
            GraphiscHelper.FiledRectangle(new Vector2(0, 0), GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, Flags.color1, Flags.color2);

            #region intro
            if (Flags.flag_start)
            {
                GraphiscHelper.FiledRectangleTexture(new Vector2(0, 0), GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 1.0f * Flags.remainingDelay / Flags.intro_duration, texture_front);
            }


            #endregion
            VLight = new Vector3(0, 0, 0.5f);
            VLight = Vector3.Normalize(VLight);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScaleRotate = Matrix.CreateScale(Camera.OneOverZoom, Camera.OneOverZoom, Camera.OneOverZoom) * Camera.Rotate;



            for (int i = 0; i < molecule.NumberOfAtoms; i++)
            {
                int Ordered_z = molecule.atom[i].Z_Depth;

                #region CPK
                if (molecule.atom[Ordered_z].style == 0)
                {
                    float Size = molecule.AtomRadius[molecule.atom[Ordered_z].AtomicNumber, Flags.AtomicRadiiType];
                    Size /= 6;
                    AtomMatrix = Matrix.CreateScale(Size, Size, Size);
                    AtomMatrix *= Matrix.CreateTranslation(molecule.atom[Ordered_z].Coordinate);
                    AtomMatrix *= ScaleRotate;


                    Model tm;
                    // if (Camera.flag_end_rotation)
                    tm = Big_ball;
                    //else
                    //    tm = Small_ball;
                    transforms = new Matrix[tm.Bones.Count];
                    tm.CopyAbsoluteBoneTransformsTo(transforms);
                    foreach (ModelMesh mesh in tm.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {

                            effect.PreferPerPixelLighting = false;
                            effect.LightingEnabled = true;
                            if (molecule.atom[Ordered_z].selected)
                                effect.AmbientLightColor = new Vector3(0.9f, 0.9f, 0.1f);
                            else
                                effect.AmbientLightColor = molecule.color[molecule.atom[Ordered_z].AtomicNumber];
                            effect.SpecularColor = new Vector3(0.9f, 0.9f, 0.9f);
                            effect.SpecularPower = 10;
                            effect.Alpha = molecule.atom[Ordered_z].occupancy;
                            effect.DirectionalLight0.Direction = VLight;

                            if (molecule.atom[Ordered_z].selected)
                                effect.DirectionalLight0.DiffuseColor = new Vector3(0.9f, 0.9f, 0.1f);
                            else
                                effect.DirectionalLight0.DiffuseColor = molecule.color[molecule.atom[Ordered_z].AtomicNumber];

                            effect.DirectionalLight0.SpecularColor = new Vector3(.9f, .9f, .9f);

                            effect.View = Camera.View;
                            effect.Projection = Camera.Projection;
                            effect.World = transforms[mesh.ParentBone.Index] * AtomMatrix;
                        }
                        mesh.Draw();
                    }
                }

                #endregion

                #region Stisks
                if (molecule.atom[Ordered_z].style == 1 || molecule.atom[Ordered_z].style == 2 || molecule.atom[Ordered_z].style == 3)
                {
                    for (int j = 0; j < molecule.atom[Ordered_z].NrBondedAtoms; ++j)
                    {
                        AtomMatrix = Matrix.CreateScale(.0127f * molecule.atom[Ordered_z].Distance[j], .1f, .1f);
                        AtomMatrix *= Matrix.CreateRotationY(molecule.atom[Ordered_z].AngleZ[j]);
                        AtomMatrix *= Matrix.CreateRotationZ(molecule.atom[Ordered_z].AngleXY[j]);
                        AtomMatrix *= Matrix.CreateTranslation(molecule.atom[Ordered_z].Coordinate);
                        AtomMatrix *= ScaleRotate;

                        transforms = new Matrix[Small_rod.Bones.Count];
                        Small_rod.CopyAbsoluteBoneTransformsTo(transforms);
                        foreach (ModelMesh mesh in Small_rod.Meshes)
                        {
                            foreach (BasicEffect effect in mesh.Effects)
                            {
                                effect.PreferPerPixelLighting = true;
                                effect.LightingEnabled = true;

                                if (molecule.atom[Ordered_z].selected)
                                    effect.DirectionalLight0.DiffuseColor = effect.AmbientLightColor = new Vector3(0.9f, 0.9f, 0.1f);
                                else
                                    effect.DirectionalLight0.DiffuseColor = effect.AmbientLightColor = molecule.color[molecule.atom[Ordered_z].AtomicNumber];
                                effect.SpecularColor = new Vector3(0.9f, 0.9f, 0.9f);
                                effect.DirectionalLight0.Direction = VLight;
                                effect.DirectionalLight0.SpecularColor = new Vector3(.9f, .9f, .9f);
                                effect.SpecularPower = 10;
                                effect.Alpha = molecule.atom[Ordered_z].BondOccupancy[j];
                                effect.View = Camera.View;
                                effect.Projection = Camera.Projection;
                                effect.World = transforms[mesh.ParentBone.Index] * AtomMatrix;
                            }
                            mesh.Draw();
                        }
                    }
                }

                #endregion

                #region balls

                if (molecule.atom[Ordered_z].style == 2)
                {
                    float Size = .05f;//Define the size
                    AtomMatrix = Matrix.CreateScale(Size, Size, Size);
                    AtomMatrix *= Matrix.CreateTranslation(molecule.atom[Ordered_z].Coordinate);
                    AtomMatrix *= ScaleRotate;

                    Model tm;
                    if (Camera.flag_end_rotation)
                        tm = Big_ball;
                    else
                        tm = Small_ball;
                    transforms = new Matrix[tm.Bones.Count];
                    tm.CopyAbsoluteBoneTransformsTo(transforms);
                    foreach (ModelMesh mesh in tm.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.PreferPerPixelLighting = true;
                            effect.LightingEnabled = true;

                            if (molecule.atom[Ordered_z].selected)
                                effect.AmbientLightColor = new Vector3(0.9f, 0.9f, 0.1f);
                            else
                                effect.AmbientLightColor = molecule.color[molecule.atom[Ordered_z].AtomicNumber];
                            effect.SpecularColor = new Vector3(0.9f, 0.9f, 0.9f);
                            effect.SpecularPower = 10;
                            effect.Alpha = molecule.atom[Ordered_z].occupancy;
                            effect.DirectionalLight0.Direction = VLight;

                            if (molecule.atom[Ordered_z].selected)
                                effect.DirectionalLight0.DiffuseColor = new Vector3(0.9f, 0.9f, 0.1f);
                            else
                                effect.DirectionalLight0.DiffuseColor = molecule.color[molecule.atom[Ordered_z].AtomicNumber];
                            effect.DirectionalLight0.SpecularColor = new Vector3(.9f, .9f, .9f);

                            effect.View = Camera.View;
                            effect.Projection = Camera.Projection;
                            effect.World = transforms[mesh.ParentBone.Index] * AtomMatrix;
                        }
                        mesh.Draw();
                    }
                }
                #endregion

                #region ellipsoids
                if (molecule.atom[Ordered_z].style == 3)
                {
                    Model tm;
                    if (Camera.flag_end_rotation)
                        tm = Big_ball;
                    else
                        tm = Small_ball;

                    float Size = 1f;

                    Matrix ScaleRotateTranslation = Matrix.CreateTranslation(molecule.atom[Ordered_z].Coordinate) * ScaleRotate;

                    //Hydrogen size here 
                    if (molecule.atom[Ordered_z].AtomicNumber == 1)
                    {
                        Size /= 20;
                        AtomMatrix = Matrix.CreateScale(Size, Size, Size);
                    }
                    else
                    {
                        AtomMatrix = Matrix.CreateScale(Size, Size, Size);
                        AtomMatrix *= molecule.atom[Ordered_z].Transformation;
                        AtomMatrix *= molecule.atom[Ordered_z].Un; //thermal elipsoid
                    }
                    AtomMatrix *= ScaleRotateTranslation;

                    transforms = new Matrix[tm.Bones.Count];
                    tm.CopyAbsoluteBoneTransformsTo(transforms);

                    if (molecule.atom[Ordered_z].AtomicNumber != 1)
                    {
                        float size_elips = Size + 0.05f;
                        float size_low = 0.15f;

                        //Elipsoid lines
                        for (int elips = 0; elips < 3; elips++)
                        {
                            Matrix AllTransforms;
                            AllTransforms = molecule.atom[Ordered_z].Transformation * molecule.atom[Ordered_z].Un * ScaleRotateTranslation;

                            switch (elips)
                            {
                                case 0:
                                    AtomElipse = Matrix.CreateScale(size_low, size_elips, size_elips);
                                    break;
                                case 1:
                                    AtomElipse = Matrix.CreateScale(size_elips, size_low, size_elips);
                                    break;
                                default:
                                    AtomElipse = Matrix.CreateScale(size_elips, size_elips, size_low);
                                    break;
                            }

                            //Multiply by all matrices
                            AtomElipse *= AllTransforms;
                            foreach (ModelMesh mesh in tm.Meshes)
                            {
                                foreach (BasicEffect effect in mesh.Effects)
                                {
                                    effect.PreferPerPixelLighting = false;
                                    effect.LightingEnabled = true;
                                    effect.DirectionalLight0.DiffuseColor = effect.AmbientLightColor = new Vector3(0.0f, 0.0f, 0.0f);
                                    effect.SpecularColor = new Vector3(0.2f, 0.2f, 0.2f);
                                    effect.SpecularPower = 10;
                                    effect.Alpha = molecule.atom[Ordered_z].occupancy;
                                    effect.DirectionalLight0.Direction = VLight; //molecule.color[molecule.atom[Ordered_z].AtomicNumber];
                                    effect.DirectionalLight0.SpecularColor = new Vector3(.1f, .1f, .1f);
                                    effect.View = Camera.View;
                                    effect.Projection = Camera.Projection;
                                    effect.World = transforms[mesh.ParentBone.Index] * AtomElipse;
                                }
                                mesh.Draw();
                            }
                        }
                    }

                    foreach (ModelMesh mesh in tm.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.PreferPerPixelLighting = true;
                            effect.LightingEnabled = true;

                            if (molecule.atom[Ordered_z].selected)
                                effect.DirectionalLight0.DiffuseColor = effect.AmbientLightColor = new Vector3(0.9f, 0.9f, 0.1f);
                            else
                                effect.DirectionalLight0.DiffuseColor = effect.AmbientLightColor = molecule.color[molecule.atom[Ordered_z].AtomicNumber];
                            effect.DirectionalLight0.SpecularColor = effect.SpecularColor = new Vector3(0.9f, 0.9f, 0.9f);
                            effect.SpecularPower = 10;
                            effect.Alpha = molecule.atom[Ordered_z].occupancy;
                            effect.DirectionalLight0.Direction = VLight;
                            effect.View = Camera.View;
                            effect.Projection = Camera.Projection;
                            effect.World = transforms[mesh.ParentBone.Index] * AtomMatrix;
                        }
                        mesh.Draw();
                    }
                }
                #endregion

                #region H-bonds
                if (MyButtons.button_hydrogen.state)
                {
                    for (int j = 0; j < molecule.atom[Ordered_z].H_NrBondedAtoms; ++j)
                    {
                        AtomMatrix = Matrix.CreateScale(.0127f * molecule.atom[Ordered_z].H_Distance[j], .05f, .05f);
                        AtomMatrix *= Matrix.CreateRotationY(molecule.atom[Ordered_z].H_AngleZ[j]);
                        AtomMatrix *= Matrix.CreateRotationZ(molecule.atom[Ordered_z].H_AngleXY[j]);
                        AtomMatrix *= Matrix.CreateTranslation(molecule.atom[Ordered_z].Coordinate);
                        AtomMatrix *= ScaleRotate;

                        transforms = new Matrix[Small_rod.Bones.Count];
                        Small_rod.CopyAbsoluteBoneTransformsTo(transforms);
                        foreach (ModelMesh mesh in Small_rod.Meshes)
                        {
                            foreach (BasicEffect effect in mesh.Effects)
                            {
                                effect.PreferPerPixelLighting = true;
                                effect.LightingEnabled = true;

                                //hydrogen bond color
                                effect.DirectionalLight0.DiffuseColor = effect.AmbientLightColor = new Vector3(0.0f, 0.9f, 0.0f);
                                effect.SpecularColor = new Vector3(0.0f, 0.0f, 0.0f);
                                effect.DirectionalLight0.Direction = VLight;
                                effect.DirectionalLight0.SpecularColor = new Vector3(.0f, .99f, .3f);
                                effect.SpecularPower = 10;
                                effect.Alpha = molecule.atom[Ordered_z].occupancy;
                                effect.View = Camera.View;
                                effect.Projection = Camera.Projection;
                                effect.World = transforms[mesh.ParentBone.Index] * AtomMatrix;
                            }
                            mesh.Draw();
                        }
                    }
                }

                #endregion
            }

            if (MyButtons.button_polihedral.state)
                DrawPolihedrals(ScaleRotate, VLight);

            if (MyButtons.button_cell.state)
                DrawCell(ScaleRotate);
        }

        void DrawPolihedrals(Matrix ScaleRotate, Vector3 VLight)
        {
            if (PolihedralsClass.VertexesList == null)
                return;

            // Polihedrals
            foreach (PolihedralsClass.VertexStruct vs in PolihedralsClass.VertexesList)//iterate all polihedrals
            {
                //Position of the first vertex in space, the rest is from matrix
                Matrix AtomMatrix = Matrix.Identity;
                AtomMatrix *= Matrix.CreateTranslation(vs.position);
                AtomMatrix *= ScaleRotate;

                GraphiscHelper.FiledTriangle(vs.vertices_n, vs.color, AtomMatrix, VLight);
                GraphiscHelper.FiledTriangle(vs.vertices_s, vs.color, AtomMatrix, VLight);
            }
        }

        void DrawCell(Matrix ScaleRotate)
        {
            Matrix CellMatrix;
            for (int j = 0; j < Molecule.UnitCellFrame.Count; ++j)
            {
                CellMatrix = Matrix.CreateScale(.0127f * 2 * Molecule.UnitCellFrame[j].distance, .05f, .05f);
                CellMatrix *= Matrix.CreateRotationY(Molecule.UnitCellFrame[j].AngleZ);
                CellMatrix *= Matrix.CreateRotationZ(Molecule.UnitCellFrame[j].AngleXY);
                CellMatrix *= Matrix.CreateTranslation(Molecule.UnitCellFrame[j].position);
                CellMatrix *= ScaleRotate;

                Matrix[] transforms = new Matrix[Small_rod.Bones.Count];
                Small_rod.CopyAbsoluteBoneTransformsTo(transforms);
                foreach (ModelMesh mesh in Small_rod.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.PreferPerPixelLighting = false;
                        effect.LightingEnabled = true;

                        effect.AmbientLightColor = Molecule.UnitCellFrame[j].color;
                        effect.SpecularColor = new Vector3(0.9f, 0.9f, 0.9f);
                        effect.SpecularPower = 10;
                        // effect.DirectionalLight0.Direction = VLight;

                        effect.DirectionalLight0.DiffuseColor = effect.AmbientLightColor;

                        effect.DirectionalLight0.SpecularColor = new Vector3(.9f, .9f, .9f);
                        effect.SpecularPower = 10;
                        effect.Alpha = 1;
                        effect.View = Camera.View;
                        effect.Projection = Camera.Projection;
                        effect.World = transforms[mesh.ParentBone.Index] * CellMatrix;
                    }
                    mesh.Draw();
                }
            }

        }

        void Order_Atoms3D(Matrix RotateViewnProjMatrix)
        {
            Vector3 v_real;
            float last_z;
            int iterate = 0;

            bool[] atomCheck = new bool[molecule.NumberOfAtoms];

            for (int m = 0; m < molecule.NumberOfAtoms; ++m)
            {
                last_z = -10000.0f;
                for (int n = 0; n < molecule.NumberOfAtoms; ++n)
                {
                    //MainPage.molecule.atom[n].Coordinate.X
                    v_real = Vector3.Transform(molecule.atom[n].Coordinate, RotateViewnProjMatrix);

                    if (atomCheck[n] == false && v_real.Z >= last_z)
                    {
                        last_z = v_real.Z;
                        iterate = n;
                    }
                }
                molecule.atom[m].Z_Depth = iterate;
                if (iterate < molecule.NumberOfAtoms)
                    atomCheck[iterate] = true;
            }
        }

        public static void CheckIfSelected(int style)
        {
            //Check if eny selected
            int selected = 0;
            for (int i = 0; i < molecule.NumberOfAtoms; i++)
                if (molecule.atom[i].selected)
                    selected++;


            if (selected > 0)
            {
                for (int i = 0; i < molecule.NumberOfAtoms; i++)
                    if (molecule.atom[i].selected)
                        molecule.atom[i].style = style;
            }
            else
                  if (!MyButtons.button_lasso.state)
                for (int i = 0; i < molecule.NumberOfAtoms; i++)
                    molecule.atom[i].style = style;
        }

        void SelectedAll()
        {

            if (molecule.NumberSelected == 0)
            {
                for (int i = 0; i < molecule.NumberOfAtoms; i++)
                    molecule.atom[i].selected = true;
                molecule.NumberSelected = molecule.NumberOfAtoms;
            }
            else
            {
                for (int i = 0; i < molecule.NumberOfAtoms; i++)
                {
                    molecule.atom[i].selected = false;
                    molecule.atom[i].SelectionOrder = 0;
                }
                molecule.NumberSelected = 0;
            }
        }

        public void Photo()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                GraphicsDevice.SetRenderTarget(renderTarget);
                Scene();
                //copy renderTarget to shadowMap1
                GraphicsDevice.SetRenderTarget(null);
                shadowMap = (Texture2D)renderTarget;

                shadowMap.SaveAsJpeg(stream, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                stream.Seek(0, SeekOrigin.Begin);

                string filename = "photo_"
                     + DateTime.Now.ToString("yyyy-MM-dd_hh:mm:ss")
                     + ".jpg";


                var newFile = Path.Combine(FileSystem.AppDataDirectory, filename);// in CacheDirectory, you could try to save in other folders  
                using (var newStream = File.OpenWrite(newFile))
                    stream.CopyToAsync(newStream);//not working


                shadowMap = null;

                MyButtons.button_foto.state = false;
                Camera.flag_rotate = true;
            }


        }


        void DrawInfo()
        {

            string Temp_string = "";
            Vector2 Position;
            Position.X = 130;
            Position.Y = 120;
            float lenght;
            float font_size = 1;

            GraphiscHelper.FiledRectangle(Position, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height - 300, 0, Color.FromNonPremultiplied(new Vector4(.1f, .1f, .1f, .9f)), Color.FromNonPremultiplied(new Vector4(.1f, .1f, .1f, .9f)));

            Vector2 TextPos = new Vector2(160, 130);

            Temp_string = "Name: " + Cif.chemical_name_common.Replace("   ", "").Replace("  ", "").Replace(" ", "");
            lenght = 1;// Temp_string.Length * 18;
            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.LightBlue, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);

            TextPos.Y += 70;
            if (Cif.space_group_IT_number <= 0)
                Temp_string = "System: ?";
            else
            {
                Temp_string = "System: " + SymmetryElements.SymmetryOperations[Cif.space_group_IT_number][1];
                spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.White, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);
                TextPos.Y += 70;

                Temp_string = "Space group: " + SymmetryElements.SymmetryOperations[Cif.space_group_IT_number][2];
                spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.White, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);
                TextPos.Y += 70;
            }
            Temp_string = "a= " + Cif.a + "(" + Measure.RErr(Cif.a, Cif.a_e) + ")" + "  b= " + Cif.b + "(" + Measure.RErr(Cif.b, Cif.b_e) + ")" + " c= " + Cif.c + "(" + Measure.RErr(Cif.c, Cif.c_e) + ")";
            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.White, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);

            TextPos.Y += 70;
            Temp_string = "alp= " + Cif.alp + "(" + Measure.RErr(Cif.alp, Cif.alp_e) + ")" + " bet= " + Cif.bet + "(" + Measure.RErr(Cif.bet, Cif.bet_e) + ")" + " gam= " + Cif.gam + "(" + Measure.RErr(Cif.bet, Cif.bet_e) + ")";
            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.White, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);

            TextPos.Y += 70;
            Temp_string = "V: " + Cif.cell_volume + "(" + Measure.RErr(Cif.cell_volume, Cif.cell_volume_e) + ")";
            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.LightCoral, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);




            TextPos.Y += 70;
            if (Cif.formula_units_Z <= 0)
                Temp_string = "Z: ?";
            else
                Temp_string = "Z: " + Cif.formula_units_Z;
            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.Green, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);

            TextPos.Y += 70;
            if (Cif.formula_sum == "")
                Temp_string = "Formula: ?";
            else
                Temp_string = "Formula: " + Cif.formula_sum.Replace("   ", "").Replace("  ", "").Replace(" ", "");
            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.Yellow, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);

            TextPos.Y += 70;
            if (Cif.formula_weight <= 0)
                Temp_string = "Weight: ?";
            Temp_string = "Weight: " + Cif.formula_weight;
            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.LightCoral, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);

            TextPos.Y += 70;
            if (Cif.exptl_crystal_density_diffrn >= 0)
            {
                Temp_string = "Density [g/cm3]: " + Cif.exptl_crystal_density_diffrn;
            }
            else
                Temp_string = "Density [g/cm3]: ?";

            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.LightCoral, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);



            TextPos.Y += 70;
            if (Cif.measurement_temperature <= 0)
                Temp_string = "Measurement temp.: ?";
            else
                Temp_string = "Measurement temp.: " + Cif.measurement_temperature + " K";
            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.Red, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);


            TextPos.Y += 70;
            if (Cif.exptl_absorpt_coefficient_mu >= 0)
            {
                Temp_string = "Absorpt coef. mu: " + Cif.exptl_absorpt_coefficient_mu;
            }
            else
                Temp_string = "Absorpt coef. mu: ?";

            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.Yellow, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);

            TextPos.Y += 70;
            if (Cif.exptl_absorpt_correction_type != "")
            {
                Temp_string = "Absorpt type: " + Cif.exptl_absorpt_correction_type;
            }
            else
                Temp_string = "Absorpt type: ?";

            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.Yellow, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);

            TextPos.Y += 70;
            if (Cif.cell_measurement_reflns_used >= 0)
            {
                Temp_string = "Reflections used: " + Cif.cell_measurement_reflns_used;
            }
            else
                Temp_string = "Reflections used: ?";

            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.Yellow, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);

            TextPos.Y += 70;
            if (Cif.s_R_factor_gt <= 0)
            {
                Temp_string = "R1: " + Cif.s_R_factor_gt * 100 + "%";
            }
            else
                Temp_string = "R1: ?";
            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.Yellow, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);


            TextPos.Y += 70;
            if (Cif.chemical_oxdiff_usercomment != "")
            {
                Temp_string = "User comments: " + Cif.chemical_oxdiff_usercomment;
            }
            else
                Temp_string = "User comments: ?";

            spriteBatch.DrawString(spriteFont, Temp_string, TextPos, Color.LightSkyBlue, 0, new Vector2(lenght / 2, 0), font_size, SpriteEffects.None, .9f);

        }
    }
}