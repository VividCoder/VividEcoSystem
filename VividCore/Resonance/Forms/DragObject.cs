using System;

namespace Vivid.Resonance.Forms
{
    public class DragObject : UIForm
    {
        public Texture.Texture2D DragImg = null;

        public Object DragObj;
        public Object DragSubObj;

        public DragObject()
        {
            W = 64;
            H = 64;

            void DrawFunc()
            {
                DrawForm(DragImg, new OpenTK.Vector4(0.9f, 0.9f, 0.9f, 0.65f));
            }

            Draw = DrawFunc;
        }
    }
}