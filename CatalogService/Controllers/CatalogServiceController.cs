using CatalogService.Entities;
using CatalogService.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Category>> GetCategories()
        {
            try
            {
                var categories = _repository.GetCategories();
                var json = JsonConvert.SerializeObject(categories);
                return Ok(json);
            }
            catch {
                return BadRequest();
            }
        }

        [Route("category")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult AddCategory(string name)
        {
            try
            {
                var category = _repository.AddCategory(name);
                var json = JsonConvert.SerializeObject(category);
                return Ok(json);
            }
            catch {
                return Conflict();
            }
        }

        [Route("category/{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateCategory(int id, string name)
        {
            try
            {
                var category = _repository.UpdateCategory(id,name);
                var json = JsonConvert.SerializeObject(category);
                return Ok(json);
            }
            catch {
                return NotFound();
            }
        }

        [Route("category/{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteCategory(int id)
        {
            try
            {
                _repository.DeleteCategory(id);
                return Ok();
            }
            catch {
                return NotFound();
            }
        }

        [Route("category/{categoryId}/items")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<Item>> GetItems(int categoryId, int page)
        {
            try
            {
                var items = _repository.GetItems(categoryId, page);
                var json = JsonConvert.SerializeObject(items);
                return Ok(json);
            }
            catch {
                return BadRequest();
            }
        }

        [Route("category/{categoryId}/item")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult AddItem(int categoryId, string name)
        {
            try
            {
                var item = _repository.AddItem(categoryId, name);
                var json = JsonConvert.SerializeObject(item);
                return Ok(json);
            }
            catch {
                return Conflict();
            }
        }

        [Route("category/{categoryId}/item/{id}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateItem(int categoryId, int id, string name)
        {
            try
            {
                var category = _repository.UpdateItem(categoryId, id, name);
                var json = JsonConvert.SerializeObject(category);
                return Ok(json);
            }
            catch {
                return NotFound();
            }
        }

        [Route("category/{categoryId}/item/{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteItem(int categoryId, int id)
        {
            try
            {
                _repository.DeleteItem(categoryId, id);
                return Ok();
            }
            catch {
                return NotFound();
            }
        }
    }
}