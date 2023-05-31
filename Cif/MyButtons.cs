//***********************************************************************
//                        CIF program
//                Created by Daniel M. Kaminski
//                        Lublin 2023
//                     Under GNU licence
//***********************************************************************

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cif
{
    public static class MyButtons
    {
        public static Button button_download;
        public static Button button_polihedral;

        public static Button button_rotate;
        //  public static Button button_shift;
        public static Button button_select;
        public static Button button_lasso;
        public static Button button_settings;
        public static Button button_foto;
        public static Button button_labels;
        public static Button button_view;
        public static Button button_perspective;

        public static Button button_cpk;
        public static Button button_sticks;
        public static Button button_ballsticks;
        public static Button button_elipsoids;

        public static Button button_measure_distance;
        public static Button button_measure_angle;
        public static Button button_measure_dichedral;
        public static Button button_delete;
        public static Button button_cell;
        public static Button button_move_to_cell;
        public static Button button_info;
        public static Button button_hydrogen;

        #region Textures define
        static Texture2D textureRotate;
        //static Texture2D textureShift;
        //static Texture2D textureShift_n;
        static Texture2D textureSelect;
        static Texture2D textureLasso;
        static Texture2D textureRotate_n;
        static Texture2D textureSelect_n;
        static Texture2D textureLasso_n;

        static Texture2D textureCPK;
        static Texture2D textureStcks;
        static Texture2D textureBallSticks;
        static Texture2D textureEllipsoids;
        static Texture2D textureCPK_n;
        static Texture2D textureStcks_n;
        static Texture2D textureBallSticks_n;
        static Texture2D textureEllipsoids_n;

        static Texture2D textureDistance;
        static Texture2D textureAngle;
        static Texture2D textureDichedral;
        static Texture2D textureDelete;
        static Texture2D textureDistance_n;
        static Texture2D textureAngle_n;
        static Texture2D textureDichedral_n;
        static Texture2D textureDelete_n;

        static Texture2D textureSettings;
        static Texture2D textureSettings_n;

        static Texture2D texture_foto;
        static Texture2D texture_foto_n;

        static Texture2D texture_download;
        static Texture2D texture_download_n;

        static Texture2D texture_polihedral;
        static Texture2D texture_polihedral_n;

        static Texture2D texture_text;
        static Texture2D texture_text_n;

        static Texture2D texture_cell;
        static Texture2D texture_cell_n;

        static Texture2D texture_move_to_the_cell;
        static Texture2D texture_move_to_the_cell_n;

        static Texture2D texture_info;
        static Texture2D texture_info_n;

        static Texture2D texture_view;
        static Texture2D texture_view_n;

        static Texture2D texture_perspective;
        static Texture2D texture_perspective_n;

        static Texture2D texture_hydrogen;
        static Texture2D texture_hydrogen_n;
        #endregion;


        public static void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            #region  buttons create
            button_rotate = new Button();
            // button_shift = new Button();
            button_select = new Button();
            button_lasso = new Button();
            button_settings = new Button();

            button_cpk = new Button();
            button_sticks = new Button();
            button_ballsticks = new Button();
            button_elipsoids = new Button();

            button_measure_distance = new Button();
            button_measure_angle = new Button();
            button_measure_dichedral = new Button();
            button_delete = new Button();

            button_foto = new Button();
            button_download = new Button();
            button_polihedral = new Button();
            button_labels = new Button();
            button_cell = new Button();
            button_move_to_cell = new Button();
            button_info = new Button();
            button_view = new Button();
            button_perspective = new Button();
            button_hydrogen = new Button();
            #endregion

            #region Button read texture
            textureRotate = Content.Load<Texture2D>("buttonrotate");
            //textureShift = Content.Load<Texture2D>("button_shift");
            //textureShift_n = Content.Load<Texture2D>("button_shift_n");
            textureSelect = Content.Load<Texture2D>("button_select");
            textureLasso = Content.Load<Texture2D>("button_lasso");
            textureRotate_n = Content.Load<Texture2D>("buttonrotate_n");

            textureSelect_n = Content.Load<Texture2D>("button_select_n");
            textureLasso_n = Content.Load<Texture2D>("button_lasso_n");
            textureSettings = Content.Load<Texture2D>("button_settings");
            textureSettings_n = Content.Load<Texture2D>("button_settings_n");
            textureCPK = Content.Load<Texture2D>("button_cpk");
            textureStcks = Content.Load<Texture2D>("button_sticks");
            textureBallSticks = Content.Load<Texture2D>("button_ballsticks");
            textureEllipsoids = Content.Load<Texture2D>("buttonellipsoids");
            textureCPK_n = Content.Load<Texture2D>("button_cpk_n");
            textureStcks_n = Content.Load<Texture2D>("button_sticks_n");
            textureBallSticks_n = Content.Load<Texture2D>("button_ballsticks_n");
            textureEllipsoids_n = Content.Load<Texture2D>("buttonellipsoids_n");
            textureDistance = Content.Load<Texture2D>("button_distance_n");
            textureAngle = Content.Load<Texture2D>("button_angle");
            textureDichedral = Content.Load<Texture2D>("button_dichedral");
            textureDelete = Content.Load<Texture2D>("button_delete");
            textureDistance_n = Content.Load<Texture2D>("button_distance_n");
            textureAngle_n = Content.Load<Texture2D>("button_angle_n");
            textureDichedral_n = Content.Load<Texture2D>("button_dichedral_n");
            textureDelete_n = Content.Load<Texture2D>("button_delete_n");
            texture_foto = Content.Load<Texture2D>("button_foto");
            texture_foto_n = Content.Load<Texture2D>("button_foto_n");
            texture_download = Content.Load<Texture2D>("button_open");
            texture_download_n = Content.Load<Texture2D>("button_open_n");
            texture_polihedral = Content.Load<Texture2D>("button_polihedrons");
            texture_polihedral_n = Content.Load<Texture2D>("button_polihedrons_n");
            texture_text = Content.Load<Texture2D>("button_text");
            texture_text_n = Content.Load<Texture2D>("button_text_n");
            texture_cell = Content.Load<Texture2D>("button_cell");
            texture_cell_n = Content.Load<Texture2D>("button_cell_n");
            texture_move_to_the_cell = Content.Load<Texture2D>("button_move_to_cell");
            texture_move_to_the_cell_n = Content.Load<Texture2D>("button_move_to_cell_n");
            texture_info = Content.Load<Texture2D>("button_info");
            texture_info_n = Content.Load<Texture2D>("button_info_n");
            texture_view = Content.Load<Texture2D>("button_view");
            texture_view_n = Content.Load<Texture2D>("button_view_n");
            texture_perspective = Content.Load<Texture2D>("button_perspective");
            texture_perspective_n = Content.Load<Texture2D>("button_perspective_n");
            texture_hydrogen = Content.Load<Texture2D>("button_hydrogen");
            texture_hydrogen_n = Content.Load<Texture2D>("button_hydrogen_n");
            #endregion

            #region button define
            //bottom
            Vector2 Position = new Vector2();
            Position.X = 200;
            Position.Y = GraphicsDevice.Viewport.Height - 70;
            button_rotate.InitializeButton(Position, "rotate", textureRotate, textureRotate_n);
            //Position.X += 140;
            //button_shift.InitializeButton(Position, "shift", textureShift, textureShift_n);
            Position.X += 140;
            button_select.InitializeButton(Position, "select", textureSelect, textureSelect_n);
            Position.X += 140;
            button_lasso.InitializeButton(Position, "lasso", textureLasso, textureLasso_n);
            Position.X += 140;
            button_settings.InitializeButton(Position, "settings", textureSettings, textureSettings_n);
            Position.X += 140;
            button_foto.InitializeButton(Position, "foto", texture_foto, texture_foto_n);

            Position.X = 50;
            Position.Y = 150;
            button_download.InitializeButton(Position, "download", texture_download, texture_download_n);
            Position.Y += 140;
            button_cell.InitializeButton(Position, "cell", texture_cell, texture_cell_n);
            Position.Y += 140;
            button_move_to_cell.InitializeButton(Position, "move atoms to cell", texture_move_to_the_cell, texture_move_to_the_cell_n);
            Position.Y += 140;
            button_labels.InitializeButton(Position, "sym. grow", texture_text, texture_text_n);
            Position.Y += 140;
            button_info.InitializeButton(Position, "info", texture_info, texture_info_n);
            Position.Y += 140;
            button_view.InitializeButton(Position, "view", texture_view, texture_view_n);
            Position.Y += 140;
            button_perspective.InitializeButton(Position, "perspective", texture_perspective, texture_perspective_n);


            //buttons style
            Position.X = GraphicsDevice.Viewport.Width - 60;
            Position.Y = 150;
            button_cpk.InitializeButton(Position, "CPK", textureCPK, textureCPK_n);
            Position.Y += 140;
            button_sticks.InitializeButton(Position, "sticks", textureStcks, textureStcks_n);
            Position.Y += 140;
            button_ballsticks.InitializeButton(Position, "ball-sticks", textureBallSticks, textureBallSticks_n);
            Position.Y += 140;
            button_elipsoids.InitializeButton(Position, "ellipsoids", textureEllipsoids, textureEllipsoids_n);
            Position.Y += 140;
            button_polihedral.InitializeButton(Position, "polihedral", texture_polihedral, texture_polihedral_n);
            Position.Y += 140;
            button_delete.InitializeButton(Position, "delete", textureDelete, textureDelete_n);
            Position.Y += 140;
            button_hydrogen.InitializeButton(Position, "hydrogen", texture_hydrogen, texture_hydrogen_n);

            //buttons measurement
            Position.X = GraphicsDevice.Viewport.Width - 200;
            Position.Y = 150;
            button_measure_distance.InitializeButton(Position, "disance", textureDistance, textureDistance_n);
            Position.Y += 140;
            button_measure_angle.InitializeButton(Position, "angle", textureAngle, textureAngle_n);
            Position.Y += 140;
            button_measure_dichedral.InitializeButton(Position, "dichedral", textureDichedral, textureDichedral_n);
            #endregion

            button_rotate.state = true;
            // button_shift.state = false;
            button_select.state = false;
            button_lasso.state = false;
            button_settings.state = false;
            button_polihedral.state = false;
            button_ballsticks.state = true;

            Camera.flag_rotate = true;
        }


        public static void Update(GraphicsDevice GraphicsDevice)
        {
            bool click;
            click = button_download.CheckButton();
            if (button_download.state)
            {
                Flags.Reset();
                button_ballsticks.state = true;
                button_cpk.state = false;
                button_sticks.state = false;
                button_elipsoids.state = false;

                Camera.flag_rotate = true;
                Camera.flag_translate = false;
                Camera.flag_lasso = false;
                Camera.flag_select = false;

                button_rotate.state = true;
                // button_shift.state = false;
                button_select.state = false;
                button_lasso.state = false;
                button_settings.state = false;

                button_download.state = false;
                Measure.Message = "";

                button_move_to_cell.state = false;

                PolihedralsClass.VertexesList.Clear();
                PolihedralsClass.Polihedrals.Clear();
                MainProgram1.molecule = new Molecule();//filling is assync so must be empty ealier

                if (button_polihedral.state)
                    PolihedralsClass.FindPolihedral();

                if (!Flags.flag_start)
                    MainProgram1.molecule.InitializeAsync();
            }
            #region operations
            click = button_rotate.CheckButton();
            if (button_rotate.state)
            {
                if (click)
                {
                    Camera.flag_rotate = true;
                    Camera.flag_translate = false;
                    Camera.flag_lasso = false;
                    Camera.flag_select = false;

                    //button_shift.state = false;
                    button_select.state = false;
                    button_lasso.state = false;
                    button_settings.state = false;

                    Measure.Message = "";
                    CheckRepetitions();
                }
            }

            //click = button_shift.CheckButton();
            //if (button_shift.state)
            //{
            //    if (click)
            //    {
            //        Camera.flag_rotate = false;
            //        Camera.flag_translate = true;
            //        Camera.flag_lasso = false;
            //        Camera.flag_select = false;

            //        button_rotate.state = false;
            //        button_select.state = false;
            //        button_lasso.state = false;
            //        button_settings.state = false;
            //        button_polihedral.state = false;

            //        Measure.Message = "";
            //        CheckRepetitions();
            //    }
            //}

            click = button_select.CheckButton();
            if (button_select.state)
            {
                if (click)
                {
                    Camera.flag_rotate = false;
                    Camera.flag_translate = false;
                    Camera.flag_lasso = false;
                    Camera.flag_select = true;

                    button_rotate.state = false;
                    //      button_shift.state = false;
                    button_lasso.state = false;
                    button_settings.state = false;
                    button_polihedral.state = false;
                    CheckRepetitions();
                }

                if (Camera.ptMouseSelect.Y < GraphicsDevice.Viewport.Height - Camera.MarginY)
                {
                    if (Camera.flag_selected)
                    {
                        Measure.MouseSelect(MainProgram1.molecule, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Camera.Scale * Camera.Rotate * Camera.View * Camera.Projection, Camera.ptMouseSelect);
                        Camera.ptMouseSelect.Y = 0;
                        Camera.ptMouseSelect.X = 0;
                        Camera.flag_selected = false;
                    }
                }
                #region measure
                click = button_measure_distance.CheckButton2();
                if (click & button_measure_distance.state)
                {
                    button_measure_distance.state = false;
                    Measure.MeasureDistance(MainProgram1.molecule);
                }

                click = button_measure_angle.CheckButton2();
                if (click & button_measure_angle.state)
                {
                    button_measure_angle.state = false;
                    Measure.MeasureAngle(MainProgram1.molecule);
                }

                click = button_measure_dichedral.CheckButton2();
                if (click & button_measure_dichedral.state)
                {
                    button_measure_dichedral.state = false;
                    Measure.MeasureTorsian(MainProgram1.molecule);
                }
                #endregion
            }

            click = button_lasso.CheckButton();
            if (button_lasso.state)
            {
                if (click)
                {
                    Camera.flag_rotate = false;
                    Camera.flag_translate = false;
                    Camera.flag_lasso = true;
                    Camera.flag_select = false;

                    button_rotate.state = false;
                    //     button_shift.state = false;
                    button_select.state = false;
                    button_settings.state = false;
                    button_polihedral.state = false;
                    CheckRepetitions();
                }
            }
            #endregion

            #region style
            click = button_cpk.CheckButton();
            if (button_cpk.state)
            {
                if (click)
                {
                    button_cpk.state = true;
                    button_sticks.state = false;
                    button_ballsticks.state = false;
                    button_elipsoids.state = false;
                    MainProgram1.CheckIfSelected(0);
                }
            }

            click = button_sticks.CheckButton();
            if (button_sticks.state)
            {
                if (click)
                {
                    button_cpk.state = false;
                    button_sticks.state = true;
                    button_ballsticks.state = false;
                    button_elipsoids.state = false;
                    MainProgram1.CheckIfSelected(1);
                }
            }

            click = button_ballsticks.CheckButton();
            if (button_ballsticks.state)
            {
                if (click)
                {
                    button_cpk.state = false;
                    button_sticks.state = false;
                    button_ballsticks.state = true;
                    button_elipsoids.state = false;

                    MainProgram1.CheckIfSelected(2);
                }
            }

            click = button_elipsoids.CheckButton();
            if (button_elipsoids.state)
            {
                if (click)
                {
                    button_cpk.state = false;
                    button_sticks.state = false;
                    button_ballsticks.state = false;
                    button_elipsoids.state = true;

                    MainProgram1.CheckIfSelected(3);
                }
            }

            click = button_polihedral.CheckButton2();
            if (button_polihedral.state)
            {
                if (click)
                {
                    PolihedralsClass.FindPolihedral();
                }
            }
            else
            {
                PolihedralsClass.VertexesList.Clear();
                PolihedralsClass.Polihedrals.Clear();
            }


            click = button_delete.CheckButton2();
            if (button_delete.state & click)
            {
                button_delete.state = false;

                for (int i = 0; i < MainProgram1.molecule.NumberOfAtoms; i++)
                    if (MainProgram1.molecule.atom[i].selected)
                    {
                        MainProgram1.molecule.atom[i].style = 10;
                        MainProgram1.molecule.atom[i].selected = false;
                    }

                for (int i = 0; i < MainProgram1.molecule.NumberOfAtoms; i++)
                {
                    for (int j = 0; j < MainProgram1.molecule.atom[i].NrBondedAtoms; j++)
                        if (MainProgram1.molecule.atom[MainProgram1.molecule.atom[i].BondedAtom[j]].style == 10)
                        {
                            MainProgram1.molecule.atom[i].BondedAtom[j] = i;
                            MainProgram1.molecule.atom[i].AngleXY[j] = 0;
                            MainProgram1.molecule.atom[i].AngleZ[j] = 0;
                            MainProgram1.molecule.atom[i].Distance[j] = 0;
                        }
                }
            }

            #endregion

            click = button_settings.CheckButton2();
            if (button_settings.state)
            {
                if (click)
                {
                    Camera.flag_rotate = false;
                    Camera.flag_translate = false;
                    Camera.flag_lasso = false;
                    Camera.flag_select = false;
                }
            }
            else
            {
                if (click)
                {
                    Camera.flag_rotate = true;
                    PolihedralsClass.VertexesList.Clear();
                    PolihedralsClass.Polihedrals.Clear();

                    if (button_cpk.state) Flags.Presentation = 0;
                    if (button_sticks.state) Flags.Presentation = 1;
                    if (button_ballsticks.state) Flags.Presentation = 2;
                    if (button_elipsoids.state) Flags.Presentation = 3;

                    MainProgram1.molecule.Presentation(Flags.GenerateEquvalents, Flags.Presentation);
                    if (button_polihedral.state)
                    {
                        PolihedralsClass.FindPolihedral();
                    }
                }
            }

            click = button_labels.CheckButton2();
            if (button_labels.state)
            {
                if (click)
                {
                    button_labels.state = true;
                }
            }

            click = button_cell.CheckButton2();
            if (button_cell.state)
            {
                button_cell.state = true;
            }

            button_info.CheckButton2();
            if (button_info.state)
            {
            }


            click = button_move_to_cell.CheckButton2();
            if (button_move_to_cell.state)
            {
                if (click)
                {
                    if (button_polihedral.state)
                    {
                        PolihedralsClass.FindPolihedral();
                    }

                    Flags.GenerateEquvalents = 1;
                    button_move_to_cell.state = true;


                    if (button_cpk.state) Flags.Presentation = 0;
                    if (button_sticks.state) Flags.Presentation = 1;
                    if (button_ballsticks.state) Flags.Presentation = 2;
                    if (button_elipsoids.state) Flags.Presentation = 3;

                    Flags.repeatOld_x = Flags.repeat_x;
                    Flags.repeatOld_y = Flags.repeat_y;
                    Flags.repeatOld_z = Flags.repeat_z;

                    MainProgram1.molecule.Presentation(Flags.GenerateEquvalents, Flags.Presentation);
                }
            }
            else
            {
                if (click)
                {
                    Flags.GenerateEquvalents = 0;
                    button_move_to_cell.state = false;
                    PolihedralsClass.VertexesList.Clear();
                    PolihedralsClass.Polihedrals.Clear();

                    if (button_cpk.state) Flags.Presentation = 0;
                    if (button_sticks.state) Flags.Presentation = 1;
                    if (button_ballsticks.state) Flags.Presentation = 2;
                    if (button_elipsoids.state) Flags.Presentation = 3;

                    MainProgram1.molecule.Presentation(Flags.GenerateEquvalents, Flags.Presentation);
                }
            }

            click = button_foto.CheckButton2();
            if (button_foto.state)
            {
                //#region save to disk
                if (click)
                {

                }
            }

            click = button_perspective.CheckButton2();
            if (button_perspective.state)
            {
                if (click)
                {
                    Camera.PerspectiveAngle = 20;
                    Camera.PerspectiveUpdate();
                }
            }
            else
            {
                if (click)
                {
                    Camera.PerspectiveAngle = 2f;
                    Camera.PerspectiveUpdate();
                }
            }


            click = button_hydrogen.CheckButton2();
            if (button_view.state)
            {
                if (click)
                {
                }
            }


            click = button_view.CheckButton2();
            if (button_view.state)
            {
                //#region save to disk
                if (click)
                {
                     
                    Camera.flag_rotate = false;
                    button_rotate.state = false;
                    float correction = 1;
                    if (MainProgram1.molecule.beta < MathHelper.Pi / 2.0f)
                        correction = 1.0f - 0.36f * (MathHelper.Pi / 2.0f - MainProgram1.molecule.beta);

                    Flags.view++;
                    if (Flags.view > 2) Flags.view = 0;

                    switch (Flags.view)
                    {
                        //align x
                        case 0:
                            Measure.Message = "View along X direction";
                            Camera.Rotate = Matrix.Identity
                                          * Matrix.CreateRotationY(MathHelper.Pi);                                          ;
                            break;

                          

                        //align y 
                        case 1:
                            Measure.Message = "View along Y direction";
                            Camera.Rotate = Matrix.Identity
                                          * Matrix.CreateRotationZ(-Molecule.UnitCellFrame[1].AngleXY)
                                          * Matrix.CreateRotationY(-Molecule.UnitCellFrame[1].AngleZ)
                                          * Matrix.CreateRotationY(MathHelper.Pi);

                            break;

                        //align z
                        case 2:
                            Measure.Message = "View along Z direction";

                            Camera.Rotate = Matrix.Identity
                                          * Matrix.CreateRotationZ(-Molecule.UnitCellFrame[2].AngleXY)
                                          * Matrix.CreateRotationY(-Molecule.UnitCellFrame[2].AngleZ)
                                          * Matrix.CreateRotationY(MathHelper.Pi);                                          ;
 

                            break;
                    }
                    button_view.state = false;
                     
                }
            }
        }


        public static void Draw()
        {
            button_settings.ShowButton();

            if (!button_settings.state)
            {

                button_rotate.ShowButton();
                //   button_shift.ShowButton();
                button_select.ShowButton();
                button_lasso.ShowButton();

                button_foto.ShowButton();
                button_download.ShowButton();
                button_polihedral.ShowButton();
                button_labels.ShowButton();
                button_cell.ShowButton();
                button_move_to_cell.ShowButton();
                button_info.ShowButton();
                button_view.ShowButton();
                button_perspective.ShowButton();

                button_cpk.ShowButton();
                button_sticks.ShowButton();
                button_ballsticks.ShowButton();
                button_elipsoids.ShowButton();
                button_delete.ShowButton();
                button_hydrogen.ShowButton();
            }
            #region measure
            if (button_select.state)
            {
                if (!button_settings.state)
                {
                    button_measure_distance.ShowButton();
                    button_measure_angle.ShowButton();
                    button_measure_dichedral.ShowButton();
                }
            }

            #endregion
        }

        static bool CheckRepetitions()
        {
            bool res = true;
            if (Flags.repeatOld_x != Flags.repeat_x) res = false;
            if (Flags.repeatOld_y != Flags.repeat_y) res = false;
            if (Flags.repeatOld_z != Flags.repeat_z) res = false;

            if (!res)
            {
                MainProgram1.molecule.Presentation(Flags.GenerateEquvalents, Flags.Presentation);
                //Check polichedrals
                if (button_polihedral.state)
                {
                    PolihedralsClass.FindPolihedral();
                }

                Flags.repeatOld_x = Flags.repeat_x;
                Flags.repeatOld_y = Flags.repeat_y;
                Flags.repeatOld_z = Flags.repeat_z;
            }
            return res;
        }
    }


}
