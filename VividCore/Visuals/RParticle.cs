namespace Vivid.Visuals
{
    public class RParticle : Renderer
    {
        public override void Init()
        {
            Layers.Add(new RLParticle());
        }
    }
}