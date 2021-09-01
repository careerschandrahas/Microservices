using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Entities;
using Catalog.API.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        // GET: api/values
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products =  await _productRepository.GetProducts();
            return Ok(products);
        }

        // GET api/values/5
        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _productRepository.GetProduct(id);
            if(product == null)
            {
                _logger.LogError($"Product with id: {id}, not found");
                return NotFound();
            }
            return Ok(product);
        }

        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var product = await _productRepository.GetProductByCategory(category);
            return Ok(product);
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepository.createProduct(product);
           return CreatedAtRoute("GetProduct", new { id = product.Id }, product);

        }

        // PUT api/values/5
        [HttpPut]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _productRepository.updateProduct(product));
        }

        // DELETE api/values/5
        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> DeleteProductById(string id)
        {
            return Ok(await _productRepository.deleteProduct(id));
        }
    }
}
