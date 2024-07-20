using AutoMapper;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces.PostServices;
using DataAccessLayer.Interfaces;
using DTO.BuildingPost;
using EntityLayer.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace BusinessLayer.Services.PostServices;

public class PostService :  IPostService
{
    private readonly IPostRepository _buildingPostRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateBuildingPostDto> _createBuildingPostValidator;
    private readonly IValidator<UpdateBuildingPostDto> _updateBuildingPostValidator;
    private readonly ILogger<PostService> _logger;

    public PostService(
        IPostRepository buildingPostRepository,
        IMapper mapper,
        IValidator<CreateBuildingPostDto> createBuildingPostValidator,
        IValidator<UpdateBuildingPostDto> updateBuildingPostValidator,
        ILogger<PostService> logger)
    {
        _buildingPostRepository = buildingPostRepository;
        _mapper = mapper;
        _createBuildingPostValidator = createBuildingPostValidator;
        _updateBuildingPostValidator = updateBuildingPostValidator;
        _logger = logger;
    }

    public async Task<BuildingPostDto> GetBuildingPostByIdAsync(Guid id)
    {
        var buildingPost = await _buildingPostRepository.GetBuildingPostByIdAsync(id);
        if (buildingPost == null)
            throw new NotFoundException("BuildingPost not found.");

        return _mapper.Map<BuildingPostDto>(buildingPost);
    }

    public async Task<IEnumerable<BuildingPostDto>> GetAllBuildingPostsAsync()
    {
        var buildingPosts = await _buildingPostRepository.GetAllBuildingPostsAsync();
        return _mapper.Map<IEnumerable<BuildingPostDto>>(buildingPosts);
    }

    public async Task<IEnumerable<BuildingPostDto>> GetBuildingPostsByFilterAsync(Expression<Func<BuildingPostDto, bool>> filter, int pageNumber, int pageSize)
    {
        var predicate = _mapper.Map<Expression<Func<BuildingPost, bool>>>(filter);
        var buildingPosts = await _buildingPostRepository.GetBuildingPostsByFilterAsync(predicate, pageNumber, pageSize);
        return _mapper.Map<IEnumerable<BuildingPostDto>>(buildingPosts);
    }

    public async Task<BuildingPostDto> CreateBuildingPostAsync(CreateBuildingPostDto createBuildingPostDto)
    {
        var validationResult = await _createBuildingPostValidator.ValidateAsync(createBuildingPostDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning($"CreateBuildingPost validation failed: {string.Join(", ", validationResult.Errors)}");
            throw new ValidationException(validationResult.Errors);
        }

        var buildingPost = _mapper.Map<BuildingPost>(createBuildingPostDto);
        buildingPost.Id = Guid.NewGuid();
        await _buildingPostRepository.AddBuildingPostAsync(buildingPost);

        return _mapper.Map<BuildingPostDto>(buildingPost);
    }

    public async Task<BuildingPostDto> UpdateBuildingPostAsync(Guid id, UpdateBuildingPostDto updateBuildingPostDto)
    {
        var validationResult = await _updateBuildingPostValidator.ValidateAsync(updateBuildingPostDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning($"UpdateBuildingPost validation failed: {string.Join(", ", validationResult.Errors)}");
            throw new ValidationException(validationResult.Errors);
        }

        var buildingPost = await _buildingPostRepository.GetBuildingPostByIdAsync(id);
        if (buildingPost == null)
            throw new NotFoundException("BuildingPost not found.");

        _mapper.Map(updateBuildingPostDto, buildingPost);
        await _buildingPostRepository.UpdateBuildingPostAsync(buildingPost);

        return _mapper.Map<BuildingPostDto>(buildingPost);
    }

    public async Task<bool> DeleteBuildingPostAsync(Guid id)
    {
        var buildingPost = await _buildingPostRepository.GetBuildingPostByIdAsync(id);
        if (buildingPost == null)
            throw new NotFoundException("BuildingPost not found.");

        await _buildingPostRepository.DeleteBuildingPostAsync(buildingPost);
        return true;
    }
}
