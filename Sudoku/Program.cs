using System;
using Sudoku.Classes;

namespace Sudoku
{
    /* 
     * Main Program class 
     */

    class Program
    {
        // store variables that are part of the game  
        private const int boardSize = 9;
        private static GameBoard gameBoard;

        static void Initialise()
        {
            // anything to initialise structure 

            //Pass in difficulty 
            int removedValues = Menu.DifficultyMenu();

            gameBoard = new GameBoard(boardSize, removedValues);
        }

        //Main method for game 
        static void Main(string[] args)
        {
            Initialise();

            bool playing = true;

            while (playing)
            {
                ClearAndDrawGameBoard();

                GamePlayChoice choice = Menu.GamePlayMenu();

                //Switch statement for game play choices
                switch (choice)
                {
                    //Enter Value
                    case GamePlayChoice.EnterValue:
                        ClearAndDrawGameBoard();
                        EnterValue input = Menu.GridInputMenu(boardSize);

                        if (input.success)
                        {
                            if (!gameBoard.TryAddValue(input))
                            {
                                Menu.DisplayError("Incorrect guess, please try again...");
                            }
                        }
                        break;
                    
                     //Undo
                    case GamePlayChoice.Undo:
                        if (!gameBoard.TryUndo())
                        {
                            Menu.DisplayError("Unable to perform Undo function...");
                        }
                        break;
                    
                    //Redo
                    case GamePlayChoice.Redo:
                        if (!gameBoard.TryRedo())
                        {
                            Menu.DisplayError("Unable to perform Redo function...");
                        }
                        break;

                    //Check if Sudoku is complete
                    case GamePlayChoice.CheckComplete:
                        if (!gameBoard.CheckIfGameComplete())
                        {
                            Menu.DisplayError("The game is not complete...");
                        }
                        else
                        {
                            Console.WriteLine("Congratulations - you have completed the Sudoku!");
                            playing = false;
                        }
                        break;

                    //Quit the game
                    case GamePlayChoice.Quit:
                        Console.WriteLine("Thank you for playing!");
                        playing = false;
                        break;
                }
            }
        }

        //Function when called, clears previous game play text from the command line and re-draws the Sudoku board
        private static void ClearAndDrawGameBoard()
        {
            //Clear console to improve on clarity 
            Console.Clear();

            //Draw Board 
            gameBoard.DrawBoard();
        }
    }
}