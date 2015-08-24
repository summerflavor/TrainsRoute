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
        static List<Edge> RailsNetWork = null;

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

                ProceedRoutes(routes, end);
            }

            return null;
        }

        private static void ProceedRoutes(List<Route> routes)
        {
            foreach (var route in routes)
            {
                Route tmp = (Route)route.Clone();
                bool hasFoundOne = false;

                foreach (var edge in RailsNetWork)
                {
                    if (tmp.HasReached(edge.Start))
                    {
                        if (!hasFoundOne)
                        {
                            route.AddEdge(edge);
                            hasFoundOne = true;
                        }
                        else
                        {
                            Route newRoute = (Route)tmp.Clone();
                            newRoute.AddEdge(edge);
                            routes.Add(newRoute);
                        }
                    }
                }
            }
        }

        private static void ProceedRoutes(List<Route> routes, Stop end)
        {
            foreach (var route in routes)
            {
                Route tmp = (Route)route.Clone();
                bool hasFoundOne = false;

                foreach (var edge in RailsNetWork)
                {
                    if (!tmp.HasReached(end) && tmp.HasReached(edge.Start))
                    {
                        if (!hasFoundOne)
                        {
                            route.AddEdge(edge);
                            hasFoundOne = true;
                        }
                        else
                        {
                            Route newRoute = (Route)tmp.Clone();
                            newRoute.AddEdge(edge);
                            routes.Add(newRoute);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="distance"></param>
        /// <returns>Indicate if any of the route can proceed</returns>
        private static bool ProceedRoutesWihtinDistance(List<Route> routes, int distance)
        {
            bool canStillProceed = false; 

            foreach (var route in routes)
            {
                if (route.Distance >= distance)
                {
                    continue;
                }

                Route tmpRoute = (Route)route.Clone();
                bool hasFoundOne = false;

                foreach (var edge in RailsNetWork)
                {
                    if (tmpRoute.HasReached(edge.Start))
                    {
                        if (!hasFoundOne)
                        {
                            route.AddEdge(edge);
                            hasFoundOne = true;
                        }
                        else
                        {
                            routes.Add(new Route(edge));
                        }

                        if((canStillProceed == false) && (tmpRoute.Distance + edge.Distance) < distance)
                        {
                            canStillProceed = true;
                        }
                    }
                }
            }

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
                routes.AddRange(RoutesWithCertainStops(start, end, i));
            }

            return routes;
        }

        /// <summary>
        /// Get the routes with certain number of stops between start and end
        /// </summary>
        /// <param name="start">The start stop</param>
        /// <param name="end">The edn stop</param>
        /// <param name="stopsNumber">The number of stops</param>
        /// <returns></returns>
        public static List<Route> RoutesWithCertainStops(Stop start, Stop end, int stopsNumber)
        {
            List<Route> routes = new List<Route>();

            foreach (var edge in RailsNetWork)
            {
                if(edge.Start.Name == start.Name)
                {
                    routes.Add(new Route(edge));
                }
            }

            for (int i = 0; i < stopsNumber - 1; i++)
            {
                ProceedRoutes(routes);
            }

            foreach (var route in routes)
            {
                if (!route.HasReached(end))
                {
                    routes.Remove(route);
                }
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
                    if(route.HasReached(end))
                    {
                        tmpRoutes.Add((Route)route.Clone());
                    }
                }

            } while (ProceedRoutesWihtinDistance(routes, distance));

            return null;
        }
    }
}
