using System.Collections.Generic;

namespace Promob.Grafos.Dijkstra
{
    public class DijkstraNode
    {       
        public string Name { get; set; }
        public IList<DijkstraNode> Adjacencias { get; set; } = new List<DijkstraNode>();
        public double Cost { get; set; }

        public DijkstraNode(string name)
        {
            Name = name;
            Cost = 0;
        }
        public DijkstraNode()
        {
            Name = "Aux";
            Cost = 0;
        }
    }
}
