using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainsRoute
{
    class Edge
    {
        public Stop Start { get; set; }

        public Stop End { get; set; }

        public int Distance { get; set; }

    }
}
