namespace dotnet_rpg.Dto.Character
{
    public class GetAllCharactersDto
    {
                public int Id { get; set; }
                public String Name { get; set; } = "Learn";

            public int Hitpoints { get; set; } = 100;
            public int stringth { get; set; }= 10;
            public int defence { get; set; }=10;
            public int inteligence { get; set; }=10;
          //  public int MyProperty { get; set; }

          public  Rpgclass Class { get; set; }= Rpgclass.knight;
    }
}