using GameCatalog.InputModel;
using GameCatalog.Services;
using GameCatalog.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalog.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Search for all games with a page system
        /// </summary>
        /// <param name="page">Shows what page is being consulted</param>
        /// <param name="quantity">Shows how many registered games are per page. Minimum of 1 and maximum of 50</param>
        /// <response code="200">Returns the list of games</response>
        /// <response code="204">If there are no games</response>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameViewModel>>> Get([FromQuery, Range(1, int.MaxValue)] int page = 1, [FromQuery, Range(1, 50)] int quantity = 4) 
        {
            var games = await _gameService.Get(page, quantity);
            if (games.Count() == 0) 
                return NoContent();
            return Ok(games);
        }

        /// <summary>
        /// Searches a specific game by its id
        /// </summary>
        /// <param name="idGame">Id of the desired game</param>
        /// <response code="200">Returns the desired game</response>
        /// <response code="204">If there is no game with this id</response>
        /// <returns></returns>
        [HttpGet("{idGame:guid}")]
        public async Task<ActionResult<GameViewModel>> Get([FromRoute] Guid idGame)
        {
            var game = await _gameService.Get(idGame);
            if (game == null)
                return NoContent();
            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<GameViewModel>> InsertGame([FromBody]GameInputModel gameInputModel)
        {
            try
            {
                var game = await _gameService.Insert(gameInputModel);
                return Ok(game);
            }
            catch(Exception ex)
            {
                return UnprocessableEntity("A game with this name and developer already exists.");
            }
        }
        [HttpPut]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid idGame, [FromBody] GameInputModel gameInputModel)
        {
            try
            {
                await _gameService.Update(idGame, gameInputModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound("This game does not exist");
            }
        }
        [HttpPatch("{idGame:guid}/price/{price:double}")]
        public async Task<ActionResult> UpdateGame(Guid idGame, [FromRoute] double price)
        {
            try
            {
                await _gameService.Update(idGame, price);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound("This game does not exist");
            }
        }
        [HttpDelete("{idGame:guid}")]
        public async Task<ActionResult> RemoveGame([FromRoute] Guid idGame)
        {
            try
            {
                await _gameService.Remove(idGame);

                return Ok();

            }
            catch (Exception ex)
            {
                return NotFound("This game does not exist");
            }
        }
    }
}
