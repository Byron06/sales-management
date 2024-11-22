using ApiVentas.Models;
using ApiVentas.Models.Dtos;
using ApiVentas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiVentas.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        // Se agregan los posibles codes que puede responder
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCustomers() 
        { 
            var customerList = _customerRepository.GetCustomers();
            var customerDtoList = customerList.Select(customer => _mapper.Map<CustomerDto>(customer)).ToList();

            /*
            var customerDtoList = new List<CustomerDto>();
            foreach (var customer in customerList) {
                customerDtoList.Add(_mapper.Map<CustomerDto>(customer));
            }
            */

            return Ok(customerDtoList);
        }

        [HttpGet("{id:long}", Name = "GetCustomer")]
        // Se agregan los posibles codes que puede responder
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCustomer(long id)
        {
            var customer = _customerRepository.GetCustomer(id);

            if (customer == null) { 
                return NotFound();
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return Ok(customerDto);
        }

        [HttpGet("GetSearchCustomers")]
        // Se agregan los posibles codes que puede responder
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetSearchCustomers(string text)
        {
            try
            {
                var customerList = _customerRepository.GetSearchCustomers(text);

                if (customerList.Any()) {
                    return Ok(customerList);
                }

                return NotFound();
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al recuperar la información");
            }            
        }

        [HttpPost]
        // Se agregan los posibles codes que puede responder
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult PostCustomer([FromBody] CustomerCreateDto customerCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (customerCreateDto == null) {
                return BadRequest();
            }

            if (_customerRepository.ExistCustomer(customerCreateDto.name))
            {
                ModelState.AddModelError("", $"El cliente {customerCreateDto.name} ya existe");
                return StatusCode(404, ModelState);
            }

            var customer = _mapper.Map<Customer>(customerCreateDto);

            if (!_customerRepository.CreateCustomer(customer)) {
                ModelState.AddModelError("", $"Algo salio mal guardando el cliente {customerCreateDto.name}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetCustomer", new { id = customer.Id}, customer);
        }

        [HttpPatch("{id:long}", Name = "PatchCustomer")]
        // Se agregan los posibles codes que puede responder
        [ProducesResponseType(StatusCodes.Status204NoContent)]        
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PatchCustomer(long id, [FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (customerDto == null || id != customerDto.id)
            {
                return BadRequest(ModelState);
            }

            var customerExist = _customerRepository.GetCustomer(id);

            if (customerExist == null)
            {
                return NotFound($"No se encontro el cliente {id}");
            }

            var customer = _mapper.Map<Customer>(customerDto);

            if (!_customerRepository.UpdateCustomer(customer))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el cliente {customerDto.name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpPut("{id:long}", Name = "UpdateCustomer")]
        // Se agregan los posibles codes que puede responder
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCustomer(long id, [FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (customerDto == null || id != customerDto.id)
            {
                return BadRequest(ModelState);
            }

            var customerExist = _customerRepository.GetCustomer(id);

            if (customerExist == null)
            {
                return NotFound($"No se encontro el cliente {id}");
            }

            var customer = _mapper.Map<Customer>(customerDto);

            if (!_customerRepository.UpdateCustomer(customer))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el cliente {customerDto.name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:long}", Name = "DeleteCustomer")]
        // Se agregan los posibles codes que puede responder
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCustomer(long id)
        {

            if (!_customerRepository.ExistCustomer(id))
            {
                return NotFound($"No se encontro el cliente {id}");
            }

            var customer = _customerRepository.GetCustomer(id);

            if (!_customerRepository.DeleteCustomer(customer))
            {
                ModelState.AddModelError("", $"Algo salio mal eliminando el cliente: {customer.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
