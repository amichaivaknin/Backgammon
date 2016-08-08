using System.Collections.Generic;

namespace BackgammonLogic
{
    public interface IBackgammonGame
    {
        int FirstDice { get; }
        int SecondDice { get; }
        PlayerColor CurrentPlayer { get;}
        PlayerColor WinnerPlayer { get;}

        bool PreGame();
        void Roll();
        bool Move(int from, int to);
        bool CheckValidMoves();
        IEnumerable<Triangle> GetBoard();  
    }

    public struct Triangle
    {
        public readonly PlayerColor PlayerColor;
        public readonly int NumOfStones;

        public Triangle(PlayerColor playerColor, int numOfStones)
        {
            PlayerColor = playerColor;
            NumOfStones = numOfStones;
        }
    }

    public enum PlayerColor { Black, White, Null };


}
