using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace shows_buzz_feed.Mappings.Quiz
{
    public class QuizProfile : Profile
    {
        public QuizProfile()
        {
            CreateMap<Models.Quiz, QuizViewModel>()
            .ForMember(pDTO => pDTO.Id, opt => opt.MapFrom(e => e.Id))
            .ForMember(pDTO => pDTO.Name, opt => opt.MapFrom(e => e.Name))
            .ForMember(pDTO => pDTO.TimesCompleted, opt => opt.MapFrom(e => e.TimesCompleted))
            .ForMember(pDTO => pDTO.UserId, opt => opt.MapFrom(e => e.UserId));
        }
    }
}
