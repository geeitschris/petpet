using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace petpet.Models {
    public class dashboardModel {
        public User userLogged { get; set; }
        public Pet pet { get; set; }
        public Comment comment { get; set; }
    }
}