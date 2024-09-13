using AutoMapper;
using Xanes.Models.Dtos.eSiafN4;

namespace Xanes.Web;

public class MappingConfig : Profile
{
    public MappingConfig()
    {

        CreateMap<TransaccionesBcoDto, TransaccionesBcoDtoCreate>().ReverseMap();
        CreateMap<TransaccionesBcoDto, TransaccionesBcoDtoUpdate>().ReverseMap();

    }
}
