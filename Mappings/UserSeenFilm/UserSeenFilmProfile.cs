using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.UserSeenFilm
{
    public class UserSeenFilmProfile : Profile
    {
        public UserSeenFilmProfile()
        {
            CreateMap<Models.UserSeenFilm, UserSeenFilmViewModel>()
                .ForMember(pDTO => pDTO.Id, opt => opt.MapFrom(e => e.Id))
                .ForMember(pDTO => pDTO.FilmId, opt => opt.MapFrom(e => e.FilmId))
                .ForMember(pDTO => pDTO.UserId, opt => opt.MapFrom(e => e.UserId))
                .ForMember(pDTO => pDTO.FilmName, opt => opt.MapFrom(e => e.Film != null ? e.Film.Name : null))
                .ForMember(pDTO => pDTO.UserName, opt => opt.MapFrom(e => e.User != null ? e.User.Name : null))
                .ForMember(pDTO => pDTO.CreatedAt, opt => opt.MapFrom(e => e.CreatedAt));
        }
    }
}
