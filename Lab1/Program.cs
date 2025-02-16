//Варіант 9.
//Створити консольний застосунок, що моделює таку гру. На ігровому полі 
//розмістити три двопалубні кораблі. За задану кількість пострілів спробувати 
//знищити кораблі. 
//Розмір поля та кількість пострілів задаються користувачем. Кораблі 
//розміщуються випадковим чином. У разі промаху, позицію, по якій був 
//зроблений постріл, позначити символом '.'; у разі влучення – символом 'X'. 
//Гра закінчується після вичерпання усіх пострілів, або у разі знищення усіх 
//кораблів. Вивести повідомлення про результати гри


//using System;

class Program
{
    static int fieldSize;
    static int shots;
    static char[,] field;
    static (int, int)[][] ships = new (int, int)[3][];
    static int remainingShips = 3;

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.Write("Введіть розмір поля: ");
        while (!int.TryParse(Console.ReadLine(), out fieldSize) || fieldSize <= 0)
        {
            Console.WriteLine("Некоректне значення! Введіть ціле додатне число.");
            Console.Write("Введіть розмір поля: ");
        }

        Console.Write("Введіть кількість пострілів: ");
        while (!int.TryParse(Console.ReadLine(), out shots) || shots <= 0)
        {
            Console.WriteLine("Некоректне значення! Введіть ціле додатне число.");
            Console.Write("Введіть кількість пострілів: ");
        }

        field = new char[fieldSize, fieldSize];
        for (int i = 0; i < fieldSize; i++)
            for (int j = 0; j < fieldSize; j++)
                field[i, j] = '~';

        PlaceShips();

        while (shots > 0 && remainingShips > 0)
        {
            Console.Clear();
            PrintField();

            int input_x, input_y;
            while (true)
            {
                Console.Write("Введіть координату X: ");
                if (!int.TryParse(Console.ReadLine(), out input_x) || input_x < 1 || input_x > fieldSize)
                {
                    Console.WriteLine("Некоректна координата X! Спробуйте ще раз.");
                    continue;
                }

                Console.Write("Введіть координату Y: ");
                if (!int.TryParse(Console.ReadLine(), out input_y) || input_y < 1 || input_y > fieldSize)
                {
                    Console.WriteLine("Некоректна координата Y! Спробуйте ще раз.");
                    continue;
                }

                input_x--; input_y--;
                if (field[input_x, input_y] == 'X' || field[input_x, input_y] == '.')
                {
                    Console.WriteLine("Ви вже стріляли сюди! Спробуйте інше місце.");
                    continue;
                }
                break;
            }

            bool hit = false;
            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i] != null)
                {
                    for (int j = 0; j < ships[i].Length; j++)
                    {
                        if (ships[i][j] == (input_x, input_y))
                        {
                            field[input_x, input_y] = 'X';
                            ships[i][j] = (-1, -1);
                            hit = true;
                            if (Array.TrueForAll(ships[i], pos => pos == (-1, -1)))
                            {
                                Console.WriteLine("Корабель знищено!");
                                ships[i] = null;
                                remainingShips--;
                            }
                            break;
                        }
                    }
                }
            }

            if (!hit)
            {
                field[input_x, input_y] = '.';
                Console.WriteLine("Промах!");
            }

            shots--;
            Console.WriteLine("Натисніть Enter для продовження...");
            Console.ReadLine();
        }

        Console.Clear();
        PrintField();
        Console.WriteLine(remainingShips == 0 ? "Вітаємо! Ви знищили всі кораблі!" : "Гру завершено! Ви не знищили всі кораблі.");
    }

    static void PlaceShips()
    {
        Random rand = new Random();
        for (int i = 0; i < 3; i++)
        {
            bool placed = false;
            while (!placed)
            {
                int x = rand.Next(fieldSize);
                int y = rand.Next(fieldSize);
                bool horizontal = rand.Next(2) == 0;

                if (horizontal && y + 1 < fieldSize && field[x, y] == '~' && field[x, y + 1] == '~')
                {
                    ships[i] = new (int, int)[] { (x, y), (x, y + 1) };
                    field[x, y] = field[x, y + 1] = 'S';
                    placed = true;
                }
                else if (!horizontal && x + 1 < fieldSize && field[x, y] == '~' && field[x + 1, y] == '~')
                {
                    ships[i] = new (int, int)[] { (x, y), (x + 1, y) };
                    field[x, y] = field[x + 1, y] = 'S';
                    placed = true;
                }
            }
        }

        for (int i = 0; i < fieldSize; i++)
            for (int j = 0; j < fieldSize; j++)
                if (field[i, j] == 'S') field[i, j] = '~';
    }

    static void PrintField()
    {
        Console.WriteLine("  " + string.Join(" ", Enumerable.Range(1, fieldSize)));
        for (int i = 0; i < fieldSize; i++)
        {
            Console.Write((i + 1).ToString().PadLeft(2) + " ");
            for (int j = 0; j < fieldSize; j++)
                Console.Write(field[i, j] + " ");
            Console.WriteLine();
        }
    }
}
