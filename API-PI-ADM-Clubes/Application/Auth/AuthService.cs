using API_PI_ADM_Clubes.Application.DTOs;
using API_PI_ADM_Clubes.Application.Email;
using API_PI_ADM_Clubes.Application.Interfaces.IRepositories;

using API_PI_ADM_Clubes.Infrastructure.Security.Interfaces;
using API_PI_ADM_Clubes.Infrastructure.Settings;
using API_PI_ADM_Clubes.Model;
using API_PI_ADM_Clubes.Model.Enums;
using API_PI_ADM_Clubes.Model.ValueObjects;
using System.Security.Claims;
using API_PI_ADM_Clubes.Application.Exceptions;
using API_PI_ADM_Clubes.Application.Interfaces.IServices;
using API_PI_ADM_Clubes.Application.Validators;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;


namespace API_PI_ADM_Clubes.Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IManagerRepository _repository;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IPlayerService _playerService;
        private readonly ICpfEncryptionService _cpfEncryptionService;

        public AuthService(
            IManagerRepository repository,
            IUserService userService,
            ITokenService tokenService,
            IPasswordHasher passwordHasher,
            IEmailService emailService,
            IConfiguration config,
            IPlayerService playerService,
            ICpfEncryptionService cpfEncryptionService
        )
        {
            _repository = repository;
            _userService = userService;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _config = config;
            _playerService = playerService;
            _cpfEncryptionService = cpfEncryptionService;
        }

        public async Task<Manager> LoginAsync(AuthDTO dto, CancellationToken ct)
        {
            var email = dto.Email.Trim().ToLowerInvariant();
            var manager = await _repository.GetByEmailAsync(email, ct);

            if (manager is null || !manager.IsActive ||
                !_passwordHasher.Verify(dto.Password, manager.PasswordHash))
                throw new Exception("E-mail ou senha inválidos.");

            return manager;
        }

        public async Task<Manager> GoogleLogin(string idToken, CancellationToken cancellationToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [_config["Google:ClientId"]]
            };

            GoogleJsonWebSignature.Payload payload;

            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            }
            catch (InvalidJwtException)
            {
                throw new ValidationException("Token do Google inválido ou expirado.");
            }

            var manager = await _repository.GetByEmailAsync(payload.Email, cancellationToken);
            if (manager is null)
                throw new ValidationException("Nenhuma conta encontrada com esse e-mail");

            return manager;
        }
    }
}
