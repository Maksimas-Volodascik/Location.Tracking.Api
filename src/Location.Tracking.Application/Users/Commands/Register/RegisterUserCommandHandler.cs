using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Location.Tracking.Application.Users.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterCommand, Result<User>>
    {
        private readonly IUserRepository _userRepository;
        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<User>> Handle(RegisterCommand credentials, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetUserByEmailAsync(credentials.Email) != null) 
                return Result<User>.Failure(Errors.UserErrors.UserExists);

            User newUser = new User
            {
                FirstName = credentials.FirstName,
                LastName = credentials.LastName,
                Email = credentials.Email,
            };

            var hashedPassword = new PasswordHasher<User>().HashPassword(newUser, credentials.Password);

            newUser.PasswordHash = hashedPassword;

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            return Result<User>.Success(newUser);
        }
    }
}
