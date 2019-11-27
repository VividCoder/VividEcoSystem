namespace Vivid.State
{
    public class VividState
    {

        public Resonance.UI SUI = null;

        public string Name
        {
            get;
            set;
        }

        public bool Running
        {
            get;
            set;
        }

        public void InitUI()
        {
            SUI = new Resonance.UI();
        }

        public VividState(string name = "")
        {
            Name = name;
            Running = false;
            // SUI = new Resonance.UI();
        }

        public virtual void ResizeState(int w, int h)
        {
        }

        public virtual void InitState()
        {
        }

        public virtual void StartState()
        {
        }

        public virtual void UpdateState()
        {
        }

        public virtual void DrawState()
        {
        }

        public virtual void StopState()
        {
        }

        public virtual void ResumeState()
        {
        }

        public void InternalUpdate()
        {

        }
    }
}