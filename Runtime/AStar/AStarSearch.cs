//----------------------------------------
// MIT License
// Copyright(c) 2021 Jonas Boetel
//----------------------------------------
using System.Collections.Generic;
using Lumpn.Pathfinding.AStar;

namespace Lumpn.Pathfinding
{
    public sealed class AStarSearch : ISearch
    {
        private static readonly NodeComparer comparer = new NodeComparer();

        public delegate float Heuristic(IGraph graph, int startId, int destinationId);

        private readonly Heuristic heuristic;

        public AStarSearch(Heuristic heuristic)
        {
            this.heuristic = heuristic;
        }

        public Path Search(IGraph graph, int startId, int destinationId)
        {
            var nodeCount = graph.nodeCount;
            var explored = new bool[nodeCount];
            var parents = new int[nodeCount];

            var queue = new Heap<Node>(comparer, graph.nodeCount);

            queue.Push(new Node(startId, -1, 0f, heuristic(graph, startId, destinationId)));

            while (queue.Count > 0)
            {
                var entry = queue.Pop();
                var nodeId = entry.id;

                if (explored[nodeId]) continue;
                explored[nodeId] = true;
                parents[nodeId] = entry.parentId;

                if (nodeId == destinationId)
                {
                    var path = ReconstructPath(nodeId, entry.cost, parents);
                    return path;
                }

                foreach (var edge in graph.GetEdges(nodeId))
                {
                    var targetId = edge.targetNodeId;
                    if (!explored[targetId])
                    {
                        var cost = entry.cost + edge.cost;
                        queue.Push(new Node(targetId, nodeId, cost, heuristic(graph, targetId, destinationId)));
                    }
                }
            }

            return Path.invalid;
        }

        private static Path ReconstructPath(int lastId, float cost, int[] parents)
        {
            var nodes = new List<int>();
            for (var id = lastId; id >= 0; id = parents[id])
            {
                nodes.Add(id);
            }
            nodes.Reverse();

            return new Path(cost, nodes);
        }
    }
}
