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
        public string CurrentLocal { get; set; }
        public List<string> PickUpWords { get; set; }
        public List<string> MoveWords { get; set; }
        public List<string> UseWords { get; set; }
        public List<string> LookingWords { get; set; }
        public List<string> UnlockWords { get; set; }
    }

    public class Game(GameData data)
    {
        private GameData gameData = data;

        public void ProcessString(string enteredString)
        {
            enteredString = enteredString.Trim().ToLower();
            string[] commands = enteredString.Split(" ");

            //Investigating a room
            for (int i = 0; i < gameData.LookingWords.Count; i++)
            {
                if (commands[0].Contains(gameData.LookingWords[i]))//If the user entered a investigating key word
                {
                    switch (gameData.CurrentLocal)
                    {
                        case "base"://Currently in the basement
                            Console.WriteLine(gameData.Rooms[0].Description);
                            if (!gameData.Items[1].IsCollected)//If the key isn't picked up yet
                            {
                                Console.WriteLine("You see a key on the table");
                            }
                            break;

                        case "enter"://Currently in the enterance
                            Console.WriteLine(gameData.Rooms[1].Description);
                            if (!gameData.Items[0].IsCollected)//If the crowbar isn't picked up yet
                            {
                                Console.WriteLine("Under the couch you find a crowbar");
                            }
                            break;

                        case "kitch"://Currently in the kitchen
                            Console.WriteLine(gameData.Rooms[2].Description);
                            break;

                        default:
                            Console.WriteLine("Somehow you ended up in a room that shouldn't exist");
                            break;
                    }
                    return;
                }
            }

            //Moving towards another room
            for (int i = 0; i < gameData.MoveWords.Count; i++)
            {
                if (commands[0].Contains(gameData.MoveWords[i]))//If the user enters a movement key word
                {
                    for (i = 0; i < commands.Length; i++)
                    {
                        for (int j = 0; j < gameData.Rooms.Count; j++)//I have no other ideas on how to do this without a triple for loop
                        {
                            if (commands[i].Contains(gameData.Rooms[j].Reference))//If the user mentions the room
                            {
                                if (gameData.Rooms[j].IsUnlocked)//If the room is unlocked
                                {
                                    Console.WriteLine("You moved to the " + gameData.Rooms[j].Name);
                                    if (gameData.Rooms[j].Reference == "out")
                                    {
                                        Console.WriteLine("Congrats you escaped");
                                    }
                                    gameData.CurrentLocal = gameData.Rooms[j].Reference;
                                    return;
                                }
                                else//If the room is locked
                                {
                                    Console.WriteLine("That room is locked");
                                    return;
                                }
                            }
                        }
                    }

                    Console.WriteLine("Unknown room with that name");
                    return;
                }
            }

            //Picking up an item
            for (int i = 0; i < gameData.PickUpWords.Count; i++)
            {
                if (commands[0].Contains(gameData.PickUpWords[i]))
                {
                    for (i = 0; i < commands.Length; i++)
                    {
                        for (int j = 0; j < gameData.Items.Count; j++)
                        {
                            if (commands[i].Contains(gameData.Items[j].Reference))//If the user mention a item
                            {
                                if (!gameData.Items[j].IsCollected)//If they already have the item
                                {
                                    if (gameData.Items[j].Location == gameData.CurrentLocal)//If they're in the room with that item
                                    {
                                        Console.WriteLine("You got the " + gameData.Items[j].Name);
                                        gameData.Items[j].IsCollected = true;
                                        return;
                                    }
                                    else
                                    {
                                        Console.WriteLine("You aren't in the room with that item");
                                        return;
                                    }
                                }
                                else//If the room is locked
                                {
                                    Console.WriteLine("You already have " + gameData.Items[j].Name);
                                    return;
                                }
                            }
                        }
                    }

                    Console.WriteLine("Unknown item with that name");
                    return;
                }
            }

            //Using an item
            for (int i = 0; i < gameData.UseWords.Count; i++)
            {
                if (commands[0].Contains(gameData.UseWords[i]))
                {
                    for (i = 0; i < commands.Length; i++)
                    {
                        for (int j = 0; j < gameData.Items.Count; j++)
                        {
                            if (commands[i].Contains(gameData.Items[j].Reference))//If the user mention a item
                            {
                                if (gameData.Items[j].IsCollected)//If they have the item
                                {
                                    if (gameData.Items[j].UseLocation == gameData.CurrentLocal)//If they're in the room to use this item
                                    {
                                        for (int k = 0; k < gameData.Rooms.Count; k++)
                                        {
                                            if (gameData.Items[j].UnlockRoom == gameData.Rooms[k].Reference)//Looking for the room thats supposed to be unlocked
                                            {
                                                gameData.Rooms[k].IsUnlocked = true;
                                                Console.WriteLine("You unlocked the way to the " + gameData.Rooms[k].Name);
                                                return;
                                            }
                                        }
                                    }
                                    else//If the player is in the wrong position
                                    {
                                        Console.WriteLine("You aren't in the room that needs that item");
                                        return;
                                    }
                                }
                                else//If the room is locked
                                {
                                    Console.WriteLine("You don't have that item");
                                    return;
                                }
                            }
                        }
                    }

                    Console.WriteLine("Unknown item with that name");
                    return;
                }
            }


            //Unlocking a room
            for (int i = 0; i < gameData.UnlockWords.Count; i++)
            {
                if (commands[0].Contains(gameData.UnlockWords[i]))
                {
                    for (i = 0; i < commands.Length; i++)
                    {
                        for (int j = 0; j < gameData.Rooms.Count; j++)
                        {
                            if (commands[i].Contains(gameData.Rooms[j].Reference))//If the user mentions a room
                            {
                                if (!gameData.Rooms[j].IsUnlocked)//If the room is already unlocked
                                {
                                    for (int k = 0; k < gameData.Items.Count; k++)
                                    {
                                        if (gameData.Items[k].IsCollected)//Looking for the item that's supposed to be used
                                        {
                                            if (gameData.Items[k].UnlockRoom == gameData.Rooms[j].Reference)
                                            {
                                                gameData.Rooms[j].IsUnlocked = true;
                                                Console.WriteLine("You unlocked the way to the " + gameData.Rooms[j].Name);
                                                return;
                                            }
                                        }
                                    }
                                    Console.WriteLine("You don't have the necessary item");
                                    return;
                                }
                                else//If the room is locked
                                {
                                    Console.WriteLine("That room is already unlocked");
                                    if (gameData.Rooms[j].Reference == "base")
                                    {
                                        Console.WriteLine("The door you want to unlock is the enterance door");
                                    }
                                    if (gameData.Rooms[j].Reference == "enter")
                                    {
                                        Console.WriteLine("The door you want to unlock is the outside door");
                                    }
                                    return;
                                }
                            }
                        }
                    }

                    Console.WriteLine("Unknown room with that name");
                    return;
                }
            }
            Console.WriteLine("Unknown command");
        }
    }

}


