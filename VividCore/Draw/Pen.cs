using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using Vivid.App;
using Vivid.Effect;
using Vivid.Texture;

namespace Vivid.Draw
{
    public class QuadHold
    {
        public int X, Y, W, H;
        public float R, G, B, A;
        public int VA, VB;
        public int LastUsed;
    }

    public class FXQuadBlur2 : Effect3D
    {
        public Vector4 Col = Vector4.One;
        public float Blur = 0.2f;
        public bool Bound = false;

        public FXQuadBlur2() : base("", "data/Shader/blur2VS.glsl", "data/Shader/blur2FS.glsl")
        {
        }

        public override void SetPars()
        {
            SetTex("tR", 0);
            SetVec4("col", Col);
            SetFloat("blur", Blur);
            SetMat("proj", Matrix4.CreateOrthographicOffCenter(0, AppInfo.RW, AppInfo.RH, 0, -1, 1));
        }
    }

    public class FXQuadBlur : Effect3D
    {
        public bool Bound = false;
        public Vector4 Col = Vector4.One;
        public float Blur = 0.2f;
        public float Refract = 0.25f;

        public FXQuadBlur() : base("", "data/Shader/blurVS.glsl", "data/Shader/blurFS.glsl")
        {
        }

        public override void SetPars()
        {
            SetTex("tR", 0);
            SetTex("tB", 1);
            SetTex("tN", 2);
            SetFloat("blur", Blur);
            SetFloat("refract", Refract);
            SetBool("refractOn", Refract > 0);
            SetVec4("col", Col);
            SetMat("proj", Matrix4.CreateOrthographicOffCenter(0, AppInfo.RW, AppInfo.RH, 0, -1, 1));
        }
    }

    public class FXQuad : Effect3D
    {
        public Vector4 Col = Vector4.One;
        public bool Bound = false;

        public FXQuad() : base("", "data/Shader/drawVS.txt", "data/Shader/drawFS.txt")
        {
        }

        public override void SetPars()
        {
            SetTex("tR", 0);
            SetVec4("col", Col);
            SetMat("proj", Matrix4.CreateOrthographicOffCenter(0, AppInfo.RW, AppInfo.RH, 0, -1, 1));
            // Console.WriteLine("OW:" + AppInfo.RW + " OH:" + AppInfo.RH);
            // Console.WriteLine("W:" + AppInfo.RW + " H:" + AppInfo.RH);
        }
    }

    public enum PenBlend
    {
        Solid, Alpha, Additive, Modulate, ModulateX2, ModulateX4, Subtract, Burn
    }

    public static class Pen2D
    {
        public static Color4 ForeCol = Color4.White;
        public static Color4 BackCol = Color4.Black;
        public static PenBlend BlendMod = PenBlend.Solid;
        public static Matrix4 DrawMat = Matrix4.Identity;
        public static Matrix4 PrevMat = Matrix4.Identity;
        public static FXQuad QFX = null;
        public static FXQuadBlur BFX = null;
        public static FXQuadBlur2 BFX2 = null;
        public static int qva = -1, qvb = -1;
        public static Texture2D WhiteTex = null;
        public static uint[] ind = new uint[4];
        public static int inb = 0;

        public static void InitDraw()
        {
            QFX = new FXQuad();
            BFX = new FXQuadBlur();
            BFX2 = new FXQuadBlur2();
            WhiteTex = new Texture2D("data/ui/skin/white.png", LoadMethod.Single);

            ind[0] = 0;
            ind[1] = 1;
            ind[2] = 2;
            ind[3] = 3;

            qva = GL.GenVertexArray();
            qvb = GL.GenBuffer();
            inb = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, inb);
            GL.BufferData(BufferTarget.ElementArrayBuffer, 4 * 4, ind, BufferUsageHint.StaticDraw);

            //      qd[0] = x;
            //        qd[1] = y;
            qd[2] = 0;

            qd[3] = 0;
            qd[4] = 0;
            //    qd[5] = c1.X;
            //   qd[6] = c1.Y;
            //  qd[7] = c1.Z;
            // qd[8] = c1.W;

            // qd[9] = x + w;
            // qd[10] = y;
            qd[11] = 0;
            qd[12] = 1;
            qd[13] = 0;
            //   qd[14] = c1.X;
            //  qd[15] = c1.Y;
            // qd[16] = c1.Z;
            //qd[17] = c1.W;

            //  qd[18] = x + w;
            //  qd[19] = y + h;
            qd[20] = 0;
            qd[21] = 1;
            qd[22] = 1;
            // qd[23] = c2.X;
            // qd[24] = c2.Y;
            // qd[25] = c2.Z;
            // qd[26] = c2.W;

            //  qd[27] = x;
            //  qd[28] = y + h;
            qd[29] = 0;
            qd[30] = 0;
            qd[31] = 1;
            //  qd[32] = c2.X;
            //  qd[33] = c2.Y;
            //  qd[34] = c2.Z;
            //  qd[35] = c2.W;
        }

        private static float[] qd = new float[36];

        public static void DraqQuadBlur2()
        {
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);

            SetVP.Set(0, 0, AppInfo.W, AppInfo.H);
            //  GL.Disable(EnableCap.Blend);
            //GL.Disable(EnableCap.)

            // WhiteTex.Bind(0);

            //BFX.Refract = refract;
            //         Console.WriteLine("R2:" + refract);
            //           BFX.Blur = blur;

            BFX2.Bind();

            GL.BindVertexArray(qva);

            GL.BindBuffer(BufferTarget.ArrayBuffer, qvb);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * 4, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 9 * 4, 3 * 4);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 9 * 4, 5 * 4);

            uint[] ind = new uint[4];
            ind[0] = 0;
            ind[1] = 1;
            ind[2] = 2;
            ind[3] = 3;
            GL.DrawElements<uint>(PrimitiveType.Quads, 4, DrawElementsType.UnsignedInt, ind);
            //GL.DrawArrays(PrimitiveType.Quads, 0, 4);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            BFX2.Release();

            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
        }

        public static void DrawQuadBlur(float blur, float refract = 0)
        {
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);

            SetVP.Set(0, 0, AppInfo.W, AppInfo.H);
            //  GL.Disable(EnableCap.Blend);
            //GL.Disable(EnableCap.)

            // WhiteTex.Bind(0);

            BFX.Refract = refract;

            BFX.Blur = blur;

            BFX.Bind();

            GL.BindVertexArray(qva);

            GL.BindBuffer(BufferTarget.ArrayBuffer, qvb);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * 4, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 9 * 4, 3 * 4);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 9 * 4, 5 * 4);

            uint[] ind = new uint[4];
            ind[0] = 0;
            ind[1] = 1;
            ind[2] = 2;
            ind[3] = 3;
            GL.DrawElements<uint>(PrimitiveType.Quads, 4, DrawElementsType.UnsignedInt, ind);
            //GL.DrawArrays(PrimitiveType.Quads, 0, 4);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            BFX.Release();

            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
        }

        public static void UpdateCache()
        {
            System.Collections.Generic.List<QuadHold> rem = new System.Collections.Generic.List<QuadHold>();
            int time = Environment.TickCount;

            foreach (var q in Quads)
            {
                if ((q.LastUsed + 500) < time)
                {
                    rem.Add(q);
                }
            }
            foreach (var rq in rem)
            {
                Quads.Remove(rq);
            }
        }

        public static bool disableflag = false;

        public static void DrawQuad()
        {
            if (disableflag == false)
            {
                GL.Disable(EnableCap.CullFace);
                GL.Disable(EnableCap.DepthTest);
                disableflag = true;
            }
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);
            SetVP.Set(0, 0, AppInfo.W, AppInfo.H);

            //  GL.Disable(EnableCap.Blend);
            //GL.Disable(EnableCap.)

            // WhiteTex.Bind(0);

            QFX.Bind();

            if (boundva == false)
            {
                boundva = true;
                GL.BindVertexArray(qva);
            }
            if (boundvb == false)
            {
                boundvb = true;
                GL.BindBuffer(BufferTarget.ArrayBuffer, qvb);
            }
            if (boundaa == false)
            {
                //boundaa = true;
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * 4, 0);

                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 9 * 4, 3 * 4);

                GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 9 * 4, 5 * 4);
            }

            // GL.DrawElements<uint>(PrimitiveType.Quads, 4, DrawElementsType.UnsignedInt, ind);
            //GL.DrawArrays(PrimitiveType.Quads, 0, 4);
            if (boundin == false)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, inb);
                // boundin = true;
            }
            GL.DrawElements(BeginMode.Quads, 4, DrawElementsType.UnsignedInt, 0);
            // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //   QFX.Release ( );

            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);

            // WhiteTex.Release(0);
        }

        public static bool boundaa = false;

        public static void Line(int x, int y, int x2, int y2)
        {
            Line(x, y, x2, y2, Vector4.One);
        }

        public static void Line(int x, int y, int x2, int y2, Vector4 c)
        {
            Line(x, y, x2, y2, c, c);
        }

        public static void Line(int x, int y, int x2, int y2, Vector4 c1, Vector4 c2)
        {
            return;
            float a1 = x;
            float b1 = y;
            float a2 = x2;
            float b2 = y2;

            float steps = 0;
            float d1 = Math.Abs(a2 - a1);
            float d2 = Math.Abs(b2 - b1);
            if (d1 > d2)
            {
                steps = d1;
            }
            else
            {
                steps = d2;
            }
            float xi = (a2 - a1) / steps;
            float yi = (b2 - b1) / steps;
            float dx = a1;
            float dy = b1;
            WhiteTex.Bind(0);
            Vector4 vc = Vector4.One;
            float cr1 = c1.X;
            float cg1 = c1.Y;
            float cb1 = c1.Z;
            float ca1 = c1.W;
            float ri = (c2.X - cr1) / steps;
            float gi = (c2.Y - cg1) / steps;
            float bi = (c2.Z - cb1) / steps;
            float ai = (c2.W - ca1) / steps;

            xi *= 2;
            yi *= 2;
            ri *= 2;
            gi *= 2;
            bi *= 2;
            ai *= 2;

            for (int i = 0; i < steps; i += 2)
            {
                // RectRaw((int)dx,(int) dy, 2, 2, Vector4.One,Vector4.One);

                vc.X = cr1;
                vc.Y = cg1;
                vc.Z = cb1;
                vc.W = ca1;
                //GenQuad ( ( int ) dx, ( int ) dy, 2, 2, vc, vc );

                // DrawQuad ( );

                dx += xi;
                dy += yi;
                cr1 += ri;
                cg1 += gi;
                cb1 += bi;
                ca1 += ai;
            }
            WhiteTex.Release(0);
        }

        // public float[] qd = new float[36];
        public static System.Collections.Generic.List<QuadHold> Quads = new System.Collections.Generic.List<QuadHold>();

        public static void GenQuad(int x, int y, int w, int h, Vector4 c1, Vector4 c2)
        {
            /*
            QuadHold oq = new QuadHold();
            oq.LastUsed = -1;
            foreach(var qh in Quads)
            {
                if(qh.X == x && qh.Y == y && qh.W == w && qh.H == h && qh.R == c1.X && qh.G == c1.Y && qh.B == c1.Z && qh.A == c1.W)
                {
                    oq = qh;
                    break;
                }
            }
            if (oq.LastUsed > -1)
            {
                oq.LastUsed = Environment.TickCount;
                return oq;
            }

            QuadHold nq = new QuadHold
            {
                X = x,
                Y = y,
                W = w,
                H = h,
                R = c1.X,
                G = c1.Y,
                B = c1.Z,
                A = c1.W,
                LastUsed = Environment.TickCount
            };

            Quads.Add(nq);

            nq.VA = GL.GenVertexArray ( );
            */

            if (boundva == false)
            {
                boundva = true;
                GL.BindVertexArray(qva);
            }

            qd[0] = x;
            qd[1] = y;

            qd[5] = c1.X;
            qd[6] = c1.Y;
            qd[7] = c1.Z;
            qd[8] = c1.W;

            qd[9] = x + w;
            qd[10] = y;

            qd[14] = c1.X;
            qd[15] = c1.Y;
            qd[16] = c1.Z;
            qd[17] = c1.W;

            qd[18] = x + w;
            qd[19] = y + h;

            qd[23] = c2.X;
            qd[24] = c2.Y;
            qd[25] = c2.Z;
            qd[26] = c2.W;

            qd[27] = x;
            qd[28] = y + h;

            qd[32] = c2.X;
            qd[33] = c2.Y;
            qd[34] = c2.Z;
            qd[35] = c2.W;

            /*
            qd[20] = x;
            qd[21] = y+h;
            qd[22] = z;
            qd[23] = 0;
            qd[24] = 1;

            qd[25] = x;
            qd[26] = y;
            qd[27] = z;
            qd[28] = 0;
            qd[29] = 0;
            */

            if (boundvb == false)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, qvb);
                boundvb = true;
            }
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(36 * 4), qd, BufferUsageHint.DynamicDraw);
            // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public static bool boundva = false, boundvb = false, boundin = false;

        public static void Unbindv()
        {
            boundva = false;
            boundvb = false;
            boundaa = false;
            boundin = false;
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            disableflag = false;
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public static void SetProj(int x, int y, int w, int h)
        {
            DrawMat = Matrix4.CreateOrthographicOffCenter(x, x + w, y + h, y, 0, 1);
        }

        public static void Bind()
        {
            // GL.Color4(ForeCol);
            switch (BlendMod)
            {
                case PenBlend.Solid:

                    GL.Disable(EnableCap.Blend);
                    break;

                case PenBlend.Alpha:
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                    break;

                case PenBlend.Additive:

                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                    break;
            }

            //  GL.MatrixMode(MatrixMode.Projection);
            //  GL.LoadMatrix(ref DrawMat);
            // GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();
        }

        public static void Release()
        {
        }

        public static void Rect(int x, int y, int w, int h, Texture2D img)
        {
            Rect(x, y, w, h, img, Vector4.One);
        }

        public static void Rect(int x, int y, int w, int h, Texture2D img, Vector4 c)
        {
            Rect(x, y, w, h, img, c, c);
        }

        public static void RectRaw(int x, int y, int w, int h, Vector4 t1, Vector4 t2)
        {
            GenQuad(x, y, w, h, t1, t2);

            // DrawQuad ( );

            // GL.Begin(BeginMode.Quads);
            // GL.Vertex2(x, y);
            //GL.Vertex2(x + width, y);
            //GL.Vertex2(x + width, y + height);
            //GL.Vertex2(x, y + height);
            //GL.End();
        }

        public static void RectBlurRefract(int x, int y, int w, int h, Texture2D img, Texture2D bimg, Texture2D nimg, Vector4 tc, Vector4 bc, float blur, float refract)
        {
            BFX.Col = tc;

            Bind();
            GenQuad(x, y, w, h, tc, bc);
            img.Bind(0);
            bimg.Bind(1);
            nimg.Bind(2);
            DrawQuadBlur(blur, refract);
            nimg.Release(2);
            bimg.Release(1);
            img.Release(0);
            // GL.Begin(BeginMode.Quads);
            // GL.Vertex2(x, y);
            //GL.Vertex2(x + width, y);
            //GL.Vertex2(x + width, y + height);
            //GL.Vertex2(x, y + height);
            //GL.End();
            Release();
        }

        public static void RectBlur2(int x, int y, int w, int h, Texture2D img, Vector4 col, float blur)
        {
            BFX2.Col = col;
            BFX2.Blur = blur;
            Bind();
            GenQuad(x, y, w, h, col, col);
            img.Bind(0);
            DraqQuadBlur2();
            img.Release(0);
            Release();
        }

        public static void RectBlur(int x, int y, int w, int h, Texture2D img, Texture2D bimg, Vector4 tc, Vector4 bc, float blur)
        {
            BFX.Col = tc;
            BFX.Refract = 0;
            Bind();
            GenQuad(x, y, w, h, tc, bc);
            img.Bind(0);
            bimg.Bind(1);
            DrawQuadBlur(blur);
            bimg.Release(1);
            img.Release(0);
            // GL.Begin(BeginMode.Quads);
            // GL.Vertex2(x, y);
            //GL.Vertex2(x + width, y);
            //GL.Vertex2(x + width, y + height);
            //GL.Vertex2(x, y + height);
            //GL.End();
            Release();
        }

        public static void Rect(int x, int y, int w, int h, Texture2D img, Vector4 tc, Vector4 bc)
        {
            QFX.Col = tc;
            Bind();
            //  IntPtr q = Native.CBridge.genQuad(x, y, w, h);
            GenQuad(x, y, w, h, tc, bc);
            img.Bind(0);
            DrawQuad();
            img.Release(0);
            // GL.Begin(BeginMode.Quads);
            // GL.Vertex2(x, y);
            //GL.Vertex2(x + width, y);
            //GL.Vertex2(x + width, y + height);
            //GL.Vertex2(x, y + height);
            //GL.End();
            Release();
        }

        public static void Rect(int x, int y, int w, int h, Vector4 tc, Vector4 bc)
        {
            QFX.Col = tc;
            Bind();

            GenQuad(x, y, w, h, tc, tc);

            WhiteTex.Bind(0);

            DrawQuad();

            WhiteTex.Release(0);
            // GL.Begin(BeginMode.Quads);
            // GL.Vertex2(x, y);
            //GL.Vertex2(x + width, y);
            //GL.Vertex2(x + width, y + height);
            //GL.Vertex2(x, y + height);
            //GL.End();
            Release();
        }

        public static void Rect(int x, int y, int width, int height, Vector4 col)
        {
            Rect(x, y, width, height, col, col);
        }

        public static void Rect(float x, float y, float w, float h, Vector4 col)
        {
            Rect((int)x, (int)y, (int)w, (int)h, col);
        }

        public static void Rect(float x, float y, float w, float h, Vector4 tc, Vector4 bc)
        {
            Rect((int)x, (int)y, (int)w, (int)h, tc, bc);
        }

        public static void Rect(float x, float y, float w, float h, Texture2D img)
        {
            Rect((int)x, (int)y, (int)w, (int)h, img);
        }

        public static void Rect(float x, float y, float w, float h, Texture2D img, Vector4 col)
        {
            Rect((int)x, (int)y, (int)w, (int)h, img, col);
        }

        public static void Rect(float x, float y, float w, float h, Texture2D img, Vector4 tc, Vector4 bc)
        {
            Rect((int)x, (int)y, (int)w, (int)h, img, tc, bc);
        }

        public static void Line(float x, float y, float x2, float y2)
        {
            Line((int)x, (int)y, (int)x2, (int)y2);
        }

        public static void Line(float x, float y, float x2, float y2, Vector4 col)
        {
            Line((int)x, (int)y, (int)x2, (int)y2, col);
        }

        public static void Line(float x, float y, float x2, float y2, Vector4 c1, Vector4 c2)
        {
            Line((int)x, (int)y, (int)x2, (int)y2, c1, c2);
        }
    }
}