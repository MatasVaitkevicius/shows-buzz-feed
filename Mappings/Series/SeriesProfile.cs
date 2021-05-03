using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace shows_buzz_feed.Mappings.Series
{
    public class SeriesProfile : Profile
    {
        public SeriesProfile()
        {
            CreateMap<Models.Series, SeriesViewModel>()
                .ForMember(pDTO => pDTO.Id, opt => opt.MapFrom(e => e.Id))
                .ForMember(pDTO => pDTO.Name, opt => opt.MapFrom(e => e.Name))
                .ForMember(pDTO => pDTO.Genre, opt => opt.MapFrom(e => e.Genre));
        }
    }
}
