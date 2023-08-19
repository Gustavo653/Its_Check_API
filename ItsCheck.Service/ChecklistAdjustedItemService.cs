using Common.DTO;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace ItsCheck.Service
{
    public class ChecklistAdjustedItemService : IChecklistAdjustedItemService
    {
        private readonly IChecklistAdjustedItemRepository _checklistAdjustedItemRepository;
        private readonly IChecklistRepository _checklistRepository;
        private readonly IItemRepository _itemRepository;

        public ChecklistAdjustedItemService(IChecklistAdjustedItemRepository checklistAdjustedItemRepository,
                                            IChecklistRepository checklistRepository,
                                            IItemRepository itemRepository)
        {
            _checklistAdjustedItemRepository = checklistAdjustedItemRepository;
            _checklistRepository = checklistRepository;
            _itemRepository = itemRepository;
        }

        public async Task<ResponseDTO> Create(ChecklistAdjustedItemDTO checklistAdjustedItemDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklist = await _checklistRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistAdjustedItemDTO.IdChecklist);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {checklistAdjustedItemDTO.IdChecklist} não existe!");
                    return responseDTO;
                }
                var item = await _itemRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistAdjustedItemDTO.IdItem);
                if (item == null)
                {
                    responseDTO.SetBadInput($"O item {checklistAdjustedItemDTO.IdItem} não existe!");
                    return responseDTO;
                }

                var checklistAdjustedItem = new ChecklistAdjustedItem
                {
                    Quantity = checklistAdjustedItemDTO.Quantity,
                    Checklist = checklist,
                    Item = item,
                };
                checklistAdjustedItem.SetCreatedAt();
                await _checklistAdjustedItemRepository.InsertAsync(checklistAdjustedItem);
                await _checklistAdjustedItemRepository.SaveChangesAsync();
                responseDTO.Object = checklistAdjustedItem;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, ChecklistAdjustedItemDTO checklistAdjustedItemDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklistAdjustedItem = await _checklistAdjustedItemRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (checklistAdjustedItem == null)
                {
                    responseDTO.SetBadInput($"O item do checklist ajustado {id} não existe!");
                    return responseDTO;
                }

                var checklist = await _checklistRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistAdjustedItemDTO.IdChecklist);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {checklistAdjustedItemDTO.IdChecklist} não existe!");
                    return responseDTO;
                }
                var item = await _itemRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistAdjustedItemDTO.IdItem);
                if (item == null)
                {
                    responseDTO.SetBadInput($"O item {checklistAdjustedItemDTO.IdItem} não existe!");
                    return responseDTO;
                }

                checklistAdjustedItem.Quantity = checklistAdjustedItemDTO.Quantity;
                checklistAdjustedItem.Checklist = checklist;
                checklistAdjustedItem.Item = item;
                checklistAdjustedItem.SetUpdatedAt();

                await _checklistAdjustedItemRepository.SaveChangesAsync();
                responseDTO.Object = checklistAdjustedItem;
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
                var checklistAdjustedItem = await _checklistAdjustedItemRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (checklistAdjustedItem == null)
                {
                    responseDTO.SetBadInput($"O item do checklist ajustado com id: {id} não existe!");
                    return responseDTO;
                }
                _checklistAdjustedItemRepository.Delete(checklistAdjustedItem);
                await _checklistAdjustedItemRepository.SaveChangesAsync();
                responseDTO.Object = checklistAdjustedItem;
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
                responseDTO.Object = await _checklistAdjustedItemRepository.GetEntities().ToListAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}