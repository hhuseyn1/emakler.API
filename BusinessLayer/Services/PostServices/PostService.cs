using AutoMapper;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces.PostServices;
using DataAccessLayer.Interfaces;
using DTO.BuildingPost;
using EntityLayer.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace BusinessLayer.Services.PostServices;

public class PostService : IPostService
{
    private readonly IPostRepository _buildingPostRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateBuildingPostDto> _createBuildingPostValidator;
    private readonly IValidator<UpdateBuildingPostDto> _updateBuildingPostValidator;
    private readonly ILogger<PostService> _logger;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public PostService(
        IPostRepository buildingPostRepository,
        IMapper mapper,
        IValidator<CreateBuildingPostDto> createBuildingPostValidator,
        IValidator<UpdateBuildingPostDto> updateBuildingPostValidator,
        ILogger<PostService> logger,
        IWebHostEnvironment hostingEnvironment)
    {
        _buildingPostRepository = buildingPostRepository;
        _mapper = mapper;
        _createBuildingPostValidator = createBuildingPostValidator;
        _updateBuildingPostValidator = updateBuildingPostValidator;
        _logger = logger;
        _hostingEnvironment = hostingEnvironment;
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

    public async Task<BuildingPostDto> CreateBuildingPostAsync(CreateBuildingPostDto createBuildingPostDto, IList<IFormFile> images)
    {
        var validationResult = await _createBuildingPostValidator.ValidateAsync(createBuildingPostDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning($"CreateBuildingPost validation failed: {string.Join(", ", validationResult.Errors)}");
            throw new ValidationException(validationResult.Errors);
        }

        if (images.Any(img => img != null && !IsImageFile(img)))
        {
            _logger.LogWarning("Invalid file type. Only image files are allowed.");
            throw new ValidationException("Invalid file type. Only image files are allowed.");
        }

        var buildingPost = _mapper.Map<BuildingPost>(createBuildingPostDto);
        buildingPost.Id = Guid.NewGuid();

        if (images != null && images.Count > 0)
        {
            var imagePaths = new List<string>();
            foreach (var image in images)
            {
                var filePath = Path.Combine("uploads", buildingPost.Id.ToString(), image.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                imagePaths.Add(filePath);
            }
            buildingPost.ImagePaths = imagePaths;
        }

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

    private bool IsImageFile(IFormFile file)
    {
        var validImageMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/svg+xml" };
        var validImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg" };

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return validImageMimeTypes.Contains(file.ContentType) && validImageExtensions.Contains(fileExtension);
    }
}
