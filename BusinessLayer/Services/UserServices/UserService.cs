using AutoMapper;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces.UserServices;
using DataAccessLayer.Interfaces;
using DTO.User;
using EntityLayer.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services.UserServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateUserDto> _createUserValidator;
    private readonly IValidator<UpdateUserDto> _updateUserValidator;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        IValidator<CreateUserDto> createUserValidator,
        IValidator<UpdateUserDto> updateUserValidator,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _createUserValidator = createUserValidator;
        _updateUserValidator = updateUserValidator;
        _logger = logger;
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found.");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        var validationResult = await _createUserValidator.ValidateAsync(createUserDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning($"User creation validation failed: {string.Join(", ", validationResult.Errors)}");
            throw new ValidationException(validationResult.Errors);
        }

        var user = _mapper.Map<User>(createUserDto);
        user.Id = Guid.NewGuid();
        await _userRepository.AddUserAsync(user);

        _logger.LogInformation($"User created successfully with ID: {user.Id}");
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateUserbyIdAsync(Guid userId, UpdateUserDto updateUserDto)
    {
        var validationResult = await _updateUserValidator.ValidateAsync(updateUserDto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning($"User update validation failed: {string.Join(", ", validationResult.Errors)}");
            throw new ValidationException(validationResult.Errors);
        }

        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found.");

        _mapper.Map(updateUserDto, user);
        await _userRepository.UpdateUserAsync(user);

        _logger.LogInformation($"User updated successfully with ID: {userId}");
        return _mapper.Map<UserDto>(user);
    }

    public async Task<bool> DeleteUserbyIdAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found.");

        await _userRepository.DeleteUserAsync(user);
        _logger.LogInformation($"User deleted successfully with ID: {userId}");
        return true;
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByPredicateAsync(u => u.UserMail.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (user == null)
            return null;

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetUserByContactNumberAsync(string contactNumber)
    {
        var user = await _userRepository.GetByPredicateAsync(u => u.ContactNumber.Equals(contactNumber, StringComparison.OrdinalIgnoreCase));
        if (user == null)
            return null;

        return _mapper.Map<UserDto>(user);
    }
}
