using OpenTK;
using System;
using Vivid.Draw;
using Vivid.Texture;

namespace Vivid.Resonance.Forms
{
    public class ButtonForm : UIForm
    {
        private bool Pressed = false, Over = false;
        private Vector4 NormCol = new Vector4(0.7f,0.7f, 0.7f, 1f);
        private Vector4 OverCol = new Vector4(1f, 1f, 1f, 1f);
        private Vector4 PressCol = new Vector4(0.2f, 1.6f, 1.6f, 1);
        public static Vivid.Audio.VSoundSource BleepSrc;
        public static Vivid.Audio.VSoundSource BingSrc;
        public static Texture2D ButTex = null;

        public ButtonForm()
        {
            Selectable = true;
            if (BleepSrc == null)
            {
                //BleepSrc = new Vivid.Audio.Songs.
                BleepSrc = Vivid.Audio.Songs.LoadSound("Corona/Sound/UI/beep1.wav");
                BingSrc = Vivid.Audio.Songs.LoadSound("Corona/Sound/UI/click1.wav");
            }
            if (Font == null)
            {
                Font = new Font2.OrchidFont("data/font/font1.ttf", 12);
            }

            if (ButTex == null)
            {
                ButTex = new Texture2D("data/nxUI/button/button4.png", LoadMethod.Single, true);
            }
            SetImage(ButTex);
            Col = NormCol;

            void DrawFunc()
            {
                Pen2D.BlendMod = PenBlend.Alpha;

             //   DrawFormSolid(new Vector4(0, 0, 0, 1));
                DrawForm(CoreTex,new Vector4(Col.X * UI.CurUI.FadeAlpha,Col.Y * UI.CurUI.FadeAlpha,Col.Z * UI.CurUI.FadeAlpha,UI.CurUI.FadeAlpha) ,1, 1, W - 2, H - 2);
                //if (Text == "") return;
               
                //DrawText(Text, (W / 2 - Font.Width(Text) / 2)+4, (H / 2 - Font.Height())+4, new Vector4(0, 0, 0, 1));
                DrawText(Text, W / 2 - Font.Width(Text) / 2, H / 2 - Font.Height(), new Vector4(0, 1*UI.CurUI.FadeAlpha, 1*UI.CurUI.FadeAlpha, 1*UI.CurUI.FadeAlpha));
            }

            void MouseEnterFunc()
            {
                if (Pressed == false)
                {
                    Col = OverCol;
                }
                Vivid.Audio.Songs.PlaySource(BingSrc);
                Over = true;
            }

            void MouseLeaveFunc()
            {
                if (Pressed == false)
                {
                    Col = NormCol;
                }
                Over = false;
            }

            void MouseMoveFunc(int x, int y, int dx, int dy)
            {
                if (Pressed)
                {
                    // Drag?.Invoke(dx, dy);
                }
            }

            void MouseDownFunc(int b)
            {
                Col = PressCol;
                Vivid.Audio.Songs.PlaySource(BleepSrc);
                Pressed = true;
            }

            void MouseUpFunc(int b)
            {
                if (Over)
                {
                    Col = OverCol;
                }
                else
                {
                    Col = NormCol;
                }
                Pressed = false;
                Console.WriteLine("CLicked!");
                if (Click != null)
                {
                    Console.WriteLine("Has click");
                }
                Click?.Invoke(b);
            }

            Draw = DrawFunc;
            MouseEnter = MouseEnterFunc;
            MouseLeave = MouseLeaveFunc;
            MouseMove = MouseMoveFunc;
            MouseDown = MouseDownFunc;
            MouseUp = MouseUpFunc;
        }
    }
}