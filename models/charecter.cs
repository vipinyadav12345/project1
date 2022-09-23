using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.models
{
    public class character
    {
        public int Id { get; set; }

        public String Name { get; set; } = "Learn";

        public int Hitpoints { get; set; } = 100;
        public int stringth { get; set; }= 10;
        public int defence { get; set; }=10;
        public int inteligence { get; set; }=10;
          //  public int MyProperty { get; set; }

        public  Rpgclass Class { get; set; }= Rpgclass.knight;
        public User? user { get; set; }
        public Weapon Weapon { get; set; }
        public List<Skill> Skills { get; set; }
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
     
    }
}