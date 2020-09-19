
using HamsterCheese.AmongUsMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YourCheese
{
    class Program
    {
        static int tableWidth = 75;
        static List<PlayerData> playerDatas = new List<PlayerData>();
        static void UpdateCheat()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Test Read Player Datas..");
                PrintRow("offset", "Name", "OwnerId", "PlayerId", "Color", "spawnid", "spawnflag");
                PrintLine();

                foreach (var data in playerDatas)
                {
                    if (data.IsLocalPlayer)
                        Console.ForegroundColor = ConsoleColor.Green;
                    if (data.PlayerInfo.Value.IsDead == 1)
                        Console.ForegroundColor = ConsoleColor.Red;
                    if (data.PlayerInfo.Value.IsImpostor == 1)
                        Console.ForegroundColor = ConsoleColor.Blue;

                    var colorName = "";
                    switch (data.PlayerInfo.Value.ColorId)
                    {
                        case 0:
                            colorName = "Red";
                            break;
                        case 1:
                            colorName = "Blue";
                            break;
                        case 2:
                            colorName = "Green";
                            break;
                        case 3:
                            colorName = "Pink";
                            break;
                        case 4:
                            colorName = "Orange";
                            break;
                        case 5:
                            colorName = "Yellow";
                            break;
                        case 6:
                            colorName = "Black";
                            break;
                        case 7:
                            colorName = "White";
                            break;
                        case 8:
                            colorName = "Purple";
                            break;
                        case 9:
                            colorName = "Brown";
                            break;
                        case 10:
                            colorName = "Cyan";
                            break;
                        case 11:
                            colorName = "Lime";
                            break;
                    }
                    var Name = HamsterCheese.AmongUsMemory.Utils.ReadString(data.PlayerInfo.Value.PlayerName);
                    PrintRow($"{(data.IsLocalPlayer == true ? "Me->" : "")}{data.offset_str}", $"{Name}", $"{data.Instance.OwnerId}", $"{data.Instance.PlayerId}", $"{colorName}", $"{data.Instance.SpawnId}", $"{data.Instance.SpawnFlags}");
                    Console.ForegroundColor = ConsoleColor.White;
                    PrintLine();
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        static void Main(string[] args)
        {
            // Cheat Init
            if (HamsterCheese.AmongUsMemory.Cheese.Init())
            {
                // Update Player Data When Every Game
                HamsterCheese.AmongUsMemory.Cheese.ObserveShipStatus((x) =>
                {
                    foreach (var player in playerDatas)
                    {
                        player.StopObserveState();
                    }

                    playerDatas = HamsterCheese.AmongUsMemory.Cheese.GetAllPlayers();

                    foreach (var player in playerDatas)
                    {
                        player.onDie += (pos, colorId) =>
                        {
                            Console.WriteLine("Player died! Color:" + colorId);
                        };
                        // player state check
                        player.StartObserveState();
                    }
                });

                // Cheat Logic
                CancellationTokenSource cts = new CancellationTokenSource();
                Task.Factory.StartNew(
                    UpdateCheat
                , cts.Token);
            }
            System.Threading.Thread.Sleep(1000000);
        }

        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }
            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}


