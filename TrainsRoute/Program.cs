using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace TrainsRoute
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@".\Input.xml");

            XmlNodeList nodeList = xmlDoc.SelectNodes("/Input/Edges/*");

            foreach (var item in nodeList)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
