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

        public Edge() { }

        public Edge(Stop start, Stop end)
        {
            this.Start = start;
            this.End = end;
            this.Distance = 0;
        }

        public Edge(Stop start, Stop end, int distance)
        {
            this.Start = start;
            this.End = end;
            this.Distance = distance;
        }

    }
}
