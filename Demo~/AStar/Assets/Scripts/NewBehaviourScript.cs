using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
        var n1 = new Node("Start");
        var n2 = new Node("End");

        var graph = new Graph();
        var idx1 = graph.AddNode("Start");
        var idx2 = graph.AddNode("End");
        graph.AddEdge(idx1, idx2, 5f);

        var path = Search(graph, idx1, idx2, heuristic);
        Debug.LogFormat("Path length {0}, cost {1}", path.length, path.cost);

        foreach (var node in path)
        {
            Debug.LogFormat("Node {0}", node.position);
        }
    }

    private static object SearchDijkstra(IGraph graph, int idx1, int idx2)
    {
        var searchNodes = graph.GetNodes().Select(p => new SearchNode()).ToArray();

        var frontier = new PriorityQueue<int>();
        frontier.Enqueue(idx1);
        searchNodes[idx1].explored = true;

        while (frontier.Count > 0)
        {
            var idx = frontier.Dequeue();
            if (idx == idx2)
            {
                return true; // TODO Jonas: reconstruct path
            }

            var node = searchNodes[idx];
            node.explored = true;

            foreach (var edge in graph.GetEdges(idx))
            {
                var dest = edge.idx;
                var destNode = searchNodes[dest];
                if (!destNode.explored)
                {
                    var cost = node.cost + edge.cost;
                    frontier.Enqueue(dest);
                }
            }
        }
    }

    private static object SearchAStar(IGraph graph, int idx1, int idx2, System.Func<IGraph, int, int, float> heuristic)
    {
        var searchNodes = graph.GetNodes().Select(p => new SearchNode()).ToArray();

        var openList = new PriorityQueue<int>();
        openList.Enqueue(idx1);

        while (openList.Count > 0)
        {
            var idx = openList.Dequeue();
            if (idx == idx2)
            {
                return new object(); // TODO Jonas: reconstruct path;
            }

            var current = searchNodes[idx];
            current.closed = true;

            var edge = graph.GetEdges(idx);
            foreach (var edge in edges)
            {
                var tentativeScore = current.g_score + edge.cost;
                var dest = edge.index;
                var destNode = searchNodes[dest];
                if (tentativeScore < destNode.g_score)
                {
                    destNode.prev = current;
                    destNode.g_score = tentativeScore;
                    destNode.f_score = destNode.g_score + heuristic(graph, dest, idx2);
                    if (explored.Add(dest))
                    {
                        openList.Enqueue(destNode);
                    }
                }
            }
        }

        return null;
    }
}
