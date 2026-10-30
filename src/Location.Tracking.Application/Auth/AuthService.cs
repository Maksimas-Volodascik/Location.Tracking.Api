using Location.Tracking.Application.Auth.Dtos;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Application.Users.Commands.Login;
using Location.Tracking.Application.Workspaces;
using Location.Tracking.Domain.Entities;
using Location.Tracking.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Location.Tracking.Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ITrackingDbContext _context;
        private readonly ITokenIssuer _tokenIssuer;
        private readonly IWorkspaceService _workspaceService;
        public AuthService(ITrackingDbContext context, ITokenIssuer tokenIssuer, IWorkspaceService workspaceService)
        {
            _context = context;
            _tokenIssuer = tokenIssuer;
            _workspaceService = workspaceService;
        }

        public async Task<Result> AcceptInvite(AcceptInviteRequest request)
        {
            var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(request.Token)));
            var invitation = await _context.Invitations.FirstOrDefaultAsync(i => i.TokenHash == tokenHash);

            if (invitation == null || invitation.ExpiresAt < DateTimeOffset.UtcNow || invitation.UsedAt != null)
            {
                return Result.Failure(Errors.InvitationErrors.Invalid);
            }

            var result = await AddUser(invitation.WorkspaceId, request.FirstName, request.LastName,
               request.Email, request.Password, invitation.Role);

            invitation.UsedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<string>> CreateInvite(Guid inviterId, InviteUserRequest request)
        {
            var user = await _context.Users.FindAsync(inviterId);
            if (user is null)
                return Result<string>.Failure(Errors.UserErrors.UserNotFound);

            if(user.WorkspaceId != request.WorkspaceId)
            {
                return Result<string>.Failure(Errors.WorkspaceErrors.WorkspaceNotFound);
            }

            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

            _context.Invitations.Add(new Invitation
            {
                CreatedByUserId = user.Id,
                TokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token))),
                WorkspaceId = request.WorkspaceId,
                Role = request.Role,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(7),
            });

            await _context.SaveChangesAsync();
            return Result<string>.Success(token);
        }

        public async Task<Result<TokenResponse>> Login(LoginRequest loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(b => b.Email == loginRequest.Email);

            if (user == null) return Result<TokenResponse>.Failure(Errors.UserErrors.InvalidCredentials); // user does not exist

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password) == PasswordVerificationResult.Failed)
            {
                return Result<TokenResponse>.Failure(Errors.UserErrors.InvalidCredentials); //invalid password
            }

            var token = new TokenResponse { accessToken = _tokenIssuer.CreateAccessToken(user) };

            return Result<TokenResponse>.Success(token);
        }

        public async Task<Result> Register(RegisterRequest registerRequest)
        {
            var workspace = _workspaceService.AddNewWorkspace(registerRequest.WorkspaceName);

            var result = await AddUser(workspace.Id, registerRequest.FirstName, registerRequest.LastName,
                registerRequest.Email, registerRequest.Password, UserRole.Demo);

            if (!result.IsSuccess)
            {
                return result;
            }

            await _context.SaveChangesAsync();

            return Result.Success();
        }

        private async Task<Result<User>> AddUser(Guid workspaceId, string firstName,
            string lastName, string email, string password, UserRole role)
        {

            if (await _context.Users.FirstOrDefaultAsync(b => b.Email == email) != null)
                return Result<User>.Failure(Errors.UserErrors.UserExists);

            User newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Role = role,
                WorkspaceId = workspaceId
            };

            newUser.PasswordHash = new PasswordHasher<User>().HashPassword(newUser, password); ;

            _context.Users.AddAsync(newUser);

            return Result<User>.Success(newUser);
        }
    }
}
