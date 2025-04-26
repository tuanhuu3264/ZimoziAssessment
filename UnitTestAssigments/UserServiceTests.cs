using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Xunit;
using Zimozi.Assessment.BusinessModels.RequestModels.Login;
using Zimozi.Assessment.Models;
using Zimozi.Assessment.Repository.UserRepository;
using Zimozi.Assessment.Services.UserService;

namespace Zimozi.Assessment.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<UserService>>();
            _mockConfiguration = new Mock<IConfiguration>();

            // Setup Configuration for JWT
            var configSection = new Mock<IConfigurationSection>();
            configSection.Setup(c => c.Value).Returns("very_long_test_key_at_least_128_bits_for_hmacsha256_algorithm");
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("very_long_test_key_at_least_128_bits_for_hmacsha256_algorithm");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("test_issuer");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("test_audience");

            _userService = new UserService(_mockUserRepository.Object, _mockLogger.Object, _mockConfiguration.Object);
        }

        #region Login Tests

        [Fact]
        public async System.Threading.Tasks.Task Login_ValidCredentials_ReturnsSuccessResultWithToken()
        {
            var loginModel = new LoginModel
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                Name = "Test User",
                Role = Role.User
            };

            _mockUserRepository.Setup(repo => repo.Login(loginModel.Email, loginModel.Password))
                .ReturnsAsync(user);

            var result = await _userService.Login(loginModel);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Login successfully", result.Message);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(result.Data.Token);

            var nameIdentifierClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            Assert.Equal("1", nameIdentifierClaim.Value);

            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/email");
            Assert.NotNull(emailClaim);
            Assert.Equal("test@example.com", emailClaim.Value);

            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            Assert.NotNull(roleClaim);
            Assert.Equal("User", roleClaim.Value);
        }

        [Fact]
        public async System.Threading.Tasks.Task Login_InvalidCredentials_ReturnsUnauthorizedResult()
        {
            var loginModel = new LoginModel
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            _mockUserRepository.Setup(repo => repo.Login(loginModel.Email, loginModel.Password))
                .ReturnsAsync((User)null);

            var result = await _userService.Login(loginModel);

            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal("There is not found any user", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async System.Threading.Tasks.Task Login_ThrowsException_ReturnsErrorResult()
        {
            var loginModel = new LoginModel
            {
                Email = "test@example.com",
                Password = "password123"
            };

            _mockUserRepository.Setup(repo => repo.Login(loginModel.Email, loginModel.Password))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _userService.Login(loginModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Database error", result.Message);
            Assert.Null(result.Data);
        }

        #endregion
    }
}