using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Models;

namespace DataAccess.Repository
{
    public interface IMemberRepository
    {
        void Add(Member member);
        Member Get(int id);

        IEnumerable<Member> GetAll();
        void Update(Member member);
        void Delete(int id);
        int GetMaxId();

        Member GetMemberByEmail(string email);
    }
}
