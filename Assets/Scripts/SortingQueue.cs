using System.Collections;
using System.Collections.Generic;

public class SortingQueue<T> where T:class
{
    private List<T> nodes = new List<T>();

    public int Count
    {
        get
        {
            return this.nodes.Count;
        }
    }

    public bool Contains(T node)
    {
        return this.nodes.Contains(node);
    }

    public T Dequeue()
    {
        if (nodes.Count > 0)
        {
            T node = this.nodes[0];
            this.nodes.RemoveAt(0);

            return node;
        }
        return null;
    }

    public void Enqueue(T node)
    {
        this.nodes.Add(node);
        this.nodes.Sort();
    }

    //private void ShowOpenQueue()
    //{
    //    string resultStr = "";
    //    foreach (T node in this.nodes)
    //    {
    //        resultStr += node.ToString();
    //    }
    //    resultStr += "\n";
    //}
}
