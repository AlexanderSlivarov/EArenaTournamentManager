using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.Services.Implementations.Base;
using EArenaTournamentManager.Application.Services.Interfaces.Users;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using EArenaTournamentManager.Infrastructure.Security.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.Services.Implementations.Users
{
    public class UserService : BaseService<User>, IUserService
    {      
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }   
        
        protected override IRepository<User> GetRepository() => _unitOfWork.Users;

        public async Task<User?> GetByUsernameAsync(string username)
           => await _unitOfWork.Users.GetByUsernameAsync(username);

        public async Task<User?> GetByEmailAsync(string email)
            => await _unitOfWork.Users.GetByEmailAsync(email);            
            
        public override async Task<ServiceResult<User>> SaveAsync(User entity)
        {
            var existingUsername = await GetByUsernameAsync(entity.Username);

            if (existingUsername is not null && existingUsername.Id != entity.Id)
            {
                return ServiceResultExtensions.Failure(
                    entity,
                    "UsernameValidation",
                    "Username already exists."
                );
            }

            var existingEmail = await GetByEmailAsync(entity.Email);

            if (existingEmail is not null && existingEmail.Id != entity.Id)
            {
                return ServiceResultExtensions.Failure(
                    entity,
                    "EmailValidation",
                    "Email already exists."
                );
            }
            
            return await base.SaveAsync(entity);
        }
    }
}
