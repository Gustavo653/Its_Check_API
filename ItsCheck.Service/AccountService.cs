using Common.DTO;
using Common.Functions;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain.Enum;
using ItsCheck.Domain.Identity;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ItsCheck.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        public AccountService(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IUserRepository userRepository,
                              ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        private async Task<SignInResult> CheckUserPassword(User user, UserLoginDTO userLoginDTO)
        {
            try
            {
                return await _signInManager.CheckPasswordSignInAsync(user, userLoginDTO.Password, false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar senha do usuário. Erro: {ex.Message}");
            }
        }

        private async Task<User?> GetUserByUserName(string userName)
        {
            try
            {
                return await _userRepository.GetEntities()
                                            .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                                            .FirstOrDefaultAsync(x => x.NormalizedUserName == userName.ToUpper());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter o usuário. Erro: {ex.Message}");
            }
        }

        public async Task<ResponseDTO> Login(UserLoginDTO userDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var user = await GetUserByUserName(userDTO.UserName);
                if (user == null)
                {
                    responseDTO.Code = 401;
                    responseDTO.Message = "Não autenticado!";
                    return responseDTO;
                }
                var password = await CheckUserPassword(user, userDTO);
                if (!password.Succeeded)
                {
                    responseDTO.Code = 401;
                    responseDTO.Message = $"Não autenticado! {password}";
                    return responseDTO;
                }

                responseDTO.Object = new
                {
                    userName = user.UserName,
                    name = user.Name,
                    email = user.Email,
                    token = await _tokenService.CreateToken(user)
                };
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetCurrent(string userName)
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await GetUserByUserName(userName);
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> CreateUser(UserDTO userDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var user = await _userManager.FindByNameAsync(userDTO.UserName);
                if (user != null)
                {
                    responseDTO.SetBadInput($"Já existe um usuário cadastrado com este e-mail: {userDTO.Email}!");
                    return responseDTO;
                }
                if (userDTO.Password == null)
                {
                    responseDTO.SetBadInput($"A senha deve ser preenchida");
                    return responseDTO;
                }
                var userEntity = new User();
                PropertyCopier<UserDTO, User>.Copy(userDTO, userEntity);
                await _userManager.CreateAsync(userEntity, userDTO.Password);
                foreach (var item in userDTO.Roles)
                    await AddUserInRole(userEntity, item);
                responseDTO.Object = userEntity;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> UpdateUser(int id, UserDTO userDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var userEntity = await _userManager.FindByIdAsync(id.ToString());
                if (userEntity == null)
                {
                    responseDTO.SetBadInput($"Usuário não encotrado com este id: {id}!");
                    return responseDTO;
                }
                PropertyCopier<UserDTO, User>.Copy(userDTO, userEntity);
                await _userManager.UpdateAsync(userEntity);
                if (userDTO.Password != null)
                {
                    await _userManager.RemovePasswordAsync(userEntity);
                    await _userManager.AddPasswordAsync(userEntity, userDTO.Password);
                }
                var userRoles = await _userManager.GetRolesAsync(userEntity);
                await _userManager.RemoveFromRolesAsync(userEntity, userRoles);
                foreach (var item in userDTO.Roles)
                    await AddUserInRole(userEntity, item);
                responseDTO.Object = userEntity;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> RemoveUser(int id)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var userEntity = await _userManager.FindByIdAsync(id.ToString());
                if (userEntity == null)
                {
                    responseDTO.SetBadInput($"Usuário não encontrado com este id: {id}!");
                    return responseDTO;
                }
                var userRoles = await _userManager.GetRolesAsync(userEntity);
                await _userManager.RemoveFromRolesAsync(userEntity, userRoles);
                await _userManager.DeleteAsync(userEntity);
                responseDTO.Object = userEntity;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        private async Task AddUserInRole(User user, RoleName role)
        {
            if (!await _userManager.IsInRoleAsync(user, role.ToString()))
                await _userManager.AddToRoleAsync(user, role.ToString());
        }

        public async Task<ResponseDTO> GetUsers()
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await _userManager.Users.Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Email,
                    x.UserName,
                    roles = string.Join(",", x.UserRoles.Select(ur => ur.Role.NormalizedName))
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}