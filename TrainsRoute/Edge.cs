using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainsRoute
{
    class Edge
    {
        public string Start { get; set; }

        public string End { get; set; }

        public int Distance { get; set; }

        public Edge() { }

        public Edge(string start, string end)
        {
            this.Start = start;
            this.End = end;
            this.Distance = 0;
        }

        public Edge(string start, string end, int distance)
        {
            this.Start = start;
            this.End = end;
            this.Distance = distance;
        }

    }
}
