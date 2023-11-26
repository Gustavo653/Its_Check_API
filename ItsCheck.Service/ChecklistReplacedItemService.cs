using Common.DTO;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace ItsCheck.Service
{
    public class ChecklistReplacedItemService : IChecklistReplacedItemService
    {
        private readonly IChecklistReplacedItemRepository _ChecklistReplacedItemRepository;
        private readonly IChecklistRepository _checklistRepository;
        private readonly IItemRepository _itemRepository;

        public ChecklistReplacedItemService(IChecklistReplacedItemRepository ChecklistReplacedItemRepository,
                                            IChecklistRepository checklistRepository,
                                            IItemRepository itemRepository)
        {
            _ChecklistReplacedItemRepository = ChecklistReplacedItemRepository;
            _checklistRepository = checklistRepository;
            _itemRepository = itemRepository;
        }

        public async Task<ResponseDTO> Create(ChecklistReplacedItemDTO ChecklistReplacedItemDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklist = await _checklistRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == ChecklistReplacedItemDTO.IdChecklist);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {ChecklistReplacedItemDTO.IdChecklist} não existe!");
                    return responseDTO;
                }
                var item = await _itemRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == ChecklistReplacedItemDTO.IdItem);
                if (item == null)
                {
                    responseDTO.SetBadInput($"O item {ChecklistReplacedItemDTO.IdItem} não existe!");
                    return responseDTO;
                }

                var ChecklistReplacedItem = new ChecklistReplacedItem
                {
                    AmountReplaced = ChecklistReplacedItemDTO.AmountReplaced,
                    Checklist = checklist,
                    Item = item,
                };
                ChecklistReplacedItem.SetCreatedAt();
                await _ChecklistReplacedItemRepository.InsertAsync(ChecklistReplacedItem);
                await _ChecklistReplacedItemRepository.SaveChangesAsync();
                responseDTO.Object = ChecklistReplacedItem;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, ChecklistReplacedItemDTO ChecklistReplacedItemDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var ChecklistReplacedItem = await _ChecklistReplacedItemRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (ChecklistReplacedItem == null)
                {
                    responseDTO.SetBadInput($"O item do checklist ajustado {id} não existe!");
                    return responseDTO;
                }

                var checklist = await _checklistRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == ChecklistReplacedItemDTO.IdChecklist);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {ChecklistReplacedItemDTO.IdChecklist} não existe!");
                    return responseDTO;
                }
                var item = await _itemRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == ChecklistReplacedItemDTO.IdItem);
                if (item == null)
                {
                    responseDTO.SetBadInput($"O item {ChecklistReplacedItemDTO.IdItem} não existe!");
                    return responseDTO;
                }

                ChecklistReplacedItem.AmountReplaced = ChecklistReplacedItemDTO.AmountReplaced;
                ChecklistReplacedItem.Checklist = checklist;
                ChecklistReplacedItem.Item = item;
                ChecklistReplacedItem.SetUpdatedAt();

                await _ChecklistReplacedItemRepository.SaveChangesAsync();
                responseDTO.Object = ChecklistReplacedItem;
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
                var ChecklistReplacedItem = await _ChecklistReplacedItemRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (ChecklistReplacedItem == null)
                {
                    responseDTO.SetBadInput($"O item do checklist ajustado com id: {id} não existe!");
                    return responseDTO;
                }
                _ChecklistReplacedItemRepository.Delete(ChecklistReplacedItem);
                await _ChecklistReplacedItemRepository.SaveChangesAsync();
                responseDTO.Object = ChecklistReplacedItem;
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
                responseDTO.Object = await _ChecklistReplacedItemRepository.GetEntities().ToListAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}