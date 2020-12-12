using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using CommandAPI.Models;
using AutoMapper;
using CommandAPI.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace CommandAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CommandsController : ControllerBase
  {
    // comment to test auto build pipeline
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepo commandRepo, IMapper mapper)
    {
      _commandRepo = commandRepo;
      _mapper = mapper;
    }
    
    [Authorize]
    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
    {
      IEnumerable<Command> commands = _commandRepo.GetAll();
      return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{id}", Name="GetCommandById")]
    public ActionResult<CommandReadDto> GetCommandById(int id)
    {
      var commandItem = _commandRepo.GetById(id);
      if (commandItem == null)
      {
        return NotFound();
      }
      return Ok(_mapper.Map<CommandReadDto>(commandItem));
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
    {
      Command commandModel = _mapper.Map<Command>(commandCreateDto);
      _commandRepo.Create(commandModel);
      _commandRepo.SaveChanges();
      CommandReadDto commandReadDto = _mapper.Map<CommandReadDto>(commandModel);
      return CreatedAtRoute(nameof(GetCommandById),
        new {Id = commandReadDto.Id}, commandReadDto);
    }

    [HttpPatch("{id}")]
    public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
    {  
      var commandModelFromRepo = _commandRepo.GetById(id);
      if (commandModelFromRepo == null)
      {
        return NotFound();
      }
      var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
      patchDoc.ApplyTo(commandToPatch, ModelState);
      if (!TryValidateModel(commandToPatch))
      {
        return ValidationProblem(ModelState);
      }
      _mapper.Map(commandToPatch, commandModelFromRepo);
      _commandRepo.SaveChanges();
      return RedirectToAction(nameof(GetCommandById), commandModelFromRepo);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCommand(int id)
    {
      var commandModelFromRepo = _commandRepo.GetById(id);
      if (commandModelFromRepo == null)
      {
        return NotFound();
      }
      _commandRepo.Delete(commandModelFromRepo);
      _commandRepo.SaveChanges();
      return NoContent();
    }
  }
}