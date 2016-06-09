using UnityEngine;
using System.Collections;
using System.Linq;

public enum TileType
{
    Empty = -1,
    Grass = 15
}

public class Map {

    public Tile[] tiles;
    public int columns;
    public int rows;

    public Tile[] coastTiles
    {
        get {
            return tiles.Where(t => t.autotileID < (int)TileType.Grass).ToArray();
                }
    }

    public void NewMap(int width, int height)
    {
        columns = width;
        rows = height;

        tiles = new Tile[columns * rows];

        CreateTiles();
    }

    public void CreateIsland(
        float erodePercent,
        int erodeIterations
        ){
        for (var i = 0; i < erodeIterations; i++)
        {
            DecorateTiles(coastTiles, erodePercent, TileType.Empty);
        }
    }

    private void CreateTiles()
    {
        var total = tiles.Length;

        for (int i = 0; i < total; i++)
        {
            var tile = new Tile();
            tile.id = i;

            tiles[i] = tile;
        }

        FindNeighbours();
    }


    private void FindNeighbours()
    {
        for (var r = 0; r < rows; r++)
        {
            for (var c = 0;c  < columns; c++)
            {
                var tile = tiles[columns * r + c];

                if (r < rows - 1)
                {
                    tile.AddNeighbour(Sides.Bottom, tiles[columns * (r + 1) + c]);
                }

                if (c < columns - 1)
                {
                    tile.AddNeighbour(Sides.Right, tiles[columns * r + c + 1]);
                }

                if (c>0)
                {
                    tile.AddNeighbour(Sides.Left, tiles[columns * r + c - 1]);
                }

                if (r > 0)
                {
                    tile.AddNeighbour(Sides.Top, tiles[columns * (r - 1) + c]);
                }

            }
        }


    }


    public void DecorateTiles(Tile[] tiles, float percent, TileType type)
    {
        // get whole number
        var total = Mathf.FloorToInt(tiles.Length * percent);

        RandomizeTileArray(tiles);

        for(var i = 0; i < total; i++)
        {
            var tile = tiles[i];

            if (type == TileType.Empty)
            {
                tile.ClearNeighbours();
                tile.autotileID = (int)type;
            }

        }

    }

    public void RandomizeTileArray(Tile[] tiles)
    {
        for(var i = 0; i < tiles.Length; i++)
        {
            var tmp = tiles[i];
            var r = Random.Range(i, tiles.Length);
            tiles[i] = tiles[r];
            tiles[r] = tmp;
        }
    }
}
