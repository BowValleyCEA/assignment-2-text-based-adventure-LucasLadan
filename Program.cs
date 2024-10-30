// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using game1402_a2_starter;

string fileName = "C:/Users/l.ladan419/Documents/Project - Lessons/Git/text adventure/game_data.json";//if you are ever worried about whether your json is valid or not, check out JSON Lint: 

GameData yourGameData;
string jsonString = File.ReadAllText(@fileName);
yourGameData = JsonSerializer.Deserialize<GameData>(jsonString);
Game yourGame = new Game(yourGameData);
Console.WriteLine("You are trapped in the basement of a house, you need to escape");
while (true)
{
    yourGame.ProcessString(Console.ReadLine());
}
