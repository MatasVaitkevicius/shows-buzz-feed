using AutoMapper;
using shows_buzz_feed.Mappings.User;
using shows_buzz_feed.Mappings.UserSeenFilm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Models.User, UserViewModel>()
                .ForMember(pDTO => pDTO.Id, opt => opt.MapFrom(e => e.Id))
                .ForMember(pDTO => pDTO.Name, opt => opt.MapFrom(e => e.Name))
                .ForMember(pDTO => pDTO.UserSeenFilms, opt => opt.MapFrom(e => e.UserSeenFilms.Any() ?
                    e.UserSeenFilms.Select(u => new UserSeenFilmViewModel
                    {
                        FilmName = u.Film.Name,
                        FilmId = u.Film.Id,
                        Id = u.Id,
                        UserId = u.UserId,
                        UserName = u.User.Name,
                        CreatedAt = u.CreatedAt
                        }) : null));
        }
    }
}
