using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using Location.Tracking.Domain.Entities;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace Location.Tracking.Domain.Tests.Domain
{
    public class UserTests
    {
        [Fact]
        public void Constructor_Init_Default_Values()
        {
            // Arrange and Act
            var user = new User("test@email.com","hash123");

            //Assert
            Assert.Equal(Guid.Empty, user.Id);
            Assert.Equal("test@email.com", user.Email);
            Assert.Equal("hash123", user.PasswordHash);
            Assert.Equal("User", user.Role);
            Assert.Empty(user.FirstName);
            Assert.Empty(user.LastName);
            Assert.Empty(user.Devices);
            Assert.True(user.IsActive);
            Assert.True(user.CreatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void User_Success_When_Valid_Email()
        {
            // Arrange
            var user = new User("test@email.com", "hash123");
            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(user, context, results, true);

            //Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("@mail.com")]
        [InlineData("mail.com")]
        [InlineData("myemail@")]
        [InlineData("myemailaddress")]
        public void User_Fail_When_Invalid_Email(string email)
        {
            // Arrange
            var user = new User(email, "hash123");
            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(user, context, results, true);

            //Assert
            Assert.False(isValid);
            Assert.Contains("Invalid email address", results[0].ErrorMessage);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]

        public void User_Fail_When_Email_Empty(string email)
        {
            // Arrange
            var user = new User(email, "hash123");
            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(user, context, results, true);

            //Assert
            Assert.False(isValid);
            Assert.Contains("Email is required", results[0].ErrorMessage);
        }
    }
}
