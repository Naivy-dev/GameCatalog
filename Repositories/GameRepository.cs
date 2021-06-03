using GameCatalog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalog.Repositories
{
    public class GameRepository : IGameRepository
    {
        private static Dictionary<Guid, Game> games = new Dictionary<Guid, Game>()
        {
            {Guid.Parse("b8db4758-03f7-4b5c-b279-a3d5061ac3c7"), new Game { Id = Guid.Parse("b8db4758-03f7-4b5c-b279-a3d5061ac3c7"),
                Name = "Kingdom Hearts III", Developer = "Square Enix", Publisher = "Square Enix", Price = 250,
                Description = "Sequel to Kingdom Hearts II"}},
            {Guid.Parse("91f682a0-5063-4292-bc0e-dba11acd8c09"), new Game { Id = Guid.Parse("91f682a0-5063-4292-bc0e-dba11acd8c09"),
                Name = "A Hat in Time", Developer = "Gears for Breakfast", Publisher = "Gears for Breakfast", Price = 55,
                Description = "Cute 3D Platform Game"}},
            {Guid.Parse("06dcaac9-07b6-4aa4-aebd-ab832c86a1b4"), new Game { Id = Guid.Parse("06dcaac9-07b6-4aa4-aebd-ab832c86a1b4"),
                Name = "Death Stranding", Developer = "KOJIMA PRODUCTIONS", Publisher = "505 Games", Price = 240,
                Description = "A stranding genre game"}},
            {Guid.Parse("0c1bbd1a-9627-4432-adb2-69d710dad788"), new Game { Id = Guid.Parse("0c1bbd1a-9627-4432-adb2-69d710dad788"),
                Name = "Borderlands 3", Developer = "Gearbox", Publisher = "2k", Price = 120,
                Description = "Sequel to Borderlands 2"}}
        };

        public Task<List<Game>> Get(int page, int quantity)
        {
            return Task.FromResult(games.Values.Skip((page - 1) * quantity).Take(quantity).ToList());
        }

        public Task<Game> Get(Guid id)
        {
            if (!games.ContainsKey(id))
                return null;
            return Task.FromResult(games[id]);
        }

        public Task<List<Game>> Get(string name, string developer)
        {
            return Task.FromResult(games.Values.Where(game => game.Name.Equals(name) && game.Developer.Equals(developer)).ToList());
        }

        public Task<List<Game>> GetWithoutLambda(string name, string developer)
        {
            var returns = new List<Game>();

            foreach(var game in games.Values)
            {
                if (game.Name.Equals(name) && game.Developer.Equals(developer))
                    returns.Add(game);
            }

            return Task.FromResult(returns);
        }


        public Task Insert(Game game)
        {
            games.Add(game.Id, game);
            return Task.CompletedTask;
        }

        public Task Remove(Guid id)
        {
            games.Remove(id);
            return Task.CompletedTask;
        }

        public Task Update(Game game)
        {
            games[game.Id] = game;
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            
        }
    }
}
