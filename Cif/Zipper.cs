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
    public class Zipper
    {
        public static SpriteBatch spriteBatch;
        public static SpriteFont spriteFont;


        public Vector2 Position;
        public static int ButtonRadious = 75;
        public int ZipperLenght = 256;
        public int ZipperUp = 256;
        public int ZipperLow = 0;
        //private string label;
        //  public bool ButtonPressed;

        public int Value;
        Vector2 ButtonPosition;

        private Color color1;
        private Color color2;

        static private Texture2D texture_zipper;
        static private Texture2D texture_zipper_button;

        public Zipper(Vector2 _position, Color col1, Color col2)
        {

            color1 = col1;
            color2 = col2;

            Position = _position;
            ButtonPosition = Position;
        }

        public static void LoadContent(ContentManager Content)
        {
            texture_zipper = Content.Load<Texture2D>("zipper");
            texture_zipper_button = Content.Load<Texture2D>("zipper_button");
        }
        public void Check()
        {
            Vector2 Pos = Camera.ptMouseCurrent;
            Vector2 Pressed = Camera.ptMousePressed;

            //Check if presed is in range of zipper button
            if (!Camera.flag_release)
                if (Pressed.X > Position.X - ButtonRadious && Pressed.X < Position.X + ButtonRadious &&
                  Pressed.Y > Position.Y - ZipperLenght && Pressed.Y < Position.Y + ZipperLenght)
                {
                    //  ButtonPressed = true;

                    if (!Camera.flag_release & !Camera.flag_preset)
                    {
                        ButtonPosition.X = Position.X;
                        ButtonPosition.Y = Pos.Y;

                        //Borders
                        if (ButtonPosition.Y > Position.Y + ZipperLow)
                            ButtonPosition.Y = Position.Y + ZipperLow;

                        if (ButtonPosition.Y < Position.Y - ZipperUp)
                            ButtonPosition.Y = Position.Y - ZipperUp;

                        Value = (int)(Position.Y - Pos.Y);
                        if (Value > ZipperUp) Value = ZipperUp;
                        if (Value < ZipperLow) Value = ZipperLow;
                    }
                }

            //if (Camera.flag_release)
            //{
            //    ButtonPressed = false;
            //}
        }

        public void Draw(float orientation = 0)
        {

            Vector2 corr = new Vector2(20, -ZipperLenght);
            GraphiscHelper.FiledRectangle(Position + corr, 20, ZipperLenght, 0, color2, color1);
            corr = new Vector2(25, -ZipperLenght - 50);
            int l = Value.ToString().Length * 18;
            spriteBatch.DrawString(spriteFont, "" + Value, Position + corr, Color.DeepPink, 0, new Vector2(l / 2, 0), 1.0f, SpriteEffects.None, .9f);

            spriteBatch.Draw(texture_zipper_button, ButtonPosition, null, Color.FromNonPremultiplied(new Vector4(.9f, .9f, .9f, .8f)), 0, new Vector2(0, texture_zipper_button.Height / 2), 1, SpriteEffects.None, 1);
            //background
            //spriteBatch.Draw(texture_zipper , Position - Correction, null, Color.FromNonPremultiplied(new Vector4(.9f, .9f, .9f, .8f)), 0, new Vector2(40, texture_zipper.Height / 2 - 15), 1, SpriteEffects.None, 1);
        }

        public void SetButtoon(int value)
        {
            Value = value;
            ButtonPosition.Y = Position.Y - Value;
        }
    }
}
