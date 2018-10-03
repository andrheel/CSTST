using CS1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSTST.Data
{
    public interface IUserDAL
    {
        Task<List<Users>> All();
        Users Create(string Name);
        void Delete(int Id);
        Task<Users> Find(int Id);
        void Link(int GroupId, int UserId);
        void UnLink(int GroupId, int UserId);
    }
}
