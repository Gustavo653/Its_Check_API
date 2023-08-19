using Common.DTO;
using Microsoft.EntityFrameworkCore;
using ItsCheck.Domain;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace ItsCheck.Service
{
    public class ChecklistReviewService : IChecklistReviewService
    {
        private readonly IChecklistReviewRepository _checklistReviewRepository;
        private readonly IChecklistRepository _checklistRepository;
        private readonly IAmbulanceRepository _ambulanceRepository;
        private readonly UserManager<User> _userManager;

        public ChecklistReviewService(IChecklistReviewRepository checklistReviewRepository,
                                    IChecklistRepository checklistRepository,
                                    IAmbulanceRepository ambulanceRepository,
                                    UserManager<User> userManager)
        {
            _checklistReviewRepository = checklistReviewRepository;
            _checklistRepository = checklistRepository;
            _ambulanceRepository = ambulanceRepository;
            _userManager = userManager;
        }

        public async Task<ResponseDTO> Create(ChecklistReviewDTO checklistReviewDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklist = await _checklistRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistReviewDTO.IdChecklist);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {checklistReviewDTO.IdChecklist} não existe!");
                    return responseDTO;
                }
                var ambulance = await _ambulanceRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistReviewDTO.IdAmbulance);
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
                await _checklistReviewRepository.SaveChangesAsync();
                responseDTO.Object = checklistReview;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, ChecklistReviewDTO checklistReviewDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklistReview = await _checklistReviewRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (checklistReview == null)
                {
                    responseDTO.SetBadInput($"A conferência do checklist {id} não existe!");
                    return responseDTO;
                }

                var checklist = await _checklistRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistReviewDTO.IdChecklist);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {checklistReviewDTO.IdChecklist} não existe!");
                    return responseDTO;
                }
                var ambulance = await _ambulanceRepository.GetEntities().FirstOrDefaultAsync(x => x.Id == checklistReviewDTO.IdAmbulance);
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
                var checklistReview = await _checklistReviewRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (checklistReview == null)
                {
                    responseDTO.SetBadInput($"A ambulância com id: {id} não existe!");
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
                responseDTO.Object = await _checklistReviewRepository.GetEntities().ToListAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}