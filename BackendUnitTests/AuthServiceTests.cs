using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;
using Tech_Test_Backend.Models;
using Tech_Test_Backend.Models.Dtos;
using Tech_Test_Backend.Services;

namespace Backend_Unit_Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<User>> userManagerMock;
        private readonly Mock<SignInManager<User>> signInManagerMock;
        private readonly Mock<IConfiguration> configurationMock;
        private readonly Mock<IHttpContextAccessor> httpContextAccessorMock;
        private readonly AuthService authService;

        public AuthServiceTests()
        {
            //Instantiate required dependencies for AuthService
            userManagerMock = MockUserManager();
            signInManagerMock = MockSignInManager(userManagerMock.Object);
            configurationMock = new Mock<IConfiguration>();
            httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            authService = new AuthService(
            userManagerMock.Object,
            signInManagerMock.Object,
            configurationMock.Object,
            httpContextAccessorMock.Object
            );
        }

        [Fact]
        public async Task Register_Valid_User()
        {
            //Arrange
            var registerDto = new RegisterDto { Username = "testUser", Email = "test@test.com", Password = "TestPassword" };
            var user = new User { UserName = registerDto.Username, Email = registerDto.Email };

            userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            //Act
            var result = await authService.RegisterAsync(registerDto);

            //Assert
            Assert.True(result.Succeeded);

        }

        [Fact]
        public async Task WhoAmIAsync_ValidUser_ShouldReturnUserIdentityDto()
        {
            // Arrange
            var username = "testUser";
            var user = new User { UserName = username, Email = "test@test.com", CreatedAt = DateTime.UtcNow };

            // Mock HttpContext to simulate a logged-in user
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
        new Claim(ClaimTypes.Name, username)
    }));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(claimsPrincipal);

            httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContextMock.Object);

            // Mock UserManager to return the user by username
            userManagerMock.Setup(um => um.FindByNameAsync(username))
                .ReturnsAsync(user);

            // Act
            var result = await authService.WhoAmIAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.UserName, result.Name);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.CreatedAt, result.CreatedAt);
        }

        [Fact]
        public async Task AuthenticateAsync_ValidCredentials_ShouldReturnJwtToken()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "testUser", Password = "TestPassword" };
            var user = new User { UserName = loginDto.Username, Email = "test@test.com" };

            // Mock UserManager to find user by username
            userManagerMock.Setup(um => um.FindByNameAsync(loginDto.Username))
                .ReturnsAsync(user);

            // Mock SignInManager to check password sign-in success
            signInManagerMock.Setup(sm => sm.CheckPasswordSignInAsync(user, loginDto.Password, false))
                .ReturnsAsync(SignInResult.Success);

            // Mock IConfiguration to return a valid JWT key (at least 32 bytes long)
            var validJwtKey = new string('A', 32); // Replace with a valid key of length 32 bytes
            configurationMock.Setup(c => c["Jwt:Key"]).Returns(validJwtKey);
            configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("your-issuer");
            configurationMock.Setup(c => c["Jwt:Audience"]).Returns("your-audience");

            // Act
            var result = await authService.AuthenticateAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual("Invalid login attempt", result); // Token should not be an error message
        }

        [Fact]
        public async Task AuthenticateAsync_InvalidCredentials_ShouldReturnInvalidLoginMessage()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "testUser", Password = "WrongPassword" };

            // Mock UserManager to find user by username
            var user = new User { UserName = loginDto.Username };
            userManagerMock.Setup(um => um.FindByNameAsync(loginDto.Username))
                .ReturnsAsync(user);

            // Mock SignInManager to fail password sign-in
            signInManagerMock.Setup(sm => sm.CheckPasswordSignInAsync(user, loginDto.Password, false))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await authService.AuthenticateAsync(loginDto);

            // Assert
            Assert.Equal("Invalid login attempt", result);
        }

        private Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private Mock<SignInManager<User>> MockSignInManager(UserManager<User> userManager)
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            return new Mock<SignInManager<User>>(userManager, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
        }
    }
}