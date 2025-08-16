using AppTaskManager.Models;
using Microsoft.EntityFrameworkCore;

namespace AppTaskManager.Controllers
{
    /// <summary>
    /// AppContext for Entity framework
    /// </summary>
    internal class AppContext : DbContext
    {
        /// <summary>
        /// Property holding Task models from DB
        /// </summary>
        public DbSet<TaskModel> TaskModels { get; set; }

        /// <summary>
        /// Property holding Task checks from DB
        /// </summary>
        public DbSet<TaskCheck> TaskChecks { get; set; }

        public AppContext()
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Overrided method for configuring DB
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = taskDB.db"); 
        }

    }
}
