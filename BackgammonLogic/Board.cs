using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BackgammonLogic
{
    internal class Board
    {
        internal int[] GameBoard { get; }

        internal Board()
        {
            GameBoard = new [] {0,
                             2, 0, 0, 0, 0, -5,
                             0, -3, 0, 0, 0, 5,
                             -5, 0, 0, 0, 3, 0,
                             5, 0, 0, 0, 0, -2,
                             0,0,0};
        }

        internal bool WhiteMove(int from, int to)
        {
            if (GameBoard[from] >= 1)
            {
                if (GameBoard[to] > -1)
                {
                    WhiteSimpleStep(from, to);
                    return true;
                }

                if (GameBoard[to] < -1) return false;

                WhiteHitStep(from, to);
                return true;
            }

            return false;
        }

        internal bool WhiteOut(int from, int to, int first, int second)
        {
            if (!CheckWhiteOut(from, first, second)) return false;
            WhiteSimpleStep(from, to);
            return true;
        }

        internal bool CheckWhiteOut(int from, int first, int second)
        {
            if (GameBoard[27] > 0)
            {
                return false;
            }

            var sum=0;
            for (var i = 19; i < 26; i++)
            {
                sum += GameBoard[i];
            }

            return sum==15 &&GameBoard[from] > 0 && ((from + first) >= 24 || (from + second >= 24));
        }

        private void WhiteHitStep(int from, int to)
        {
            GameBoard[26]--;
            GameBoard[to] = 1;
            GameBoard[from]--;
        }

        private void WhiteSimpleStep(int from, int to)
        {
            GameBoard[to]++;
            GameBoard[from]--;
        }

        internal bool BlackMove(int from, int to)
        {
            if (GameBoard[from] <= -1)
            {
                if (GameBoard[to] < 1)
                {
                    BlackSimpleStep(from, to);
                    return true;
                }

                if (GameBoard[to] > 1) return false;

                BlackHitStep(from, to);
                return true;
            }
            return false;
        }

        internal bool BlackOut(int from, int to, int first, int second)
        {
            if (!CheckBlackOut(from, first, second)) return false;
            BlackSimpleStep(from, to);
            return true;
        }

        internal bool CheckBlackOut(int from, int first, int second)
        {
            if (GameBoard[26] < 0)
            {
                return false;
            }

            var sum = 0;

            for (var i = 6; i >-1 ; i--)
            {
                sum += GameBoard[i];
            }
            return sum==-15 &&GameBoard[from] < 0 &&
                ((from - first) <= 0 || (from - second <= 0));
        }

        private void BlackHitStep(int from, int to)
        {
            GameBoard[27]++;
            GameBoard[to] = -1;
            GameBoard[from]++;
        }

        private void BlackSimpleStep(int from, int to)
        {
            GameBoard[to]--;
            GameBoard[from]++;
        }
    }
}
