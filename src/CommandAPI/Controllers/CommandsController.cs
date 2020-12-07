using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using CommandAPI.Models;

namespace CommandAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CommandsController : ControllerBase
  {
    private readonly ICommandRepo _commandRepo;

    public CommandsController(ICommandRepo commandRepo)
    {
      _commandRepo = commandRepo;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<Command>> GetAllCommands()
    {
      IEnumerable<Command> commands = _commandRepo.GetAll();
      return Ok(commands);
    }

    [HttpGet("{id}")]
    public ActionResult<Command> GetCommandById(int id)
    {
      var commandItem = _commandRepo.GetById(id);
      if (commandItem == null)
      {
        return NotFound();
      }
      return Ok(commandItem);
    }
  }
}