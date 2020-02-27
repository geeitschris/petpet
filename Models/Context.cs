using Microsoft.EntityFrameworkCore;

namespace petpet.Models {
    public class Context : DbContext {
        // base() calls the parent class' constructor passing the "options" parameter along
        public Context (DbContextOptions options) : base (options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Comment> PetComments { get; set; }
        public DbSet<Mail> AllMail { get; set; }

    }
}