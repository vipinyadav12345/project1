using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Dto.Fight
{
    public class SkillAttackDto
    {
        public int AttackerID { get; set; }
        public int OpponentID { get; set; }
        public int SkillId { get; set; }
    }
}