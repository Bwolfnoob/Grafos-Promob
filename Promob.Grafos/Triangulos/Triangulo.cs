using Promob.Grafos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Promob.Grafos.Triangulos
{
    public class Triangulo
    {
        public Node Canto1 { get; private set; }
        public Node Canto2 { get; private set; }
        public Node Canto3 { get; private set; }

        public double Angulo1
        {
            get
            {
                return Angle(DistanciaAB, DistanciaAC, DistanciaBC);
            }
        }

        public double Angulo2
        {
            get
            {
                return Angle(DistanciaAC, DistanciaAB, DistanciaBC);
            }
        }

        public double Angulo3
        {
            get
            {
                return Angle(DistanciaBC, DistanciaAB, DistanciaAC);
            }
        }

        public double Area
        {
            get
            {
                var S = SemiPerimetro(DistanciaAB, DistanciaAC, DistanciaBC);
                return Math.Sqrt(S * (S - DistanciaAB) * (S - DistanciaAC) * (S - DistanciaBC));
            }
        }

        public Triangulo(Node c1, Node c2, Node c3)
        {
            List<Node> cantos = new List<Node>() { c1, c2, c3 }.OrderBy(c => c.Name).ToList();
            Canto1 = cantos[0];
            Canto2 = cantos[1];
            Canto3 = cantos[2];
        }

        private double DistanciaAB
        {
            get { return CalcularDistancia(Canto1.Coordenates, Canto2.Coordenates); }
        }

        private double DistanciaAC
        {
            get { return CalcularDistancia(Canto1.Coordenates, Canto3.Coordenates); }
        }

        private double DistanciaBC
        {
            get { return CalcularDistancia(Canto2.Coordenates, Canto3.Coordenates); }
        }

        private Func<double, double, double, double> SemiPerimetro = (double d1, double d2, double d3) => (d1 + d2 + d3) / 2;

        private Func<double, double, double, double> Angle = (double d1, double d2, double d3) => RadianToDegree(Math.Acos((Math.Pow(d1, 2) - Math.Pow(d2, 2) - Math.Pow(d3, 2)) / (-2 * d3 * d2)));

        private static Func<double, double> RadianToDegree = (double angle) => angle * (180.0 / Math.PI);

        private Func<Point, Point, double> CalcularDistancia = (Point p1, Point p2) => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));

        public override bool Equals(object obj)
        {
            var tr = (Triangulo)obj;
            if (Canto1.Name == tr.Canto1.Name && Canto2.Name == tr.Canto2.Name && Canto3.Name == tr.Canto3.Name)
            {
                return true;
            }
            return false;
        }
    }
}
