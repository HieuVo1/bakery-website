using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class RoleApp:IdentityRole<Guid>
    {
        public string Description { set; get; }
    }
}
