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

        public ChecklistService(IChecklistRepository checklistRepository)
        {
            _checklistRepository = checklistRepository;
        }

        public async Task<ResponseDTO> Create(BasicDTO basicDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklistExists = await _checklistRepository.GetEntities().AnyAsync(c => c.Name == basicDTO.Name);
                if (checklistExists)
                {
                    responseDTO.SetBadInput($"O checklist {basicDTO.Name} já existe!");
                    return responseDTO;
                }
                var checklist = new Checklist
                {
                    Name = basicDTO.Name,
                };
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

        public async Task<ResponseDTO> Update(int id, BasicDTO basicDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var checklist = await _checklistRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (checklist == null)
                {
                    responseDTO.SetBadInput($"O checklist {basicDTO.Name} não existe!");
                    return responseDTO;
                }
                checklist.Name = basicDTO.Name;
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
    }
}