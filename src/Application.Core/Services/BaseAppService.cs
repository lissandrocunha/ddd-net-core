using AutoMapper;
using Domain.Core.Bus;
using Domain.Core.Interfaces;
using Domain.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Core.Services
{
    public abstract class BaseAppService
    {

        #region Variables

        private readonly IMediatorHandler _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        #endregion

        #region Properties

        public IMapper Mapper => _mapper;
        public IMediatorHandler Bus => _bus;
        public IUnitOfWork UoW => _uow;

        #endregion

        #region Constructors

        protected BaseAppService(IMediatorHandler bus,
                                 IMapper mapper,
                                 IUnitOfWork uow)
        {
            _bus = bus;
            _mapper = mapper;
            _uow = uow;
        }

        #endregion

        #region Methods

        protected void NotificarErro(string evento, string mensagem)
        {
            _bus.PublicarEvento(new DomainNotification(evento, mensagem));
        }

        #endregion

    }
}
