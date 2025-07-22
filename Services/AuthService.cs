using Microsoft.AspNetCore.Identity;
using CSharpServer.Models;

namespace CSharpServer.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenService _tokenService;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            
            if (!result.Succeeded)
            {
                return null;
            }

            return CreateAuthResponse(user);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            User? user = null;

            // Check if input is email or username
            if (loginDto.UsernameOrEmail.Contains('@'))
            {
                user = await _userManager.FindByEmailAsync(loginDto.UsernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(loginDto.UsernameOrEmail);
            }

            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            
            if (!result.Succeeded)
            {
                return null;
            }

            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return CreateAuthResponse(user);
        }

        private AuthResponseDto CreateAuthResponse(User user)
        {
            var token = _tokenService.GenerateToken(user);
            var expirationHours = 24; // Default to 24 hours

            return new AuthResponseDto
            {
                Token = token,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                ExpiresAt = DateTime.UtcNow.AddHours(expirationHours)
            };
        }
    }
}