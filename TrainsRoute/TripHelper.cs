using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainsRoute
{
    class TripHelper
    {
        /// <summary>
        /// The network of the railway
        /// </summary>
        public static List<Edge> RailsNetWork = new List<Edge>();

        /// <summary>
        /// Calculate the Distance of the given route
        /// </summary>
        /// <param name="route">Route to get distance of</param>
        /// <returns></returns>
        public static int RouteDistance(Route route)
        {
            return route.Distance;
        }

        /// <summary>
        /// Calculate the shortest routes between two stops
        /// </summary>
        /// <param name="start">The start stop</param>
        /// <param name="end">The end stop</param>
        /// <returns></returns>
        public static Route ShortestRoute(Stop start, Stop end)
        {
            List<Route> routes = new List<Route>();

            foreach (var edge in RailsNetWork)
            {
                if (edge.Start.Name == start.Name)
                {
                    Route r = new Route();
                    r.AddEdge(edge);
                    routes.Add(r);
                }
            }

            for (int i = 0; i < RailsNetWork.Count - 1; i++)
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

                ProceedRoutes(ref routes, end);
            }

            return null;
        }

        /// <summary>
        /// Just proceed the route one more stop
        /// </summary>
        /// <param name="routes"></param>
        private static void ProceedRoutes(ref List<Route> routes)
        {
            List<Route> tmpRoutes = new List<Route>();

            foreach (var route in routes)
            {
                foreach (var edge in RailsNetWork)
                {
                    if (route.HasReached(edge.Start))
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
        /// Proceed the routes to the end stop
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="end"></param>
        private static void ProceedRoutes(ref List<Route> routes, Stop end)
        {
            List<Route> tmpRoutes = new List<Route>();

            foreach (var route in routes)
            {
                if (route.HasReached(end)) // The route has reached the end just copy
                {
                    tmpRoutes.Add(route);
                }
                else // the route hasn't reached the end just proceed
                {
                    foreach (var edge in RailsNetWork)
                    {
                        if (route.HasReached(edge.Start))
                        {
                            Route tmpRoute = (Route)route.Clone();
                            tmpRoute.AddEdge(edge);
                            tmpRoutes.Add(tmpRoute);
                        }
                    }
                }

            }

            routes = tmpRoutes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="distance"></param>
        /// <returns>Indicate if any of the route can proceed</returns>
        private static bool ProceedRoutesWihtinDistance(ref List<Route> routes, int distance)
        {
            bool canStillProceed = false;

            List<Route> tmpRoutes = new List<Route>();

            foreach (var route in routes)
            {
                foreach (var edge in RailsNetWork)
                {
                    if (route.HasReached(edge.Start))
                    {
                        Route tmpRoute = (Route)route.Clone();
                        tmpRoute.AddEdge(edge);

                        if (tmpRoute.Distance < distance)
                        {
                            tmpRoutes.Add(tmpRoute);
                            canStillProceed = true;
                        }
                    }
                }
            }

            routes = tmpRoutes;

            return canStillProceed;
        }

        /// <summary>
        /// Get the routes within certain number of stops between start and end
        /// </summary>
        /// <param name="start">The start stop</param>
        /// <param name="end">The edn stop</param>
        /// <param name="stopsNumber">The number of stops</param>
        /// <returns></returns>
        public static List<Route> RoutesWithinCertainStops(Stop start, Stop end, int stopsNumber)
        {
            List<Route> routes = new List<Route>();

            for (int i = 1; i <= stopsNumber; i++)
            {
                routes.AddRange(RoutesWithExactStops(start, end, i));
            }

            return routes;
        }

        /// <summary>
        /// Get the routes with exact number of stops between start and end
        /// </summary>
        /// <param name="start">The start stop</param>
        /// <param name="end">The edn stop</param>
        /// <param name="stopsNumber">The number of stops</param>
        /// <returns></returns>
        public static List<Route> RoutesWithExactStops(Stop start, Stop end, int stopsNumber)
        {
            List<Route> routes = new List<Route>();

            foreach (var edge in RailsNetWork)
            {
                if (edge.Start.Name == start.Name)
                {
                    routes.Add(new Route(edge));
                }
            }

            for (int i = 0; i < stopsNumber - 1; i++)
            {
                ProceedRoutes(ref routes);
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
        /// Get the routes within certain distance between start and end
        /// </summary>
        /// <param name="start">The start stop</param>
        /// <param name="end">The edn stop</param>
        /// <param name="stopsNumber">The distance required</param>
        /// <returns></returns>
        public static List<Route> RoutesWithinCertainDistance(Stop start, Stop end, int distance)
        {
            List<Route> routes = new List<Route>();
            List<Route> tmpRoutes = new List<Route>();

            foreach (var edge in RailsNetWork)
            {
                if (edge.Start.Name == start.Name)
                {
                    routes.Add(new Route(edge));
                }
            }

            do
            {
                foreach (var route in routes)
                {
                    if (route.HasReached(end))
                    {
                        tmpRoutes.Add((Route)route.Clone());
                    }
                }

            } while (ProceedRoutesWihtinDistance(ref routes, distance));

            return tmpRoutes;
        }
    }
}
