using Location.Tracking.Application.DTOs.Auth;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Services;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Tests.UserTests
{
    public class UserLoginTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public UserLoginTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            _jwtSettings = Options.Create(new JwtSettings
            {
                Token = "Z7xQp2M9vLs8Tn4KcW5rB1yF0Hj6UeA3dGqXhRkP9mC2tYvN8sJwL5oD4bIu1fE7",
                Audience = "location.tracking",
                Issuer = "location.tracking",
                TokenExpiryInHours = 8000
            });
            _userService = new UserService(_userRepositoryMock.Object, _jwtSettings);
        }

        [Fact]
        public async Task LoginAsync_ValidData_ReturnsSuccessResult()
        {
            // Arrange
            var password = "Password123!";
            var user = new User
            {
                Email = "test@test.com"
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, password);

            var loginDto = new LoginDto
            {
                Email = "test@test.com",
                Password = password
            };

            _userRepositoryMock
                .Setup(x => x.GetUserByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.accessToken));
            Assert.NotEqual("password does not match", result.accessToken);
        }

        [Fact]
        public async Task LoginAsync_UserDoesNotExist_ReturnsErrorResult()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "unknown@test.com",
                Password = "Password123!"
            };

            _userRepositoryMock
                .Setup(x => x.GetUserByEmailAsync(loginDto.Email))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            Assert.Null(result);
        }

        [Fact] 
        public async Task LoginAsync_InvalidPassword_ReturnsErrorResult()
        {
            // Arrange
            var user = new User
            {
                Email = "test@test.com"
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, "CorrectPassword");

            var loginDto = new LoginDto
            {
                Email = "test@test.com",
                Password = "WrongPassword"
            };

            _userRepositoryMock
                .Setup(x => x.GetUserByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("password does not match", result.accessToken);
        }
    }
}
