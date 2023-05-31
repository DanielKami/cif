//***********************************************************************
//                        CIF program
//                Created by Daniel M. Kaminski
//                        Lublin 2023
//                     Under GNU licence
//***********************************************************************

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace Cif
{
    public static class Measure
    {
        public struct Measurement
        {
            public int type;
            public int Atom1;
            public int Atom2;
            public int Atom3;
            public int Atom4;
            public float value;
        }

        public static List<Measurement> measurement = new List<Measurement>();



        public static string Message = "";


        public static void MouseSelect(Molecule molecule, int Width, int Height, Matrix RotateViewProjectionMatrix, Vector2 MousePosition)
        {

            int i;
            int TOLERANCE = 40;
            Matrix world;

 
            for (i = 0; i < molecule.NumberOfAtoms; ++i)
            {

                world = Matrix.CreateTranslation(molecule.atom[i].Coordinate);
                Matrix worldViewProjectionMatrix = world * RotateViewProjectionMatrix;

                int modelX = (int)(Width / 2 * (1.0 + worldViewProjectionMatrix.M41 / worldViewProjectionMatrix.M44));
                int modelY = (int)(Height / 2 * (1.0 - worldViewProjectionMatrix.M42 / worldViewProjectionMatrix.M44));


                if (modelX < MousePosition.X + TOLERANCE && modelX > MousePosition.X - TOLERANCE && modelY < MousePosition.Y + TOLERANCE && modelY > MousePosition.Y - TOLERANCE)
                {
                    if (!molecule.atom[i].selected)
                    {
                        if (molecule.atom[i].style != 10)
                        {
                            molecule.atom[i].selected = true;
                            molecule.NumberSelected++;
                            molecule.atom[i].SelectionOrder = molecule.NumberSelected;
                        }
                    }
                    else
                    {
                        molecule.atom[i].selected = false;
                        molecule.NumberSelected--;
                        molecule.atom[i].SelectionOrder = 0;
                    }
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        //
        //    Distance between atoms
        //
        //--------------------------------------------------------------------------------------------------------------------------------
        public static bool MeasureDistance(Molecule molecule)
        {
            int j;
            float x1 = 0, y1 = 0, z1 = 0;
            float x2 = 0, y2 = 0, z2 = 0;
            string Symbol1;
            string Symbol2;
            double distans;
            float error;

            Measurement measure = new Measurement();

            Symbol1 = "";
            Symbol2 = "";

            //Check
            if (molecule.NumberSelected > 2)
            {
                Message = "Too many selected atoms! (" + molecule.NumberSelected + ")";
                return false;
            }

            if (molecule.NumberSelected < 2)
            {
                Message = "Not enought atoms selected! (" + molecule.NumberSelected + ")";
                return false;
            }

            //If the number of atoms is 2 we can find the atoms coordinates
            for (j = 0; j < molecule.NumberOfAtoms; ++j)
            {
                if (molecule.atom[j].selected && molecule.atom[j].SelectionOrder == 1)
                {
                    x1 = molecule.atom[j].Coordinate.X;
                    y1 = molecule.atom[j].Coordinate.Y;
                    z1 = molecule.atom[j].Coordinate.Z;
                    Symbol1 = molecule.atom[j].name;
                    measure.Atom1 = j;
                    break;
                }
            }

            for (j = 0; j < molecule.NumberOfAtoms; ++j)
            {
                if (molecule.atom[j].selected && molecule.atom[j].SelectionOrder == 2)
                {
                    x2 = molecule.atom[j].Coordinate.X;
                    y2 = molecule.atom[j].Coordinate.Y;
                    z2 = molecule.atom[j].Coordinate.Z;
                    Symbol2 = molecule.atom[j].name;
                    measure.Atom2 = j;
                    break;
                }
            }

            //distance
            distans = Math.Sqrt((x1 - x2) * (x1 - x2) +
                         (y1 - y2) * (y1 - y2) +
                         (z1 - z2) * (z1 - z2));

            error = CalculateDistanceError(molecule.atom[measure.Atom1], molecule.atom[measure.Atom2]);
            error = LErr(ref distans, error);
            Message = "Distance " + Symbol1 + ", " + Symbol2 + ": " + distans.ToString("#.####") + " (" + (int)error + ")";

            //copy the measurement for presentation
            measure.type = 1; //distance
            measure.value = (float)distans;
            measurement.Add(measure);

            return true;
        }

        //--------------------------------------------------------------------------------------------------------------------------------
        //Measure angle between 3 selected atoms
        //
        //--------------------------------------------------------------------------------------------------------------------------------
        public static bool MeasureAngle(Molecule molecule)
        {

            int j;
             
            Vector3 v1 = Vector3.Zero;
            Vector3 v2 = Vector3.Zero;
            Vector3 v3 = Vector3.Zero;
            double angle;
            string Symbol1 = "", Symbol2 = "", Symbol3 = "";

            Measurement measure = new Measurement();

            //Check  atoms
            if (molecule.NumberSelected > 3)
            {
                Message = "Too many selected atoms! (" + molecule.NumberSelected + ")";
                return false;
            }


            if (molecule.NumberSelected < 3)
            {
                Message = "Not enought atoms selected! (" + molecule.NumberSelected + ")";
                return false;
            }

            //Copy if they are there
            for (j = 0; j < molecule.NumberOfAtoms; ++j)
            {
                if (molecule.atom[j].selected && molecule.atom[j].SelectionOrder == 1)
                {
                    v1 = molecule.atom[j].Coordinate;
                    Symbol1 = molecule.atom[j].name;
                    measure.Atom1 = j;
                }
            }

            for (j = 0; j < molecule.NumberOfAtoms; ++j)
            {
                if (molecule.atom[j].selected && molecule.atom[j].SelectionOrder == 2)
                {
                    v2 = molecule.atom[j].Coordinate;
                    Symbol2 = molecule.atom[j].name;
                    measure.Atom2 = j;
                }
            }

            for (j = 0; j < molecule.NumberOfAtoms; ++j)
            {
                if (molecule.atom[j].selected && molecule.atom[j].SelectionOrder == 3)
                {
                    v3 = molecule.atom[j].Coordinate;
                    Symbol3 = molecule.atom[j].name;
                    measure.Atom3 = j;
                }
            }

            //---------------------------
            float distance1 = Vector3.Distance(v1, v2);
            float distance2 = Vector3.Distance(v2, v3);
            float distance3 = Vector3.Distance(v1, v3);

            //ABC = arc cos (AB*AB + BC*BC - AC*AC) / (2*AB*AC)

            double temp_calc = distance1 * distance1 + distance2 * distance2 - distance3 * distance3;
            temp_calc /= 2.0 * distance1 * distance2;
            angle = MathHelper.ToDegrees((float)Math.Acos(temp_calc));

            float error = CalculateAngleError(molecule.atom[measure.Atom1], molecule.atom[measure.Atom2], molecule.atom[measure.Atom3]);
            error = LErr(ref angle, error);
            Message = "Angle: " + Symbol1 + ", " + Symbol2 + ", " + Symbol3 + ": " + angle.ToString("#.##") + "(" + error + ")";



            //copy the measurement for presentation
            measure.type = 2; //distance
            measure.value = (float)angle;
            measurement.Add(measure);

            return true;
        }


        public static bool MeasureTorsian(Molecule molecule)
        {

            int j;
            double[] vector_w = new double[3];
            double[] vector_v = new double[3];
            double[] cross = new double[3];
            double[] plane1 = new double[4];
            double[] plane2 = new double[4];
            
            float error;


            double angle;
            string Symbol1 = "", Symbol2 = "", Symbol3 = "", Symbol4 = "";


            Measurement measure = new Measurement();
            //Check atoms

            if (molecule.NumberSelected > 4)
            {
                Message = "Too many selected atoms! (" + molecule.NumberSelected + ")";
                return false;
            }



            if (molecule.NumberSelected < 4)
            {
                Message = "Not enought atoms selected! (" + molecule.NumberSelected + ")";
                return false;
            }

            Vector3 V1 = Vector3.Zero, V2 = Vector3.Zero, V3 = Vector3.Zero, V4 = Vector3.Zero;

            //Copy if they are there
            for (j = 0; j < molecule.NumberOfAtoms; ++j)
            {
                if (molecule.atom[j].selected && molecule.atom[j].SelectionOrder == 1)
                {
                    V1 = molecule.atom[j].Coordinate;

                    Symbol1 = molecule.atom[j].name;
                    measure.Atom1 = j;
                }
            }

            for (j = 0; j < molecule.NumberOfAtoms; ++j)
            {
                if (molecule.atom[j].selected && molecule.atom[j].SelectionOrder == 2)
                {
                    V2 = molecule.atom[j].Coordinate;

                    Symbol2 = molecule.atom[j].name;
                    measure.Atom2 = j;
                }
            }

            for (j = 0; j < molecule.NumberOfAtoms; ++j)
            {
                if (molecule.atom[j].selected && molecule.atom[j].SelectionOrder == 3)
                {
                    V3 = molecule.atom[j].Coordinate;

                    Symbol3 = molecule.atom[j].name;
                    measure.Atom3 = j;
                }
            }

            for (j = 0; j < molecule.NumberOfAtoms; ++j)
            {
                if (molecule.atom[j].SelectionOrder == 4)
                {
                    V4 = molecule.atom[j].Coordinate;

                    Symbol4 = molecule.atom[j].name;
                    measure.Atom4 = j;
                }
            }
            //-----------------------------------------------------------------------------------------------------------------
            //Surface 1
            //Cros product of atoms 1, 2, 3 : W x V = <Wy*Vz - Wz*Vy, Wz*Vx - Wx*Vz, Wx*Vy - Wy*Vx>

            Vector3 U = Vector3.Normalize(V1 - V3);
            Vector3 V = Vector3.Normalize(V2 - V3);
            Vector3 W = Vector3.Normalize(V4 - V3);

            Vector3 Cross1 = Vector3.Cross(U, V);
            Vector3 Cross2 = Vector3.Cross(V, W);
            Vector3 Cross3 = Vector3.Cross(Cross1, V);

            double x = Vector3.Dot(Cross1, Cross2);
            double y = Vector3.Dot(Cross3, Cross2);

            angle = MathHelper.ToDegrees((float)Math.Atan2(x, y));

            error = CalculateDichedralError(molecule.atom[measure.Atom1], molecule.atom[measure.Atom2], molecule.atom[measure.Atom3], molecule.atom[measure.Atom4]);
            error = LErr(ref angle, error);

            Message = "Torsian:" + Symbol1 + ", " + Symbol2 + ", " + Symbol3 + ", " + Symbol4 + ": " + angle.ToString("#.##") + "(" + error + ")";


            //copy the measurement for presentation
            measure.type = 2; //distance
            measure.value = (float)angle;
            measurement.Add(measure);

            return true;
        }

        static double sqr(double x)
        {
            return x * x;
        }

        static float CalculateDistanceError(Molecule.Atom A1, Molecule.Atom A2)
        {
            double error1 = CalculateVectorError(A1.Coordinate_error);
            double error2 = CalculateVectorError(A2.Coordinate_error);

            return (float)Math.Sqrt(sqr(error1) + sqr(error2));
        }

        static float CalculateAngleError(Molecule.Atom A1, Molecule.Atom A2, Molecule.Atom A3)
        {
            Vector3 V1 = A1.Coordinate - A2.Coordinate;
            Vector3 V2 = A3.Coordinate - A2.Coordinate;
            double ax_e = Math.Sqrt(sqr(A1.Coordinate_error.X) + sqr(A2.Coordinate_error.X));
            double ay_e = Math.Sqrt(sqr(A1.Coordinate_error.Y) + sqr(A2.Coordinate_error.Y));
            double az_e = Math.Sqrt(sqr(A1.Coordinate_error.Z) + sqr(A2.Coordinate_error.Z));
            double bx_e = Math.Sqrt(sqr(A3.Coordinate_error.X) + sqr(A2.Coordinate_error.X));
            double by_e = Math.Sqrt(sqr(A3.Coordinate_error.Y) + sqr(A2.Coordinate_error.Y));
            double bz_e = Math.Sqrt(sqr(A3.Coordinate_error.Z) + sqr(A2.Coordinate_error.Z));

            //Derivative for V1.X
            double ax = V1.X; //a/x
            double ay = V1.Y; //c
            double az = V1.Z; //e
            double bx = V2.X; //b
            double by = V2.Y; //d
            double bz = V2.Z; //s

            //Error ax
            double er = AngleDerivative(ax, ay, az, bx, by, bz);
            double e_ax = sqr(er) * sqr(ax_e);
            //Error ay
            ax = V1.Y; //a/x
            ay = V1.X; //c
            er = AngleDerivative(ax, ay, az, bx, by, bz);
            double e_ay = sqr(er) * sqr(ay_e);
            //Error az
            ax = V1.Z; //a/x
            ay = V1.Y; //c
            az = V1.X; //e
            er = AngleDerivative(ax, ay, az, bx, by, bz);
            double e_az = sqr(er) * sqr(az_e);
            //Error bx
            ax = V2.X; //a/x
            ay = V2.Y; //c
            az = V2.Z; //e
            bx = V1.X; //b
            by = V1.Y; //d
            bz = V1.Z; //s
            er = AngleDerivative(ax, ay, az, bx, by, bz);
            double e_bx = sqr(er) * sqr(bx_e);
            ax = V2.Y; //a/x
            ay = V2.X; //c
            er = AngleDerivative(ax, ay, az, bx, by, bz);
            double e_by = sqr(er) * sqr(bx_e);
            //Error bz
            ax = V2.Z; //a/x
            ay = V2.Y; //c
            az = V2.X; //e
            er = AngleDerivative(ax, ay, az, bx, by, bz);
            double e_bz = sqr(er) * sqr(az_e);


            return (float)(Math.Sqrt(e_ax + e_ay + e_az + e_bx + e_by + e_bz) * 180.0f / Math.PI);
        }

        static double AngleDerivative(double ax, double ay, double az, double bx, double by, double bz)
        {
            double v1 = (-ay * ax * ax - 2.0 * ay * by * ax - 2.0 * az * ax * bz + az * az * bx + bx * ay * ay);
            double v2 = (ax * ax + ay * ay + az * az);
            double v3 = sqr(ax * ax + ay * ay + az * az) * sqr(bz * bz + bx * bx + ay * ay);
            double v4 = 4.0 * sqr(bx * ax + ay * by + az * bz);
            double v5 = v3 - v4;
            if (v5 < 0) v5 *= -1;
            double v6 = v2 * Math.Sqrt(v5);
            if (v6 == 0) v6 = 1;

            return -2.0 * v1 / (v6);
        }
        static float CalculateDichedralError(Molecule.Atom A1, Molecule.Atom A2, Molecule.Atom A3, Molecule.Atom A4)
        {
            double error1 = CalculateVectorError(A1.Coordinate_error);
            double error2 = CalculateVectorError(A2.Coordinate_error);
            double error3 = CalculateVectorError(A3.Coordinate_error);
            double error4 = CalculateVectorError(A4.Coordinate_error);

            Vector3 W = Vector3.Normalize(A1.Coordinate - A3.Coordinate);
            Vector3 V = Vector3.Normalize(A2.Coordinate - A3.Coordinate);
            Vector3 Cross1 = Vector3.Cross(W, V);

            Vector3 Wt = W;
            Wt.X = 1; // dWt/dx
            Vector3 e1 = Vector3.Cross(Wt, V) * (float)Math.Sqrt(sqr(A1.Coordinate_error.X) + sqr(A3.Coordinate_error.X));
            Wt = W;
            Wt.Y = 1; // dWt/dy
            Vector3 e2 = Vector3.Cross(Wt, V) * (float)Math.Sqrt(sqr(A1.Coordinate_error.Y) + sqr(A3.Coordinate_error.Y));
            Wt = W;
            Wt.Z = 1; // dWt/dz
            Vector3 e3 = Vector3.Cross(Wt, V) * (float)Math.Sqrt(sqr(A1.Coordinate_error.Z) + sqr(A3.Coordinate_error.Z));

            Vector3 Cross1_e;
            Cross1_e.X = (float)Math.Sqrt(sqr(e1.X) + sqr(e2.X) + sqr(e3.X));
            Cross1_e.Y = (float)Math.Sqrt(sqr(e1.Y) + sqr(e2.Y) + sqr(e3.Y));
            Cross1_e.Z = (float)Math.Sqrt(sqr(e1.Z) + sqr(e2.Z) + sqr(e3.Z));

            /////////////////////////////////////////////////////////////////////////////////
            W = Vector3.Normalize(A4.Coordinate - A3.Coordinate);
            Vector3 Cross2 = Vector3.Cross(W, V);
            Wt = W;
            Wt.X = 1; // dWt/dx
            e1 = Vector3.Cross(Wt, V) * (float)Math.Sqrt(sqr(A4.Coordinate_error.X) + sqr(A3.Coordinate_error.X));
            Wt = W;
            Wt.Y = 1; // dWt/dy
            e2 = Vector3.Cross(Wt, V) * (float)Math.Sqrt(sqr(A4.Coordinate_error.Y) + sqr(A3.Coordinate_error.Y));
            Wt = W;
            Wt.Z = 1; // dWt/dz
            e3 = Vector3.Cross(Wt, V) * (float)Math.Sqrt(sqr(A4.Coordinate_error.Z) + sqr(A3.Coordinate_error.Z));

            Vector3 Cross2_e;
            Cross2_e.X = (float)Math.Sqrt(sqr(e1.X) + sqr(e2.X) + sqr(e3.X));
            Cross2_e.Y = (float)Math.Sqrt(sqr(e1.Y) + sqr(e2.Y) + sqr(e3.Y));
            Cross2_e.Z = (float)Math.Sqrt(sqr(e1.Z) + sqr(e2.Z) + sqr(e3.Z));

            //We have values and errors of cross products so use the standart approach


            //Derivative for V1.X
            double ax = Cross1.X; //a/x
            double ay = Cross1.Y; //c
            double az = Cross1.Z; //e
            double bx = Cross2.X; //b
            double by = Cross2.Y; //d
            double bz = Cross2.Z; //s

            double ax_e = Cross1_e.X; //a/x
            double ay_e = Cross1_e.Y; //c
            double az_e = Cross1_e.Z; //e
            double bx_e = Cross2_e.X; //b
            double by_e = Cross2_e.Y; //d
            double bz_e = Cross2_e.Z; //s

            //Error ax
            double er = AngleDerivative(ax, ay, az, bx, by, bz);
            double e_ax = sqr(er) * sqr(ax_e);
            //Error ay
            ax = Cross1.Y; //a/x
            ay = Cross1.X; //c
            er = AngleDerivative(ax, ay, az, bx, by, bz);
            double e_ay = sqr(er) * sqr(ay_e);
            //Error az
            ax = Cross1.Z; //a/x
            ay = Cross1.Y; //c
            az = Cross1.X; //e
            er = AngleDerivative(ax, ay, az, bx, by, bz);
            double e_az = sqr(er) * sqr(az_e);
            //Error bx
            ax = Cross2.X; //a/x
            ay = Cross2.Y; //c
            az = Cross2.Z; //e
            bx = Cross1.X; //b
            by = Cross1.Y; //d
            bz = Cross1.Z; //s
            er = AngleDerivative(ax, ay, az, bx, by, bz);
            double e_bx = sqr(er) * sqr(bx_e);
            ax = Cross2.Y; //a/x
            ay = Cross2.X; //c
            er = AngleDerivative(ax, ay, az, bx, by, bz);
            double e_by = sqr(er) * sqr(bx_e);
            //Error bz
            ax = Cross2.Z; //a/x
            ay = Cross2.Y; //c
            az = Cross2.X; //e
            er = AngleDerivative(ax, ay, az, bx, by, bz);
            double e_bz = sqr(er) * sqr(az_e);


            return (float)(Math.Sqrt(e_ax + e_ay + e_az + e_bx + e_by + e_bz) * 180.0f / Math.PI);

        }

        static double CalculateVectorError(Vector3 A)
        {
            double x = sqr(A.X);
            double y = sqr(A.Y);
            double z = sqr(A.Z);
            return Math.Sqrt(x + y + z);
        }

        public static int LErr(ref double value, float error)
        {
            //Find firs significant number in error rounded up.
            int res = 0;
            int i;
            int dev = 1;
            for (i = 0; i < 6; i++)
            {
                dev = (int)Math.Pow(10, i);
                res = (int)Math.Round(error * dev);
                if (res > 0)
                {
                    break; //we have the signiificant place after coma
                }
            }
            value = (float)((1.0 * (int)(value * dev)) / dev);

            return (int)res;

        }

        public static int RErr(float value, float error)
        {
            int count = 0;
            double dev = 1;
            string[] splits = (value.ToString()).Split(',', '.');

            if (splits.Length > 1)
                count = splits[1].Length;
            else
                return 0;

            if (count != 0)
                dev = Math.Pow(10, count);

            double res = Math.Ceiling(error * dev);
            return (int)res;
        }
    }
}
