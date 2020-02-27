using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace petpet.Models {
    public class PetAndComments {
        public Pet pet { get; set; }
        public Comment comment { get; set; }
    }
}