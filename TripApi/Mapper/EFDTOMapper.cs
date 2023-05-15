using AutoMapper;

namespace TripApi.Mapper {

    public static class EFDTOMapper<EF, DTO> {

        public static IMapper GetMapper() {
            var config = new MapperConfiguration(config => config.CreateMap<EF, DTO>());
            var mapper = config.CreateMapper();
            return mapper;
        }

    }

}
