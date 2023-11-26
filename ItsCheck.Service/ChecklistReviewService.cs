using Common.DTO;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain;
using ItsCheck.Domain.Identity;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ItsCheck.Service
{
    public class ChecklistReviewService : IChecklistReviewService
    {
        private readonly IChecklistReviewRepository _checklistReviewRepository;
        private readonly IChecklistRepository _checklistRepository;
        private readonly IAmbulanceRepository _ambulanceRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IChecklistItemRepository _checklistItemRepository;
        private readonly UserManager<User> _userManager;

        public ChecklistReviewService(IChecklistReviewRepository checklistReviewRepository,
                                      IChecklistRepository checklistRepository,
                                      IAmbulanceRepository ambulanceRepository,
                                      ICategoryRepository categoryRepository,
                                      IItemRepository itemRepository,
                                      IChecklistItemRepository checklistItemRepository,
                                      UserManager<User> userManager)
        {
            _checklistReviewRepository = checklistReviewRepository;
            _checklistRepository = checklistRepository;
            _ambulanceRepository = ambulanceRepository;
            _categoryRepository = categoryRepository;
            _itemRepository = itemRepository;
            _checklistItemRepository = checklistItemRepository;
            _userManager = userManager;
        }

        public async Task<ResponseDTO> Create(ChecklistReviewDTO checklistReviewDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklist = await _checklistRepository.GetTrackedEntities()
                                                          .FirstOrDefaultAsync(x => x.Id == checklistReviewDTO.IdChecklist);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {checklistReviewDTO.IdChecklist} não existe!");
                    return responseDTO;
                }

                var ambulance = await _ambulanceRepository.GetTrackedEntities()
                                                          .FirstOrDefaultAsync(x => x.Id == checklistReviewDTO.IdAmbulance);
                if (ambulance == null)
                {
                    responseDTO.SetBadInput($"A ambulância {checklistReviewDTO.IdAmbulance} não existe!");
                    return responseDTO;
                }

                var user = await _userManager.FindByIdAsync(checklistReviewDTO.IdUser.ToString());
                if (user == null)
                {
                    responseDTO.SetBadInput($"O usuário {checklistReviewDTO.IdUser} não existe!");
                    return responseDTO;
                }

                var checklistReview = new ChecklistReview
                {
                    Type = checklistReviewDTO.Type,
                    Observation = checklistReviewDTO.Observation,
                    Checklist = checklist,
                    Ambulance = ambulance,
                    User = user,
                };
                checklistReview.SetCreatedAt();

                await _checklistReviewRepository.InsertAsync(checklistReview);

                await ProcessChecklistReviewItems(checklistReviewDTO, checklistReview);

                await _checklistReviewRepository.SaveChangesAsync();
                responseDTO.Object = checklistReview;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }

        private async Task ProcessChecklistReviewItems(ChecklistReviewDTO checklistReviewDTO, ChecklistReview checklistReview)
        {
            foreach (var categoryReviewDTO in checklistReviewDTO.Categories)
            {
                var category = await _categoryRepository.GetTrackedEntities()
                                                        .FirstOrDefaultAsync(x => x.Id == categoryReviewDTO.Id);
                if (category == null) continue;
                foreach (var itemReviewDTO in categoryReviewDTO.Items)
                {
                    var item = await _checklistItemRepository.GetTrackedEntities()
                                                             .Include(x => x.ChecklistReplacedItems)
                                                             .Include(x => x.Item)
                                                             .FirstOrDefaultAsync(x => x.Id == itemReviewDTO.Id);
                    if (item == null) continue;

                    var checklistReplacedItem = new ChecklistReplacedItem()
                    {
                        ChecklistItem = item,
                        ChecklistReview = checklistReview,
                        AmountReplaced = itemReviewDTO.AmountReplaced
                    };
                    checklistReplacedItem.SetCreatedAt();
                    item.ChecklistReplacedItems?.Add(checklistReplacedItem);
                }
            }
        }

        public async Task<ResponseDTO> Update(int id, ChecklistReviewDTO checklistReviewDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklistReview = await _checklistReviewRepository.GetTrackedEntities()
                                                                      .FirstOrDefaultAsync(c => c.Id == id);
                if (checklistReview == null)
                {
                    responseDTO.SetBadInput($"A conferência do checklist {id} não existe!");
                    return responseDTO;
                }

                var checklist = await _checklistRepository.GetTrackedEntities()
                                                          .FirstOrDefaultAsync(x => x.Id == checklistReviewDTO.IdChecklist);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {checklistReviewDTO.IdChecklist} não existe!");
                    return responseDTO;
                }

                var ambulance = await _ambulanceRepository.GetTrackedEntities()
                                                          .FirstOrDefaultAsync(x => x.Id == checklistReviewDTO.IdAmbulance);
                if (ambulance == null)
                {
                    responseDTO.SetBadInput($"A ambulância {checklistReviewDTO.IdAmbulance} não existe!");
                    return responseDTO;
                }

                var user = await _userManager.FindByIdAsync(checklistReviewDTO.IdUser.ToString());
                if (user == null)
                {
                    responseDTO.SetBadInput($"O usuário {checklistReviewDTO.IdUser} não existe!");
                    return responseDTO;
                }

                checklistReview.Type = checklistReviewDTO.Type;
                checklistReview.Observation = checklistReviewDTO.Observation;
                checklistReview.Checklist = checklist;
                checklistReview.Ambulance = ambulance;
                checklistReview.User = user;

                checklistReview.Checklist.ChecklistItems.RemoveAll(_ => true);

                await ProcessChecklistReviewItems(checklistReviewDTO, checklistReview);

                checklistReview.SetUpdatedAt();
                await _checklistReviewRepository.SaveChangesAsync();
                responseDTO.Object = checklistReview;
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
                var checklistReview = await _checklistReviewRepository.GetTrackedEntities()
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (checklistReview == null)
                {
                    responseDTO.SetBadInput($"O checklist com id: {id} não existe!");
                    return responseDTO;
                }

                _checklistReviewRepository.Delete(checklistReview);
                await _checklistReviewRepository.SaveChangesAsync();
                responseDTO.Object = checklistReview;
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
                responseDTO.Object =
                    await _checklistReviewRepository.GetEntities()
                        .Include(x => x.Ambulance).ThenInclude(x => x.Checklist).ThenInclude(x => x.ChecklistItems).ThenInclude(x => x.Item)
                        .Include(x => x.User)
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }
    }
}