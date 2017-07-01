using System;
using System.Collections.Generic;
using System.Linq;

namespace Promob.Grafos.Fleury
{
    public class FleuryMain
    {
        private IList<FleuryNode> Vertices = new List<FleuryNode>();

        private IList<FleuryNode> G = new List<FleuryNode>();

        public IList<FleuryNode> Caminho = new List<FleuryNode>();

        public bool GrafoEuleriano = true;

        IList<FleuryNode> nodosImpar;

        public void AddNodoVertice(string nodoName)
        {
            Vertices.Add(new FleuryNode(nodoName));
        }

        public void AddAdjascenciaVertice(string nodeName, string nodeAdjacente)
        {
            FleuryNode node = GetNodeFromVertice(nodeName);
            var adjascencia = GetNodeFromVertice(nodeAdjacente);
            node.Adjacencias.Add(adjascencia);
        }

        private FleuryNode GetNodeFromVertice(string nodeName)
        {
            return Vertices.First(c => c.Name.Equals(nodeName));
        }

        public void Run()
        {
            G = Vertices.ToList();

            nodosImpar = new List<FleuryNode>();

            Caminho = new List<FleuryNode>();

            CriarFila(Vertices.First());
      
            if (VerificaCaminhoAdjacencia())
            {
                Fleury();
            }
            else
            {
                GrafoEuleriano = false;
            }
        }

        public void CriarArvore(FleuryNode nodoA)
        {
            FleuryNode nodoPai = new FleuryNode(nodoA.Name);

            Arvore.Add(nodoPai);

            foreach (var item in nodoA.Adjacencias)
            {
                if (!Arvore.Any(c => c.Name.Equals(item.Name)))
                {
                    FleuryNode nodoFilho = new FleuryNode(item.Name);
                    nodoPai.Adjacencias.Add(nodoFilho);
                    CriarArvore(item);
                }
            }
        }

        private bool VerificaCaminhoAdjacencia()
        {
            int qtdImpar = 0;
            foreach (var item in Vertices)
            {
                if (item.Grau % 2 != 0)
                {
                    nodosImpar.Add(item);
                    qtdImpar++;
                }
            }

            if (qtdImpar == 1 || qtdImpar > 2)
            {
                return false;
            }
            return true;
        }

        IList<FleuryNode> Arvore = new List<FleuryNode>();

        Queue<FleuryNode> F = new Queue<FleuryNode>();

        public void CriarFila(FleuryNode nodoA)
        {
            //Enqueue final da lista
            //Dequeue Remover
            int aux = 0;
            FleuryNode nodoArvore;
            nodoA.Nivel = aux;
            Arvore.Add(new FleuryNode(nodoA.Name));

            F.Enqueue(nodoA);

            while (F.Count > 0)
            {
                aux++;
                FleuryNode nd = F.Dequeue();

                nodoArvore = Arvore.First(c => c.Name.Equals(nd.Name));

                foreach (var item in nd.Adjacencias)
                {
                    if (!Arvore.Any(c => c.Name.Equals(item.Name)))
                    {
                        FleuryNode nodoFilho = new FleuryNode(item.Name);
                        nodoFilho.Nivel = aux;
                        nodoArvore.Adjacencias.Add(nodoFilho);
                        Arvore.Add(nodoFilho);
                        F.Enqueue(item);
                    }
                }
            }
        }

        public void RemoveAresta(IList<FleuryNode> grafo, FleuryNode vertice, FleuryNode adjacencia)
        {
            var verticeGrafo = G.First(c => c.Name.Equals(vertice.Name));
            verticeGrafo.Adjacencias.Remove(adjacencia);
        }

        public void Fleury()
        {
            FleuryNode vertice;

            if (nodosImpar.Count > 0)
            {
                vertice = G.First(c => c.Name.Equals(nodosImpar.First().Name));
            }
            else
            {
                vertice = G.First();
            }

            Caminho.Add(vertice);

            IList<string> C1 = new List<string>();

            while (G.Sum(c => c.Grau) > 0)
            {
                var vi = Caminho.Last();
                FleuryNode ai = new FleuryNode();

                if (vi.Grau == 1)
                {
                    ai = vi.Adjacencias.First();
                }
                else
                {
                    foreach (var item in vi.Adjacencias.OrderBy(c => c.Grau))
                    {
                        var a = Arvore.First(c => c.Name.Equals(item.Name));
                        var viG = Arvore.First(c => c.Name.Equals(vi.Name));

                        if (a.Nivel < viG.Nivel)
                        {
                            ai = item;
                            continue;
                        }
                        else
                        {
                          ai = item;
                          break;
                       }
                    }
                }
                
                RemoveAresta(G, vi, ai);
                RemoveAresta(G, ai, vi);

                Caminho.Add(ai);
            }
        }
    }
}