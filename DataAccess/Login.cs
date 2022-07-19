using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using BussinessObject.Models;

namespace DataAccess
{
    
    public class Login
    {

        public string CheckLogin(string Email, string Password)
        {
            string fileName = "appsetting.json";
            string json = File.ReadAllText(fileName);
            Account account = JsonSerializer.Deserialize<Account>(json);

            string result = "";
            if (account != null)
            {
                if (Email == account.Email && Password == account.Password)
                {
                    result = account.Role;
                }
                else
                {
                        Member mem = MemberDAO.Instance.CheckLogin(Email, Password);
                    if(mem != null)
                    {
                        result = "user";
                    } else
                    {
                        throw new Exception("Wrong email or password");
                    }
                        
                }

            }
            else
            {
                throw new Exception("Login Error");
            }


            return result;

        }
    }
}
public class Account
{

    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}