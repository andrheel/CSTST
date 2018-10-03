using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace CSTST.Data
{
    public class NonTransacted : IDbContextTransaction
    {
        public Guid TransactionId => Guid.Empty;
        public void Commit() { }
        public void Dispose() { }
        public void Rollback() { }
    }
}