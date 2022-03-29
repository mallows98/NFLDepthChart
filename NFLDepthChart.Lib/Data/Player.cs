using System;
using System.Collections.Generic;
using System.Linq;

namespace NFLDepthChart.Lib
{
    public class Player
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }

    public interface IPlayerRepository
    {
        Player GetPlayerByName(string name);
        IList<Player> GetAll();
    }

    public class PlayerRepository : IPlayerRepository
    {
        public Player GetPlayerByName(string name)
        {
            return Generate()
                .FirstOrDefault(p => string.Equals(p.Name.Trim().ToLowerInvariant(), name.Trim().ToLowerInvariant(),
                    StringComparison.InvariantCultureIgnoreCase));
        }

        public IList<Player> GetAll()
        {
            return Generate();
        }

        private IList<Player> Generate()
        {
            return new List<Player>()
            {
                new Player()
                {
                    Name = "Evans, Mike",
                    Number = 13
                },

                new Player()
                {
                    Name = "Johnson, Tyler",
                    Number = 18
                },

                new Player()
                {
                    Name = "Smith, Donovan",
                    Number = 76
                },

                new Player()
                {
                    Name = "Marpet, Ali",
                    Number = 74
                },

                new Player()
                {
                    Name = "Brady, Tom",
                    Number = 12
                },
            };
        }
    }
}

