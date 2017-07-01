using System;
using System.Windows;

namespace Promob.Grafos.Models
{
    internal class Aresta
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public string P1Name { get; set; }
        public string P2Name { get; set; }

        public Aresta(Point p1, Point p2,string p1Name , string p2Name)
        {
            P1 = p1;
            P2 = p2;
            P1Name = p1Name;
            P2Name = p2Name;
        }
          
        public double Distancia
        {
            get { return CalcularDistancia(P1, P2); }
        }

        private Func<Point, Point, double> CalcularDistancia = (Point p1, Point p2) => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
}