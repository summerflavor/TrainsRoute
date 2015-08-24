using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainsRoute
{
    class Route : ICloneable
    {
        private int numberofStops;
        private int distance;
        private List<Edge> edges = new List<Edge>();

        public Route()
        {

        }

        public Route(Edge statEdge)
        {
            AddEdge(statEdge);
        }

        public List<Edge> Edges
        {
            get
            {
                return edges;
            }
        }

        public void AddEdge(Edge edge)
        {
            this.edges.Add(edge);
            this.NumberofStops++;
            this.Distance += edge.Distance;
        }

        public object Clone()
        {
            Route newRoute = new Route();

            foreach (var item in edges)
            {
                newRoute.edges.Add(item);
            }

            return newRoute;
        }

        public int NumberofStops
        {
            get
            {
                return numberofStops;
            }

            private set
            {
                numberofStops = value;
            }
        }

        public int Distance
        {
            get
            {
                return distance;
            }

            private set
            {
                distance = value;
            }
        }

        public bool HasReached(Stop stop)
        {
            return edges[edges.Count - 1].End.Name == stop.Name;
        }
    }
}
