using Domain.Core.Bus;
using Domain.Core.Commands;
using Domain.Core.Events;
using System;

namespace Infra.CrossCutting.Bus
{
    public sealed class InMemoryBus : IBus
    {

        #region Variables

        private static IServiceProvider _container => ContainerAccessor();

        #endregion

        #region Properties

        public static Func<IServiceProvider> ContainerAccessor { get; set; }

        #endregion

        #region Methods

        private static void Publish<T>(T message) where T : Message
        {
            if (_container == null) return;

            //var obj = _container.GetService(message.MessageType.Equals("DomainNotification")
            //    ? typeof(IDomainNotificationHandler<T>)
            //    : typeof(IHandler<T>));

            //((IHandler<T>)obj).Handle(message);

        }

        public void SendCommand<T>(T theCommand) where T : Command
        {
            Publish(theCommand);
        }

        public void RaiseEvent<T>(T theEvent) where T : Event
        {
            Publish(theEvent);
        }

        #endregion

    }
}
