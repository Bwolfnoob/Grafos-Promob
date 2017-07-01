using System.Collections.Generic;
using System.Windows;

namespace Promob.Grafos.Models
{
    public class Node
    {
        public string Name { get; set; }
        public Point Coordenates { get; set; }
        public IList<string> Vertices { get; set; }     
    }
}
