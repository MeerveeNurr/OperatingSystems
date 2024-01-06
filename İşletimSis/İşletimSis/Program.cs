using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        List<int> numbers = new List<int>();
        List<int> primeNumbers = new List<int>();
        List<int> evenNumbers = new List<int>();
        List<int> oddNumbers = new List<int>();

       
        for (int i = 1; i <= 1000000; i++)
        {
            numbers.Add(i);
        }

       
        Console.WriteLine("Sayılar: " + string.Join(", ", numbers));

       
        int chunkSize = numbers.Count / 4;
        List<List<int>> chunks = new List<List<int>>();
        for (int i = 0; i < 4; i++)
        {
            chunks.Add(numbers.GetRange(i * chunkSize, chunkSize));
        }

       
        List<Thread> threads = new List<Thread>();

        
        Thread mainThread = new Thread(() =>
        {
           
            for (int i = 0; i < 4; i++)
            {
                List<int> chunk = chunks[i];
                Thread thread = new Thread(() => ProcessNumbers(chunk, primeNumbers, evenNumbers, oddNumbers));
                threads.Add(thread);
                thread.Start();
            }

           
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("Toplam sayı adedi: " + numbers.Count);
            Console.WriteLine("Asal sayı adedi: " + primeNumbers.Count);
            Console.WriteLine("Çift sayı adedi: " + evenNumbers.Count);
            Console.WriteLine("Tek sayı adedi: " + oddNumbers.Count);
        });

        
        mainThread.Start();

        
        mainThread.Join();

        Console.ReadLine();
    }

    static void ProcessNumbers(List<int> numbers, List<int> primeNumbers, List<int> evenNumbers, List<int> oddNumbers)
    {
        foreach (int number in numbers)
        {
            if (IsPrime(number))
            {
                lock (primeNumbers)
                {
                    primeNumbers.Add(number);
                }
            }
            if (number % 2 == 0)
            {
                lock (evenNumbers)
                {
                    evenNumbers.Add(number);
                }
            }
            else
            {
                lock (oddNumbers)
                {
                    oddNumbers.Add(number);
                }
            }
        }
    }

    static bool IsPrime(int n)
    {
        if (n <= 1)
        {
            return false;
        }
        for (int i = 2; i <= Math.Sqrt(n); i++)
        {
            if (n % i == 0)
            {
                return false;
            }
        }
        return true;
    }
}
