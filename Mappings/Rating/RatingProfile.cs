using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.Rating
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<Models.Rating, RatingViewModel>()
                .ForMember(pDTO => pDTO.Id, opt => opt.MapFrom(e => e.Id))
                .ForMember(pDTO => pDTO.DateRated, opt => opt.MapFrom(e => e.DateRated))
                .ForMember(pDTO => pDTO.Comment, opt => opt.MapFrom(e => e.Comment))
                .ForMember(pDTO => pDTO.Rate, opt => opt.MapFrom(e => e.Rate));
        }
    }
}
