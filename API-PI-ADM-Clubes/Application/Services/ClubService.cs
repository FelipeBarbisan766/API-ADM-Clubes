using API_PI_ADM_Clubes.Application.DTOs;
using API_PI_ADM_Clubes.Application.Exceptions;
using API_PI_ADM_Clubes.Application.Interfaces.IMappers;
using API_PI_ADM_Clubes.Application.Interfaces.IRepositories;
using API_PI_ADM_Clubes.Application.Interfaces.IServices;
using API_PI_ADM_Clubes.Infrastructure.Extensions;
using API_PI_ADM_Clubes.Model;
using API_PI_ADM_Clubes.Model.Enums;
using API_PI_ADM_Clubes.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace API_PI_ADM_Clubes.Application.Services
{
    public class ClubService : IClubService
    {
        private readonly IClubRepository _repository;
        private readonly IClubMapper _mapper;
        private readonly IImageRepository _imageRepository;
        private readonly IPlanLimitService _planLimitService;
        private readonly IClubReviewRepository _clubReviewRepository; 

        public ClubService(IClubMapper mapper,
            IClubRepository repository,
            IImageRepository imageRepository,
            IPlanLimitService planLimitService,
            IClubReviewRepository clubReviewRepository 
        )
        {
            _mapper = mapper;
            _repository = repository;
            _imageRepository = imageRepository;
            _planLimitService = planLimitService;
            _clubReviewRepository = clubReviewRepository;
        }

        public async Task<PagedResultDTO<ResponseClubDTO>> GetAll(ClubQueryDTO query, CancellationToken cancellationToken)
        {
            var (items, total) = await _repository.GetAllAsync(query, cancellationToken);

            return new PagedResultDTO<ResponseClubDTO>
            {
                Data = items,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<ResponseClubByIdDTO> GetById(Guid id, CancellationToken cancellationToken)
        {
            ValidateId(id);

            var data = await _repository.GetByIdAsync(id, cancellationToken);
            if (data == null)
                throw new NotFoundException("Clube", id);

            var dto = _mapper.ToDTOById(data);

            var summary = await _clubReviewRepository.GetSummaryByClubIdAsync(id, cancellationToken);
            dto.AverageRating = summary.AverageRating;
            dto.TotalReviews = summary.TotalReviews;

            return dto;
        }

        public async Task<List<ResponseClubDTO>> GetAllByAdminId(Guid id, CancellationToken cancellationToken)
        {
            ValidateId(id);

            var data = await _repository.GetAllByAdminIdAsync(id, cancellationToken);

            if (data == null)
                throw new NotFoundException("Admin", id);

            return data;
        }

        public async Task<ResponseDashboardDTO> GetDashboard(Guid id, CancellationToken cancellationToken)
        {
            ValidateId(id);

            var data = await _repository.GetDashboardAsync(id, cancellationToken);

            if (data == null)
                throw new NotFoundException("Clube", id);

            return data;
        }

        public async Task<ResponseClubDTO> Update(Guid userId, Guid id, UpdateClubDTO dto, CancellationToken cancellationToken)
        {
            ValidateId(id);
            ValidateUpdateClubDTO(dto);
            await AuthorizeOwnership(userId, id, cancellationToken);

            var data = await _repository.GetByIdAsync(id, cancellationToken);
            if (data == null)
                throw new Exception("Club not found");

            data.Name = dto.Name;
            data.PhoneNumber = dto.PhoneNumber;
            data.Address = new AddressVO(
                dto.ZipCode, dto.Street, dto.Number, dto.Neighborhood,
                dto.Complement, dto.City, dto.State, dto.Country
            );
            data.Description = dto.Description;
            data.UpdatedAt = DateTime.UtcNow;

            _repository.Update(data);
            await _repository.SaveChangesAsync(cancellationToken);
            return _mapper.ToDTO(data);
        }

        public async Task Delete(Guid userId, Guid id, CancellationToken cancellationToken)
        {
            ValidateId(id);
            await AuthorizeOwnership(userId, id, cancellationToken);

            var exists = await _repository.ExistsAsync(id, cancellationToken);
            if (!exists)
                throw new NotFoundException("Clube", id);

            await _repository.DeleteAsync(id, cancellationToken);
        }
        

        private static void ValidateId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException("O ID informado é inválido.");
        }

        private async Task AuthorizeOwnership(Guid userId, Guid id, CancellationToken cancellationToken)
        {
            var isOwner = await _repository.IsOwnedByUserAsync(id, userId, cancellationToken);
            if (!isOwner)
                throw new ForbiddenException("Você não tem permissão para gerenciar este clube.");
        }

        private static void ValidateClubDTO(CreateClubDTO dto)
        {
            if (dto == null)
                throw new ValidationException(nameof(dto));
        }

        private static void ValidateUpdateClubDTO(UpdateClubDTO dto)
        {
            if (dto == null)
                throw new ValidationException(nameof(dto));
        }

        
    }
}