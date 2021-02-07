using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_DoList.Infrastructure;
using To_DoList.Models;

namespace To_DoList.Controllers
{
    public class TodoController : Controller
    {
        private readonly TodoContext context;
        public TodoController(TodoContext context)
        {
            this.context = context;
        }
        //GET / 
        public async Task<ActionResult> Index()
        {
            IQueryable<TodoList> items = from i in context.TodoList orderby i.Id select i;

            List<TodoList> todoList = await items.ToListAsync();
            return View(todoList);
        }
        //GET /Todo/Create
        public IActionResult Create() => View();
        //POST /Todo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Add(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been added!";
                return RedirectToAction("Index");
            }
            return View(item);
        }
        //GET /Todo/Edit/...
        public async Task<ActionResult> Edit(int id)
        {
            TodoList item = await context.TodoList.FindAsync(id);
            if (item == null)
            {
                return NotFound(item);
            }
            return View(item);
        }
        //POST /todo/Edit/...
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Update(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been updated!";
                return RedirectToAction("Index");
            }
            return View(item);
        }
        //GET /todo/Delete/...
        public async Task<ActionResult> Delete(int id)
        {
            TodoList item = await context.TodoList.FindAsync(id);
            if (item == null)
            {
                TempData["Error"] = "The item does not exist!";
            }
            else
            {
                context.TodoList.Remove(item);
                await context.SaveChangesAsync();
                TempData["Success"] = "The item has been deleted!";
            }
            return RedirectToAction("Index");
        }
    }
}
