using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promob.Grafos.Fleury
{
    public class FleuryNode
    {
        public string Name { get; set; }
        public IList<FleuryNode> Adjacencias { get; set; } = new List<FleuryNode>();
        public int Nivel { get; set; }

        public int Grau
        {
            get { return Adjacencias == null ? 0 : Adjacencias.Count(); }
        }


        public FleuryNode(string name)
        {
            Name = name;
        }

        public FleuryNode()
        {
            Name = "Aux";
        }
    }
}
