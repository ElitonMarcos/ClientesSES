using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clientes.Models;
using Clientes.Models.ValueObjects;


namespace Clientes.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationContext _context;

        public ClientesController(ApplicationContext context)
        {
            _context = context;
        }

       
        // GET: Clientes
        public async Task<IActionResult> Index(string nome, string documento)
        {
            var clientes = _context.Clientes.Where(c => !c.IsDeleted);

            if (!string.IsNullOrEmpty(nome))
            {
                clientes = clientes.Where(c => c.Nome.Contains(nome));
            }

            if (!string.IsNullOrEmpty(documento))
            {
                clientes = clientes.Where(c => c.Documento.Contains(documento));
            }


            return View(await clientes.ToListAsync());
        }

        public ActionResult Search(string nome, string documento)
        {
            var clientes = _context.Clientes.Where(c => !c.IsDeleted);

            if (!string.IsNullOrEmpty(nome))
            {
                clientes = clientes.Where(c => c.Nome.Contains(nome));
            }

            if (!string.IsNullOrEmpty(documento))
            {
                clientes = clientes.Where(c => c.Documento.Contains(documento));
            }

            var result = clientes.Select(c => new
            {
                c.Id,
                c.Nome,
                TipoCliente = c.TipoCliente.ToString()=="PF" ? "Pessoa Física" : c.TipoCliente.ToString()=="PJ" ? "Pessoa Jurídica" : "",
                c.Documento,
                Cadastro = c.Cadastro.ToString("dd/MM/yyyy"),
                c.Telefone
            }).ToList();

            return Json(result);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewBag.TipoCliente = new SelectList(Enum.GetValues(typeof(TipoCliente)));
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,TipoCliente,Documento,Cadastro,Telefone")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewBag.TipoCliente = new SelectList(Enum.GetValues(typeof(TipoCliente)));
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,TipoCliente,Documento,Cadastro,Telefone")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            cliente.IsDeleted = true;
            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
