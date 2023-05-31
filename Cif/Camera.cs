//***********************************************************************
//                        CIF program
//                Created by Daniel M. Kaminski
//                        Lublin 2023
//                     Under GNU licence
//***********************************************************************

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace Cif
{
    public static class Camera
    {
        static GraphicsDeviceManager _graphics;

        static public bool FlagChangeRotationMode;
        //Matrices
        static public Matrix View;
        static public Matrix Projection;
        static public Matrix Projection2;//ortogonal
        static public Matrix Rotate;
        static public Matrix Scale;

        static public float zoom;
        static public float OneOverZoom;
        static public bool flag_zooming;
        private static bool flag_after_zooming;

        static public bool flag_end_rotation;
        static public bool flag_rotating;
        private static float RotationSlowDown;
        private static float ZoomingSlowDown;
        static public Vector3 g_cameraPos;//EyePt
        static public Vector3 g_targetPos;
        static public Vector3 g_system_translate;

        //mouse operation
        static public bool flag_rotate;
        static public bool flag_translate;
        static public bool flag_double_tap;
        static public bool flag_lasso;
        static public bool flag_select;
        static public bool flag_selected;
        static public bool flag_release;
        static public bool flag_preset;

        //TouchPanel inputs
        static public Vector2 ptMouseCurrent;
        static public Vector2 ptMousePrevious;
        static public Vector2 ptMouseDoubleTap;
        static public Vector2 ptMouseSelect;
        static public Vector2 ptMousePressed;

        static private float g_yow;
        static private float g_pitch;
        static private Vector2 g_arc;

        static private Vector3 CameraRotateAroundX;			// When the orientation of camera change the z direction for arcball too
        static private Vector3 CameraRotateAroundY;			// to follow these changes we have to change 
        static private Vector3 CameraRotateAroundZ;			// the rotation directions to be z always pointing inside the screen

        //Camera parameters for 3D
        static public float PerspectiveAngle = 2f;
        static float ROTATION_STEP = 0.0035f;
        static float ROTATION_STEP_Z = 0.002f;
        static float SHIFT_SPEED = 0.008f;

        static float ShiftSpeed = SHIFT_SPEED;

        public static int MarginY = 120;
        static float m_fCameraYawAngle;		//Yaw to follow the position of camera
        static float m_fCameraPitchAngle;      //Pich to follow the position of camera

        static int count = 0;


        //Quaternions
        //static private Quaternion MainQuad;
        //static int Quaternion_normalisation;
        //---------------------------------------------------------------

        static public void Initiaqlize(GraphicsDeviceManager __graphics)
        {
            _graphics = __graphics;

            TouchPanel.EnabledGestures =
                GestureType.DoubleTap |
                GestureType.DragComplete |
                GestureType.Flick |
                GestureType.Pinch |
                GestureType.PinchComplete |
                GestureType.FreeDrag;

            PerspectiveUpdate();

            //  Quaternion_normalisation = 0;
            g_cameraPos = new Vector3(40, 0, 0);
            g_targetPos = new Vector3(0, 0, 0);

            m_fCameraYawAngle = 0.0f;		//Yaw to follow the position of camera
            m_fCameraPitchAngle = 0.0f;     //Pich to follow the position of camera
                                            //Initialize the main quaternion for the orientation matrix calculation

            //Does't work with quaternion match something wrong in XNA
            //MainQuad = new Quaternion(1, 0, 0, 0);
            Rotate = Matrix.Identity;

            //Translation
            g_system_translate = new Vector3(0, 0, 0);
            flag_rotating = false;
            flag_zooming = false;
            flag_after_zooming = false;
            RotationSlowDown = 0.95f;
            ZoomingSlowDown = 1;
            ptMouseDoubleTap = Vector2.Zero;
            flag_double_tap = false;//to clear all selections
            flag_release = true;
            flag_preset = false;
        }

        static public void PerspectiveUpdate()
        {
            //correct shift for perspective           
            ShiftSpeed = SHIFT_SPEED / (20 - PerspectiveAngle + 1);

            FlagChangeRotationMode = false; //standard mode
            zoom = 60f / (PerspectiveAngle);
            OneOverZoom = 1.0f / zoom;
            Scale = Matrix.CreateScale(OneOverZoom, OneOverZoom, OneOverZoom);

        }
        static public void Update(Viewport viewport)
        {
            #region T1:
            {
                ptMouseCurrent = Vector2.Zero;
                TouchCollection touchCollection = TouchPanel.GetState();
                foreach (TouchLocation tl in touchCollection)
                {
                    switch (tl.State)
                    {
                        case TouchLocationState.Pressed:
                            ptMousePrevious = tl.Position;
                            ptMousePressed = tl.Position;
                            flag_release = false;
                            flag_end_rotation = false;
                            flag_rotating = false;
                            flag_preset = true;
                            g_yow = 0;
                            g_pitch = 0;
                            g_arc = Vector2.Zero;

                            if (flag_select)
                            {
                                ptMouseSelect.X = 0;
                                ptMouseSelect.Y = 0;
                                flag_selected = false;
                            }
                            break;

                        case TouchLocationState.Moved:
                            ptMouseCurrent = tl.Position;
                            flag_preset = false;

                            if (ptMouseCurrent.Y > viewport.Height - MarginY)
                            {
                                flag_end_rotation = false;
                                flag_rotating = false;
                                // flag_rotate = false;
                                flag_translate = false;
                                flag_lasso = false;
                                g_yow = 0;
                                g_pitch = 0;
                                g_arc = Vector2.Zero;

                                ptMouseCurrent.Y = viewport.Height - MarginY; //to avoid menu visiting
                            }

                            if (ptMouseCurrent.Y < MarginY)
                            {
                                flag_end_rotation = false;
                                flag_rotating = false;

                                flag_translate = false;
                                flag_lasso = false;
                                g_yow = 0;
                                g_pitch = 0;
                                g_arc = Vector2.Zero;

                                ptMouseCurrent.Y = MarginY; //to avoid menu visiting
                            }

                            if (!flag_zooming && !flag_after_zooming)
                            {
                                flag_end_rotation = false;

                                if (flag_rotate)
                                {
                                    g_yow = ROTATION_STEP * (ptMousePrevious.X - ptMouseCurrent.X);
                                    g_pitch = ROTATION_STEP * (ptMousePrevious.Y - ptMouseCurrent.Y);

                                    g_arc.X = (_graphics.GraphicsDevice.Viewport.Width) / 2 - ptMouseCurrent.X;
                                    g_arc.Y = (_graphics.GraphicsDevice.Viewport.Height) / 2 - ptMouseCurrent.Y;
                                    flag_rotating = true;
                                }

                                if (flag_translate)
                                {
                                    flag_rotating = false;
                                    g_yow = 0;
                                    g_pitch = 0;

                                    float dx = (ptMouseCurrent.X - ptMousePrevious.X);
                                    float dy = (ptMouseCurrent.Y - ptMousePrevious.Y);

                                    g_system_translate.X += dx * ShiftSpeed;
                                    g_system_translate.Y -= dy * ShiftSpeed;
                                }

                                if (flag_select)//move
                                {
                                    ptMouseSelect.X = 0;
                                    ptMouseSelect.Y = 0;
                                    flag_selected = false;
                                }

                                if (flag_lasso)
                                {
                                    flag_rotating = false;
                                    g_yow = 0;
                                    g_pitch = 0;
                                    g_arc = Vector2.Zero;
                                    ptMouseSelect.X = ptMouseCurrent.X;
                                    ptMouseSelect.Y = ptMouseCurrent.Y;
                                }

                                count = 0;
                                ptMousePrevious = ptMouseCurrent;
                            }
                            break;

                        case TouchLocationState.Released:
                            flag_preset = false;
                            ptMouseCurrent = tl.Position;
                            flag_end_rotation = true;
                            flag_release = true;

                            if (flag_lasso)
                            {
                                flag_rotate = false;
                                flag_rotating = false;
                                g_yow = 0;
                                g_pitch = 0;
                                ptMouseSelect.X = -2; //signal of selection end                                                     
                            }

                            if (flag_select)
                            {
                                ptMouseSelect.X = ptMouseCurrent.X;
                                ptMouseSelect.Y = ptMouseCurrent.Y;
                                flag_selected = true;
                            }
                            break;
                    }
                }
            }
            #endregion

            #region T2:
            {
                while (TouchPanel.IsGestureAvailable)
                {
                    flag_rotating = false;
                    count = 0;
                    GestureSample gesture = TouchPanel.ReadGesture();
                    switch (gesture.GestureType)
                    {
                        case GestureType.Pinch:
                            flag_zooming = true;

                            Vector2 oldPosition1 = gesture.Position - gesture.Delta;
                            Vector2 oldPosition2 = gesture.Position2 - gesture.Delta2;
                            float newDistance = Vector2.Distance(gesture.Position, gesture.Position2);
                            float oldDistance = Vector2.Distance(oldPosition1, oldPosition2);

                            ZoomingSlowDown = (oldDistance / newDistance);
                            zoom *= ZoomingSlowDown;

                            OneOverZoom = 1.0f / zoom;
                            Scale = Matrix.CreateScale(OneOverZoom, OneOverZoom, OneOverZoom);

                            //shift the center
                            Vector2 shift = (-gesture.Delta + -gesture.Delta2) / 2;
                            g_system_translate.X -= shift.X * ShiftSpeed;
                            g_system_translate.Y += shift.Y * ShiftSpeed;

                            //Rotate around center
                            //Vector2 oldCenter = (oldPosition1 + oldPosition2) / 2;
                            //g_arc = (gesture.Position- gesture.Position2) / 2000;


                            flag_zooming = true;
                            flag_after_zooming = false;
                            g_yow = 0;
                            g_pitch = 0;

                            break;

                        case GestureType.PinchComplete:
                            flag_zooming = false;
                            flag_after_zooming = true;

                            break;

                        case GestureType.Hold:
                            flag_zooming = false;
                            flag_after_zooming = true;
                            break;
                        /*
                                                case GestureType.Flick:
                                                    if (flag_zooming == true)
                                                    {
                                                        flag_zooming = false;
                                                        flag_after_zooming = true;
                                                    }
                                                    //g_yow = 0;
                                                    //g_pitch = 0;
                                                    break;
                        */
                        case GestureType.DoubleTap:
                            flag_zooming = false;
                            flag_after_zooming = false;

                            ptMouseDoubleTap = gesture.Position;

                            if (gesture.Position.Y > MarginY && gesture.Position.Y < viewport.Height - MarginY)//for menu
                                flag_double_tap = true;//to clear all selections

                            break;

                    }
                }
            }
            #endregion

        }


        //-----------------------------------------------------------------------------
        static void UpdateCameraOrientationPichYow()
        {

            Matrix mCameraRot;
            mCameraRot = Matrix.CreateRotationX(m_fCameraYawAngle);
            mCameraRot = Matrix.CreateRotationY(m_fCameraPitchAngle);

            //axis vectors 
            Vector3 LocalRotateAroundX = new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 LocalRotateAroundY = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 LocalRotateAroundZ = new Vector3(0.0f, 0.0f, 1.0f);

            Matrix InvView;
            //D3DXMatrixInverse( &InvView, NULL, &view );
            InvView = Matrix.Invert(View);
            // The axis basis vectors and camera position are stored inside the 
            // position matrix in the 4 rows of the camera's world matrix.
            // To figure out the yaw/pitch of the camera, we just need the Z basis vector
            // this is neccesary to follow by the arc ball the camera position in the word,
            // the basic rotators of quaternions are changed
            //D3DXVECTOR3* pZBasis = ( D3DXVECTOR3* )&InvView._31;
            Vector3 pZBasis;
            pZBasis.X = InvView.M31;
            pZBasis.Y = InvView.M32;
            pZBasis.Z = InvView.M33;

            m_fCameraYawAngle = (float)Math.Atan2(pZBasis.X, pZBasis.Z);
            m_fCameraPitchAngle = (float)(-Math.Atan2(pZBasis.Y, Math.Sqrt(pZBasis.Z * pZBasis.Z + pZBasis.X * pZBasis.X)));

            CameraRotateAroundX = Vector3.Transform(LocalRotateAroundX, mCameraRot);
            CameraRotateAroundY = Vector3.Transform(LocalRotateAroundY, mCameraRot);
            CameraRotateAroundZ = Vector3.Transform(LocalRotateAroundZ, mCameraRot);


            CameraRotateAroundX = LocalRotateAroundX;
            CameraRotateAroundY = LocalRotateAroundY;
            CameraRotateAroundZ = LocalRotateAroundZ;

        }


        static void UpdateQuadRotation()
        {


            //Rotate around X,Y,Z of arcball
            //g_pitch added to rotate with different speed
            float roll_X = g_pitch * g_arc.X;

            //g_yowto added to rotate with different speed
            float roll_Y = g_yow * g_arc.Y;

            float roll = ROTATION_STEP_Z * (roll_Y - roll_X);

            //if (flag_zooming)
            //    roll = (g_arc.Y - g_arc.X);

            count++;
            //Continous to rotate
            if (!flag_rotating && !flag_zooming && !flag_after_zooming)
            {
                g_pitch *= RotationSlowDown;
                g_yow *= RotationSlowDown;
                count = 0;
            }
            //Continous to rotate
            if (!flag_rotating && !flag_zooming && flag_after_zooming)
            {

                zoom *= ZoomingSlowDown;
                OneOverZoom = 1.0f / zoom;
                Scale = Matrix.CreateScale(OneOverZoom, OneOverZoom, OneOverZoom);

                if (count >= 5)
                {
                    flag_after_zooming = false;
                    count = 0;
                }
                g_pitch = 0;
                g_yow = 0;
            }

            ////Rotate around X axis
            //Quaternion LocalRotQuadX = Quaternion.CreateFromAxisAngle(CameraRotateAroundY, g_yow);
            ////Rotate around Y axis
            //Quaternion LocalRotQuadY = Quaternion.CreateFromAxisAngle(CameraRotateAroundZ, g_pitch);
            ////Rotate around Z axis
            //Quaternion LocalRotQuadZ = Quaternion.CreateFromAxisAngle(CameraRotateAroundX, roll);

            Matrix xtmp = Matrix.CreateFromAxisAngle(CameraRotateAroundY, -g_yow);
            Matrix ytmp = Matrix.CreateFromAxisAngle(CameraRotateAroundZ, g_pitch);
            Matrix ztmp = Matrix.CreateFromAxisAngle(CameraRotateAroundX, roll);

            Rotate *= xtmp * ytmp * ztmp;

            //Combine all rotations and update the main rotation quaternion
            //MainQuad *= LocalRotQuadX;
            //MainQuad *= LocalRotQuadY;
            //MainQuad *= LocalRotQuadZ;

            //Rotate = Matrix.CreateFromQuaternion(MainQuad);


            //Check for Quaternion normalisation after N rotation events to recover it
            //Quaternion_normalisation++;

            //if (Quaternion_normalisation > 100)
            //{
            //    Quaternion_normalisation = 0;
            //    Quaternion.Normalize(MainQuad);
            //}


        }

        ///-----------------------------------------------------------------------------
        /// Calculate all camera matrices. The rotation matrics is recovery every time from quaternion which stores the rotation state.
        ///-----------------------------------------------------------------------------
        public static void ApplyEffects()
        {

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(PerspectiveAngle),
                                                            _graphics.GraphicsDevice.Viewport.AspectRatio, 1f, 200f);



            Projection2 = Matrix.CreateOrthographicOffCenter(0, _graphics.GraphicsDevice.Viewport.Width,
                                                             _graphics.GraphicsDevice.Viewport.Height, 0, 0, 1000);

            Vector3 LookUp = new Vector3(0.0f, 1.0f, 0.0f);

            if (g_cameraPos.X == 0 && g_cameraPos.Z == 0) LookUp = new Vector3(0.0f, 0.0f, 1.0f);

            View = Matrix.CreateLookAt(g_cameraPos, g_targetPos, LookUp);

            Matrix Translate = Matrix.CreateTranslation(g_system_translate);
            View *= Translate;

            UpdateCameraOrientationPichYow();
            UpdateQuadRotation();

            //build rotation matrix from quaternion
            //Rotate = Matrix.CreateFromQuaternion(MainQuad);


            //Matrix m_temp = Matrix.CreateTranslation(g_system_translate);
            //View *= m_temp;





        }

    }
}
