// See https://aka.ms/new-console-template for more information
using System;
using System.ComponentModel.Design;
using System.Data.Common;


namespace Zork
{
    class Program
    {

        private static string CurrentRoom
        {
            get
            {
                return Rooms[Location.Row, LocationColumn];
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");
            InitializeRoomDescriptions();

            Room previousRoom = null;

            if (previousRoom != CurrentRoom)
            {
                Console.WriteLine(CurrentRoom.Description);
                previousRoom = CurrentRoom;
            }

            Commands command = Commands.UNKNOWN;
            while (command != Commands.QUIT)
            {
                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());

                string outputString;
                switch (command)
                {
                    case Commands.QUIT:
                        outputString = "Thank you for playing!";
                        break;

                    case Commands.LOOK:
                        outputString = "This is an open field west of a white house, with a boarded front door. \nA rubber mat saying 'Welcome to Zork!' lies by the door.";
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        outputString = $"You moved {command}.";
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Console.WriteLine(outputString);
                Console.WriteLine(CurrentRoom);
            }


            /*string inputString = Console.ReadLine();
            inputString = inputString.ToUpper();
            Commands command = ToCommand(inputString.Trim().ToUpper());
            Console.WriteLine(command);*/
        }

        private static bool Move(Commands command)
        {
            Assert.IsTrue(IsDirection(command), "Invalid direction.");

            bool isValidMove = true;
            switch (command)
            {
                case Commands.NORTH when Location.Row < Rooms.GetLength(0) - 1:
                    Location.Row++;
                    break;

                case Commands.SOUTH when Location.Row > 0:
                    Location.Row--;
                    break;

                case Commands.EAST when Location.Column < Rooms.GetLength(1) - 1:
                    Location.Column++;
                    break;

                case Commands.WEST when Location.Column > 0:
                    Location.Column--;
                    break;

                default:
                    isValidMove = false;
                    break;

                return isValidMove;
            }
        }

        private static Commands ToCommand(string commandString)
        {
            return Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;

            /*Commands command;

            if (commandString == "QUIT")
            {
                command = Commands.QUIT;
            }
            else if (commandString == "LOOK")
            {
                command = Commands.LOOK;
            }
            else if (commandString == "NORTH")
            {
                command = Commands.NORTH;
            }
            else if (commandString == "SOUTH")
            {
                command = Commands.SOUTH;
            }
            else if (commandString == "EAST")
            {
                command = Commands.EAST;
            }
            else if (commandString == "WEST")
            {
                command = Commands.WEST;
            }
            else
            {
                command = Commands.UNKNOWN;
            }

            return command;*/
        }

        private static bool IsDirection(Commands command) => Directions.Contains(command);

        private static readonly Room[,] Rooms =
        {
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            {new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            {new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };

        private static readonly List<Commands> Directions = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST,
        };

        private static (int Row, int Column) Location = (1, 1);

        private static void InitializeRoomDescriptions()
        {
            /*Rooms[0, 0].Description = "You are on a rock-strewn trail.";
            Rooms[0, 1].Description = "You are facing the south side of a white house. There is no door here, and all the windows are barred.";
            Rooms[0, 2].Description = "You are at the top of the Great Canyon on its south wall";

            Rooms[1, 0].Description = "This is a forest, with trees in all directions around you.";
            Rooms[1, 1].Description = "This is an open field west of a white house, with a boarded front door.";
            Rooms[1, 2].Description = "You are behind the white house. In one corner of the house there is a small window which is slightly ajar.";

            Rooms[2, 0].Description = "This is a dimly lit forest, with large trees all around. To the east, there appears to be sunlight.";
            Rooms[2, 1].Description = "You are facing the north side of the white house. There is no door here, and all of the windows are barred.";
            Rooms[2, 2].Description = "You are in a clearing, with a forest surrounding you on the west and south.";*/

            const string fieldDelimiter = '##';
            const int expectedFieldCount = 2;

            string[] lines = File.ReadAllLines(roomsFilename);
            foreach (string line in lines)
            {
                string[] fields = line.Split(fieldDelimiter);
                if (fields.Length != expectedFieldCount)
                {
                    throw new InvalidDataException("Invalid record.");
                }

                string name = fields[(int)Fields.Name];
                string description = fields[(int)Fields.Description];

                RoomMap[name].Desription = description;
            }
        }

        private enum Fields
        {
            Name = 0,
            Description
        }

        public static Room CurrentRoom
        {
            get
            {
                return Rooms[Location.Row, Location.Column];
            }
        }

        static Program()
        {
            RoomMap = new Dictionary<string, Room>();
            foreach (Room room in Rooms)
            {
                RoomMap[room.Name] = room;
            }
        }
    }
}
