using System;
using System.Collections.Generic;
using AutoFixture;
using Moq;
using NFLDepthChart.Lib;
using System.Linq;
using Xunit;

namespace NFLDepthChart.Tests
{
    public class PlayerTests
    {
        private Fixture fixture;
        private Mock<IFootballPositionRepository> _footballPositionRepositoryMock;
        private Mock<IPlayerRepository> _playerRepositoryMock;

        public PlayerTests()
        {
            fixture = new Fixture();
            _footballPositionRepositoryMock = fixture.Create<Mock<IFootballPositionRepository>>();
            _playerRepositoryMock = fixture.Create<Mock<IPlayerRepository>>();
        }

        [Fact]
        public void AddPlayerToDepthChart_InvalidPositionName_ThrowArgumentException()
        {
            var player = new Player()
            {
                Name = "Brady, Tom",
                Number = 12
            };

            _footballPositionRepositoryMock.Setup(f => f.GetPosition(It.IsAny<string>()))
                .Returns((FootballPosition) null);

            _playerRepositoryMock.Setup(p => p.GetPlayerByName(It.IsAny<string>()))
                .Returns(player);

            var management = new DepthChartManagement(_footballPositionRepositoryMock.Object,
                _playerRepositoryMock.Object
            )
            {
                Chart = new DepthChart()
            };

            Assert.Throws<ArgumentException>(() =>
            {
                management.AddPlayerToDepthChart("xxxx", "Tom Brady", null);
            });
        }

        [Fact]
        public void AddPlayerToDepthChart_InvalidPlayerName_ThrowArgumentException()
        {
            var footballPosition = new FootballPosition()
            {
                Code = "QB",
                Name = "Quarterback",
                Squad = "Offense"
            };

            _footballPositionRepositoryMock.Setup(f => f.GetPosition(It.IsAny<string>()))
                .Returns(footballPosition);

            _playerRepositoryMock.Setup(p => p.GetPlayerByName(It.IsAny<string>()))
                .Returns((Player) null);

            var management = new DepthChartManagement(_footballPositionRepositoryMock.Object,
                _playerRepositoryMock.Object
            )
            {
                Chart = new DepthChart()
            };

            Assert.Throws<ArgumentException>(() =>
            {
                management.AddPlayerToDepthChart("Quarterback", "xxxxxx", null);
            });
        }

        [Fact]
        public void AddPlayerToDepthChart_WithFirstRank_FirstRankResult()
        {
            var footballPosition = new FootballPosition()
            {
                Code = "QB",
                Name = "Quarterback",
                Squad = "Offense"
            };

            var player = new Player()
            {
                Name = "Brady, Tom",
                Number = 12
            };

            var expectedResultItem = new DepthChartItem()
            {
                Player = player,
                Position = footballPosition
            };

            _footballPositionRepositoryMock.Setup(f => f.GetPosition(It.IsAny<string>()))
                .Returns(footballPosition);

            _playerRepositoryMock.Setup(p => p.GetPlayerByName(It.IsAny<string>()))
                .Returns(player);

            var management = new DepthChartManagement(_footballPositionRepositoryMock.Object,
                _playerRepositoryMock.Object
            )
            {
                Chart = new DepthChart()
            };

            management.AddPlayerToDepthChart("QB", "Tom Brady", 1);

            _footballPositionRepositoryMock.Verify(f => f.GetPosition(It.IsAny<string>()),
                Times.Once);
            _playerRepositoryMock.Verify(p => p.GetPlayerByName(It.IsAny<string>()),
                Times.Once);

            Assert.NotEmpty(management.Chart.Contents);

            Assert.True(management.Chart.Contents.FirstOrDefault().Key == 1);
            Assert.True(management.Chart.Contents.FirstOrDefault().Value.Player == player);
            Assert.True(management.Chart.Contents.FirstOrDefault().Value.Position == footballPosition);
        }

        [Fact]
        public void AddPlayerToDepthChart_WithNoRank_LastRankResult()
        {
            var chart = new DepthChart();

            var footballPosition = new FootballPosition()
            {
                Code = "QB",
                Name = "Quarterback",
                Squad = "Offense"
            };

            var player1 = new Player()
            {
                Name = "Brady, Tom",
                Number = 12
            };

            var player2 = new Player()
            {
                Name = "Doe, John",
                Number = 13
            };

            var player3 = new Player()
            {
                Name = "Stark, Tony",
                Number = 10
            };

            _footballPositionRepositoryMock.Setup(f => f.GetPosition(It.IsAny<string>()))
                .Returns(footballPosition);

            // add player 1
            _playerRepositoryMock.Setup(p => p.GetPlayerByName(It.IsAny<string>()))
                .Returns(player1);

            var management = new DepthChartManagement(_footballPositionRepositoryMock.Object,
                _playerRepositoryMock.Object
            )
            {
                Chart = chart
            };

            management.AddPlayerToDepthChart("QB", "Tom Brady", 1);

            // add player 2
            _playerRepositoryMock.Setup(p => p.GetPlayerByName(It.IsAny<string>()))
                .Returns(player2);

            management = new DepthChartManagement(_footballPositionRepositoryMock.Object,
                _playerRepositoryMock.Object
            )
            {
                Chart = chart
            };
            management.AddPlayerToDepthChart("QB", "John Doe", 2);

            // add player 3
            _playerRepositoryMock.Setup(p => p.GetPlayerByName(It.IsAny<string>()))
                .Returns(player3);

            management = new DepthChartManagement(_footballPositionRepositoryMock.Object,
                _playerRepositoryMock.Object
            )
            {
                Chart = chart
            };
            management.AddPlayerToDepthChart("QB", "Tony Stark", null);

            Assert.NotEmpty(chart.Contents);

            Assert.True(management.Chart.Contents.FirstOrDefault().Key == 1);
            Assert.True(management.Chart.Contents.FirstOrDefault().Value.Player == player1);
            Assert.True(management.Chart.Contents.FirstOrDefault().Value.Position == footballPosition);

            Assert.True(management.Chart.Contents.LastOrDefault().Key == 3);
            Assert.True(management.Chart.Contents.LastOrDefault().Value.Player == player3);
            Assert.True(management.Chart.Contents.LastOrDefault().Value.Position == footballPosition);
        }

        [Fact]
        public void AddPlayerToDepthChart_WithDuplicateRank_HasTwoItemsWithSameRank()
        {
            var chart = new DepthChart();

            var footballPosition = new FootballPosition()
            {
                Code = "QB",
                Name = "Quarterback",
                Squad = "Offense"
            };

            var player1 = new Player()
            {
                Name = "Brady, Tom",
                Number = 12
            };

            var player2 = new Player()
            {
                Name = "Doe, John",
                Number = 13
            };

            var player3 = new Player()
            {
                Name = "Stark, Tony",
                Number = 10
            };

            _footballPositionRepositoryMock.Setup(f => f.GetPosition(It.IsAny<string>()))
                .Returns(footballPosition);

            // add player 1
            _playerRepositoryMock.Setup(p => p.GetPlayerByName(It.IsAny<string>()))
                .Returns(player1);

            var management = new DepthChartManagement(_footballPositionRepositoryMock.Object,
                _playerRepositoryMock.Object
            )
            {
                Chart = chart
            };

            management.AddPlayerToDepthChart("QB", "Tom Brady", 1);

            // add player 2
            _playerRepositoryMock.Setup(p => p.GetPlayerByName(It.IsAny<string>()))
                .Returns(player2);

            management = new DepthChartManagement(_footballPositionRepositoryMock.Object,
                _playerRepositoryMock.Object
            )
            {
                Chart = chart
            };
            management.AddPlayerToDepthChart("QB", "John Doe", 1);

            // add player 3
            _playerRepositoryMock.Setup(p => p.GetPlayerByName(It.IsAny<string>()))
                .Returns(player3);

            management = new DepthChartManagement(_footballPositionRepositoryMock.Object,
                _playerRepositoryMock.Object
            )
            {
                Chart = chart
            };
            management.AddPlayerToDepthChart("QB", "Tony Stark", null);

            var tally = chart.Contents.Count(x => x.Key == 1);
            var players = chart.Contents.Where(x => x.Key == 1);

            Assert.NotEmpty(chart.Contents);
            Assert.Equal(2, tally);
            Assert.NotEmpty(players);
        }

        [Fact]
        public void RemovePlayer()
        {
            var chart = new DepthChart();

            var footballPosition = new FootballPosition()
            {
                Code = "QB",
                Name = "Quarterback",
                Squad = "Offense"
            };

            var player1 = new Player()
            {
                Name = "Brady, Tom",
                Number = 12
            };

            _footballPositionRepositoryMock.Setup(f => f.GetPosition(It.IsAny<string>()))
                .Returns(footballPosition);

            // add player 1
            _playerRepositoryMock.Setup(p => p.GetPlayerByName(It.IsAny<string>()))
                .Returns(player1);

            var management = new DepthChartManagement(_footballPositionRepositoryMock.Object,
                _playerRepositoryMock.Object
            )
            {
                Chart = chart
            };

            management.AddPlayerToDepthChart("QB", "Tom Brady", 1);


            var result = management.RemovePlayerToDepthChart("QB", "Tom Brady");

            Assert.Empty(chart.Contents);
            Assert.NotNull(result);
            Assert.Equal(result.Name, player1.Name);
        }

        [Fact]
        public void RemovePlayer_NonExistentPlayer_ReturnNull()
        {
            var chart = new DepthChart();

            var footballPosition = new FootballPosition()
            {
                Code = "QB",
                Name = "Quarterback",
                Squad = "Offense"
            };

            var player1 = new Player()
            {
                Name = "Brady, Tom",
                Number = 12
            };

            _footballPositionRepositoryMock.Setup(f => f.GetPosition(It.IsAny<string>()))
                .Returns(footballPosition);

            // add player 1
            _playerRepositoryMock.Setup(p => p.GetPlayerByName(It.IsAny<string>()))
                .Returns(player1);

            var management = new DepthChartManagement(_footballPositionRepositoryMock.Object,
                _playerRepositoryMock.Object
            )
            {
                Chart = chart
            };

            management.RemovePlayerToDepthChart("QB", "John Doe");

            Assert.Empty(chart.Contents);
        }

        [Fact]
        public void GetBackups_ReturnsTwoResultsExcludingSearchedPlayer()
        {
            var footballPosition = new FootballPosition()
            {
                Code = "QB",
                Name = "Quarterback",
                Squad = "Offense"
            };

            var player1 = new Player()
            {
                Name = "Brady, Tom",
                Number = 12
            };

            var player2 = new Player()
            {
                Name = "Doe, John",
                Number = 13
            };

            var player3 = new Player()
            {
                Name = "Stark, Tony",
                Number = 10
            };

            var chart = new DepthChart
            {
                Contents = new List<KeyValuePair<int, DepthChartItem>>()
                {
                    new KeyValuePair<int, DepthChartItem>(1, new DepthChartItem()
                    {
                        Player = player1,
                        Position = footballPosition
                    }),
                    new KeyValuePair<int, DepthChartItem>(2, new DepthChartItem()
                    {
                        Player = player2,
                        Position = footballPosition
                    }),
                    new KeyValuePair<int, DepthChartItem>(3, new DepthChartItem()
                    {
                        Player = player3,
                        Position = footballPosition
                    }),
                }
            };

            _footballPositionRepositoryMock.Setup(f => f.GetPosition(It.IsAny<string>()))
                .Returns(footballPosition);

            // add player 1
            _playerRepositoryMock.Setup(p => p.GetPlayerByName(It.IsAny<string>()))
                .Returns(player1);

            var management = new DepthChartManagement(_footballPositionRepositoryMock.Object,
                _playerRepositoryMock.Object
            )
            {
                Chart = chart
            };

            var result = management.GetBackups("QB", "Tom Brady");


            Assert.NotEmpty(result);
            Assert.True(result.Count == 2);
            Assert.True(result.Any(x => x.Value.Player.Name != player1.Name));
        }

        [Fact]
        public void GetFullDepthChart_ReturnsString()
        {
            var footballPosition1 = new FootballPosition()
            {
                Code = "QB",
                Name = "Quarterback",
                Squad = "Offense"
            };

            var footballPosition2 = new FootballPosition()
            {
                Code = "C",
                Name = "Center",
                Squad = "Offense"
            };

            var player1 = new Player()
            {
                Name = "Brady, Tom",
                Number = 12
            };

            var player2 = new Player()
            {
                Name = "Doe, John",
                Number = 13
            };

            var player3 = new Player()
            {
                Name = "Stark, Tony",
                Number = 10
            };

            var chart = new DepthChart
            {
                Contents = new List<KeyValuePair<int, DepthChartItem>>()
                {
                    new KeyValuePair<int, DepthChartItem>(1, new DepthChartItem()
                    {
                        Player = player1,
                        Position = footballPosition1
                    }),
                    new KeyValuePair<int, DepthChartItem>(2, new DepthChartItem()
                    {
                        Player = player2,
                        Position = footballPosition1
                    }),
                    new KeyValuePair<int, DepthChartItem>(3, new DepthChartItem()
                    {
                        Player = player3,
                        Position = footballPosition1
                    }),
                    new KeyValuePair<int, DepthChartItem>(1, new DepthChartItem()
                    {
                        Player = player1,
                        Position = footballPosition2
                    }),
                    new KeyValuePair<int, DepthChartItem>(2, new DepthChartItem()
                    {
                        Player = player2,
                        Position = footballPosition2
                    }),
                    new KeyValuePair<int, DepthChartItem>(3, new DepthChartItem()
                    {
                        Player = player3,
                        Position = footballPosition2
                    }),
                }
            };

            _footballPositionRepositoryMock.Setup(f => f.GetPosition(It.IsAny<string>()))
                .Returns(footballPosition1);

            _playerRepositoryMock.Setup(p => p.GetPlayerByName(It.IsAny<string>()))
                .Returns(player1);

            var management = new DepthChartManagement(_footballPositionRepositoryMock.Object,
                _playerRepositoryMock.Object
            )
            {
                Chart = chart
            };

            var result = management.GetFullDepthChart();

            Assert.NotEmpty(result);
        }
    };
}
