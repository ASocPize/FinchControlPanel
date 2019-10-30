using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control
    // Description: 
    // Application Type: Console
    // Author: Jacob Post
    // Dated Created: 
    // Last Modified: 
    //
    // **************************************************

    class Program
    {
        public enum Command
        {
            NONE,
            MOVEFORWARD,
            MOVEBACKWARD,
            STOPMOTORS,
            WAIT,
            TURNRIGHT,
            TURNLEFT,
            LEDON,
            LEDOFF,
            DONE
        }
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();

            DisplayMainMenu();

            DisplayClosingScreen();
        }

        /// <summary>
        /// display welcome screen
        /// </summary>
        /// 

        static void SetTheme()
        {
            string dataPath = @"Data\Theme.txt";
            string forgroundColorString;
            ConsoleColor forgroundColor;

            forgroundColorString = File.ReadAllText(dataPath);

            Enum.TryParse(forgroundColorString, out forgroundColor);

            Console.ForegroundColor = forgroundColor;
        }

        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        static void DisplayMainMenu()
        {
            //++++++++++++++++++++++++++++//
            // Instantiate a Finch Object //
            //++++++++++++++++++++++++++++//

            Finch finchRobot = new Finch();
            
            bool finchRobotConnected = false;
            bool quitApplication = false;
            string menuChoice;

            do
            {
                finchRobot.setLED(0, 0, 0);
                DisplayScreenHeader("Main Menu");

                //+++++++++++++++++++++++++++//
                // Get Menu Choice from User //
                //+++++++++++++++++++++++++++//

                Console.WriteLine("a) Connect Finch Robot");
                Console.WriteLine("b) Talent Show");
                Console.WriteLine("c) Data Recorder");
                Console.WriteLine("d) Alarm System");
                Console.WriteLine("e) User Programming");
                Console.WriteLine("f) Disconnect Finch Robot");
                Console.WriteLine("q) Exit Program");
                Console.WriteLine("Enter Choice:");
                menuChoice = Console.ReadLine().ToUpper().Trim();

                //+++++++++++++++++++++//
                // Process Menu Choice //
                //+++++++++++++++++++++//

                switch (menuChoice)
                {
                    case "A":
                        if (finchRobotConnected)
                        {
                            Console.Clear();
                            Console.WriteLine("Finch Robot already connected, goober. Returning to menu.");
                            DisplayContinuePrompt();
                        }

                        else
                        {
                            finchRobotConnected = DisplayConnectFinchRobot(finchRobot);
                        }

                        break;
                    case "B":
                        if (finchRobotConnected)
                        {
                            DisplayTalentShow(finchRobot);
                        }

                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Finch Robot not connected. Return to main menu and connect.");
                            DisplayContinuePrompt();
                        }

                        break;
                    case "C":
                        if (finchRobotConnected)
                        {
                            DisplayDataRecorder(finchRobot);
                        }

                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Finch Robot not connected. Return to main menu and connect.");
                            DisplayContinuePrompt();
                        }

                        break;
                    case "D":
                        if (finchRobotConnected)
                        {
                            DisplayAlarmSystem(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Finch Robot not connected. Return to main menu and connect.");
                            DisplayContinuePrompt();
                        }
                        break;
                    case "E":
                        if (finchRobotConnected)
                        {
                            DisplayUserProgramming(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Finch Robot not connected. Return to main menu and connect.");
                            DisplayContinuePrompt();
                        }
                        break;
                    case "F":
                        if (!finchRobotConnected)
                        {
                            Console.WriteLine("Your Finch bot is already disconnected, you goober.");
                        }
                        else
                        {
                            DisplayDisconnectFinchRobot(finchRobot);
                        }
                        break;
                    case "Q":
                        quitApplication = true;

                        break;
                    default:
                        Console.WriteLine("\t***************************");
                        Console.WriteLine("\tPlease Enter A Menu Choice.");
                        Console.WriteLine("\t***************************");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);

        }
        #region User Programming And Its Methods
        static void DisplayUserProgramming(Finch finchRobot)
        {
            (int motorSpeed, int ledBrightness, int waitSeconds) CommandParameters;
            CommandParameters.motorSpeed = 0;
            CommandParameters.ledBrightness = 0;
            CommandParameters.waitSeconds = 0;

            List<Command> commands = new List<Command>();

            string menuChoice;
            bool quitApplication = false;

            do
            {
                DisplayScreenHeader("Main Menu");

                //++++++++++++++++++++++//
                // Get User Menu Choice //
                //++++++++++++++++++++++//

                Console.WriteLine("a) Set Command Parameters");
                Console.WriteLine("b) Add Commands");
                Console.WriteLine("c) View Commands");
                Console.WriteLine("d) Execute Commands");
                Console.WriteLine("e) Save Commands To Text File");
                Console.WriteLine("f) Load Commands From Text File");
                Console.WriteLine("q) Quit");
                Console.Write("Enter Choice:");

                menuChoice = Console.ReadLine().ToLower();



                //++++++++++++++++++++++++++//
                // Process User Menu Choice //
                //++++++++++++++++++++++++++//

                switch (menuChoice)
                {
                    case "a":
                        CommandParameters = DisplayGetFinchCommandParameters();
                        break;

                    case "b":
                        DisplayGetFinchCommands(commands);
                        break;

                    case "c":
                        DisplayFinchCommands(commands);
                        break;

                    case "d":
                        DisplayExecuteCommands(finchRobot, commands, CommandParameters);
                        break;

                    case "e":
                        DisplayWriteUserProgrammingData(commands);
                        break;

                    case "f":
                        commands = DisplayReadProgrammingData();
                        break;

                    case "q":
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Please enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }
            } while (!quitApplication);
        }

        static List<Command> DisplayReadProgrammingData()
        {
            string dataPath = @"Data\Data.txt";
            List<Command> commands = new List<Command>();
            string[] commandsString;
 
            DisplayScreenHeader("Read Commands From Data File");

            Console.WriteLine("Ready to Read Commands from the Data File.");
            Console.WriteLine();

            commandsString = File.ReadAllLines(dataPath);

            Command command;
            foreach (string commandString in commandsString)
            {
                Enum.TryParse(commandString, out command);

                commands.Add(command);
            }

            Console.WriteLine("Data from file successfully read.");


            DisplayContinuePrompt();

            return commands;
        }

        static void DisplayWriteUserProgrammingData(List<Command> commands)
        {
            string dataPath = @"Data\Data.txt";
            List<string> commandsString = new List<string>();

            DisplayScreenHeader("Write Commands to Data File");

            //++++++++++++++++++++++++++++++++++//
            // Create a list of command strings //
            //++++++++++++++++++++++++++++++++++//

            foreach (Command command in commands)
            {
                commandsString.Add(command.ToString());
            }

            Console.WriteLine("Ready to write to the data file");
            DisplayContinuePrompt();

            File.WriteAllLines(dataPath, commandsString.ToArray());

            Console.WriteLine();
            Console.WriteLine("Commands written to the data file.");
            
            DisplayContinuePrompt();
        }

        static void DisplayExecuteCommands(Finch finchRobot, List<Command> commands, (int motorSpeed, int ledBrightness, int waitSeconds) CommandParameters)
        {
            DisplayScreenHeader("Execute Finch Commands");

            int motorSpeed = CommandParameters.motorSpeed;
            int ledBrightness = CommandParameters.ledBrightness;
            int waitMilliSeconds = CommandParameters.waitSeconds * 1000;

            //++++++++++++++++//
            // Info and Pause //
            //++++++++++++++++//

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        break;
                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        finchRobot.wait(waitMilliSeconds);
                        finchRobot.setMotors(0, 0);
                        break;
                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-motorSpeed,-motorSpeed);
                        finchRobot.wait(waitMilliSeconds);
                        finchRobot.setMotors(0, 0);
                        break;
                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0,0);
                        break;
                    case Command.WAIT:
                        finchRobot.wait(waitMilliSeconds);
                        break;
                    case Command.TURNRIGHT:
                        break;
                    case Command.TURNLEFT:
                        break;
                    case Command.LEDON:
                        break;
                    case Command.LEDOFF:
                        break;
                    case Command.DONE:
                        break;
                    default:
                        break;
                }
            }

            DisplayContinuePrompt();
        }

        static void DisplayFinchCommands(List<Command> commands)
        {
            DisplayScreenHeader("Display Finch Commands");

            foreach (Command command in commands)
            {
                Console.WriteLine(command);
            }

            DisplayContinuePrompt();
        }

        static void DisplayGetFinchCommands(List<Command> commands)
        {
            Command command = Command.NONE;
            string userResponse;
            
            DisplayScreenHeader("Finch Robot Commands");

            while (command != Command.DONE)
            {
                Console.Write("Enter Command:");
                userResponse = Console.ReadLine().ToUpper();
                Enum.TryParse(userResponse, out command);

                commands.Add(command);
            }

            //+++++++++++++++//
            // Echo Commands //
            //+++++++++++++++//

            DisplayContinuePrompt();
        }

        static (int motorSpeed, int ledBrightness, int waitSeconds) DisplayGetFinchCommandParameters()
        {
            (int motorSpeed, int ledBrightness, int waitSeconds) CommandParameters;
            CommandParameters.motorSpeed = 0;
            CommandParameters.ledBrightness = 0;
            CommandParameters.waitSeconds = 0;
            DisplayScreenHeader("Command Parameters");

            Console.Write("Enter Motor Speed [1-255]:");
            CommandParameters.motorSpeed = int.Parse(Console.ReadLine());

            Console.Write("Enter LED Brightness [1-255]:");
            CommandParameters.ledBrightness = int.Parse(Console.ReadLine());

            Console.Write("Enter Seconds to wait for command:");
            CommandParameters.waitSeconds = int.Parse(Console.ReadLine());

            DisplayContinuePrompt();
            return CommandParameters;
        }
        #endregion

        #region Alarm System And Its Methods
        static void DisplayAlarmSystem(Finch finchrobot)
        {
            string alarmType;
            int maxseconds;
            double threshold;
            bool thresholdExceeded;

            DisplayScreenHeader("Alarm System");

            alarmType = DisplayGetAlarmType();
            maxseconds = DisplayGetMaxSeconds();
            threshold = DisplayGetThreshold(finchrobot, alarmType);

            if (alarmType == "light")
            {
                thresholdExceeded = MonitorCurrentLightLevel(finchrobot, threshold, maxseconds);
            }
            else
            {
                thresholdExceeded = MonitorCurrentTemperature(finchrobot, threshold, maxseconds);
            }


            if (thresholdExceeded)
            {
                if (alarmType == "light")
                {
                    finchrobot.setLED(255, 0, 0);
                    Console.WriteLine("Maximum Light Level Exceeded");
                }
                else
                {
                    finchrobot.setLED(255, 0, 0);
                    Console.WriteLine("Maximum Temperature Exceeded");
                }
            }
            else
            {
                Console.WriteLine("Maximum Monitoring Time Exceeded");
            }


            DisplayContinuePrompt();
        }

        static string DisplayGetAlarmType()
        {
            Console.WriteLine("Alarm Type [light or temperature]");
            return Console.ReadLine().ToLower().Trim();
        }

        static int DisplayGetMaxSeconds()
        {
            Console.WriteLine("Seconds to Monitor:");
            return int.Parse(Console.ReadLine());
        }

        static bool MonitorCurrentLightLevel(Finch finchrobot, double threshold, int maxseconds)
        {
            bool thresholdExceeded = false;
            int currentLightLevel;
            double seconds = 0;

            while (!thresholdExceeded && seconds <= maxseconds)
            {
                finchrobot.setLED(0, 255, 0);
                DisplayScreenHeader("Monitoring Light Levels");
                currentLightLevel = finchrobot.getLeftLightSensor();
                Console.WriteLine($"Maximum Light Level: {threshold}");
                Console.WriteLine($"Current Light Level: {currentLightLevel}");

                if (currentLightLevel > threshold)
                {
                    thresholdExceeded = true;
                }

                finchrobot.wait(500);
                seconds += 0.5;
            }

            return thresholdExceeded;
        }

        static bool MonitorCurrentTemperature(Finch finchrobot, double threshold, int maxseconds)
        {
            bool thresholdExceeded = false;
            double currentTemperature;
            double seconds = 0;

            while (!thresholdExceeded && seconds <= maxseconds)
            {
                finchrobot.setLED(0, 255, 0);
                DisplayScreenHeader("Monitoring Temperature");
                currentTemperature = finchrobot.getTemperature() * (9 / 5) + 32;
                Console.WriteLine($"Maximum Temperature: {threshold}\u00B0F");
                Console.WriteLine($"Current Temperature: {currentTemperature}\u00B0F");

                if (currentTemperature > threshold)
                {
                    thresholdExceeded = true;
                }

                finchrobot.wait(500);
                seconds += 0.5;
            }

            return thresholdExceeded;
        }

        static double DisplayGetThreshold(Finch finchRobot, string alarmType)
        {
            double threshold = 0;

            DisplayScreenHeader("Threshold Value");

            switch (alarmType)
            {
                case "light":
                    Console.WriteLine($"Current Light Level: {finchRobot.getLeftLightSensor()}");
                    Console.Write("Enter Maximum Light Level [0 - 255]:");
                    threshold = double.Parse(Console.ReadLine());
                    break;
                case "temperature":
                    Console.WriteLine($"Current Temperature: {finchRobot.getTemperature() * (9 / 5) + 32}\u00B0F");
                    Console.Write("Enter Maximum Temperature:");
                    threshold = double.Parse(Console.ReadLine());
                    break;
                default:
                    Console.WriteLine("Invalid input, you goober.");
                    break;
            }

            DisplayContinuePrompt();

            return threshold;
        }
        #endregion

        #region Data Recorder And Its Methods

        static void DisplayDataRecorder(Finch finchRobot)
        {
            double dataPointFrequency;
            int numberOfDataPoints;

            DisplayScreenHeader("Data Recorder");

            //+++++++++++++++++//
            // Inform the user //
            //+++++++++++++++++//

            dataPointFrequency = DisplayGetDataPointFrequency();
            numberOfDataPoints = DisplayGetNumberOfDataPoints();

            double[] Temperatures = new double[numberOfDataPoints];

            DisplayGetData(numberOfDataPoints, dataPointFrequency, Temperatures, finchRobot);

            DisplayData(Temperatures);

            DisplayContinuePrompt();
        }

        static void DisplayGetData(int numberofDataPoints, double DataPointFrequency, double[] Temperatures, Finch finchRobot)
        {
            DisplayScreenHeader("Get Data");

            //++++++++++++++++++++++++++++++++++//
            // Provide The User Info and Prompt //
            //++++++++++++++++++++++++++++++++++//

            //+++++++++++++//
            // Record Data //
            //+++++++++++++//

            for (int index = 0; index < numberofDataPoints; index++)
            {
                Temperatures[index] = finchRobot.getTemperature() * (9 / 5) + 32;
                int milliSeconds = ((int)(DataPointFrequency * 1000));
                finchRobot.wait(milliSeconds);

                //+++++++++++//
                // Echo Data //
                //+++++++++++//

                Console.WriteLine($"Temperature {index + 1}: {Temperatures[index]}\u00B0F");
            }


            DisplayContinuePrompt();
        }

        static void DisplayData(double[] temperature)
        {
            DisplayScreenHeader("Temperature Data");

            for (int index = 0; index < temperature.Length; index++)
            {
                Console.WriteLine($"Temperature {index + 1}: {temperature[index]}\u00B0F");
            }

            DisplayContinuePrompt();
        }

        static int DisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;

            DisplayScreenHeader("Number of Data points");

            Console.Write("Enter Number of Data Points: ");
            int.TryParse(Console.ReadLine(), out numberOfDataPoints);

            DisplayContinuePrompt();

            return numberOfDataPoints;
        }

        static double DisplayGetDataPointFrequency()
        {
            //++++++++++++++++++++//
            // NEW PARSING METHOD //
            //++++++++++++++++++++//

            double dataPointFrequency;

            DisplayScreenHeader("Data Point Frequency");

            Console.Write("Enter Data Point Frequency: ");
            double.TryParse(Console.ReadLine(), out dataPointFrequency);

            DisplayContinuePrompt();

            return dataPointFrequency;
        }
        #endregion        

        #region Talent Show And It's Methods
        static void DisplayTalentShow(Finch finchRobot)
        {
            DisplayScreenHeader("Talent Show");
            Console.WriteLine("Finch robot is ready to perform!");

            DisplaySinging(finchRobot);

            Console.WriteLine("TA DA!");

            DisplayContinuePrompt();
        }

        static void DisplaySinging(Finch finchRobot)
        {
            finchRobot.noteOn(1975);
            finchRobot.wait(500);
            finchRobot.noteOff();
            DisplayDancingFR(finchRobot);
            DisplayLEDR(finchRobot);

            finchRobot.noteOn(1760);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFL(finchRobot);
            DisplayLEDG(finchRobot);

            finchRobot.noteOn(1975);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBR(finchRobot);
            DisplayLEDB(finchRobot);

            finchRobot.noteOn(988);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBL(finchRobot);
            DisplayLEDR(finchRobot);

            finchRobot.noteOn(1760);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFR(finchRobot);
            DisplayLEDG(finchRobot);

            finchRobot.noteOn(1975);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFL(finchRobot);
            DisplayLEDB(finchRobot);

            finchRobot.noteOn(988);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBR(finchRobot);
            DisplayLEDR(finchRobot);

            finchRobot.noteOn(1760);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBL(finchRobot);
            DisplayLEDG(finchRobot);

            finchRobot.noteOn(1975);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFR(finchRobot);
            DisplayLEDB(finchRobot);

            finchRobot.noteOn(2093);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFL(finchRobot);
            DisplayLEDR(finchRobot);

            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBR(finchRobot);
            DisplayLEDG(finchRobot);

            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBL(finchRobot);
            DisplayLEDB(finchRobot);

            finchRobot.noteOn(2093);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFR(finchRobot);
            DisplayLEDR(finchRobot);

            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFL(finchRobot);
            DisplayLEDG(finchRobot);

            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBR(finchRobot);
            DisplayLEDB(finchRobot);

            finchRobot.noteOn(2093);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBL(finchRobot);
            DisplayLEDR(finchRobot);

            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFR(finchRobot);
            DisplayLEDG(finchRobot);

            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFL(finchRobot);
            DisplayLEDB(finchRobot);

            finchRobot.noteOn(2093);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBR(finchRobot);
            DisplayLEDR(finchRobot);

            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBL(finchRobot);
            DisplayLEDG(finchRobot);

            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFR(finchRobot);
            DisplayLEDB(finchRobot);

            finchRobot.noteOn(2093);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFL(finchRobot);
            DisplayLEDR(finchRobot);

            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBR(finchRobot);
            DisplayLEDG(finchRobot);

            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBL(finchRobot);
            DisplayLEDB(finchRobot);

            finchRobot.noteOn(2093);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFR(finchRobot);
            DisplayLEDR(finchRobot);

            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFL(finchRobot);
            DisplayLEDG(finchRobot);

            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBR(finchRobot);
            DisplayLEDB(finchRobot);

            finchRobot.noteOn(2093);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBL(finchRobot);
            DisplayLEDR(finchRobot);

            finchRobot.noteOn(2349);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFR(finchRobot);
            DisplayLEDG(finchRobot);

            finchRobot.noteOn(2349);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFL(finchRobot);
            DisplayLEDB(finchRobot);

            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBR(finchRobot);
            DisplayLEDR(finchRobot);

            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBL(finchRobot);
            DisplayLEDG(finchRobot);

            finchRobot.noteOn(2093);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFR(finchRobot);
            DisplayLEDB(finchRobot);

            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFL(finchRobot);
            DisplayLEDR(finchRobot);

            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBR(finchRobot);
            DisplayLEDG(finchRobot);

            finchRobot.noteOn(2093);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBL(finchRobot);
            DisplayLEDB(finchRobot);

            finchRobot.noteOn(1046);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFR(finchRobot);
            DisplayLEDR(finchRobot);

            finchRobot.noteOn(1318);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingFL(finchRobot);
            DisplayLEDG(finchRobot);

            finchRobot.noteOn(2093);
            finchRobot.wait(250);
            finchRobot.noteOff();
            DisplayDancingBR(finchRobot);
            DisplayLEDB(finchRobot);
            finchRobot.setMotors(0, 0);
        }

        /// <summary>
        /// FR Foward Right, FL Foward Left, BR Backward Right, BL Backward Left
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DisplayDancingFR(Finch finchRobot)
        {
            finchRobot.setMotors(0, 100);
        }
        static void DisplayDancingFL(Finch finchRobot)
        {
            finchRobot.setMotors(100, 0);
        }
        static void DisplayDancingBR(Finch finchRobot)
        {
            finchRobot.setMotors(0, -100);
        }
        static void DisplayDancingBL(Finch finchRobot)
        {
            finchRobot.setMotors(-100, 0);
        }

        /// <summary>
        /// LEDR Red, LEDG Green, LEDB Blue
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DisplayLEDR(Finch finchRobot)
        {
            finchRobot.setLED(255, 0, 0);
        }
        static void DisplayLEDG(Finch finchRobot)
        {
            finchRobot.setLED(0, 255, 0);
        }
        static void DisplayLEDB(Finch finchRobot)
        {
            finchRobot.setLED(0, 0, 255);
        }

        //for (int i = 0; i < 3; i++)
        //{
        //    finchRobot.noteOn(1000);
        //    finchRobot.setLED(255, 0, 0);
        //    finchRobot.setMotors(255, 255);
        //    finchRobot.setLED(0, 0, 255);
        //    finchRobot.wait(500);
        //    finchRobot.noteOff();
        //    finchRobot.setLED(0, 255, 0);

        //    finchRobot.noteOn(750);
        //    finchRobot.setLED(255, 0, 0);
        //    finchRobot.setMotors(-100, 100);
        //    finchRobot.setLED(0, 0, 255);
        //    finchRobot.wait(500);
        //    finchRobot.noteOff();
        //    finchRobot.setLED(0, 255, 0);

        //    finchRobot.noteOn(1250);
        //    finchRobot.setLED(255, 0, 0);
        //    finchRobot.setMotors(100, -100);
        //    finchRobot.setLED(0, 0, 255);
        //    finchRobot.wait(500);
        //    finchRobot.noteOff();
        //    finchRobot.setLED(0, 255, 0);

        //    finchRobot.noteOn(1100);
        //    finchRobot.setLED(255, 0, 0);
        //    finchRobot.setMotors(-255, -255);
        //    finchRobot.setLED(0, 0, 255);
        //    finchRobot.wait(500);
        //    finchRobot.noteOff();
        //    finchRobot.setLED(0, 255, 0);
        //}
        //finchRobot.setMotors(0, 0);
        #endregion

        #region Finch Robot Management
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            DisplayScreenHeader("Disconnect Finch Robot.");

            Console.WriteLine();
            Console.WriteLine("Ready to disconnect the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine();
            Console.WriteLine("Finch robot is now disconnected.");

            DisplayContinuePrompt();
        }

        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            bool finchRobotConnected;

            DisplayScreenHeader("Connect To Finch Robot");

            Console.WriteLine("Ready to connect to the Finch robot. Be sure to connect the USB cable to the robot and computer.");
            DisplayContinuePrompt();

            finchRobotConnected = finchRobot.connect();

            if (finchRobotConnected)
            {

                finchRobot.setLED(0, 255, 0);
                finchRobot.noteOn(1500);
                finchRobot.wait(1000);
                finchRobot.noteOff();

                Console.WriteLine();
                Console.WriteLine("Finch robot is now connected.");
                DisplayContinuePrompt();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Unable to connect to the Finch robot.");
                DisplayContinuePrompt();
            }
            return finchRobotConnected;
        }
        #endregion        

        #region Helper Methods
        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        /// <summary>
        /// Display Closing Screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }
        #endregion
    }
}
