//***********************************************************************
//                        CIF program
//                Created by Daniel M. Kaminski
//                        Lublin 2023
//                     Under GNU licence
//***********************************************************************


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Cif
{
    public class Button
    {
        public bool state;
        public bool state_active_drawing;

        public bool ButtonPressed;
        public Vector2 Position;
        private const int ButtonRadious = 50;

        private string label1;

        public static SpriteFont spriteFont;
        public static SpriteBatch spriteBatch;
        private Texture2D texture1;
        private Texture2D texture2;

        public Button()
        {
            //Button logic
            state = false;
            ButtonPressed = false;
        }

        public bool CheckButton()
        {
            Vector2 Pos = Camera.ptMouseCurrent;

            if (Pos.X > Position.X - ButtonRadious && Pos.X < Position.X + ButtonRadious &&
                Pos.Y > Position.Y - ButtonRadious && Pos.Y < Position.Y + ButtonRadious &&
                !ButtonPressed)

            {
                state = true;
                ButtonPressed = true;
                state_active_drawing = false;
            }

            if (Pos.X == 0 && Pos.Y == 0)
                ButtonPressed = false;

            return ButtonPressed;
        }

        public bool CheckButton2()
        {
            Vector2 Pos = Camera.ptMouseCurrent;

            if (Pos.X > Position.X - ButtonRadious && Pos.X < Position.X + ButtonRadious &&
                Pos.Y > Position.Y - ButtonRadious && Pos.Y < Position.Y + ButtonRadious &&
                !ButtonPressed)

                if (state)
                {
                    state = false;
                    ButtonPressed = true;
                }
                else
                {
                    state = true;
                    ButtonPressed = true;
                }

            if (Pos.X == 0 && Pos.Y == 0) ButtonPressed = false;
            return ButtonPressed;
        }
        public void ShowButton(float orientation = 0)
        {
            Vector2 correction = new Vector2(ButtonRadious, ButtonRadious);

            if (!state)
            {
                spriteBatch.Draw(texture1, Position - correction, null, Color.FromNonPremultiplied(new Vector4(.9f, .9f, .9f, .7f)), 0, new Vector2(0, 0), 2, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(texture2, Position - correction, null, Color.FromNonPremultiplied(new Vector4(.9f, .9f, .9f, .7f)), orientation, new Vector2(0, 0), 2, SpriteEffects.None, 1);
                state_active_drawing = true;
            }

        }

        public void InitializeButton(Vector2 Position_, string label1_, Texture2D texture2_, Texture2D texture1_)
        {
            Position = Position_;
            texture1 = texture1_;
            texture2 = texture2_;
        }
    }
}