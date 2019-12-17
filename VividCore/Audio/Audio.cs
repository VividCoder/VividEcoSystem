using IrrKlang;



namespace Vivid.Audio
{

    public class VSound
    {
        public ISound Snd;
        public bool Playing
        {
            get
            {
                return Snd.Finished == false;

            }
        }
    }
    public class VSoundSource
    {
        public ISoundSource Src;
    }
    public static class Songs
    {
        public static ISoundEngine engine;
        public static ISound SongSound;
        public static VSoundSource LoadSound(string path)
        {

            if (engine == null)
            {
                engine = new ISoundEngine();

            }

            var src = engine.AddSoundSourceFromFile(path);
            var vs = new VSoundSource();
            vs.Src = src;
            return vs;
        }

        public static VSound PlaySource(VSoundSource src,bool loop =false)
        {

            var snd = engine.Play2D(src.Src, loop,false,false);
            var vs = new VSound();
            vs.Snd = snd;
            return vs;

        }


        public static void PlaySong(string song)
        {

            if (engine == null)
            {
                engine = new ISoundEngine();

            }

            // To play a sound, we only to call play2D(). The second parameter
            // tells the engine to play it looped.

            SongSound = engine.Play2D(song, true);

        }
        public static void StopSong()
        {
            if (SongSound != null)
            {
                if (!SongSound.Finished)
                {
                    SongSound.Stop();
                }
                SongSound = null;

            }
        }
    }

}