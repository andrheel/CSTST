using CS1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSTST.Data
{
    public interface IGroupDAL
    {
        Task<List<Groups>> All();
        Groups Create(string Name);
        void Delete(int Id);
        void Link(int GroupId, int ChildId);
        void UnLink(int GroupId, int ChildId);
        Task<Groups> Find(int Id);
        Task<List<Groups>> FindByUserId(int Id);
        Task<List<Groups>> FindAsParent(int Id);
        Task<Groups> FindByName(string тфьу);
    }
}
