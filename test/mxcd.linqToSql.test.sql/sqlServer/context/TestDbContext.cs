using Microsoft.EntityFrameworkCore;

namespace mxcd.linqToSql.test.sql
{
    public class TestDbContext:DbContext
    {

        public DbSet<Paciente> Pacientes { get; set; }

        public string ConnectionString => "Server=localhost;Database=myDataBase;User Id=sa;Password=deVops.Docker!;";

        public void EnsureExists()
        {
            using (var dbContext = new TestDbContext())
            {
                //Ensure database is created
                dbContext.Database.EnsureCreated();
            }
        }
    }
}