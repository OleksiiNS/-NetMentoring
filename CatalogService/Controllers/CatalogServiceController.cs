using CatalogService.Entities;
using CatalogService.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class CatalogServiceController : ControllerBase
    {
        private readonly ILogger<CatalogServiceController> _logger;
        private ICategoryRepository _repository;

        public CatalogServiceController(ILogger<CatalogServiceController> logger, ICategoryRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [Route("categories")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Category>> GetCategories()
        {
            try
            {
                var categories = _repository.GetCategories();
                return Ok(categories);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);;
            }
        }

        [Route("categories")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddCategory(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest();
                }
                var category = _repository.AddCategory(name);
                return Ok(category);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("categories/{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateCategory(int id, string name)
        {
            try
            {
                if (id.GetType() != typeof(int) || string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest();
                }
                var category = _repository.UpdateCategory(id,name);
                return Ok(category);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("categories/{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteCategory(int id)
        {
            try
            {
                if (id.GetType() != typeof(int))
                {
                    return BadRequest();
                }
                var deleted = _repository.DeleteCategory(id);
                return deleted? Ok() : NotFound();
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("categories/{categoryId}/items")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Item>> GetItems(int categoryId, int page)
        {
            try
            {
                if (categoryId.GetType() != typeof(int))
                {
                    return BadRequest();
                }
                var items = _repository.GetItems(categoryId, page);
                return Ok(items);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("categories/{categoryId}/items")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddItem(int categoryId, string name)
        {
            try
            {
                if (categoryId.GetType() != typeof(int) || string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest();
                }
                var item = _repository.AddItem(categoryId, name);
                return Ok(item);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("categories/{categoryId}/items/{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateItem(int categoryId, int id, string name)
        {
            try
            {
                if (categoryId.GetType() != typeof(int) || id.GetType() != typeof(int) ||string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest();
                }
                var category = _repository.UpdateItem(categoryId, id, name);
                return Ok(category);
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("category/{categoryId}/item/{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteItem(int categoryId, int id)
        {
            try
            {
                if (categoryId.GetType() != typeof(int) || id.GetType() != typeof(int))
                {
                    return BadRequest();
                }
                var deleted = _repository.DeleteItem(categoryId, id);
                return deleted ? Ok() : NotFound();
            }
            catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}