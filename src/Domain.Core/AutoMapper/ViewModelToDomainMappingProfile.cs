using AutoMapper;
using Domain.Core.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {

            CreateMap<long, CEP>()
                .ConstructUsing(c => new CEP(c));
            CreateMap<string, CEP>()
                .ConstructUsing(c => new CEP(c));

            CreateMap<long, CNPJ>()
                .ConstructUsing(c => new CNPJ(c));
            CreateMap<string, CNPJ>()
                .ConstructUsing(c => new CNPJ(c));

            CreateMap<long, CPF>()
                .ConstructUsing(c => new CPF(c, null));
            CreateMap<string, CPF>()
                .ConstructUsing(c => new CPF(c, null));

            CreateMap<string, Name>()
                .ConstructUsing(c => new Name(c, null));

            CreateMap<long, PIS>()
                .ConstructUsing(c => new PIS(c));
            CreateMap<string, PIS>()
                .ConstructUsing(c => new PIS(c));

        }
    }
}
