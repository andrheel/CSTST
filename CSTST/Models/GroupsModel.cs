using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS1.Models
{
    public class Groups
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GroupLinks
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
    }
}
