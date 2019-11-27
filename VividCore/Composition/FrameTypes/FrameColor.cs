namespace Vivid.Composition.FrameTypes
{
    public class FrameColor : FrameType
    {
        public override void Generate()
        {
            if (Regenerate == false)
            {
                return;
            }

            BindTarget();

            OpenTK.Graphics.OpenGL4.GL.Viewport(0, 0, FrameBuffer.IW, FrameBuffer.IH);

            App.AppInfo.RW = FrameBuffer.IW;
            App.AppInfo.RH = FrameBuffer.IH;

            //OpenTK.Graphics.OpenGL4.GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);

            OpenTK.Graphics.OpenGL4.GL.Clear(OpenTK.Graphics.OpenGL4.ClearBufferMask.ColorBufferBit | OpenTK.Graphics.OpenGL4.ClearBufferMask.DepthBufferBit);

            Graph.Render();

            ReleaseTarget();
        }
    }
}