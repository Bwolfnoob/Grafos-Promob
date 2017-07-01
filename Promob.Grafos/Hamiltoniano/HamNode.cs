using System.Collections.Generic;

namespace Promob.Grafos.Hamiltoniano
{
   public class HamNode
    {
        public string Name { get; set; }
        public IList<HamNode> Adjacencias { get; set; } = new List<HamNode>();
        public double Cost { get; set; }

        public HamNode(string name)
        {
            Name = name;
            Cost = 0;
        }
        public HamNode()
        {
            Name = "Aux";
            Cost = 0;
        }
    }
}