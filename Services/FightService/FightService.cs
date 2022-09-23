using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dto.Fight;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FightService(DataContext Context , IMapper mapper)
        {
            _mapper = mapper;
            _context = Context;
        }

        public async  Task<ServiceResponse<FightResultDto>> FightResult(FightRequestDto request)
        {
          var response = new ServiceResponse<FightResultDto>
          {
            Data = new FightResultDto()
          }; 
            try
            {
                     var characters = await _context.character
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => request.CharacterIds.Contains(c.Id)).ToListAsync();

                bool defeated = false;
                while (!defeated)
                {
                 foreach (character attacker in characters)
                    {  
                        var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];
                        
                        int damage = 0;
                        string attackUsed = string.Empty;

                         bool useWeapon = new Random().Next(2) == 0 ;
                         if(useWeapon){
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                         }
                         else
                         {
                         var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                         }

                          response.Data.Log
                            .Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage.");
                     if (opponent.Hitpoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            response.Data.Log.Add($"{opponent.Name} has been defeated!");
                            response.Data.Log.Add($"{attacker.Name} wins with {attacker.Hitpoints} HP left!");
                            break;
                        }

                }
                }
                 characters.ForEach(c =>
                {
                    c.Fights++;
                    c.Hitpoints = 100;
                });
                  await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                response.success = false;
                response.Message = ex.Message;
                
            }
            return response;

        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
           var response =  new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.character
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerID);
                var opponent = await _context.character
                .FirstOrDefaultAsync(c => c.Id == request.OpponentID);
                var Skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);
                if (Skill == null)
                {
                    response.success = false;
                    response.Message = $"{attacker.Name} doesn't know that skill.";
                    return response;
                }

                int damage = DoSkillAttack(attacker, opponent, Skill);

                if (opponent.Hitpoints <= 0)
                    response.Message = $"{opponent.Name} has been defeated!";
                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.Hitpoints,
                    OpponentHP = opponent.Hitpoints,
                    Damage = damage

                };

            }
            catch (Exception ex)
            {
                response.success = false;
                response.Message = ex.Message;
                
            }
            return response;
        }

        private static int DoSkillAttack(character? attacker, character? opponent, Skill? Skill)
        {
            int damage = Skill.Damage + (new Random().Next(attacker.inteligence));

            damage -= new Random().Next(opponent.defence);

            if (damage > 0)
                opponent.Hitpoints -= damage;
            return damage;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        { 
            var response =  new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.character
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.character
                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                // int Damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                int damage = DoWeaponAttack(attacker, opponent);

                if (opponent.Hitpoints <= 0)
                    response.Message = $"{opponent.Name} has been defeated!";
                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.Hitpoints,
                    OpponentHP = opponent.Hitpoints,
                    Damage = damage

                };

            }
            catch (Exception ex)
            {
                response.success = false;
                response.Message = ex.Message;
                
            }
            return response;
        }

        private static int DoWeaponAttack(character? attacker, character? opponent)
        {
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.stringth));

            damage -= new Random().Next(opponent.defence);

            if (damage > 0)
                opponent.Hitpoints -= damage;
            return damage;
        }

        public async Task<ServiceResponse<List<HighscoreDto>>> GetHighscore()
        {
           // throw new NotImplementedException();
           var characters = await _context.character
           .Where(c => c.Fights > 0)
           .OrderByDescending(c => c.Victories)
           .ThenBy(c => c.Defeats)
           .ToListAsync();

            var response = new ServiceResponse<List<HighscoreDto>>
            {
                Data = characters.Select(c => _mapper.Map<HighscoreDto>(c)).ToList()
            };
            

            return response ;
        }
    }
}