using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Services;
using Location.Tracking.Application.Shared;
using Microsoft.Extensions.Options;
using Moq;


namespace Location.Tracking.Application.Tests.UserTests
{
    public class UserRegisterTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public UserRegisterTests()
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

        /*[Fact]
        public async Task RegisterAsync_ValidData_ReturnsSuccessResult()
        {
            //Arrange
            RegisterDto credentials = new RegisterDto
            {
                FirstName = "firstName",
                LastName = "lastName",
                Email = "test@test.com",
                Password = "password123"
            };

            _userRepositoryMock.Setup(u => u.GetUserByEmailAsync(credentials.Email))
                .ReturnsAsync((User)null);

            _userRepositoryMock.Setup(u => u.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            _userRepositoryMock.Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            //Act
            var response = await _userService.RegisterAsync(credentials);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(response.Email, credentials.Email);
            Assert.Equal(response.FirstName, credentials.FirstName);
            Assert.Equal(response.LastName, credentials.LastName);
            Assert.False(string.IsNullOrEmpty(response.PasswordHash));

            _userRepositoryMock.Verify(u => u.AddAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_UserExists_ReturnsErrorResult()
        {
            //Arrange
            RegisterDto credentials = new RegisterDto
            {
                FirstName = "firstName",
                LastName = "lastName",
                Email = "test@test.com",
                Password = "password123"
            };

            var user = new User
            {
                FirstName = "Existing",
                LastName = "User",
                Email = "john@test.com",
                PasswordHash = "hash"
            };

            _userRepositoryMock.Setup(u => u.GetUserByEmailAsync(credentials.Email)
            ).ReturnsAsync(user);

            //Act
            var response = await _userService.RegisterAsync(credentials);

            //Assert
            Assert.Null(response);

            _userRepositoryMock.Verify(u => u.AddAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }*/
    }
}
