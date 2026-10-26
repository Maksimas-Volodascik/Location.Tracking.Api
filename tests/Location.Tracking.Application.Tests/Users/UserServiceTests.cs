using AutoMapper;
using Location.Tracking.Application.Mapper;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Application.Tests.Shared;
using Location.Tracking.Application.Users;
using Location.Tracking.Application.Users.Dtos;
using Location.Tracking.Domain.Entities;
using Location.Tracking.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Location.Tracking.Application.Tests.Users
{
    public class UserServiceTests
    {
        private readonly Guid ExistingUserId = new("11111111-1111-1111-1111-111111111111");
        private const string ExistingFirstName = "John";
        private const string ExistingLastName = "Smith";
        private const string ExistingEmail = "john.smith@test.com";

        private static IMapper _mapper;

        private readonly Mock<ITrackingDbContext> _mockDbContext;
        private readonly Mock<DbSet<User>> _mockUsers;
        private readonly User _existingUser;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile<UserProfile>();
            }, NullLoggerFactory.Instance).CreateMapper();

            _existingUser = new User
            {
                Id = ExistingUserId,
                Email = ExistingEmail,
                FirstName = ExistingFirstName,
                LastName = ExistingLastName,
                Role = UserRole.User
            };

            _mockUsers = DbSetMockFactory.Create(new List<User> { _existingUser }, x => x.Id);

            _mockDbContext = new Mock<ITrackingDbContext>();
            _mockDbContext.Setup(c => c.Users).Returns(_mockUsers.Object);
            _mockDbContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _userService = new UserService(_mockDbContext.Object, _mapper);
        }

        [Fact]
        public async Task DeleteUser_ExistingUser_ReturnsSuccess()
        {
            var result = await _userService.DeleteUser(ExistingUserId);

            Assert.True(result.IsSuccess, result.Error?.ErrorMessage);
            _mockUsers.Verify(s => s.Remove(_existingUser), Times.Once);
            _mockDbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_UserNotFound_ReturnsFailure()
        {
            var unknownUserId = Guid.NewGuid();

            var result = await _userService.DeleteUser(unknownUserId);

            Assert.Equal(Errors.UserErrors.UserNotFound.ErrorMessage, result.Error?.ErrorMessage);
            _mockUsers.Verify(s => s.Remove(It.IsAny<User>()), Times.Never);
            _mockDbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateUser_ValidData_UpdatesAllFields()
        {
            var request = new UserConfiguration { FirstName = "Jane", LastName = "Doe", Role = UserRole.Admin };

            var result = await _userService.UpdateUser(ExistingUserId, request);

            Assert.True(result.IsSuccess, result.Error?.ErrorMessage);
            Assert.Equal("Jane", _existingUser.FirstName);
            Assert.Equal("Doe", _existingUser.LastName);
            Assert.Equal(UserRole.Admin, _existingUser.Role);
            _mockDbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_OnlyFirstNameProvided_KeepsOtherFields()
        {
            var request = new UserConfiguration { FirstName = "Jane" };

            var result = await _userService.UpdateUser(ExistingUserId, request);

            Assert.True(result.IsSuccess, result.Error?.ErrorMessage);
            Assert.Equal("Jane", _existingUser.FirstName);
            Assert.Equal(ExistingLastName, _existingUser.LastName);
            Assert.Equal(UserRole.User, _existingUser.Role);
        }

        [Fact]
        public async Task UpdateUser_UserNotFound_ReturnsFailure()
        {
            var unknownUserId = Guid.NewGuid();
            var request = new UserConfiguration { FirstName = "Jane" };

            var result = await _userService.UpdateUser(unknownUserId, request);

            Assert.Equal(Errors.UserErrors.UserNotFound.ErrorMessage, result.Error?.ErrorMessage);
            Assert.Equal(ExistingFirstName, _existingUser.FirstName);
            _mockDbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        /*[Fact]
        public async Task GetAllUsers_ReturnsMappedUsers()
        {
            var result = await _userService.GetAllUsers();

            Assert.True(result.IsSuccess, result.Error?.ErrorMessage);
            var user = Assert.Single(result.Data!);
            Assert.Equal(ExistingUserId, user.UserGuid);
            Assert.Equal(ExistingFirstName, user.FirstName);
            Assert.Equal(ExistingLastName, user.LastName);
            Assert.Equal(ExistingEmail, user.Email);
            Assert.Equal(nameof(UserRole.User), user.Role);
        }*/

    }
}
