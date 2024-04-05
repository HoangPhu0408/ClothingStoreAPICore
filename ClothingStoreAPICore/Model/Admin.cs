using System;
using System.Collections.Generic;

namespace ClothingStoreAPICore.Model
{
    public partial class Admin
    {
        public int AdminId { get; set; }

        public string? AdminUserName { get; set; }

        public string? AdminPassword { get; set; }
    }

}
