using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;


namespace Pliz_Work
{

    class Program
    {


        static void Main(string[] args)
        {

            Console.WriteLine("funkar");

            //Game myGame = new Game(15, 6);
            //Level level = new Level();
            //RandomTestLevel rndLevel = new RandomTestLevel(10, 10);

            // rndLevel.Draw(10, 10);


            Game myGame = new Game(22, 8);
            while (true)
            {
                myGame.UpdateBoard();
                myGame.DrawBoard();
                //level.Start(0,0);
                //Console.ReadKey();
                //Console.ReadKey();
            }
        }
    }

    class Game
    {

        List<GameObject> GameObjects = new List<GameObject>();
        List<LevelBase> levels = new List<LevelBase>();
        List<Scene> scenes = new List<Scene>();


        public Game(int xSize, int ySize)
        {


            //levels.Add(new RandomTestLevel(10, 10));

            //for (int i = 0; i < ySize + 2; i++)
            //{
            //    for (int j = 0; j < xSize + 2; j++)
            //    {
            //        if (j == 0 || i == 0 || i == ySize + 1 || j == xSize + 1)
            //        {
            //            GameObjects.Add(new Wall(j, i));
            //        }
            //    }
            //}

            for (int i = 0; i < ySize + 2; i++)
            {
                for (int j = 0; j < xSize + 2; j++)
                {
                    if (j == 0 || i == 0 || i == ySize + 1 || j == xSize + 1)
                    {
                        GameObjects.Add(new Wall(j, i));
                    }
                }
            }
            GameObjects.Add(new Player(5, 5));
        }

        public void DrawBoard()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Draw(5, 3);
            }

            foreach (LevelBase level in levels)
            {

            }





        }

        public void UpdateBoard()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Update();
            }

        }
    }

    abstract class GameObject
    {
        public int XPosition;
        public int YPosition;
        public abstract void Draw(int xBoxSize, int yBoxSize);
        public abstract void Update();
    }

    class Wall : GameObject
    {
        public Wall(int xPosition, int yPosition)
        {
            XPosition = xPosition;
            YPosition = yPosition;
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            int startX = XPosition * xBoxSize;
            int startY = YPosition * yBoxSize;
            //Console.SetCursorPosition(startX, startY);
            //Console.Write("█");
            //Console.SetCursorPosition(startX, startY + 1);
            //Console.Write(" ██ ");
            //Console.SetCursorPosition(startX, startY + 2);
            //Console.Write("█  █");
        }

        public override void Update()
        {

        }
    }

    class Player : GameObject
    {
        int lastX;
        int lastY;

        public Player(int xPos, int yPos)
        {
            XPosition = xPos;
            YPosition = yPos;
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            int curX = XPosition * xBoxSize;
            int curY = YPosition * yBoxSize;
            Console.SetCursorPosition(curX, curY);
            Console.Write("█████");
            Console.SetCursorPosition(curX, curY + 1);
            Console.Write("█████");
            Console.SetCursorPosition(curX, curY + 2);
            Console.Write("█████");

            lastY = curY;
            lastX = curX;
        }

        public override void Update()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();

            Erase();

            if (keyInfo.Key == ConsoleKey.W)
            {
                YPosition--;
            }
            else if (keyInfo.Key == ConsoleKey.S)
            {
                YPosition++;
            }
            else if (keyInfo.Key == ConsoleKey.D)
            {
                XPosition++;
            }
            else if (keyInfo.Key == ConsoleKey.A)
            {
                XPosition--;
            }
        }

        public void Erase()
        {
            Console.SetCursorPosition(lastX, lastY);
            Console.Write("     ");
            Console.SetCursorPosition(lastX, lastY + 1);
            Console.Write("     ");
            Console.SetCursorPosition(lastX, lastY + 2);
            Console.Write("     ");
        }
    }

    abstract class Scene
    {
        public abstract void Start();
    }

    class MainMenu : Scene
    {
        public override void Start()
        {
            Console.WriteLine("Main Menu");
            Console.WriteLine("1: Start game");
            Console.WriteLine("2: Load game");
            Console.WriteLine("3: Highscore");
            Console.WriteLine("4: Exit game");

            bool rightAns = false;

            while (!rightAns)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.D1)
                {
                    rightAns = true;
                }
                else if (keyInfo.Key == ConsoleKey.D2)
                {
                    rightAns = true;
                }
                else if (keyInfo.Key == ConsoleKey.D3)
                {
                    rightAns = true;
                }
                else if (keyInfo.Key == ConsoleKey.D4)
                {
                    rightAns = true;
                }
                else
                {
                    rightAns = false;
                }
            }
        }
    }

    class Level
    {

        Random rnd;

        //string ranLevel = File.ReadAllText(@"C:\Users\patrik.fridh\Documents\Github\GridGame\Test.txt");
        char[] lines;
        public Level()
        {

        }

        public void Start(int startX, int startY)
        {
            int curY = startX;
            int curX = startY;

            rnd = new Random();

            int rndNum = 0;

            int offset = 1;

            bool write = false;

            List<Block> blocks = new List<Block>();

            //lines = ranLevel.ToCharArray(0, ranLevel.Length);

            for (int i = 0; i < lines.Length; i++)
            {
                rndNum = rnd.Next(3);

                char curChar = lines[i];

                if (curChar.ToString() == "1")
                {
                    write = true;
                    curX += offset;
                }
                if (curChar.ToString() == "0")
                {
                    write = false;
                    curX += offset;
                }
                if (curChar.ToString() == "2")
                {
                    write = false;
                    curX = startX;
                    curY += offset;
                }
                if (curChar.ToString() == "3")
                {
                    if (rndNum == 1)
                    {
                        write = true;
                    }
                    else
                    {
                        write = false;
                    }

                    curX += offset;
                }


                blocks.Add(new Block(curX, curY, write));
                blocks[i].Draw(curX, curY);
            }
        }
    }

    class Block : GameObject
    {
        int xPosition;
        int yPosition;

        bool write;

        public Block(int xPos, int yPos, bool ifWrite)
        {
            xPosition = xPos;
            yPosition = yPos;
            write = ifWrite;
        }


        public override void Update()
        {

        }

        public override void Draw(int x, int y)
        {

            if (write)
            {
                Console.SetCursorPosition(x, y);
                Console.Write("█");
                //Console.SetCursorPosition(x, y + 1);
                //Console.Write("█");
            }
            else
            {
                Console.Write("    ");
            }
        }
    }

    abstract class LevelBase
    {

        public abstract void Draw(int x, int y);

    }

    class RandomTestLevel : LevelBase
    {
        int levelXSize;
        int levelYSize;

        bool RIGHT = true;
        bool LEFT = true;
        bool UP = true;
        bool Down = true;

        Random rnd = new Random();

        int curX;
        int curY;

        bool done = false;

        int moves = 0;

        public RandomTestLevel(int xSize, int ySize)
        {
            levelXSize = xSize;
            levelYSize = ySize;

            while (!done)
            {
                int dir = rnd.Next(4);

                //Console.WriteLine(dir);

                if (dir == 0 && curX < 100)
                {
                    curX++;
                }
                if (dir == 1 && curX > 0 && curX < 100)
                {
                    curX--;
                }
                if (dir == 2 && curY < 100)
                {
                    curY++;
                }
                if (dir == 3 && curY > 0 && curX < 100)
                {
                    curY--;
                }
                else
                {
                    curX++;
                }

                Draw(curX, curY);

                moves++;

                if (moves == 100)
                {
                    done = true;
                }
            }

        }

        public override void Draw(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("████");
            Console.SetCursorPosition(x, y + 1);
            Console.Write("████");
        }
    }
}