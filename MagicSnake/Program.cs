using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

// Класс, представляющий точку на игровом поле
class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

// Класс, представляющий игровое поле
class GameBoard
{
    private int width;
    private int height;
    private List<Point> snake;
    private Point food;
    private int score;
    private bool gameOver;

    public GameBoard(int width, int height)
    {
        this.width = width;
        this.height = height;
        snake = new List<Point>();
        snake.Add(new Point(width / 2, height / 2));
        food = GenerateFood();
        score = 0;
        gameOver = false;
    }

    // Генерация случайного положения еды
    private Point GenerateFood()
    {
        Random random = new Random();
        int x = random.Next(0, width);
        int y = random.Next(0, height);
        return new Point(x, y);
    }

    // Отрисовка игрового поля
    public void Draw()
    {
        Console.Clear();
        Console.WriteLine("Score: " + score);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (snake.Any(p => p.X == x && p.Y == y))
                    Console.Write("O");
                else if (food.X == x && food.Y == y)
                    Console.Write("X");
                else
                    Console.Write(".");
            }
            Console.WriteLine();
        }
    }

    // Обработка нажатия клавиши
    public void HandleKeyPress(ConsoleKey key)
    {
        if (gameOver)
            return;

        Point head = snake[0];
        Point newHead = null;

        switch (key)
        {
            case ConsoleKey.LeftArrow:
                newHead = new Point(head.X - 1, head.Y);
                break;
            case ConsoleKey.RightArrow:
                newHead = new Point(head.X + 1, head.Y);
                break;
            case ConsoleKey.UpArrow:
                newHead = new Point(head.X, head.Y - 1);
                break;
            case ConsoleKey.DownArrow:
                newHead = new Point(head.X, head.Y + 1);
                break;
        }

        if (newHead == null || snake.Any(p => p.X == newHead.X && p.Y == newHead.Y) ||
            newHead.X < 0 || newHead.X >= width || newHead.Y < 0 || newHead.Y >= height)
        {
            gameOver = true;
            return;
        }

        snake.Insert(0, newHead);
        if (food.X == newHead.X && food.Y == newHead.Y)
        {
            score++;
            food = GenerateFood();
        }
        else
        {
            snake.RemoveAt(snake.Count - 1);
        }
    }

    // Запуск игры
    public void Run()
    {
        while (!gameOver)
        {
            Draw();
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            HandleKeyPress(keyInfo.Key);
            Thread.Sleep(100);
        }
        Console.WriteLine("Game Over! Your score: " + score);
    }
}

class Program
{
    static void Main()
    {
        GameBoard game = new GameBoard(20, 10);
        game.Run();
    }
}
