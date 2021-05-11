using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace shows_buzz_feed.Mappings.Answer
{
    public class AnswerProfile : Profile
    {
        public AnswerProfile()
        {
            CreateMap<Models.Answer, AnswerViewModel>()
                .ForMember(pDTO => pDTO.Id, opt => opt.MapFrom(e => e.Id))
                .ForMember(pDTO => pDTO.Weight, opt => opt.MapFrom(e => e.Weight))
                .ForMember(pDTO => pDTO.Text, opt => opt.MapFrom(e => e.Text));
        }
    }
}
