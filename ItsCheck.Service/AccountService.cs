using ItsCheck.Domain;
using ItsCheck.Domain.Enum;
using ItsCheck.Domain.Identity;
using ItsCheck.DTO;
using ItsCheck.DTO.Base;
using ItsCheck.Infrastructure.Repository;
using ItsCheck.Infrastructure.Service;
using ItsCheck.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ItsCheck.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IAmbulanceRepository _ambulanceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountService(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IUserRepository userRepository,
                              ITokenService tokenService,
                              IAmbulanceRepository ambulanceRepository,
                              IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _ambulanceRepository = ambulanceRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession _session => _httpContextAccessor.HttpContext.Session;

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

        private async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                return await _userRepository.GetEntities()
                    .Include(x => x.Ambulance)
                    .Include(x => x.Tenant)
                    .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync(x => x.NormalizedEmail == email.ToUpper());
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
                var user = await GetUserByEmail(userDTO.Email);
                if (user == null)
                {
                    responseDTO.Code = 401;
                    responseDTO.Message = "Não autenticado! Email inexistente!";
                    return responseDTO;
                }

                var password = await CheckUserPassword(user, userDTO);
                if (!password.Succeeded)
                {
                    responseDTO.Code = 401;
                    responseDTO.Message = $"Não autenticado! {password}!";
                    return responseDTO;
                }

                responseDTO.Object = new
                {
                    userName = user.UserName,
                    role = user.UserRoles?.FirstOrDefault()?.Role.Name,
                    name = user.Name,
                    email = user.Email,
                    ambulance = user.Ambulance,
                    token = await _tokenService.CreateToken(user)
                };
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> GetCurrent()
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await GetUserByEmail(_session.GetString(Consts.ClaimEmail));
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
                if (userDTO.Role == RoleName.Admin)
                {
                    var requestUser = await _userManager.FindByIdAsync(_session.GetString(Consts.ClaimUserId));
                    var requestUserRoleAdmin = await _userManager.IsInRoleAsync(requestUser!, RoleName.Admin.ToString());
                    if (!requestUserRoleAdmin)
                    {
                        responseDTO.Code = 403;
                        return responseDTO;
                    }
                    userDTO.IdAmbulance = null;
                    userDTO.IdTenant = null;
                }

                var user = await _userManager.FindByEmailAsync(userDTO.Email);
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

                var ambulance = await _ambulanceRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == userDTO.IdAmbulance);
                if (RoleName.Employee == userDTO.Role)
                {
                    if (ambulance == null)
                    {
                        responseDTO.SetBadInput($"A ambulância {userDTO.IdAmbulance} não existe!");
                        return responseDTO;
                    }
                }

                var userEntity = new User
                {
                    Name = userDTO.Name,
                    Ambulance = ambulance,
                    NormalizedEmail = userDTO.Email.ToUpper(),
                    NormalizedUserName = userDTO.Email.ToUpper(),
                    TenantId = userDTO.IdTenant
                };
                userEntity.PasswordHash = _userManager.PasswordHasher.HashPassword(userEntity, userDTO.Password);

                PropertyCopier<UserDTO, User>.Copy(userDTO, userEntity);

                await _userRepository.InsertAsync(userEntity);
                await _userRepository.SaveChangesAsync();
                await _userManager.UpdateSecurityStampAsync(userEntity);

                await AddUserInRole(userEntity, userDTO.Role);
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
                if (userDTO.Role == RoleName.Admin)
                {
                    var requestUser = await _userManager.FindByIdAsync(_session.GetString(Consts.ClaimUserId));
                    var requestUserRoleAdmin = await _userManager.IsInRoleAsync(requestUser!, RoleName.Admin.ToString());
                    if (!requestUserRoleAdmin)
                    {
                        responseDTO.Code = 403;
                        return responseDTO;
                    }
                    userDTO.IdAmbulance = null;
                    userDTO.IdTenant = null;
                }

                var userEntity = await _userRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == id);
                if (userEntity == null)
                {
                    responseDTO.SetBadInput($"Usuário não encotrado com este id: {id}!");
                    return responseDTO;
                }

                if (RoleName.Employee == userDTO.Role)
                {
                    var ambulance = await _ambulanceRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == userDTO.IdAmbulance);
                    if (ambulance == null)
                    {
                        responseDTO.SetBadInput($"A ambulância {userDTO.IdAmbulance} não existe!");
                        return responseDTO;
                    }
                    userEntity.Ambulance = ambulance;
                }

                PropertyCopier<UserDTO, User>.Copy(userDTO, userEntity);
                userEntity.TenantId = userDTO.IdTenant;

                if (userDTO.Password != null)
                    userEntity.PasswordHash = _userManager.PasswordHasher.HashPassword(userEntity, userDTO.Password);

                var userRoles = await _userManager.GetRolesAsync(userEntity);
                await _userManager.RemoveFromRolesAsync(userEntity, userRoles);
                await AddUserInRole(userEntity, userDTO.Role);

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
                _session.TryGetValue("tenantId", out byte[] tenantId);
                responseDTO.Object = await _userRepository.GetEntities()
                                                          .Where(x => Encoding.UTF8.GetString(tenantId) == string.Empty ||
                                                                      Encoding.UTF8.GetString(tenantId) == x.TenantId.ToString())
                                                          .Select(x => new
                                                          {
                                                              x.Id,
                                                              x.Name,
                                                              x.Email,
                                                              x.UserName,
                                                              x.Ambulance,
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