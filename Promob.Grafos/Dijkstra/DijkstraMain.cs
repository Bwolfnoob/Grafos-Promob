using System.Collections.Generic;
using System.Linq;

namespace Promob.Grafos.Dijkstra
{
    public class DijkstraMain
    {        
        public IList<string> Result = new List<string>();
        public double PathCost;
        /**
         * vertices : Esse array contem todo o grafo, é o grafo em si.
         */
        private IList<DijkstraNode> Vertices = new List<DijkstraNode>();
        private IList<DijkstraNode> T;
        private Dictionary<string, string> Path;
        private Dictionary<string, double?> Y;

        public void AddNodoVertice(string nodoName)
        {
            Vertices.Add(new DijkstraNode(nodoName));
        }

        public void AddAdjacencia(string nodeName, string nodeAdjacente, double cost)
        {
            DijkstraNode node = GetNodeFromVertice(nodeName);
            var aux = GetNodeFromVertice(nodeAdjacente);
            node.Adjacencias.Add(new DijkstraNode() { Name = aux.Name, Adjacencias = null, Cost = cost });
        }

        public void Dijkstra(string nodoInicial, string nodoFinal)
        {
            PathCost = 0;           
            Y = new Dictionary<string, double?>();
            Path = new Dictionary<string, string>();

            DijkstraNode nodoAux = null;
            double? menor = double.MaxValue;

            foreach (DijkstraNode nodo in Vertices)
            {
                if (nodo.Name.Equals(nodoInicial))
                {
                    Y.Add(nodo.Name, 0);
                }
                else
                {
                    Y.Add(nodo.Name, null);
                }
            }

            T = Vertices.ToList();
            nodoAux = null;

            while (true)
            {
                foreach (DijkstraNode elementoT in T)
                {
                    if (GetValueFromY(elementoT.Name) != null)
                    {
                        if (GetValueFromY(elementoT.Name) < menor)
                        {
                            menor = GetValueFromY(elementoT.Name);
                            nodoAux = elementoT;
                        }
                    }
                }

                if (nodoAux.Name.Equals(nodoFinal))
                {
                    GeneratePath(nodoInicial, nodoFinal);
                    return;
                }

                foreach (DijkstraNode nodoAdjacente in nodoAux.Adjacencias)
                {
                    if (ContainsInT(nodoAdjacente.Name))
                    {
                        if (GetValueFromY(nodoAdjacente.Name) == null)
                        {
                            Y[nodoAdjacente.Name] = nodoAdjacente.Cost + ((GetValueFromY(nodoAux.Name) == null)
                                                    ? 0 : GetValueFromY(nodoAux.Name));
                            Path.Add(nodoAdjacente.Name, nodoAux.Name);
                            continue;
                        }

                        if (GetValueFromY(nodoAdjacente.Name) > (GetValueFromY(nodoAux.Name) + nodoAdjacente.Cost))
                        {
                            Y[nodoAdjacente.Name] = GetValueFromY(nodoAux.Name) + nodoAdjacente.Cost;

                            if (Path.ContainsKey(nodoAdjacente.Name))
                            {
                                //Verificar porque nao esta removendo
                                Path.Remove(nodoAdjacente.Name);
                            }
                            Path.Add(nodoAdjacente.Name, nodoAux.Name);
                        }
                    }
                }
                T.Remove(T.First(c => c.Name.Equals(nodoAux.Name)));
                menor = int.MaxValue;
            }
        }

        public void AddPathCost(string nodo, string nodoAdjascente)
        {
            PathCost += Vertices.First(c => c.Name.Equals(nodo)).Adjacencias.First(c=>c.Name.Equals(nodoAdjascente)).Cost;
        }

        private void GeneratePath(string nodoInicial, string nodoFinal)
        {
            Result.Add(nodoFinal);
            var i = Path.First(c => c.Key.Equals(nodoFinal)).Value;

            AddPathCost(nodoFinal , i);

            while (!string.IsNullOrEmpty(i))
            {
                var iAux = i;
                Result.Add(i);
                i = Path.FirstOrDefault(c => c.Key.Equals(i)).Value;
                if (!string.IsNullOrEmpty(i)) {
                    AddPathCost(iAux, i);
                }
            }
        }
     
        private bool ContainsInT(string nome)
        {
            return T.Any(c => c.Name.Equals(nome));
        }

        private double? GetValueFromY(string nodoName)
        {
            return Y.First(c => c.Key.Equals(nodoName)).Value;
        }

        public DijkstraNode GetNodeFromVertice(string nodoName)
        {
            return Vertices.First(c => c.Name.Equals(nodoName));
        }
    }
}
