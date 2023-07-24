using Microsoft.EntityFrameworkCore;
using Task_Management.Model.Data;
using Assignment = Task_Management.Model.Data.Assignment;

namespace Task_Management.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Progress> Progresses { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<AssignMap> AssignMaps { get; set; }
    public DbSet<AccountProgress> AccountProgresss { get; set; }
    public DbSet<Additional> Additionals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Set Unique
        modelBuilder.Entity<Account>().HasIndex(acc => new
        {
            acc.Email,
            acc.Username
        }).IsUnique();


        //Set Relationship
        //Role - AccountRole (One to Many)
        modelBuilder.Entity<Role>()
            .HasMany(role => role.AccountRoles)
            .WithOne(acc_role => acc_role.Role)
            .HasForeignKey(acc_role => acc_role.RoleGuid)
            .OnDelete(DeleteBehavior.SetNull);

        //Category - AssignMap (One to Many)
        modelBuilder.Entity<Category>()
            .HasMany(cat => cat.AssignMaps)
            .WithOne(assign_map => assign_map.Category)
            .HasForeignKey(assign_map => assign_map.CategoryGuid)
            .OnDelete(DeleteBehavior.SetNull);

        //Account - AccountRole (One to Many)
        modelBuilder.Entity<Account>()
            .HasMany(acc => acc.AccountRoles)
            .WithOne(acc_role => acc_role.Account)
            .HasForeignKey(acc_role => acc_role.AccountGuid)
            .OnDelete(DeleteBehavior.Cascade);

        //Assignment - AssignMap (One to Many)
        modelBuilder.Entity<Assignment>()
            .HasMany(assign => assign.AssignMaps)
            .WithOne(assign_map => assign_map.Assignment)
            .HasForeignKey(assign_map => assign_map.AssignmentGuid)
            .OnDelete(DeleteBehavior.Cascade);

        //Account - AccountProgress (One to Many)
        modelBuilder.Entity<Account>()
            .HasMany(acc => acc.AccountProgresses)
            .WithOne(prog => prog.Account)
            .HasForeignKey (prog => prog.AccountGuid)
            .OnDelete(DeleteBehavior.SetNull);

        //Account - Assignment (One to Many)
        modelBuilder.Entity<Account>()
            .HasMany(acc => acc.Assignments)
            .WithOne(assignment => assignment.Account)
            .HasForeignKey(assignment => assignment.ManagerGuid)
            .OnDelete(DeleteBehavior.SetNull);

        //Assignment - Progress (One to Many)
        modelBuilder.Entity<Assignment>()
            .HasMany(assign => assign.Progresses)
            .WithOne(prog => prog.Assignment)
            .HasForeignKey(prog => prog.AssignmentGuid)
            .OnDelete(DeleteBehavior.SetNull);

        //Progress - AccountProgress (One to Many)
        modelBuilder.Entity<Progress>()
            .HasMany(prog => prog.AccountProgress)
            .WithOne(acc_prog => acc_prog.Progress)
            .HasForeignKey(acc_prog => acc_prog.ProgressGuid)
            .OnDelete(DeleteBehavior.SetNull);

        //Progress - Additional (One to Many)
        modelBuilder.Entity<Progress>()
            .HasMany(prog => prog.Additionals)
            .WithOne(additional => additional.Progress)
            .HasForeignKey(additional => additional.ProgressGuid)
            .OnDelete (DeleteBehavior.SetNull);
    }
}
