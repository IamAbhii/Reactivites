using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
  public class Edit
  {
    public class Command : IRequest
    {
      public Guid Id { get; set; }
      public string Title { get; set; }
      public string Discription { get; set; }
      public string Category { get; set; }
      public DateTime? Date { get; set; }
      public string City { get; set; }
      public string Venue { get; set; }
    }

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
        //handler logic 
        var activity = await _context.Activities.FindAsync(request.Id);

       if (activity == null) throw new RestException(HttpStatusCode.NotFound,new {activity="Not Found"});

        //Used ?? operand to store old value if it is not edited
        activity.Title = request.Title ?? activity.Title;
        activity.Discription = request.Discription ?? activity.Discription;
        activity.Category = request.Category ?? activity.Category;
        activity.Date = request.Date ?? activity.Date;
        activity.City = request.City ?? activity.City;
        activity.Venue = request.Venue ?? activity.Venue;

        //here savechangesAsync retuns int, this int is number of changes saved in database so if its 0 then no changes is saved.
        var success = await _context.SaveChangesAsync() > 0;

        if (success) return Unit.Value;

        throw new Exception("Problem saving new event");

      }
    }
  }
}