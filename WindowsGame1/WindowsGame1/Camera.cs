using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class Camera
    {
        int[][] m_visualCoordinates;

        int convertToVisual(int a_logicX, int a_logicY, Piece a_piece)
        {
            int sizeOfTile = 64;
            int borderSize = 64;
            int visualX = borderSize + a_piece.getXposition() * sizeOfTile;
            int visualY = borderSize + a_piece.getYPosition() * sizeOfTile;

            return m_visualCoordinates[visualX][visualY];
        }
    }
}
