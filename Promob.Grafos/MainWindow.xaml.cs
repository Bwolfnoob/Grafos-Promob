using Newtonsoft.Json;
using Promob.Grafos.Dijkstra;
using Promob.Grafos.Models;
using Promob.Grafos.Triangulos;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System;
using Promob.Grafos.Fleury;
using System.Windows.Input;
using System.Windows.Threading;
using Promob.Grafos.Hamiltoniano;

namespace Promob.Grafos
{
    public partial class MainWindow : Window
    {
        #region DrawVariables

        public double passoY;
        public double passoX;
        private int DiametroNode;
        private int RaioNode;
        private double zeroX;
        private double zeroY;
        private double proporcao;
        #endregion

        internal IList<Node> Nodes { get; set; } = new List<Node>();
        internal IList<Aresta> Arestas = new List<Aresta>();

        private DijkstraMain dm = new DijkstraMain();
        private FleuryMain fm = new FleuryMain();
        private HamiltonianoMain hm = new HamiltonianoMain();
        private TrianguloMain trM = new TrianguloMain();
        private DispatcherTimer eulerDispatcherTimer = new DispatcherTimer();
        private DispatcherTimer hamiltonianoDispatcherTimer = new DispatcherTimer();

        #region DrawVariables
        private Node NdCaminhoEuleriano = new Node();
        private int IndexCaminhoEuleriano = 0;
        private int IndexCaminhoHamiltoniano = 0;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            DiametroNode = 20;
            RaioNode = DiametroNode / 2;

            eulerDispatcherTimer.Tick += eulerDispatcherTimer_Tick;
            eulerDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            hamiltonianoDispatcherTimer.Tick += hamiltonianoDispatcherTimer_Tick;
            hamiltonianoDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

        }

        private void hamiltonianoDispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (IndexCaminhoHamiltoniano + 1 >= hm.Caminho.Count)
            {
                IndexCaminhoHamiltoniano = 0;
                hamiltonianoDispatcherTimer.Stop();
            }
            else
            {
                var a = Nodes.First(c => c.Name.Equals(hm.Caminho[IndexCaminhoHamiltoniano].Name));
                IndexCaminhoHamiltoniano++;
                var b = Nodes.First(c => c.Name.Equals(hm.Caminho[IndexCaminhoHamiltoniano].Name));
                DrawLine(a, b, Brushes.Bisque, 4);

                PrintNode(a, Colors.Blue);
                PrintNode(b, Colors.Blue);
            }
        }

        private void eulerDispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (IndexCaminhoEuleriano + 1 >= fm.Caminho.Count)
            {
                IndexCaminhoEuleriano = 0;
                eulerDispatcherTimer.Stop();

            }
            else
            {
                var a = Nodes.First(c => c.Name.Equals(fm.Caminho[IndexCaminhoEuleriano].Name));
                IndexCaminhoEuleriano++;
                var b = Nodes.First(c => c.Name.Equals(fm.Caminho[IndexCaminhoEuleriano].Name));
                DrawLine(a, b, Brushes.Green, 4);

                PrintNode(a, Colors.Blue);
                PrintNode(b, Colors.Blue);
            }
        }

        private void AddAresta(Point p1, Point p2, string p1Name, string p2Name)
        {
            if (p1Name.CompareTo(p2Name) <= 0)
            {
                if (!VerificarArestaIncluida(Arestas, p1Name, p2Name))
                {
                    Arestas.Add(new Aresta(p1, p2, p1Name, p2Name));
                }
            }
            else
            {
                if (!VerificarArestaIncluida(Arestas, p2Name, p1Name))
                {
                    Arestas.Add(new Aresta(p1, p2, p2Name, p1Name));
                }
            }
        }

        private Func<IList<Aresta>, string, string, bool> VerificarArestaIncluida = (IList<Aresta> arestas, string p1, string p2) => arestas.Any(c => c.P1Name.Equals(p1) && c.P2Name.Equals(p2));

        private void DrawArestas()
        {
            Arestas.Clear();

            foreach (var item in Nodes)
            {
                foreach (var item1 in item.Vertices)
                {
                    var nodoVertice = Nodes.First(c => c.Name == item1);

                    Line lineY = new Line();
                    lineY.Stroke = Brushes.Red;

                    lineY.X1 = zeroX + item.Coordenates.X * passoX;
                    lineY.Y1 = zeroY - item.Coordenates.Y * passoY;

                    lineY.X2 = zeroX + nodoVertice.Coordenates.X * passoX;
                    lineY.Y2 = zeroY - nodoVertice.Coordenates.Y * passoY;

                    lineY.StrokeThickness = 1;
                    canvas.Children.Add(lineY);
                    AddAresta(item.Coordenates, nodoVertice.Coordenates, item.Name, nodoVertice.Name);
                }
            }
        }

        #region DesenharEixos

        private void DrawEixos()
        {
            Line lineY = new Line();
            lineY.Stroke = Brushes.Black;
            lineY.X1 = zeroX;
            lineY.X2 = zeroX;
            lineY.StrokeThickness = 0.1;
            lineY.Y1 = canvas.Width;
            lineY.Y2 = 0;
            canvas.Children.Add(lineY);

            Line lineX = new Line();
            lineX.Stroke = Brushes.Black;
            lineX.X1 = canvas.Width;
            lineX.X2 = 0;
            lineX.StrokeThickness = 0.1;
            lineX.Y1 = zeroY;
            lineX.Y2 = zeroY;
            canvas.Children.Add(lineX);

            Ellipse zeroPoint = new Ellipse();
            zeroPoint.Width = 5;
            zeroPoint.Height = 5;
            zeroPoint.Fill = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(zeroPoint, zeroX - zeroPoint.Width / 2);
            Canvas.SetTop(zeroPoint, zeroY - zeroPoint.Height / 2);
            canvas.Children.Add(zeroPoint);

        }
        #endregion

        #region DesenharNodes      

        private void DrawNodes()
        {
            foreach (var item in Nodes)
            {
                PrintNode(item, Colors.Blue);
            }
        }

        private void PrintNode(Node item, Color color)
        {
            double x = zeroX + item.Coordenates.X * passoX;
            double y = zeroY - item.Coordenates.Y * passoY;
            Ellipse el = new Ellipse();
            el.Width = DiametroNode;
            el.Height = DiametroNode;
            el.Fill = new SolidColorBrush(color);
            Canvas.SetLeft(el, x - RaioNode);
            Canvas.SetTop(el, y - RaioNode);
            canvas.Children.Add(el);
            el.MouseUp += ellipse_MouseUp;

            TextBlock txt = new TextBlock();
            txt.Inlines.Add(new Run(item.Name) { FontWeight = FontWeights.Bold });
            txt.Foreground = new SolidColorBrush(Colors.White);
            txt.FontWeight = FontWeight.FromOpenTypeWeight(10);
            Canvas.SetLeft(txt, x - 5);
            Canvas.SetTop(txt, y - 8);
            canvas.Children.Add(txt);
        }

        private void ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //  throw new NotImplementedException();
        }
        #endregion

        private void SetPasso(double passo = 35)
        {
            proporcao = passo;
            passoY = canvas.Height / passo;
            passoX = canvas.Width / passo;
        }

        private void CarregaJson(string jsonName)
        {
            string json;
            var path = Environment.CurrentDirectory + @"\Jsons\" + jsonName;
            using (StreamReader r = new StreamReader(path))
            {
                json = r.ReadToEnd();
            }

            Nodes = JsonConvert.DeserializeObject<List<Node>>(json);
        }

        private void grafo_0_Click(object sender, RoutedEventArgs e)
        {
            SetPasso();
            zeroX = canvas.Width / 2;
            zeroY = canvas.Height / 2;
            CarregaJson(@"Grafo0.json");
            PreencheCombos();
            Draw();
        }
        private void grafo_1_Click(object sender, RoutedEventArgs e)
        {
            SetPasso();
            zeroX = -50 + canvas.Width / 2;
            zeroY = -50 + canvas.Height / 2;
            CarregaJson(@"Grafo1.json");
            PreencheCombos();
            Draw();
        }
        private void grafo_2_Click(object sender, RoutedEventArgs e)
        {
            SetPasso(45);
            zeroX = canvas.Width / 2;
            zeroY = canvas.Height / 2;
            CarregaJson(@"Grafo2.json");
            PreencheCombos();
            Draw();

        }
        private void grafo_3_Click(object sender, RoutedEventArgs e)
        {
            SetPasso();
            zeroX = 100 + canvas.Width / 2;
            zeroY = 100 + canvas.Height / 2;
            CarregaJson(@"Grafo3.json");
            PreencheCombos();
            Draw();
        }
        private void grafo_4_Click(object sender, RoutedEventArgs e)
        {
            SetPasso(40);
            zeroX = canvas.Width / 2;
            zeroY = canvas.Height / 2;
            CarregaJson(@"Grafo4.json");
            PreencheCombos();
            Draw();
        }
        private void grafo_5_Click(object sender, RoutedEventArgs e)
        {
            SetPasso();
            zeroX = -100 + canvas.Width / 2;
            zeroY = -100 + canvas.Height / 2;
            CarregaJson(@"Grafo5.json");
            PreencheCombos();
            Draw();
        }
        private void grafo_6_Click(object sender, RoutedEventArgs e)
        {
            SetPasso();
            zeroX = 210 + canvas.Width / 2;
            zeroY = -60 + canvas.Height / 2;
            CarregaJson(@"Grafo6.json");
            PreencheCombos();
            Draw();
        }
        private void grafo_k5_Click(object sender, RoutedEventArgs e)
        {
            SetPasso();
            zeroX = canvas.Width / 2;
            zeroY = canvas.Height / 2;
            CarregaJson(@"k5.json");
            PreencheCombos();
            Draw();
        }
        private void grafo_k33_Click(object sender, RoutedEventArgs e)
        {
            SetPasso();
            zeroX = canvas.Width / 2;
            zeroY = canvas.Height / 2;
            CarregaJson(@"k33.json");
            PreencheCombos();
            Draw();
        }

        private void PreencheCombos()
        {
            comboNodoInicio.Items.Clear();
            comboNodoFim.Items.Clear();
            foreach (var item in Nodes)
            {
                comboNodoInicio.Items.Add(item.Name);
                comboNodoFim.Items.Add(item.Name);
            }
            comboNodoInicio.SelectedIndex = 0;
            comboNodoFim.SelectedIndex = 1;
        }

        private void Draw()
        {
            canvas.Children.Clear();
            DrawEixos();
            DrawArestas();
            DrawNodes();
            listView.Items.Clear();
            //listView.Items.Add("Arestas:" + Arestas.Count);
        }

        private void Dijkstra_Click(object sender, RoutedEventArgs e)
        {
            eulerDispatcherTimer.Stop();
            hamiltonianoDispatcherTimer.Stop();
            dm = new DijkstraMain();

            foreach (var item in Nodes)
            {
                dm.AddNodoVertice(item.Name);
            }

            foreach (var item in Arestas)
            {
                //  Console.WriteLine(item.P1Name + item.P2Name);
                //Ida e Volta
                dm.AddAdjacencia(item.P1Name, item.P2Name, item.Distancia);
                dm.AddAdjacencia(item.P2Name, item.P1Name, item.Distancia);
            }

            string inicio = comboNodoInicio.SelectedItem.ToString();
            string fim = comboNodoFim.SelectedItem.ToString();
            if (!inicio.Equals(fim))
            {
                dm.Dijkstra(inicio, fim);
            }
            else
            {
                MessageBox.Show("Escolha dois pontos validos!!!");
            }
            DrawResult();
        }
        private void DrawResult()
        {
            Draw();
            listView.Items.Add("Distancia: " + Math.Round(dm.PathCost, 5));

            Node n1 = new Node();
            Node n2 = null;

            foreach (var item in dm.Result)
            {
                n1 = Nodes.First(c => c.Name.Equals(item));

                if (n2 == null)
                {
                    n2 = n1;
                    continue;
                }
                DrawLine(n2, n1, Brushes.Azure, 4);
                PrintNode(n2, Colors.Green);
                PrintNode(n1, Colors.Green);
                n2 = n1;
            }
        }
        private void Calcula_Euleriano_Click(object sender, RoutedEventArgs e)
        {
            hamiltonianoDispatcherTimer.Stop();
            Draw();
            fm = new FleuryMain();

            foreach (var item in Nodes)
            {
                fm.AddNodoVertice(item.Name);
            }

            foreach (var item in Arestas)
            {
                //Ida e Volta
                fm.AddAdjascenciaVertice(item.P1Name, item.P2Name);
                fm.AddAdjascenciaVertice(item.P2Name, item.P1Name);
            }

            fm.Run();

            if (fm.GrafoEuleriano)
            {
                eulerDispatcherTimer.Start();
            }
            else
            {
                MessageBox.Show("Não possui um caminho Euleriano!!!");
            }

        }
        private void Calcula_Hamiltoniano_Click(object sender, RoutedEventArgs e)
        {
            Draw();
            hm = new HamiltonianoMain();

            foreach (var item in Nodes)
            {
                hm.AddNodoVertice(item.Name);
            }

            foreach (var item in Arestas)
            {
                //Ida e Volta
                hm.AddAdjacencia(item.P1Name, item.P2Name, item.Distancia);
                hm.AddAdjacencia(item.P2Name, item.P1Name, item.Distancia);
            }

            hm.Run();

            if (hm.PossuiCaminho())
            {
                listView.Items.Add("Custo do Caminho: " + Math.Round(hm.Caminho.Sum(c => c.Cost), 5)); ;
                hamiltonianoDispatcherTimer.Start();
            }
            else
            {
                MessageBox.Show("Este grafo não possui um caminho Hamiltoniano!!!");
            }
        }
        private void DrawLine(Node nd1, Node nd2, Brush brush = null, double stroke = 2)
        {
            Line lineY = new Line();

            if (brush == null)
            {
                lineY.Stroke = Brushes.Black;
            }
            else
            {
                lineY.Stroke = brush;
            }
            lineY.X1 = zeroX + nd2.Coordenates.X * passoX;
            lineY.Y1 = zeroY - nd2.Coordenates.Y * passoY;
            lineY.X2 = zeroX + nd1.Coordenates.X * passoX;
            lineY.Y2 = zeroY - nd1.Coordenates.Y * passoY;
            lineY.StrokeThickness = stroke;

            canvas.Children.Add(lineY);
        }
        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Calcula_Triangulos_Click(object sender, RoutedEventArgs e)
        {
            hamiltonianoDispatcherTimer.Stop();
            Draw();
            listView.Items.Clear();
            trM.CalculaTriangulos(Nodes, Arestas);

            if (trM.Triangulos.Count == 0)
            {
                listView.Items.Add("Nenhum triângulo \n encontrado!!!");
                MessageBox.Show("Nenhum triângulo encontrado!!!");
            }
            else
            {
                Triangulo maiorTriangulo = trM.Triangulos.First(c => c.Area == trM.Triangulos.Max(c1 => c1.Area));
                listView.Items.Add("Maior Triângulo : " + maiorTriangulo.Canto1.Name + "," + maiorTriangulo.Canto2.Name + "," + maiorTriangulo.Canto3.Name);
                listView.Items.Add("Área : " + Math.Round(maiorTriangulo.Area, 5));
                listView.Items.Add("Ângulos : " + Math.Round(maiorTriangulo.Angulo1, 2) + "°;  " + Math.Round(maiorTriangulo.Angulo2, 2) + "°;  " + Math.Round(maiorTriangulo.Angulo3, 2) + "°");

                // listView.Items.Add("Angulo 1 : " + Math.Round(maiorTriangulo.Angulo1, 5) + "°");
                // listView.Items.Add("Angulo 2 : " + Math.Round(maiorTriangulo.Angulo2, 5) + "°");
                // listView.Items.Add("Angulo 3 : " + Math.Round(maiorTriangulo.Angulo3, 5) + "°");

                PointCollection pointCollection = new PointCollection();

                pointCollection.Add(new Point(zeroX + maiorTriangulo.Canto1.Coordenates.X * passoX, zeroY - maiorTriangulo.Canto1.Coordenates.Y * passoY));
                pointCollection.Add(new Point(zeroX + maiorTriangulo.Canto2.Coordenates.X * passoX, zeroY - maiorTriangulo.Canto2.Coordenates.Y * passoY));
                pointCollection.Add(new Point(zeroX + maiorTriangulo.Canto3.Coordenates.X * passoX, zeroY - maiorTriangulo.Canto3.Coordenates.Y * passoY));

                Polygon polygon = new Polygon();
                polygon.Points = pointCollection;
                polygon.Fill = Brushes.Blue;
                polygon.Stretch = Stretch.Fill;
                polygon.Stroke = Brushes.Black;

                Canvas.SetLeft(polygon, zeroX + Menor(maiorTriangulo.Canto1.Coordenates.X, maiorTriangulo.Canto2.Coordenates.X, maiorTriangulo.Canto3.Coordenates.X) * passoX);
                Canvas.SetTop(polygon, zeroY - Maior(maiorTriangulo.Canto1.Coordenates.Y, maiorTriangulo.Canto2.Coordenates.Y, maiorTriangulo.Canto3.Coordenates.Y) * passoY);

                canvas.Children.Add(polygon);

                PrintNode(maiorTriangulo.Canto1, Colors.Green);
                PrintNode(maiorTriangulo.Canto2, Colors.Green);
                PrintNode(maiorTriangulo.Canto3, Colors.Green);
            }
        }

        private Func<double, double, double, double> Menor = (double v1, double v2, double v3) => (v1 <= v2) ? (v1 <= v3 ? v1 : v3) : (v2 <= v3 ? v2 : v3);
        private Func<double, double, double, double> Maior = (double v1, double v2, double v3) => (v1 >= v2) ? (v1 >= v3 ? v1 : v3) : (v2 >= v3 ? v2 : v3);       
    }
}
