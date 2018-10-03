using CS1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSTST.Data
{
    public partial class DALEF : IUserDAL
    {
        Users IUserDAL.Create(string Name) => atomed(db =>
                db.Users.Add(new Users() { Name = Name })
            ).Entity;


        void IUserDAL.Delete(int Id) => atomed(db =>
            {
                db.Users.Remove(db.Users.Find(Id));
                db.UserGroups.RemoveRange(db.UserGroups.Where(e => e.UserId == Id));
            });


        void IUserDAL.Link(int GroupId, int UserId) => atomed(db =>
            db.UserGroups.Add(new UserGroups() { GroupId = GroupId, UserId = UserId })
        );


        void IUserDAL.UnLink(int GroupId, int UserId) => atomed(db =>
            db.UserGroups.RemoveRange(db.UserGroups.Where(e => e.GroupId == GroupId && e.UserId == UserId))
        );


        Task<List<Users>> IUserDAL.All() => db.Users.ToListAsync();


        async Task<Users> IUserDAL.Find(int Id) => await db.Users.FindAsync(Id);


    }
}
