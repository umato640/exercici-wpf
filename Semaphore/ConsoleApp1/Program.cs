using System;
using System.Threading;

namespace MultiThreadingExample
{
    class Program
    {
        static int increment = 1;
        static Semaphore semaphore = new Semaphore(2, 2);

        static void Main(string[] args)
        {
            Console.WriteLine("Introdueix el nombre de fils a executar:");
            int nThreads = int.Parse(Console.ReadLine());

            ProcessSync(nThreads);

            Console.WriteLine($"Valor final de la variable 'increment': {increment}");
            Console.WriteLine("Pressiona qualsevol tecla per sortir...");
            Console.ReadKey();
        }

        /// <summary>
        /// Funció principal que executa el procés.
        /// </summary>
        /// <param name="nThreads">Nombre de fils a generar.</param>
        static void ProcessSync(int nThreads)
        {
            Thread[] threads = new Thread[nThreads];

            for (int i = 0; i < nThreads; i++)
            {
                int threadNumber = i + 1;
                threads[i] = new Thread(() => ThreadProcess(threadNumber));
                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join(); // Espera fins que tots els fils hagin acabat
            }
        }

        /// <summary>
        /// Funció que defineix el procés a realitzar per cada fil.
        /// </summary>
        /// <param name="threadNumber">Número de fil.</param>
        static void ThreadProcess(int threadNumber)
        {
            Random random = new Random();

            for (int i = 1; i <= threadNumber; i++)
            {
                int randomSeconds = random.Next(1, 6);
                Console.WriteLine($"Soc el fil número {threadNumber}, estic al pas {i} i m'espero {randomSeconds} segons");

                Thread.Sleep(randomSeconds * 1000);

                semaphore.WaitOne(); // Entrar en la secció crítica

                Console.WriteLine($"Incrementant variable per fil número {threadNumber}");
                increment++;

                semaphore.Release(); // Sortir de la secció crítica

                Thread.Sleep(randomSeconds * 1000);
            }

            Console.WriteLine($"Fi del fil número {threadNumber}");
        }
    }
}