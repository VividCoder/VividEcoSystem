using System.Collections.Generic;
using Vivid.Scene;

namespace Vivid.Gameplay
{
    public class Gameplay
    {
        public static Gameplay ActiveGame = null;
        public Scene.Cam3D GameCam = null;
        public List<SceneGraph3D> Graphs = new List<SceneGraph3D>();
        public InputAgent Input = null;
        public CameraAgent PlayerCam = null;
        public Scene.SceneGraph3D EditGraph = null;
        public Scene.Cam3D EditCam = null;

        public virtual void Init()
        {
        }

        public virtual void Update()
        {
            if (Input != null)
            {
                Input.Update();
            }

            if (PlayerCam != null)
            {
                PlayerCam.Update();
            }
        }
    }
}