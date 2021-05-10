using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace shows_buzz_feed.Mappings.Question
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Models.Question, QuestionViewModel>()
            .ForMember(pDTO => pDTO.Id, opt => opt.MapFrom(e => e.Id))
            .ForMember(pDTO => pDTO.Content, opt => opt.MapFrom(e => e.Content))
            .ForMember(pDTO => pDTO.Question_No, opt => opt.MapFrom(e => e.Question_No))
            .ForMember(pDTO => pDTO.Row_No, opt => opt.MapFrom(e => e.Row_No));
        }
    }
}
