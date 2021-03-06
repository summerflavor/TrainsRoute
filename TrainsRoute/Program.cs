﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace TrainsRoute
{
    class Program
    {
        private const string RouteDistance = "RouteDistance";
        private const string RouteWthinCertainStops = "RouteWthinCertainStops";
        private const string RouteWithExactStops = "RouteWithExactStops";
        private const string ShortestRoute = "ShortestRoute";
        private const string RouteWthinCertainDistance = "RouteWthinCertainDistance";

        static void Main(string[] args)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"../../Input.xml");

            XmlNodeList nodeList = xmlDoc.SelectNodes("/Input/Edges/*");

            // Initialize the railwat network with the given input
            RailwayNetwork railwayNetwork = new RailwayNetwork();
            List<Edge> edges = new List<Edge>();

            foreach (XmlElement item in nodeList)
            {
                Char[] chars = item.InnerText.ToCharArray();
                Edge tmpEdge = new Edge( chars[0].ToString(), chars[1].ToString(), int.Parse( chars[2].ToString()));
                edges.Add(tmpEdge);
            }

            railwayNetwork.Initialize(edges);

            // Get the questions from the given input
            XmlNodeList qustionList = xmlDoc.SelectNodes("/Input/Qustions/*");

            foreach (XmlElement item in qustionList)
            {
                if (item.Name == Program.RouteDistance)
                {
                    string[] strStops = item.InnerText.Split('-');
                    Route route = new Route();

                    for (int i = 0; i < strStops.Length - 1; i++)
                    {
                        route.AddEdge(new Edge(strStops[i], strStops[i + 1]));
                    }

                    Console.WriteLine("Route distance:");

                    if (railwayNetwork.IsRouteUnimpeded(route))
                    {
                        route.AddDistanceToEdges(railwayNetwork);
                        Console.WriteLine(route.ToString());
                    }
                    else
                    {
                        Console.WriteLine(route.ToString());
                        Console.WriteLine("NO SUCH ROUTE");
                    }

                    Console.WriteLine();

                }
                else if (item.Name == Program.ShortestRoute)
                {
                    string[] strStops = item.InnerText.Split('-');

                    Route route = railwayNetwork.ShortestRoute(strStops[0], strStops[1]);

                    Console.WriteLine("The shortest route from {0} to {1} is :", strStops[0], strStops[1]);
                    Console.WriteLine(route.ToString());
                    Console.WriteLine();
                }
                else if (item.Name == Program.RouteWithExactStops)
                {
                    string[] strParams = item.InnerText.Split('-');

                    List<Route> routes = railwayNetwork.RoutesWithExactStops(strParams[0], strParams[1], int.Parse(strParams[2]));

                    Console.WriteLine("There are {0} routes from {1} to {2} with {3} stops:", routes.Count, strParams[0], strParams[1], strParams[2]);

                    foreach (var route in routes)
                    {
                        Console.WriteLine(route.ToString());
                    }
                    Console.WriteLine();
                }
                else if (item.Name == Program.RouteWthinCertainStops)
                {
                    string[] strParams = item.InnerText.Split('-');

                    List<Route> routes = railwayNetwork.RoutesWithinCertainStops(strParams[0], strParams[1], int.Parse(strParams[2]));

                    Console.WriteLine("There are {0} routes from {1} to {2} within {3} stops:", routes.Count, strParams[0], strParams[1], strParams[2]);

                    foreach (var route in routes)
                    {
                        Console.WriteLine(route.ToString());
                    }
                    Console.WriteLine();
                }
                else if (item.Name == Program.RouteWthinCertainDistance)
                {
                    string[] strParams = item.InnerText.Split('-');

                    List<Route> routes = railwayNetwork.RoutesWithinCertainDistance(strParams[0], strParams[1], int.Parse(strParams[2]));

                    Console.WriteLine("There are {0} routes from {1} to {2} within distance {3}:", routes.Count, strParams[0], strParams[1], strParams[2]);

                    foreach (var route in routes)
                    {
                        Console.WriteLine(route.ToString());
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Nothing was done");
                }
            }

            Console.ReadKey();
        }
    }
}
