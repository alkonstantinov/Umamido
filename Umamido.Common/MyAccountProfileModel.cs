using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class MyAccountProfileModel
    {
        public int ClientId { get; set; }

        public String Name { get; set; }

        public String Family { get; set; }

        public String EMail { get; set; }

        public String Phone { get; set; }

        public String Error { get; set; }

        public String Password { get; set; }

        public String PasswordMd5
        {
            get
            {

                // step 1, calculate MD5 hash from input
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(this.Password ?? "");
                byte[] hash = md5.ComputeHash(inputBytes);

                // step 2, convert byte array to hex string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString();
            }

        }

        public String NewPassword { get; set; }

        public String NewPasswordMd5
        {
            get
            {

                // step 1, calculate MD5 hash from input
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(this.NewPassword ?? "");
                byte[] hash = md5.ComputeHash(inputBytes);

                // step 2, convert byte array to hex string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString();
            }

        }

        public String RePassword { get; set; }




    }
}
