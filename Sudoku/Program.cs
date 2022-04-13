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
        // like undo, redo, game board - globaly available to entire game loop 
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

                switch (choice)
                {
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

                    case GamePlayChoice.Undo:
                        if (!gameBoard.TryUndo())
                        {
                            Menu.DisplayError("Unable to perform Undo function...");
                        }
                        break;

                }

                // first ask what difficulty the user wants 
                // generate the sudoku board - pass in diffculty parameter, return the game board (design a class to handle game board) 
                // once got board generated, will have main loop - draw board, ask for inupt, process input, update board, repeat  
            }
        }
        private static void ClearAndDrawGameBoard()
        {
            //Clear console to improve on clarity 
            Console.Clear();

            //Draw Board 
            gameBoard.DrawBoard();
        }

        static void InputValue()
        {
        }
    }
}