using homework8WEB_API.Moduls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace homework8WEB_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    static List<Game> Games = new List<Game>()
    {
        new Game(){ Id = 0, Title = "Title0", Description = "Description0", Priority = 0, IsCompleted = true},
        new Game(){ Id = 1, Title = "Title1", Description = "Description1", Priority = 1, IsCompleted = false},
        new Game(){ Id = 2, Title = "Title2", Description = "Description2", Priority = 2, IsCompleted = true},
        new Game(){ Id = 3, Title = "Title3", Description = "Description3", Priority = 3, IsCompleted = false},
        new Game(){ Id = 4, Title = "Title4", Description = "Description4", Priority = 4, IsCompleted = true},
        new Game(){ Id = 5, Title = "Title5", Description = "Description5", Priority = 5, IsCompleted = false}
    };

    [ProducesResponseType<string>(200)]
    [ProducesResponseType<string>(404)]
    [ProducesResponseType<string>(409)]
    [ProducesResponseType<string>(500)]
    [HttpPut("{Id}")]
    public ActionResult UpdateTitle([FromRoute] int Id, [FromBody] string Title)
    {
        try
        {
            var gameToUpdate = Games.FirstOrDefault(g => g.Id == Id);

            if(gameToUpdate == null)
            {
                return NotFound("Game with id:" + Id.ToString() + " not found");
            }

            if (gameToUpdate.Title == Title)
            {
                return Conflict("Game with title: " + Title + " already exist");
            }

            gameToUpdate.Title = Title;
            
            return Ok("Game title was updated => new title: " + Title);
        }
        catch(Exception ex)
        {
            return StatusCode(500, "API ERROR: " + ex.Message);
        }
    }

    [ProducesResponseType<string>(200)]
    [ProducesResponseType<string>(404)]
    [ProducesResponseType<string>(409)]
    [ProducesResponseType<string>(500)]
    [HttpPost("{Id}/complete")]
    public ActionResult CompleteGame([FromRoute] int Id)
    {
        try
        {
            var gameToUpdate = Games.FirstOrDefault(g => g.Id == Id);

            if (gameToUpdate == null)
            {
                return NotFound("Game with id:" + Id.ToString() + " not found");
            }

            if (gameToUpdate.IsCompleted)
            {
                return Conflict("Game is already completed");
            }

            gameToUpdate.IsCompleted = true;
            return Ok("Game " + gameToUpdate.Title + " is completed now");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "API ERROR: " + ex.Message);
        }
    }

    [ProducesResponseType<string>(200)]
    [ProducesResponseType<string>(404)]
    [ProducesResponseType<string>(409)]
    [ProducesResponseType<string>(500)]
    [HttpDelete("{Id}")]
    public ActionResult DeleteGame([FromRoute] int Id)
    {
        try
        {
            var gameToDelete = Games.FirstOrDefault(g => g.Id == Id);

            if (gameToDelete == null)
            {
                return NotFound("Game with id:" + Id.ToString() + " not found");
            }

            if(!gameToDelete.IsCompleted && gameToDelete.Priority == 5)
            {
                return Conflict("Game has hightest priority and is still not completed, you can't delete it");
            }

            Games.Remove(gameToDelete);
            return Ok("Game " + gameToDelete.Title + " is successfully deleted");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "API ERROR: " + ex.Message);
        }
    }

    [ProducesResponseType<int>(200)]
    [ProducesResponseType<string>(404)]
    [ProducesResponseType<string>(500)]
    [HttpDelete("completed")]
    public ActionResult DeleteCompletedGames()
    {
        try
        {
            var gamesToDelete = Games.Where(g => g.IsCompleted == true).ToList();

            if (gamesToDelete.Count == 0)
            {
                return NotFound("You have no completed games");
            }

            foreach(var game in gamesToDelete)
            {
                Games.Remove(game);
            }

            return Ok(gamesToDelete.Count);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "API ERROR: " + ex.Message);
        }
    }
}
