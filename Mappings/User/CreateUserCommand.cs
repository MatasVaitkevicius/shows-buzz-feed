using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shows_buzz_feed.Mappings.User
{
    public class CreateUserCommand
    {
        [Key]
        public int Id { get; set; }
        public int Name { get; set; }
    }
}
