using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    public class Paging
    {
        public Paging(int start, int count)
        {
            Start = start;
            Count = count;
        }

        private int Start { get; }
        private int Count { get; }
    }
}
