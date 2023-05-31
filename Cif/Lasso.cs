using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cif
{
    public static class Lasso
    {
        public static List<Vector2> VertexList = new List<Vector2>(); //List of vertices for selection lasso


        public static bool[] Mask;
        static List<Pixel> LassoPoint = new List<Pixel>();

        struct Pixel
        {
            public int x;
            public int y;
        };


        public static void MouseLasso(Molecule molecule, int Width, int Height)
        {
            Matrix RotateViewProjectionMatrix = Camera.Scale * Camera.Rotate * Camera.View * Camera.Projection;

            if (VertexList.Count < 2)
                return;

            int i, j;
            int maxsize = (Height + 1) * (Width + 1);
            Mask = new bool[maxsize];


            //First close the shape eliminate all gaps
            for (i = 1; i < VertexList.Count; ++i)
            {
                LineLasso(VertexList[i - 1], VertexList[i]);
            }

            LineLasso(VertexList[VertexList.Count - 1], VertexList[0]);

            for (i = 0; i < LassoPoint.Count; ++i)
            {
                Mask[LassoPoint[i].y * Width + LassoPoint[i].x] = true;
            }

            //Fill the outside area and prepare the mask //works
            //can be thread separated
            BoundaryFill(1, 1, Width, Height, true, true);

            LassoPoint.Clear();

            //Reverse selection
            for (i = 0; i < Width; ++i)
            {
                for (j = 0; j < Height; ++j)
                {
                    if (Mask[i + j * Width])
                        Mask[i + j * Width] = false;
                    else Mask[i + j * Width] = true;
                }
            }


            //__________________________________________________________________________________________________________________
            //	Find atoms
            molecule.NumberSelected = 0;
            Matrix world;
            for (i = 0; i < molecule.NumberOfAtoms; ++i)
            {
                world = Matrix.CreateTranslation(molecule.atom[i].Coordinate);
                Matrix worldViewProjectionMatrix = world * RotateViewProjectionMatrix;

                int modelX = (int)(Width / 2 * (1.0 + worldViewProjectionMatrix.M41 / worldViewProjectionMatrix.M44));
                int modelY = (int)(Height / 2 * (1.0 - worldViewProjectionMatrix.M42 / worldViewProjectionMatrix.M44));

                //Corection for viewport size related to menu height
                //modelY = (int)(modelY - MenuHeight / 4 + 10);
                int temp = modelX + modelY * Width;
                if (temp > 0 && temp < Mask.Length)
                    if (Mask[temp])
                        molecule.atom[i].selected = true;

                if (molecule.atom[i].selected && molecule.atom[i].style != 10) //different than 10 because deleted
                {
                    molecule.NumberSelected++;

                }
            }
            VertexList.Clear();
        }

        static double disyance(Pixel p1, Pixel p2)
        {
            double w1 = p1.x - p2.x;
            double w2 = p1.y - p2.y;
            return Math.Sqrt(w1 * w1 + w2 * w2);
        }

        //Fill intermediate points
        static void LineLasso(Vector2 Pos1, Vector2 Pos2)
        {
            float delta_x;
            float delta_y;

            int Adelta_x;
            int Adelta_y;
            int i;
            int delta_i;
            Pixel pixel;

            delta_x = (Pos2.X - Pos1.X);
            delta_y = (Pos2.Y - Pos1.Y);
            Adelta_x = Math.Abs((int)delta_x);
            Adelta_y = Math.Abs((int)delta_y);
            //take longer vector
            if (Adelta_x > Adelta_y)
                delta_i = Adelta_x + 1;
            else
                delta_i = Adelta_y + 1;

            delta_x /= delta_i;
            delta_y /= delta_i;

            for (i = 0; i < delta_i; ++i)
            {
                pixel.x = (int)(Pos1.X + delta_x * i);
                pixel.y = (int)(Pos1.Y + delta_y * i);
                LassoPoint.Add(pixel);
            }
        }

        static void BoundaryFill(int _x, int _y, int Width, int Height, bool fill_color, bool boundary_color)
        {

            List<Pixel> Point = new List<Pixel>();

            //check the starting point
            int z = _x + _y * Width;
            if (Mask[z] == fill_color)// || Mask[z] == boundary_color 
                return;

            Pixel pixel, tmp_pixel;
            pixel.x = _x; pixel.y = _y;
            Point.Add(pixel);
            int position = 0;

            //fill the first pixel
            Mask[z] = fill_color;

            while (Point.Count > 0)
            {
                position = Point.Count;
                pixel = Point.ElementAt(Point.Count - 1);

                //x-1
                z = pixel.x - 1 + pixel.y * Width;
                if ((pixel.x - 1) >= 0 && Mask[z] != fill_color) //  && Mask[z] != boundary_color
                {
                    Mask[z] = fill_color;

                    tmp_pixel.x = pixel.x - 1; tmp_pixel.y = pixel.y;
                    Point.Add(tmp_pixel);
                }

                //y-1
                z = pixel.x + (pixel.y - 1) * Width;
                if ((pixel.y - 1) >= 0 && Mask[z] != fill_color) //&& Mask[z] != boundary_color 
                {
                    Mask[z] = fill_color;

                    tmp_pixel.x = pixel.x; tmp_pixel.y = pixel.y - 1;
                    Point.Add(tmp_pixel);
                }

                //x+1
                z = pixel.x + 1 + pixel.y * Width;
                if ((pixel.x + 1) <= Width && Mask[z] != fill_color)//&& Mask[z] != boundary_color
                {
                    Mask[z] = fill_color;

                    tmp_pixel.x = pixel.x + 1; tmp_pixel.y = pixel.y;
                    Point.Add(tmp_pixel);
                }

                //y+1
                z = pixel.x + (pixel.y + 1) * Width;
                if ((pixel.y + 1) <= Height && Mask[z] != fill_color) //&& Mask[z] != boundary_color 
                {
                    Mask[z] = fill_color;

                    tmp_pixel.x = pixel.x; tmp_pixel.y = pixel.y + 1;
                    Point.Add(tmp_pixel);
                }
                Point.RemoveAt(position - 1);
            }
        }
    }
}
