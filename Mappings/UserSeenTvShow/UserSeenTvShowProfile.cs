using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.UserSeenTvShow
{
    public class UserSeenTvShowProfile : Profile
    {
        public UserSeenTvShowProfile()
        {
            CreateMap<Models.UserSeenTvShow, UserSeenTvShowViewModel>()
                .ForMember(pDTO => pDTO.Id, opt => opt.MapFrom(e => e.Id))
                .ForMember(pDTO => pDTO.TvShowId, opt => opt.MapFrom(e => e.TvShowId))
                .ForMember(pDTO => pDTO.UserId, opt => opt.MapFrom(e => e.UserId))
                .ForMember(pDTO => pDTO.TvShowName, opt => opt.MapFrom(e => e.TvShow != null ? e.TvShow.Name : null))
                .ForMember(pDTO => pDTO.UserName, opt => opt.MapFrom(e => e.User != null ? e.User.Name : null))
                .ForMember(pDTO => pDTO.CreatedAt, opt => opt.MapFrom(e => e.CreatedAt));
        }
    }
}
