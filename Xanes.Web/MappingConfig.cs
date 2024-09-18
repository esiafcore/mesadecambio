using AutoMapper;
using Xanes.Models;
using Xanes.Models.Dtos.eSiafN4;
using Xanes.Models.Dtos.XanesN8;

namespace Xanes.Web;

public class MappingConfig : Profile
{
    public MappingConfig()
    {

        CreateMap<TransaccionesBcoDto, TransaccionesBcoDtoCreate>().ReverseMap();
        CreateMap<TransaccionesBcoDto, TransaccionesBcoDtoUpdate>().ReverseMap();

        CreateMap<QuotationDetail, QuotationDetailDto>()
            .ForMember(dest => dest.BankSourceCode,
                opt => opt
                    .MapFrom(src
                        => src.BankSourceTrx.Code))
            .ForMember(dest => dest.BankTargetCode,
                opt => opt
                    .MapFrom(src
                        => src.BankTargetTrx.Code))
            .ReverseMap();

    }
}
