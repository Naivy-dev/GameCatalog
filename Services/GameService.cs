using GameCatalog.Entities;
using GameCatalog.Exceptions;
using GameCatalog.InputModel;
using GameCatalog.Repositories;
using GameCatalog.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalog.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<List<GameViewModel>> Get(int page, int quantity)
        {
            var games = await _gameRepository.Get(page, quantity);

            return games.Select(game => new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                Developer = game.Developer,
                Publisher = game.Publisher,
                Price = game.Price
            }).ToList();
        }

        public async Task<GameViewModel> Get(Guid id)
        {
            var game = await _gameRepository.Get(id);
            if (game == null)
                return null;
            return new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                Developer = game.Developer,
                Publisher = game.Publisher,
                Price = game.Price
            };
        }

        public async Task<GameViewModel> Insert(GameInputModel game)
        {
            var entityGame = await _gameRepository.Get(game.Name, game.Developer);

            if (entityGame.Count > 0)
                throw new GameAlreadyRegisteredException();

            var gameInsert = new Game
            {
                Id = Guid.NewGuid(),
                Name = game.Name,
                Description = game.Description,
                Developer = game.Developer,
                Publisher = game.Publisher,
                Price = game.Price
            };
            await _gameRepository.Insert(gameInsert);

            return new GameViewModel
            {
                Id = gameInsert.Id,
                Name = game.Name,
                Description = game.Description,
                Developer = game.Developer,
                Publisher = game.Publisher,
                Price = game.Price
            };
            
        }

        public async Task Update(Guid id, GameInputModel game)
        {
            var entityGame = await _gameRepository.Get(id);

            if (entityGame == null)
                throw new GameNotRegisteredException();
            entityGame.Name = game.Name;
            entityGame.Description = game.Description;
            entityGame.Developer = game.Developer;
            entityGame.Publisher = game.Publisher;
            entityGame.Price = game.Price;

            await _gameRepository.Update(entityGame);
        }

        public async Task Update(Guid id, double price)
        {
            var entityGame = await _gameRepository.Get(id);

            if (entityGame == null)
                throw new GameNotRegisteredException();
            
            entityGame.Price = price;

            await _gameRepository.Update(entityGame);
        }
        public async Task Remove(Guid id)
        {
            var game = await _gameRepository.Get(id);
            if (game == null)
                throw new GameNotRegisteredException();
            await _gameRepository.Remove(id);
        }
        public void Dispose()
        {
            _gameRepository?.Dispose();
        }
    }
}
