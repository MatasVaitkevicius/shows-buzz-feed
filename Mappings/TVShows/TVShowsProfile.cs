using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace shows_buzz_feed.Mappings.TVShows
{
    public class TVShowsProfile : Profile
    {
        public TVShowsProfile()
        {
            CreateMap<Models.TVShows, TVShowsViewModel>()
                .ForMember(pDTO => pDTO.Id, opt => opt.MapFrom(e => e.Id))
                .ForMember(pDTO => pDTO.Name, opt => opt.MapFrom(e => e.Name))
                .ForMember(pDTO => pDTO.Genre, opt => opt.MapFrom(e => e.Genre))
                .ForMember(pDTO => pDTO.Director, opt => opt.MapFrom(e => e.Director))
                .ForMember(pDTO => pDTO.ReleaseYear, opt => opt.MapFrom(e => e.ReleaseYear));

        }
    }
}
