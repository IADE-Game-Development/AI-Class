using System.Collections.Generic;

public class PriorityQueue<T>
{
    private readonly List<(T Item, float Priority)> elements = new List<(T Item, float Priority)>();

    public int Count => elements.Count;

    public void Enqueue(T item, float priority)
    {
        int index = elements.FindIndex(x => EqualityComparer<T>.Default.Equals(x.Item, item));
        if (index >= 0)
            elements[index] = (item, priority);
        else
            elements.Add((item, priority));

        elements.Sort((x, y) => x.Priority.CompareTo(y.Priority));
    }

    public T Dequeue()
    {
        if (elements.Count == 0)
            throw new System.InvalidOperationException("The queue is empty.");

        T item = elements[0].Item;
        elements.RemoveAt(0);
        return item;
    }

    public bool Contains(T item)
    {
        return elements.Exists(x => EqualityComparer<T>.Default.Equals(x.Item, item));
    }

    public void UpdatePriority(T item, float newPriority)
    {
        Enqueue(item, newPriority);
    }
}
