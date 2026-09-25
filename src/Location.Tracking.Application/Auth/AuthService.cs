using Location.Tracking.Application.Auth.Dtos;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Application.Users.Commands.Login;
using Location.Tracking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Location.Tracking.Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ITrackingDbContext _context;
        private readonly ITokenIssuer _tokenIssuer;
        public AuthService(ITrackingDbContext context, ITokenIssuer tokenIssuer)
        {
            _context = context;
            _tokenIssuer = tokenIssuer;
        }

        public async Task<Result<TokenResponse>> Login(LoginRequest loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(b => b.Email == loginRequest.Email);

            if (user == null) return Result<TokenResponse>.Failure(Errors.UserErrors.InvalidCredentials); // user does not exist

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password) == PasswordVerificationResult.Failed)
            {
                return Result<TokenResponse>.Failure(Errors.UserErrors.InvalidCredentials); //invalid password
            }

            TokenResponse token = new TokenResponse { accessToken = _tokenIssuer.CreateAccessToken(user) };

            return Result<TokenResponse>.Success(token);
        }

        public async Task<Result> Register(RegisterRequest registerRequest)
        {

            if (await _context.Users.FirstOrDefaultAsync(b => b.Email == registerRequest.Email) != null)
                return Result.Failure(Errors.UserErrors.UserExists);

            User newUser = new User
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                Role = "demo"
            };

            var hashedPassword = new PasswordHasher<User>().HashPassword(newUser, registerRequest.Password);

            newUser.PasswordHash = hashedPassword;

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
