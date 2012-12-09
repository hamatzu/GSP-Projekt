using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Model
{
    class ChessGame
    {
        PlayerTurn m_playerTurn = PlayerTurn.White;

        public enum PlayerTurn
        {
            White,
            Black
        }

        internal void setPlayerTurn(PlayerTurn playerTurn)
        {
            m_playerTurn = playerTurn;
        }

        internal string getPlayer()
        {
            return m_playerTurn.ToString();
        }

        internal PlayerTurn getPlayerTurn()
        {
            return m_playerTurn;
        }
    }
}
