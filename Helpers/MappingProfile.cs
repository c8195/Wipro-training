using AutoMapper;
using DoConnect.API.Models;
using DoConnect.API.Models.DTOs;

namespace DoConnect.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles will be set manually

            // Question mappings
            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.AnswerCount, opt => opt.MapFrom(src => src.Answers.Count));
            
            CreateMap<Question, QuestionListDto>()
                .ForMember(dest => dest.AnswerCount, opt => opt.MapFrom(src => src.Answers.Count));

            CreateMap<CreateQuestionDto, Question>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Answers, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            // Answer mappings
            CreateMap<Answer, AnswerDto>();
            CreateMap<CreateAnswerDto, Answer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Question, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            // Image mappings
            CreateMap<Image, ImageDto>();
        }
    }
}