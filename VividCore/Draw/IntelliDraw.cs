using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using Vivid.Effect;
using Vivid.Tex;
using Vivid.Texture;

namespace Vivid.Draw
{
    public class DrawData
    {
        public Tex2D Img2D = null;
        public Tex2D Norm2D = null;
        public Texture2D Img = null;
        public int X, Y, W, H;
        public Vector4 Col = new Vector4(1, 1, 1, 1);
        public float Z = 0.0f;
        public float[] xc;
        public float[] yc;
        public bool flipuv = false;
        public bool simple = true;
    }

    public class DrawList
    {
        public List<DrawData> Data = new List<DrawData>();

        public void AddDraw(float[] xc, float[] yc, float z, Tex2D img,Tex2D norm, Vector4 col, bool flipuv = false)
        {
            DrawData draw_data = new DrawData();
            draw_data.Img2D = img;
            draw_data.Norm2D = norm;
            draw_data.Col = col;
            //draw_data.X = x;
            //draw_data.Y = y;
            //draw_data.W = w;
            //draw_data.H = h;
            draw_data.xc = xc;
            draw_data.yc = yc;
            draw_data.flipuv = flipuv;
            draw_data.simple = false;
            draw_data.Z = z;
            Data.Add(draw_data);
        }
        public void AddDraw(int x, int y, int w, int h, Texture2D img, Vector4 col, float z, bool flipuv = false)
        {
            DrawData draw_data = new DrawData();
            draw_data.Img = img;
            draw_data.Col = col;
            draw_data.X = x;
            draw_data.Y = y;
            draw_data.W = w;
            draw_data.H = h;
            draw_data.Z = z;
            draw_data.flipuv = flipuv;
            Data.Add(draw_data);
        }
    }

    public static class IntelliDraw
    {
        public static List<DrawList> Draws = new List<DrawList>();
        public static float Draw_Z = 0.0f;
        public static XQuad DrawFX = null;
        public static Texture2D EmptyTex;
        public static Texture2D ProxyTex = new Texture2D(32, 32, true);
        public static void BeginDraw(bool leavez = false)
        {
            if (begun)
            {
                EndDraw();
            }
            begun = true;
            Draws.Clear();
            if (!leavez)
            {
                Draw_Z = 0.02f;
            }
            if (DrawFX == null)
            {
                DrawFX = new XQuad();
            }
        }

        public static DrawList GetDrawList(Tex2D img)
        {

            foreach (var draw_list in Draws)
            {
                if (draw_list.Data[0].Img2D == img)
                {
                    return draw_list;
                }
            }

            var draw_listr = new DrawList();

            Draws.Add(draw_listr);

            return draw_listr;
        }

        public static DrawList GetDrawList(Texture2D img)
        {
            foreach (var draw_list in Draws)
            {
                if (draw_list.Data[0].Img == img)
                {
                    return draw_list;
                }
            }

            var draw_listr = new DrawList();

            Draws.Add(draw_listr);

            return draw_listr;
        }

        public static void DrawImg(int x, int y, int w, int h, Texture2D img, Vector4 col, bool flipuv = false)
        {
            if (!begun) return;
            var draw_list = GetDrawList(img);

            draw_list.AddDraw(x, y, w, h, img, col, Draw_Z, flipuv);

            Draw_Z += 0.002f;
        }

        public static void DrawImg2D(float[] xc, float[] yc, Tex.Tex2D img,Tex.Tex2D norm, Vector4 col,float z=1.0f)
        {

            if (!begun) return;
            var draw_list = GetDrawList(img);

            draw_list.AddDraw(xc, yc,z, img,norm, col);

            Draw_Z += 0.0002f;

        }

        public static void DrawImg(int x, int y, int w, int h)
        {

            if (!begun) return;
            var draw_list = GetDrawList(ProxyTex);

            draw_list.AddDraw(x, y, w, h, ProxyTex, new Vector4(1, 1, 1, 1), Draw_Z);

            Draw_Z += 0.002f;


        }

        static bool begun = false;

        public static void EndDraw2D()
        {
            if (!begun) return;
            begun = false;
            GL.Disable(EnableCap.DepthTest);
            //  GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);

            //GL.Viewport(0, 0, Vivid.App.AppInfo.W, Vivid.App.AppInfo.H);

            // DrawFX.Bind();
           

            foreach (var draw_list in Draws)
            {
                var vert_arr = GL.GenVertexArray();
                var vert_buf = GL.GenBuffer();
                // var ind_buf = GL.GenBuffer();

                int draw_c = draw_list.Data.Count * 4;

                int draw_i = 0;

                float[] vert_data = new float[draw_c * 9];
                uint[] ind_data = new uint[draw_c];

                int vert_i = 0;
                int int_i = 0;

                draw_list.Data[0].Img2D.Bind(0);
                if (draw_list.Data[0].Norm2D != null)
                {
                    draw_list.Data[0].Norm2D.Bind(2);
                }
                else{

                }

                foreach (var data in draw_list.Data)
                {
                    
                    for (int i = 0; i < 4; i++)
                    {
                        ind_data[int_i] = (uint)int_i++;
                    }

                    vert_data[vert_i++] = data.xc[0];
                    vert_data[vert_i++] = data.yc[0];
                    vert_data[vert_i++] = data.Z;

                    vert_data[vert_i++] = 0;
                    vert_data[vert_i++] = 0;

                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;

                    vert_data[vert_i++] = data.xc[1];
                    vert_data[vert_i++] = data.yc[1];
                    vert_data[vert_i++] = data.Z;

                    vert_data[vert_i++] = 1;
                    vert_data[vert_i++] = 0;

                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;

                    vert_data[vert_i++] = data.xc[2];
                    vert_data[vert_i++] = data.yc[2];
                    vert_data[vert_i++] = data.Z;

                    vert_data[vert_i++] = 1;
                    vert_data[vert_i++] = 1;

                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;

                    vert_data[vert_i++] = data.xc[3];
                    vert_data[vert_i++] = data.yc[3];
                    vert_data[vert_i++] = data.Z;

                    vert_data[vert_i++] = 0;
                    vert_data[vert_i++] = 1;

                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;
                }

                //   GL.BindBuffer(BufferTarget.ElementArrayBuffer, ind_buf);
                //  GL.BufferData(BufferTarget.ElementArrayBuffer, 4 * 4, ind_data, BufferUsageHint.StaticDraw);

                GL.BindVertexArray(vert_arr);

                GL.BindBuffer(BufferTarget.ArrayBuffer, vert_buf);

                GL.BufferData(BufferTarget.ArrayBuffer, vert_data.Length * 4, vert_data, BufferUsageHint.StaticDraw);

                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * 4, 0);

                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 9 * 4, 3 * 4);

                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 9 * 4, 5 * 4);

                GL.DrawElements<uint>(PrimitiveType.Quads, draw_c, DrawElementsType.UnsignedInt, ind_data);

                GL.DisableVertexAttribArray(0);
                GL.DisableVertexAttribArray(1);
                GL.DisableVertexAttribArray(2);

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                //  GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

                GL.DeleteBuffer(vert_buf);
                GL.DeleteVertexArray(vert_arr);
                // GL.DeleteBuffer(ind_buf);

                draw_list.Data[0].Img2D.Unbind(0);
            }


            // DrawFX.Release();
        }


        public static void EndDraw(Effect3D fx, Binder bind)
        {

            if (!begun) return;
            begun = false;
            GL.Enable(EnableCap.DepthTest);

            GL.Disable(EnableCap.CullFace);

            //  GL.Viewport(0, 0, Vivid.App.AppInfo.W, Vivid.App.AppInfo.H);

            fx.Bind();

            foreach (var draw_list in Draws)
            {
                var vert_arr = GL.GenVertexArray();
                var vert_buf = GL.GenBuffer();
                // var ind_buf = GL.GenBuffer();

                int draw_c = draw_list.Data.Count * 4;

                int draw_i = 0;

                float[] vert_data = new float[draw_c * 9];
                uint[] ind_data = new uint[draw_c];

                int vert_i = 0;
                int int_i = 0;

                bind?.Invoke();
                //draw_list.Data[0].Img.Bind(0);

                foreach (var data in draw_list.Data)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        ind_data[int_i] = (uint)int_i++;
                    }

                    vert_data[vert_i++] = data.X;
                    vert_data[vert_i++] = data.Y;
                    vert_data[vert_i++] = data.Z;

                    vert_data[vert_i++] = 0;
                    vert_data[vert_i++] = 0;

                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;

                    vert_data[vert_i++] = data.X + data.W;
                    vert_data[vert_i++] = data.Y;
                    vert_data[vert_i++] = data.Z;

                    vert_data[vert_i++] = 1;
                    vert_data[vert_i++] = 0;

                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;

                    vert_data[vert_i++] = data.X + data.W;
                    vert_data[vert_i++] = data.Y + data.H;
                    vert_data[vert_i++] = data.Z;

                    vert_data[vert_i++] = 1;
                    vert_data[vert_i++] = 1;

                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;

                    vert_data[vert_i++] = data.X;
                    vert_data[vert_i++] = data.Y + data.H;
                    vert_data[vert_i++] = data.Z;

                    vert_data[vert_i++] = 0;
                    vert_data[vert_i++] = 1;

                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;
                }

                //   GL.BindBuffer(BufferTarget.ElementArrayBuffer, ind_buf);
                //  GL.BufferData(BufferTarget.ElementArrayBuffer, 4 * 4, ind_data, BufferUsageHint.StaticDraw);

                GL.BindVertexArray(vert_arr);

                GL.BindBuffer(BufferTarget.ArrayBuffer, vert_buf);

                GL.BufferData(BufferTarget.ArrayBuffer, vert_data.Length * 4, vert_data, BufferUsageHint.StaticDraw);

                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * 4, 0);

                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 9 * 4, 3 * 4);

                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 9 * 4, 5 * 4);

                GL.DrawElements<uint>(PrimitiveType.Quads, draw_c, DrawElementsType.UnsignedInt, ind_data);

                GL.DisableVertexAttribArray(0);
                GL.DisableVertexAttribArray(1);
                GL.DisableVertexAttribArray(2);

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                //  GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

                GL.DeleteBuffer(vert_buf);
                GL.DeleteVertexArray(vert_arr);
                // GL.DeleteBuffer(ind_buf);

                // draw_list.Data[0].Img.Release(0);
            }


            fx.Release();
        }

        public delegate void Binder();
        public static void EndDraw3()
        {

            if (!begun) return;
            begun = false;
            GL.Enable(EnableCap.DepthTest);
            // GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);
            // GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            //GL.Viewport(0, 0, Vivid.App.AppInfo.W, Vivid.App.AppInfo.H);

            DrawFX.Bind();

            foreach (var draw_list in Draws)
            {
                var vert_arr = GL.GenVertexArray();
                var vert_buf = GL.GenBuffer();
                // var ind_buf = GL.GenBuffer();

                int draw_c = draw_list.Data.Count * 4;

                int draw_i = 0;

                float[] vert_data = new float[draw_c * 9];
                uint[] ind_data = new uint[draw_c];

                int vert_i = 0;
                int int_i = 0;

                draw_list.Data[0].Img.Bind(0);

                foreach (var data in draw_list.Data)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        ind_data[int_i] = (uint)int_i++;
                    }

                    vert_data[vert_i++] = data.X;
                    vert_data[vert_i++] = data.Y;
                    vert_data[vert_i++] = data.Z;

                    if (data.flipuv)
                    {
                        vert_data[vert_i++] = 0;
                        vert_data[vert_i++] = 1;
                    }
                    else
                    {
                        vert_data[vert_i++] = 0;
                        vert_data[vert_i++] = 0;
                    }
                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;

                    vert_data[vert_i++] = data.X + data.W;
                    vert_data[vert_i++] = data.Y;
                    vert_data[vert_i++] = data.Z;

                    if (data.flipuv)
                    {
                        vert_data[vert_i++] = 1;
                        vert_data[vert_i++] = 1;
                    }
                    else
                    {
                        vert_data[vert_i++] = 1;
                        vert_data[vert_i++] = 0;
                    }

                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;

                    vert_data[vert_i++] = data.X + data.W;
                    vert_data[vert_i++] = data.Y + data.H;
                    vert_data[vert_i++] = data.Z;

                    if (data.flipuv)
                    {
                        vert_data[vert_i++] = 1;
                        vert_data[vert_i++] = -0;

                    }
                    else
                    {
                        vert_data[vert_i++] = 1;
                        vert_data[vert_i++] = 1;
                    }
                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;

                    vert_data[vert_i++] = data.X;
                    vert_data[vert_i++] = data.Y + data.H;
                    vert_data[vert_i++] = data.Z;

                    if (data.flipuv)
                    {
                        vert_data[vert_i++] = 0;
                        vert_data[vert_i++] = 0;
                    }
                    else
                    {
                        vert_data[vert_i++] = 0;
                        vert_data[vert_i++] = 1;
                    }
                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;
                }

                //   GL.BindBuffer(BufferTarget.ElementArrayBuffer, ind_buf);
                //  GL.BufferData(BufferTarget.ElementArrayBuffer, 4 * 4, ind_data, BufferUsageHint.StaticDraw);

                GL.BindVertexArray(vert_arr);

                GL.BindBuffer(BufferTarget.ArrayBuffer, vert_buf);

                GL.BufferData(BufferTarget.ArrayBuffer, vert_data.Length * 4, vert_data, BufferUsageHint.StaticDraw);

                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * 4, 0);

                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 9 * 4, 3 * 4);

                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 9 * 4, 5 * 4);

                GL.DrawElements<uint>(PrimitiveType.Quads, draw_c, DrawElementsType.UnsignedInt, ind_data);

                GL.DisableVertexAttribArray(0);
                GL.DisableVertexAttribArray(1);
                GL.DisableVertexAttribArray(2);

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                //  GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

                GL.DeleteBuffer(vert_buf);
                GL.DeleteVertexArray(vert_arr);
                // GL.DeleteBuffer(ind_buf);

                draw_list.Data[0].Img.Release(0);
            }


            //  DrawFX.Release();
        }
        public static void EndDraw(Vivid.FX.VEffect fxOver = null)
        {
            if (!begun) return;
            begun = false;
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            //GL.Viewport(0, 0, Vivid.App.AppInfo.W, Vivid.App.AppInfo.H);

            if (fxOver != null)
            {
                fxOver.Bind();
            }
            else
            {
                DrawFX.Bind();
            }
            foreach (var draw_list in Draws)
            {
                var vert_arr = GL.GenVertexArray();
                var vert_buf = GL.GenBuffer();
                // var ind_buf = GL.GenBuffer();

                int draw_c = draw_list.Data.Count * 4;

                int draw_i = 0;

                float[] vert_data = new float[draw_c * 9];
                uint[] ind_data = new uint[draw_c];

                int vert_i = 0;
                int int_i = 0;

                draw_list.Data[0].Img.Bind(0);

                foreach (var data in draw_list.Data)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        ind_data[int_i] = (uint)int_i++;
                    }

                    vert_data[vert_i++] = data.X;
                    vert_data[vert_i++] = data.Y;
                    vert_data[vert_i++] = data.Z;

                    if (data.flipuv)
                    {
                        vert_data[vert_i++] = 0;
                        vert_data[vert_i++] = 1;
                    }
                    else
                    {
                        vert_data[vert_i++] = 0;
                        vert_data[vert_i++] = 0;
                    }
                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;

                    vert_data[vert_i++] = data.X + data.W;
                    vert_data[vert_i++] = data.Y;
                    vert_data[vert_i++] = data.Z;

                    if (data.flipuv)
                    {
                        vert_data[vert_i++] = 1;
                        vert_data[vert_i++] = 1;
                    }
                    else
                    {
                        vert_data[vert_i++] = 1;
                        vert_data[vert_i++] = 0;
                    }

                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;

                    vert_data[vert_i++] = data.X + data.W;
                    vert_data[vert_i++] = data.Y + data.H;
                    vert_data[vert_i++] = data.Z;

                    if (data.flipuv)
                    {
                        vert_data[vert_i++] = 1;
                        vert_data[vert_i++] = -0;

                    }
                    else
                    {
                        vert_data[vert_i++] = 1;
                        vert_data[vert_i++] = 1;
                    }
                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;

                    vert_data[vert_i++] = data.X;
                    vert_data[vert_i++] = data.Y + data.H;
                    vert_data[vert_i++] = data.Z;

                    if (data.flipuv)
                    {
                        vert_data[vert_i++] = 0;
                        vert_data[vert_i++] = 0;
                    }
                    else
                    {
                        vert_data[vert_i++] = 0;
                        vert_data[vert_i++] = 1;
                    }
                    vert_data[vert_i++] = data.Col.X;
                    vert_data[vert_i++] = data.Col.Y;
                    vert_data[vert_i++] = data.Col.Z;
                    vert_data[vert_i++] = data.Col.W;
                }

                //   GL.BindBuffer(BufferTarget.ElementArrayBuffer, ind_buf);
                //  GL.BufferData(BufferTarget.ElementArrayBuffer, 4 * 4, ind_data, BufferUsageHint.StaticDraw);

                GL.BindVertexArray(vert_arr);

                GL.BindBuffer(BufferTarget.ArrayBuffer, vert_buf);

                GL.BufferData(BufferTarget.ArrayBuffer, vert_data.Length * 4, vert_data, BufferUsageHint.StaticDraw);

                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * 4, 0);

                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 9 * 4, 3 * 4);

                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 9 * 4, 5 * 4);

                GL.DrawElements<uint>(PrimitiveType.Quads, draw_c, DrawElementsType.UnsignedInt, ind_data);

                GL.DisableVertexAttribArray(0);
                GL.DisableVertexAttribArray(1);
                GL.DisableVertexAttribArray(2);

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                //  GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

                GL.DeleteBuffer(vert_buf);
                GL.DeleteVertexArray(vert_arr);
                // GL.DeleteBuffer(ind_buf);

                draw_list.Data[0].Img.Release(0);
            }

            if (fxOver == null)
            {

                DrawFX.Release();
            }
            else
            {
                fxOver.Release();
            }
        }
    }

    public class XQuad : Effect3D
    {
        public Vector4 Col = Vector4.One;
        public bool Bound = false;

        public XQuad() : base("", "data/Shader/drawVS1.glsl", "data/Shader/drawFS1.glsl")
        {
        }

        public override void SetPars()
        {
            SetTex("tR", 0);

            SetMat("proj", Matrix4.CreateOrthographicOffCenter(0, Vivid.App.AppInfo.RW, Vivid.App.AppInfo.RH, 0, -1, 1000));
            // Console.WriteLine("OW:" + AppInfo.RW + " OH:" + AppInfo.RH);
            // Console.WriteLine("W:" + AppInfo.RW + " H:" + AppInfo.RH);
        }
    }
}