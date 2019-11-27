namespace Vivid.Composition.Compositers
{
    public class BloomCompositer : Compositer
    {
        public float MinLevel { get; set; }
        public float BlurLevel { get; set; }

        public void Changed()
        {
            dynamic fe = Types[0];
            fe.FX.MinLevel = MinLevel;
            dynamic f2 = Types[1];

            f2.FX.Blur = BlurLevel;
        }

        public BloomCompositer() : base(3)
        {
            Name = "BloomPP";
            MinLevel = 0.8f;
            BlurLevel = 2.0f;

            InputFrame = new FrameTypes.FrameColor();

            Types[0] = new FrameTypes.FrameEffect();

            Types[1] = new FrameTypes.FrameEffect();

            Types[2] = new FrameTypes.FrameEffect();

            dynamic fe = Types[0];

            fe.FX = new PostProcess.Processes.VEExtract();

            fe.FX.MinLevel = 0.45f;

            Types[0].TexBind.Add(InputFrame);

            dynamic f2 = Types[1];

            f2.FX = new PostProcess.Processes.VEBlur();

            f2.FX.Blur = 2.0f;

            Types[1].TexBind.Add(Types[0]);

            dynamic f3 = Types[2];

            f3.FX = new PostProcess.Processes.VEBloom();

            //  Types [ 1 ].TexBind.Add ( Types [ 1 ] );

            Types[2].TexBind.Add(Types[1]);
            Types[2].TexBind.Add(InputFrame);
            //Types[3].TexBind.Add( )

            OutputFrame = Types[2]; // Types [ 2 ];
            Blend = FrameBlend.Solid;
        }

        public override void PreGen()
        {
            return;
            Types[0] = InputFrame;
            Types[1].TexBind.Clear();
            Types[1].TexBind.Add(InputFrame);
            Types[2].TexBind.Clear();
            Types[2].TexBind.Add(InputFrame);
        }
    }
}