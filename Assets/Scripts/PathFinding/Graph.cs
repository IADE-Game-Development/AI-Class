using System;
using UnityEngine;
using System.Collections.Generic;

public class Graph : MonoBehaviour
{
    public GraphNode[] Nodes;

    private float Heuristic(GraphNode a, GraphNode b)
        => Vector3.Distance(a.transform.position, b.transform.position);

    private List<GraphNode> ReconstructPath(Dictionary<GraphNode, GraphNode> cameFrom, GraphNode current,
        GraphNode start)
    {
        List<GraphNode> totalPath = new List<GraphNode>();
        totalPath.Add(current);
        while (current != start)
        {
            if (!cameFrom.TryGetValue(current, out GraphNode previous))
                return new List<GraphNode>();

            current = previous;
            totalPath.Add(current);
        }

        totalPath.Reverse();
        return totalPath;
    }

    public List<GraphNode> FindPath(GraphNode start, GraphNode goal)
    {
        if (start == null || goal == null)
            return new List<GraphNode>();

        HashSet<GraphNode> closedNodes = new HashSet<GraphNode>();
        PriorityQueue<GraphNode> openNodes = new PriorityQueue<GraphNode>();

        openNodes.Enqueue(start, Heuristic(start, goal));

        Dictionary<GraphNode, GraphNode> cameFrom = new Dictionary<GraphNode, GraphNode>();
        Dictionary<GraphNode, float> gScore = new Dictionary<GraphNode, float>();
        gScore[start] = 0;

        while (openNodes.Count > 0)
        {
            GraphNode current = openNodes.Dequeue();
            if (!closedNodes.Add(current))
                continue;

            if (current == goal)
                return ReconstructPath(cameFrom, current, start);

            if (current.edges == null)
                continue;

            foreach (GraphNode neighbor in current.edges)
            {
                if (neighbor == null || closedNodes.Contains(neighbor))
                    continue;

                float tentativeGScore = gScore[current] + Heuristic(current, neighbor);

                if (gScore.TryGetValue(neighbor, out float existingGScore) && tentativeGScore >= existingGScore)
                    continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;

                float priority = tentativeGScore + Heuristic(neighbor, goal);
                if (openNodes.Contains(neighbor))
                    openNodes.UpdatePriority(neighbor, priority);
                else
                    openNodes.Enqueue(neighbor, priority);
            }
        }

        return new List<GraphNode>();
    }

    [ContextMenu("Find All Nodes")]
    public void FindAllNodes()
    {
        Debug.Log("Finding all nodes...");
        Nodes = FindObjectsByType<GraphNode>(FindObjectsSortMode.None);

        // Mark as dirty
        UnityEditor.EditorUtility.SetDirty(this);
    }

    [ContextMenu("Check Graph")]
    public void CheckGraph()
    {
        // Check if all nodes are connected both-ways
    }
}