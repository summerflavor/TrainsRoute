using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainsRoute
{
    class Route : ICloneable
    {
        private List<Edge> edges = new List<Edge>();

        public Route() { }

        public Route(Edge statEdge)
        {
            AddEdge(statEdge);
        }

        public Route(List<Edge> edges)
        {
            this.edges.AddRange(edges);
        }

        public string FirstStop
        {
            get
            {
                if(edges.Count > 0)
                {
                    return edges[0].Start;
                }

                throw new Exception("The route is empty.");
            }
        }

        public string LastStop
        {
            get
            {
                if (edges.Count > 0)
                {
                    return edges[edges.Count - 1].End;
                }

                throw new Exception("The route is empty.");
            }
        }

        public void AddDistanceToEdges(RailwayNetwork railwayNetwork)
        {
            foreach (var edge in edges)
            {
                edge.Distance = railwayNetwork.GetDistance(edge.Start, edge.End);
            }
        }

        public List<Edge> Edges
        {
            get
            {
                return new List<Edge>(edges);
            }
        }

        public void AddEdge(Edge edge)
        {
            this.edges.Add(edge);
        }

        public object Clone()
        {
            Route newRoute = (Route)this.MemberwiseClone();
            newRoute.edges = new List<Edge>();

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
                return edges.Count;
            }
        }

        public int Distance
        {
            get
            {
                int distance = 0;
                foreach (var edge in edges)
                {
                    distance += edge.Distance;
                }
                return distance;
            }
        }

        public bool HasReached(string stop)
        {
            return edges[edges.Count - 1].End == stop;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Route:");

            if (this.edges.Count > 0)
            {
                sb.Append(this.edges[0].Start);

                foreach (var edge in this.edges)
                {
                    sb.Append("->");
                    sb.Append(edge.End);
                }

                sb.Append(", Distance: "); sb.Append(Distance);
                sb.Append(", Stops: "); sb.Append(NumberofStops);
            }
            else
            {
                sb.Append("Empty route");
            }

            return sb.ToString();
        }
    }
}
