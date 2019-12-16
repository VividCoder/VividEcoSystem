﻿using OpenTK;

using System;

namespace Vivid.Resonance.Forms
{
    public delegate void OnEnter(string text);

    public class TextBoxForm : UIForm
    {
        public int ClaretI = 0;
        public bool Active = false;
        public int StartI = 0;
        public bool ShowClaret = false;
        public int NextClaret;
        public int ClaretE = 0;
        public OnEnter Enter = null;

        public TextBoxForm()
        {
            void KeyPressFunc(OpenTK.Input.Key key, bool shift)
            {
                string k = "";
                switch (key)
                {
                    case OpenTK.Input.Key.AltLeft:

                        break;

                    case OpenTK.Input.Key.Right:
                        if (Text.Length == 1)
                        {
                            if (ClaretI == 0)
                            {
                                ClaretI = 1;
                                return;
                            }
                        }
                        if (Text.Length == 0)
                        {
                            return;
                        }
                        if (Text.Length > 1)
                        {
                            if (ClaretI < Text.Length)
                            {
                                ClaretI++;
                            }
                        }
                        if (ClaretI > ClaretE)
                        {
                            if (StartI < Text.Length)
                            {
                                StartI++;
                            }
                        }
                        return;
                        break;

                    case OpenTK.Input.Key.Left:

                        if (ClaretI == StartI)
                        {
                            if (StartI > 0)
                            {
                                StartI--;
                            }
                        }
                        if (Text.Length == 1)
                        {
                            ClaretI = 0;
                            return;
                        }
                        if (Text.Length == 0)
                        {
                            return;
                        }

                        if (Text.Length > 1)
                        {
                            if (ClaretI > 0)
                            {
                                ClaretI--;
                                return;
                            }
                        }
                        return;
                        break;

                    case OpenTK.Input.Key.BackSpace:

                        if (Text.Length == 1)
                        {
                            ClaretI = 0;
                            Text = "";
                            return;
                        }
                        if (Text.Length > 1)
                        {
                            if (ClaretI == Text.Length)
                            {
                                Text = Text.Substring(0, Text.Length - 1);
                                ClaretI--;
                                return;
                            }
                            string os = Text.Substring(0, ClaretI - 1) + Text.Substring(ClaretI);
                            Text = os;
                            ClaretI--;
                            return;
                        }

                        break;

                    case OpenTK.Input.Key.Delete:
                        if (Text.Length == 1 && ClaretI == 0)
                        {
                            Text = "";
                            ClaretI = 0;
                            return;
                        }
                        if (Text.Length > 1 && ClaretI < Text.Length)
                        {
                            if ((Text.Length - ClaretI) > 1)
                            {
                                Text = Text.Substring(0, ClaretI) + Text.Substring(ClaretI + 1);
                            }
                            else
                            {
                                Text = Text.Substring(0, Text.Length - 1);
                            }
                            return;
                        }
                        return;
                        break;

                    case OpenTK.Input.Key.Comma:
                        k = ",";
                        if (shift) k = "<";
                        break;

                    case OpenTK.Input.Key.Period:
                        k = ".";
                        if (shift) k = ">";
                        break;

                    case OpenTK.Input.Key.Slash:
                        k = "/";
                        if (shift) k = "?";
                        break;

                    case OpenTK.Input.Key.Semicolon:
                        k = ";";
                        if (shift) k = ":";
                        break;

                    case OpenTK.Input.Key.BackSlash:
                    case OpenTK.Input.Key.NonUSBackSlash:
                        k = "\\";
                        if (shift) k = "|";
                        break;

                    case OpenTK.Input.Key.Space:

                        k = " ";
                        break;

                    case OpenTK.Input.Key.Quote:
                        k = "\"";
                        break;

                    case OpenTK.Input.Key.Minus:
                        k = "-";
                        if (shift) k = "_";
                        break;

                    case OpenTK.Input.Key.Plus:
                        k = "=";
                        if (shift) k = "+";
                        break;

                    case OpenTK.Input.Key.Number0:
                        k = "0";
                        if (shift) k = ")";
                        break;

                    case OpenTK.Input.Key.Number1:
                        k = "1";
                        if (shift) k = "!";
                        break;

                    case OpenTK.Input.Key.Number2:
                        k = "2";
                        if (shift) k = "@";
                        break;

                    case OpenTK.Input.Key.Number3:
                        k = "3";
                        if (shift) k = "#";
                        break;

                    case OpenTK.Input.Key.Number4:
                        k = "4";
                        if (shift) k = "$";
                        break;

                    case OpenTK.Input.Key.Number5:
                        k = "5";
                        if (shift) k = "%";
                        break;

                    case OpenTK.Input.Key.Number6:
                        k = "6";
                        if (shift) k = "^";
                        break; ;
                    case OpenTK.Input.Key.Number7:
                        k = "7";
                        if (shift) k = "&";
                        break;

                    case OpenTK.Input.Key.Number8:
                        k = "8";
                        if (shift) k = "*";
                        break;

                    case OpenTK.Input.Key.Number9:
                        k = "9";
                        if (shift) k = "(";
                        break;

                    case OpenTK.Input.Key.Enter:
                    case OpenTK.Input.Key.KeypadEnter:
                        Enter?.Invoke(Text);
                        ShowClaret = false;
                        Active = false;
                        return;
                        break;

                    default:

                        k = shift ? key.ToString().ToUpper() : key.ToString().ToLower();
                        break;
                }
                if (Text.Length == 0)
                {
                    Text = k;
                    ClaretI = 1;
                }
                else
                {
                    if (ClaretI == Text.Length)
                    {
                        Text = Text + k;
                        ClaretI++;

                        int iv = StartI;
                        string sw = Text.Substring(iv);
                        if (UI.Font.Width(sw) > W)
                        {
                            StartI++;
                        }
                        return;
                    }
                    string os = Text.Substring(0, ClaretI) + k;
                    if (Text.Length > ClaretI)
                    {
                        os = os + Text.Substring(ClaretI);
                    }
                    Text = os;
                    ClaretI++;
                }
            }
            KeyPress = KeyPressFunc;

            void UpdateFunc()
            {
                if (Active)
                {
                    if (Environment.TickCount > NextClaret)
                    {
                        ShowClaret = ShowClaret ? false : true;
                        NextClaret = Environment.TickCount + 450;
                        Console.WriteLine("Claret:" + ShowClaret.ToString());
                    }
                }
            }

            Update = UpdateFunc;

            void ActiveFunc()
            {
                Active = true;
            }

            void DeactiveFunc()
            {
                Active = false;
            }

            Deactivate = DeactiveFunc;
            Activate = ActiveFunc;

            void DrawFunc()
            {
                DrawFormSolid(Col);

                /*
                for (int i = StartI; i < Text.Substring(StartI).Length; i++)
                {
                    tw += UI.Font.Width(Text.Substring(i, 1));
                    if (tw > W - 10) break;
                    ii++;
                }
                */
                //vc = ClaretI - StartI;

                if (Text == null)
                {
                    Text = " ";
                    return;
                }

                string dis = Text.Substring(StartI);

                int cc = 0;
                int t2 = 0;
                string rtxt = "";
                for (int i = 0; i < dis.Length; i++)
                {
                    rtxt = rtxt + dis.Substring(i, 1);
                    string cr = dis.Substring(i, 1);
                    t2 += UI.Font.Width(cr);
                    if (t2 > W - 30)
                    {
                        break;
                    }
                    cc++;
                }
                ClaretE = cc;

                DrawText(rtxt, 5, 0, new Vector4(0.2f, 0.2f, 0.2f, 0.9f));

                if (Text.Length == 0)
                {
                    ClaretI = 0;
                }

                if (ShowClaret)
                {
                    // Console.WriteLine("Claret!");
                    int cx = 0;
                    if (Text.Length > 0)
                    {
                        int cv = 0;
                        for (int i = StartI; i < ClaretI; i++)
                        {
                            int cw = UI.Font.Width(Text.Substring(i, 1));
                            cx = cx + (cw);
                            cv++;
                            if (cv > cc)
                            {
                                break;
                            }
                        }
                    }
                    DrawFormSolid(new Vector4(0.2f, 0.2f, 0.2f, 0.8f), cx + (Text.Length > 0 ? 5 : 0), 0, 2, 20);
                }
            }

            Draw = DrawFunc;
        }
    }
}