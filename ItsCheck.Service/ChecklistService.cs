using Common.DTO;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace ItsCheck.Service
{
    public class ChecklistService : IChecklistService
    {
        private readonly IChecklistRepository _checklistRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IItemRepository _itemRepository;

        public ChecklistService(IChecklistRepository checklistRepository, ICategoryRepository categoryRepository, IItemRepository itemRepository)
        {
            _checklistRepository = checklistRepository;
            _categoryRepository = categoryRepository;
            _itemRepository = itemRepository;
        }

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
                responseDTO.Object = await _checklistRepository.GetEntities().ToListAsync();
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

                foreach (var categoryDTO in checklistDTO.Categories)
                {
                    var category = await _categoryRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == categoryDTO.CategoryId);
                    if (category == null) continue;
                    foreach (var itemDTO in categoryDTO.Items)
                    {
                        var item = await _itemRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == itemDTO.ItemId);
                        if (item == null) continue;
                        checklist.ChecklistItems.Add(new ChecklistItem() { Category = category, Checklist = checklist, Item = item, RequiredQuantity = itemDTO.Quantity });
                    }
                }

                checklist.SetCreatedAt();
                await _checklistRepository.InsertAsync(checklist);
                await _checklistRepository.SaveChangesAsync();
                responseDTO.Object = checklist;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, ChecklistDTO checklistDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklist = await _checklistRepository.GetTrackedEntities().Include(x => x.ChecklistItems).FirstOrDefaultAsync(c => c.Id == id);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {checklistDTO.Name} não existe!");
                    return responseDTO;
                }
                checklist.Name = checklistDTO.Name;
                checklist.ChecklistItems.RemoveAll(x => 1 == 1);

                foreach (var categoryDTO in checklistDTO.Categories)
                {
                    var category = await _categoryRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == categoryDTO.CategoryId);
                    if (category == null) continue;
                    foreach (var itemDTO in categoryDTO.Items)
                    {
                        var item = await _itemRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == itemDTO.ItemId);
                        if (item == null) continue;
                        checklist.ChecklistItems.Add(new ChecklistItem() { Category = category, Checklist = checklist, Item = item, RequiredQuantity = itemDTO.Quantity });
                    }
                }

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
    }
}