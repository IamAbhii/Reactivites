using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.User
{
  public class CurrentUser
  {
    public class Query : IRequest<User> { }

    public class Handler : IRequestHandler<Query, User>
    {

      private readonly IUserAccessor _userAccessor;
      private readonly IJWTGenerator _jwtGenerator;
      private readonly UserManager<AppUser> _userManager;

      public Handler(UserManager<AppUser> userManager, IJWTGenerator jwtGenerator, IUserAccessor userAccessor)
      {
        _userManager = userManager;
        _jwtGenerator = jwtGenerator;
        _userAccessor = userAccessor;

      }

      public async Task<User> Handle(Query request, CancellationToken cancellationToken)
      {
        var user=await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

        return new User
        {
            DisplayName = user.DisplayName,
            Username=user.UserName,
            Token=_jwtGenerator.CreateToken(user),
            Image=null,
        };
      }
    }
  }
}