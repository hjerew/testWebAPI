#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testWebAPI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace testWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryItemsController : ControllerBase
    {
        private readonly CountryContext _context;
        private readonly FirebaseClient _client;

        //string firebaseURL = "https://fir-functions-api-430be-default-rtdb.firebaseio.com/.json";


        // Constructor - pass it a DI service in the same framework as "context"
        public CountryItemsController(CountryContext context, FirebaseClient client)
        {
            _context = context;
            _client = client;
        }

        // GET: api/CountryItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryItem>>> GetCountryItems()
        {
            //getstreamasync is a function of httpclient not firebaseclient, fix dat
            var countryItems = await _client.GetItemsAsync();
            if (countryItems == null)
            {
                return await _context.CountryItems.ToListAsync();
            }
            else
            {
                return countryItems;
            }

        }

        // GET: api/CountryItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryItem>> GetCountryItem(long id)
        {
            //var countryItem = await _context.CountryItems.FindAsync(id);
            var countryItem = await _client.GetItemAsync(id);

            if (countryItem == null)
            {
                return NotFound();
            }

            return countryItem;
        }

        // PUT: api/CountryItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountryItem(long id, CountryItem countryItem)
        {
            if (id != countryItem.Id)
            {
                return BadRequest();
            }

            //_context.Entry(countryItem).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)

            string result = await _client.SaveChangesAsync(id, countryItem);

            {
                if (!CountryItemExists(id))
                {
                    return NotFound();
                }
                //else
                //{
                //    throw;
                //}
            }

            return NoContent();
        }

        // POST: api/CountryItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CountryItem>> PostCountryItem(CountryItem countryItem)
        {
            //_context.CountryItems.Add(countryItem);
            //await _context.SaveChangesAsync();

            //if no ID assigned with post, append an ID 
            long newId;
            if (countryItem.Id == 0)
            {
                newId = await _client.GetNewIdAsync();
                countryItem.Id = newId;
                await _client.SaveChangesAsync(newId, countryItem);
            }
            else
            {
                await _client.PostAsync(countryItem);
            }

            return CreatedAtAction("GetCountryItem", new { id = countryItem.Id }, countryItem);
            //test
        }

        // DELETE: api/CountryItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountryItem(long id)
        {
            var countryItem = await _context.CountryItems.FindAsync(id);
            if (countryItem == null)
            {
                return NotFound();
            }

            _context.CountryItems.Remove(countryItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryItemExists(long id)
        {
            return _context.CountryItems.Any(e => e.Id == id);
        }
    }
}

