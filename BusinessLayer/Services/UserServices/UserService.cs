using AutoMapper;
using BusinessLayer.Interfaces.UserServices;
using DataAccessLayer.Interfaces;
using DTO.User;
using EntityLayer.Entities;
using FluentValidation;
using Serilog;

namespace BusinessLayer.Services.UserServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<UserDto> _createUserValidator;

    public UserService(IUserRepository userRepository, IMapper mapper,
                       IValidator<UserDto> createUserValidator)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _createUserValidator = createUserValidator;
    }

    public async Task<User> CreateUserAsync(UserDto userDto)
    {
        try
        {
            var validationResult = await _createUserValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                Log.Warning($"CreateUserAsync validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}");
                throw new ValidationException(validationResult.Errors);
            }

            var user = _mapper.Map<User>(userDto);
            await _userRepository.AddUserAsync(user);
            Log.Information($"User created successfully with ID: {user.Id}");
            return user;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, $"An error occurred while creating a user with Email: {userDto.Email}");
            throw;
        }
    }

    public async Task<User> UpdateUserbyIdAsync(Guid userId, UserDto userDto)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                Log.Warning($"User with ID: {userId} not found");
                throw new ArgumentException("User not found.");
            }

            _mapper.Map(userDto, user);
            await _userRepository.UpdateUserByIdAsync(user);
            Log.Information($"User updated successfully with ID: {userId}");
            return user;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, $"An error occurred while updating a user with ID: {userId}");
            throw;
        }
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();
            Log.Information($"Retrieved {users.Count()} users");
            return users;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving all users");
            throw;
        }
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                Log.Warning($"User with ID: {userId} not found");
                throw new ArgumentException("User not found.");
            }

            Log.Information($"User retrieved with ID: {userId}");
            return user;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, $"An error occurred while retrieving a user with ID: {userId}");
            throw;
        }
    }

    public async Task<User> GetUserByContactNumberAsync(string contactNumber)
    {
        try
        {
            var user = await _userRepository.GetUserByPredicateAsync(u => u.ContactNumber == contactNumber);
            if (user == null)
            {
                Log.Warning($"User with contact number: {contactNumber} not found");
                throw new ArgumentException("User not found.");
            }

            Log.Information($"User retrieved with contact number: {contactNumber}");
            return user;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, $"An error occurred while retrieving a user with contact number: {contactNumber}");
            throw;
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await _userRepository.GetUserByPredicateAsync(u => u.Email == email);
            if (user == null)
            {
                Log.Warning($"User with email: {email} not found");
                throw new ArgumentException("User not found.");
            }

            Log.Information($"User retrieved with email: {email}");
            return user;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, $"An error occurred while retrieving a user with email: {email}");
            throw;
        }
    }

    public async Task<bool> DeleteUserbyIdAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                Log.Warning($"User with ID: {userId} not found");
                throw new ArgumentException("User not found.");
            }

            await _userRepository.DeleteUserByIdAsync(user);
            Log.Information($"User deleted successfully with ID: {userId}");
            return true;
        }
        catch (System.Exception ex)
        {
            Log.Error(ex, $"An error occurred while deleting a user with ID: {userId}");
            throw;
        }
    }
}
