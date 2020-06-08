using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Application.Activities
{
  public class Create
  {
    public class Command : IRequest
    {
      public Guid Id { get; set; }
      public string Title { get; set; }
      public string Discription { get; set; }
      public string Category { get; set; }
      public DateTime Date { get; set; }
      public string City { get; set; }
      public string Venue { get; set; }
    }

    //Model validation for error handling
    public class CommandValidator : AbstractValidator<Command>
    {
      public CommandValidator()
      {
        RuleFor(x=>x.Title).NotEmpty();
        RuleFor(x=>x.Discription).NotEmpty();
        RuleFor(x=>x.Category).NotEmpty();
        RuleFor(x=>x.Date).NotEmpty();
        RuleFor(x=>x.City).NotEmpty();
        RuleFor(x=>x.Venue).NotEmpty();
      }
    }

    public class Handler : IRequestHandler<Command>
    {
      private readonly DataContext _context;
      public Handler(DataContext context)
      {
        _context = context;
      }

      public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
      {
        var activity = new Activity
        {
          Id = request.Id,
          Title = request.Title,
          Discription = request.Discription,
          Category = request.Category,
          Date = request.Date,
          City = request.City,
          Venue = request.Venue,
        };

        _context.Activities.Add(activity);

        //here savechangesAsync retuns int, this int is number of changes saved in database so if its 0 then no changes is saved.
        var success = await _context.SaveChangesAsync() > 0;

        if (success) return Unit.Value;

        throw new Exception("Problem saving new event");

      }
    }
  }
}