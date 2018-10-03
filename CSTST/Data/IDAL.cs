namespace CSTST.Data
{
    public interface IDAL : IGroupDAL, IUserDAL
    {
        void ResetDB();
        object StatDB();
    }
}