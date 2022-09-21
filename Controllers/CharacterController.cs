using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dotnet_rpg.Dto;
using dotnet_rpg.Dto.Character;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
       
      private readonly ICharacterService  _characterService;
      public  CharacterController(ICharacterService characterService)
      {
            _characterService = characterService;

      }
      //  [AllowAnonymous]
        [HttpGet("GetAll")]
       // [HttpGet]

        public  async Task<ActionResult<ServiceResponse<List<GetAllCharactersDto>>>> Get()
        {
         // int userId = int.Parse(User.Claims.FirstOrDefault(c =>c.Type == ClaimTypes.NameIdentifier).Value);
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetAllCharactersDto>>>>Delete(int id)
        {
          //  return Ok(await _characterService.GetCharacterById(id));
           var response = await _characterService.DeletCharacter(id);
        if(response.Data == null)
        {
            return NotFound(response);
        }
         return Ok(response);

}

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetAllCharactersDto>>>Getsingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));

}

        [HttpPost]
        
        [Route("add")]
         public  async Task<ActionResult<List<ServiceResponse<GetAllCharactersDto>>>> AddCharacter(AddCharacterDto NewCharacter){
         return Ok(await _characterService.AddCharacter(NewCharacter));
         }


    

        [HttpPut]
         public  async Task<ActionResult<ServiceResponse<GetAllCharactersDto>>> UpdateCharacte(UpdateCharacterDto updateCharacter)
    {
        var response = await _characterService.UpdateCharacte(updateCharacter);
        if(response.Data == null)
        {
            return NotFound(response);
        }
         return Ok(response);
         }

    }
}