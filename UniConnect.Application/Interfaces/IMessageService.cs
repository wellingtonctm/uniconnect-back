using UniConnect.Application.DTOs;
using UniConnect.Domain.Entities;

namespace UniConnect.Application.Interfaces;

public interface IMessageService
{
    Task<IEnumerable<MessageItemDto>> List();
    Task Create(CreateMessageDto createMessageDto);
    Task Update(UpdateMessageDto updateMessageDto);
    Task Enable(long id);
    Task Disable(long id);
    Task Delete(long id);
}