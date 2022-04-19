using System;
using System.Collections.Generic;
using System.Text;


namespace Sudoku.Classes

{
    /* 
     * Menu class 
     * Welcomes user to the game 
     * Asks for user input of difficulty level 
     * reads this input and returns int value to be used when creating the sudoku game 
     */

    public enum GamePlayChoice
    {
        Invalid,
        EnterValue,
        Undo,
        Redo,
        CheckComplete,
        Quit
    }

    public class EnterValue
    {
        public bool success = false;
        public int x;
        public int y;
        public int value;
    }

    static class Menu
    {
        //Function to check the x and y co-ordinates entered by the user and the value to see if they are valid 
        public static EnterValue GridInputMenu(int maxValue)
        {
            EnterValue returnValue = new EnterValue();

            //For x value 
            Console.WriteLine("Please enter a value between 1 and " + maxValue + " for the x co-ordinate:\n");
            int x = -1;

            //If x value isn't valid, error 
            if (!int.TryParse(Console.ReadLine(), out x))
            {
                DisplayError("Invalid Entry for X Co-ordinate...");
                return null;
            }
            else
            {
                if (x < 1 || x > maxValue)
                {
                    DisplayError("Invalid Entry, out of range, should be between 1 and " + maxValue + "...");
                    return null;
                }
                else
                {
                    ///Got a good value, store it in our class 
                    returnValue.x = x;
                }
            }

            //For y value 
            Console.WriteLine("Please enter a value between 1 and " + maxValue + " for the y co-ordinate:\n");
            int y = -1;

            //If y value isn't valid, error 
            if (!int.TryParse(Console.ReadLine(), out y))
            {
                DisplayError("Invalid Entry for Y Co-ordinate...");
                return null;
            }
            else
            {
                if (y < 1 || y > maxValue)
                {
                    DisplayError("Invalid Entry, out of range, should be between 1 and " + maxValue + "...");
                    return null;
                }
                else
                {
                    ///Got a good value, store it in our class 
                    returnValue.y = y;
                }
            }

            //For value entered 
            Console.WriteLine("Please enter a value between 1 and " + maxValue + " for the value:\n");
            int value = -1;

            //If the value isn't valid, error 
            if (!int.TryParse(Console.ReadLine(), out value))
            {
                DisplayError("Invalid Entry for the value...");
                return null;
            }
            else
            {
                if (value < 1 || value > maxValue)
                {
                    DisplayError("Invalid Entry, out of range, should be between 1 and " + maxValue + "...");
                    return null;
                }
                else
                {
                    ///Got a good value, store it in our class 
                    returnValue.value = value;
                }
            }

            returnValue.success = true;
            return returnValue;
        }

        //Function for the Game Play Menu showing the choices a user can make 
        public static GamePlayChoice GamePlayMenu()
        {
            //Ask the user to select an option 
            Console.WriteLine("Please choose an option:\n");

            Console.WriteLine("1 - Enter a value");
            Console.WriteLine("2 - Undo");
            Console.WriteLine("3 - Redo");
            Console.WriteLine("4 - Check to see if the Sudoku is complete");
            Console.WriteLine("5 - Quit");

            //Read the user's input 
            string input = Console.ReadLine();

            //Switch statement for the different game play choices 
            switch (input)
            {
                case "1":
                    return GamePlayChoice.EnterValue;
                case "2":
                    return GamePlayChoice.Undo;
                case "3":
                    return GamePlayChoice.Redo;
                case "4":
                    return GamePlayChoice.CheckComplete;
                case "5":
                    return GamePlayChoice.Quit;

                //If incorrect value entered, asks user to try again - pressing enter to bring back the game play choices  
                default:
                    DisplayError("Invalid entry, Please try again...");
                    return GamePlayChoice.Invalid;
            }
        }

        //Function for the Difficulty Menu 
        public static int DifficultyMenu()
        {
            //Write welcome message 
            Console.WriteLine("Welcome to this simple command-line Sudoku Game!\n");
            //Ask user to enter a difficulty level 
            Console.WriteLine("Please select a difficulty level below by entering the corresponding number, then press the ENTER key:\n");
            Console.WriteLine("1 - Easy \n" + "2 - Medium \n" + "3 - Hard \n");

            //Read user inputted difficulty level 
            string input = Console.ReadLine();

            //Switch statement for the different diffuclty options 
            switch (input)
            {
                //Easy 
                case "1":
                    return 10;

                //Medium 
                case "2":
                    return 20;

                //Hard 
                case "3":
                    return 30;

                //If incorrect value entered, asks user to try again - pressing enter to bring back difficulty menu options 
                default:
                    DisplayError("Invalid entry, Please try again...");
                    return DifficultyMenu();
            }
        }

        //Function to Display an error 
        public static void DisplayError(string errorMessage)
        {
            Console.WriteLine(errorMessage + "\n Press the ENTER key To Continue");

            //While enter button hasn't been pressed, do nothing 
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                //Do nothing 
            }

            //Clear console to improve on clarity 
            Console.Clear();
        }
    }
}

