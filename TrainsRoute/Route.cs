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

        public bool IsValidOn(List<Edge> railwayNetwork)
        {
            bool notFound = true;

            foreach (var edge in edges)
            {
                notFound = true;

                foreach (var railEdge in railwayNetwork)
                {
                    if (edge.Start.Name == railEdge.Start.Name && edge.End.Name == railEdge.End.Name)
                    {
                        notFound = false;
                    }
                }

                if (notFound)
                {
                    return false;
                }
            }

            return true;
        }

        public void AddDistanceForm(List<Edge> railwayNetwork)
        {
            foreach (var edge in edges)
            {
                foreach (var railEdge in railwayNetwork)
                {
                    if (edge.Start.Name == railEdge.Start.Name && edge.End.Name == edge.End.Name)
                    {
                        edge.Distance = railEdge.Distance;
                    }
                }
            }
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

        public bool HasReached(Stop stop)
        {
            return edges[edges.Count - 1].End.Name == stop.Name;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Route:");

            if (this.edges.Count > 0)
            {
                sb.Append(this.edges[0].Start.Name);

                foreach (var edge in this.edges)
                {
                    sb.Append("->");
                    sb.Append(edge.End.Name);
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
