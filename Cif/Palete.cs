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
    public class Palete
    {
        public static SpriteBatch spriteBatch;
        public static SpriteFont spriteFont;

        Vector2 Position;
        Zipper zipperR;
        Zipper zipperG;
        Zipper zipperB;
        string str = "";

        int R, G, B;
        public Color color;

        public Palete(Vector2 _position, string _Str)
        {
            str = _Str;
            R = 10; G = 20; B = 30;
            Position = _position;

            Zipper.spriteBatch = spriteBatch;
            Zipper.spriteFont = spriteFont;


            Vector2 Local;
            Local = new Vector2(0, 200);
            zipperR = new Zipper(Position + Local, Color.Black, Color.Red);
            zipperR.SetButtoon(R);
            Local = new Vector2(150, 200);
            zipperG = new Zipper(Position + Local, Color.Black, Color.Green);
            zipperG.SetButtoon(G);
            Local = new Vector2(300, 200);
            zipperB = new Zipper(Position + Local, Color.Black, Color.Blue);
            zipperB.SetButtoon(B);
        }

        public static void LoadContent(ContentManager Content)
        {
            Zipper.LoadContent(Content);
        }

        public void Check()
        {
            zipperR.Check();
            zipperG.Check();
            zipperB.Check();

            R = zipperR.Value;
            G = zipperG.Value;
            B = zipperB.Value;

            color = Color.FromNonPremultiplied(new Vector4(1.0f * R / 256, 1.0f * G / 256, 1.0f * B / 256, 1.0f));
        }

        public void Draw()
        {
            Vector2 corr = new Vector2(-52, -122);
            GraphiscHelper.FiledRectangle(Position + corr, 440 + 4, 350 + 4, 0, Color.DeepPink, Color.DeepPink);
            corr = new Vector2(-50, -120);
            GraphiscHelper.FiledRectangle(Position + corr, 440, 350, 0, Color.Black, Color.Black);
            zipperR.Draw();
            zipperG.Draw();
            zipperB.Draw();

            corr = new Vector2(-10, -170);
            spriteBatch.DrawString(spriteFont, str, Position + corr, Color.DeepPink, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, .9f);

            //  GraphiscHelper.FiledRectangle(Position + corr, 100, 100, 0, color, color);


        }
    }
}