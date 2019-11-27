using IrrKlang;



namespace Vivid.Audio
{

    public static class Songs
    {
        public static ISoundEngine engine;
        public static ISound SongSound;
        public static void PlaySong(string song)
        {

            engine = new ISoundEngine();


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