//***********************************************************************
//                        CIF program
//                Created by Daniel M. Kaminski
//                        Lublin 2023
//                     Under GNU licence
//***********************************************************************

 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cif
{
    public class Atom
    {
        public Atom() { }

        public string name;
        public int type;

        public float x;
        public float y;
        public float z;

        public float x_e;
        public float y_e;
        public float z_e;

        public string adp_type;

        public float occ;
        public float occ_e;

        public float iso;
        public float iso_e;

        public int symmetry_order;
        public string calc_flag;

        public float u11;
        public float u22;
        public float u33;
        public float u12;
        public float u13;
        public float u23;

        public float u11_e;
        public float u22_e;
        public float u33_e;
        public float u12_e;
        public float u13_e;
        public float u23_e;

        public int disorder_group;

    }

    public class Matrix_U
    {
        public Matrix_U() { }

        public string name;

        public float u11;
        public float u22;
        public float u33;
        public float u12;
        public float u13;
        public float u23;

        public float u11_e;
        public float u22_e;
        public float u33_e;
        public float u12_e;
        public float u13_e;
        public float u23_e;
    }


    public static class Cif
    {
        static Cif() { }

        public static string FlieName = "";
        public static float a, b, c;
        public static float alp, bet, gam;
        public static float a_e, b_e, c_e;
        public static float alp_e, bet_e, gam_e;
        public static float cell_volume;
        public static float cell_volume_e;
        public static int space_group_IT_number;
        public static float formula_units_Z;

        public static List<Atom> atom = new List<Atom>();
        public static List<string> SymmetryOperations = new List<string>();

        //other
        public static string chemical_name_common = "";
        public static float formula_weight;
        public static string formula_sum;
        public static float measurement_temperature;
        public static float s_R_factor_gt;

        public static string chemical_oxdiff_usercomment = "";
        public static int cell_measurement_reflns_used;
        public static float exptl_absorpt_coefficient_mu;
        public static string exptl_absorpt_correction_type = "";
        public static float exptl_crystal_density_diffrn;


        public static bool completed;  //cif file is completlyy reat


        public static void Clean()
        {
            completed = false;
            a = b = c = 0;
            alp = bet = gam = 0;
            a_e = b_e = c_e = 0;
            alp_e = bet_e = gam_e = 0;
            cell_volume = 0;
            cell_volume_e = 0;
            space_group_IT_number = 0;
            formula_units_Z = 0;

            chemical_name_common = "";
            formula_weight = 0;
            formula_sum = "";
            measurement_temperature = 0;
            s_R_factor_gt = 0;
            atom.Clear();
            SymmetryOperations.Clear();
        }
    }

    internal class Read
    {

        #region Define cif flags
        static bool loop_ = false;
        static bool atoms_positions = false;
        static bool matrix_U = false;

        static bool _atom_site_label;
        static bool _atom_site_type_symbol;
        static bool _atom_site_fract_x;
        static bool _atom_site_fract_y;
        static bool _atom_site_fract_z;
        static bool _atom_site_U_iso_or_equiv;
        static bool _atom_site_adp_type;
        static bool _atom_site_occupancy;
        static bool _atom_site_site_symmetry_order;
        static bool _atom_site_calc_flag;
        static bool _atom_site_refinement_flags_posn;
        static bool _atom_site_refinement_flags_adp;
        static bool _atom_site_refinement_flags_occupancy;
        static bool _atom_site_disorder_assembly;
        static bool _atom_site_disorder_group;

        static bool _atom_site_aniso_label;
        static bool _atom_site_aniso_U_11;
        static bool _atom_site_aniso_U_22;
        static bool _atom_site_aniso_U_33;
        static bool _atom_site_aniso_U_23;
        static bool _atom_site_aniso_U_13;
        static bool _atom_site_aniso_U_12;

        static bool _space_group_symop_operation_xyz;

        static string[] elements =
{
            "H","He","Li","Be","B","C","N","O","F","Ne","Na","Mg",
            "Al","Si","P","S","Cl","Ar","K","Ca","Sc","Ti","V","Cr",
            "Mn","Fe","Co","Ni","Cu","Zn","Ga","Ge","As","Se","Br",
            "Kr","Rb","Sr","Y","Zr","Nb","Mo","Tc","Ru","Rh","Pd",
            "Ag","Cd","In","Sn","Sb","Te","I","Xe","Cs","Ba","La",
            "Ce","Pr","Nd","Pm","Sm","Eu","Gd","Tb","Dy","Ho","Er",
            "Tm","Yb","Lu","Hf","Ta","W","Re","Os","Ir","Pt","Au",
            "Hg","Tl","Pb","Bi","Po","At","Rn","Fr","Ra","Ac","Th",
            "Pa","U","Np","Pu","Am","Cm","Bk","Cf"
            };
        //Position in loop Cif properties
        struct AtomFlag
        {
            public int position;
            public bool exist;
        }

        static AtomFlag[] _atom_sites;//atom site elements
        static int[] _atom_site_aniso; //U order
        #endregion

        static void Clear()
        {

            loop_ = false;
            atoms_positions = false;
            matrix_U = false;

            _atom_site_label = false;
            _atom_site_type_symbol = false;
            _atom_site_fract_x = false;
            _atom_site_fract_y = false;
            _atom_site_fract_z = false;
            _atom_site_U_iso_or_equiv = false;
            _atom_site_adp_type = false;
            _atom_site_occupancy = false;
            _atom_site_site_symmetry_order = false;
            _atom_site_calc_flag = false;
            _atom_site_refinement_flags_posn = false;
            _atom_site_refinement_flags_adp = false;
            _atom_site_refinement_flags_occupancy = false;
            _atom_site_disorder_assembly = false;
            _atom_site_disorder_group = false;

            _atom_site_aniso_label = false;
            _atom_site_aniso_U_11 = false;
            _atom_site_aniso_U_22 = false;
            _atom_site_aniso_U_33 = false;
            _atom_site_aniso_U_23 = false;
            _atom_site_aniso_U_13 = false;
            _atom_site_aniso_U_12 = false;

            _space_group_symop_operation_xyz = false;
        }
        public static bool Read_data(string str)
        {
            string[] lines = Regex.Split(str, "\r\n|\r|\n");

            int i;
            int start;
            //Clear flags
            Clear();
            Cif.Clean();

            SymmetryElements.TransformationsClean();

            //Temporary matrix U
            List<Matrix_U> matrix_u = new List<Matrix_U>();

            #region Look for simple records
            foreach (string line in lines)
            {
                if (line.Contains("_cell_length_a"))
                {
                    Cif.a = GetValue(line);
                    Cif.a_e = GetError(line);
                }
                else if (line.Contains("_cell_length_b"))
                {
                    Cif.b = GetValue(line);
                    Cif.b_e = GetError(line);
                }
                else if (line.Contains("_cell_length_c"))
                {
                    Cif.c = GetValue(line);
                    Cif.c_e = GetError(line);
                }
                else if (line.Contains("_cell_angle_alpha"))
                {
                    Cif.alp = GetValue(line);
                    Cif.alp_e = GetError(line);
                }
                else if (line.Contains("_cell_angle_beta"))
                {
                    Cif.bet = GetValue(line);
                    Cif.bet_e = GetError(line);
                }
                else if (line.Contains("_cell_angle_gamma"))
                {
                    Cif.gam = GetValue(line);
                    Cif.gam_e = GetError(line);
                }
                else if (line.Contains("_refine_ls_R_factor_gt"))
                {
                    Cif.s_R_factor_gt = GetValue(line);
                }
                else if (line.Contains("_cell_volume"))
                {
                    Cif.cell_volume = GetValue(line);
                    Cif.cell_volume_e = GetError(line);
                }
                else if (line.Contains("_cell_formula_units_Z"))
                {
                    Cif.formula_units_Z = GetValue(line);
                }
                else if (line.Contains("_space_group_IT_number"))
                {
                    Cif.space_group_IT_number = (int)GetValue(line);
                }
                else if (line.Contains("_chemical_name_common"))
                {
                    Cif.chemical_name_common = line.Replace("_chemical_name_common", "");
                }
                else if (line.Contains("_chemical_formula_weight"))
                {
                    Cif.formula_weight = GetValue(line);
                }
                else if (line.Contains("_chemical_formula_sum"))
                {
                    Cif.formula_sum = line.Replace("_chemical_formula_sum", "");
                }
                else if (line.Contains("_cell_measurement_temperature"))
                {
                    Cif.measurement_temperature = GetValue(line);
                }
                else if (line.Contains("_chemical_oxdiff_usercomment"))
                {
                    Cif.chemical_oxdiff_usercomment = line.Replace("_chemical_oxdiff_usercomment", "");
                }
                else if (line.Contains("_cell_measurement_reflns_used"))
                {
                    Cif.cell_measurement_reflns_used = (int)GetValue(line);
                }
                else if (line.Contains("_exptl_absorpt_coefficient_mu"))
                {
                    Cif.exptl_absorpt_coefficient_mu = GetValue(line);
                }
                else if (line.Contains("_exptl_absorpt_correction_type"))
                {
                    Cif.exptl_absorpt_correction_type = line.Replace("_exptl_absorpt_correction_type", "");
                }
                else if (line.Contains("_exptl_crystal_density_diffrn"))
                {
                    Cif.exptl_crystal_density_diffrn = GetValue(line);
                }
            }

            #endregion
           // if (Cif.space_group_IT_number == 0)
            {
                //Find symmetry operations
                loop_ = false;
                //Find symmetry positions
                i = -1;

                start = 0;
                foreach (string line in lines)
                {
                    i++;
                    //look for _loop
                    if (line.Contains("loop_"))
                    {
                        loop_ = true;
                        start = i + 1;
                    }

                    if (loop_)
                    {
                        //check if the part of file contains flags
                        //The order can be important
                        if (line.Contains("_space_group_symop_operation_xyz")) { _space_group_symop_operation_xyz = true; }

                        //must be line with symmetry positions
                        if (_space_group_symop_operation_xyz)
                        {
                            i++;//omit flag
                            break;
                        }

                    }
                }
                //We know the atom side order
                //here read the atom positions from the lines
                loop_ = false;
                if (i < lines.Length)
                {
                    while (lines[i].Length > 1) //empty line is the end of loop
                    {
                        if (lines[i].Contains("loop_"))
                            break;

                        if (lines[i].Contains("_space_group_symop_operation_xyz"))
                            i++;

                        Cif.SymmetryOperations.Add(lines[i]);
                        SymmetryElements.CreateTransformationsFromFile(lines[i]);
                        i++;

                        if (i >= lines.Length - 1)
                            break;
                    }
                }
            }


            loop_ = false;
            //Find atom positions
            i = -1;
            atoms_positions = false;
            _atom_sites = new AtomFlag[15];//atom site elements
            start = 0;
            foreach (string line in lines)
            {
                i++;
                //look for _loop
                if (line.Contains("loop_"))
                {
                    loop_ = true;
                    start = i + 1;
                }

                if (loop_)
                {
                    //check if the part of file contains flags
                    //The order can be important
                    if (line.Contains("_atom_site_label")) { _atom_sites[0].position = i - start; _atom_sites[0].exist = _atom_site_label = true; }
                    if (line.Contains("_atom_site_type_symbol")) { _atom_sites[1].position = i - start; _atom_sites[1].exist = _atom_site_type_symbol = true; }
                    if (line.Contains("_atom_site_fract_x")) { _atom_sites[2].position = i - start; _atom_sites[2].exist = _atom_site_fract_x = true; }
                    if (line.Contains("_atom_site_fract_y")) { _atom_sites[3].position = i - start; _atom_sites[3].exist = _atom_site_fract_y = true; }
                    if (line.Contains("_atom_site_fract_z")) { _atom_sites[4].position = i - start; _atom_sites[4].exist = _atom_site_fract_z = true; }
                    if (line.Contains("_atom_site_U_iso_or_equiv")) { _atom_sites[5].position = i - start; _atom_sites[5].exist = _atom_site_U_iso_or_equiv = true; }
                    if (line.Contains("_atom_site_adp_type")) { _atom_sites[6].position = i - start; _atom_sites[6].exist = _atom_site_adp_type = true; }
                    if (line.Contains("_atom_site_occupancy")) { _atom_sites[7].position = i - start; _atom_sites[7].exist = _atom_site_occupancy = true; }
                    if (line.Contains("_atom_site_site_symmetry_order")) { _atom_sites[8].position = i - start; _atom_sites[8].exist = _atom_site_site_symmetry_order = true; }
                    if (line.Contains("_atom_site_calc_flag")) { _atom_sites[9].position = i - start; _atom_sites[9].exist = _atom_site_calc_flag = true; }

                    if (line.Contains("_atom_site_refinement_flags_posn")) { _atom_sites[10].position = i - start; _atom_sites[10].exist = _atom_site_refinement_flags_posn = true; }
                    if (line.Contains("_atom_site_refinement_flags_adp")) { _atom_sites[11].position = i - start; _atom_sites[11].exist = _atom_site_refinement_flags_adp = true; }
                    if (line.Contains("_atom_site_refinement_flags_occupancy")) { _atom_sites[12].position = i - start; _atom_sites[12].exist = _atom_site_refinement_flags_occupancy = true; }
                    if (line.Contains("_atom_site_disorder_assembly")) { _atom_sites[13].position = i - start; _atom_sites[13].exist = _atom_site_disorder_assembly = true; }
                    if (line.Contains("_atom_site_disorder_group")) { _atom_sites[14].position = i - start; _atom_sites[14].exist = _atom_site_disorder_group = true; }

      

                    //must be line with atom positions
                    if (_atom_site_label & _atom_site_fract_x & _atom_site_fract_y & _atom_site_fract_z)
                        atoms_positions = true;
                    //omit line with _atom_site and loop_
                    if (atoms_positions)
                        if (!line.Contains("_atom_site") && !line.Contains("loop_"))
                        {
                            break;
                        }
                }
            }
            //We know the atom side order

            //here read the atom positions from the lines
            loop_ = false;
            if (i < lines.Length)
            {
                while (lines[i].Length > 1) //empty line is the end of loop
                {
                    if (lines[i].Contains("loop_"))
                        break;

                    GetAtoms(lines[i]);
                    i++;
                    if (i >= lines.Length - 1)
                        break;
                }
            }

            //if Cif has only names without atomic numbers
            for (int l = 0; l < Cif.atom.Count; l++)
            {
                if (Cif.atom[l].name != "" && Cif.atom[l].type == 0)
                {
                    Cif.atom[l].type = AsignType(Cif.atom[l].name);
                }
            }



            //Look for thermal coeficients
            _atom_site_aniso = new int[7];
            start = i + 2;
            i++;// go to next line
            for (int t = i; t < lines.Length; t++)
            {
                //look for _loop
                if (lines[t].Contains("loop_"))
                {
                    loop_ = true;
                }

                if (loop_)
                {
                    //Contains flags
                    if (lines[t].Contains("_atom_site_aniso_label")) { _atom_site_aniso[0] = t - start; _atom_site_aniso_label = true; }
                    if (lines[t].Contains("_atom_site_aniso_U_11")) { _atom_site_aniso[1] = t - start; _atom_site_aniso_U_11 = true; }
                    if (lines[t].Contains("_atom_site_aniso_U_22")) { _atom_site_aniso[2] = t - start; _atom_site_aniso_U_22 = true; }
                    if (lines[t].Contains("_atom_site_aniso_U_33")) { _atom_site_aniso[3] = t - start; _atom_site_aniso_U_33 = true; }
                    if (lines[t].Contains("_atom_site_aniso_U_23")) { _atom_site_aniso[4] = t - start; _atom_site_aniso_U_23 = true; }
                    if (lines[t].Contains("_atom_site_aniso_U_13")) { _atom_site_aniso[5] = t - start; _atom_site_aniso_U_13 = true; }
                    if (lines[t].Contains("_atom_site_aniso_U_12")) { _atom_site_aniso[6] = t - start; _atom_site_aniso_U_12 = true; }


                    //must be line with atom positions
                    if (_atom_site_aniso_label & _atom_site_aniso_U_11 & _atom_site_aniso_U_22 & _atom_site_aniso_U_33 & _atom_site_aniso_U_23 & _atom_site_aniso_U_13 & _atom_site_aniso_U_12)
                        matrix_U = true;

                    if (matrix_U)
                        if (!lines[t].Contains("_atom_site_aniso") && !lines[t].Contains("loop_"))
                        {
                            i = t;
                            break;
                        }
                }
                i = t;
            }

            //here read the atom U from the lines
            if (i < lines.Length - 1)
            {
                while (lines[i].Length > 1) //empty lien is end of loop
                {
                    if (lines[i].Contains("loop_"))
                        break;
                    GetsU(lines[i], matrix_u);
                    i++;
                }
                AsignU(matrix_u);
            }

            Cif.completed = true;
            return true;
        }

        static float GetValue(string line)
        {
            if (line == "") return -1;

            float result = 0;
            string str = Regex.Match(line, @"-?\d+[.]\d+").Value;
            if (str == "")
            {
                str = Regex.Match(line, @"-?\d+").Value;
                float.TryParse(str, NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result);

            }
            else
                float.TryParse(str, NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result);

            return result;
        }

        static float GetError(string line)
        {
            float mul;
            float result = 0;

            if (line == "") return 0;

            string pos = Regex.Match(line, @"[.]\d+").Value;
            int lenght = pos.Length - 1;//it is considered that error is after coma!
            if (lenght > 0)
            {
                mul = (float)Math.Pow(10, lenght);
            }
            else
                mul = 1;

            string str = Regex.Match(line, @"[(]\d+[)]").Value;
            if (str == "") return 0;

            string str2 = Regex.Match(str, @"\d+").Value;
            if (str == "") return 0;
            result = float.Parse(str2) / mul;

            return result;
        }

        static void GetAtoms(string line)
        {
            string[] splits = line.Split(' ');

            //remove empty record
            if (splits[0] == "")
                splits = splits.Where((w, i) => i != 0).ToArray();

            Atom atom = new Atom();

            //"_atom_site_label" _atom_sites[0]
            //"_atom_site_type_symbol")) _atom_sites[1]
            //"_atom_site_fract_x")) _atom_sites[2]
            //"_atom_site_fract_y")) _atom_sites[3]
            //"_atom_site_fract_z")) _atom_sites[4]
            //"_atom_site_U_iso_or_equiv")) _atom_sites[5]
            //"_atom_site_occupancy")) _atom_sites[6]

            //if not found and must be:
            atom.occ = 1;
            atom.occ_e = 0;
            atom.iso = 0.05f;
            atom.occ_e = 0;
            string str = "";


            if (_atom_sites[0].exist)
            {
                str = GetPosition(splits, _atom_sites[0].position);
                atom.name = str;
            }

            if (_atom_sites[1].exist)
                if (splits.Length > _atom_sites[1].position)
                {
                    str = GetPosition(splits, _atom_sites[1].position);
                    atom.type = AsignType(str);
                }

            if (_atom_sites[2].exist)
                if (splits.Length > _atom_sites[2].position)
                {
                    str = GetPosition(splits, _atom_sites[2].position);
                    atom.x = GetValue(str);
                    atom.x_e = GetError(str);
                }

            if (_atom_sites[3].exist)
                if (splits.Length > _atom_sites[3].position)
                {
                    str = GetPosition(splits, _atom_sites[3].position);
                    atom.y = GetValue(str);
                    atom.y_e = GetError(str);
                }

            if (_atom_sites[4].exist)
                if (splits.Length > _atom_sites[4].position)
                {
                    str = GetPosition(splits, _atom_sites[4].position);
                    atom.z = GetValue(str);
                    atom.z_e = GetError(str);
                }

            if (_atom_sites[5].exist)
                if (splits.Length > _atom_sites[5].position)
                {
                    str = GetPosition(splits, _atom_sites[5].position);
                    atom.iso = GetValue(str);
                    atom.iso_e = GetError(str);
                }

            if (_atom_sites[6].exist)
                if (splits.Length > _atom_sites[6].position)
                {
                    str = GetPosition(splits, _atom_sites[6].position);
                    atom.adp_type = str;
                }

            if (_atom_sites[7].exist)
                if (splits.Length > _atom_sites[7].position)
                {
                    str = GetPosition(splits, _atom_sites[7].position);
                    atom.occ = GetValue(str);
                    atom.occ_e = GetError(str);
                }

            if (_atom_sites[8].exist)
                if (splits.Length > _atom_sites[8].position)
                {
                    str = GetPosition(splits, _atom_sites[8].position);
                    atom.symmetry_order = (int)GetValue(str);
                }

            if (_atom_sites[9].exist)
            {
                str = GetPosition(splits, _atom_sites[9].position);
                if (str != "")
                    atom.calc_flag = str;
            }

            if (_atom_sites[14].exist)
            {
                str = GetPosition(splits, _atom_sites[14].position);
                if (str != "")
                    atom.disorder_group = (int)GetValue(str);
             }

            Cif.atom.Add(atom);

        }

        private static string GetPosition(string[] input, int position)
        {
            int count = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != "")
                {
                    if (count == position)
                        return input[i];
                    count++;
                }
            }

            return "";

        }

        static void GetsU(string line, List<Matrix_U> matrix_u)
        {
            string[] splits = line.Split(' ');
            Matrix_U U = new Matrix_U();

            if (splits[0] == "")
                splits = splits.Where((w, i) => i != 0).ToArray();

            U.name = splits[_atom_sites[0].position];
            U.u11 = GetValue(splits[_atom_sites[1].position]);
            U.u11_e = GetError(splits[_atom_sites[1].position]);

            U.u22 = GetValue(splits[_atom_sites[2].position]);
            U.u22_e = GetError(splits[_atom_sites[2].position]);

            U.u33 = GetValue(splits[_atom_sites[3].position]);
            U.u33_e = GetError(splits[_atom_sites[3].position]);

            U.u23 = GetValue(splits[_atom_sites[4].position]);
            U.u23_e = GetError(splits[_atom_sites[4].position]);

            U.u13 = GetValue(splits[_atom_sites[5].position]);
            U.u13_e = GetError(splits[_atom_sites[5].position]);

            U.u12 = GetValue(splits[_atom_sites[6].position]);
            U.u12_e = GetError(splits[_atom_sites[6].position]);

            matrix_u.Add(U);
            //Console.WriteLine(U.u11 + "  " + U.u11_e);
        }

        //Return 0 if the element is not recognised
        static int AsignType(string str1)
        {
            string str3;

            if (str1 == "") return 12;//return carbon

            var str2 = Regex.Replace(str1, @"[\d-]", string.Empty); //reomve any numbers in case

            if (str2.Count() == 0) return 0;
            if (str2.Count() == 1 || str2.Count() == 2) str3 = str2;
            else
                str3 = str2.Substring(0, 2); //take first 2 elements from string - it should be a name of element

            for (int i = 0; i < elements.Length; i++)
                if (elements[i] == str3)
                    return i + 1;
            return 0;
        }

        static void AsignU(List<Matrix_U> matrix_u)
        {
            for (int j = 0; j < Cif.atom.Count; j++)
            {
                for (int i = 0; i < matrix_u.Count; i++)
                {
                    if (matrix_u[i].name == Cif.atom[j].name)
                    {

                        Cif.atom[j].u11 = matrix_u[i].u11;
                        Cif.atom[j].u22 = matrix_u[i].u22;
                        Cif.atom[j].u33 = matrix_u[i].u33;
                        Cif.atom[j].u12 = matrix_u[i].u12;
                        Cif.atom[j].u13 = matrix_u[i].u13;
                        Cif.atom[j].u23 = matrix_u[i].u23;

                        Cif.atom[j].u11_e = matrix_u[i].u11_e;
                        Cif.atom[j].u22_e = matrix_u[i].u22_e;
                        Cif.atom[j].u33_e = matrix_u[i].u33_e;
                        Cif.atom[j].u12_e = matrix_u[i].u12_e;
                        Cif.atom[j].u13_e = matrix_u[i].u13_e;
                        Cif.atom[j].u23_e = matrix_u[i].u23_e;

                    }
                }

            }
        }
    }

}
