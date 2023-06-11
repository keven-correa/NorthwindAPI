using AutoMapper;
using NorthwindAPI.Entities;

namespace NorthwindAPI.Dtos.Mapper
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
                CreateMap<Product, CreateProductDto>().ReverseMap();
        }
    }
}
