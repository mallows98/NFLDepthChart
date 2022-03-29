using System;
using System.Collections.Generic;
using System.Linq;

namespace NFLDepthChart.Lib
{
    public class FootballPosition
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Squad { get; set; }
    }

    public interface IFootballPositionRepository
    {
        FootballPosition GetPosition(string position);
    }

    public class FootballPositionRepository : IFootballPositionRepository
    {
        public FootballPosition GetPosition(string position)
        {
            var result = Generate()
                .FirstOrDefault(p => string.Equals(p.Code.Trim().ToLowerInvariant(), position.Trim().ToLowerInvariant(),
                    StringComparison.InvariantCultureIgnoreCase));

            if (result == null)
                throw new ArgumentException("Invalid position");

            return result;
        }

        public IList<FootballPosition> GetPositionsBySquad(string squad)
        {
            return Generate()
                .Where(s => string.Equals(s.Squad.Trim().ToLowerInvariant(), squad.Trim().ToLowerInvariant(),
                    StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        public IList<FootballPosition> GetAllPositions()
        {
            return Generate();
        }


        private IList<FootballPosition> Generate()
        {
            return new List<FootballPosition>()
            {
                // offense
                new FootballPosition()
                {
                    Code = "C",
                    Name = "Center",
                    Squad = "Offense"
                },
                new FootballPosition()
                {
                    Code = "LOG",
                    Name = "Left Offensive Guard",
                    Squad = "Offense"
                },
                new FootballPosition()
                {
                    Code = "ROG",
                    Name = "Right Offensive Guard",
                    Squad = "Offense"
                },
                new FootballPosition()
                {
                    Code = "LOT",
                    Name = "Left Offensive Tackle",
                    Squad = "Offense"
                },
                new FootballPosition()
                {
                    Code = "ROT",
                    Name = "Right Offensive Tackle",
                    Squad = "Offense"
                },
                new FootballPosition()
                {
                    Code = "QB",
                    Name = "Quarterback",
                    Squad = "Offense"
                },
                new FootballPosition()
                {
                    Code = "RB",
                    Name = "Running Back",
                    Squad = "Offense"
                },
                new FootballPosition()
                {
                    Code = "FB",
                    Name = "Full Back",
                    Squad = "Offense"
                },
                new FootballPosition()
                {
                    Code = "WR",
                    Name = "Wide Receiver",
                    Squad = "Offense"
                },
                new FootballPosition()
                {
                    Code = "TE",
                    Name = "Tight end",
                    Squad = "Offense"
                },

                // defense
                new FootballPosition()
                {
                    Code = "NT",
                    Name = "Nose tackle",
                    Squad = "Defense"
                },
                new FootballPosition()
                {
                    Code = "DT",
                    Name = "Defensive tackle",
                    Squad = "Defense"
                },
                new FootballPosition()
                {
                    Code = "DE",
                    Name = "Defensive End",
                    Squad = "Defense"
                },
                new FootballPosition()
                {
                    Code = "MLB",
                    Name = "Middle Line Backer",
                    Squad = "Defense"
                },
                new FootballPosition()
                {
                    Code = "LOLB",
                    Name = "Left Outside Line Backer",
                    Squad = "Defense"
                },
                new FootballPosition()
                {
                    Code = "ROLB",
                    Name = "Right Outside Line Backer",
                    Squad = "Defense"
                },
                new FootballPosition()
                {
                    Code = "CB",
                    Name = "Corner Back",
                    Squad = "Defense"
                },
                new FootballPosition()
                {
                    Code = "FS",
                    Name = "Free Safety",
                    Squad = "Defense"
                },
                new FootballPosition()
                {
                    Code = "SS",
                    Name = "Strong Safety",
                    Squad = "Defense"
                },

                // special teams
                new FootballPosition()
                {
                    Code = "K",
                    Name = "Kicker",
                    Squad = "Special Teams"
                },
                new FootballPosition()
                {
                    Code = "P",
                    Name = "Punter",
                    Squad = "Special Teams"
                },
                new FootballPosition()
                {
                    Code = "LS",
                    Name = "Long Snapper",
                    Squad = "Special Teams"
                },
                new FootballPosition()
                {
                    Code = "H",
                    Name = "Holder",
                    Squad = "Special Teams"
                },
                new FootballPosition()
                {
                    Code = "KR",
                    Name = "Kick Returner",
                    Squad = "Special Teams"
                },
                new FootballPosition()
                {
                    Code = "PR",
                    Name = "Punt Returner",
                    Squad = "Special Teams"
                },

                // reserves
                new FootballPosition()
                {
                    Code = "RES",
                    Name = "Reserve",
                    Squad = "Reserves"
                },
                new FootballPosition()
                {
                    Code = "FUT",
                    Name = "Future",
                    Squad = "Reserves"
                },
            };
        }
    }
}