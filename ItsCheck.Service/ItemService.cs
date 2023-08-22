using Common.DTO;
using ItsCheck.DataAccess.Interface;
using ItsCheck.Domain;
using ItsCheck.DTO;
using ItsCheck.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ItsCheck.Service
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<ResponseDTO> ImportCSV(IFormFile csvFile)
        {
            ResponseDTO responseDTO = new();
            try
            {
                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        var values = line.Split(',');

                        if (values.Length < 1)
                            continue;

                        var itemName = values[0];

                        var itemExists = await _itemRepository.GetEntities().AnyAsync(c => c.Name == itemName);
                        if (itemExists)
                        {
                            responseDTO.SetBadInput($"O item {itemName} já existe!");
                            return responseDTO;
                        }

                        var item = new Item
                        {
                            Name = itemName,
                        };
                        item.SetCreatedAt();
                        await _itemRepository.InsertAsync(item);
                    }

                    await _itemRepository.SaveChangesAsync();
                    responseDTO.Message = "Importação concluída com sucesso!";
                }
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
        
        public async Task<ResponseDTO> Create(BasicDTO basicDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var itemExists = await _itemRepository.GetEntities().AnyAsync(c => c.Name == basicDTO.Name);
                if (itemExists)
                {
                    responseDTO.SetBadInput($"O item {basicDTO.Name} já existe!");
                    return responseDTO;
                }
                var item = new Item
                {
                    Name = basicDTO.Name,
                };
                item.SetCreatedAt();
                await _itemRepository.InsertAsync(item);
                await _itemRepository.SaveChangesAsync();
                responseDTO.Object = item;
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
                var item = await _itemRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (item == null)
                {
                    responseDTO.SetBadInput($"O item {basicDTO.Name} não existe!");
                    return responseDTO;
                }
                item.Name = basicDTO.Name;
                item.SetUpdatedAt();
                await _itemRepository.SaveChangesAsync();
                responseDTO.Object = item;
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
                var item = await _itemRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (item == null)
                {
                    responseDTO.SetBadInput($"O item com id: {id} não existe!");
                    return responseDTO;
                }
                _itemRepository.Delete(item);
                await _itemRepository.SaveChangesAsync();
                responseDTO.Object = item;
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
                responseDTO.Object = await _itemRepository.GetEntities().ToListAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}