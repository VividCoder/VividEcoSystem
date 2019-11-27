using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceEngine.Map.TileSet
{
    public class TileSet
    {

        public List<Tile.Tile> Tiles
        {
            get;
                  
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public TileSet(string name)
        {

            Tiles = new List<Tile.Tile>();
            Name = name;

        }

        void AddTile(Tile.Tile tile)
        {

            Tiles.Add(tile);

        }

        public void Load(string path)
        {

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            int c = br.ReadInt32();

            for (int i = 0; i < c; i++)
            {

                var tile = new Tile.Tile(br);
                Tiles.Add(tile);

            }
        }

   

        public void Save(string path)
        {

            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(Tiles.Count);
            
            

            for(int i = 0; i < Tiles.Count; i++)
            {

                Tiles[i].Write(bw);

            


            }

            bw.Flush();
            fs.Flush();
            bw.Close();
            fs.Close();
            fs = null;
            bw = null;

        }

    }
}
