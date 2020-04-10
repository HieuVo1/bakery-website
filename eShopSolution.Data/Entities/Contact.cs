using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Contact
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string  Email { set; get; }
        public string  Message { set; get; }
    }
}
