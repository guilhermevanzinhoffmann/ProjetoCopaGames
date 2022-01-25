using CopaGamesBackEnd.Interfaces;
using CopaGamesBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CopaGamesBackEnd.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        
        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Game>>> GetAllAsync()
        {
            try
            {
                var request = await _gameService.GetGamesAsync();

                if (request.IsSuccessStatusCode)
                {
                    var  result = request.Content.ReadFromJsonAsync<IEnumerable<Game>>();

                    var games = result.Result;
                    
                    if(games != null && !games.Any())     
                        return NoContent();

                    return Ok(games);
                }
                else if(request.StatusCode == System.Net.HttpStatusCode.NotFound )
                {
                    return NotFound();
                }
                else { 
                    return new StatusCodeResult((int)HttpStatusCode.InternalServerError); 
                }
                    
            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);

            }
        }

        [HttpPost("result")]
        public ActionResult<IEnumerable<Game>> Result([FromBody]IEnumerable<Game> games)
        {
            try
            {
                var result = _gameService.GetResult(games);

                if (!result.Any())
                    return NoContent();
                
                return Ok(result);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(e.Message);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}