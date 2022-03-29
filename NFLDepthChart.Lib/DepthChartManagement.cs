using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFLDepthChart.Lib
{
    public interface IDepthChartManagement
    {
        public DepthChart Chart { get; set; }

        void AddPlayerToDepthChart(string positionName, string playerName, int? positionDepth);
        Player RemovePlayerToDepthChart(string positionName, string playerName);
        IList<KeyValuePair<int, DepthChartItem>> GetBackups(string positionName, string playerName);
        string GetFullDepthChart();
    }

    public class DepthChartManagement : IDepthChartManagement
    {
        public DepthChart Chart { get; set; } = new DepthChart();

        private readonly IFootballPositionRepository _footballPositionRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly int _maxDepthLevel;

        public DepthChartManagement(IFootballPositionRepository footballPositionRepository,
            IPlayerRepository playerRepository,
            int maxDepthLevel = 5)
        {
            _footballPositionRepository = footballPositionRepository;
            _playerRepository = playerRepository;
            _maxDepthLevel = maxDepthLevel;
        }

        public void AddPlayerToDepthChart(string positionName, string playerName, int? positionDepth)
        {
            FootballPosition position = _footballPositionRepository.GetPosition(positionName);
            Player player = _playerRepository.GetPlayerByName(playerName);

            int? lastRank = (int?)Chart.Contents.LastOrDefault().Key ?? 0;
            int lowestRank = lastRank.Value > 0 ? lastRank.Value + 1 : 0;
            int key = positionDepth ?? lowestRank;

            Chart.Contents.Add(new KeyValuePair<int, DepthChartItem>(key, new DepthChartItem()
            {
                Player = player,
                Position = position
            }));
        }

        public Player RemovePlayerToDepthChart(string positionName, string playerName)
        {
            FootballPosition position = _footballPositionRepository.GetPosition(positionName);
            Player player = _playerRepository.GetPlayerByName(playerName);

            var item = Chart.Contents.FirstOrDefault(x =>
                x.Value.Player == player && x.Value.Position == position);

            if (item.Value == null)
                return null;

            Chart.Contents.Remove(item);
            
            return item.Value.Player;
        }

        public IList<KeyValuePair<int, DepthChartItem>> GetBackups(string positionName, string playerName)
        {
            FootballPosition position = _footballPositionRepository.GetPosition(positionName);
            Player player = _playerRepository.GetPlayerByName(playerName);

            var item = Chart.Contents.FirstOrDefault(x =>
                string.Equals(x.Value.Player.Name.Trim(), player.Name.Trim(),
                    StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(x.Value.Position.Name.Trim().ToLowerInvariant(),
                    position.Name.Trim().ToLowerInvariant(), StringComparison.InvariantCultureIgnoreCase));

            return Chart.Contents.Where(x => x.Key > item.Key).ToList();
        }

        public string GetFullDepthChart()
        {
            var contentToPrint = new StringBuilder();

            if (Chart?.Contents == null)
                return null;

            var squads = Chart.Contents.Select(c => c.Value.Position.Squad).Distinct().ToList();

            foreach (var squad in squads)
            {
                contentToPrint.AppendLine(squad);

                var depthChartItems = Chart.Contents.Where(c => c.Value.Position.Squad == squad);

                string lastPositionName = null;

                foreach (var (key, value) in depthChartItems)
                {
                    if (lastPositionName != value.Position.Code)
                    {
                        contentToPrint.AppendLine("--------");
                        lastPositionName = value.Position.Code;
                    }

                    contentToPrint.AppendLine(
                        $"\t|--- Position:{key}, Player: {value.Position.Code} - {value.Player.Number} - {value.Player.Name}");

                }
            }

            return contentToPrint.ToString();
        }

    }
}