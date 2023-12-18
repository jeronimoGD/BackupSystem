using AutoMapper;
using Azure;
using BackupSystem.ApplicationSettings;
using BackupSystem.Common.Interfaces.Repository;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Common.Repository;
using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.LoginDTO;
using BackupSystem.DTO.RegisterDTO;
using BackupSystem.DTO.UserDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BackupSystem.Common.Services
{
    public class UserService : BaseEntityService<ApplicationUser>, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly APISettings _apiSettings;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IOptions<APISettings> apiSettings, IUnitOfWork unitOfWork) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiSettings = apiSettings.Value;
        }

        public async Task<bool> DoesUserExists(string userName)
        {
            var usuario = await _userManager.FindByNameAsync(userName);
            return usuario != null;
        }


        public async Task<APIResponse> GetUser(Expression<Func<ApplicationUser, bool>>? filtro = null)
        {
            APIResponse response = new APIResponse();

            try
            {
                var usrByName = await _unitOfWork.ApplicationUsers.Get(filtro);

                if (usrByName != null)
                {
                    response = APIResponse.Ok(usrByName);
                }
                else
                {
                    response = APIResponse.NotFound($"No user found.");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<APIResponse> UpdateUser(ApplicationUserUpdateDTO updateDTO, Expression<Func<ApplicationUser, bool>>? filtro = null)
        {
            APIResponse response = new APIResponse();

            try
            {
                var userToUpdate = await _unitOfWork.ApplicationUsers.Get(filtro);

                if (userToUpdate != null)
                {
                    var isPasswordOk = await _userManager.CheckPasswordAsync(userToUpdate, updateDTO.CurrentPassword);

                    if (isPasswordOk)
                    {
                        ApplicationUser newUserData = _mapper.Map<ApplicationUser>(updateDTO);
                        userToUpdate.Id = userToUpdate.Id;
                        await _unitOfWork.ApplicationUsers.Update(newUserData);
                        response = APIResponse.Ok(newUserData);
                    }
                    else
                    {
                        response = APIResponse.BadRequest(updateDTO, $"Wrong Password.");
                    }
                }
                else
                {
                    response = APIResponse.NotFound($"No user found");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<APIResponse> LogIn(LoginRequestDTO loginRequestDTO)
        {

            APIResponse response = new APIResponse();

            try
            {
                LoginResponseDTO loginResponse = new LoginResponseDTO();

                // _signInManager.SignInAsync
                var logedUser = await _userManager.FindByNameAsync(loginRequestDTO.UserName);

                if (logedUser != null)
                {
                    var isPasswordOk = await _userManager.CheckPasswordAsync(logedUser, loginRequestDTO.Password);

                    if (isPasswordOk)
                    {
                        var roles = await _userManager.GetRolesAsync(logedUser);
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_apiSettings.JwtAuthFields.Key);
                        var tokenDecsriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.NameIdentifier, logedUser.Id.ToString()),
                                new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                            }),
                            Expires = DateTime.UtcNow.AddDays(1),
                            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

                        };

                        var token = tokenHandler.CreateToken(tokenDecsriptor);

                        loginResponse = new LoginResponseDTO()
                        {
                            UserName = logedUser.UserName,
                            Token = tokenHandler.WriteToken(token),
                            Role = roles.FirstOrDefault()
                        };

                        response = APIResponse.Ok(loginResponse);
                    }
                    else
                    {
                        response = APIResponse.BadRequest(loginRequestDTO, "Wrong password.");
                    }
                }
                else
                {
                    response = APIResponse.NotFound("User does not exists.");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<APIResponse> Register(RegisterRequestDTO registerRequestDTO)
        {
            APIResponse response = new APIResponse();

            try
            {
                var queryResponse = new IdentityResult { };

                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                }

                ApplicationUser newUser = new ApplicationUser
                {
                    UserName = registerRequestDTO.UserName,
                    Email = registerRequestDTO.Email
                };

                queryResponse = await _userManager.CreateAsync(newUser, registerRequestDTO.Password);

                if (queryResponse.Succeeded)
                {
                    queryResponse = await _userManager.AddToRoleAsync(newUser, "admin");

                    if (queryResponse.Succeeded)
                    {
                        var usuarioApp = await _userManager.FindByNameAsync(registerRequestDTO.UserName);
                        response = APIResponse.Ok(usuarioApp);
                    }
                    else
                    {
                        response = APIResponse.BadRequest(registerRequestDTO, queryResponse.Errors.ToString());
                    }
                }
                else
                {
                    response = APIResponse.BadRequest(registerRequestDTO, queryResponse.Errors.ToString());
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }
    }
}
