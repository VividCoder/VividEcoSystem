namespace Vivid.Composition.Compositers
{
    public class SSRCompositer : Compositer
    {
        public SSRCompositer() : base(4)
        {
            Name = "SSRPP";

            InputFrame = new FrameTypes.FrameColor();

            Types[1] = new FrameTypes.FramePositionMap();

            Types[0] = new FrameTypes.FrameNormalMap();

            Types[3] = new FrameTypes.FrameEffect();

            Types[2] = new FrameTypes.FrameSSRExtras();

            dynamic f2 = Types[3];

            f2.FX = new VESSR();

            Types[3].TexBind.Add(InputFrame);
            Types[3].TexBind.Add(Types[0]);
            Types[3].TexBind.Add(Types[1]);
            Types[3].TexBind.Add(Types[2]);

            OutputFrame = Types[3];
            Blend = FrameBlend.Solid;
        }
    }

    public class VESSR : Effect.Effect3D
    {
        public float Blur = 0.5f;

        public VESSR() : base("", "data/Shader/vsSSR.glsl", "data/Shader/fsSSR2.glsl")
        {
        }

        public override void SetPars()
        {
            //          SetMat("pMatrix", Scene.SceneGraph3D.LastCam.ProjMat);
            //            SetMat("InvPMatrix", Scene.SceneGraph3D.LastCam.ProjMat.Inverted());

            SetMat("proj", Scene.SceneGraph3D.LastCam.ProjMat);

            SetMat("view", Scene.SceneGraph3D.LastCam.CamWorld);

            SetTex("tFrame", 0);
            SetTex("tNorm", 1);
            SetTex("tPos", 2);
            SetTex("tExtra", 3);
        }
    }
}