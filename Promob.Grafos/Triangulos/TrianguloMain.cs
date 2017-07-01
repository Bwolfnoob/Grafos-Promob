using System.Collections.Generic;
using System.Linq;
using Promob.Grafos.Models;

namespace Promob.Grafos.Triangulos
{
    public class TrianguloMain
    {
        public IList<Triangulo> Triangulos { get; set; }
        internal void CalculaTriangulos(IList<Node> nodes, IList<Aresta> arestas)
        {
            Triangulos =new List<Triangulo>();
            foreach (Node node in nodes)
            {
                foreach (string vertice in node.Vertices)
                {
                    Node ndVertice = nodes.First(c=>c.Name.Equals(vertice));

                    foreach (string item in ndVertice.Vertices)
                    {
                        Node ndItem = nodes.First(c => c.Name.Equals(item));

                        if (ndItem.Vertices.Contains(node.Name))
                        {
                            AddTriangulo(new Triangulo(node, ndVertice, ndItem));
                        }
                    }
                }
            }
        }

        private void AddTriangulo(Triangulo triangulo)
        {
            if (!Triangulos.Contains(triangulo))
            {
                Triangulos.Add(triangulo);
            }
        }
    }
}
