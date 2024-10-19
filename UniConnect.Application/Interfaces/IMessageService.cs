using UniConnect.Application.DTOs;
using UniConnect.Domain.Entities;

namespace UniConnect.Application.Interfaces;

public interface IMessageService {
    Task<IEnumerable<Message>> List();
    Task Create(CreateMessageDto createMessageDto);
    Task Update(UpdateMessageDto updateMessageDto);
    Task Delete(long id);
}