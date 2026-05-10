using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static Queue<int> numeri = new Queue<int>();
    static object lockCoda = new object();
    static bool fine = false;

    static void Main()
    {
        Thread produttore = new Thread(Produttore);
        Thread consumatore = new Thread(Consumatore);

        produttore.Start();
        consumatore.Start();

        produttore.Join();
        consumatore.Join();

        Console.WriteLine("Programma terminato.");
    }

    static void Produttore()
    {
        Random rnd = new Random();
        while (true)
        {
            int numero = rnd.Next(0, 11);
            lock (lockCoda)
            {
                numeri.Enqueue(numero);
                Console.WriteLine($"Prodotto: {numero}");
                Monitor.Pulse(lockCoda);
                if (numero == 0)
                {
                    fine = true;
                    return;
                }
            }
            Thread.Sleep(100);  
        }
    }

    static void Consumatore()
    {
        while (true)
        {
            int numero;
            lock (lockCoda)
            {
                while (numeri.Count == 0)
                {
                    Monitor.Wait(lockCoda);
                }
                numero = numeri.Dequeue();
            }
            Console.WriteLine($"Consumato: {numero}");
            Thread.Sleep(1000);
            if (numero == 0)
            {
                return;
            }
        }
    }
}