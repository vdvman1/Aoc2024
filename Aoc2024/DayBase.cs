using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2024
{
    public abstract class DayBase
    {
        public DayBase() => ParseData();

        public abstract void ParseData();

        public abstract string Solve1();

        public abstract string Solve2();
    }
}
