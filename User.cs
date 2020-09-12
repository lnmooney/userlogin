using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UFISApp
{ 
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LoginErrorMessage { get; set; }
    }
}