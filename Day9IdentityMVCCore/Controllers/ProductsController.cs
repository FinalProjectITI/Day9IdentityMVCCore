using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdminDashBoard.Models;
using AdminDashBoard.Data;
using Microsoft.AspNetCore.Authorization;
using AdminDashBoard.ViewModel;
using AdminDashBoard.UploadImages;
using Microsoft.AspNetCore.Hosting;

namespace AdminDashBoard.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
       private readonly AlaslyfactoryContext _context;
        [Obsolete]
        public static IHostingEnvironment _environment;
        [Obsolete]
        public ProductsController(AlaslyfactoryContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var AlaslyfactoryContext = _context.Products.Include(p => p.Category).Include(p => p.Season).Include(p => p.Type);
            return View(await AlaslyfactoryContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Season)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["SeasonId"] = new SelectList(_context.Seasons, "Id", "Name");
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name");
            return View();
        }

        // POST: Products/Create
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,Quntity,Discount,CategoryId,TypeId,SeasonId,ShowInHome,ImagePath")] ProductImageVM productvm)
        {
            if (ModelState.IsValid)
            {
                Product newproduct = new Product
                {
                    Name = productvm.Name,
                    Description = productvm.Description,
                    Price = productvm.Price,
                    Discount = productvm.Discount,
                    CategoryId = productvm.CategoryId,
                    Quntity = productvm.Quntity,
                    SeasonId = productvm.SeasonId,
                    ShowInHome = productvm.ShowInHome,
                    TypeId = productvm.TypeId
                };
                _context.Add(newproduct);
                await _context.SaveChangesAsync();
                foreach (var image in productvm.ImagePath)
                {
                    string path =await Images.uploadImage(image, _environment);
                    ProductImage productImage = new ProductImage()
                    {
                        ImagePath = path,
                        ProductId = newproduct.Id
                    };
                    _context.Add(productImage);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productvm.CategoryId);
            ViewData["SeasonId"] = new SelectList(_context.Seasons, "Id", "Name", productvm.SeasonId);
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name", productvm.TypeId);
            return View(productvm);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["SeasonId"] = new SelectList(_context.Seasons, "Id", "Name", product.SeasonId);
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name", product.TypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,Quntity,Discount,CategoryId,TypeId,SeasonId,ShowInHome")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["SeasonId"] = new SelectList(_context.Seasons, "Id", "Name", product.SeasonId);
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name", product.TypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Season)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            var images = await _context.ProductImages.Where(I => I.ProductId == id).ToListAsync();
            foreach(var item in images)
            {
                Images.DeleteImage(item.ImagePath);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
