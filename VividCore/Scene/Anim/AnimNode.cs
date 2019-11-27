namespace Vivid.Scene.Anim
{
    public class AnimNode
    {

        public string Name
        {
            get;
            set;
        }

        public AnimNode Top
        {
            get;

            set;
        }


        public double BeginTime
        {
            get;
            set;
        }

        public double Endtime
        {
            get;
            set;
        }

        public double TimeMod
        {
            get;
            set;
        }

        public virtual string GetName()
        {
            return "Null";
        }

        public AnimNode()
        {

            BeginTime = Endtime = 0.0;
            TimeMod = 1.0;

        }


    }
}
