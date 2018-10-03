using CS1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSTST.Data
{
    public partial class DALEF : IDAL
    {
        readonly UserManagmentContext db;


        public DALEF(UserManagmentContext dbContext) { db = dbContext; }


        IDbContextTransaction Transaction(bool use) => use
                ? db.Database.BeginTransaction()
                : new NonTransacted();


        T atomed<T>(Func<UserManagmentContext, T> f)
        {
            using (var t = Transaction(true))
            {
                var result = f(db);
                db.SaveChanges();
                t.Commit();
                return result;
            }
        }


        void atomed(Action<UserManagmentContext> f)
        {
            using (var t = Transaction(true))
            {
                f(db);
                db.SaveChanges();
                t.Commit();
            }
        }

        async Task aatomed(Func<UserManagmentContext, Task> f)
        {
            using (var t = Transaction(true))
            {
                await f(db);
                t.Commit();
            }
        }

        async Task<U> aatomed<U>(Func<UserManagmentContext, Task<U>> f)
        {
            using (var t = Transaction(true))
            {
                var result = await f(db);
                t.Commit();
                return result;
            }
        }

        void IDAL.ResetDB() =>  atomed(db => 
        {
            db.UserGroups.RemoveRange(db.UserGroups);
            db.GroupLinks.RemoveRange(db.GroupLinks);
            db.Groups.RemoveRange(db.Groups);
            db.Users.RemoveRange(db.Users);

            var grps =
                "Employers,US,Manager,Ukraine,Developers,Main Ofice"
                .Split(',')
                .Select(e => new Groups() { Name = e })
                .ToArray()
                ;
            db.Groups.AddRange(grps);
            db.SaveChanges();

            var usrs =
                "O.Cole,J.Shane,V.Petrov,M.Popov"
                .Split(',')
                .Select(e => new Users() { Name = e })
                .ToArray()
                ;
            db.Users.AddRange(usrs);
            db.SaveChanges();

            // 
            int asInt(string x) => Convert.ToInt32(x);

            var gmaps = 
                "0:1,2,3,4|3:5|4:5".Split('|')
                .Select(m => m.Split(':'))
                .SelectMany(kv => kv[1].Split(',').Select(e => 
                    new GroupLinks() { ParentId = grps[asInt(kv[0])].Id, ChildId = grps[asInt(e)].Id }))
                ;
            db.GroupLinks.AddRange(gmaps);
            db.SaveChanges();


            var umap = "1:0,1|2:1|5:2,3".Split('|')
                .Select(m => m.Split(':'))
                .SelectMany(kv => kv[1].Split(',').Select(e => 
                    new UserGroups() { GroupId = grps[asInt(kv[0])].Id, UserId = usrs[asInt(e)].Id }))
                ;
            db.UserGroups.AddRange(umap);
            db.SaveChanges();

        });



        object IDAL.StatDB()
        {
            var ucnt = db.Users.Count();
            var gcnt = db.Groups.Count();
            var glcnt = db.GroupLinks.Count();
            var ugcnt = db.UserGroups.Count();
            return new { ucnt, gcnt, glcnt, ugcnt };
        }
    }


}
