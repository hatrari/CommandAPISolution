using System;
using System.Collections.Generic;
using System.Linq;
using CommandAPI.Models;

namespace CommandAPI.Data
{
  public class CommandRepo : ICommandRepo
  {
    private readonly CommandContext _context;
    
    public CommandRepo(CommandContext context)
    {
      _context = context;
    }

    public void Create(Command cmd)
    {
      if(cmd == null)
      {
        throw new ArgumentNullException(nameof(cmd));
      }
      _context.Commands.Add(cmd);
    }

    public void Delete(Command cmd)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerable<Command> GetAll()
    {
      return _context.Commands.ToList();
    }

    public Command GetById(int id)
    {
      return _context.Commands.FirstOrDefault(c => c.Id == id);
    }

    public bool SaveChanges()
    {
      return (_context.SaveChanges() >= 0);
    }

    public void Update(Command cmd)
    {
      
    }
  }
}