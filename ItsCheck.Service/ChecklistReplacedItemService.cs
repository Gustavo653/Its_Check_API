using ItsCheck.Domain;
using ItsCheck.DTO;
using ItsCheck.DTO.Base;
using ItsCheck.Infrastructure.Repository;
using ItsCheck.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;

namespace ItsCheck.Service
{
    public class ChecklistReplacedItemService : IChecklistReplacedItemService
    {
        private readonly IChecklistReplacedItemRepository _ChecklistReplacedItemRepository;
        private readonly IChecklistReviewRepository _checklistReviewRepository;
        private readonly IItemRepository _itemRepository;
        private IChecklistItemRepository _checklistItemRepository;

        public ChecklistReplacedItemService(IChecklistReplacedItemRepository ChecklistReplacedItemRepository,
                                            IChecklistReviewRepository checklistReviewRepository,
                                            IItemRepository itemRepository,
                                            IChecklistItemRepository checklistItemRepository)
        {
            _ChecklistReplacedItemRepository = ChecklistReplacedItemRepository;
            _checklistReviewRepository = checklistReviewRepository;
            _itemRepository = itemRepository;
            _checklistItemRepository = checklistItemRepository;
        }

        public async Task<ResponseDTO> Create(ChecklistReplacedItemDTO ChecklistReplacedItemDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklistReview = await _checklistReviewRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == ChecklistReplacedItemDTO.IdChecklistReview);
                if (checklistReview == null)
                {
                    responseDTO.SetBadInput($"O checklist ajustado {ChecklistReplacedItemDTO.IdChecklistReview} não existe!");
                    return responseDTO;
                }
                var checklistItem = await _checklistItemRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == ChecklistReplacedItemDTO.IdChecklistItem);
                if (checklistItem == null)
                {
                    responseDTO.SetBadInput($"O item {ChecklistReplacedItemDTO.IdChecklistItem} não existe!");
                    return responseDTO;
                }

                var ChecklistReplacedItem = new ChecklistReplacedItem
                {
                    ChecklistItem = checklistItem,
                    ChecklistReview = checklistReview,
                    AmountReplaced = ChecklistReplacedItemDTO.AmountReplaced,
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

                var checklistReview = await _checklistReviewRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == ChecklistReplacedItemDTO.IdChecklistReview);
                if (checklistReview == null)
                {
                    responseDTO.SetBadInput($"O checklist {ChecklistReplacedItemDTO.IdChecklistReview} não existe!");
                    return responseDTO;
                }
                var checklistItem = await _checklistItemRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == ChecklistReplacedItemDTO.IdChecklistItem);
                if (checklistItem == null)
                {
                    responseDTO.SetBadInput($"O item {ChecklistReplacedItemDTO.IdChecklistItem} não existe!");
                    return responseDTO;
                }

                ChecklistReplacedItem.AmountReplaced = ChecklistReplacedItemDTO.AmountReplaced;
                ChecklistReplacedItem.ChecklistReview = checklistReview;
                ChecklistReplacedItem.ChecklistItem = checklistItem;
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