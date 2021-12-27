using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToreadApi.Models;

namespace ToreadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ToreadItemsController : ControllerBase
    {
        private readonly ToreadContext _context;

        public ToreadItemsController(ToreadContext context)
        {
            _context = context;
        }



        // GET: api/ToreadItems   notice: we don't use method name
        [HttpGet]
        [Produces("application/json")]
        [SwaggerOperation("Zwraca wszystkie zadania.", "Używa EF")]
        [SwaggerResponse(200, "Sukces", Type = typeof(List<ToreadItem>))]        
        public async Task<ActionResult<IEnumerable<ToreadItem>>> GetToreadItems()
        {
            return await _context.ToreadItems.ToListAsync();  //http 200
        }



        // GET: api/ToreadItems/5
        [HttpGet("{id}")]        
        [Produces("application/json")]
        [SwaggerOperation("Zwraca zadanie o podanym {id}.", "Używa EF FindAsync()")]
        [SwaggerResponse(200, "Znaleziono zadanie o podanym {id}", Type = typeof(ToreadItem))]
        [SwaggerResponse(404, "Nie znaleziono zadania o podanym {id}")]
        public async Task<ActionResult<ToreadItem>> GetToreadItem(
            [SwaggerParameter("Podaj nr zadnia które chcesz odczytać", Required = true)]
            int id)
        {
            var toreadItem = await _context.ToreadItems.FindAsync(id);

            if (toreadItem == null)
            {
                return NotFound(); //http 404
            }

            return toreadItem;    //http 200
        }


        // PUT: api/ToreadItems/5        
        [HttpPut("{id}")]
        [Produces("application/json")]
        [SwaggerOperation("Aktualizuje zadanie o podanym {id}.", "Używa EF")]
        [SwaggerResponse(204, "Zaktualizowano zadanie o podanym {id}")]
        [SwaggerResponse(400, "Nie rozpoznano danych wejściowych")]
        [SwaggerResponse(404, "Nie znaleziono zadania o podanym {id}")]
        public async Task<IActionResult> PutToreadItem(
            [SwaggerParameter("Podaj nr zadnia które chcesz zaktualizować", Required = true)]
            int id,
            [SwaggerParameter("Definicja zadania", Required = true)]
            ToreadItem toreadItem)
        {
            if (id != toreadItem.Id)
            {
                return BadRequest(); //http 400
            }

            _context.Entry(toreadItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToreadItemExists(id))
                {
                    return NotFound();  //http 404
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); //http 204
        }


        // POST: api/ToreadItems        
        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation("Tworzy nowe zadanie.", "Używa EF")]
        [SwaggerResponse(201, "Zapis z sukcesem.", Type = typeof(ToreadItem))]
        public async Task<ActionResult<ToreadItem>> PostToreadItem(
            [SwaggerParameter("Definicja zadania", Required = true)]
            ToreadItem toreadItem)
        {
            _context.ToreadItems.Add(toreadItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToreadItem", new { id = toreadItem.Id }, toreadItem);  //http 201, add Location header
        }



        // DELETE: api/ToreadItems/5
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [SwaggerOperation("Usuwa zadanie o podanym {id}.", "Używa EF")]
        [SwaggerResponse(204, "Usunięto zadanie o podanym {id}")]        
        [SwaggerResponse(404, "Nie znaleziono zadania o podanym {id}")]
        public async Task<IActionResult> DeleteToreadItem(
            [SwaggerParameter("Podaj nr zadnia które chcesz usunąć", Required = true)]
            int id)
        {
            var toreadItem = await _context.ToreadItems.FindAsync(id);
            if (toreadItem == null)
            {
                return NotFound();  //http 404
            }

            _context.ToreadItems.Remove(toreadItem);
            await _context.SaveChangesAsync();

            return NoContent(); //http 204
        }



        private bool ToreadItemExists(int id)
        {
            return _context.ToreadItems.Any(e => e.Id == id);
        }
    }
}
