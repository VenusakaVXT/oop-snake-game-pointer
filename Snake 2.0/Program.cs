using System;
namespace gameSnake;
class SnakeException : Exception
{
    public SnakeException(string error) : base(error) { }
}
class Frame
{
    private int width;
    private int height;

    public Frame()
    {
        width = 50;
        height = 25;
        Console.CursorVisible = false;
    }

    public Frame(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public int Width { get { return width; } set { width = value; } }
    public int Height { get { return height; } set { height = value; } }

    public void displayFrame()
    {
        Console.Clear();

        // Set cursor
        for (int i = 0; i < width; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("#");
        }

        for (int i = 0; i < width; i++)
        {
            Console.SetCursorPosition(i, height);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("#");
        }

        for (int i = 0; i < height; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("#");
        }

        for (int i = 0; i < height; i++)
        {
            Console.SetCursorPosition(width, i);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("#");
        }
    }
}
class Snake
{
    private int firstValue;
    private int lastValue;
    List<Body> snakeBody;

    public Snake()
    {
        firstValue = 20;
        lastValue = 20;
        snakeBody = new List<Body>();
        snakeBody.Add(new Body(firstValue, lastValue));
    }

    public Snake(int firstValue, int lastValue)
    {
        this.firstValue = firstValue;
        this.lastValue = lastValue;
    }

    public int FirstValue { get { return firstValue; } set { firstValue = value; } }
    public int LastValue { get { return lastValue; } set { lastValue = value; } }

    public void displaySnake()
    {
        foreach (Body venus in snakeBody)
        {
            Console.SetCursorPosition(venus.HeadSnake, venus.TailSnake);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("@");
        }
    }

    ConsoleKeyInfo buttonInfo = new ConsoleKeyInfo();
    char buttonLocation = 'w';
    char cmd = 'u';

    public void fflushStdin()
    {
        if (Console.KeyAvailable)
        {
            buttonInfo = Console.ReadKey(true);
            buttonLocation = buttonInfo.KeyChar;
        }
    }

    private void Direction()
    {
        if (buttonLocation == 'w' && cmd != 'd')
        {
            cmd = 'u';
        }
        else if (buttonLocation == 's' && cmd != 'u')
        {
            cmd = 'd';
        }
        else if (buttonLocation == 'a' && cmd != 'r')
        {
            cmd = 'l';
        }
        else if (buttonLocation == 'd' && cmd != 'l')
        {
            cmd = 'r';
        }
    }
    public void moveSnake()
    {
        Direction();

        if (cmd == 'u')
        {
            lastValue--;
        }
        else if (cmd == 'd')
        {
            lastValue++;
        }
        else if (cmd == 'l')
        {
            firstValue--;
        }
        else if (cmd == 'r')
        {
            firstValue++;
        }

        snakeBody.Add(new Body(firstValue, lastValue));
        snakeBody.RemoveAt(0);
        Thread.Sleep(100);
    }

    public void eatFood(Body level, Food food)
    {
        Body growSnake = snakeBody[snakeBody.Count - 1];
        if (growSnake.HeadSnake == level.HeadSnake && growSnake.TailSnake == level.TailSnake)
        {
            snakeBody.Add(new Body(firstValue, lastValue));
            food.newlocationFood();
        }
    }

    public void isDead()
    {
        Body head = snakeBody[snakeBody.Count - 1];
        for (int i = 0; i < snakeBody.Count - 2; i++)
        {
            if (head.HeadSnake == snakeBody[i].HeadSnake && head.TailSnake == snakeBody[i].TailSnake)
            {
                Console.ResetColor();
                throw (new SnakeException("Game Over"));
            }
        }
    }

    public void hitWall(Frame wall)
    {
        Body head = snakeBody[snakeBody.Count - 1];
        if (head.HeadSnake >= wall.Width || head.HeadSnake <= 0 || head.TailSnake >= wall.Height || head.TailSnake <= 0)
        {
            Console.ResetColor();
            throw (new SnakeException("Game Over"));
        }
    }
}
class Body
{
    private int headSnake;
    private int tailSnake;

    public Body() { }
    public Body(int headSnake, int tailSnake)
    {
        this.headSnake = headSnake;
        this.tailSnake = tailSnake;
    }

    public int HeadSnake { get { return headSnake; } set { headSnake = value; } }
    public int TailSnake { get { return tailSnake; } set { tailSnake = value; } }
}
class Food
{
    public Body preySnake = new Body();
    Random random = new Random();
    Frame frames = new Frame();

    public Food()
    {
        preySnake.HeadSnake = random.Next(5, frames.Width);
        preySnake.TailSnake = random.Next(5, frames.Height);
    }

    public void displayFood()
    {
        Console.SetCursorPosition(preySnake.HeadSnake, preySnake.TailSnake);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("$");
    }

    public Body locationFood()
    {
        return preySnake;
    }

    public void newlocationFood()
    {
        preySnake.HeadSnake = random.Next(5, frames.Width);
        preySnake.TailSnake = random.Next(5, frames.Height);
    }
}
class Tester
{
    static void Main()
    {
        Console.Title = "Snake Game";
        // Initialization
        bool finish = false;
        Frame frame = new Frame();
        Snake snake = new Snake();
        Food food = new Food();

        // Welcome
        Console.Write("\n*****GAME SNAKE*****");
        Console.Write("\n1.Start game");
        Console.Write("\n2.Quit game\n");
        int option;

        do
        {
            option = int.Parse(Console.ReadLine());
            switch (option)
            {
                case 1:
                    while (!finish)
                    {
                        try
                        {
                            frame.displayFrame();
                            snake.fflushStdin();
                            food.displayFood();
                            snake.displaySnake();
                            snake.moveSnake();
                            snake.eatFood(food.locationFood(), food);
                            snake.isDead();
                            snake.hitWall(frame);
                        }
                        catch (SnakeException e)
                        {
                            Console.Clear();
                            Console.WriteLine(e.Message);
                            Console.WriteLine(e.StackTrace);
                            goto default;
                        }
                    }
                    break;
                default: return;
            }
        }
        while (option != 2);
    }
}