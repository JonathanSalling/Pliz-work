using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;


namespace GridGame
{

    class Program
    {


        static void Main(string[] args)
        {
            Game myGame = new Game(15, 6);

            while (true)
            {
                myGame.UpdateBoard();
                myGame.DrawBoard();
            }

        }
    }

    class Game
    {
        int trys = 0; // var sjunde turn så ritas bannan ut igen

        string curLevelFileName = "LevelOne"; // använd för att hitta vilken level som väljs

        List<GameObject> GameObjects = new List<GameObject>();
        List<Collectible> collss;
        List<LevelBase> Levels = new List<LevelBase>();
        List<Block> blockss;

        public Game(int xSize, int ySize)
        {
            GameObjects.Add(new Player(1, 1));
            AddLevels();
            GameObjects.Add(new Enemy(10, 20));
            Levels.Add(new Level());
        }

        public void DrawBoard()
        {

            if (trys == 7)
            {
                foreach (LevelBase i in Levels)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    i.Start(0, 0);
                }
                trys = 0;
            }
            foreach (GameObject gameObject in GameObjects)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                gameObject.Draw(5, 3);
                Console.ForegroundColor = ConsoleColor.White;
            }

            trys++;

        }

        public void AddLevels()
        {
            trys = 7;
            Levels.Add(new Level(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "LevelOne.txt"))));

        }

        public void getLists()
        {
            collss = Levels[1].SendColls;
            blockss = Levels[1].SendBlocks;
        }

        public void UpdateBoard()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Update();
                Console.CursorVisible = false;
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

    class Player : GameObject
    {
        int lastX;
        int lastY;

        List<Collectible> collse;

        public Player(int xPos, int yPos)
        {
            XPosition = xPos;
            YPosition = yPos;

        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            int curX = XPosition;
            int curY = YPosition;
            Console.SetCursorPosition(curX, curY);
            Console.Write("█");

            lastY = curY;
            lastX = curX;
        }

        public override void Update()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

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
            Console.Write(" ");
        }
    }

    class Enemy : GameObject
    {
        int enemyLastX;
        int enemyLastY;


        public Enemy(int enemyYPos, int enemyXPos)
        {
            YPosition = enemyYPos;
            XPosition = enemyXPos;
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            Console.SetCursorPosition(XPosition, YPosition);
            Console.Write("█");
        }

        public override void Update()
        {

            Erase();
        }

        public void Erase()
        {
            Console.SetCursorPosition(enemyLastX, enemyLastY);
            Console.Write(" ");
        }
    }

    abstract class LevelBase
    {
        public string fileText;

        public abstract void Start(int x, int y);
        public List<Collectible> SendColls;
        public List<Block> SendBlocks;
    }

    class Level : LevelBase
    {
        public List<Block> blocks = new List<Block>();
        public List<Collectible> colls = new List<Collectible>();

        char[] lines;

        ConsoleColor curColor;

        Random rnd;

        public Level(string levelData)
        {
            fileText = levelData;
        }

        public override void Start(int x, int y)
        {
            int curY = x;
            int curX = y;
            int rndNum = 0;
            int offset = 1;

            bool write = false;
            bool vertical = false;

            rnd = new Random();



            lines = fileText.ToCharArray(0, fileText.Length);

            for (int i = 0; i < lines.Length; i++)
            {
                rndNum = rnd.Next(2);

                char curChar = lines[i];

                if (curChar.ToString() == "1")
                {
                    write = true;
                    curX += offset;
                    curColor = ConsoleColor.White;
                }
                if (curChar.ToString() == "0")
                {
                    write = false;
                    vertical = false;
                    curX += offset;
                    curColor = ConsoleColor.White;
                }
                if (curChar.ToString() == "2")
                {
                    write = false;
                    vertical = false;
                    curX = x;
                    curY += offset;
                    curColor = ConsoleColor.White;
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
                if (curChar.ToString() == "4")
                {
                    write = false;
                    curX += offset;
                    curColor = ConsoleColor.Green;
                    colls.Add(new Collectible(curX, curY, curColor, false));
                }
                if (curChar.ToString() == "5")
                {
                    write = false;
                    curX += offset;
                    curColor = ConsoleColor.Blue;
                    colls.Add(new Collectible(curX, curY, curColor, true));
                }

                blocks.Add(new Block(curX, curY, write, vertical, curColor));
                blocks[i].Draw(curX, curY);

            }
            for (int i = 0; i < colls.Count; i++)
            {
                colls[i].Draw(0, 0);
            }
        }
        public List<Collectible> SendColls()
        {
            return colls;
        }
        public List<Block> SendBlocks()
        {
            return blocks;
        }
    }

    class Collectible : GameObject
    {

        int xPosition;
        int yPosition;

        public static bool touched = false;
        public bool destroyed = false;
        public bool chest;

        ConsoleColor collColor;

        public Collectible(int xPos, int yPos, ConsoleColor color, bool ischest)
        {
            xPosition = xPos;
            yPosition = yPos;
            collColor = color;
            chest = ischest;
        }


        public override void Draw(int xBoxSize, int yBoxSize)
        {

            if (!destroyed)
            {
                Console.SetCursorPosition(xPosition, yPosition);

                Console.ForegroundColor = collColor;

                Console.Write("█");
            }
            else
            {
                Console.Write(" ");
            }

        }

        public override void Update()
        {
            if (touched)
            {
                destroyed = true;
            }
        }

        public void istouched(bool thouched)
        {
            touched = thouched;
        }


    }

    class Block : GameObject
    {
        int xPosition;
        int yPosition;

        ConsoleColor blockColor;

        bool write;

        bool vert;

        public Block(int xPos, int yPos, bool ifWrite, bool vertical, ConsoleColor color)
        {
            xPosition = xPos;
            yPosition = yPos;
            write = ifWrite;
            vert = vertical;
            blockColor = color;
        }


        public override void Update()
        {

        }

        public override void Draw(int x, int y)
        {

            if (write)
            {
                if (vert)
                {
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = blockColor;
                    Console.Write("█");
                    //Console.SetCursorPosition(x, y + 1);
                    //Console.Write("██");

                }
                else if (!vert)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write("█");
                }
            }
            else
            {
                Console.Write(" ");
            }
        }
    }
}

