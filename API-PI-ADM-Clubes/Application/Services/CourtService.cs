using API_PI_ADM_Clubes.Application.DTOs;
using API_PI_ADM_Clubes.Application.Exceptions;
using API_PI_ADM_Clubes.Application.Interfaces.IMappers;
using API_PI_ADM_Clubes.Application.Interfaces.IRepositories;
using API_PI_ADM_Clubes.Application.Interfaces.IServices;
using API_PI_ADM_Clubes.Infrastructure.Extensions;
using API_PI_ADM_Clubes.Infrastructure.Repositories;
using API_PI_ADM_Clubes.Model;
using API_PI_ADM_Clubes.Model.Enums;


namespace API_PI_ADM_Clubes.Application.Services
{
    public class CourtService : ICourtService
    {
        private readonly ICourtRepository _repository;
        private readonly ICourtMapper _mapper;
        private readonly IImageRepository _imageRepository;
        private readonly IPlanLimitService _planLimitService;

        private readonly ISportRepository _sportRepository;

        public CourtService(ICourtMapper mapper,
            ICourtRepository repository,
            IImageRepository imageRepository,
            ISportRepository sportRepository,
            IPlanLimitService planLimitService
        )
        {
            _mapper = mapper;
            _repository = repository;
            _imageRepository = imageRepository;
            _sportRepository = sportRepository;
            _planLimitService = planLimitService;
        }

        public async Task<PagedResultDTO<ResponseCourtDTO>> GetAll(CourtQueryDTO query,
            CancellationToken cancellationToken)
        {
            var (items, total) = await _repository.GetAllAsync(query, cancellationToken);

            return new PagedResultDTO<ResponseCourtDTO>
            {
                Data = items,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<ResponseCourtDTO> GetById(Guid id, CancellationToken cancellationToken)
        {
            ValidateId(id);

            var data = await _repository.GetByIdAsync(id, cancellationToken);

            if (data == null)
                throw new NotFoundException("Quadra", id);

            return _mapper.ToDTO(data);
        }

        public async Task<List<ResponseCourtDTO>> GetByClubId(Guid id, CancellationToken cancellationToken)
        {
            ValidateId(id);
            var data = await _repository.GetAllByClubIdAsync(id, cancellationToken);
            if (data == null)
                throw new NotFoundException("Clube", id);

            return data;
        }


        public async Task<ResponseCourtDTO> Update(Guid userId, Guid id, UpdateCourtDTO dto,
            CancellationToken cancellationToken)
        {
            ValidateId(id);
            ValidateUpdateCourtDTO(dto);
            await ValidateSportIdsAsync(dto.SportIds, cancellationToken);
            await AuthorizeOwnership(userId, id, cancellationToken);

            var data = await _repository.GetByIdAsync(id, cancellationToken);

            if (data == null)
                throw new NotFoundException("Quadra", id);

            data.Name = dto.Name;
            data.Surface = dto.Surface;
            data.IsCovered = dto.IsCovered;
            data.PricePerHour = dto.PricePerHour;
            data.Description = dto.Description;
            data.UpdatedAt = DateTime.UtcNow;

            SyncCourtSports(data, dto.SportIds);

            _repository.Update(data);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.ToDTO(data);
        }

        private static void SyncCourtSports(Court court, List<Guid> newSportIds)
        {
            var newIds = newSportIds.Distinct().ToHashSet();
            var currentIds = court.CourtSports.Select(cs => cs.SportId).ToHashSet();

            var toRemove = court.CourtSports.Where(cs => !newIds.Contains(cs.SportId)).ToList();
            foreach (var cs in toRemove)
                court.CourtSports.Remove(cs);

            var toAdd = newIds.Where(sid => !currentIds.Contains(sid));
            foreach (var sportId in toAdd)
                court.CourtSports.Add(new CourtSport { CourtId = court.Id, SportId = sportId });
        }

        public async Task Delete(Guid userId, Guid id, CancellationToken cancellationToken)
        {
            ValidateId(id);
            await AuthorizeOwnership(userId, id, cancellationToken);

            var exists = await _repository.ExistsAsync(id, cancellationToken);

            if (!exists)
                throw new NotFoundException("Quadra", id);

            await _repository.DeleteAsync(id, cancellationToken);
        }

        

       

        private async Task AuthorizeOwnership(Guid userId, Guid id, CancellationToken cancellationToken)
        {
            var isOwner = await _repository.IsOwnedByUserAsync(id, userId, cancellationToken);
            if (!isOwner)
                throw new ForbiddenException("Você não tem permissão para gerenciar esta quadra.");
        }

        private static void ValidateId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException("O ID informado é inválido.");
        }

        private static void ValidateCourtDTO(CreatCourtDTO dto)
        {
            if (dto == null)
                throw new ValidationException("Os dados da quadra são obrigatórios.");
        }

        private static void ValidateUpdateCourtDTO(UpdateCourtDTO dto)
        {
            if (dto == null)
                throw new ValidationException("Os dados de atualização são obrigatórios.");
        }

        private async Task ValidateSportIdsAsync(List<Guid> sportIds, CancellationToken cancellationToken)
        {
            if (sportIds == null || sportIds.Count == 0)
                throw new ValidationException("A quadra deve ter ao menos um esporte.");

            var distinctIds = sportIds.Distinct().ToList();
            var existingCount = await _sportRepository.CountExistingAsync(distinctIds, cancellationToken);

            if (existingCount != distinctIds.Count)
                throw new ValidationException("Um ou mais esportes informados são inválidos.");
        }

        
    }
}