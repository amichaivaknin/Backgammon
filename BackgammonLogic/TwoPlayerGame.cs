using System;
using System.Collections.Generic;
using System.Linq;

namespace BackgammonLogic
{
    public class TwoPlayerGame: IBackgammonGame
    {
        private int _numOfMove;
        private readonly Dice _dice;
        private readonly Board _board;

        public int FirstDice { get; private set; }
        public int SecondDice { get; private set; }
        public PlayerColor CurrentPlayer { get; private set; }
        public PlayerColor WinnerPlayer { get; private set; }

        public TwoPlayerGame()
        {
            _board=new Board();
            _dice = new Dice();
            _numOfMove = 0;
            SecondDice = 0;
            FirstDice = 0;
            CurrentPlayer=PlayerColor.Black;
            WinnerPlayer= PlayerColor.Null;
        }

        public bool PreGame()
        {
            FirstDice = _dice.Roll();
            SecondDice = _dice.Roll();

            if (FirstDice > SecondDice)
            {
                CurrentPlayer = PlayerColor.Black;
                return true;
            }

            if (SecondDice > FirstDice)
            {
                CurrentPlayer = PlayerColor.White;
                return true;
            }
            CurrentPlayer = PlayerColor.Null;
            return false;
        }  

        public void Roll()
        {
            SecondDice = _dice.Roll();
            FirstDice = _dice.Roll();
            _numOfMove = FirstDice==SecondDice ? 4 : 2;
        }

        public bool Move(int from, int to)
        {
            if (_numOfMove == 0)
            {
                ChangePlayer();
                return false;
            }

            if (_numOfMove<1||_numOfMove>4)
            {
                return false;
            }

            if (CurrentPlayer == PlayerColor.Null)
            {
                return false;
            }

            var fl = CurrentPlayer == PlayerColor.Black ? BlackMove(from, to) : WhiteMove(from, to);

            if (!fl) return false;

            if (_numOfMove<3)
            {
                if (Math.Abs(from - to)==FirstDice)
                {
                    FirstDice = 0;
                }
                else if(Math.Abs(from - to) == SecondDice)
                {
                    SecondDice = 0;
                }   
            }
            _numOfMove--;

            if (_board.GameBoard[0] == -15)
            {
                WinnerPlayer=PlayerColor.Black;
            }
            else if(_board.GameBoard[25] == 15)
            {
                WinnerPlayer = PlayerColor.White;
            }
            
            return true;
        }

        public bool CheckValidMoves()
        {
            if (_numOfMove==0)
            {
                ChangePlayer();
                return false;
            }

            if (CurrentPlayer == PlayerColor.White && CheckWhiteValidMoves())
            {
                return true;
            }
        
            if (CurrentPlayer == PlayerColor.Black && CheckBlackValidMoves())
            {
                return true;
            }

            ChangePlayer();
            return false;
        }

        private bool WhiteMove(int from, int to)
        {
            if ((from < 1 || from > 24 || (to - from < 1) || to < 1 || to > 25) && from != 27)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (_board.GameBoard[27] > 0)
            {
                if (from == 27 && (to == FirstDice || to == SecondDice))
                {
                    return _board.WhiteMove(from, to);
                }
                return false;
            }

            if (to == 25 && _board.WhiteOut(from, to, FirstDice, SecondDice)) return true;

            if (to - from == FirstDice)
            {
                return _board.WhiteMove(from, to);
            }

            return to - from == SecondDice && _board.WhiteMove(from, to);
        }

        private bool BlackMove(int from, int to)
        {

            if ((from < 1 || from > 24 || to < 0 || to > 24 || to - from > -1) && from != 26)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (_board.GameBoard[26] < 0)
            {
                if (from == 26 && (to == 25 - FirstDice || to == 25 - SecondDice))
                {
                    return _board.BlackMove(from, to);
                }
                return false;
            }

            if (to == 0 && _board.BlackOut(from, to, FirstDice, SecondDice)) return true;

            if (from - to == FirstDice)
            {
                return _board.BlackMove(from, to);
            }
            return from - to == SecondDice && _board.BlackMove(from, to);
        }

        private bool CheckBlackValidMoves()
        {
            if (_board.GameBoard[26] < 0)
            {
                return _board.GameBoard[25 - FirstDice] < 2 || _board.GameBoard[25 - SecondDice] < 2;
            }
            
                for (var i = 24; i > 1; i--)
                {
                    if (_board.GameBoard[i] < 0 &&
                        (((i - FirstDice > 0) && (_board.GameBoard[i - FirstDice] < 2))
                        || ((i - SecondDice > 0) && (_board.GameBoard[i - SecondDice] < 2))))
                    {
                        return true;
                    }
                }

            for (var i = 6; i > 0; i--)
            {
                if (_board.CheckBlackOut(i, FirstDice, SecondDice))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckWhiteValidMoves()
        {
            if (_board.GameBoard[27] > 0)
            {
                if (_board.GameBoard[FirstDice] > -2 || _board.GameBoard[SecondDice] > -2) return true;
            }

                for (var i = 1; i < 24; i++)
                {
                    if (_board.GameBoard[i] > 0 && 
                        (((i + FirstDice < 24) && (_board.GameBoard[i + FirstDice] > -2))
                        || (i + SecondDice < 24) && _board.GameBoard[i + SecondDice] > -2))
                    {
                        return true;
                    }
                }

            for (var i = 19; i < 25; i++)
            {
                if (_board.CheckWhiteOut(i,FirstDice,SecondDice))
                {
                    return true;
                }
            }

            return false;
        }

        private void ChangePlayer()
        {
            switch (CurrentPlayer)
            {
                case PlayerColor.White:
                    CurrentPlayer = PlayerColor.Black;
                    break;
                case PlayerColor.Black:
                    CurrentPlayer=PlayerColor.White;
                    break;
            }
        }

        public IEnumerable<Triangle> GetBoard()
        {
            return _board.GameBoard.Select(i =>
            {
                if (i > 0)
                {
                  return new Triangle(PlayerColor.White, i);
                }
                return i < 0 ? new Triangle(PlayerColor.Black, -i) :
                new Triangle(PlayerColor.Null, 0);
            });
        }
    }
}
