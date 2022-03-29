using System;
using System.Net;
using NFLDepthChart.Lib;

namespace NFLDepthChart
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isExit = false;

            var depthChartManagement = new DepthChartManagement (
                new FootballPositionRepository(),
                new PlayerRepository(),
                5
            );

            try
            {
                do
                {
                    Console.WriteLine("============================");
                    Console.WriteLine("Please select from one of the following:");
                    Console.WriteLine("1 - Add Player to Depth Chart");
                    Console.WriteLine("2 - Remove Player from Depth Chart");
                    Console.WriteLine("3 - Get Backups");
                    Console.WriteLine("4 - Get Full Depth Chart");
                    Console.WriteLine("5 - Exit");
                    Console.WriteLine("============================");

                    var selection = Console.ReadLine();

                    string positionCode;
                    string name;
                    string rankPosition;

                    switch (selection)
                    {
                        case "1":
                            Console.WriteLine("Enter Position Name:");
                            positionCode = Console.ReadLine();
                            Console.WriteLine("Enter Player Rank:");
                            rankPosition = Console.ReadLine();

                            if (!int.TryParse(rankPosition, out int rank))
                                Console.WriteLine("Invalid rank");
                            else
                            {
                                Console.WriteLine("Enter Player Name:");
                                name = Console.ReadLine();

                                depthChartManagement.AddPlayerToDepthChart(positionCode, name, rank);

                                Console.WriteLine("Player Added");
                                Console.WriteLine(depthChartManagement.GetFullDepthChart());
                            }

                            isExit = false;

                            break;
                        case "2":
                            Console.WriteLine("Enter Position Name:");
                            positionCode = Console.ReadLine();

                            Console.WriteLine("Enter Player Name:");
                            name = Console.ReadLine();

                            depthChartManagement.RemovePlayerToDepthChart(positionCode, name);

                            Console.WriteLine("Player removed");
                            Console.WriteLine(depthChartManagement.GetFullDepthChart());
                            isExit = false;
                            break;
                        case "3":
                            Console.WriteLine("Enter Position Name:");
                            positionCode = Console.ReadLine();

                            Console.WriteLine("Enter Player Name:");
                            name = Console.ReadLine();

                            depthChartManagement.GetBackups(positionCode, name);
                            isExit = false;
                            break;
                        case "4":
                            var content = depthChartManagement.GetFullDepthChart();
                            Console.WriteLine(content);
                            isExit = false;
                            break;
                        default:
                            isExit = true;
                            break;
                    }
                } while (!isExit);

                if (isExit)
                    Environment.Exit(0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
