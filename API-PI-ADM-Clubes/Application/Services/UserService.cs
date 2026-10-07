using API_PI_ADM_Clubes.Application.DTOs;
using API_PI_ADM_Clubes.Application.Email;
using API_PI_ADM_Clubes.Application.Exceptions;
using API_PI_ADM_Clubes.Application.Interfaces.IMappers;
using API_PI_ADM_Clubes.Application.Interfaces.IRepositories;
using API_PI_ADM_Clubes.Application.Interfaces.IServices;
using API_PI_ADM_Clubes.Infrastructure.Security.Interfaces;
using API_PI_ADM_Clubes.Model;
using API_PI_ADM_Clubes.Model.Enums;

namespace API_PI_ADM_Clubes.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserMapper _mapper;
        private readonly IHttpClientFactory  _httpClientFactory;

        public UserService(
            IUserRepository userRepository, 
            IPasswordHasher passwordHasher, 
            IUserMapper mapper,
            IHttpClientFactory  httpClientFactory
            )
        {
            _repository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseUserDTO> GetById(Guid id, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(id, cancellationToken);

            if (user == null)
                throw new NotFoundException("Usuário", id); 

            return _mapper.ToDTO(user);
        }


        public async Task<ResponseUserDTO> Update(Guid id, UpdateUserDTO dto, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(id, cancellationToken);

            if (user == null)
                throw new NotFoundException("Usuário", id);

            if(dto.Name != null)
                user.Name = dto.Name;
            if (dto.PhoneNumber != null)
                user.PhoneNumber = dto.PhoneNumber;
            user.UpdatedAt = DateTime.UtcNow;

            _repository.Update(user);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.ToDTO(user);
        }

        public async Task UpdateRole(Guid id, RoleEnum role, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(id, cancellationToken);

            if (user == null)
                throw new NotFoundException("Usuário", id);

            user.Role = role;
            user.UpdatedAt = DateTime.UtcNow;

            _repository.Update(user);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        
       
        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(id, cancellationToken);

            if (user == null)
                throw new NotFoundException("Usuário", id);

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            _repository.Update(user);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        
        
    }
}
