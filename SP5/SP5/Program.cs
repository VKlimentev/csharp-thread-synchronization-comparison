using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static int sharedVariable = 0;

    static Mutex mutex = new Mutex();
    static Semaphore semaphore = new Semaphore(1, 1);
    static AutoResetEvent autoResetEvent = new AutoResetEvent(true);

    static void Main()
    {
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();
        Thread thread1 = new Thread(IncrementWithMutex);
        Thread thread2 = new Thread(IncrementWithMutex);

        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();

        stopwatch.Stop();
        Console.WriteLine($"Мьютекс\t Значение переменной: {sharedVariable} Время:" + stopwatch.Elapsed);

        sharedVariable = 0;

        stopwatch.Restart();
        thread1 = new Thread(IncrementWithSemaphore);
        thread2 = new Thread(IncrementWithSemaphore);

        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();

        stopwatch.Stop();
        Console.WriteLine($"Семафор\t Значение переменной: {sharedVariable} Время:" + stopwatch.Elapsed);

        sharedVariable = 0;

        stopwatch.Restart();
        thread1 = new Thread(IncrementWithAutoResetEvent);
        thread2 = new Thread(IncrementWithAutoResetEvent);

        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();

        stopwatch.Stop();
        Console.WriteLine($"Усл.перем\t Значение переменной: {sharedVariable} Время:" + stopwatch.Elapsed);

        sharedVariable = 0;

        stopwatch.Restart();
        thread1 = new Thread(IncrementWithoutSynchronization);
        thread2 = new Thread(IncrementWithoutSynchronization);

        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();

        stopwatch.Stop();
        Console.WriteLine($"Без синх\t Значение переменной: {sharedVariable} Время:" + stopwatch.Elapsed);

        sharedVariable = 0;

        stopwatch.Restart();
        IncrementWithoutSynchronization();
        IncrementWithoutSynchronization();
        stopwatch.Stop();
        Console.WriteLine($"Последовательный\t Значение переменной: {sharedVariable} Время:" + stopwatch.Elapsed);


    }
    static void IncrementWithMutex()
    {
        for (int i = 0; i < 1000000; i++)
        {
            mutex.WaitOne();
            sharedVariable++;
            mutex.ReleaseMutex();
        }
    }

    static void IncrementWithSemaphore()
    {
        for (int i = 0; i < 1000000; i++)
        {
            semaphore.WaitOne();
            sharedVariable++;
            semaphore.Release();
        }
    }

    static void IncrementWithAutoResetEvent()
    {
        for (int i = 0; i < 1000000; i++)
        {
            autoResetEvent.WaitOne();
            sharedVariable++;
            autoResetEvent.Set();
        }
    }

    static void IncrementWithoutSynchronization()
    {
        for (int i = 0; i < 1000000; i++)
        {
            sharedVariable++;
        }
    }
}