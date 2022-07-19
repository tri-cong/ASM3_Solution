using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MemberRepository : IMemberRepository
    {
        public Member GetMemberByEmail(string email) => MemberDAO.Instance.GetMemberByEmail(email);

        void IMemberRepository.Add(Member member) => MemberDAO.Instance.Add(member);

        void IMemberRepository.Delete(int id) => MemberDAO.Instance.Delete(id);

        Member IMemberRepository.Get(int id) => MemberDAO.Instance.GetMemberById(id);

        IEnumerable<Member> IMemberRepository.GetAll() => MemberDAO.Instance.GetAllMember();

        int IMemberRepository.GetMaxId() => MemberDAO.Instance.GetMaxId();

        void IMemberRepository.Update(Member member) => MemberDAO.Instance.Update(member);
    }
}
