using System;
using System.Linq;
using BackgammonLogic;

namespace ConsoleBackgammon
{
    class Program
    {
        private static readonly IBackgammonGame _game = new TwoPlayerGame();

        static void Main(string[] args)
        {
           
            PreGame();
            Game();
        }

        private static void Game()
        {
            while (_game.WinnerPlayer==PlayerColor.Null)
            {
                Console.Clear();
                DrawBoard();
                Console.WriteLine($"{_game.CurrentPlayer} Turn");
                Console.Write("press Enter to roll the dice");
                Console.ReadLine();
                _game.Roll();
                while (_game.CheckValidMoves())
                {
                    Console.Clear();
                    DrawBoard();
                    Console.WriteLine($"{_game.CurrentPlayer} Turn");
                    Console.WriteLine($"{_game.FirstDice} {_game.SecondDice}");
                    Console.Write("Select a location to move from ");
                    var location = Console.ReadLine();
                    var loc = 0;
                    if (int.TryParse(location, out loc))
                    {
                        Console.Write("Select a target move to");
                        var to = Console.ReadLine();
                        var toNum = 0;
                        if (int.TryParse(to, out toNum))
                        {
                            try
                            {
                                Console.WriteLine(_game.Move(loc, toNum) ? "Move occurred" : "Bad Move");
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e);
                            }
                            Console.Read();
                        }
                    }  
                }
            }

            Console.WriteLine($"{_game.WinnerPlayer} Win");
        }

        private static void PreGame()
        {
            var fl=false;
            do
            {
                fl = _game.PreGame();
                Console.WriteLine("press any key to see how start the game");
                Console.Read();
                Console.WriteLine($"Black result : {_game.FirstDice}");
                Console.WriteLine();
                Console.WriteLine($"White result : {_game.SecondDice}");
                Console.ReadLine();
            } while (!fl);
            Console.ReadLine();
        }

        private static void DrawBoard()
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Blue;
            var board = _game.GetBoard().ToArray();
            Console.WriteLine();
            Console.Write("|");
            for (int i = 1; i < 25; i++)
            {
                Console.Write($"{i}|");
            }
            Console.WriteLine();
            Console.Write("|");
            for (var i = 1; i < 25; i++)
            {
                if (i > 9)
                {
                    Console.Write(" ");
                }
                if (board[i].PlayerColor == PlayerColor.Black)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(board[i].NumOfStones);

                }
                else if (board[i].PlayerColor == PlayerColor.White)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(board[i].NumOfStones);
                }
                else
                {
                    Console.Write(" ");
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("|");

            }
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"27: White dead bar {board[27].NumOfStones} , 25: White home {board[25].NumOfStones}");
            Console.WriteLine($"26: Black dead bar {board[26].NumOfStones} , 0: Black home {board[0].NumOfStones}");
        }
    }
}
