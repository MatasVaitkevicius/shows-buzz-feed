using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace shows_buzz_feed.Mappings.Film
{
    public class FilmProfile : Profile
    {
        public FilmProfile()
        {
            CreateMap<Models.Film, FilmViewModel>()
                .ForMember(pDTO => pDTO.Id, opt => opt.MapFrom(e => e.Id))
                .ForMember(pDTO => pDTO.Length, opt => opt.MapFrom(e => e.Length))
                .ForMember(pDTO => pDTO.Budget, opt => opt.MapFrom(e => e.Budget))
                .ForMember(pDTO => pDTO.ReleaseYear, opt => opt.MapFrom(e => e.ReleaseYear))
                .ForMember(pDTO => pDTO.Name, opt => opt.MapFrom(e => e.Name))
                .ForMember(pDTO => pDTO.Genre, opt => opt.MapFrom(e => e.Genre))
                .ForMember(pDTO => pDTO.Director, opt => opt.MapFrom(e => e.Director));
        }
    }
}
