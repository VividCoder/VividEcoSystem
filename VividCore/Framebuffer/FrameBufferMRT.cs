using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using Vivid.App;
using Vivid.Texture;

namespace Vivid.FrameBuffer
{
    public class FrameBufferColorMRT
    {
        public int FBO = 0;
        public Texture2D BB;
        public TextureDepth DB;
        public int IW, IH;
        public int DRB = 0;
        public List<Texture2D> Targets = new List<Texture2D>();
        public FrameBufferColorMRT(int num, int w, int h, TextureFormat format = TextureFormat.Normal)
        {
            IW = w;
            IH = h;
            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            for (int i = 0; i < num; i++)
            {
                Targets.Add(new Texture2D(w, h, false, format));
            }
            //BB = new Texture2D(w, h, false, format);
            DB = new TextureDepth(w, h);

            DRB = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, DRB);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, w, h);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, DRB);
            DrawBuffersEnum[] drawb = new DrawBuffersEnum[num];
            for (int i = 0; i < num; i++)
            {
                FramebufferAttachment fa = FramebufferAttachment.ColorAttachment0;
                switch (i)
                {
                    case 0:
                        fa = FramebufferAttachment.ColorAttachment0;
                        drawb[0] = DrawBuffersEnum.ColorAttachment0;
                        break;
                    case 1:
                        fa = FramebufferAttachment.ColorAttachment1;
                        drawb[1] = DrawBuffersEnum.ColorAttachment1;
                        break;
                    case 2:
                        fa = FramebufferAttachment.ColorAttachment2;
                        drawb[2] = DrawBuffersEnum.ColorAttachment2;
                        break;
                    case 3:
                        fa = FramebufferAttachment.ColorAttachment3;
                        drawb[3] = DrawBuffersEnum.ColorAttachment3;
                        break;
                    case 4:
                        fa = FramebufferAttachment.ColorAttachment4;
                        drawb[4] = DrawBuffersEnum.ColorAttachment4;
                        break;
                    case 5:
                        fa = FramebufferAttachment.ColorAttachment5;
                        drawb[5] = DrawBuffersEnum.ColorAttachment5;
                        break;
                    case 6:
                        fa = FramebufferAttachment.ColorAttachment6;
                        drawb[6] = DrawBuffersEnum.ColorAttachment6;
                        break;
                    case 7:
                        fa = FramebufferAttachment.ColorAttachment7;
                        drawb[7] = DrawBuffersEnum.ColorAttachment7;
                        break;
                }

                GL.FramebufferTexture(FramebufferTarget.Framebuffer, fa, Targets[i].ID, 0);
            }

            //  DrawBuffersEnum db = DrawBuffersEnum.ColorAttachment0;

            GL.DrawBuffers(drawb.Length, drawb);

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer failure.");
            }
            Console.WriteLine("Framebuffer success.");
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            SetVP.Set(0, 0, IW, IH);
            AppInfo.RW = IW;
            AppInfo.RH = IH;
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void BindFree()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void ReleaseFree()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Release()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            SetVP.Set(0, 0, AppInfo.W, AppInfo.H);
            AppInfo.RW = AppInfo.W;
            AppInfo.RH = AppInfo.H;
        }
    }
}