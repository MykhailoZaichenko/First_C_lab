//Варіант 9.
//Створити консольний застосунок, що моделює таку гру. На ігровому полі 
//розмістити три двопалубні кораблі. За задану кількість пострілів спробувати 
//знищити кораблі. 
//Розмір поля та кількість пострілів задаються користувачем. Кораблі 
//розміщуються випадковим чином. У разі промаху, позицію, по якій був 
//зроблений постріл, позначити символом '.'; у разі влучення – символом 'X'. 
//Гра закінчується після вичерпання усіх пострілів, або у разі знищення усіх 
//кораблів. Вивести повідомлення про результати гри

using System.Runtime.InteropServices;

class Program
{
    static int fieldSize;
    static int shots;
    static char[,] field;
    static int remainingShips = 3;
    static int[,] ships = new int[remainingShips, 4];


    static void PlaceShips()
    {
        Random rnd = new Random(1);
        for (int i = 0; i < remainingShips; i++)
        {
            bool placed_ships = false;
            while (!placed_ships)
            {
                int x = rnd.Next(fieldSize);
                int y = rnd.Next(fieldSize);
                bool horizontal = rnd.Next(2) == 0;
                //Перевірка чи можно розмістити корабель горизонтально
                if (horizontal && y + 1 < fieldSize && field[x, y] == '~' && field[x, y + 1] == '~')
                {
                    ships[i, 0] = x;
                    ships[i, 1] = y;
                    ships[i, 2] = x;
                    ships[i, 3] = y + 1;
                    //перевірка розміщення
                    field[x, y] = 'S';
                    field[x, y + 1] = 'S';
                    placed_ships = true;
                }
                //Перевірка чи можно розмістити корабель вертикально
                else if (!horizontal && x + 1 < fieldSize && field[x, y] == '~' && field[x + 1, y] == '~')
                {
                    ships[i, 0] = x;
                    ships[i, 1] = y;
                    ships[i, 2] = x + 1;
                    ships[i, 3] = y;
                    //перевірка розміщення
                    field[x, y] = 'S';
                    field[x + 1, y] = 'S';
                    placed_ships = true;
                }
            }
        }

    }

    static void PrintField()
    {
        Console.Write("  ");
        for (int i = 1; i <= fieldSize; i++)
        {
            Console.Write(i + " ");
        }
        Console.WriteLine();
        for (int i = 0; i < fieldSize; i++)
        {
            Console.Write((i + 1).ToString().PadLeft(2) + " "); // Виводить номер рядка починаючи з 1. А SPadLeft(2) робить ганішим вивод, тобто перетворює число в рядок, а потім додає перед числом пробіл, якщо воно однозначне, щоб усі номери рядків займали 2 символи.
            for (int j = 0; j < fieldSize; j++)
                Console.Write(field[i, j] + " "); // Виводить рядок поля
            Console.WriteLine();
        }
    }

    //Заповнюємо 'водою' поле
    static void FillWater()
    {
        field = new char[fieldSize, fieldSize];
        for (int i = 0; i < fieldSize; i++)
            for (int j = 0; j < fieldSize; j++)
                field[i, j] = '~';
    }

    //Пеевіряємо на влучання у корабель
    static void HitCheck(ref int input_x, ref int input_y)
    {
        bool hit = false;
        for (int i = 0; i < ships.GetLength(0); i++)
        {
            if ((ships[i, 0] == input_x && ships[i, 1] == input_y) || (ships[i, 2] == input_x && ships[i, 3] == input_y))
            {
                field[input_x, input_y] = 'X';

                // Позначаємо цю частину корабля як знищену
                if (ships[i, 0] == input_x && ships[i, 1] == input_y)
                {
                    ships[i, 0] = ships[i, 1] = -1;
                }
                else
                {
                    ships[i, 2] = ships[i, 3] = -1;
                }

                hit = true;

                // Якщо обидві частини корабля знищені, видаляємо його
                if (ships[i, 0] == -1 && ships[i, 1] == -1 && ships[i, 2] == -1 && ships[i, 3] == -1)
                {
                    Console.WriteLine("Корабель знищено!");
                    remainingShips--;
                }
                else
                {
                    Console.WriteLine("Влучили!");
                }
                break;
            }
        }

        if (!hit)
        {
        field[input_x, input_y] = '.';
        Console.WriteLine("Промах!");
        }
    }

    static void Main()
    {
        //Щоб вивод був українською
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        //Стартове меню
        Console.Write("Введіть розмір поля: ");
        while (!int.TryParse(Console.ReadLine(), out fieldSize) || fieldSize <= 2)
        {
            Console.WriteLine("Некоректне значення! Введіть ціле додатне число. Більше 2.");
            Console.Write("Введіть розмір поля: ");
        }

        Console.Write("Введіть кількість пострілів: ");
        while (!int.TryParse(Console.ReadLine(), out shots) || shots <= 0)
        {
            Console.WriteLine("Некоректне значення! Введіть ціле додатне число.");
            Console.Write("Введіть кількість пострілів: ");
        }

        //Заповнюємо "водою" поле
        FillWater();
        //Додаємо кораблі
        PlaceShips();

        //Основнйи геймплей
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
                    Console.WriteLine($"Некоректна координата X! Введіть значення від 1 до {fieldSize}.");
                    continue;
                }

                Console.Write("Введіть координату Y: ");
                if (!int.TryParse(Console.ReadLine(), out input_y) || input_y < 1 || input_y > fieldSize)
                {
                    Console.WriteLine($"Некоректна координата Y! Спробуйте ще раз.");
                    continue;
                }

                input_x--; input_y--;
                if (field[input_x, input_y] == 'X' || field[input_x, input_y] == '.')
                {
                    Console.WriteLine("Ви вже стріляли сюди!");
                    continue;
                }
                break;
            }

            //Перевірка на влучання у корабель
            HitCheck(ref input_x, ref input_y);

            shots--;
            Console.WriteLine("Натисніть Enter для продовження...");
            Console.ReadLine();
        }

        Console.Clear();
        PrintField();
        //Кінець гри
        Console.WriteLine(remainingShips == 0 ? "Вітаємо! Ви знищили всі кораблі!" : $"Поразка! Лишилось {remainingShips} кораблі.");
    }
}
