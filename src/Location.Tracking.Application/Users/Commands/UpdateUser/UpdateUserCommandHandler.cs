using AutoMapper;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user is null) return Result.Failure(Errors.UserErrors.UserNotFound);

            user = _mapper.Map(request.UserConfiguration, user);

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
