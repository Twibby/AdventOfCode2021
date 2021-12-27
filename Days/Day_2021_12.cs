using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2021_12 : DayScript2021
{
    protected override string part_1()
    {
        Graph caveGraph = new Graph(_input);

        return caveGraph.GetAllPaths(true).Count.ToString();
    }

    protected override string part_2()
    {
        Graph caveGraph = new Graph(_input);

        List < List < Node >> paths = caveGraph.GetAllPaths(false);
        //foreach (List<Node> path in paths)
        //{
        //    Debug.Log(System.String.Join("-", path));
        //}
        return paths.Count.ToString();
    }


    class Graph
    {
        public List<Node> nodes;
        public List<Edge> edges;

        public Graph(List<Node> pNodes, List<Edge> pEdges)
        {
            this.nodes = pNodes;
            this.edges = pEdges;
        }

        public Graph(string input)
        {
            this.nodes = new List<Node>();
            this.edges = new List<Edge>();

            foreach (string edge in input.Split('\n'))
            {
                string[] bounds = edge.Split('-');
                if (!nodes.Exists(x => x.name == bounds[0]))
                    nodes.Add(new Node() { name = bounds[0] });
                Node startNode = nodes.Find(x => x.name == bounds[0]);

                if (!nodes.Exists(x => x.name == bounds[1]))
                    nodes.Add(new Node() { name = bounds[1] });
                Node endNode = nodes.Find(x => x.name == bounds[1]);

                edges.Add(new Edge() { start = startNode, end = endNode });
            }
        }

        public List<Node> GetConnectedNode(Node startingPoint)
        {
            List<Node> result = new List<Node>();
            foreach (var edge in edges)
            {
                if (edge.start.name == startingPoint.name)
                    result.Add(edge.end);
                else if (edge.end.name == startingPoint.name)
                    result.Add(edge.start);
            }
            return result;
        }

        public List<List<Node>> GetAllPaths(bool isPart1)
        {
            Node startNode = this.nodes.Find(x => x.name == "start");
            if (isPart1)
                return GetPathsFromNode(startNode, new List<Node>());
            else
            {
                return GetPathsFromNodeP2(startNode, new List<Node>());
            }
        }

        public List<List<Node>> GetPathsFromNode(Node node, List<Node> passedNodes)
        {
            List<List<Node>> result = new List<List<Node>>();
            List<Node> passedNodes2 = new List<Node>(passedNodes);
            passedNodes2.Add(node);

            foreach (Node n in this.GetConnectedNode(node))
            {
                if (passedNodes.Contains(n) && !n.isBigRoom)
                    continue;

                if (n.name == "end")
                {
                    result.Add(passedNodes2);
                    continue;
                }

                result.AddRange(GetPathsFromNode(n, passedNodes2));
            }

            return result;
        }

        public List<List<Node>> GetPathsFromNodeP2(Node node, List<Node> passedNodes, int offset = 0)
        {
            List<List<Node>> result = new List<List<Node>>();
            Node? smallTwice = getDoubleSmallNode(passedNodes, node);           

            foreach (Node n in this.GetConnectedNode(node))
            {
                List<Node> passedNodes2 = new List<Node>(passedNodes);
                passedNodes2.Add(node);

                if (n.name == "start")
                    continue;

                if (n.name == "end")
                {
                    passedNodes2.Add(n);
                    result.Add(passedNodes2);
                    continue;
                }

                if (!n.isBigRoom)
                {
                    if (smallTwice != null && passedNodes.Contains(n))
                    {
                        continue;
                    }
                }

                result.AddRange(GetPathsFromNodeP2(n, passedNodes2, offset+1));
            }

            return result;
        }

        public Node? getDoubleSmallNode(List<Node> nodes, Node currentNode)
        {
            List<Node> tmp = new List<Node>(nodes);
            tmp.Add(currentNode);
            for (int i = 0; i < tmp.Count; i++)
            {
                if (tmp[i].isBigRoom)
                    continue;

                if (tmp.FindAll(x => x.name == tmp[i].name).Count == 2)
                    return tmp[i];
            }
            return null;
        }

    }

    struct Node
    {
        public string name;
        public bool isBigRoom { get { return name.ToUpper() == name; } }

        public override string ToString() {return name; }
    }

    struct Edge
    {
        public Node start;
        public Node end;
        public override string ToString()
        {
            return start.ToString() + " - " + end.ToString();
        }
    }
}
