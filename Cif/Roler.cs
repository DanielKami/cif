//***********************************************************************
//                        CIF program
//                Created by Daniel M. Kaminski
//                        Lublin 2023
//                     Under GNU licence
//***********************************************************************

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Cif
{

    public class Roler
    {
        public static SpriteBatch spriteBatch;
        public static SpriteFont spriteFont;


        public int Value = 1;
        public Vector2 Position;
        public float radious;

        float old_pos;
        double angle;
        readonly double SPEED = 0.005;
        readonly double ACCEPTED = 0.96;
        readonly int SizeX = 150;
        string str = "";


        //int min = 0;
        //int max = 9;

        int step = 36;
        int start = 0;
        int end = 360;
        public Roler(Vector2 _Position)
        {
            Position = _Position;
            //CalculateCenter();
            old_pos = 0;
            angle = 0;
            radious = 150;

        }

        public void Ini(string _str)
        {
            angle = -MathHelper.ToRadians(Value * step);
            str = _str;
        }

        static public void Download(ContentManager contentManager)
        {

        }


        public bool CheckEntry()
        {
            Vector2 Pos = Camera.ptMouseCurrent;

            //Active area
            if (Pos.X > Position.X - SizeX && Pos.X < Position.X + SizeX &&
                Pos.Y > Position.Y - radious && Pos.Y < Position.Y + radious)
            {
                //Start position
                if (Pos.Y == Camera.ptMousePressed.Y)
                    old_pos = Pos.Y;

                double del = Pos.Y - old_pos;
                old_pos = Pos.Y;

                angle += del * SPEED;

                if (Camera.flag_release)
                    for (int i = start; i < end; i += step)
                    {
                        double Cos = Math.Cos(angle + MathHelper.ToRadians(i));
                        if (Cos > ACCEPTED - 0.02)
                        {
                            Value = i / step;
                            angle = -MathHelper.ToRadians(i);
                            return true;
                        }
                    }
            }

            return false;
        }

        public void draw()
        {
            Vector2 PosCorr = new Vector2(SizeX / 2, radious);
            GraphiscHelper.FiledRectangle(Position - PosCorr, SizeX, 2.2f * radious, 0, Color.FromNonPremultiplied(0, 0, 0, 50), Color.FromNonPremultiplied(0, 0, 0, 50));

            PosCorr = new Vector2(SizeX / 2, 0);
            spriteBatch.DrawString(spriteFont, "<" + str, Position + PosCorr, Color.DeepPink, 0, new Vector2(0, 0), 1, SpriteEffects.None, .9f);


            for (int i = start; i < end; i += step)
            {
                //if (i / step >= min)
                //    i = min * step;
                double rot = angle + MathHelper.ToRadians(i);
                double Sin = Math.Sin(rot);
                double Cos = Math.Cos(rot);

                float VY = (float)(Position.Y + radious * Sin);

                if (Cos > 0)
                {
                    Color c;
                    int A = (int)(Cos * 255);

                    if (Cos > ACCEPTED)
                        c = Color.FromNonPremultiplied(250, 170, 255, A);
                    else
                        c = Color.FromNonPremultiplied(250, 0, 255, A);

                    float value = i / step;
                    spriteBatch.DrawString(spriteFont, "" + value, new Vector2(Position.X, VY), c, 0, new Vector2(0, 0), 1, SpriteEffects.None, .9f);

                }
            }

        }
    }
}
