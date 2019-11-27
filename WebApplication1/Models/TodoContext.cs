using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class TodoContext: DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        /**
         * With repository pattern is necesary to register 
         * the DbSets for each entity in a DbContext, in this case, TodoContext
         * See: GenericRepository.cs for more information.
         */

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
