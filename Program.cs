using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace mindsweeper_så_bra_som_jeg_kan
{    
    class Borad
    {
        public Borad() 
        {
            mineNum = 2;
            
            kalX = 0;
           
            kalY = 0;
            
            størX = 10;
            
            størY = 10;
           
            hvis = new bool[størX, størY];
           
            harMine = new bool[størX, størY];
           
            harFlag = new bool[størX, størY];
           
            runtMines = new int[størX, størY];
        }

        private void putFrame(int x, int y, bool del)
        {
            if (del)
            {
                Console.SetCursorPosition(x - 1, y);
                Console.Write(" ");
               
                Console.SetCursorPosition(x + 1, y);
                Console.Write(" ");
               
                Console.SetCursorPosition(x, y - 1);
                Console.Write(" ");
              
                Console.SetCursorPosition(x, y + 1);
                Console.Write(" ");
            }
            else
            {
                Console.SetCursorPosition(x - 1, y);
                Console.Write("|");
                
                Console.SetCursorPosition(x + 1, y);
                Console.Write("|");
                
                Console.SetCursorPosition(x, y - 1);
                Console.Write("-");
                
                Console.SetCursorPosition(x, y + 1);
                Console.Write("-");
            }
        }

        public void display()
        {
            for (int x = 0; x < størX; x++)
            {
                for (int y = 0; y < størY; y++)
                {
                    Console.SetCursorPosition(x * 2 + 1, y * 2 + 1);
                    if (hvis[x, y])
                    {
                        if (harMine[x, y])
                            Console.Write('*');
                        else
                        {
                            if (runtMines[x, y] != 0)
                                Console.Write(runtMines[x, y]);
                            else
                                Console.Write(' ');
                        }
                    }
                    else
                    {
                        if (harFlag[x, y])
                            Console.Write('F');
                        else
                            Console.Write('#');

                    }
                }
            }
        }

        private Boolean isXYvalid(int x, int y)
        {
            return ((x >= 0) && (y >= 0) && (x < størX) && (y < størY));
        }

        private Boolean placeMine(int x, int y)
        {
            if ((isXYvalid(x, y)) && (!harMine[x, y]))
            {
                harMine[x, y] = true;
                for (int xx = -1; xx <= 1; xx++)
                    for (int yy = -1; yy <= 1; yy++)
                    {
                        if (((xx != 0) || (yy != 0)) && isXYvalid(x + xx, y + yy))
                            runtMines[x + xx, y + yy]++;
                    }
                return true;
            }
            else
                return false;


        }

        private int rand(int range, int index)
        {
            return (int)(Math.Cos(index * 1000 + Math.Sin(index * 101)) * range);
        }

        public void makeBoard(int seed)
        {
            reveledNum = 0;

            for (int x = 0; x < størX; x++)
                for (int y = 0; y < størY; y++)
                {
                    hvis[x, y] = false;
                    harMine[x, y] = false;
                    harFlag[x, y] = false;
                    runtMines[x, y] = 0;
                }

            Random rnd = new Random();
            int count = 0;
            int i = 0;
            while (count < mineNum)
            {
                i++;
                if (placeMine(rnd.Next(størX), rnd.Next(størY)))
                    count++;
            }


        }

        public void putFlag(int x, int y)
        {
            if (!hvis[x, y])
                harFlag[x, y] = true;
        }

        public void removeFlag(int x, int y)
        {
            
            harFlag[x, y] = false;
        }

        public bool revelBlock(int x, int y)
        {
            if (!harFlag[x, y])
            {
                reveledNum++;
                hvis[x, y] = true;
                int newX, newY;
                if ((runtMines[x, y] == 0) && (!harMine[x, y]))
                {
                    for (int xx = -1; xx <= 1; xx++)
                        for (int yy = -1; yy <= 1; yy++)
                        {
                            newX = x + xx;
                            newY = y + yy;
                            if ((isXYvalid(newX, newY)) && (!hvis[newX, newY]) && (!harFlag[newX, newY]))
                                revelBlock(newX, newY);
                        }
                }
                return harMine[x, y];
            }
            else
                return false;
        }

        public bool wonGame()
        {
            Console.WriteLine(størX * størY);
            Console.WriteLine(reveledNum);
            
            Console.WriteLine((størX * størY) - (reveledNum + mineNum));
            return (reveledNum + mineNum) == (størX * størY);
        }

        public bool control(ConsoleKeyInfo cki)
        {

            putFrame(størX * 2 + 1, størY * 2 + 1, true);

            if ((kalX < størX - 1) && (cki.Key == ConsoleKey.RightArrow))
                kalX++;
           
            if ((kalX > 0) && (cki.Key == ConsoleKey.LeftArrow))
                kalX--;
            if ((kalY < størY - 1) && (cki.Key == ConsoleKey.DownArrow))
                kalY++;
           
            if ((kalY > 0) && (cki.Key == ConsoleKey.UpArrow))
                kalY--;
            putFrame(kalX * 2 + 1, kalY * 2 + 1, false);

            if (cki.Key == ConsoleKey.Enter)
                putFlag(kalX, kalY);
           
            if (cki.Key == ConsoleKey.Backspace)
                removeFlag(kalX, kalY);
            
            if (cki.Key == ConsoleKey.Spacebar)
                return revelBlock(kalX, kalY);
            else
                return false;

        }
        
        private bool[,] hvis, harMine, harFlag;
        
        private int[,] runtMines;
        int størX, størY;
        
        int kalX, kalY;
        int mineNum, reveledNum;
    }

    class Program
    {


        static void Main(string[] args)
        {
            Borad game;
            bool exit = false;
            int lostNum = 1;
            ConsoleKeyInfo ch;

            game = new Borad();
            game.makeBoard(10);
            game.display();

            do
            {
                ch = Console.ReadKey(true);
                

                if (game.control(ch))
                {
                    game.makeBoard(10);
                    
                    Console.SetCursorPosition(25, lostNum);
                    Console.WriteLine("you lost");
                    
                    game.display();
                    lostNum++;

                }
                
                game.display();
                exit = ch.Key == ConsoleKey.Escape;
            }
            while (!exit && !game.wonGame());
            if (game.wonGame())
                Console.WriteLine("you won!");
            Console.ReadLine();
        }
    }
}