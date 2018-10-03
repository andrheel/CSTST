using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS1.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserGroups
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
}
