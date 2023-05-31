//***********************************************************************
//                        CIF program
//                Created by Daniel M. Kaminski
//                        Lublin 2023
//                     Under GNU licence
//***********************************************************************

using cif;
using Microsoft.Xna.Framework;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Cif.SymmetryElements;
//https://stackoverflow.com/questions/62782648/android-11-scoped-storage-permissions
namespace Cif
{
    public class object3d
    {
        public Vector3 position = new Vector3();
        public float AngleZ;
        public float AngleXY;
        public float distance;
        public Vector3 color;
    }
    public class Molecule
    {
        public static List<AtomSimple> AsymetricPart = new List<AtomSimple>();//atoms expanded by the symmetry elements
        public static List<AtomSimple> SymetryEquivalents = new List<AtomSimple>();//atoms expanded by the symmetry elements
        public static List<object3d> UnitCellFrame = new List<object3d>();//atoms expanded by the symmetry elements

        public static string error_message = "";
        //https://crystalmaker.com/support/tutorials/atomic-radii/
        #region AtomRadius:  


        public float[,] AtomRadius =
            {
            //Van-der-Waals		ionic	
                {0      ,0       },		//empty 	
                {   0.9f,   0.25f},	//	1	H
                {   1.4f,   0.31f},	//	2	He
                {   1.82f,  1.45f},	//	3	Li
                {   1.86f,  1.05f},	//	4	Be
                {   1.74f,  0.85f},	//	5	B
                {   1.70f,   0.7f},	//	6	C
                {   1.55f,  0.65f},	//	7	N
                {   1.52f,  0.6f},	//	8	O
                {   1.47f,  0.5f},	//	9   F
                {   1.54f,  0.38f},	//	10	Ne
                {   2.27f,  1.8f},	//	11	Na
                {   1.73f,  1.5f},	//	12	Mg
                {   2.11f,  1.25f},	//	13	Al
                {   2.1f,   1.1f},	//	14	Si
                {   1.8f,   1f},	//	15	P
                {   1.8f,   1f},	//	16	S
                {   1.89f,  1f},	//	17	Cl
                {   1.88f,  0.71f},	//	18	Ar
                {   2.75f,  2.2f},	//	19	K
                {   2.61f,  1.8f},	//	20	Ca
                {   2.28f,  1.6f},	//	21	Sc
                {   2.14f,  1.4f},	//	22	Ti
                {   2.03f,  1.35f},	//	23	V
                {   1.97f,  1.4f},	//	24	Cr
                {   1.96f,  1.4f},	//	25	Mn
                {   1.96f,  1.4f},	//	26fe
                {   1.95f,  1.35f},	//	27	Co
                {   1.63f,  1.35f},	//	28	Ni
                {   1.4f,   1.35f},	//	29	Cu
                {   1.39f,  1.35f},	//	30	Zn
                {   1.87f,  1.3f},	//	31	Ga
                {   2.13f,  1.25f},	//	32	Ge
                {   1.85f,  1.15f},	//	33	As
                {   1.9f,   1.15f},	//	34	Se
                {   1.85f,  1.15f},	//	35	Br
                {   2.02f,  0.88f},	//	36	Kr
                {   3.12f,  2.35f},	//	37	Rb
                {   2.45f,  2f},	//	38	Sr
                {   2.45f,  1.85f},	//	39	Y
                {   2.25f,  1.55f},	//	40	Zr
                {   2.13f,  1.45f},	//	41	Nb
                {   2.06f,  1.45f},	//	42	Mo
                {   2.04f,  1.35f},	//	43	Tc
                {   2.02f,  1.3f},	//	44	Ru
                {   2.02f,  1.35f},	//	45	Rh
                {   1.63f,  1.4f},	//	46	Pd
                {   1.72f,  1.6f},	//	47	Ag
                {   1.58f,  1.55f},	//	48	Cd
                {   1.93f,  1.55f},	//	49	In
                {   2.2f,   1.45f},	//	50	Sn
                {   2.33f,  1.45f},	//	51	Sb
                {   2.06f,  1.4f},	//	52	Te
                {   1.98f,  1.4f},	//	53	I
                {   2.16f,  1.08f},	//	54	Xe
                {   3.31f,  2.6f},	//	55	Cs
                {   2.85f,  2.15f},	//	56	Ba
                {   2.51f,  1.95f},	//	57	La
                {   3.31f,  1.85f},	//	58	Ce
                {   2.2f,   1.85f},	//	59	Pr
                {   2.45f,  1.85f},	//	60	Nd
                {   2.47f,  1.85f},	//	61	Pm
                {   2.2f,   1.85f},	//	62	Sm
                {   2.2f,   1.85f},	//	63	Eu
                {   2.2f,   1.8f},	//	64	Gd
                {   2.2f,   1.75f},	//	65	Tb
                {   2.45f,  1.75f},	//	66	Dy
                {   2.2f,   1.75f},	//	67	Ho
                {   2.2f,   1.75f},	//	68	Er
                {   2.2f,   1.75f},	//	69	Tm
                {   2.2f,   1.75f},	//	70	Yb
                {   2.2f,   1.75f},	//	71	Lu
                {   2.2f,   1.55f},	//	72	Hf
                {   2.2f,   1.45f},	//	73	Ta
                {   2.2f,   1.35f},	//	74	W
                {   2.2f,   1.35f},	//	75	Re
                {   2.2f,   1.3f},	//	76	Os
                {   2.03f,  1.35f},	//	77	Ir
                {   1.75f,  1.35f},	//	78	Pt
                {   1.69f,  1.35f},	//	79	Au
                {   1.55f,  1.5f},	//	80	Hg
                {   1.96f,  1.9f},	//	81	Tl
                {   2.02f,  1.8f},	//	82	Pb
                {   2.42f,  1.6f},	//	83	Bi
                {   2.2f,   1.9f},	//	84	Po
                {   2.2f,   1.27f},	//	85	At
                {   2.2f,   1.2f},	//	86	Rn
                {   2.2f,   2.6f},	//	87  Fr
                {   2.2f,   2.15f},	//	88	Ra
                {   2.2f,   1.95f},	//	89	Ac
                {   2.2f,   1.8f},	//	90	Th
                {   2.2f,   1.8f},	//	91	Pa
                {   1.86f,  1.75f},	//	92	U
                {   1.8f,   1.75f},	//	93	Np
                {   1.8f,   1.75f},	//	94	Pu
                {   1.8f,   1.75f},	//	95	Am
                };


        #endregion

        #region atom color:
        public Vector3[] color = {//0
                             new Vector3(.6f, .6f, .6f), //0 

                             //I
                             new Vector3(.7f, .7f, .7f), //1 H
                             new Vector3(.6f, .6f, .6f), //2 He
                             new Vector3(.6f, .6f, .6f), //3 Li                       
                             new Vector3(.6f, .6f, .6f), //4 Be                        
                             new Vector3(.6f, .6f, .6f), //5 B
                             new Vector3(.3f, .3f, .3f), //6 C
                             new Vector3(.0f, .0f, .9f), //7 N
                             new Vector3(.9f, .0f, .0f), //8 O
                             new Vector3(.6f, .3f, 1.0f), //9 F
                             new Vector3(.6f, .6f, .6f), //10 Ne
                 
                             //II           
                             new Vector3(.7f, .7f, .7f), //11 Na
                             new Vector3(.5f, .5f, .5f), //12 Mg
                             new Vector3(.8f, .6f, .0f), //13 Al                      
                             new Vector3(.6f, .3f, .6f), //14 Si                        
                             new Vector3(.1f, .6f, .5f), //55 P
                             new Vector3(.8f, .8f, .0f), //16 S
                             new Vector3(.0f, 1f, .0f), //17 Cl
                             new Vector3(.9f, .0f, .0f), //18 Ar
                             
                               //III
                             new Vector3(.6f, .6f, .6f), //19 K
                             new Vector3(.6f, .6f, .6f), //0
                             new Vector3(.7f, .7f, .7f), //1
                             new Vector3(.6f, .6f, .6f), //2
                             new Vector3(.6f, .4f, .6f), //3                        
                             new Vector3(.4f, .6f, .6f), //4                         
                             new Vector3(.6f, .6f, .3f), //5
                             new Vector3(.3f, .3f, .3f), //6
                             new Vector3(.2f, .0f, .9f), //7
                             new Vector3(.9f, .0f, .0f), //8
                             new Vector3(.6f, .6f, .6f), //9
                             //3
                             new Vector3(.6f, .6f, .6f), //0
                             new Vector3(.7f, .7f, .7f), //1
                             new Vector3(.6f, .3f, .6f), //2
                             new Vector3(.6f, .6f, .6f), //3                        
                             new Vector3(.6f, .6f, .6f), //4                         
                             new Vector3(.6f, .6f, .7f), //5
                             new Vector3(.3f, .3f, .3f), //6
                             new Vector3(.0f, .0f, .9f), //7
                             new Vector3(.9f, .0f, .0f), //8
                             new Vector3(.6f, .8f, .6f), //9
                             //4
                             new Vector3(.6f, .6f, .6f), //0
                             new Vector3(.7f, .7f, .7f), //1
                             new Vector3(.6f, .6f, .6f), //2
                             new Vector3(.6f, .6f, .6f), //3                        
                             new Vector3(.6f, .6f, .6f), //4                         
                             new Vector3(.6f, .6f, .6f), //5
                             new Vector3(.3f, .3f, .3f), //6
                             new Vector3(.0f, .0f, .9f), //7
                             new Vector3(.9f, .0f, .0f), //8
                             new Vector3(.6f, .6f, .6f), //9
                             //5
                             new Vector3(.6f, .0f, .0f), //0
                             new Vector3(.7f, .0f, .3f), //1
                             new Vector3(.4f, .2f, .6f), //2
                             new Vector3(.0f, .0f, .6f), //3                        
                             new Vector3(.1f, .0f, .6f), //4                         
                             new Vector3(.2f, .0f, .6f), //5
                             new Vector3(.4f, .3f, .3f), //6
                             new Vector3(.4f, .4f, .9f), //7
                             new Vector3(.4f, .5f, .0f), //8
                             new Vector3(.4f, .6f, .6f), //9
                             //6
                             new Vector3(.6f, .0f, .0f), //0
                             new Vector3(.7f, .0f, .3f), //1
                             new Vector3(.4f, .0f, .6f), //2
                             new Vector3(.4f, .0f, .6f), //3                        
                             new Vector3(.4f, .0f, .6f), //4                         
                             new Vector3(.4f, .0f, .6f), //5
                             new Vector3(.4f, .0f, .3f), //6
                             new Vector3(.4f, .0f, .9f), //7
                             new Vector3(.4f, .0f, .0f), //8
                             new Vector3(.4f, .0f, .6f), //9
                             //7
                             new Vector3(.6f, .0f, .0f), //0
                             new Vector3(.7f, .5f, .3f), //1
                             new Vector3(.4f, .0f, .6f), //2
                             new Vector3(.4f, .0f, .6f), //3                        
                             new Vector3(.4f, .0f, .6f), //4                         
                             new Vector3(.4f, .0f, .6f), //5
                             new Vector3(.4f, .0f, .3f), //6
                             new Vector3(.4f, .0f, .9f), //7
                             new Vector3(.4f, .0f, .0f), //8
                             new Vector3(.4f, .0f, .6f), //9
                          };
        #endregion

        #region element:
        public string[] elements =
        {
        " ","H","He","Li","Be","B","C","N","O","F","Ne","Na","Mg",
        "Al","Si","P","S","Cl","Ar","K","Ca","Sc","Ti","V","Cr",
        "Mn","Fe","Co","Ni","Cu","Zn","Ga","Ge","As","Se","Br",
        "Kr","Rb","Sr","Y","Zr","Nb","Mo","Tc","Ru","Rh","Pd",
        "Ag","Cd","In","Sn","Sb","Te","I","Xe","Cs","Ba","La",
        "Ce","Pr","Nd","Pm","Sm","Eu","Gd","Tb","Dy","Ho","Er",
        "Tm","Yb","Lu","Hf","Ta","W","Re","Os","Ir","Pt","Au",
        "Hg","Tl","Pb","Bi","Po","At","Rn","Fr","Ra","Ac","Th",
        "Pa","U","Np","Pu","Am","Cm","Bk","Cf"
        };
        #endregion


        public Vector3 MoleculeShift;
        public float Volume;

        //public string name;
        public int NumberOfAtoms;
        public int NumberSelected;  //The number of selected atoms

        public float a, b, c, alpha, beta, gamma;
        public struct AtomSimple
        {
            public string name;
            public int type;
            public float x, y, z;
            public float x_e, y_e, z_e;
            public Matrix Ue;
            public Matrix Un;

            public float occupancy;
            public Matrix Transformation;
            public int MoleculeNumber;
            public int disorder_group;
        }

        public struct Atom
        {
            public string name;
            public int style;
            public int AtomicNumber;
            public bool selected;  //is selected or not
            public int SelectionOrder;
            public bool show;   //show or not
            public Vector3 Coordinate;
            public Vector3 Coordinate_error;
            public int Z_Depth;
            public int disorder_group;

            //Normal bonds
            public int NrBondedAtoms;
            public float[] Distance;
            public float[] BondOccupancy;
            public int[] BondedAtom;
            public float[] AngleZ;
            public float[] AngleXY;

            //Hydrogen bonds
            public int H_NrBondedAtoms;
            public float[] H_Distance;
            public int[] H_BondedAtom;
            public float[] H_AngleZ;
            public float[] H_AngleXY;

            //thermal elipsoid
            public Matrix Ue;
            public Matrix Un;

            public float occupancy;
            public Matrix Transformation;
            public int MoleculeNumber;
        }

        public Atom[] atom;




        //-------------------------------------------------------------------------------------------------------------------------------------
        //Calculate number of molecules and add coordinates
        //-------------------------------------------------------------------------------------------------------------------------------------
        public async Task<int> InitializeAsync(bool action = false)
        {
            Cif.Clean();

            string str = "";




            if (!action)
            {
                //#region Permisions and read
                //try
                //{
                //    //storage
                //    var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                //    if (status != PermissionStatus.Granted)
                //    {
                //        error_message = "Set permision to the storage access memory.";

                //        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                //        {

                //        }
                //        //status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();

                //         status = await CrossPermissions.Current.RequestPermissionAsync <StoragePermission>();
                //    }
                //    else
                //        error_message = "";
                //}
                //catch (Exception ex)
                //{
                //    System.Console.WriteLine("Something went wrong: " + ex.Message);
                //}
                //#endregion

                try
                {
                    var fileTypes = new string[] { ".cif", ".res" };


                    FileData fileData = await CrossFilePicker.Current.PickFile();

                    if (fileData == null)
                        return 0; // user canceled file picking

                    Cif.FlieName = fileData.FileName;
                    str = System.Text.Encoding.Default.GetString(fileData.DataArray);
                }
                catch (Exception ex)
                {
                    error_message = "Exception choosing file: " + ex.ToString();
                    return 0;
                }


                //Translate to cif
            }
            else
            {
                Cif.FlieName = Examples.title;
                str = Examples.example1;
            }
            Read.Read_data(str);

            //Now all data are in Cif class
            a = Cif.a;
            b = Cif.b;
            c = Cif.c;
            alpha = MathHelper.ToRadians(Cif.alp);
            beta = MathHelper.ToRadians(Cif.bet);
            gamma = MathHelper.ToRadians(Cif.gam);



            //The number in base 
            //if no symmetry number (0 on input) then use the symmetry equivalents
            //If symmetry operations available always use them!!!

            SymmetryElements.CreateTransformations(Cif.space_group_IT_number);
            SymetryEquivalents.Clear();

            AsignXYZ();

            //Do the math 0- asymetric, 3 termal elipsoids
            Presentation(Flags.GenerateEquvalents, Flags.Presentation);


            //just in case
            Cif.cell_volume = 1.0f * ((int)(Volume * 100)) / 100;

            // TestMatrix();

            return NumberOfAtoms;
        }


        void MoveAtomsToCell()
        {
            List<AtomSimple> TempStorage = new List<AtomSimple>();

            for (int i = 0; i < SymetryEquivalents.Count; i++)
            {
                AtomSimple TempAtom = new AtomSimple();
                TempAtom = SymetryEquivalents[i];

                if (TempAtom.x > 1)
                    TempAtom.x--;
                if (TempAtom.y > 1)
                    TempAtom.y--;
                if (TempAtom.z > 1)
                    TempAtom.z--;

                if (TempAtom.x < 0)
                    TempAtom.x++;
                if (TempAtom.y < 0)
                    TempAtom.y++;
                if (TempAtom.z < 0)
                    TempAtom.z++;

                TempStorage.Add(TempAtom);

            }
            SymetryEquivalents.Clear();

            SymetryEquivalents.AddRange(TempStorage);
        }

        public void Presentation(int p, int style)
        {
            //Asymetric part atoms in fractional coordinates
            //return atoms in [Atoms] AsymetricPart 
            CalculateSymmetryEquivalents();

            //if (flag_move_to_cell)
            //MoveAtomsToCell();
            CenterMolecules3D();

            switch (p)
            {
                case 0:
                    NumberOfAtoms = Calculate_SymmetryAtoms3D(1, 1, 1, AsymetricPart, style);
                    break;
                case 1:
                    NumberOfAtoms = Calculate_SymmetryAtoms3D(Flags.repeat_x, Flags.repeat_y, Flags.repeat_z, SymetryEquivalents, style);
                    break;
                default:
                    NumberOfAtoms = Calculate_SymmetryAtoms3D(1, 1, 1, AsymetricPart, style);
                    break;
            }
            //shift atoms to the center of screen
            CenterAtoms3D();

            //find bonds
            Calculate_Bonds3D();
            Calculate_Hbonds3D();

            PrepareUnitCell();
        }

        void Calculate_Bonds3D()
        {
            int n;

            for (int i = 0; i < NumberOfAtoms; i++)
            {
                n = 0;
                for (int j = 0; j < NumberOfAtoms; j++)
                {
                    float distance = (float)System.Math.Sqrt(sqr(atom[i].Coordinate.X - atom[j].Coordinate.X) +
                                                        sqr(atom[i].Coordinate.Y - atom[j].Coordinate.Y) +
                                                        sqr(atom[i].Coordinate.Z - atom[j].Coordinate.Z));

                    //Hydrogen correction
                    float bond_corr = Flags.BOND_FACTOR;
                    int nu = atom[i].AtomicNumber;
                    if (nu == 3 || (nu >= 11 && nu <= 14) || (nu >= 19 && nu <= 30) || (nu >= 37 && nu <= 50) || (nu >= 55 && nu <= 83) || (nu >= 87 && nu <= 118))
                        bond_corr = Flags.BOND_FACTOR_METALS;
                    nu = atom[j].AtomicNumber;
                    if (nu == 3 || (nu >= 11 && nu <= 14) || (nu >= 19 && nu <= 30) || (nu >= 37 && nu <= 50) || (nu >= 55 && nu <= 83) || (nu >= 87 && nu <= 118))
                        bond_corr = Flags.BOND_FACTOR_METALS;

                    //disorder group
                    if (atom[i].disorder_group == atom[j].disorder_group || atom[i].disorder_group == 0 || atom[j].disorder_group == 0)
                        if (i != j && distance < Flags.MAX_ATOM_DISTANCE && distance < (AtomRadius[atom[i].AtomicNumber, Flags.AtomicRadiiType] + AtomRadius[atom[j].AtomicNumber, Flags.AtomicRadiiType]) * bond_corr)
                        {
                            float bond_occ = atom[j].occupancy;
                            if (atom[j].occupancy < bond_occ) bond_occ = atom[j].occupancy;
                            atom[i].BondOccupancy[n] = bond_occ;
                            atom[i].Distance[n] = distance;
                            atom[i].BondedAtom[n] = j;
                            atom[i].AngleZ[n] = (float)(System.Math.Asin((atom[i].Coordinate.Z - atom[j].Coordinate.Z) / distance));

                            if ((atom[i].Coordinate.X - atom[j].Coordinate.X) == 0.0f)
                            {
                                atom[i].AngleXY[n] = 0;
                                n--;
                            }
                            else
                                atom[i].AngleXY[n] = -(float)(MathHelper.ToRadians(180) - System.Math.Atan2((atom[i].Coordinate.Y - atom[j].Coordinate.Y), (atom[i].Coordinate.X - atom[j].Coordinate.X)));

                            n++;
                            if (n >= Flags.MAX_BONDED_ATOMS) break;
                        }
                }
                atom[i].NrBondedAtoms = n;
            }
        }


        void Calculate_Hbonds3D()
        {
            int n;
            float bond_corr = Flags.BOND_FACTOR;

            for (int i = 0; i < NumberOfAtoms; i++)
            {
                n = 0;
                for (int j = 0; j < NumberOfAtoms; j++)
                {
                    int nu = atom[i].AtomicNumber;
                    int mu = atom[j].AtomicNumber;
                    if (i != j)
                        if (nu == 1 && (mu == 7 || mu == 8))
                        {
                            for (int k = 0; k < atom[i].NrBondedAtoms; k++)
                            {
                                //find H bonded only to N and O
                                int bonded_atom = atom[i].BondedAtom[k];
                                if (atom[bonded_atom].AtomicNumber == 7 || atom[bonded_atom].AtomicNumber == 8)
                                {
                                    float distance = Vector3.Distance(atom[i].Coordinate, atom[j].Coordinate);
                                    float radii = (AtomRadius[atom[i].AtomicNumber, Flags.AtomicRadiiType] + AtomRadius[atom[j].AtomicNumber, Flags.AtomicRadiiType]) * bond_corr;

                                    if (distance < Flags.MAX_H_ATOM_DISTANCE && distance > radii)
                                    {
                                        //atom bonded to hydrogen [O,N] - hydrogen donor
                                        //Hydrogen atom[i]
                                        //atom[j] -hydrogen acceptor
                                        float angle = Angle(atom[bonded_atom].Coordinate, atom[i].Coordinate, atom[j].Coordinate);
                                        if (angle > 180f - Flags.H_acceptance_angle && angle < 180f + Flags.H_acceptance_angle)
                                        {
                                            atom[i].H_Distance[n] = distance * 2;
                                            atom[i].H_BondedAtom[n] = j;
                                            atom[i].H_AngleZ[n] = (float)(System.Math.Asin((atom[i].Coordinate.Z - atom[j].Coordinate.Z) / distance));

                                            atom[i].H_AngleXY[n] = -(float)(MathHelper.ToRadians(180) - System.Math.Atan2((atom[i].Coordinate.Y - atom[j].Coordinate.Y), (atom[i].Coordinate.X - atom[j].Coordinate.X)));

                                            n++;
                                            if (n >= Flags.MAX_H_BONDED_ATOMS) break;
                                        }
                                    }

                                    atom[i].H_NrBondedAtoms = n;
                                }
                            }
                        }
                }
            }
        }

        float sqr(float x) { return (x * x); }

        //--------------------------------------------------------------------------------------------------------------------------------
        //
        //  "Drawing bulk" calculate cartesian coordinates of atoms and write them to structure  
        //
        //--------------------------------------------------------------------------------------------------------------------------------

        int Calculate_SymmetryAtoms3D(int repeatx, int repeaty, int repeatz, List<AtomSimple> data, int style)
        { // Drowing the xy plane cartesian coord.

            Matrix U = Matrix.Transpose(FractionaltoCarttesianMatrix(0, 0, 0));
            atom = new Atom[data.Count * repeatx * repeaty * repeatz + 1];
            NumberOfAtoms = 0;
            for (int i = 0; i < data.Count * repeatx * repeaty * repeatz + 1; i++)
            {
                atom[i].show = true;
                atom[i].Distance = new float[Flags.MAX_BONDED_ATOMS];
                atom[i].BondOccupancy = new float[Flags.MAX_BONDED_ATOMS];
                atom[i].AngleZ = new float[Flags.MAX_BONDED_ATOMS];
                atom[i].AngleXY = new float[Flags.MAX_BONDED_ATOMS];
                atom[i].BondedAtom = new int[Flags.MAX_BONDED_ATOMS];

                atom[i].H_Distance = new float[Flags.MAX_H_BONDED_ATOMS];
                atom[i].H_AngleZ = new float[Flags.MAX_H_BONDED_ATOMS];
                atom[i].H_AngleXY = new float[Flags.MAX_H_BONDED_ATOMS];
                atom[i].H_BondedAtom = new int[Flags.MAX_H_BONDED_ATOMS];

            }

            int n;
            int repeatx_, repeaty_, repeatz_;
            int repeatz_start, repeatz_end;
            int repeatx_start, repeatx_end;
            int repeaty_start, repeaty_end;
            Matrix ident = Matrix.Identity;

            if (repeatx > 0)
            {
                repeatx_start = 0;
                repeatx_end = repeatx;
            }
            else
            {
                repeatx_start = repeatx + 1;
                repeatx_end = 1;
            }

            if (repeaty > 0)
            {
                repeaty_start = 0;
                repeaty_end = repeaty;
            }
            else
            {
                repeaty_start = repeaty + 1;
                repeaty_end = 1;
            }

            if (repeatz > 0)
            {
                repeatz_start = 0;
                repeatz_end = repeatz;
            }
            else
            {
                repeatz_start = repeatz;
                repeatz_end = 1;
            }


            for (repeatz_ = repeatz_start; repeatz_ < repeatz_end; repeatz_++)
            {
                for (repeaty_ = repeaty_start; repeaty_ < repeaty_end; repeaty_++)
                {
                    for (repeatx_ = repeatx_start; repeatx_ < repeatx_end; repeatx_++)
                    {
                        for (n = 0; n < data.Count; n++)
                        {
                            // Ploting all atoms in the cell
                            //atom[NumberOfAtoms].Coordinate.X = (float)((data[n].x + repeatx_) * a + (data[n].y + repeaty_) * b * System.Math.Cos(gamma) + (data[n].z + repeatz_) * c * System.Math.Cos(alpha));
                            //atom[NumberOfAtoms].Coordinate.Y = (float)((data[n].y + repeaty_) * b * System.Math.Sin(gamma) + (data[n].z + repeatz_) * c * System.Math.Cos(beta));
                            //atom[NumberOfAtoms].Coordinate.Z = (float)((data[n].z + repeatz_) * c);
                            Vector3 shift = new Vector3(data[n].x + repeatx_, data[n].y + repeaty_, data[n].z + repeatz_);
                            atom[NumberOfAtoms].Coordinate = Vector3.Transform(shift, U);

                            atom[NumberOfAtoms].Coordinate_error.X = data[n].x_e;
                            atom[NumberOfAtoms].Coordinate_error.Y = data[n].y_e;
                            atom[NumberOfAtoms].Coordinate_error.Z = data[n].z_e;

                            atom[NumberOfAtoms].AtomicNumber = data[n].type;
                            atom[NumberOfAtoms].Ue = data[n].Ue;
                            atom[NumberOfAtoms].Un = data[n].Un;

                            atom[NumberOfAtoms].occupancy = data[n].occupancy;
                            atom[NumberOfAtoms].style = style;//display_mode
                            atom[NumberOfAtoms].name = data[n].name;
                            atom[NumberOfAtoms].Transformation = data[n].Transformation;
                            atom[NumberOfAtoms].disorder_group = data[n].disorder_group;
                            NumberOfAtoms++;
                        }
                    }
                }
            }
            return NumberOfAtoms;
        }


        //--------------------------------------------------------------------------------------------------------------------------------

        void AsignXYZ()
        {
            Matrix Amatrix = FractionaltoCarttesianMatrix(0, 0, 0);
            Matrix Nmatrix = TransformUStatToU();

            //Clean the list            
            AsymetricPart.Clear();

            for (int i = 0; i < Cif.atom.Count; i++)
            {
                AtomSimple atom = new AtomSimple();
                if (Cif.atom[i].iso < 0.05f)
                    Cif.atom[i].iso = 0.05f;

                atom.name = Cif.atom[i].name;
                atom.type = Cif.atom[i].type;
                atom.x = Cif.atom[i].x;
                atom.y = Cif.atom[i].y;
                atom.z = Cif.atom[i].z;
                atom.x_e = Cif.atom[i].x_e;
                atom.y_e = Cif.atom[i].y_e;
                atom.z_e = Cif.atom[i].z_e;
                CalculateError(ref atom.x_e, ref atom.y_e, ref atom.z_e);

                atom.occupancy = Cif.atom[i].occ;
                atom.Transformation = Matrix.Identity;
                atom.disorder_group = Cif.atom[i].disorder_group;

                if (Cif.atom[i].adp_type == "Uani")
                {
                    atom.Ue.M11 = Cif.atom[i].u11;
                    atom.Ue.M22 = Cif.atom[i].u22;
                    atom.Ue.M33 = Cif.atom[i].u33;
                    atom.Ue.M23 = atom.Ue.M32 = Cif.atom[i].u23;
                    atom.Ue.M13 = atom.Ue.M31 = Cif.atom[i].u13;
                    atom.Ue.M12 = atom.Ue.M21 = Cif.atom[i].u12;
                }
                else
                {
                    atom.Ue.M11 = atom.Ue.M22 = atom.Ue.M33 = Cif.atom[i].iso * Flags.ATP_Probability;
                    atom.Un = atom.Ue;
                }

                //transform U in fractional coordinate to cartesian U                  
                // Ucart= A * N* Ucif* Ntranspose * A transpose
                // atom.U = Amatrix * (Nmatrix * atom.U * Matrix.Transpose(Nmatrix) )* Matrix.Transpose(Amatrix);
                atom.Ue = Amatrix * Nmatrix * atom.Ue * Matrix.Transpose(Nmatrix) * Matrix.Transpose(Amatrix);
                atom.Un = Amatrix * Nmatrix * atom.Un * Matrix.Transpose(Nmatrix) * Matrix.Transpose(Amatrix);

                //Calculate eigenvectors and multiply by eigenvslues (to create ortogonal elipses)
                atom.Un = Eigen.main(ref atom.Ue);

                //Add mnimal atom size
                if (atom.Ue.M11 < 0.01f)
                    atom.Ue.M11 += 0.01f;
                if (atom.Ue.M22 < 0.01f)
                    atom.Ue.M22 += 0.01f;
                if (atom.Ue.M33 < 0.01f)
                    atom.Ue.M33 += 0.01f;

                if (atom.Un.M11 < 0.01f)
                    atom.Un.M11 += 0.01f;
                if (atom.Un.M22 < 0.01f)
                    atom.Un.M22 += 0.01f;
                if (atom.Un.M33 < 0.01f)
                    atom.Un.M33 += 0.01f;

                atom.Ue *= Flags.ATP_Probability;//probability
                atom.Un *= Flags.ATP_Probability;//probability

                atom.Ue.M44 = 1; //must be positive zoom
                atom.Un.M44 = 1;//must be positive zoom


                AsymetricPart.Add(atom);
            }
        }

        Matrix TransformUStatToU()
        {
            double aStar, bStar, cStar, cosAlpStar;

            double sinAlp, sinBet, sinGam;
            double cosAlp, cosBet, cosGam;

            sinAlp = System.Math.Sin(alpha);
            cosAlp = System.Math.Cos(alpha);
            sinBet = System.Math.Sin(beta);
            cosBet = System.Math.Cos(beta);
            sinGam = System.Math.Sin(gamma);
            cosGam = System.Math.Cos(gamma);

            Volume = (float)(a * b * c * System.Math.Sqrt(1.0f
                                                           - cosAlp * cosAlp
                                                           - cosBet * cosBet
                                                           - cosGam * cosGam
                                                           + 2.0f * cosAlp * cosBet * cosGam));

            aStar = (float)(b * c * sinAlp / Volume);
            bStar = (float)(a * c * sinBet / Volume);
            cStar = (float)(a * b * sinGam / Volume);

            cosAlpStar = (cosBet * cosGam - cosAlp) / (sinBet * sinGam);


            Matrix U = new Matrix();

            U.M11 = (float)aStar;
            U.M12 = 0;
            U.M13 = 0;
            U.M14 = 0;

            U.M21 = 0;
            U.M22 = (float)bStar;
            U.M23 = 0;
            U.M24 = 0;

            U.M31 = 0;
            U.M32 = 0;
            U.M33 = (float)cStar;
            U.M34 = 0;

            U.M41 = 0; U.M42 = 0; U.M43 = 0; U.M44 = 1;
            return U;

        }

        //Function calculates errors from fractional to cartesian system using error propagation law
        void CalculateError(ref float x_e, ref float y_e, ref float z_e)
        {

            //Function derivatives
            Matrix X = Matrix.Transpose(FractionaltoCarttesianMatrix(1, 0, 0));
            Matrix Y = Matrix.Transpose(FractionaltoCarttesianMatrix(0, 1, 0));
            Matrix Z = Matrix.Transpose(FractionaltoCarttesianMatrix(0, 0, 1));

            float x = sqr(x_e * X[0, 0]) + sqr(y_e * Y[1, 0]) + sqr(z_e * Z[2, 0]);
            float y = sqr(x_e * X[0, 1]) + sqr(y_e * Y[1, 1]) + sqr(z_e * Z[2, 1]);
            float z = sqr(x_e * X[0, 2]) + sqr(y_e * Y[1, 2]) + sqr(z_e * Z[2, 2]);

            x_e = (float)Math.Sqrt(x);
            y_e = (float)Math.Sqrt(y);
            z_e = (float)Math.Sqrt(z);
        }

        Matrix FractionaltoCarttesianMatrix(float a_, float b_, float c_)
        {
            if (a_ == 0) a_ = a;
            if (b_ == 0) b_ = b;
            if (c_ == 0) c_ = c;

            double aStar, bStar, cStar, sinAlpStar, cosAlpStar, sinBetStar, cosBetStar, sinGamStar, cosGamStar;

            double sinAlp, sinBet, sinGam;
            double cosAlp, cosBet, cosGam;

            sinAlp = System.Math.Sin(alpha);
            cosAlp = System.Math.Cos(alpha);
            sinBet = System.Math.Sin(beta);
            cosBet = System.Math.Cos(beta);
            sinGam = System.Math.Sin(gamma);
            cosGam = System.Math.Cos(gamma);

            float V = (float)(a_ * b_ * c_ * System.Math.Sqrt(1.0f
                                                            - cosAlp * cosAlp
                                                            - cosBet * cosBet
                                                            - cosGam * cosGam
                                                            + 2.0f * cosAlp * cosBet * cosGam));


            aStar = (float)(b_ * c_ * sinAlp / V);
            bStar = (float)(a_ * c_ * sinBet / V);
            cStar = (float)(a_ * b_ * sinGam / V);

            sinAlpStar = V / (a_ * b_ * c_ * sinBet * sinGam);
            cosAlpStar = (cosBet * cosGam - cosAlp) / (sinBet * sinGam);

            sinBetStar = V / (a_ * b_ * c_ * sinAlp * sinGam);
            cosBetStar = (cosAlp * cosGam - cosBet) / (sinAlp * sinGam);

            sinGamStar = V / (a_ * b_ * c_ * sinAlp * sinBet);
            cosGamStar = (cosAlp * cosBet - cosGam) / (sinAlp * sinBet);

            Matrix U = new Matrix();

            U.M11 = a_;
            U.M12 = (float)(b_ * cosGam);
            U.M13 = (float)(c_ * cosBet);
            U.M14 = 0;

            U.M21 = 0;
            U.M22 = (float)(b_ * sinGam);
            U.M23 = (float)(-c_ * sinBet * cosAlpStar);
            U.M24 = 0;

            U.M31 = 0;
            U.M32 = 0;
            U.M33 = (float)(c * Math.Sqrt(sqr((float)sinBet) - sqr((float)(-sinBet * cosAlpStar))));//                    (1.0 / cStar);// (c * sinBet * sinAlpStar);
            U.M34 = 0;

            U.M41 = 0; U.M42 = 0; U.M43 = 0; U.M44 = 1;
            return U;
        }


        Matrix ReciprocalToReallMatrix()
        {
            // a* = (bc sin α)/V b* = (ac sin β)/V c* = (ab sin γ)/V 
            // sin α* = V/(abc sin β sin γ) cos α* = (cos β cos γ – cos α)/(sin β sin γ) 
            // sin β* = V/(abc sin α sin γ) cos β* = (cos α cos γ – cos β)/(sin α sin γ) 
            // sin γ* = V/(abc sin α sin β) cos γ* = (cos α cos β – cos γ)/(sin α sin β) 
            // V = 1/V* = abc(1 – cos2 α – cos2 β – cos2 γ + 2 cos α cos β cos γ)1/2

            double aStar, bStar, cStar, sinAlpStar, cosAlpStar, sinBetStar, cosBetStar, sinGamStar, cosGamStar;

            double sinAlp, sinBet, sinGam;
            double cosAlp, cosBet, cosGam;

            sinAlp = System.Math.Sin(alpha);
            cosAlp = System.Math.Cos(alpha);
            sinBet = System.Math.Sin(beta);
            cosBet = System.Math.Cos(beta);
            sinGam = System.Math.Sin(gamma);
            cosGam = System.Math.Cos(gamma);


            float V = (float)(a * b * c * System.Math.Sqrt(1.0f - cosAlp * cosAlp -
                                                           cosBet * cosBet -
                                                           cosGam * cosGam +
                                                           2.0f * cosAlp * cosBet * cosGam));

            aStar = (float)(b * c * sinAlp / V);
            bStar = (float)(a * c * sinBet / V);
            cStar = (float)(a * b * sinGam / V);

            sinAlpStar = V / (a * b * c * sinBet * sinGam);
            cosAlpStar = (cosBet * cosGam - cosAlp) / (sinBet * sinGam);

            sinBetStar = V / (a * b * c * sinAlp * sinGam);
            cosBetStar = (cosAlp * cosGam - cosBet) / (sinAlp * sinGam);

            sinGamStar = V / (a * b * c * sinAlp * sinBet);
            cosGamStar = (cosAlp * cosBet - cosGam) / (sinAlp * sinBet);


            Matrix N = new Matrix();
            N = Matrix.Identity;

            //new
            N.M11 = (float)(aStar * aStar);
            N.M12 = (float)(aStar * bStar * cosGamStar);
            N.M13 = (float)(aStar * cStar * cosBetStar);

            N.M21 = N.M12;
            N.M22 = (float)(bStar * bStar);
            N.M23 = (float)(bStar * cStar * cosAlpStar);

            N.M31 = N.M13;
            N.M32 = N.M23;
            N.M33 = (float)(cStar * cStar);

            return N;
        }

        private void CalculateSymmetryEquivalents()
        {
            //do the hard work calculate all atoms in unit cell      
            ReadSymm matrix = new ReadSymm();

            SymetryEquivalents.Clear();

            for (int j = 0; j < AsymetricPart.Count; j++)
            {
                for (int i = 0; i < SymmetryElements.SMatrix.Count; i++)
                {
                    matrix = SymmetryElements.SMatrix[i];
                    AtomSimple atom_local = new AtomSimple();
                    //asign correct axis for transformation
                    float[] pos = new float[3];

                    pos[0] = AsymetricPart[j].x;
                    pos[1] = AsymetricPart[j].y;
                    pos[2] = AsymetricPart[j].z;

                    int sig1 = matrix.sign2[0];
                    int sig2 = matrix.sign2[0];

                    if (sig1 == 0) sig1 = 1;
                    if (sig2 == 0) sig2 = 1;

                    //still fractional coordinates
                    atom_local.x = pos[matrix.pos1[0]] * matrix.sign1[0] + pos[matrix.pos2[0]] * matrix.sign2[0] + matrix.shift[0];
                    atom_local.y = pos[matrix.pos1[1]] * matrix.sign1[1] + pos[matrix.pos2[1]] * matrix.sign2[1] + matrix.shift[1];
                    atom_local.z = pos[matrix.pos1[2]] * matrix.sign1[2] + pos[matrix.pos2[2]] * matrix.sign2[2] + matrix.shift[2];
                    //---------------------------error

                    atom_local.x_e = AsymetricPart[j].x_e;
                    atom_local.y_e = AsymetricPart[j].y_e;
                    atom_local.z_e = AsymetricPart[j].z_e;

                    //---------------------------
                    atom_local.Un.M11 = AsymetricPart[j].Un.M11 * matrix.sign1[0] * sig1;
                    atom_local.Un.M12 = AsymetricPart[j].Un.M12 * matrix.sign1[1] * sig2;
                    atom_local.Un.M13 = AsymetricPart[j].Un.M13 * matrix.sign1[2];
                    atom_local.Un.M14 = 0;

                    atom_local.Un.M21 = AsymetricPart[j].Un.M21 * matrix.sign1[0] * sig1;
                    atom_local.Un.M22 = AsymetricPart[j].Un.M22 * matrix.sign1[1] * sig2;
                    atom_local.Un.M23 = AsymetricPart[j].Un.M23 * matrix.sign1[2];
                    atom_local.Un.M24 = 0;

                    atom_local.Un.M31 = AsymetricPart[j].Un.M31 * matrix.sign1[0] * sig1;
                    atom_local.Un.M32 = AsymetricPart[j].Un.M32 * matrix.sign1[1] * sig2;
                    atom_local.Un.M33 = AsymetricPart[j].Un.M33 * matrix.sign1[2];
                    atom_local.Un.M34 = 0;
                    atom_local.Un.M44 = AsymetricPart[j].Un.M44;

                    //--------------------------
                    atom_local.Ue.M11 = AsymetricPart[j].Ue.M11 * matrix.sign1[0] * sig1;
                    atom_local.Ue.M12 = AsymetricPart[j].Ue.M12 * matrix.sign1[1] * sig2;
                    atom_local.Ue.M13 = AsymetricPart[j].Ue.M13 * matrix.sign1[2];
                    atom_local.Ue.M14 = 0;

                    atom_local.Ue.M21 = AsymetricPart[j].Ue.M21 * matrix.sign1[0] * sig1;
                    atom_local.Ue.M22 = AsymetricPart[j].Ue.M22 * matrix.sign1[1] * sig2;
                    atom_local.Ue.M23 = AsymetricPart[j].Ue.M23 * matrix.sign1[2];
                    atom_local.Ue.M24 = 0;

                    atom_local.Ue.M31 = AsymetricPart[j].Ue.M31 * matrix.sign1[0] * sig1;
                    atom_local.Ue.M32 = AsymetricPart[j].Ue.M32 * matrix.sign1[1] * sig2;
                    atom_local.Ue.M33 = AsymetricPart[j].Ue.M33 * matrix.sign1[2];
                    atom_local.Ue.M34 = 0;
                    atom_local.Ue.M44 = AsymetricPart[j].Ue.M44;

                    atom_local.Transformation.M11 = matrix.sign1[0];
                    atom_local.Transformation.M12 = 0;
                    atom_local.Transformation.M13 = 0;
                    atom_local.Transformation.M21 = 0;

                    atom_local.Transformation.M22 = matrix.sign1[1];
                    atom_local.Transformation.M23 = 0;
                    atom_local.Transformation.M31 = 0;
                    atom_local.Transformation.M32 = 0;
                    atom_local.Transformation.M33 = matrix.sign1[2];

                    atom_local.Transformation.M14 = atom_local.Transformation.M24 = atom_local.Transformation.M34 = 0;
                    atom_local.Transformation.M44 = 1;

                    atom_local.type = AsymetricPart[j].type;
                    atom_local.name = AsymetricPart[j].name;
                    atom_local.occupancy = AsymetricPart[j].occupancy;
                    atom_local.disorder_group = AsymetricPart[j].disorder_group;

                    atom_local.MoleculeNumber = i;

                    if (!IsTheSamePosition(atom_local))
                        SymetryEquivalents.Add(atom_local);
                }
            }
        }


        void CenterMolecules3D()
        {
            AtomSimple local_atom = new AtomSimple();
            List<AtomSimple> List_atoms = new List<AtomSimple>();
            //--------------------------------------------------------------------------------------------------------------------------------
            int i;


            float[] temp_x = new float[SymmetryElements.SMatrix.Count];
            float[] temp_y = new float[SymmetryElements.SMatrix.Count];
            float[] temp_z = new float[SymmetryElements.SMatrix.Count];


            if (SymetryEquivalents.Count < 0)
                return;

            //Extract molecules
            //Center bulk x coordinate
            for (int j = 0; j < SymmetryElements.SMatrix.Count; j++)
            {
                temp_x[j] = 0;
                temp_y[j] = 0;
                temp_z[j] = 0;

                for (i = 0; i < SymetryEquivalents.Count; i++)
                {
                    //i=molecule atoms * symmetry equivalents
                    if (j == SymetryEquivalents[i].MoleculeNumber)
                    {
                        temp_x[j] += SymetryEquivalents[i].x;
                        temp_y[j] += SymetryEquivalents[i].y;
                        temp_z[j] += SymetryEquivalents[i].z;
                    }
                }
                temp_x[j] /= SymetryEquivalents.Count;
                temp_y[j] /= SymetryEquivalents.Count;
                temp_z[j] /= SymetryEquivalents.Count;
            }


            for (int j = 0; j < SymmetryElements.SMatrix.Count; j++)
            {
                for (i = 0; i < SymetryEquivalents.Count; i++)
                {
                    if (j == SymetryEquivalents[i].MoleculeNumber)
                    {
                        local_atom = SymetryEquivalents[i];
                        if (temp_x[j] < 0) local_atom.x += 1;
                        if (temp_y[j] < 0) local_atom.y += 1;
                        if (temp_z[j] < 0) local_atom.z += 1;

                        if (temp_x[j] > 1) local_atom.x -= 1;
                        if (temp_y[j] > 1) local_atom.y -= 1;
                        if (temp_z[j] > 1) local_atom.z -= 1;

                        List_atoms.Add(local_atom);
                    }
                }
            }

            SymetryEquivalents.Clear();
            SymetryEquivalents.AddRange(List_atoms);

        }


        void CenterAtoms3D()
        {
            //--------------------------------------------------------------------------------------------------------------------------------
            int i;
            float temp = 0.0f;


            if (NumberOfAtoms != 0)
            {
                //Center bulk x coordinate
                for (i = 0; i < NumberOfAtoms; i++)
                    temp += atom[i].Coordinate.X;
            }

            temp /= (NumberOfAtoms);
            MoleculeShift.X = temp;

            //center bulk x
            for (i = 0; i < NumberOfAtoms; i++)
                atom[i].Coordinate.X -= temp;

            //Center bulk y coordinate
            if (NumberOfAtoms != 0)
            {
                for (i = 0; i < NumberOfAtoms; i++)
                    temp += atom[i].Coordinate.Y;
            }

            temp /= (NumberOfAtoms);
            MoleculeShift.Y = temp;

            //center bulk y
            for (i = 0; i < NumberOfAtoms; i++)
                atom[i].Coordinate.Y -= temp;

            if (NumberOfAtoms != 0)
            {
                //Center bulk z coordinate
                for (i = 0; i < NumberOfAtoms; i++)
                    temp += atom[i].Coordinate.Z;
            }

            temp /= (NumberOfAtoms);
            MoleculeShift.Z = temp;

            //center bulk x
            for (i = 0; i < NumberOfAtoms; i++)
                atom[i].Coordinate.Z -= temp;

        }

        private void PrepareUnitCell()
        {
            UnitCellFrame.Clear();

            //Fractional coordinnates units
            Vector3[] position = new Vector3[8];

            position[0] = new Vector3(0, 0, 0);
            position[1] = new Vector3(1, 0, 0);
            position[2] = new Vector3(0, 1, 0);
            position[3] = new Vector3(0, 0, 1);
            position[4] = new Vector3(1, 1, 0);
            position[5] = new Vector3(1, 0, 1);
            position[6] = new Vector3(1, 1, 1);
            position[7] = new Vector3(0, 1, 1);

            Matrix U = Matrix.Transpose(FractionaltoCarttesianMatrix(0, 0, 0));

            //cartesian coordinates
            for (int i = 0; i < position.Length; i++)
            {
                position[i] = Vector3.Transform(position[i], U);
                position[i] -= MoleculeShift;
            }


            //x        
            object3d ob = CalcAngles(position[0], position[1]);
            ob.color = new Vector3(0.9f, 0.0f, 0.0f);
            UnitCellFrame.Add(ob);

            //y
            ob = CalcAngles(position[0], position[2]);
            ob.color = new Vector3(0.0f, 0.9f, 0.0f);
            UnitCellFrame.Add(ob);

            //z
            ob = CalcAngles(position[0], position[3]);
            ob.color = new Vector3(0.0f, 0.0f, 0.9f);
            UnitCellFrame.Add(ob);

            ob = CalcAngles(position[1], position[4]);
            ob.color = new Vector3(0.9f, 0.9f, 0.9f);
            UnitCellFrame.Add(ob);

            ob = CalcAngles(position[1], position[5]);
            ob.color = new Vector3(0.9f, 0.9f, 0.9f);
            UnitCellFrame.Add(ob);

            ob = CalcAngles(position[4], position[6]);
            ob.color = new Vector3(0.9f, 0.9f, 0.9f);
            UnitCellFrame.Add(ob);

            ob = CalcAngles(position[2], position[7]);
            ob.color = new Vector3(0.9f, 0.9f, 0.9f);
            UnitCellFrame.Add(ob);

            ob = CalcAngles(position[2], position[4]);
            ob.color = new Vector3(0.9f, 0.9f, 0.9f);
            UnitCellFrame.Add(ob);

            ob = CalcAngles(position[3], position[7]);
            ob.color = new Vector3(0.9f, 0.9f, 0.9f);
            UnitCellFrame.Add(ob);

            ob = CalcAngles(position[3], position[5]);
            ob.color = new Vector3(0.9f, 0.9f, 0.9f);
            UnitCellFrame.Add(ob);

            ob = CalcAngles(position[5], position[6]);
            ob.color = new Vector3(0.9f, 0.9f, 0.9f);
            UnitCellFrame.Add(ob);

            ob = CalcAngles(position[6], position[7]);
            ob.color = new Vector3(0.9f, 0.9f, 0.9f);
            UnitCellFrame.Add(ob);
        }

        object3d CalcAngles(Vector3 v1, Vector3 v2)
        {
            object3d res = new object3d();

            res.position = v1;
            res.distance = Vector3.Distance(v1, v2);
            res.AngleZ = (float)(System.Math.Asin((v1.Z - v2.Z) / res.distance));
            res.AngleXY = -(float)(MathHelper.ToRadians(180) - System.Math.Atan2((v1.Y - v2.Y), (v1.X - v2.X)));
            return res;
        }

        private bool IsTheSamePosition(AtomSimple new_record)
        {
            bool res = false;
            float acceptance = 0.01f;
            foreach (AtomSimple a in SymetryEquivalents)
            {
                if (Math.Abs(a.x - new_record.x) < acceptance &&
                    Math.Abs(a.y - new_record.y) < acceptance &&
                    Math.Abs(a.z - new_record.z) < acceptance
                    )
                { res = true; }

            }
            return res;
        }

        public void TestMatrix()
        {
            string res = "";

            foreach (ReadSymm sm in SMatrix)
            {
                string tx = "", ty = "", tz = "";
                string tx2 = "", ty2 = "", tz2 = "";


                if (sm.sign1[0] == -1) tx = "-";
                else tx = "+";
                if (sm.pos1[0] == 0) tx += "x";
                if (sm.pos1[0] == 1) tx += "y";
                if (sm.pos1[0] == 2) tx += "z";

                //remove 
                if (sm.sign2[0] != 0)
                {
                    if (sm.sign2[0] == -1) tx2 = "-";
                    else tx2 = "+";
                    if (sm.pos2[0] == 0) tx2 += "x";
                    if (sm.pos2[0] == 1) tx2 += "y";
                    if (sm.pos2[0] == 2) tx2 += "z";
                }


                if (sm.sign1[1] == -1) ty = "-";
                else ty = "+";
                if (sm.pos1[1] == 0) ty += "x";
                if (sm.pos1[1] == 1) ty += "y";
                if (sm.pos1[1] == 2) ty += "z";

                if (sm.sign2[1] != 0)
                {
                    if (sm.sign2[1] == -1) ty2 = "-";
                    else ty2 = "+";
                    if (sm.pos2[1] == 0) ty2 += "x";
                    if (sm.pos2[1] == 1) ty2 += "y";
                    if (sm.pos2[1] == 2) ty2 += "z";
                }


                if (sm.sign1[2] == -1) tz = "-";
                else tz = "+";
                if (sm.pos1[2] == 0) tz += "x";
                if (sm.pos1[2] == 1) tz += "y";
                if (sm.pos1[2] == 2) tz += "z";

                res += "" + ConvertTo(sm.shift[0]) + tx + tx2 + ",  " + ConvertTo(sm.shift[1]) + ty + ty2 + ",  " + ConvertTo(sm.shift[2]) + tz + "\n\r";
            }

            // string a = sm.
        }

        string ConvertTo(float a)
        {
            string re = "";

            if (a == 0.5) re = "1/2";
            if (a == 0.25) re = "1/4";
            if (a == 0.75) re = "3/4";
            if (a >= 0.33333 && a < 0.33334) re = "1/3";
            if (a >= 0.66666 && a < 0.66668) re = "2/3";

            return re;
        }

        //REturn the angle between 3 atoms
        static float Angle(Vector3 v1, Vector3 v2, Vector3 v3)
        {

            float distance1 = Vector3.Distance(v1, v2);
            float distance2 = Vector3.Distance(v2, v3);
            float distance3 = Vector3.Distance(v1, v3);



            //ABC = arc cos (AB*AB + BC*BC - AC*AC) / (2*AB*AC)

            double temp_calc = distance1 * distance1 + distance2 * distance2 - distance3 * distance3;
            temp_calc /= 2.0 * distance1 * distance2;
            return MathHelper.ToDegrees((float)Math.Acos(temp_calc));
        }

    }
}

