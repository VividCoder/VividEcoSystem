﻿using OpenTK.Graphics.OpenGL4;
using System;
using Vivid.App;
using Vivid.Texture;

namespace Vivid.FrameBuffer
{
    public class FrameBufferCube
    {
        public int FBO, FBD;
        public TextureCube Cube;
        public int W, H;

        public FrameBufferCube(int w, int h)
        {
            W = w;
            H = h;
            Cube = new TextureCube(w, h);
            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            FBD = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, FBD);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent24, w, h);
            // GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            //G/             L.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, FBO);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.TextureCubeMapPositiveX, Cube.ID, 0);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, FBD);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.TextureCubeMapPositiveX, Cube.ID, 0);

            CheckFBO();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            Cube.Release(0);
        }

        private static void CheckFBO()
        {
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer failure.");
                while (true)
                {
                }
            }
        }

        public TextureTarget SetFace(int face)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            SetVP.Set(0, 0, W, H);
            App.AppInfo.RW = W;
            App.AppInfo.RH = H;
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.TextureCubeMapPositiveX + face, Cube.ID, 0);
            CheckFBO();
            return TextureTarget.TextureCubeMapPositiveX + face;
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