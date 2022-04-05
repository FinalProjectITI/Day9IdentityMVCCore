using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdminDashBoard.Data;
using AdminDashBoard.Models;

namespace AdminDashBoard.Controllers
{
    public class OrdersController : Controller
    {
        List<State> states = new List<State>() { 
            new State() { Id = 0, Name = "انتظار"},
            new State() { Id = 1, Name = "تم الشحن"},
            new State() { Id = 2, Name = "تم التوصيل"},
        };
        private readonly AlaslyfactoryContext _context;

        public OrdersController(AlaslyfactoryContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var alaslyfactoryContext = _context.Orders.Include(o => o.Cart).Include(o => o.User);
            return View(await alaslyfactoryContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Cart)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["StateName"] = states.Where(s => s.Id == order.status).First().Name;
            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CartID"] = new SelectList(_context.Carts, "ID", "ID");
            ViewData["UserID"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserID,CartID,TotalPrice,status,Address,PaymentMethod")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartID"] = new SelectList(_context.Carts, "ID", "ID");
            ViewData["UserID"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            //ViewData["State"] = new SelectList(states, "Id", "Name", order.status);
            if(order.status < 2)
            {
                int newState = order.status + 1;
                ViewData["NextStateName"] = states.Where(s => s.Id == newState).First().Name;
            }
            ViewData["StateName"] = states.Where(s => s.Id == order.status).First().Name;
            //ViewData["CartID"] = new SelectList(_context.Carts, "ID", "UserID", order.CartID);
            ViewData["CartID"] = order.CartID;
            //ViewData["UserID"] = new SelectList(_context.AspNetUsers, "Id", "Id", order.UserID);
            ViewData["UserID"] = _context.AspNetUsers.Where(u => u.Id == order.UserID).First().UserName;
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,CartID,TotalPrice,status,Address,PaymentMethod")] Order order)
        {
            if (id != order.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(order);
                    Order order1 = _context.Orders.Find(id);
                    order1.status += 1;
                    _context.Update(order1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CartID"] = new SelectList(_context.Carts, "ID", "UserID", order.CartID);
            //ViewData["UserID"] = new SelectList(_context.AspNetUsers, "Id", "Id", order.UserID);
            int newState = order.status + 1;
            ViewData["NextStateName"] = states.Where(s => s.Id == newState).First().Name;
            ViewData["StateName"] = states.Where(s => s.Id == order.status).First().Name;
            ViewData["CartID"] = order.CartID;
            ViewData["UserID"] = _context.AspNetUsers.Where(u => u.Id == order.UserID).First().UserName;
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Cart)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["StateName"] = states.Where(s => s.Id == order.status).First().Name;
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }
    }
    public class State {
        public int Id { set; get; }
        public string Name { set; get; }
    }
}
