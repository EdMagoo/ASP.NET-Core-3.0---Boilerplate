using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class TodoRepository : GenericRepository<TodoItem, long, TodoContext>
    {
        public TodoRepository(TodoContext todoContext) : base(todoContext)
        {

        }

        // add more methods if needed. For more information see: 
        // https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application


    }
}
