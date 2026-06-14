using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using Location.Tracking.Application.Users.Commands.Login;
using Location.Tracking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace Location.Tracking.Application.Tests.UserTests.Commands
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly LoginUserCommandHandler _handler;
        public LoginUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            _jwtSettings = Options.Create(new JwtSettings
            {
                Token = "Z7xQp2M9vLs8Tn4KcW5rB1yF0Hj6UeA3dGqXhRkP9mC2tYvN8sJwL5oD4bIu1fE7",
                Audience = "location.tracking",
                Issuer = "location.tracking",
                TokenExpiryInHours = 8000
            });
            _handler = new LoginUserCommandHandler(_userRepositoryMock.Object, _jwtSettings);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsSuccessResult()
        {
            // Arrange
            var password = "Password123!";
            var user = new User
            {
                Email = "test@test.com"
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, password);

            var loginCommand = new LoginCommand
            {
                Email = "test@test.com",
                Password = password
            };

            _userRepositoryMock
                .Setup(x => x.GetUserByEmailAsync(loginCommand.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(loginCommand, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.accessToken);
            Assert.NotEmpty(result.Data.accessToken);

            _userRepositoryMock.Verify(u => u.GetUserByEmailAsync(loginCommand.Email), Times.Once);
        }

        [Fact]
        public async Task Handle_UserDoesNotExist_ReturnsErrorResult()
        {
            // Arrange
            var loginCommand = new LoginCommand
            {
                Email = "unknown@test.com",
                Password = "Password123!"
            };

            _userRepositoryMock
                .Setup(x => x.GetUserByEmailAsync(loginCommand.Email))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(loginCommand, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);

            _userRepositoryMock.Verify(u => u.GetUserByEmailAsync(loginCommand.Email), Times.Once);
        }

        [Fact] 
        public async Task Handle_InvalidPassword_ReturnsErrorResult()
        {
            // Arrange
            var user = new User
            {
                Email = "test@test.com"
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, "CorrectPassword");

            var loginCommand = new LoginCommand
            {
                Email = "test@test.com",
                Password = "WrongPassword"
            };

            _userRepositoryMock
                .Setup(x => x.GetUserByEmailAsync(loginCommand.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(loginCommand, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Error);
            Assert.Equal("invalid_credentials", result.Error.ErrorType);

            _userRepositoryMock.Verify(u => u.GetUserByEmailAsync(loginCommand.Email), Times.Once);
        }
    }
}
