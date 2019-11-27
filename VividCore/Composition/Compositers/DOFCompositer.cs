namespace Vivid.Composition.Compositers
{
    public class DOFCompositer : Compositer
    {

        public DOFCompositer() : base(2)
        {

            InputFrame = new FrameTypes.FrameColor();
            Types[0] = new FrameTypes.FrameDepth();
            Types[1] = new FrameTypes.FrameEffect();

            dynamic t1 = Types[1];

            t1.FX = new VEDof();

            Types[1].TexBind.Add(InputFrame);
            Types[1].TexBind.Add(Types[0]);


            OutputFrame = Types[1];
        }

    }

    public class VEDof : Vivid.Effect.Effect3D
    {
        public float Blur = 0.5f;
        public float FocalZ = 0.05f;
        public float FocalRange = 0.05f;

        public VEDof() : base("", "data/Shader/vsDof.glsl", "data/Shader/fsDof.glsl")
        {
        }

        public override void SetPars()
        {
            SetTex("colorTex", 0);
            SetTex("depthTex", 1);
            SetFloat("blur", Blur);
            SetFloat("focalZ", FocalZ);
            SetFloat("focalRange", FocalRange);
        }
    }
}
