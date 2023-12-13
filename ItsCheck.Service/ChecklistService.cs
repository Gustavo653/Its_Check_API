using ItsCheck.Domain;
using ItsCheck.DTO;
using ItsCheck.DTO.Base;
using ItsCheck.Infrastructure.Repository;
using ItsCheck.Infrastructure.Service;
using ItsCheck.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ItsCheck.Service
{
    public class ChecklistService : IChecklistService
    {
        private readonly IChecklistRepository _checklistRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAmbulanceRepository _ambulanceRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChecklistService(IChecklistRepository checklistRepository,
                                ICategoryRepository categoryRepository,
                                IAmbulanceRepository ambulanceRepository,
                                IItemRepository itemRepository,
                                IUserRepository userRepository,
                                IHttpContextAccessor httpContextAccessor)
        {
            _checklistRepository = checklistRepository;
            _categoryRepository = categoryRepository;
            _ambulanceRepository = ambulanceRepository;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public Task<ResponseDTO> Create(BasicDTO basicDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> Update(int id, BasicDTO basicDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO> Remove(int id)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var ambulanceExists = await _ambulanceRepository.GetEntities().AnyAsync(x => x.Checklist.Id == id);
                if (ambulanceExists)
                {
                    responseDTO.SetBadInput("Não é possível apagar o checklist, já existe uma ambulância vinculada!");
                    return responseDTO;
                }

                var checklist = await _checklistRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist com id: {id} não existe!");
                    return responseDTO;
                }

                _checklistRepository.Delete(checklist);
                await _checklistRepository.SaveChangesAsync();
                responseDTO.Object = checklist;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> GetList()
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklists = await _checklistRepository.GetEntities()
                    .Include(x => x.ChecklistItems).ThenInclude(x => x.Item)
                    .Include(x => x.ChecklistItems).ThenInclude(x => x.Category)
                    .ToListAsync();

                var jsonData = checklists.Select(checklist => new
                {
                    id = checklist.Id,
                    checklist.CreatedAt,
                    checklist.UpdatedAt,
                    name = checklist.Name,
                    categories = checklist.ChecklistItems
                            .Select(item => new
                            {
                                id = item.Category.Id,
                                name = item.Category.Name,
                                items = new List<object>
                                {
                                    new
                                    {
                                        id = item.Item.Id,
                                        name = item.Item.Name,
                                        amountRequired = item.AmountRequired
                                    }
                                }
                            })
                }).GroupBy(checklist => new { checklist.id, checklist.name, checklist.CreatedAt, checklist.UpdatedAt })
                    .Select(groupedChecklist => new
                    {
                        groupedChecklist.Key.id,
                        groupedChecklist.Key.CreatedAt,
                        groupedChecklist.Key.UpdatedAt,
                        groupedChecklist.Key.name,
                        categories = groupedChecklist
                            .SelectMany(checklist => checklist.categories)
                            .GroupBy(category => new { category.id, category.name })
                            .Select(groupedCategory => new
                            {
                                groupedCategory.Key.id,
                                groupedCategory.Key.name,
                                items = groupedCategory.SelectMany(category => category.items)
                            })
                    });

                responseDTO.Object = jsonData;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> Create(ChecklistDTO checklistDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklistExists = await _checklistRepository.GetEntities().AnyAsync(c => c.Name == checklistDTO.Name);
                if (checklistExists)
                {
                    responseDTO.SetBadInput($"O checklist {checklistDTO.Name} já existe!");
                    return responseDTO;
                }

                var checklist = new Checklist
                {
                    Name = checklistDTO.Name,
                    ChecklistItems = new List<ChecklistItem>()
                };

                await ProcessChecklistItems(checklistDTO, checklist);

                checklist.SetCreatedAt();
                await _checklistRepository.InsertAsync(checklist);
                await _checklistRepository.SaveChangesAsync();
                responseDTO.Object = checklistDTO;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }

        private async Task ProcessChecklistItems(ChecklistDTO checklistDTO, Checklist checklist)
        {

            foreach (var categoryDTO in checklistDTO.Categories)
            {
                var category = await _categoryRepository.GetTrackedEntities()
                                                        .FirstOrDefaultAsync(x => x.Id == categoryDTO.Id) ??
                                                        throw new Exception($"A categoria {categoryDTO.Id} não existe");
                foreach (var itemDTO in categoryDTO.Items)
                {
                    var item = await _itemRepository.GetTrackedEntities()
                                                    .FirstOrDefaultAsync(x => x.Id == itemDTO.Id) ??
                                                    throw new Exception($"O item {itemDTO.Id} não existe");
                    var checklistItem = new ChecklistItem()
                    {
                        Category = category,
                        Checklist = checklist,
                        Item = item,
                        AmountRequired = itemDTO.AmountRequired,
                        TenantId = Convert.ToInt32(_session.GetString(Consts.ClaimTenantId))
                    };
                    checklistItem.SetCreatedAt();
                    checklist.ChecklistItems.Add(checklistItem);
                }
            }
        }

        public async Task<ResponseDTO> Update(int id, ChecklistDTO checklistDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklist = await _checklistRepository.GetTrackedEntities()
                                                          .Include(x => x.ChecklistItems)
                                                          .FirstOrDefaultAsync(c => c.Id == id);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {checklistDTO.Name} não existe!");
                    return responseDTO;
                }

                checklist.Name = checklistDTO.Name;
                checklist.ChecklistItems.RemoveAll(_ => true);

                await ProcessChecklistItems(checklistDTO, checklist);

                checklist.SetUpdatedAt();
                await _checklistRepository.SaveChangesAsync();

                responseDTO.Object = checklist;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> GetById(int id)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklist = await _checklistRepository.GetEntities()
                    .Include(x => x.ChecklistItems).ThenInclude(x => x.Item)
                    .Include(x => x.ChecklistItems).ThenInclude(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (checklist == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                var jsonData = new
                {
                    id = checklist.Id,
                    checklist.CreatedAt,
                    checklist.UpdatedAt,
                    name = checklist.Name,
                    categories = checklist.ChecklistItems
                        .Select(item => new
                        {
                            id = item.Category.Id,
                            name = item.Category.Name,
                            items = new List<object>
                            {
                                new
                                {
                                    id = item.Item.Id,
                                    name = item.Item.Name,
                                    amountRequired = item.AmountRequired
                                }
                            }
                        })
                        .GroupBy(category => new { category.id, category.name })
                        .Select(groupedCategory => new
                        {
                            groupedCategory.Key.id,
                            groupedCategory.Key.name,
                            items = groupedCategory.SelectMany(category => category.items)
                        })
                };

                responseDTO.Object = jsonData;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }

        public async Task<ResponseDTO> GetByAmbulanceId()
        {
            ResponseDTO responseDTO = new();
            try
            {
                var user = await _userRepository.GetEntities()
                    .Include(x => x.Ambulance).ThenInclude(x => x.Checklist).ThenInclude(x => x.ChecklistItems).ThenInclude(x => x.Category)
                    .Include(x => x.Ambulance).ThenInclude(x => x.Checklist).ThenInclude(x => x.ChecklistItems).ThenInclude(x => x.Item)
                        .FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(_session.GetString(Consts.ClaimUserId)));

                if (user == null || user.Ambulance == null || user.Ambulance.Checklist == null)
                {
                    responseDTO.SetNotFound();
                    return responseDTO;
                }

                var jsonData = new
                {
                    id = user.Ambulance?.Checklist.Id,
                    name = user.Ambulance?.Checklist.Name,
                    categories = user.Ambulance?.Checklist.ChecklistItems
                        .Select(item => new
                        {
                            id = item.Category.Id,
                            name = item.Category.Name,
                            items = new List<object>
                            {
                                new
                                {
                                    id = item.Item.Id,
                                    name = item.Item.Name,
                                    amountRequired = item.AmountRequired
                                }
                            }
                        })
                        .GroupBy(category => new { category.id, category.name })
                        .Select(groupedCategory => new
                        {
                            groupedCategory.Key.id,
                            groupedCategory.Key.name,
                            items = groupedCategory.SelectMany(category => category.items)
                        })
                };

                responseDTO.Object = jsonData;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }
    }
}