using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku.Classes
{
    /* 
     * Game Board class  
     * Pulls in board size and difficulty level  
     * Generates the game grid, fills the three diagonal matrices, recursively the other matrices, removes randonly the number of values correspondign to the difficulty level chosen 
     */

    class GameBoard
    {
        //Declare variables 
        private int gridSize = 0;
        private int squareRoot = 3;
        private int[,] gameGrid;

        private int historyCursor = 0;
        private List<int[,]> history;

        private int removeDigitsNumber = 0;

        //Class constructor 
        public GameBoard(int size, int removedDigits)
        {
            gridSize = size;

            removeDigitsNumber = removedDigits;

            //Generate grid using 2d Array 
            gameGrid = new int[size, size];

            //Generate the contents of the grid 
            AddValues();

            //list to store history 
            history = new List<int[,]>();

            //Adding the new gamegrid to history 
            AddHistory();
        }

        /***************************************************************************************
        *    Title: GeeksForGeeks Program for Sudoku Generator
        *    Author: Ankur Trisal
        *    Date: 12th November 2021
        *    Code version: <code version>
        *    Availability: https://www.geeksforgeeks.org/program-sudoku-generator/
        *    Functions altered and used: AddValues(), AddDiagonal(), NotInBox(), AddBox(), RandomNumberGenerator(), CheckCanAdd(), NotInRow(), NotInCol(), AddRemaining(), RemoveDigits()
        *
        ***************************************************************************************/

        //Generate Sudoku values 
        public void AddValues()
        {
            //Add the diagonal of squareRoot * squareRoot matrices 
            AddDiagonal();

            //Add the remaining values to fill the sudoku 
            AddRemaining(0, squareRoot);

            //randomly remove the number of values that match up with difficulty entered by user 
            RemoveDigits();
        }

        //Function that adds 3, 3x3 matrices 
        private void AddDiagonal()
        {
            for (int x = 0; x < gridSize; x = x + squareRoot)
            {
                AddBox(x, x);
            }
        }

        //Bool function that checks if a 3x3 matric contains a number, returns false if so 
        private bool NotInBox(int rowStart, int colStart, int number)
        {
            for (int x = 0; x < squareRoot; x++)
                for (int y = 0; y < squareRoot; y++)
                    if (gameGrid[rowStart + x, colStart + y] == number)
                        return false;

            return true;
        }

        //Function to fill a 3x3 matrix 
        private void AddBox(int row, int col)
        {
            int number;

            for (int x = 0; x < squareRoot; x++)
            {
                for (int y = 0; y < squareRoot; y++)
                {
                    do
                    {
                        number = RandomNumberGenerator(gridSize);
                    }

                    while (!NotInBox(row, col, number));

                    gameGrid[row + x, col + y] = number;
                }
            }
        }

        //Function to create random number betwee 1 and 9 
        int RandomNumberGenerator(int number)
        {
            Random random = new Random();
            return (int)Math.Floor((double)(random.NextDouble() * number + 1));
        }


        //Bool function to check if the number can be put into a cell 
        private bool CheckCanAdd(int x, int y, int number)
        {
            return (NotInRow(x, number) &&
                    NotInCol(y, number) &&
                    NotInBox(x - x % squareRoot, y - y % squareRoot, number));
        }

        //Bool function to check if number not used in row 
        private bool NotInRow(int x, int number)
        {
            for (int y = 0; y < gridSize; y++)
                if (gameGrid[x, y] == number)
                    return false;

            return true;
        }

        //Bool function to check if number not used in column 
        private bool NotInCol(int y, int number)
        {
            for (int x = 0; x < gridSize; x++)
                if (gameGrid[x, y] == number)
                    return false;

            return true;
        }

        //Bool function to recursively add the remaining numbers to create the sudoku 
        bool AddRemaining(int x, int y)
        {
            if (y >= gridSize && x < gridSize - 1)
            {
                x = x + 1;
                y = 0;
            }

            if (x >= gridSize && y >= gridSize)
                return true;

            if (x < squareRoot)
            {
                if (y < squareRoot)
                    y = squareRoot;
            }
            else if (x < gridSize - squareRoot)
            {
                if (y == (int)(x / squareRoot) * squareRoot)
                    y = y + squareRoot;
            }
            else
            {
                if (y == gridSize - squareRoot)
                {
                    x = x + 1;
                    
                    y = 0;
                    if (x >= gridSize)
                        return true;
                }
            }

            for (int number = 1; number <= gridSize; number++)
            {
                if (CheckCanAdd(x, y, number))
                {
                    gameGrid[x, y] = number;
                    if (AddRemaining(x, y + 1))
                        return true;
                 
                    gameGrid[x, y] = 0;
                }
            }
            return false;
        }

        //Function to remove number of digits from the board corresponding to the difficulty level chosen by user
        public void RemoveDigits()
        {
            int count = removeDigitsNumber;
            while (count != 0)
            {
                //Find random cell in grid 
                int cellId = RandomNumberGenerator(gridSize * gridSize) - 1;

                //Extract the coordiantes of the cell 
                int x = (cellId / gridSize);
                int y = cellId % 9;
                if (y != 0)
                    y = y - 1;

                //If the value is not 0, then make it 0 
                if (gameGrid[x, y] != 0)
                {
                    count--;
                    gameGrid[x, y] = 0;
                }
            }
        }

        //Function to try and add a value entered by the user 
        public bool TryAddValue(EnterValue value)
        {
            if (CheckCanAdd(value.x - 1, value.y - 1, value.value))
            {
                gameGrid[value.x - 1, value.y - 1] = value.value;
                //add to history 
                AddHistory();
                return true;
            }

            return false;
        }

        //Bool function to try and add a move to the history list 
        private bool AddHistory()
        {
            if (history.Count > 0)
            {
                if (historyCursor < history.Count - 1)
                {
                    history.RemoveRange(historyCursor + 1, history.Count - historyCursor + 1);
                }
            }

            int[,] historyCopy = (int[,])gameGrid.Clone();
            history.Add(historyCopy);
            historyCursor = history.Count - 1;

            return true;
        }

        //Bool function to try and undo a move if possible, reverting to the previous game play state 
        public bool TryUndo()
        {
            //if the history cursor isn't 0, so a move has been made 
            if (historyCursor != 0)
            {
                // revert back to the previous cursor state 
                historyCursor--;

                //clone the previous game play state 
                gameGrid = (int[,])history[historyCursor].Clone();

                return true;
            }

            //if not, return false 
            return false;
        }

        ////Bool function to try and redo a move if possible
        public bool TryRedo()
        {
            //If the history cursor isn't 0, so a move has been made
            if (historyCursor != 0)
            {
                //redo the undo
                historyCursor++;

                gameGrid = (int[,])history[historyCursor].Clone();

                return true;
            }

            return false;
        }

        //Bool function to see if the Sudoku grid has been successfully completed 
        public bool CheckIfGameComplete()
        {
            //if there are any cells with a value of zero, return false as the game has not been completed
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (gameGrid[x, y] == 0)
                        return false;
                }
            }

            //If there are no cells equal to zero, then return true (means sudoku is complete!)
            return true;
        }

        //Function that 'draws' the board 
        public void DrawBoard()
        {
            string header = "  | ";

            //For loop for when x is less that grid size 
            for (int x = 0; x < gridSize; x++)
            {
                //If x + 1 divided by square root is equal to zero, add | 
                if ((x + 1) % squareRoot == 0)
                {
                    header += (x + 1) + " | ";
                }
                //Else, display "   " 
                else
                {
                    header += (x + 1) + "   ";
                }
            }
            //Display 
            Console.WriteLine(header);

            //For loop for when y is less that grid size 
            for (int y = 0; y < gridSize; y++)
            {
                //If y divided by squareRoot is equal to zero 
                if (y % squareRoot == 0)
                {
                    string divider = "";

                    //For loop for when x is less than grid size times 4.25, "-" 
                    for (int x = 0; x < (gridSize * 4.25f); x++)
                    {
                        divider += "-";
                    }
                    //Display 
                    Console.WriteLine(divider);
                }
                //Else, add "" 
                else
                {
                    Console.WriteLine("");
                }

                string line = "";
                line = (y + 1) + " | ";

                //For loop for when x is less than grid size 
                for (int x = 0; x < gridSize; x++)
                {
                    //If x + 1 divided by square root is equal to zero, add line ("") + " | " 
                    if ((x + 1) % squareRoot == 0)
                    {
                        if (gameGrid[x, y] == 0)
                        {
                            line += "  | ";
                        }
                        else
                        {
                            line += gameGrid[x, y] + " | ";
                        }
                    }
                    //Else, add line ("") + "   " 
                    else
                    {
                        if (gameGrid[x, y] == 0)
                        {
                            line += "    ";
                        }
                        else
                        {
                            line += gameGrid[x, y] + "   ";
                        }
                    }
                }
                //Display 
                Console.WriteLine(line);
            }
        }
    }
}