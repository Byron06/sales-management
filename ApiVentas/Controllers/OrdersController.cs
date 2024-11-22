using ApiVentas.Models;
using ApiVentas.Models.Dtos;
using ApiVentas.Repository;
using ApiVentas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiVentas.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        // Se agregan los posibles codes que puede responder
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetOrders()
        {
            var orderList = _orderRepository.GetOrders();
            var orderDtoList = orderList.Select(order => _mapper.Map<OrderDto>(order)).ToList();

            /*
            var customerDtoList = new List<CustomerDto>();
            foreach (var customer in customerList) {
                customerDtoList.Add(_mapper.Map<CustomerDto>(customer));
            }
            */

            return Ok(orderDtoList);
        }

        [HttpGet("{id:long}", Name = "GetOrder")]
        // Se agregan los posibles codes que puede responder
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOrder(long id)
        {
            var order = _orderRepository.GetOrder(id);

            if (order == null)
            {
                return NotFound();
            }

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        [HttpPost]
        // Se agregan los posibles codes que puede responder
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult PostOrder([FromBody] OrderCreateDto orderCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (orderCreateDto == null)
            {
                return BadRequest();
            }

            if (_orderRepository.ExistOrder(orderCreateDto.name))
            {
                ModelState.AddModelError("", $"La orden {orderCreateDto.name} ya existe");
                return StatusCode(404, ModelState);
            }

            var order = _mapper.Map<Order>(orderCreateDto);

            if (!_orderRepository.CreateOrder(order))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando la orden {orderCreateDto.name}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetCustomer", new { id = order.Id }, order);
        }


        [HttpPut("{id:long}", Name = "UpdateOrder")]
        // Se agregan los posibles codes que puede responder
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateOrder(long id, [FromBody] OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (orderDto == null || id != orderDto.id)
            {
                return BadRequest(ModelState);
            }

            var orderExist = _orderRepository.GetOrder(id);

            if (orderExist == null)
            {
                return NotFound($"No se encontro la orden {id}");
            }

            var order = _mapper.Map<Order>(orderDto);

            if (!_orderRepository.UpdateOrder(order))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando la orden {orderDto.name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:long}", Name = "DeleteOrder")]
        // Se agregan los posibles codes que puede responder
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteOrder(long id)
        {

            if (!_orderRepository.ExistOrder(id))
            {
                return NotFound($"No se encontro la orden {id}");
            }

            var order = _orderRepository.GetOrder(id);

            if (!_orderRepository.DeleteOrder(order))
            {
                ModelState.AddModelError("", $"Algo salio mal eliminando la orden: {order.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("GetOrdersByCustomer/{id:long}", Name = "GetOrdersByCustomer")]
        // Se agregan los posibles codes que puede responder
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOrdersByCustomer(long id)
        {
            var orderList = _orderRepository.GetOrdersByCustomer(id);

            if (orderList == null)
            {
                return NotFound();
            }

            var orderDtoList = new List<OrderDto>();

            foreach (var order in orderList) {
                orderDtoList.Add(_mapper.Map<OrderDto>(order));
            }
 
            return Ok(orderDtoList);
        }
    }
}
