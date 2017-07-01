using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promob.Grafos.Hamiltoniano
{
    public class HamiltonianoMain
    {
        public IList<HamNode> Vertices { get; private set; } = new List<HamNode>();

        public void AddNodoVertice(string nodoName)
        {
            Vertices.Add(new HamNode(nodoName));
        }

        public void AddAdjacencia(string nodeName, string nodeAdjacente, double cost)
        {
            // DijkstraNode node = GetNodeFromVertice(nodeName);
            // var aux = GetNodeFromVertice(nodeAdjacente);
            // node.Adjacencias.Add(new DijkstraNode() { Name = aux.Name, Adjacencias = null, Cost = cost });

            HamNode node = GetNodeFromVertice(nodeName);
            var aux = GetNodeFromVertice(nodeAdjacente);
            node.Adjacencias.Add(new HamNode() { Name = aux.Name, Adjacencias = aux.Adjacencias, Cost = cost });
        }

        private HamNode GetNodeFromVertice(string nodeName)
        {
            return Vertices.First(c => c.Name.Equals(nodeName));
        }

        public IList<HamNode> Caminho;

        public void Run()
        {
            IList<HamNode> CaminhoAux;

            for (int i = 0; i < Vertices.Count; i++)
            {
                var fail = false;
                CaminhoAux = new List<HamNode>();
                HamNode v = Vertices[i];
                CaminhoAux.Add(v);

                while (Vertices.Count > CaminhoAux.Count)
                {
                    try
                    {
                        var a = v.Adjacencias.Where(c => !ContainIn(CaminhoAux, c.Name));
                        HamNode node = a.OrderBy(c => c.Adjacencias.Count).ThenBy(c => c.Cost).First();
                        CaminhoAux.Add(new HamNode(node.Name) { Cost = node.Cost });
                        v = Vertices.First(c => c.Name.Equals(node.Name));
                    }
                    catch (Exception)
                    {
                        fail = true;
                        break;
                    }
                }

                if (fail)
                {
                    continue;
                }

                if (Caminho == null)
                {
                    Caminho = CaminhoAux;
                }
                else
                {
                    var costM = Caminho.Sum(c => c.Cost);
                    var costC = CaminhoAux.Sum(c => c.Cost);

                    if (costM > costC)
                    {
                        Caminho = CaminhoAux;
                    }
                }
            }
        }

        private Func<IList<HamNode>, string, bool> ContainIn = (IList<HamNode> nodes, string nodeName) => nodes.Any(c => c.Name.Equals(nodeName));

        public bool PossuiCaminho()
        {
            return Caminho != null;
        }
    }
}
