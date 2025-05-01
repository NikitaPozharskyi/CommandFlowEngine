namespace CommandFlowEngine.Extensions;

public static class EnumerableExtensions
{
    public static Queue<T> ToQueue<T>(this IEnumerable<T> enumerable)
    {
        var queue = new Queue<T>();
        foreach (var workflow in enumerable)
        {
            queue.Enqueue(workflow);
        }

        return queue;
    }
}