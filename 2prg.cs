using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//2. Модифицировать программу нахождения минимума функции так, чтобы можно было передавать функцию в виде делегата. 
//а) Сделать меню с различными функциями и представить пользователю выбор, для какой функции и на каком отрезке находить минимум.
namespace _6_Homework2
{
    class Count
    {
        delegate double Equation(double x);

        static double Quadratic_equation(double x)
        {
            return x * x - 4 * x + 2;
        }
        static double Linear_equation(double x)
        {
            return x + 4;
        }
        static double Absolute_equation(double x)
        {
            return Math.Abs(x + 9);
        }

        static void SaveFunc(string fileName, double min, double max, double step, Equation select_equation)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            double x = min;

            while (x <= max)
            {
                bw.Write(select_equation(x));
                x += step;
            }

            bw.Close();
            fs.Close();
        }

        static double Load(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader bw = new BinaryReader(fs);

            double min = double.MaxValue;
            double d;

            for (int i = 0; i < fs.Length / sizeof(double); i++)
            {
                d = bw.ReadDouble();
                if (d < min) min = d;
            }

            bw.Close();
            fs.Close();
            return min;
        }

        public static void CountMain()
        {
            int select;
            Equation select_equation;

            try
            {
                Console.WriteLine("Выберите цифру функции для расчета минимума:");
                Console.WriteLine("1. Квадратное уравнение x^2 - 4*x + 2");
                Console.WriteLine("2. Линейное уравнение x + 4");
                Console.WriteLine("3. Абсолютное уравнение |x + 9|");
                Console.WriteLine();

                switch (select = Int32.Parse(Console.ReadLine()))
                {
                    case 1: select_equation = Quadratic_equation; break;
                    case 2: select_equation = Linear_equation; break;
                    case 3: select_equation = Absolute_equation; break;

                    default: select_equation = Quadratic_equation; break;
                }

                Console.WriteLine();
                Console.WriteLine("Выберите отрезок.");
                Console.Write("Минимум: ");
                int min = Int32.Parse(Console.ReadLine());
                Console.Write("Максимум: ");
                int max = Int32.Parse(Console.ReadLine());

                SaveFunc("data.bin", min, max, 0.5, select_equation);
                Console.WriteLine("\n" + Load("data.bin"));
                Console.ReadKey();
            }
            catch
            {
                Console.WriteLine("Только цифры с целыми значениями");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Count.CountMain();
        }
    }
}
