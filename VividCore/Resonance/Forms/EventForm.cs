namespace Vivid.Resonance.Forms
{
    public class EventForm : UIForm
    {
        public EventOp Op = null;
        public EventForm(EventOp op = null)
        {

            Op = op;

            Update = () =>
            {

                Op?.Invoke();

            };

        }



    }

    public delegate void EventOp();


}
