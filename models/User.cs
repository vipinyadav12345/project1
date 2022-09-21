using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
       
        public List<character>? Characters { get; set; }
    }
}
