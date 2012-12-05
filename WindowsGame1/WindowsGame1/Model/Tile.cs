using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Model
{
    class Tile
    {
        TileType tileType;

        private enum TileType
        {
            EMPTY = 0,
            BLOCKED,
            TRAP
        };

        internal static Tile createBlocked()
        {
            return new Tile(TileType.BLOCKED);

        }

        internal static Tile createEmpty()
        {
            return new Tile(TileType.EMPTY);
        }

        private Tile(TileType a_type)
        {
            tileType = a_type;
        }
        

        internal bool isBlocked()
        {
            return tileType == TileType.BLOCKED;
        }
    }
}
