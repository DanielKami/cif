//***********************************************************************
//                        CIF program
//                Created by Daniel M. Kaminski
//                        Lublin 2023
//                     Under GNU licence
//***********************************************************************

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Cif.MainProgram1;

namespace Cif
{
    internal static class PolihedralsClass
    {

        public struct VertexStruct
        {
            public Vector3 position;
            public Vector3 color;
            public VertexPositionNormalTexture[] vertices_n;
            public VertexPositionNormalTexture[] vertices_s;
        }
        public static List<VertexStruct> VertexesList = new List<VertexStruct>();

        //Polihedral draw
        public struct PolihedralsStruct
        {
            public List<int> PolichedralAtoms;
            public int Nr_centralAtom;
        }
        public static List<PolihedralsStruct> Polihedrals = new List<PolihedralsStruct>();


        public static void FindPolihedral()
        {

            VertexesList.Clear();
            Polihedrals.Clear();

            if (molecule.atom == null)
                return;

            //First find metal
            for (int i = 0; i < molecule.atom.Length; i++)
            {
                int nu = molecule.atom[i].AtomicNumber;

                if (nu == 3 || (nu >= 11 && nu <= 14) || (nu >= 19 && nu <= 30) || (nu >= 37 && nu <= 50) || (nu >= 55 && nu <= 83) || (nu >= 87 && nu <= 118))
                {
                    PolihedralsStruct storage = new PolihedralsStruct();
                    storage.PolichedralAtoms = new List<int>();
                    //find oxygens around
                    for (int j = 0; j < molecule.atom.Length; j++)
                    {
                        if (molecule.atom[j].AtomicNumber == 8)
                        {
                            //find dystance to it
                            float dist = Vector3.Distance(molecule.atom[j].Coordinate, molecule.atom[i].Coordinate);
                            if (dist < Flags.Poli_dyst)
                            {
                                storage.PolichedralAtoms.Add(j);
                            }
                        }
                    }
                    if (storage.PolichedralAtoms.Count >= 4)
                    {
                        storage.Nr_centralAtom = nu;
                        //for (int k = 0; k < storage.PolichedralAtoms.Count; k++)
                        //    molecule.atom[storage.PolichedralAtoms[k]].selected = true;
                        Polihedrals.Add(storage);
                    }
                }
            }

            /////////////////////////////////////////////////

            // Polihedrals
            for (int i = 0; i < Polihedrals.Count; i++)//iterate all polihedrals
            {
                VertexStruct Primitive_struct = new VertexStruct();

                //Define array of polichedral vertices
                PolihedralsStruct Vertices_temp = Polihedrals[i];//one polichedral from list of polichedrals            

         
                //Polichedral color
                Primitive_struct.color = molecule.color[Vertices_temp.Nr_centralAtom];  //Polichedral Color!

                //reference position
                Primitive_struct.position = molecule.atom[0].Coordinate;


                //FindStrategy(Vertices_temp, ref Primitive_struct);

                switch (Vertices_temp.PolichedralAtoms.Count)
                {
                    case 4:
                        FindTringle4(Vertices_temp, ref Primitive_struct);
                        break;
                    case 5:
                        FindTringle5(Vertices_temp, ref Primitive_struct);
                        break;
                    case 6:
                        FindTringle6(Vertices_temp, ref Primitive_struct);
                        break;
                        //deafolt:
                        //    FindTringleAny(Vertices_temp, ref Primitive_struct);
                }

                //Primitive_struct.vb = new VertexBuffer(GraphicsDevice, VertexPositionNormalTexture.VertexDeclaration,
                //Primitive_struct.vertices_n.Length, BufferUsage.WriteOnly);
                //Primitive_struct.vb.SetData(0, Primitive_struct.vertices_n, 0, Primitive_struct.vertices_n.Length, 0);

                VertexesList.Add(Primitive_struct);
            }
        }

        static void FindTringleAny(PolihedralsStruct Vertices, ref VertexStruct Primitive_struct)
        {

            int[,] Strategy = FindStrategy(Vertices, ref Primitive_struct);



            AsignTringles(Vertices, ref Primitive_struct, Strategy);
        }
        static void FindTringle4(PolihedralsStruct Vertices, ref VertexStruct Primitive_struct)
        {
            int[,] Strategy =
            {
                {0,1,2 },
                {2,1,3 },
                {2,3,0 },
                {0,3,1 }
            };

            Primitive_struct.vertices_n = new VertexPositionNormalTexture[Strategy.Length/3 * 3];
            Primitive_struct.vertices_s = new VertexPositionNormalTexture[Strategy.Length / 3 * 3];

            AsignTringles(Vertices, ref Primitive_struct, Strategy);
        }
        static void FindTringle5(PolihedralsStruct Vertices, ref VertexStruct Primitive_struct)
        {
            int[] a = new int[Vertices.PolichedralAtoms.Count];

            for (int i = 0; i < Vertices.PolichedralAtoms.Count; i++)
            {
                a[i] = i;
            }
            int top = FindTop(Vertices); //vertice 0

            for (int j = 0; j < Vertices.PolichedralAtoms.Count; j++)
            {
                if (top == a[j])
                {
                    int temp = a[0];
                    a[0] = top;
                    a[j] = temp;
                    break;
                }
            }
            for (int j = 1; j < Vertices.PolichedralAtoms.Count-1; j++)
                for (int i = 1; i < Vertices.PolichedralAtoms.Count; i++)
                {      
                    int p1 = Vertices.PolichedralAtoms[a[i]];
                    int p2 = Vertices.PolichedralAtoms[a[j]];
                    int p3 = Vertices.PolichedralAtoms[a[j+1]];
                    float lenght1 = Vector3.Distance(molecule.atom[p1].Coordinate, molecule.atom[p2].Coordinate);
                    float lenght2 = Vector3.Distance(molecule.atom[p1].Coordinate, molecule.atom[p3].Coordinate);
                    if (lenght1 > lenght2)
                    {
                        int temp = a[j];
                        a[j] = a[j + 1];
                        a[j + 1] = temp;
                    }
                }
       
  
            int f = a[0];
            int[,] Strategy =
            {
                {a[0],a[1],a[3] },
                {a[0],a[1],a[4] },
                {a[0],a[2],a[3] },
                {a[0],a[2],a[4] },
                {a[1],a[2],a[4] },
                {a[3],a[2],a[1] },
            };

            Primitive_struct.vertices_n = new VertexPositionNormalTexture[Strategy.Length / 3 * 3];
            Primitive_struct.vertices_s = new VertexPositionNormalTexture[Strategy.Length / 3 * 3];

            AsignTringles(Vertices, ref Primitive_struct, Strategy);
        }
        static void FindTringle6(PolihedralsStruct Vertices, ref VertexStruct Primitive_struct)
        {
            int[] a = new int[Vertices.PolichedralAtoms.Count];

            for (int i = 0; i < Vertices.PolichedralAtoms.Count; i++)
            {
                a[i] = i;
            }
            int top = FindTop(Vertices); //vertice 0

            for (int j = 0; j < Vertices.PolichedralAtoms.Count; j++)
            {
                if (top == a[j])
                {
                    int temp = a[0];
                    a[0] = top;
                    a[j] = temp;
                    break;
                }
            }
            for (int j = 1; j < Vertices.PolichedralAtoms.Count - 1; j++)
                for (int i = 1; i < Vertices.PolichedralAtoms.Count; i++)
                {
                    int p1 = Vertices.PolichedralAtoms[a[i]];
                    int p2 = Vertices.PolichedralAtoms[a[j]];
                    int p3 = Vertices.PolichedralAtoms[a[j + 1]];
                    float lenght1 = Vector3.Distance(molecule.atom[p1].Coordinate, molecule.atom[p2].Coordinate);
                    float lenght2 = Vector3.Distance(molecule.atom[p1].Coordinate, molecule.atom[p3].Coordinate);
                    if (lenght1 > lenght2)
                    {
                        int temp = a[j];
                        a[j] = a[j + 1];
                        a[j + 1] = temp;
                    }
                }

            int[,] Strategy =
            {
                {a[0], a[1], a[2] },
                {a[0], a[2], a[3] },
                {a[0], a[3], a[4] },
                {a[0], a[4], a[1] },

                {a[5], a[1], a[2] },
                {a[5], a[2], a[3] },
                {a[5], a[3], a[4] },
                {a[5], a[4], a[1] },
            };
            Primitive_struct.vertices_n = new VertexPositionNormalTexture[Strategy.Length / 3 * 3];
            Primitive_struct.vertices_s = new VertexPositionNormalTexture[Strategy.Length / 3 * 3];

            AsignTringles(Vertices, ref Primitive_struct, Strategy);
        }



        static int FindTop(PolihedralsStruct Vertices)
        {

            List<float> ave_lenght = new List<float>();
            //first find the top vertex
            for (int i = 0; i < Vertices.PolichedralAtoms.Count; i++)
            {
                float lenght = 0;
                for (int j = 0; j < Vertices.PolichedralAtoms.Count; j++)
                {
                    if (j != i)
                    {
                        int p1 = Vertices.PolichedralAtoms[i];
                        int p2 = Vertices.PolichedralAtoms[j];
                        lenght += Vector3.Distance(molecule.atom[p1].Coordinate, molecule.atom[p2].Coordinate);
                    }
                }
                ave_lenght.Add(lenght / Vertices.PolichedralAtoms.Count);
            }

             float[] sum = new float[Vertices.PolichedralAtoms.Count];
            //We have all average for all vectors from vertice so check if there is any special one
            for (int i = 0; i < Vertices.PolichedralAtoms.Count; i++)
            {
                sum[i] = 0;
                for (int j = 0; j < Vertices.PolichedralAtoms.Count; j++)
                {
                    int p1 = Vertices.PolichedralAtoms[i];
                    int p2 = Vertices.PolichedralAtoms[j];
                    float lenght = Vector3.Distance(molecule.atom[p1].Coordinate, molecule.atom[p2].Coordinate);

                    sum[i] += ((lenght - ave_lenght[i]) * (lenght - ave_lenght[i])) / (ave_lenght[i] * ave_lenght[i]);
                }

            }
            int res = 0;
            float min = 1000;
            for (int i = 0; i < Vertices.PolichedralAtoms.Count; i++)
                if (min > sum[i])
                {
                    res = i;
                    min = sum[i];
                }
            return res;
        }

        static void AsignTringles(PolihedralsStruct Vertices, ref VertexStruct Primitive_struct, int[,] Strategy)
        {

            Vector3 Vref = Primitive_struct.position;
            int vertex_nr;
            

            for (int k = 0; k < Strategy.Length/3; k++)
            {
                int k3 = 3 * k;
                vertex_nr = Vertices.PolichedralAtoms[Strategy[k, 0]];
                Vector3 pos1 = Primitive_struct.vertices_n[k3 + 0].Position = molecule.atom[vertex_nr].Coordinate - Vref;
                vertex_nr = Vertices.PolichedralAtoms[Strategy[k, 1]];
                Vector3 pos2 = Primitive_struct.vertices_n[k3 + 1].Position = molecule.atom[vertex_nr].Coordinate - Vref;
                vertex_nr = Vertices.PolichedralAtoms[Strategy[k, 2]];
                Vector3 pos3 = Primitive_struct.vertices_n[k3 + 2].Position = molecule.atom[vertex_nr].Coordinate - Vref;

                Primitive_struct.vertices_n[k3 + 0].Normal =
                Primitive_struct.vertices_n[k3 + 1].Normal =
                Primitive_struct.vertices_n[k3 + 2].Normal = CalculateNormal(pos1, pos2, pos3);

                Primitive_struct.vertices_n[k3 + 0].TextureCoordinate =
                Primitive_struct.vertices_n[k3 + 1].TextureCoordinate =
                Primitive_struct.vertices_n[k3 + 2].TextureCoordinate = Vector2.Zero;
            }

            for (int k = 0; k < Strategy.Length/3; k++)
            {
                int k3 = 3 * k;

                Primitive_struct.vertices_s[k3 + 0].Position = Primitive_struct.vertices_n[k3 + 0].Position;
                Primitive_struct.vertices_s[k3 + 1].Position = Primitive_struct.vertices_n[k3 + 1].Position;
                Primitive_struct.vertices_s[k3 + 2].Position = Primitive_struct.vertices_n[k3 + 2].Position;
                Primitive_struct.vertices_s[k3 + 0].Normal =
                Primitive_struct.vertices_s[k3 + 1].Normal =
                Primitive_struct.vertices_s[k3 + 2].Normal = -Primitive_struct.vertices_n[k3 + 0].Normal;//normal correction for space groups transformations

                Primitive_struct.vertices_s[k3 + 0].TextureCoordinate =
                Primitive_struct.vertices_s[k3 + 1].TextureCoordinate =
                Primitive_struct.vertices_s[k3 + 2].TextureCoordinate = Vector2.Zero;
            }
        }

        static Vector3 CalculateNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 vectorA = v1 - v2;
            Vector3 vectorB = v1 - v3;
            return -Vector3.Normalize(Vector3.Cross(vectorB, vectorA));
        }

 
        struct Pairs
        {
            public int v1, v2, v3;
            public Vector3 V1;
            public Vector3 V2;
            public Vector3 V3;
            public bool remove;
        }
        static int[,] FindStrategy(PolihedralsStruct Vertices, ref VertexStruct Primitive_struct)
        {
            //Find pairs of vertices
            List<Pairs> pairs = new List<Pairs>();
            //find all triangles
            List<Pairs> pairs_Local = new List<Pairs>();

            for (int i = 0; i < Vertices.PolichedralAtoms.Count; i++)
            {
                for (int j = 0; j < Vertices.PolichedralAtoms.Count; j++)
                {
                    for (int k = 0; k < Vertices.PolichedralAtoms.Count; k++)
                    {
                        if (i != j & j != k && k != i)
                        {
                            Pairs temp_pair = new Pairs();
                            temp_pair.v1 = i;
                            temp_pair.v2 = j;
                            temp_pair.v3 = k;
                            int atom_nr = Vertices.PolichedralAtoms[i];
                            temp_pair.V1 = molecule.atom[atom_nr].Coordinate;
                            atom_nr = Vertices.PolichedralAtoms[j];
                            temp_pair.V2 = molecule.atom[atom_nr].Coordinate;
                            atom_nr = Vertices.PolichedralAtoms[k];
                            temp_pair.V3 = molecule.atom[atom_nr].Coordinate;
                            // temp_pair.distance = (int)(1000 * Vector3.Distance(temp_pair.V1, temp_pair.V2));
                            temp_pair.remove = false;
                            pairs_Local.Add(temp_pair);
                        }
                    }
                    //Select only 3 the shortest pairs from the list of pairs
                    //.Sort((x, y) => x.v1 - y.v1;
                    //if (pairs_Local.Count > 3)
                    //    pairs_Local.RemoveRange(3, pairs_Local.Count - 3);


                    // pairs.AddRange(pairs_Local);
                }
            }



            List<Pairs> pairs_clean = new List<Pairs>();
            //Remove duplicats


            for (int i = 0; i < pairs_Local.Count; i++)
            {
                bool reject = false;
                for (int j = i + 1; j < pairs_Local.Count; j++)
                {
                    if (compareVertices(pairs_Local[i], pairs_Local[j]))
                    {
                        reject = true;
                        break;
                    }
                }
                if (!reject)
                    pairs_clean.Add(pairs_Local[i]);
            }





            //pairs_clean.Sort((x, y) => x.v1 - y.v1);


            int[,] Strategy = new int[pairs_clean.Count, 3];

            for (int i = 0; i < pairs_clean.Count; i++)
            {
                Strategy[i, 0] = pairs_clean[i].v1;
                Strategy[i, 1] = pairs_clean[i].v2;
                Strategy[i, 2] = pairs_clean[i].v3;
            }
            return Strategy;
        }

        static bool compareVertices(Pairs p1, Pairs p2)
        {


            int va1 = p1.v1;
            int va2 = p1.v2;
            int va3 = p1.v3;


            int vb1 = p2.v1;
            int vb2 = p2.v2;
            int vb3 = p2.v3;

            //permutacje
            if (va1 == vb1 && va2 == vb2 && va3 == vb3) return true;
            if (va1 == vb1 && va2 == vb3 && va3 == vb2) return true;
            if (va1 == vb2 && va2 == vb3 && va3 == vb1) return true;
            if (va1 == vb2 && va2 == vb1 && va3 == vb3) return true;
            if (va1 == vb3 && va2 == vb1 && va2 == vb2) return true;
            if (va1 == vb3 && va2 == vb2 && va2 == vb1) return true;

            return false;
        }


    }
}