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
            _logger.LogInformation($"AddBuildingPostAsync method called with BuildingPostDTO: {buildingPostDTO}");

            var building = _mapper.Map<Building>(buildingPostDTO.Building);
            var buildingPost = new BuildingPost
            {
                Building = building,
                IsActive = buildingPostDTO.IsActive,
                CreatedAt = buildingPostDTO.CreatedAt,
                UpdatedAt = buildingPostDTO.UpdatedAt
            };
            await _buildingRepository.AddAsync(buildingPost);

            _logger.LogInformation($"Successfully added BuildingPost with Id: {buildingPost.Building.Id}");

            await _producerKafkaService.ProduceAsync(buildingPost.Building.Id.ToString(), "BuildingPost added");
            _logger.LogInformation($"Kafka message sent for BuildingPost added with Id: {buildingPost.Building.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while adding BuildingPost with BuildingPostDTO: {buildingPostDTO}");
            throw;
        }
    }

    public async Task<BuildingPostDTO> GetBuildingPostByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"GetBuildingPostByIdAsync method called with Id: {id}");

            var buildingPost = await _buildingRepository.GetByIdAsync(id);
            if (buildingPost == null)
            {
                _logger.LogWarning($"BuildingPost with Id: {id} not found.");
                throw new KeyNotFoundException("BuildingPost not found.");
            }

            _logger.LogInformation($"Successfully retrieved BuildingPost with Id: {id}");
            return _mapper.Map<BuildingPostDTO>(buildingPost);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving BuildingPost with Id: {id}");
            throw;
        }
    }

    public async Task<IEnumerable<BuildingPostDTO>> GetAllBuildingPostsAsync()
    {
        try
        {
            _logger.LogInformation("GetAllBuildingPostsAsync method called.");

            var buildingPosts = await _buildingRepository.GetAllAsync();
            _logger.LogInformation($"Successfully retrieved all BuildingPosts. Count: {buildingPosts.Count()}");

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
            _logger.LogInformation($"GetBuildingPostsByFilterAsync method called with filter: {filter}");

            var buildingPosts = await _buildingRepository.GetByFilterAsync(filter);
            _logger.LogInformation($"Successfully retrieved BuildingPosts by filter. Count: {buildingPosts.Count()}");

            return _mapper.Map<IEnumerable<BuildingPostDTO>>(buildingPosts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving BuildingPosts by filter: {filter}");
            throw;
        }
    }

    public async Task UpdateBuildingPostAsync(Guid id, BuildingPostDTO buildingPostDTO)
    {
        try
        {
            _logger.LogInformation($"UpdateBuildingPostAsync method called with Id: {id} and BuildingPostDTO: {buildingPostDTO}");

            var existingPost = await _buildingRepository.GetByIdAsync(id);
            if (existingPost == null)
            {
                _logger.LogWarning($"BuildingPost with Id: {id} not found for update.");
                throw new KeyNotFoundException("BuildingPost not found.");
            }

            _mapper.Map(buildingPostDTO, existingPost);
            existingPost.UpdatedAt = DateTime.UtcNow;
            await _buildingRepository.UpdateAsync(existingPost);

            _logger.LogInformation($"Successfully updated BuildingPost with Id: {id}");

            await _producerKafkaService.ProduceAsync(id.ToString(), "BuildingPost updated");
            _logger.LogInformation($"Kafka message sent for BuildingPost updated with Id: {id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while updating BuildingPost with Id: {id} and BuildingPostDTO: {buildingPostDTO}");
            throw;
        }
    }

    public async Task DeleteBuildingPostAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"DeleteBuildingPostAsync method called with Id: {id}");

            var buildingPost = await _buildingRepository.GetByIdAsync(id);
            if (buildingPost == null)
            {
                _logger.LogWarning($"BuildingPost with Id: {id} not found for deletion.");
                throw new KeyNotFoundException("BuildingPost not found.");
            }

            await _buildingRepository.DeleteAsync(id);

            _logger.LogInformation($"Successfully deleted BuildingPost with Id: {id}");

            await _producerKafkaService.ProduceAsync(id.ToString(), "BuildingPost deleted");
            _logger.LogInformation($"Kafka message sent for BuildingPost deleted with Id: {id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting BuildingPost with Id: {id}");
            throw;
        }
    }

    public async Task<IEnumerable<BuildingPostDTO>> GetBuildingPostsByPaginationAsync(int pageNumber, int pageSize)
    {
        try
        {
            _logger.LogInformation($"GetBuildingPostsByPaginationAsync method called with pageNumber: {pageNumber} and pageSize: {pageSize}");

            var buildingPosts = await _buildingRepository.GetByPaginationAsync(pageNumber, pageSize);
            _logger.LogInformation($"Successfully retrieved paginated BuildingPosts. Count: {buildingPosts.Count()}");

            return _mapper.Map<IEnumerable<BuildingPostDTO>>(buildingPosts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while retrieving paginated BuildingPosts with pageNumber: {pageNumber} and pageSize: {pageSize}");
            throw;
        }
    }

    public async Task<IEnumerable<BuildingPostDTO>> SearchBuildingPostsAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation($"SearchBuildingPostsAsync method called with searchTerm: {searchTerm}");

            var buildingPosts = await _buildingRepository.SearchAsync(searchTerm);
            _logger.LogInformation($"Successfully searched BuildingPosts with searchTerm: {searchTerm}. Count: {buildingPosts.Count()}");

            return _mapper.Map<IEnumerable<BuildingPostDTO>>(buildingPosts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while searching BuildingPosts with searchTerm: {searchTerm}");
            throw;
        }
    }
}
