using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prod_Consum
{
    //El siguiente código Produce una cantidad n de Procesos en un arreglo y son consumidos posteriormente a su producción
    class Program
    {
        static int buffSize;
        static int valuesToProduce;
        static string[] buffer;
        private static Semaphore isFull;
        private static Semaphore isEmpty;
        private static int mutex;
        static string value;
        static void Main(string[] args)
        {
            Console.WriteLine("Ingrese Cantidad a Producir: ");
            validate(valuesToProduce);
            while (Convert.ToInt32(value) == 0)
            {
                Console.WriteLine("El tamaño a producir no puede ser 0.");
                validate(valuesToProduce);
            }

            valuesToProduce = Convert.ToInt32(value);

            Console.WriteLine("Ingrese Tamaño del Buffer: ");
            validate(buffSize);
            while (Convert.ToInt32(value) == 0)
            {
                Console.WriteLine("El tamaño del Buffer no puede ser 0.");
                validate(buffSize);
            }

            buffSize = Convert.ToInt32(value); 
            buffer = new string[buffSize];
            isFull = new Semaphore(buffSize, buffSize);
            isEmpty = new Semaphore(0, buffSize);
            Thread p = new Thread(new ThreadStart(Program.produce));
            Thread c = new Thread(new ThreadStart(Program.consume));

            p.Start();
            c.Start();

            Console.ReadKey();
        }

        static void validate(int entero)
        {
            while (!Int32.TryParse(value = Console.ReadLine(), out entero))
            {
                Console.WriteLine("Ingrese Valor Correcto.");
            }
        }

        
        static void produce()
        {
            for (int i = 0; i < valuesToProduce; i++)
            {
                isFull.WaitOne();
                mutex = 1;
                buffer[i % buffSize] = (string)("Proceso "+(i+1).ToString());
                Console.WriteLine("Proceso Producido: {0}", buffer[i % buffSize]);
                    isEmpty.Release(1);
                    mutex = 0;
            }
        }

        static void consume()
        {
            for (int i = 0; i < valuesToProduce; i++)
            {   
                isEmpty.WaitOne();
                mutex = 1;
                string c = buffer[i % buffSize];
                Console.WriteLine("Proceso Consumido: {0}", c);
                    isFull.Release(1);
                    mutex = 0;
            }
        }
    }
}
