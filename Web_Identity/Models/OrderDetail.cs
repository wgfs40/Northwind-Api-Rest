﻿using Repository.Pattern.Ef6;

namespace Web_Identity.Models
{
    public class OrderDetail : Entity
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public virtual Order Order { get; set; }
        //public virtual Product Product { get; set; }
    }
}
