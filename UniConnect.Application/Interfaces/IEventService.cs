namespace UniConnect.Application.Interfaces;

public interface IEventService {
    Task Create(string description);
    Task Delete(long id);
}