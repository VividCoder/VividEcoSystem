using System.Collections.Generic;

namespace Vivid.Gameplay
{
    public class GameplayTemplate
    {
        public List<Scene.SceneGraph3D> Graphs = new List<Scene.SceneGraph3D>();
        public InputAgent Input = null;
        public Scene.Cam3D PlayerCam = null;
    }
}