using EArenaTournamentManager.Application.RequestDTOs.Users;
using EArenaTournamentManager.Application.ResponseDTOs.Users;
using EArenaTournamentManager.Application.Services.Interfaces.Users;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Security.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EArenaTournamentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseCrudController<
        User,
        IUserService,
        UserRequest,
        UserGetRequest,
        UserResponse,
        UserGetResponse>
    {
        private readonly IPasswordHasher _passwordHasher;

        public UsersController(IUserService userService, IPasswordHasher passwordHasher) : base(userService) 
        {
            _passwordHasher = passwordHasher;
        }

        protected override void PopulateEntity(User entity, UserRequest model)
        {
            entity.Username = model.Username;
            entity.Email = model.Email;
            entity.AvatarImageUrl = model.AvatarImageUrl;
            entity.Role = model.Role;

            if (!string.IsNullOrEmpty(model.Password))
            {
                entity.PasswordHash = _passwordHasher.HashPassword(model.Password);
            }
        }

        protected override Expression<Func<User, bool>>? GetFilter(UserGetRequest model)
        {
            model.Filter ??= new UserGetFilterRequest();

            return u =>
                (string.IsNullOrEmpty(model.Filter.Username) ||
                     (u.Username != null && u.Username.Contains(model.Filter.Username))) &&

                (string.IsNullOrEmpty(model.Filter.Email) ||
                     (u.Email != null && u.Email.Contains(model.Filter.Email))) &&

                (!model.Filter.Role.HasValue ||
                    u.Role == model.Filter.Role.Value);
        }

        protected override void PopulateGetResponse(UserGetRequest request, UserGetResponse response)
        {
            response.Filter = request.Filter;
        }

        protected override UserResponse ToResponse(User entity)
        {
            return new UserResponse
            {
                Id = entity.Id,
                Username = entity.Username,
                Email = entity.Email,
                AvatarImageUrl = entity.AvatarImageUrl,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                IsActive = entity.IsActive,
                Role = entity.Role                
            };
        }
    }
}
