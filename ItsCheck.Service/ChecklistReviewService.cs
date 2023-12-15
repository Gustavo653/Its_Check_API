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
    public class ChecklistReviewService : IChecklistReviewService
    {
        private readonly IChecklistReviewRepository _checklistReviewRepository;
        private readonly IChecklistRepository _checklistRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IChecklistItemRepository _checklistItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAmbulanceRepository _ambulanceRepository;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public ChecklistReviewService(IChecklistReviewRepository checklistReviewRepository,
                                      IChecklistRepository checklistRepository,
                                      ICategoryRepository categoryRepository,
                                      IChecklistItemRepository checklistItemRepository,
                                      IUserRepository userRepository,
                                      IHttpContextAccessor httpContextAccessor,
                                      IAmbulanceRepository ambulanceRepository)
        {
            _checklistReviewRepository = checklistReviewRepository;
            _checklistRepository = checklistRepository;
            _categoryRepository = categoryRepository;
            _checklistItemRepository = checklistItemRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _ambulanceRepository = ambulanceRepository;
        }

        public async Task<ResponseDTO> Create(ChecklistReviewDTO checklistReviewDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                checklistReviewDTO.IdUser = Convert.ToInt32(_session.GetString(Consts.ClaimUserId));

                var checklist = await _checklistRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == checklistReviewDTO.IdChecklist);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {checklistReviewDTO.IdChecklist} não existe!");
                    return responseDTO;
                }

                var user = await _userRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == checklistReviewDTO.IdUser);
                if (user == null)
                {
                    responseDTO.SetBadInput($"O usuário {checklistReviewDTO.IdUser} não existe!");
                    return responseDTO;
                }

                var ambulance = await _ambulanceRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == checklistReviewDTO.IdAmbulance);
                if (ambulance == null)
                {
                    responseDTO.SetBadInput($"A ambulância {checklistReviewDTO.IdAmbulance} não existe!");
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

                var teste = _checklistReviewRepository.GetChanges();

                await _checklistReviewRepository.SaveChangesAsync();
                responseDTO.Object = checklistReviewDTO;
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
                                                        .FirstOrDefaultAsync(x => x.Id == categoryReviewDTO.Id) ??
                                                        throw new Exception($"A categoria {categoryReviewDTO.Id} não existe");
                foreach (var itemReviewDTO in categoryReviewDTO.Items)
                {
                    var checklistItem = await _checklistItemRepository.GetTrackedEntities()
                                                             .Include(x => x.ChecklistReplacedItems)
                                                             .Include(x => x.Item)
                                                             .FirstOrDefaultAsync(x => x.Item.Id == itemReviewDTO.Id && x.Category.Id == category.Id) ??
                                                             throw new Exception($"O item {itemReviewDTO.Id} não existe");

                    var checklistReplacedItem = new ChecklistReplacedItem()
                    {
                        ChecklistItem = checklistItem,
                        ChecklistReview = checklistReview,
                        AmountReplaced = itemReviewDTO.AmountReplaced
                    };
                    checklistReplacedItem.SetCreatedAt();
                    checklistItem.ChecklistReplacedItems?.Add(checklistReplacedItem);
                }
            }
        }

        public async Task<ResponseDTO> Update(int id, ChecklistReviewDTO checklistReviewDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                checklistReviewDTO.IdUser = Convert.ToInt32(_session.GetString(Consts.ClaimUserId));
                var checklistReview = await _checklistReviewRepository.GetTrackedEntities()
                                                                      .Include(x => x.ChecklistReplacedItems)
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

                checklistReview.Observation = checklistReviewDTO.Observation;
                checklistReview.Checklist = checklist;

                checklistReview.ChecklistReplacedItems?.RemoveAll(_ => true);

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

        public Task<ResponseDTO> GetList()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO> GetList(int? takeLast)
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await _checklistReviewRepository.GetEntities()
                                                                     .Select(x => new
                                                                     {
                                                                         x.Id,
                                                                         x.Type,
                                                                         x.CreatedAt,
                                                                         x.UpdatedAt,
                                                                         x.Ambulance,
                                                                         x.Observation,
                                                                         User = x.User.Name,
                                                                         x.Checklist,
                                                                         ChecklistReviews = x.ChecklistReplacedItems != null &&
                                                                                            x.ChecklistReplacedItems.Count != 0 ?
                                                                                            x.ChecklistReplacedItems.Select(x => new
                                                                                            {
                                                                                                category = x.ChecklistItem.Category.Name,
                                                                                                item = x.ChecklistItem.Item.Name,
                                                                                                amountReplaced = x.AmountReplaced,
                                                                                                amoutRequired = x.ChecklistItem.AmountRequired
                                                                                            }) : null
                                                                     })
                                                                     .OrderByDescending(x => x.Id)
                                                                     .Take(takeLast ?? 1000)
                                                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }

            return responseDTO;
        }

        public Task<ResponseDTO> ExistsChecklistReview()
        {
            //ResponseDTO responseDTO = new();
            //try
            //{
            //    var user = await _userRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(_session.GetString(Consts.ClaimUserId)));
            //    var checklistReviewInitialDuplicated = await _checklistReviewRepository.GetEntities()
            //                                 .AnyAsync(x => x.CreatedAt > DateTime.Now.AddHours(-12) &&
            //                                                x.User.Id == user!.Id &&
            //                                                x.Type == Domain.Enum.ReviewType.Initial);

            //    responseDTO.Object = checklistReviewInitialDuplicated;
            //}
            //catch (Exception ex)
            //{
            //    responseDTO.SetError(ex);
            //}

            //return responseDTO;
            throw new Exception("Método desativado devido ao MVP");
        }
    }
}