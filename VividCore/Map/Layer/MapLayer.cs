using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceEngine.Map.Layer
{
    public class MapLayer
    {

        public Map Owner = null;
        public Tile.Tile[,] Tiles
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public void Fill(Tile.Tile tile)
        {

            for(int y = 0; y < Height; y++)
            {
                for(int x = 0; x < Width; x++)
                {

                    Tiles[x, y] = tile;

                }
            }
            Owner.sceneChanged = true;
        }

        public MapLayer(int width,int height,Map owner)
        {

            Tiles = new Tile.Tile[width, height];
            Width = width;
            Height = height;
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    Tiles[x, y] = null;
                }
            }
            this.Owner = owner;
        }

        public void SetTile(int x,int y,Tile.Tile tile)
        {

            Tiles[x, y] = tile;
            Owner.sceneChanged = true;

        }

        public Tile.Tile GetTile(int x,int y)
        {

            return Tiles[x, y];

        }

    }
}
