using System;
using System.Linq;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    public GraphNode[] edges;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (GraphNode edge in edges)
        {
            Gizmos.DrawLine(edge.transform.position, transform.position);
        }
    }
}