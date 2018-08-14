using AutoMapper;
using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile() : base()
        {
            CreateMap<ChalcoArticle, ArticleCardViewModel>();
        }
    }
}
