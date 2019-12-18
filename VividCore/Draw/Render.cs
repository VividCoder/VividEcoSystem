﻿using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Vivid.App;
using Vivid.Tex;

namespace Vivid.Draw
{
    public enum BlendMode
    {
        None, Alpha, Add, Mod, Sub, SoftLight
    }

    public static class Render
    {
        public static Vector4 Col = new Vector4(1, 1, 1, 1);

        public static void To2D()
        {
            Matrix4 pm = Matrix4.CreateOrthographicOffCenter(0, Vivid.App.AppInfo.RW, AppInfo.RH, 0, 0, 1);

            //pm = Matrix4.CreateOrthographic(StarKnightsAPP.RW, StarKnightsAPP.RH, 0, 1);

            // GL.MatrixMode(MatrixMode.Modelview);
            // GL.LoadIdentity();
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            //GL.MultMatrix(ref pm);
            // GL.Ortho(0, StarKnightsAPP.RW, StarKnightsAPP.RH, 0, -200, 1);
        }

        public static void SetBlend(BlendMode mode)
        {
            switch (mode)
            {
                case BlendMode.None:
                    GL.Disable(EnableCap.Blend);

                    break;

                case BlendMode.Alpha:
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                    break;

                case BlendMode.Add:
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                    break;

                case BlendMode.SoftLight:
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);
                    break;
            }
        }

        public static void Image(Vector2[] p, Tex2D img,Tex2D norm,float z=1.0f)
        {
            float[] x = new float[4];
            float[] y = new float[4];
            for (int i = 0; i < 4; i++)
            {
                x[i] = p[i].X;
                y[i] = p[i].Y;
            }
            Image(x, y, img,norm,z);
        }
        public static void Begin()
        {
            Draw.IntelliDraw.BeginDraw();
        }

        public static void End2D()
        {
            Draw.IntelliDraw.EndDraw2D();

        }

        public static void Image(float[] xc, float[] yc, Tex2D img,Tex2D norm,float z=1.0f)
        {
            //img.Bind(0);



            Draw.IntelliDraw.DrawImg2D(xc, yc, img,norm, new Vector4(1, 1, 1, 1),z);


            //'  Col = new Vector4(1, 1, 1, 0.5f);
            /*
            GL.Color4(Col);

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 0);
            GL.Vertex2(xc[0], yc[0]);
            GL.TexCoord2(1, 0);
            GL.Vertex2(xc[1] , yc[1]);
            GL.TexCoord2(1, 1);
            GL.Vertex2(xc[2],yc[2]);
            GL.TexCoord2(0, 1);
            GL.Vertex2(xc[3],yc[3]);

            GL.End();
            */
            //img.Unbind(0);
        }

        static Tex2D NormBlank = null;
        public static void Image(int x, int y, int w, int h, Tex2D tex,Tex2D norm=null)
        {
            if(norm == null)
            {
                if(NormBlank == null)
                {
                   // NormBlank = new Tex2D("data/ui/normblank.png", false);
                }
                //norm = NormBlank;
            }
            
            float[] cx = new float[4];
            float[] cy = new float[4];

            cx[0] = x;
            cy[0] = y;
            cx[1] = x + w;
            cy[1] = y;
            cx[2] = x + w;
            cy[2] = y + h;
            cx[3] = x;
            cy[3] = y + h;

            Draw.IntelliDraw.DrawImg2D(cx, cy, tex, norm, new Vector4(1, 1, 1, 1), 1.0f) ;


            //tex.Bind(0);

            //'  Col = new Vector4(1, 1, 1, 0.5f);
            /*
              GL.Color4(Col);

              GL.Begin(PrimitiveType.Quads);

              GL.TexCoord2(0, 0);
              GL.Vertex2(x, y);
              GL.TexCoord2(1, 0);
              GL.Vertex2(x + w, y);
              GL.TexCoord2(1, 1);
              GL.Vertex2(x + w, y + h);
              GL.TexCoord2(0, 1);
              GL.Vertex2(x, y + h);

              GL.End();

      */
           // tex.Unbind(0);
        }

        public static void Rect(int x, int y, int w, int h)
        {
            /*
            GL.Color4(Col);

            GL.Begin(PrimitiveType.Quads);

            GL.Vertex2(x, y);
            GL.Vertex2(x + w, y);
            GL.Vertex2(x + w, y + h);
            GL.Vertex2(x, y + h);

            GL.End();
            */
        }
    }
}