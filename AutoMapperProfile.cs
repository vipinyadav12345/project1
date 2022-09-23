using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Dto;
using dotnet_rpg.Dto.Character;
using dotnet_rpg.Dto.Fight;
using dotnet_rpg.Dto.Skill;
using dotnet_rpg.Dto.Weapon;

namespace dotnet_rpg
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<character,GetCharacterDto>();
             CreateMap<AddCharacterDto,character>();
         //    CreateMap<UpdateCharacterDto,character>();
          //   CreateMap<GetCharacterDto,character>();
             CreateMap<Weapon , GetWeaponDto>();
              CreateMap<Skill , GetSkillDto>();
         CreateMap<character,HighscoreDto>();
        }
    }
}