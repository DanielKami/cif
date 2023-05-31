//***********************************************************************
//                        CIF program
//                Created by Daniel M. Kaminski
//                        Lublin 2023
//                     Under GNU licence
//***********************************************************************


using Microsoft.Xna.Framework;

namespace Cif
{
    internal static class Flags
    {

        public static string version = "v. 1.7";

        //Front page counter
        public static bool flag_start = true;
        public static float intro_duration = 4;
        public static float remainingDelay = intro_duration;// seconds
        public static int GenerateEquvalents = 0;  //How the molecule is presented asymetric part or symmetry equivalents

        public static int Presentation = 2; //CPK, bonds, ball-sticks, elipsoids atp
        //cell repetitions
        public static int repeat_x = 1;
        public static int repeat_y = 1;
        public static int repeat_z = 1;

        public static int repeatOld_x = 1;
        public static int repeatOld_y = 1;
        public static int repeatOld_z = 1;

        public static Color color1 = Color.FromNonPremultiplied(new Vector4(0.0f, 0.0f, 0.3f, 1.0f));
        public static Color color2 = Color.FromNonPremultiplied(new Vector4(0.0f, 0.0f, 0.1f, 1.0f));

        //Atoms
        public static float MAX_ATOM_DISTANCE = 3.0f;
        public static float MAX_H_ATOM_DISTANCE = 3.1f;
        public static int MAX_BONDED_ATOMS = 10;
        public static int MAX_H_BONDED_ATOMS = 5;
        public static float BOND_FACTOR = 0.58f;
        public static float H_acceptance_angle = 63.0f;  //hydrogen bond acceptance angle D-H-A
        public static float BOND_FACTOR_METALS = 0.79f;
        public static float ATP_Probability = 0.5f;
        public static int AtomicRadiiType = 0;  //from table 0-atomic radii, 1 - ionic radii
        public static float Poli_dyst = 2.9f; //simmilar to MAX_BONDED_ATOMS
        public static int view; //wiew from direction 0-x, 1-y, 2-z
        public static void Reset()
        {
            view = 0;
            GenerateEquvalents = 0;
            Presentation = 2;

            repeat_x = 1;
            repeat_y = 1;
            repeat_z = 1;

            repeatOld_x = 1;
            repeatOld_y = 1;
            repeatOld_z = 1;



        }
    }
}
