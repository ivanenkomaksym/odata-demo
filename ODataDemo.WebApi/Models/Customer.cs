﻿namespace ODataDemo.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserRole? UserRole { get; set; }
        public List<Order> Orders { get; set; }
        public string StringPropertyWithDefaultValueToBeOmitted { get; set; } = string.Empty;
        public bool BooleanPropertyWithDefaultValueToBeOmitted { get; set; } = false;
        public IContract Contract { get; set; }
    }
}
