using OpenTK.Input;

using System.Collections.Generic;
using XInputDotNetPure;

namespace Vivid.Input
{
    public class XIn
    {

        public static bool Start()
        {

            var st = XInputDotNetPure.GamePad.GetState(PlayerIndex.One);
            if(st.Buttons.Start == XInputDotNetPure.ButtonState.Pressed)
            {
                int vv = 5;
            }
            return st.Buttons.Start == XInputDotNetPure.ButtonState.Pressed;

        }
        public static bool bY()
        {
            return XInputDotNetPure.GamePad.GetState(PlayerIndex.One).Buttons.Y == XInputDotNetPure.ButtonState.Pressed;
        }
        public static bool bA()
        {
            return XInputDotNetPure.GamePad.GetState(PlayerIndex.One).Buttons.A == XInputDotNetPure.ButtonState.Pressed;
        }
        public static bool bB()
        {
            return XInputDotNetPure.GamePad.GetState(PlayerIndex.One).Buttons.B == XInputDotNetPure.ButtonState.Pressed;
        }
        public static bool bX()
        {
            return XInputDotNetPure.GamePad.GetState(PlayerIndex.One).Buttons.X == XInputDotNetPure.ButtonState.Pressed;
        }
        public static bool leftB()
        {
            return XInputDotNetPure.GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed;
        }
        public static bool rightB()
        {
            return XInputDotNetPure.GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed;
        }
        public static void Rumble()
        {
            XInputDotNetPure.GamePad.SetVibration(PlayerIndex.One, 0, 0);
             XInputDotNetPure.GamePad.SetVibration(PlayerIndex.One, 100, 100);
            //XInputDotNetPure.GamePad.GetState(PlayerIndex.One).


        }

        public static bool DUp()
        {
            return XInputDotNetPure.GamePad.GetState(PlayerIndex.One).DPad.Up == XInputDotNetPure.ButtonState.Pressed;
        }
        public static bool DDown()
        {
            return XInputDotNetPure.GamePad.GetState(PlayerIndex.One).DPad.Down == XInputDotNetPure.ButtonState.Pressed;
        }
        public static bool DLeft()
        {
            return XInputDotNetPure.GamePad.GetState(PlayerIndex.One).DPad.Left == XInputDotNetPure.ButtonState.Pressed;
        }
        public static bool DRight()
        {
            return XInputDotNetPure.GamePad.GetState(PlayerIndex.One).DPad.Right == XInputDotNetPure.ButtonState.Pressed;
        }

        public static float rightX()
        {
            return XInputDotNetPure.GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X;
        }

        public static float rightY()
        {
            return XInputDotNetPure.GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;

        }

        public static float LeftX()
        {

            var st = XInputDotNetPure.GamePad.GetState(PlayerIndex.One);

            return st.ThumbSticks.Left.X;

        }

        public static float LeftY()
        {
            var st = XInputDotNetPure.GamePad.GetState(PlayerIndex.One);
            return st.ThumbSticks.Left.Y;
        }

    }
    public class Input
    {
        public static void InitInput()
        {
            for (int i = 0; i < 32; i++)
            {
                MB[i] = false;
            }
        }

        public static bool ShiftIn = false;
        public static int MX = 0, MY = 0;
        public static float MZ = 0;
        public static int MDX = 0, MDY = 0, MDZ = 0;
        public static bool[] MB = new bool[32];
        public static Dictionary<Key, bool> Keys = new Dictionary<OpenTK.Input.Key, bool>();

        public static bool AnyButtons()
        {
            for (int i = 0; i < 32; i++)
            {
                if (MB[i]) return true;
            }
            return false;
        }

        public static int ButtonNum()
        {
            for (int i = 0; i < 32; i++)
            {
                if (MB[i]) return i;
            }
            return -1;
        }

        public static void SetKey(Key key, bool i)
        {
            if (i)
            {
                if (Keys.ContainsKey(key))
                {
                }
                else
                {
                    Keys.Add(key, true);
                }
            }
            else
            {
                if (Keys.ContainsKey(key))
                {
                    Keys.Remove(key);
                }
            }
        }

        public static bool KeyIn(Key k)
        {
            if (Keys.ContainsKey(k))
            {
                return true;
            }

            return false;
        }

        public static List<Key> KeysIn()
        {
            List<Key> ki = new List<Key>();
            foreach (KeyValuePair<Key, bool> k in Keys)
            {
                ki.Add(k.Key);
            }
            return ki;
        }

        public static string ValidKeys = "abcdefghijklmnopqrstuvwxyzNumber1Number2Number3Number4Number5Number6Number7Number8Number9Number0!@#$%^&*()_,./<>?||   ";

        public static bool TextKey(Key k)
        {
            string ks = k.ToString().ToLower();
            if (ValidKeys.Contains(ks))
            {
                return true;
            }
            if (ValidKeys.Contains(k.ToString()))
            {
                return true;
            }
            return false;
        }

        public static bool IsShiftIn()
        {
            return ShiftIn;
            if (Keys.ContainsKey(Key.ShiftLeft))
            {
                return true;
            }
            if (Keys.ContainsKey(Key.ShiftRight))
            {
                return true;
            }
            return false;
        }

        public static Key KeyIn()
        {
            if (Keys.Count == 0)
            {
                return Key.LastKey;
            }

            if (Keys.Count > 1)
            {
                foreach (KeyValuePair<Key, bool> k in Keys)
                {
                    if (k.Key == Key.ShiftLeft || k.Key == Key.ShiftRight || k.Key == Key.LShift || k.Key == Key.RShift)
                    {
                    }
                    else
                    {
                        return k.Key;
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<Key, bool> k in Keys)
                {
                    if (k.Key == Key.ShiftLeft || k.Key == Key.ShiftRight || k.Key == Key.LShift || k.Key == Key.RShift)
                    {
                    }
                    else
                    {
                        return k.Key;
                    }
                }
            }
            return Key.LastKey;
        }

        public static bool IsKeyIn(Key k)
        {
            return Keys.ContainsKey(k);
        }

        public static bool AnyKey()
        {
            return Keys.Count > 0;
        }
    }
}