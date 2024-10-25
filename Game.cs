using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace game1402_a2_starter
{
    public class GameData
    {
        public string GameName { get; set; } //This is an example of a property; for whatever reason your serializable data objects will need to be written this way
        public string Description { get; set; }
        public List<Room> Rooms { get; set; } //this is only an example. You do not ha
        public List<Item> Items { get; set; }
        public string CurrentLocal {  get; set; }
        public List<string> PickUpWords { get; set; }
        public List<string> MoveWords { get; set; }
        public List<string> UseWords { get; set; }
        public List<string> LookingWords { get; set; }
    }

    public class Game(GameData data)
    {
        private GameData gameData = data;

        public void ProcessString(string enteredString)
        {
            enteredString =
                enteredString.Trim()
                    .ToLower();
            string[]commands = enteredString.Split(" ");
            bool looping = true;

            for (int i = 0;looping && i < gameData.LookingWords.Count;i++)//Investigating a room
            {
                if (commands[0].Contains(gameData.LookingWords[i]))
                {
                    switch(gameData.CurrentLocal)
                    {
                        case "base":
                            looping = false;
                            Console.WriteLine(gameData.Rooms[0].Description);
                            if (!gameData.Items[1].IsCollected)
                            {
                                Console.WriteLine("You see a key on the table");
                            }
                            break;

                        case "enter":
                            looping = false;
                            Console.WriteLine(gameData.Rooms[1].Description);
                            break;
                        case "living":
                            looping = false;
                            Console.WriteLine(gameData.Rooms[2].Description);
                            if (!gameData.Items[0].IsCollected)
                            {
                                Console.WriteLine("Under the couch you find a crowbar");
                            }
                            break;
                        case "kitch":
                            looping = false;
                            Console.WriteLine(gameData.Rooms[3].Description);
                            break;
                    }
                    break;
                }
            }

            for (int i = 0; looping && i < gameData.MoveWords.Count; i++)//Moving towards another room
            {
                if (commands[0].Contains(gameData.MoveWords[i]))
                {
                    for (i = 0; looping && i < gameData.Rooms.Count; i++)
                    {
                        for (int j = 0; looping && commands.Length > 0; j++)
                        {
                            if (commands[j].Contains(gameData.Rooms[i].Reference))//If the user mentions the command
                            {
                                if (gameData.Rooms[i].IsUnlocked)
                                {
                                    Console.WriteLine("You moved to the "+gameData.Rooms[i].Name);
                                    gameData.CurrentLocal = gameData.Rooms[i].Reference;
                                    looping = false;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("That room is locked");
                                    looping = false;
                                    break;
                                }
                            }
                        }

                        if (!looping)//Checking if the room has alreayd been matched
                        {
                            break;
                        }
                    }

                    if (looping)//If the no words have been found
                    {
                        looping = false;
                        Console.WriteLine("Unknown room with that name");
                    }
                }
            }
        }

    }

}
