using ItsCheck.Domain;
using ItsCheck.DTO;
using ItsCheck.DTO.Base;
using ItsCheck.Infrastructure.Repository;
using ItsCheck.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;

namespace ItsCheck.Service
{
    public class AmbulanceService : IAmbulanceService
    {
        private readonly IAmbulanceRepository _ambulanceRepository;
        private readonly IAmbulanceChecklistXRefRepository _ambulanceChecklistXRefRepository;
        private readonly IChecklistRepository _checklistRepository;

        public AmbulanceService(IAmbulanceRepository ambulanceRepository,
                                IAmbulanceChecklistXRefRepository ambulanceChecklistXRefRepository,
                                IChecklistRepository checklistRepository)
        {
            _ambulanceRepository = ambulanceRepository;
            _ambulanceChecklistXRefRepository = ambulanceChecklistXRefRepository;
            _checklistRepository = checklistRepository;
        }

        public async Task<ResponseDTO> Create(AmbulanceDTO ambulanceDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var ambulanceExists = await _ambulanceRepository.GetEntities().AnyAsync(c => c.Number == ambulanceDTO.Number);
                if (ambulanceExists)
                {
                    responseDTO.SetBadInput($"A ambulância {ambulanceDTO.Number} já existe!");
                    return responseDTO;
                }

                var ambulance = new Ambulance
                {
                    Number = ambulanceDTO.Number,
                    LicensePlate = ambulanceDTO.LicensePlate,
                };
                ambulance.SetCreatedAt();
                await _ambulanceRepository.InsertAsync(ambulance);

                foreach (var item in ambulanceDTO.IdChecklists)
                {
                    var checklist = await _checklistRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == item);
                    if (checklist == null)
                    {
                        responseDTO.SetBadInput($"O checklist {item} não existe!");
                        return responseDTO;
                    }
                    var ambulanceChecklistXRef = new AmbulanceChecklistXRef() { Ambulance = ambulance, Checklist = checklist };
                    ambulanceChecklistXRef.SetCreatedAt();
                    await _ambulanceChecklistXRefRepository.InsertAsync(ambulanceChecklistXRef);
                }

                await _ambulanceRepository.SaveChangesAsync();
                responseDTO.Object = ambulance;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, AmbulanceDTO ambulanceDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var ambulance = await _ambulanceRepository.GetTrackedEntities().Include(x => x.AmbulanceChecklistXRefs).FirstOrDefaultAsync(c => c.Id == id);
                if (ambulance == null)
                {
                    responseDTO.SetBadInput($"A ambulância {ambulanceDTO.Number} não existe!");
                    return responseDTO;
                }

                ambulance.AmbulanceChecklistXRefs?.Clear();

                ambulance.Number = ambulanceDTO.Number;
                ambulance.SetUpdatedAt();

                foreach (var item in ambulanceDTO.IdChecklists)
                {
                    var checklist = await _checklistRepository.GetTrackedEntities().FirstOrDefaultAsync(x => x.Id == item);
                    if (checklist == null)
                    {
                        responseDTO.SetBadInput($"O checklist {item} não existe!");
                        return responseDTO;
                    }
                    var ambulanceChecklistXRef = new AmbulanceChecklistXRef() { Ambulance = ambulance, Checklist = checklist };
                    ambulanceChecklistXRef.SetCreatedAt();
                    await _ambulanceChecklistXRefRepository.InsertAsync(ambulanceChecklistXRef);
                }

                await _ambulanceRepository.SaveChangesAsync();
                responseDTO.Object = ambulance;
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
                var ambulance = await _ambulanceRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (ambulance == null)
                {
                    responseDTO.SetBadInput($"A ambulância com id: {id} não existe!");
                    return responseDTO;
                }
                _ambulanceRepository.Delete(ambulance);
                await _ambulanceRepository.SaveChangesAsync();
                responseDTO.Object = ambulance;
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
                responseDTO.Object = await _ambulanceRepository.GetEntities()
                                                               .Include(x => x.AmbulanceChecklistXRefs)
                                                               .ThenInclude(x => x.Checklist)
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