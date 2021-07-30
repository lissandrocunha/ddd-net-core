using Domain.Core.Events;

namespace Domain.Core.Interfaces
{
    public interface IEventStore
    {
        void SaveEvent<T>(T theEvent) where T : Event;
    }
}
