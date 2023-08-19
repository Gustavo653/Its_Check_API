using Common.DTO;
using Microsoft.EntityFrameworkCore;
using ItsCheck.Domain;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;
using ItsCheck.DataAccess.Interface;

namespace ItsCheck.Service
{
    public class ChecklistItemService : IChecklistItemService
    {
        private readonly IChecklistItemRepository _checklistItemRepository;
        private readonly IChecklistRepository _checklistRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ChecklistItemService(IChecklistItemRepository checklistItemRepository,
                                    IChecklistRepository checklistRepository,
                                    IItemRepository itemRepository,
                                    ICategoryRepository categoryRepository)
        {
            _checklistItemRepository = checklistItemRepository;
            _checklistRepository = checklistRepository;
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ResponseDTO> Create(ChecklistItemDTO checklistItemDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklist = await _checklistRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistItemDTO.IdChecklist);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {checklistItemDTO.IdChecklist} não existe!");
                    return responseDTO;
                }
                var item = await _itemRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistItemDTO.IdItem);
                if (item == null)
                {
                    responseDTO.SetBadInput($"O item {checklistItemDTO.IdItem} não existe!");
                    return responseDTO;
                }
                var category = await _categoryRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistItemDTO.IdCategory);
                if (category == null)
                {
                    responseDTO.SetBadInput($"A categoria {checklistItemDTO.IdCategory} não existe!");
                    return responseDTO;
                }

                var checklistItem = new ChecklistItem
                {
                    RequiredQuantity = checklistItemDTO.RequiredQuantity,
                    Checklist = checklist,
                    Item = item,
                    Category = category,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                await _checklistItemRepository.InsertAsync(checklistItem);
                await _checklistItemRepository.SaveChangesAsync();
                responseDTO.Object = checklistItem;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, ChecklistItemDTO checklistItemDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklistItem = await _checklistItemRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (checklistItem == null)
                {
                    responseDTO.SetBadInput($"O item do checklist {id} não existe!");
                    return responseDTO;
                }

                var checklist = await _checklistRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistItemDTO.IdChecklist);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {checklistItemDTO.IdChecklist} não existe!");
                    return responseDTO;
                }
                var item = await _itemRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistItemDTO.IdItem);
                if (item == null)
                {
                    responseDTO.SetBadInput($"O item {checklistItemDTO.IdItem} não existe!");
                    return responseDTO;
                }
                var category = await _categoryRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistItemDTO.IdCategory);
                if (category == null)
                {
                    responseDTO.SetBadInput($"A categoria {checklistItemDTO.IdCategory} não existe!");
                    return responseDTO;
                }

                checklistItem.RequiredQuantity = checklistItemDTO.RequiredQuantity;
                checklistItem.Checklist = checklist;
                checklistItem.Item = item;
                checklistItem.Category = category;
                checklistItem.UpdatedAt = DateTime.Now;
                checklistItem.Checklist = checklist;

                await _checklistItemRepository.SaveChangesAsync();
                responseDTO.Object = checklistItem;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Remove(int id)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklistItem = await _checklistItemRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (checklistItem == null)
                {
                    responseDTO.SetBadInput($"A ambulância com id: {id} não existe!");
                    return responseDTO;
                }
                _checklistItemRepository.Delete(checklistItem);
                await _checklistItemRepository.SaveChangesAsync();
                responseDTO.Object = checklistItem;
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
                responseDTO.Object = await _checklistItemRepository.GetEntities().ToListAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}