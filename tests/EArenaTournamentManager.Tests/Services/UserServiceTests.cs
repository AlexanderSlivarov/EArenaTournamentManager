using EArenaTournamentManager.Application.Services.Implementations.Users;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepoMock.Object);
            _userService = new UserService(_unitOfWorkMock.Object);
        }

        // --- GetByIdAsync ---

        [Fact]
        public async Task GetById_WithValidId_ReturnsUser()
        {
            var user = new User { Id = 1, Username = "testuser", Email = "test@earena.com" };

            _userRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(user);

            var result = await _userService.GetByIdAsync(1);

            Assert.True(result.IsSuccess);
            Assert.Equal(user.Id, result.Data!.Id);
            Assert.Equal(user.Username, result.Data.Username);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsSuccessWithNull()
        {
            _userRepoMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((User?)null);

            var result = await _userService.GetByIdAsync(99);

            Assert.True(result.IsSuccess);
            Assert.Null(result.Data);
        }                

        [Fact]
        public async Task GetByUsername_WithExistingUsername_ReturnsUser()
        {
            var user = new User { Id = 1, Username = "testuser" };

            _userRepoMock
                .Setup(r => r.GetByUsernameAsync("testuser"))
                .ReturnsAsync(user);

            var result = await _userService.GetByUsernameAsync("testuser");

            Assert.NotNull(result);
            Assert.Equal("testuser", result!.Username);
        }

        [Fact]
        public async Task GetByUsername_WithNonExistentUsername_ReturnsNull()
        {
            _userRepoMock
                .Setup(r => r.GetByUsernameAsync("ghost"))
                .ReturnsAsync((User?)null);

            var result = await _userService.GetByUsernameAsync("ghost");

            Assert.Null(result);
        }        

        [Fact]
        public async Task GetByEmail_WithExistingEmail_ReturnsUser()
        {
            var user = new User { Id = 1, Email = "test@earena.com" };

            _userRepoMock
                .Setup(r => r.GetByEmailAsync("test@earena.com"))
                .ReturnsAsync(user);

            var result = await _userService.GetByEmailAsync("test@earena.com");

            Assert.NotNull(result);
            Assert.Equal("test@earena.com", result!.Email);
        }

        [Fact]
        public async Task GetByEmail_WithNonExistentEmail_ReturnsNull()
        {
            _userRepoMock
                .Setup(r => r.GetByEmailAsync("ghost@earena.com"))
                .ReturnsAsync((User?)null);

            var result = await _userService.GetByEmailAsync("ghost@earena.com");

            Assert.Null(result);
        }        

        [Fact]
        public async Task Save_NewUser_WithUniqueCredentials_ReturnsSuccess()
        {
            var user = new User { Id = 0, Username = "newuser", Email = "new@earena.com" };

            _userRepoMock.Setup(r => r.GetByUsernameAsync("newuser")).ReturnsAsync((User?)null);
            _userRepoMock.Setup(r => r.GetByEmailAsync("new@earena.com")).ReturnsAsync((User?)null);
            _userRepoMock.Setup(r => r.InsertAsync(user)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _userService.SaveAsync(user);

            Assert.True(result.IsSuccess);
            _userRepoMock.Verify(r => r.InsertAsync(user), Times.Once);
        }

        [Fact]
        public async Task Save_NewUser_WithDuplicateUsername_ReturnsFailure()
        {
            var existing = new User { Id = 1, Username = "takenuser", Email = "other@earena.com" };
            var newUser = new User { Id = 0, Username = "takenuser", Email = "new@earena.com" };

            _userRepoMock.Setup(r => r.GetByUsernameAsync("takenuser")).ReturnsAsync(existing);

            var result = await _userService.SaveAsync(newUser);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, e => e.Key == "UsernameValidation");
        }

        [Fact]
        public async Task Save_NewUser_WithDuplicateEmail_ReturnsFailure()
        {
            var existing = new User { Id = 1, Username = "otheruser", Email = "taken@earena.com" };
            var newUser = new User { Id = 0, Username = "newuser", Email = "taken@earena.com" };

            _userRepoMock.Setup(r => r.GetByUsernameAsync("newuser")).ReturnsAsync((User?)null);
            _userRepoMock.Setup(r => r.GetByEmailAsync("taken@earena.com")).ReturnsAsync(existing);

            var result = await _userService.SaveAsync(newUser);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, e => e.Key == "EmailValidation");
        }

        [Fact]
        public async Task Save_ExistingUser_CallsUpdate_NotInsert()
        {
            var user = new User { Id = 5, Username = "existinguser", Email = "existing@earena.com" };

            _userRepoMock.Setup(r => r.GetByUsernameAsync("existinguser")).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.GetByEmailAsync("existing@earena.com")).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.Update(user));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _userService.SaveAsync(user);

            Assert.True(result.IsSuccess);
            _userRepoMock.Verify(r => r.Update(user), Times.Once);
            _userRepoMock.Verify(r => r.InsertAsync(It.IsAny<User>()), Times.Never);
        }               

        [Fact]
        public async Task Delete_ExistingUser_ReturnsSuccess()
        {
            var user = new User { Id = 1, Username = "testuser", Email = "test@earena.com" };

            _userRepoMock.Setup(r => r.Delete(user));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _userService.DeleteAsync(user);

            Assert.True(result.IsSuccess);
            _userRepoMock.Verify(r => r.Delete(user), Times.Once);
        }
    }
}
