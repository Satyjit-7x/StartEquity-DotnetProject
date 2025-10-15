using System.Collections.Generic;
using StartEquity.Models;

namespace StartEquity.Repositories
{
    public interface IRoundRepository
    {
        List<Round> GetAll();
        Round GetById(string id);
        void Insert(Round r);
        void Update(Round r);
        void Delete(Round r);
    }
}
