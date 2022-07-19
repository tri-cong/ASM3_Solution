using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Models;

namespace DataAccess
{
    public class MemberDAO
    {
        FStoreDBContext context;
        private static MemberDAO instance;
        private static readonly object InstanceLock = new object();
        private MemberDAO()
        {
            context = new FStoreDBContext();
        }

        public static MemberDAO Instance
        {
            get
            {
                lock (InstanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }


        }

        public void Add(Member member)
        {
            try
            {
                var m = context.Members.FirstOrDefault(x => x.Email.Equals(member.Email));
                if (m == null)
                {
                    context.Members.Add(member);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Email is already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Member> GetAllMember()
        {
            try
            {
                return from member in context.Members select member;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public Member GetMemberById(int id)
        {
            try
            {
                var m = context.Members.FirstOrDefault(x => x.MemberId == id);
                return m;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var m = context.Members.FirstOrDefault(x => x.MemberId == id);
                if (m != null)
                {
                    context.Members.Remove(m);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("This member is not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(Member member)
        {
            try
            {
                var m = context.Members.FirstOrDefault(x => x.Email.Equals(member.Email) && x.MemberId != member.MemberId);
                if (m == null)
                {

                    m = context.Members.FirstOrDefault(x => x.MemberId == member.MemberId);
                    context.Entry(m).CurrentValues.SetValues(member);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Email is already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int GetMaxId()
        {
            try
            {
                int id = 0;
                id = (from x in context.Members
                      orderby x.MemberId descending
                      select x.MemberId).FirstOrDefault();
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Member CheckLogin(string email, string password)
        {
            FStoreDBContext fStoreDBContext = new FStoreDBContext();
            return (from member in fStoreDBContext.Members
                    where member.Email.Equals(email) && member.Password.Equals(password)
                    select member).FirstOrDefault();
        }

        public Member GetMemberByEmail(string email)
        {
            try
            {
                return context.Members.FirstOrDefault(x => x.Email.Equals(email));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
