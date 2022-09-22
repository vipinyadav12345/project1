using System.Security.Claims;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dto;
using dotnet_rpg.Dto.Character;
using dotnet_rpg.models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{

    public class CharacterService : ICharacterService
    {
        
       //  private static List<character> characters = new List<character>(){
        //    new character(),
         //   new character { Id = 1, Name = "vipin"}
      //  };
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor  _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext  context, IHttpContextAccessor httpContextAccessor)
         {

            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
      private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
      .FindFirstValue(ClaimTypes.NameIdentifier));


 public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            character character =  _mapper.Map <character>(newCharacter);
            character.user     = await _context.users.FirstOrDefaultAsync(u => u.Id == GetUserId());


            _context.character.Add(character);
            await  _context.SaveChangesAsync();
              serviceResponse.Data =  await  _context.character
            .Where(c => c.user.Id == GetUserId())
            //characters.Add(_mapper.Map <character>(newCharacter));
          
            .Select(c=> _mapper.Map<GetCharacterDto>(c))
            .ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>>DeletCharacter(int id)
        {
           // throw new NotImplementedException(); 
               ServiceResponse<List<GetCharacterDto>> response = new  ServiceResponse<List<GetCharacterDto>>();

            try {
            character  Character =  await _context.character
            .FirstOrDefaultAsync(c => c.Id == id && c.user.Id == GetUserId());
        
            if(Character != null){


         _context.character.Remove(Character);
          await  _context.SaveChangesAsync();
          
         response.Data = _context.character
         .Where(c => c.user.Id == GetUserId())
         .Select( c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
        else{
            response.success = false ;
              response.Message = "Character not Found.";
        }

      }
            catch (Exception ex)
       {
        response.success = false ;
        response.Message = ex.Message;
       
        }

        return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var response =  new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacter = await _context.character
            .Where(c => c.user.Id == GetUserId())
            .ToListAsync();
            response.Data = dbCharacter.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
         //   return new ServiceResponse<List<GetCharacterDto>> 
           // {Data = characters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToList() };
        }

         public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {

            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter =  await _context.character

            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .FirstOrDefaultAsync(c => c.Id == id && c.user.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacte(UpdateCharacterDto updateCharacter)
        {
            ServiceResponse<GetCharacterDto> response = new  ServiceResponse<GetCharacterDto>();

            try {
           var Character =  await _context.character
           .Include(c => c.user)
            .FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);
           // _mapper.Map(updateCharacter, Character );
            if(Character.user.Id == GetUserId()){
         Character.Name  = updateCharacter.Name;
         Character.Hitpoints = updateCharacter.Hitpoints;
         Character.stringth =  updateCharacter.stringth ;
        Character.defence =  updateCharacter.defence;
        Character.inteligence =  updateCharacter.inteligence;
        Character.Class =  updateCharacter.Class ;
         await  _context.SaveChangesAsync();

        response.Data = _mapper.Map<GetCharacterDto>(Character);
            }
            else{
                  response.success = false ;
        response.Message = "Character not Found.";
            }
      }
        
            catch (Exception ex)
       {
        response.success = false ;
        response.Message = ex.Message;
       
        }

        return response;
    }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
        var response = new ServiceResponse<GetCharacterDto>();
        try{
            var Character = await _context.character
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && 
           c.user.Id == GetUserId());

            if(Character == null){
                response.success = false ;
              response.Message = "Character not Found."; 
              return response ;
            }
            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
            if(skill == null){
                 response.success = false ;
              response.Message = "Skiill not Found.";
              return response ;
            }
            Character.Skills.Add(skill);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetCharacterDto>(Character);


        }catch(Exception ex){
              response.success = false ;
              response.Message = ex.Message;
        }
            return response;
        }
    }
}
