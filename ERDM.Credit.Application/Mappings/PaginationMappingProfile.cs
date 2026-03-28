using AutoMapper;
using ERDM.Core;
using ERDM.Credit.Application.Mappings.Converters;
using ERDM.Credit.Contracts.Wrapper;


namespace ERDM.Credit.Application.Mappings
{
    public class PaginationMappingProfile : Profile
    {
        public PaginationMappingProfile()
        {
            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResponse<>))
                .ConvertUsing(typeof(PaginatedResultConverter<,>));
        }
    }
}
