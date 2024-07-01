using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.KafkaServices;
using DTO.Building;
using EntityLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services;

public class BuildingService : IBuildingService
{
    private readonly IBuildingRepository _buildingRepository;
    private readonly IMapper _mapper;
    private readonly IProducerKafkaService _producerKafkaService;
    private readonly ILogger<BuildingService> _logger;

    public BuildingService(IBuildingRepository buildingRepository, IMapper mapper, IProducerKafkaService producerKafkaService, ILogger<BuildingService> logger)
    {
        _buildingRepository = buildingRepository;
        _mapper = mapper;
        _producerKafkaService = producerKafkaService;
        _logger = logger;
    }

    public async Task AddBuildingPostAsync(BuildingPostDTO buildingPostDTO)
    {
        try
        {
            var building = _mapper.Map<Building>(buildingPostDTO.Building);
            var buildingPost = new BuildingPost
            {
                Building = building,
                IsActive = buildingPostDTO.IsActive,
                CreatedAt = buildingPostDTO.CreatedAt,
                UpdatedAt = buildingPostDTO.UpdatedAt
            };
            await _buildingRepository.AddAsync(buildingPost);

            // Log and produce Kafka message
            _logger.LogInformation("BuildingPost added with Id: {Id}", buildingPost.Building.Id);
            await _producerKafkaService.ProduceAsync(buildingPost.Building.Id.ToString(), "BuildingPost added");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding BuildingPost.");
            throw;
        }
    }

    public async Task<BuildingPostDTO> GetBuildingPostByIdAsync(Guid id)
    {
        try
        {
            var buildingPost = await _buildingRepository.GetByIdAsync(id);
            return _mapper.Map<BuildingPostDTO>(buildingPost);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving BuildingPost with Id: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<BuildingPostDTO>> GetAllBuildingPostsAsync()
    {
        try
        {
            var buildingPosts = await _buildingRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BuildingPostDTO>>(buildingPosts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all BuildingPosts.");
            throw;
        }
    }

    public async Task<IEnumerable<BuildingPostDTO>> GetBuildingPostsByFilterAsync(BuildingFilter filter)
    {
        try
        {
            var buildingPosts = await _buildingRepository.GetByFilterAsync(filter);
            return _mapper.Map<IEnumerable<BuildingPostDTO>>(buildingPosts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving BuildingPosts by filter.");
            throw;
        }
    }

    public async Task UpdateBuildingPostAsync(Guid id, BuildingPostDTO buildingPostDTO)
    {
        try
        {
            var existingPost = await _buildingRepository.GetByIdAsync(id);
            if (existingPost == null)
            {
                _logger.LogWarning("BuildingPost with Id: {Id} not found for update.", id);
                throw new KeyNotFoundException("BuildingPost not found.");
            }

            _mapper.Map(buildingPostDTO, existingPost);
            existingPost.UpdatedAt = DateTime.UtcNow;
            await _buildingRepository.UpdateAsync(existingPost);

            // Log Kafka message
            _logger.LogInformation("BuildingPost updated with Id: {Id}", id);
            await _producerKafkaService.ProduceAsync(id.ToString(), "BuildingPost updated");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating BuildingPost with Id: {Id}", id);
            throw;
        }
    }

    public async Task DeleteBuildingPostAsync(Guid id)
    {
        try
        {
            var buildingPost = await _buildingRepository.GetByIdAsync(id);
            if (buildingPost == null)
            {
                _logger.LogWarning("BuildingPost with Id: {Id} not found for deletion.", id);
                throw new KeyNotFoundException("BuildingPost not found.");
            }

            await _buildingRepository.DeleteAsync(id);

            // Log Kafka message
            _logger.LogInformation("BuildingPost deleted with Id: {Id}", id);
            await _producerKafkaService.ProduceAsync(id.ToString(), "BuildingPost deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting BuildingPost with Id: {Id}", id);
            throw;
        }
    }
}
