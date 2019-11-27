using Vivid.Texture;
using Vivid.Video;

namespace Vivid.Resonance.Forms
{
    public class VideoForm : UIForm
    {
        private VideoPlayer CurVid = null;
        private Texture2D VidTex = null;
        private VideoFrame Frm = null;
        public bool Paused = false;

        public VideoForm()
        {
        }

        public void SetVideo(string path)
        {
            CurVid = new VideoPlayer(path);

            Draw = () =>
            {
                if (Frm == null) return;
                DrawForm(CurVid.GetCurrentImage(), new OpenTK.Vector4(1, 1, 1, 1));
            };

            Update = () =>
            {
                // CurVid.DecodeNextFrame();

                var frame = CurVid.GetCurrentFrame();

                Frm = frame;
            };
        }

        public void Pause()
        {
            if (Paused) return;
            Paused = true;
            CurVid.Pause();
        }

        public void Play()
        {
            if (Paused == false) return;
            Paused = false;
            CurVid.Play();
        }
    }
}