using System;
namespace Arr
{

    //1 TASK.
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        int n, m;
    //        Console.WriteLine("Введіть n - ");
    //        n = Convert.ToInt32(Console.ReadLine());
    //        Console.WriteLine("Введіть m - ");
    //        m = Convert.ToInt32(Console.ReadLine());
    //        int[,] matrix = new int[n, m];
    //        //заовнює матрицю рандомними числами
    //        Console.WriteLine($"Вивід матриці{n} на {m}");
    //        for (int i = 0; i < n; i++)
    //        {
    //            for (int j = 0; j < m; j++)
    //            {
    //                Random random = new Random();
    //                matrix[i, j] = random.Next(1, 10);
    //                Console.Write(matrix[i, j] + " ");
    //            }
    //            Console.WriteLine();
    //        }

    //        // Знайти найменший елемент в кожному рядку
    //        double[] minInRows = new double[matrix.GetLength(0)];
    //        for (int i = 0; i < matrix.GetLength(0); i++)
    //        {
    //            minInRows[i] = matrix[i, 0];
    //            for (int j = 1; j < matrix.GetLength(1); j++)
    //            {
    //                if (matrix[i, j] < minInRows[i])
    //                {
    //                    minInRows[i] = matrix[i, j];
    //                }
    //            }
    //        }

    //        // Знайти найбільший серед мінімальних елементів
    //        double maxOfMins = minInRows[0];
    //        int rowIndex = 0;
    //        for (int i = 1; i < minInRows.Length; i++)
    //        {
    //            if (minInRows[i] > maxOfMins)
    //            {
    //                maxOfMins = minInRows[i];
    //                rowIndex = i;
    //            }
    //        }
    //        Console.WriteLine("Найбільший елемент серед мінімальних елементів: " + maxOfMins);
    //        Console.WriteLine("Індекс рядка: " + rowIndex);
    //    }
    //}


    //2 TASK.
    //1. Створити структуру Точка.
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    //бстрактний базовий клас фігура  та похідні класи – 
    //квадрат(координати верхнього лівого кута та сторона) та 
    //коло(координати центру та радіус).
    //Передбачити необхідні конструктори, властивості, методи, реалізувати IComparable для порівняння за площею.
    abstract class Shape : IComparable<Shape>
    {
        public abstract double Area();

        public int CompareTo(Shape? other)
        {
            double curentArea = this.Area();
            double otherArea = other.Area();
            if (curentArea < otherArea) return -1;
            else if (curentArea > otherArea) return 1;
            else return 0;
        }
    }

    class Square : Shape 
    {
        public Point UpperLeftCorner { get; set; }
        public double Side { get; set; }

        public Square(Point upperLeftCorner, double side)
        {
            UpperLeftCorner = upperLeftCorner;
            Side = side;
        }

        public override double Area()
        {
            return Side * Side;
        }
    }

    class Colo : Shape 
    {
        public Point Center { get; set; }
        public double Radius { get; set; }

        public Colo(Point center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        public override double Area()
        {
            return Math.PI * Radius * Radius;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Зчитання інфи з файлу 
            string inputFilePath = "input.txt";
            string[] lines = File.ReadAllLines(inputFilePath);
            Shape[] shapes = new Shape[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(' ');
                if (parts[0] == "S")
                {
                    Point upperLeftCorner = new Point(int.Parse(parts[1]), int.Parse(parts[2]));
                    double side = double.Parse(parts[3]);
                    shapes[i] = new Square(upperLeftCorner, side);
                }
                else if (parts[0] == "C")
                {
                    Point center = new Point(int.Parse(parts[1]), int.Parse(parts[2]));
                    double radius = double.Parse(parts[3]);
                    shapes[i] = new Colo(center, radius);
                }
            }

            Array.Sort(shapes);

            Console.WriteLine("Фігури в порядку зростання їх площі:");
            foreach (Shape shape in shapes)
            {
                Console.WriteLine($"Площа: {shape.Area()}");
            }
           // Обчислення сумарної площі всіх кругів, які не перетинають осі координат:
        }
    }
}
