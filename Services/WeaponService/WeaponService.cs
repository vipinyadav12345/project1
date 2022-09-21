using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dto.Character;
using dotnet_rpg.Dto.Weapon;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _HttpContextAccessor;
         private readonly IMapper _mappper;

        public WeaponService(DataContext context, IHttpContextAccessor  HttpContextAccessor, IMapper mappper)
            {
            _context = context;
            _HttpContextAccessor = HttpContextAccessor;
            _mappper = mappper;
        }


        public async  Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            try{
                character character = await _context.character
                .FirstOrDefaultAsync(c =>c.Id == newWeapon.CharacterId &&
                c.user.Id == int.Parse(_HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))) ;

                if(character == null){
                    response.success = false;
                response.Message = "Character not Found.";
                 return response;

                }
                Weapon weapon = new Weapon{
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character

                };
                    _context.Weapons.Add(weapon);
                    await _context.SaveChangesAsync();
                    response.Data = _mappper.Map<GetCharacterDto>(character);

            }catch(Exception ex){
                response.success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}