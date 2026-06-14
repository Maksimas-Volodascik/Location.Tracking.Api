using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using Location.Tracking.Application.Users.Commands.Register;
using Location.Tracking.Domain.Entities;
using Microsoft.Extensions.Options;
using Moq;


namespace Location.Tracking.Application.Tests.UserTests.Commands
{
    public class UserRegisterTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly RegisterUserCommandHandler _handler;
        public UserRegisterTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new RegisterUserCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidData_ReturnsSuccessResult()
        {
            //Arrange
            RegisterCommand credentials = new RegisterCommand
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
            var response = await _handler.Handle(credentials, CancellationToken.None);

            //Assert
            Assert.NotNull(response.Data);
            Assert.Equal(response.Data.Email, credentials.Email);
            Assert.Equal(response.Data.FirstName, credentials.FirstName);
            Assert.Equal(response.Data.LastName, credentials.LastName);
            Assert.False(string.IsNullOrEmpty(response.Data.PasswordHash));

            _userRepositoryMock.Verify(u => u.AddAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_UserExists_ReturnsErrorResult()
        {
            //Arrange
            RegisterCommand credentials = new RegisterCommand
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
            var response = await _handler.Handle(credentials, CancellationToken.None);

            //Assert
            Assert.False(response.IsSuccess);
            Assert.NotNull(response.Error);

            _userRepositoryMock.Verify(u => u.AddAsync(It.IsAny<User>()), Times.Never);
            _userRepositoryMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }
    }
}
