using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid.Audio;
namespace Vivid.Game
{
    public class GameHost
    {

        public void SetMusic(string path)
        {

            Vivid.Audio.Songs.PlaySong(path);

        }

        public virtual void Update()
        {

        }

    }
}
