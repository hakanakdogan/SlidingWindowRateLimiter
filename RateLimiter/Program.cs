using System;
using System.Collections.Generic;
using System.Threading;

public class RateLimiter
{
    private Queue<DateTime> _queue;
    private int _capacity;
    private TimeSpan _slidingWindow;

    public RateLimiter(int capacity, TimeSpan slidingWindow)
    {
        _queue = new Queue<DateTime>();
        _capacity = capacity;
        _slidingWindow = slidingWindow;
    }

    public bool GrantAccess()
    {
        if (_queue.Count < _capacity)
        {
            _queue.Enqueue(DateTime.Now);
            return true;
        }

        var oldestAccessTime = _queue.Peek();
        var passedTime = DateTime.Now - oldestAccessTime;

        if (passedTime >= _slidingWindow)
        {
            _queue.Dequeue();
            _queue.Enqueue(DateTime.Now);
            return true;
        }

        return false;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        RateLimiter rateLimiter = new RateLimiter(5, TimeSpan.FromSeconds(7));

        for (int i = 0; i < 100; i++)
        {
            Thread.Sleep(500);
            bool isAccessGranted = rateLimiter.GrantAccess();
            if (isAccessGranted)
            {
                Console.WriteLine($"Request coming from {i + 1}. instance is ALLOWED");
            }
            else {
                Console.WriteLine($"Request coming from {i + 1}. instance is NOT ALLOWED");
            }
            
        }
    }
}
