using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace AStarVisualizer
{
    static class Pathfinding<T>
       where T : IComparable<T>
    {
      /*  public static List<Vertex<T>> Djikstra(Graph<T> graph, T starting, T ending)
        {
            List<Vertex<T>> path = new List<Vertex<T>>();
            PriorityQueue<Vertex<T>, double> priorityQueue = new PriorityQueue<Vertex<T>, double>();

            Vertex<T> start = graph.Search(starting);
            Vertex<T> end = graph.Search(ending);
            Dictionary<Vertex<T>, double> distances = new Dictionary<Vertex<T>, double>();
            Dictionary<Vertex<T>, Vertex<T>> founders = new Dictionary<Vertex<T>, Vertex<T>>();
            Vertex<T> curr = null;
            foreach (Vertex<T> a in graph.Vertices)
            {
                a.isVisited = false;
            }
            priorityQueue.Enqueue(start, 0);
            distances.Add(start, 0);
            founders.Add(start, null);

            while (priorityQueue.Count > 0)
            {
                if (curr == end)
                {
                    while (curr != null)
                    {
                        path.Add(curr);
                        curr = founders[curr];
                    }
                    break;
                }
                curr = priorityQueue.Dequeue();
                if (curr.isVisited == true)
                {
                    continue;
                }

                curr.isVisited = true;
                foreach (Edge<T> a in curr.Neighbors)
                {
                    if (a.EndingPoint == curr)
                    {
                        continue;
                    }
                    if (a.EndingPoint.isVisited == false)
                    {
                        if (distances.ContainsKey(a.EndingPoint))
                        {
                            if (distances[curr] + a.Distance < distances[a.EndingPoint])
                            {
                                distances[a.EndingPoint] = distances[curr] + a.Distance;
                                founders[a.EndingPoint] = curr;
                                priorityQueue.Enqueue(a.EndingPoint, distances[a.EndingPoint]);
                            }
                        }
                        else
                        {
                            priorityQueue.Enqueue(a.EndingPoint, distances[curr] + a.Distance);
                            distances.Add(a.EndingPoint, a.Distance + distances[curr]);
                            founders.Add(a.EndingPoint, curr);
                        }

                    }
                }
            }
            return path;
        }
      */
        enum Heuristics
        {
            Manhattan,
            Chebyshev,
            Octile,
            Euclidian,
        }
        static public double Manhattan(Point point, Point goal)
        {
            double dx = Math.Abs(point.X - goal.X);
            double dy = Math.Abs(point.Y - goal.Y);

            return (dx + dy);
        }
        static public double Euclidian(Point point, Point goal)
        {
            double dx = Math.Abs(point.X - goal.X);
            double dy = Math.Abs(point.Y - goal.Y);

            return Math.Sqrt(dx * dx + dy * dy);
        }
        static public double Chebyshev(Point point, Point goal)
        {
            double dx = Math.Abs(point.X - goal.X);
            double dy = Math.Abs(point.Y - goal.Y);

            return dx + dy + (-2) * Math.Min(dx, dy);
        }
        static public double Octile(Point point, Point goal)
        {
            double dx = Math.Abs(point.X - goal.X);
            double dy = Math.Abs(point.Y - goal.Y);

            return dx + dy + (Math.Sqrt(2) - 2) * Math.Min(dx, dy);
        }

         

        public static List<Vertex<Point>> AStar(Graph<Point> graph, Vertex<Point> starting, Vertex<Point> ending, Func<Point, Point, double> Heur)
        {
            if (starting == null || ending == null || Heur == null)
            {
                return null;
            }
            List<Vertex<Point>> path = new List<Vertex<Point>>();
            PriorityQueue<Vertex<Point>, double> priorityQueue = new PriorityQueue<Vertex<Point>, double>();
            Dictionary<Vertex<Point>, Vertex<Point>> founders = new Dictionary<Vertex<Point>, Vertex<Point>>();
            Vertex<Point> curr = null;

            foreach (Vertex<Point> a in graph.Vertices)
            {
                a.isVisited = false;
                a.founder = null;
                a.FinalDistance = double.PositiveInfinity; 
                a.CumulativeDistance = double.PositiveInfinity;
            }
            starting.FinalDistance = 0;
            starting.CumulativeDistance = 0;
            founders.Add(starting, null);
            priorityQueue.Enqueue(starting, starting.CumulativeDistance); 

            while (priorityQueue.Count > 0)
            {
                curr = priorityQueue.Dequeue();
                if (curr == ending)
                {
                    while (curr != null)
                    {
                        path.Add(curr);
                        curr = founders[curr];
                    }
                    return path; 
                }
                if(curr.isVisited)
                {
                    continue; 
                }
                double tentDistance = 0;
                foreach(Edge<Point> ej in curr.Neighbors)
                {
                    if(ej.StartingPoint != curr)
                    {
                        continue; 
                    }
                    if(!ej.EndingPoint.isVisited)
                    {
                        tentDistance = Heur(ej.EndingPoint.value, ending.value) + ej.StartingPoint.CumulativeDistance; 
                        if (tentDistance < ej.EndingPoint.FinalDistance)
                        {
                            ej.EndingPoint.CumulativeDistance = ej.StartingPoint.CumulativeDistance + ej.Distance;
                            ej.EndingPoint.FinalDistance = tentDistance; 
                            founders.Add(ej.EndingPoint, curr);
                            priorityQueue.Enqueue(ej.EndingPoint, ej.EndingPoint.FinalDistance);
                        }
                    }
                    ej.StartingPoint.isVisited = true;
                    curr.isVisited = true; 
                }
            }
            return path; 
        }
    }


}
