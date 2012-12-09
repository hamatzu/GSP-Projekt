using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Model
{
    class Level
    {
        public const int LEVEL_WIDTH = 20;
        public const int LEVEL_HEIGHT = 20;

        public const int TILE_WIDTH = 1;
        public const int TILE_HEIGHT = 1;

        internal Tile[,] levelTiles = new Tile[LEVEL_WIDTH, LEVEL_HEIGHT];

        internal Level()
        {
            GenerateLevel();
        }

        //Check if player is colliding with a tile
        internal bool IsCollidingAt(FloatRectangle a_rect)
        {
            Vector2 tileSize = new Vector2(1f, 1f);
            for (int x = 0; x < LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < LEVEL_HEIGHT; y++)
                {
                    FloatRectangle rect = FloatRectangle.createFromTopLeft(new Vector2(x, y), tileSize);
                    if (a_rect.isIntersecting(rect))
                    {
                        if (levelTiles[x, y].isBlocked())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void GenerateLevel()
        {
            Random rand = new Random();
            for (int x = 0; x < LEVEL_WIDTH; x++)
            {
                for (int y = 0; y < LEVEL_HEIGHT; y++)
                {
                    setTileToRandom(rand, x, y);
                }
                setBottomAndTopToBlocked(x);
            }
        }

        private void setBottomAndTopToBlocked(int x)
        {
            levelTiles[x, 0] = Tile.createBlocked();
            levelTiles[x, LEVEL_HEIGHT - 1] = Tile.createBlocked();
        }

        private void setTileToRandom(Random rand, int x, int y)
        {
            levelTiles[x, y] = Tile.createEmpty();

            //Add some random blockes
            if (rand.Next(20) == 0)
            {
                levelTiles[x, y] = Tile.createBlocked();
                if (x > 0)
                {
                    levelTiles[x - 1, y] = Tile.createBlocked();
                }
            }

            setSidesToBlocked(x, y);
        }

        private void setSidesToBlocked(int x, int y)
        {
            if (x == 0 || x == LEVEL_WIDTH - 1)
            {
                levelTiles[x, y] = Tile.createBlocked();
            }
        }
    }
}
