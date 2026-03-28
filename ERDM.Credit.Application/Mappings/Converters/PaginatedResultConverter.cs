using AutoMapper;
using ERDM.Core;
using ERDM.Credit.Contracts.Wrapper;

namespace ERDM.Credit.Application.Mappings.Converters
{
    public class PaginatedResultConverter<TSource, TDestination> : ITypeConverter<PaginatedResult<TSource>, PaginatedResponse<TDestination>>
    {
        private readonly IMapper _mapper;

        public PaginatedResultConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public PaginatedResponse<TDestination> Convert(PaginatedResult<TSource> source, PaginatedResponse<TDestination> destination, ResolutionContext context)
        {
            if (source == null)
                return null;

            return new PaginatedResponse<TDestination>
            {
                PageNumber = source.PageNumber,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                TotalPages = source.TotalPages,
                HasPrevious = source.HasPrevious,
                HasNext = source.HasNext,
                Data = _mapper.Map<IEnumerable<TDestination>>(source.Data)
            };
        }
    }
}
