using System;

namespace BackgammonLogic
{
    internal class Dice
    {
        private readonly Random _rnd;

        internal Dice()
        {
            _rnd = new Random();
        }

        internal int Roll()
        {
            return _rnd.Next(1, 7);
        }
    }
}
