using ItsCheck.Domain;
using ItsCheck.DTO;
using ItsCheck.DTO.Base;
using ItsCheck.Infrastructure.Repository;
using ItsCheck.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ItsCheck.Service
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IChecklistReplacedItemRepository _ChecklistReplacedItemRepository;
        private readonly IChecklistItemRepository _checklistItemRepository;

        public ItemService(IItemRepository itemRepository, IChecklistReplacedItemRepository ChecklistReplacedItemRepository, IChecklistItemRepository checklistItemRepository)
        {
            _itemRepository = itemRepository;
            _ChecklistReplacedItemRepository = ChecklistReplacedItemRepository;
            _checklistItemRepository = checklistItemRepository;
        }

        public async Task<ResponseDTO> ImportCSV(IFormFile csvFile)
        {
            ResponseDTO responseDTO = new();
            List<ImportResultDTO> importResults = new();

            try
            {
                if (!csvFile.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    responseDTO.SetBadInput("O arquivo deve ser um CSV.");
                    return responseDTO;
                }

                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                {
                    var headerLine = await reader.ReadLineAsync();
                    if (headerLine == null || !headerLine.Trim().Equals("Nome", StringComparison.OrdinalIgnoreCase))
                    {
                        responseDTO.SetBadInput("O arquivo CSV deve conter um cabeçalho 'Nome'.");
                        return responseDTO;
                    }

                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        var values = line?.Split(',');

                        if (values?.Length < 1)
                            continue;

                        var itemName = values![0].Trim();

                        var itemExists = await _itemRepository.GetEntities().AnyAsync(c => c.Name == itemName);
                        if (itemExists)
                        {
                            importResults.Add(new ImportResultDTO
                            {
                                Object = itemName,
                                Success = false,
                                ErrorMessage = $"O item {itemName} já existe!"
                            });
                            continue;
                        }

                        var item = new Item
                        {
                            Name = itemName,
                        };
                        item.SetCreatedAt();
                        await _itemRepository.InsertAsync(item);
                        importResults.Add(new ImportResultDTO
                        {
                            Object = item,
                            Success = true,
                            ErrorMessage = null
                        });
                    }

                    await _itemRepository.SaveChangesAsync();
                    responseDTO.Message = "Importação concluída com sucesso!";
                    responseDTO.Object = importResults;
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
                var ChecklistReplacedItemExists = await _ChecklistReplacedItemRepository.GetEntities().AnyAsync(c => c.ChecklistItem.Item.Id == id);
                if (ChecklistReplacedItemExists)
                {
                    responseDTO.SetBadInput("Não é possível apagar o item, já existe uma reposição vinculada!");
                    return responseDTO;
                }

                var checkListItemExists = await _checklistItemRepository.GetEntities().AnyAsync(c => c.Item.Id == id);
                if (ChecklistReplacedItemExists)
                {
                    responseDTO.SetBadInput("Não é possível apagar o item, já existe um item de checklist vinculado!");
                    return responseDTO;
                }

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