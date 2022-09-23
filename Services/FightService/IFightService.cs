using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Dto.Fight;

namespace dotnet_rpg.Services.FightService
{
    public interface IFightService
    {
    Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
    Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request);
    Task<ServiceResponse<FightResultDto>> FightResult(FightRequestDto request);
    Task<ServiceResponse<List<HighscoreDto>>> GetHighscore();
    }
}