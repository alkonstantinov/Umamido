using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class CheckOutModel
    {
        public int? ClientId { get; set; }

        public string UserName { get; set; }

        public string[] Addresses { get; set; }

        public int AddressNum { get; set; }

        
        public string Note { get; set; }

        public int PaymentTypeId { get; set; }

        public Boolean Invoice { get; set; }

        public List<GoodPresentationModel> CartItems { get; set; }

        public decimal ProductsTotal { get; set; }

        public decimal DeliveryTotal { get; set; }

        public decimal TotalTotal { get; set; }

        public String FirstName { get; set; }

        public String Family { get; set; }

        public String EMail { get; set; }

        public String Phone { get; set; }

        
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


        public String CompanyName { get; set; }
        public String EIK { get; set; }
        public String VAT { get; set; }
        public String PersonName { get; set; }
        public String CompanyAddress { get; set; }
        public String Country { get; set; }
        public String PK { get; set; }

        public String Address { get; set; }
        public String Address2 { get; set; }

        public String ErrorMessage { get; set; }

        

        public CheckOutModel()
        {
            this.CartItems = new List<GoodPresentationModel>();
        }
    }
}
