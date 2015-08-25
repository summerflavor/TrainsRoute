using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainsRoute
{
    class RailwayNetwork
    {
        private List<Edge> edges = new List<Edge>();

        public int TotalEges
        {
            get
            {
                return edges.Count;
            }
        }


        public RailwayNetwork()
        {

        }

        public void Initialize(List<Edge> edges)
        {
            this.edges.AddRange(edges);
        }

        public List<Edge> GetEdgesStartFrom(string stop)
        {
            List<Edge> matchedEdges = new List<Edge>();

            foreach (var edge in edges)
            {
                if (edge.IsStartedFrom(stop))
                {
                    matchedEdges.Add(edge);
                }
            }

            return matchedEdges;
        }

        public int GetDistance(string start, string end)
        {
            foreach (var edge in edges)
            {
                if (edge.IsStartedFrom(start) && edge.IsStoppedAt(end))
                {
                    return edge.Distance;
                }
            }

            return -1;
        }

        /// <summary>
        /// Determine whether the route is valid or not
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public bool IsRouteUnimpeded(Route route)
        {
            bool notFound;

            foreach (var edge in route.Edges)
            {
                notFound = true;

                foreach (var railEdge in this.edges)
                {
                    if (edge.Start == railEdge.Start && edge.End == railEdge.End)
                    {
                        notFound = false;
                        break;
                    }
                }

                if (notFound)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Get the shortest route from start to end
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Route ShortestRoute(string start, string end)
        {
            List<Route> routes = new List<Route>();
            List<Edge> edges = GetEdgesStartFrom(start);

            foreach (var edge in edges)
            {
                routes.Add(new Route(edge));
            }

            for (int i = 0; i < TotalEges; i++)
            {
                Route tmpRoute = null;
                int shortestDistance = int.MaxValue;

                foreach (var route in routes)
                {
                    if (route.Distance <= shortestDistance)
                    {
                        shortestDistance = route.Distance;
                        tmpRoute = route;
                    }
                }

                if (tmpRoute.HasReached(end))
                {
                    return tmpRoute;
                }

                ExtendRoutes(ref routes, end);
            }

            return null;
        }

        /// <summary>
        /// Get the routes with exact number of stops between start and end
        /// </summary>
        /// <param name="start">The start stop</param>
        /// <param name="end">The edn stop</param>
        /// <param name="stopsNumber">The number of stops</param>
        /// <returns></returns>
        public List<Route> RoutesWithExactStops(string start, string end, int stopsNumber)
        {
            List<Route> routes = new List<Route>();
            List<Edge> edges = GetEdgesStartFrom(start);

            foreach (var edge in edges)
            {
                routes.Add(new Route(edge));
            }

            for (int i = 0; i < stopsNumber - 1; i++)
            {
                ExtendRoutes(ref routes);
            }

            List<Route> matchedRoutes = new List<Route>();

            foreach (var route in routes)
            {
                if (route.HasReached(end))
                {
                    matchedRoutes.Add(route);
                }
            }

            return matchedRoutes;
        }

        /// <summary>
        /// Get the routes within certain number of stops between start and end
        /// </summary>
        /// <param name="start">The start stop</param>
        /// <param name="end">The edn stop</param>
        /// <param name="stopsNumber">The number of stops</param>
        /// <returns></returns>
        public List<Route> RoutesWithinCertainStops(string start, string end, int stopsNumber)
        {
            List<Route> routes = new List<Route>();

            for (int i = 1; i <= stopsNumber; i++)
            {
                routes.AddRange(RoutesWithExactStops(start, end, i));
            }

            return routes;
        }


        /// <summary>
        /// Get the routes within certain distance between start and end
        /// </summary>
        /// <param name="start">The start stop</param>
        /// <param name="end">The edn stop</param>
        /// <param name="stopsNumber">The distance required</param>
        /// <returns></returns>
        public List<Route> RoutesWithinCertainDistance(string start, string end, int distance)
        {
            List<Route> routes = new List<Route>();
            List<Route> matchedRoutes = new List<Route>();
            List<Edge> edges = GetEdgesStartFrom(start);

            foreach (var edge in edges)
            {
                routes.Add(new Route(edge));
            }

            bool continueToExtend = false;

            do
            {
                foreach (var route in routes)
                {
                    if (route.HasReached(end))
                    {
                        matchedRoutes.Add((Route)route.Clone());
                    }
                }

                ExtendRoutes(ref routes, distance, out continueToExtend);

            } while (continueToExtend);

            return matchedRoutes;
        }

        /// <summary>
        /// Extend the routes to the specified stop
        /// </summary>
        /// <param name="routes">Routes to extend </param>
        /// <param name="stop">The stop to extend to</param>
        private void ExtendRoutes(ref List<Route> routes, string stop)
        {
            List<Route> tmpRoutes = new List<Route>();

            foreach (var route in routes)
            {
                if (route.HasReached(stop)) // The route has reached the end just copy
                {
                    tmpRoutes.Add(route);
                }
                else // the route hasn't reached the end just proceed
                {
                    List<Edge> edges = GetEdgesStartFrom(route.LastStop);
                    foreach (var edge in edges)
                    {
                        Route tmpRoute = (Route)route.Clone();
                        tmpRoute.AddEdge(edge);
                        tmpRoutes.Add(tmpRoute);
                    }
                }
            }

            routes = tmpRoutes;
        }

        /// <summary>
        /// Exntend all the routes with one more stop
        /// </summary>
        /// <param name="routes"></param>
        private void ExtendRoutes(ref List<Route> routes)
        {
            List<Route> tmpRoutes = new List<Route>();

            foreach (var route in routes)
            {
                var edges = GetEdgesStartFrom(route.LastStop);

                foreach (var edge in edges)
                {
                    Route tmpRoute = (Route)route.Clone();
                    tmpRoute.AddEdge(edge);
                    tmpRoutes.Add(tmpRoute);
                }
            }

            routes = tmpRoutes;
        }

        /// <summary>
        /// Extend the route within the distance
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="distance"></param>
        /// <returns>Indicate if any of the route can proceed</returns>
        private void ExtendRoutes(ref List<Route> routes, int distance, out bool canExtend)
        {
            canExtend = false;

            List<Route> tmpRoutes = new List<Route>();

            foreach (var route in routes)
            {
                var edges = GetEdgesStartFrom(route.LastStop);

                foreach (var edge in edges)
                {
                    Route tmpRoute = (Route)route.Clone();
                    tmpRoute.AddEdge(edge);

                    if (tmpRoute.Distance < distance)
                    {
                        tmpRoutes.Add(tmpRoute);
                        canExtend = true;
                    }
                }
            }

            routes = tmpRoutes;
        }
    }
}
