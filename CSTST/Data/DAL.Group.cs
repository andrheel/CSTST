using CS1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSTST.Data
{
    public partial class DALEF : IGroupDAL
    {
        Groups IGroupDAL.Create(string Name) => atomed(db =>
                db.Groups.Add(new Groups() { Name = Name })
            ).Entity;


        void IGroupDAL.Delete(int Id) => atomed(db =>
        {
            db.Groups.Remove(db.Groups.Find(Id));
            db.GroupLinks.RemoveRange(db.GroupLinks.Where(e => e.ChildId == Id || e.ParentId == Id));
        });


        void IGroupDAL.Link(int GroupId, int ChildId) => atomed(db =>
                db.GroupLinks.Add(new GroupLinks() { ParentId = GroupId, ChildId = ChildId })
            );


        void IGroupDAL.UnLink(int GroupId, int ChildId) => atomed(db =>
                db.GroupLinks.RemoveRange(db.GroupLinks.Where(e => e.ParentId == GroupId && e.ChildId == ChildId))
            );


        async Task<List<Groups>> IGroupDAL.All() => await db.Groups.ToListAsync();

        async Task<Groups> IGroupDAL.Find(int Id) => await db.Groups.FindAsync(Id);

        async Task<Groups> IGroupDAL.FindByName(string name) => await db.Groups.SingleOrDefaultAsync(e => e.Name == name);


        async Task<List<Groups>> IGroupDAL.FindByUserId(int Id) => await
                (from ug in db.UserGroups
                 join g in db.Groups on ug.GroupId equals g.Id
                 where ug.UserId == Id
                 select g).ToListAsync();


        async Task<List<Groups>> IGroupDAL.FindAsParent(int Id) => await
                (from gg in db.GroupLinks
                 join g in db.Groups on gg.ChildId equals g.Id
                 where gg.ParentId == Id
                 select g).ToListAsync();


    }


}
