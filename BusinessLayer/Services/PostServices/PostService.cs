using AutoMapper;
using BusinessLayer.Interfaces.PostServices;
using DataAccessLayer.Interfaces;
using DTO.BuildingPost;
using EntityLayer.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Linq.Expressions;

namespace BusinessLayer.Services.PostServices;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<BuildingPostDto> _createBuildingPostValidator;

    public PostService(IPostRepository postRepository, IMapper mapper,
                       IValidator<BuildingPostDto> createBuildingPostValidator)
    {
        _postRepository = postRepository;
        _mapper = mapper;
        _createBuildingPostValidator = createBuildingPostValidator;
    }

    public async Task<BuildingPost> GetBuildingPostByIdAsync(Guid id)
    {
        try
        {
            var post = await _postRepository.GetBuildingPostByIdAsync(id);
            if (post == null)
            {
                Log.Warning($"Building post with ID: {id} not found");
                throw new ArgumentException("Building post not found.");
            }

            Log.Information($"Building post retrieved with ID: {id}");
            return post;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, $"An error occurred while retrieving building post by ID: {id}");
            throw;
        }
    }

    public async Task<IEnumerable<BuildingPost>> GetAllBuildingPostsAsync()
    {
        try
        {
            var posts = await _postRepository.GetAllBuildingPostsAsync();
            Log.Information($"Retrieved {posts.Count()} building posts");
            return posts;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving all building posts");
            throw;
        }
    }

    public async Task<IEnumerable<BuildingPost>> GetBuildingPostsByFilterAsync(Expression<Func<BuildingPostDto, bool>> filter, int pageNumber, int pageSize)
    {
        try
        {
            var predicate = _mapper.Map<Expression<Func<BuildingPost, bool>>>(filter);
            var posts = await _postRepository.GetBuildingPostsByFilterAsync(predicate, pageNumber, pageSize);
            Log.Information($"Retrieved {posts.Count()} building posts with specified filter");
            return posts;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving building posts by filter");
            throw;
        }
    }

    public async Task<BuildingPost> CreateBuildingPostAsync(BuildingPostDto buildingPostDto, IList<IFormFile> files)
    {
        try
        {
            var validationResult = await _createBuildingPostValidator.ValidateAsync(buildingPostDto);
            if (!validationResult.IsValid)
            {
                Log.Warning($"CreateBuildingPostAsync validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}");
                throw new ValidationException(validationResult.Errors);
            }

            var post = _mapper.Map<BuildingPost>(buildingPostDto);
            post.ImageUrls = files.Select(file => file.Name).ToList(); 
            await _postRepository.AddBuildingPostAsync(post);
            Log.Information($"Building post created successfully with ID: {post.Id}");
            return post;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, "An error occurred while creating a building post");
            throw;
        }
    }

    public async Task<BuildingPost> UpdateBuildingPostByIdAsync(Guid id, BuildingPostDto buildingPostDto)
    {
        try
        {
            var post = await _postRepository.GetBuildingPostByIdAsync(id);
            if (post == null)
            {
                Log.Warning($"Building post with ID: {id} not found");
                throw new ArgumentException("Building post not found.");
            }

            _mapper.Map(buildingPostDto, post);
            await _postRepository.UpdateBuildingPostByIdAsync(post);
            Log.Information($"Building post updated successfully with ID: {id}");
            return post;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, $"An error occurred while updating building post with ID: {id}");
            throw;
        }
    }

    public async Task<bool> DeleteBuildingPostByIdAsync(Guid id)
    {
        try
        {
            var post = await _postRepository.GetBuildingPostByIdAsync(id);
            if (post == null)
            {
                Log.Warning($"Building post with ID: {id} not found");
                throw new ArgumentException("Building post not found.");
            }

            await _postRepository.DeleteBuildingPostByIdAsync(post);
            Log.Information($"Building post deleted successfully with ID: {id}");
            return true;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, $"An error occurred while deleting building post with ID: {id}");
            throw;
        }
    }
}
