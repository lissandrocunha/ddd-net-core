
using AutoMapper;
using Domain.Core.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {

            CreateMap<CEP, long>()
                .ConstructUsing(c => c.Numero);
            CreateMap<CEP, string>()
                .ConstructUsing(c => c.Formatado);

            CreateMap<CNPJ, long>()
                .ConstructUsing(c => c.Numero);
            CreateMap<CNPJ, string>()
                .ConstructUsing(c => c.Formatado);

            CreateMap<CPF, long>()
                .ConstructUsing(c => c.Numero);
            CreateMap<CPF, string>()
                .ConstructUsing(c => c.Formatado);

            CreateMap<PIS, long>()
                .ConstructUsing(c => c.Numero);
            CreateMap<PIS, string>()
                .ConstructUsing(c => c.Formatado);

        }
    }
}
