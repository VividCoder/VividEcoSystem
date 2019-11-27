namespace Vivid.Gameplay
{
    public class CameraAgent : ClassAgent
    {
        public Scene.Cam3D Cam
        {
            get;
            set;
        }

        public Scene.Entity3D Target
        {
            get;
            set;
        }

        public OpenTK.Vector3 CamOffSet
        {
            get
            {
                return _CamOffset;
            }
            set
            {
                _CamOffset = value;
            }
        }

        private OpenTK.Vector3 _CamOffset = new OpenTK.Vector3(0, 10, 30);

        public OpenTK.Vector3 CamTarget
        {
            get
            {
                return _CamTarget;
            }
            set
            {
                _CamTarget = value;
            }
        }

        private OpenTK.Vector3 _CamTarget = new OpenTK.Vector3(0, 0, 0);

        public virtual void Init()
        {
        }

        public virtual void Update()
        {
        }
    }
}