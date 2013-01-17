using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1.Model
{
    class Tile
    {
        TileType tileType;
        Vector2 tilePosition;
        private bool walkedOn = false;

        private enum TileType
        {
            EMPTY = 0,
            BLOCKED,
            TRAP,
            EXIT
        };

        internal static Tile createBlocked()
        {
            return new Tile(TileType.BLOCKED);
        }

        internal static Tile createEmpty()
        {
            return new Tile(TileType.EMPTY);
        }

        internal static Tile createTrap()
        {
            return new Tile(TileType.TRAP);
        }

        private Tile(TileType a_type)
        {
            tileType = a_type;
        }

        internal bool isTrap()
        {
            return tileType == TileType.TRAP;
        }

        internal bool isBlocked()
        {
            return tileType == TileType.BLOCKED;
        }

        internal bool isExit()
        {
            return tileType == TileType.EXIT;
        }

        internal static Tile createExit()
        {
            return new Tile(TileType.EXIT);
        }

        internal void setTilePosition(Vector2 tilePos)
        {
            tilePosition = tilePos;
        }

        internal Vector2 getTilePosition()
        {
            return tilePosition;
        }

        internal bool isWalkedOn()
        {
            return walkedOn;
        }

        internal void setWalkedOn(bool walked)
        {
            walkedOn = walked;
        }

        internal bool isEmpty()
        {
            return tileType == TileType.EMPTY;
        }
    }
}
