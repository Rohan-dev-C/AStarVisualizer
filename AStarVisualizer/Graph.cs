using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace AStarVisualizer
{
   
       
    public class Graph<T>
    {
        private List<Vertex<T>> vertices;

        public IReadOnlyList<Vertex<T>> Vertices => vertices;

        private List<Edge<T>> edges;
        public IReadOnlyList<Edge<T>> Edges => edges;
        public int VertexCount => vertices.Count;
        public IComparer<Point> comparer;


        public Graph(IComparer<Point> comparer)
        {
            vertices = new List<Vertex<T>>();
            this.comparer = comparer;
            if(comparer == null)
            {
                comparer = Comparer<Point>.Default;
            }
        }
        public void AddVertex(T vertex1)
        {
            Vertex<T> vertex = new Vertex<T>(vertex1);
            if (vertex == null || vertex.Neighbors.Count != 0 || vertices.Contains(vertex)) { throw new Exception("bad input"); }

            vertices.Add(vertex);
        }
        // FIX T to Microsoft.Xna.Framework
        public bool RemoveVertex(Point value)
        {
            Vertex<T> vertex = Search(value);
            if (Vertices.Contains(vertex))
            {
                for (int i = 0; i < Edges.Count; i++)
                {
                    if (Edges[i].EndingPoint == vertex)
                    {
                        Edges[i].StartingPoint.Neighbors.Remove(edges[i]); 
                    }
                    edges.Remove(Edges[i]); 
                }
                vertex.Neighbors.Clear();
                vertices.Remove(vertex);
                return true;
            }
            return false;
        }

        public bool AddEdge(Point a, Point b, float distance)
        {
            Vertex<T> vertex1 = Search(a);
            Vertex<T> vertex2 = Search(b);
            if (a != null && b != null)
            {
                if (vertices.Contains(vertex1) && vertices.Contains(vertex2))
                {
                    Edge<T> edge = new Edge<T>(vertex1, vertex2, distance);
                    vertex1.Neighbors.Add(edge);
                    return true;
                }
            }
            return false;
        }

        public bool RemoveEdge(Point a, Point b)
        {
            Vertex<T> vertex1 = Search(a);
            Vertex<T> vertex2 = Search(b);
            if (vertex1 != null && vertex2 != null)
            {
                if (vertices.Contains(vertex1) && vertices.Contains(vertex2))
                {
                    var search = GetEdge(vertex1, vertex2);
                    vertex1.Neighbors.Remove(search);
                    return true;
                }
            }
            return false;
        }


        public Vertex<T> Search(Point value)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (vertices[i].value.Equals(value))
                {
                    return vertices[i];
                }
            }
            return null;
        }

        private Edge<T> GetEdge(Point a, Point b)
        {
            Vertex<T> start = Search(a);
            Vertex<T> ending = Search(b);
            for (int i = 0; i < start.NeighborCount; i++)
            {
                if (start.Neighbors[i].EndingPoint == ending)
                {
                    return start.Neighbors[i];
                }
            }
            return null;
        }
        private Edge<T> GetEdge(Vertex<T> a, Vertex<T> b)
        {
            for (int i = 0; i < a.NeighborCount; i++)
            {
                if (a.Neighbors[i].EndingPoint == b)
                {
                    return a.Neighbors[i];
                }
            }
            return null;
        }

        private double DepthFirst(Point a, Point b)
        {
            Vertex<T> start = Search(a);
            Vertex<T> ending = Search(b);

            List<Vertex<T>> stack = new List<Vertex<T>>();

            Dictionary<Vertex<T>, Vertex<T>> founders = new Dictionary<Vertex<T>, Vertex<T>>();

            double total = 0;

            vertices.ForEach(a => a.isVisited = false);
            stack.Add(start);

            Vertex<T> curr = null;

            while (stack.Count > 0)
            {
                curr = stack[stack.Count - 1];
                stack.Remove(stack[stack.Count - 1]);
                curr.isVisited = true;

                if (curr == ending)
                {
                    return total;
                }
                for (int i = 0; i < curr.Neighbors.Count; i++)
                {
                    bool temp = !stack.Contains(curr.Neighbors[i].EndingPoint);
                    if (!curr.Neighbors[i].EndingPoint.isVisited && temp)
                    {
                        stack.Add(curr.Neighbors[i].EndingPoint);
                        founders.Add(curr.Neighbors[i].EndingPoint, curr);
                    }
                    else if (!temp)
                    {
                        var current = curr.Neighbors[i].EndingPoint;
                        stack.Remove(curr.Neighbors[i].EndingPoint);
                        founders[curr.Neighbors[i].EndingPoint] = curr;
                        stack.Add(current);
                    }
                }
                if (stack.Count == 0)
                {
                    return 0;
                }

            }
            var previous = founders[ending];
            List<Vertex<T>> path = new List<Vertex<T>>();
            while (previous != null)
            {
                total += GetEdge(previous, founders[previous]).Distance;
                path.Add(previous);
                previous = founders[previous];
            }
            return total;
        }

        private double BreadthFirst(Point a, Point b)
        {
            Vertex<T> start = Search(a);
            Vertex<T> ending = Search(b);

            Queue<Vertex<T>> q = new Queue<Vertex<T>>();
            vertices.ForEach(a => a.isVisited = false);

            double total = 0;

            q.Enqueue(start);

            Vertex<T> curr = null;

            if (!Vertices.Contains(ending))
            {
                return 0;
            }
            while (q.Count > 0)
            {
                if (curr != null && GetEdge(curr, q.Peek()) != null)
                {
                    total += GetEdge(curr, q.Peek()).Distance;
                }
                curr = q.Dequeue();
                curr.isVisited = true;
                for (int i = 0; i < curr.Neighbors.Count; i++)
                {
                    if (!curr.Neighbors[i].EndingPoint.isVisited)
                    {
                        q.Enqueue(curr.Neighbors[i].EndingPoint);
                    }
                    if (curr == ending)
                    {
                        return total;
                    }
                }

            }
            return 0;
        }

        public bool whichSearchBetter(Point a, Point b)
        {
            return DepthFirst(a, b) < BreadthFirst(a, b);
        }
    }

   



}
