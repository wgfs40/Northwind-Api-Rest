using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Northwind.Api.Entities;
using Northwind.Api.Helpers;
using Northwind.Api.Models;
using Northwind.Api.Services;
using System;
using System.Collections.Generic;

namespace Northwind.Api.Controllers
{
    [Route("api/customers/{CustomerId}/Order")]
    public class OrderController : Controller
    {
        private INorthwindRepository _northwindRepository;

        public OrderController(INorthwindRepository northwindRepository)
        {
            this._northwindRepository = northwindRepository;
        }

        [HttpGet()]
        public IActionResult GetOrderForCustomer(string customerid)
        {
            if (!_northwindRepository.CustomerExists(customerid))
            {
                return NotFound();
            }

            var OrderForcustomerFromRepo = this._northwindRepository.GetOrdersForCustomer(customerid);

            var OrderForcustomer = Mapper.Map<IEnumerable<OrderDto>>(OrderForcustomerFromRepo);

            return Ok(OrderForcustomer);
        }
        [HttpGet("{OrderID}",Name = "GetOrderToCustomer")]
        public IActionResult GetOrderforCustomerID(string CustomerId,int OrderId)
        {
            if (!_northwindRepository.CustomerExists(CustomerId))
            {
                return NotFound();
            }

            var orderForCustomerFromRepo = this._northwindRepository.GetOrderForCustomer(CustomerId, OrderId);
            if (orderForCustomerFromRepo == null)
            {
                return NotFound();
            }

            var orderForcustomer = Mapper.Map<OrderDto>(orderForCustomerFromRepo);
            return Ok(orderForcustomer);
        }

        [HttpPost]
        public IActionResult CreateOrderForCustomer(string CustomerID,
            [FromBody]OrderForCreationDto order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnProcessableEntityObjectResult(ModelState);
            }

            if (!this._northwindRepository.CustomerExists(CustomerID))
            {
                return NotFound();
            }

            var orderEntity = Mapper.Map<Order>(order);
            this._northwindRepository.AddOrderForCustomer(CustomerID, orderEntity);

            if (!this._northwindRepository.Save())
            {
                throw new Exception($"Creation a Order for Customer {CustomerID} faild to Save.");
            }

            var orderToReturn = Mapper.Map<OrderDto>(orderEntity);

            return CreatedAtRoute("GetOrderToCustomer", new { CustomerId = CustomerID, OrderId = orderToReturn.OrderID }, orderToReturn);
        }

        [HttpDelete("{orderid}")]
        public IActionResult DeleteOrderForCustomer(string customerid, int orderid) {

            if (!_northwindRepository.CustomerExists(customerid))
            {
                return NotFound();
            }

            var orderForCustomerRepo = _northwindRepository.GetOrderForCustomer(customerid,orderid);
            if (orderForCustomerRepo == null)
            {
                return NotFound();
            }

            _northwindRepository.DeleteOrder(orderForCustomerRepo);

            if (!_northwindRepository.Save())
            {
                throw new Exception($"Delete Order {orderid} for customer {customerid} failed on save!");
            }

            return NoContent();
        }

        [HttpPut("{OrderID}")]
        public IActionResult UpdateOrder(string CustomerID,int OrderID,
            [FromBody]UpdateOrderDto order) {

            if (order == null)
            {
                return BadRequest();
            }

            if (!this._northwindRepository.CustomerExists(CustomerID))
            {
                return NotFound();
            }
            
            var orderForCustomerFromRepo = this._northwindRepository.GetOrderForCustomer(CustomerID, OrderID);
            if (orderForCustomerFromRepo == null)
            {
                var orderAdd = Mapper.Map<Order>(order);

                _northwindRepository.AddOrderForCustomer(CustomerID, orderAdd);

                if (!_northwindRepository.Save())
                {
                    throw new Exception($"Upserting order {OrderID} for customer {CustomerID} failed to saved!.");
                }

                var orderToReturn = Mapper.Map<OrderDto>(orderAdd);

                return CreatedAtRoute("GetOrderToCustomer",new { CustomerID = CustomerID, OrderID = OrderID },
                    orderToReturn);
            }
          
            Mapper.Map(order, orderForCustomerFromRepo);

            _northwindRepository.UpdateOrderForCustomer(orderForCustomerFromRepo);

            if (!_northwindRepository.Save())
            {
                throw new Exception($"Updating order {OrderID} for Customer {CustomerID} failed on saved!.");
            }

            return NoContent();

        }

        [HttpPatch("{OrderId}")]
        public IActionResult PartiallyUpdateOrderForCustomer(string customerid, int orderid,
            [FromBody]JsonPatchDocument<UpdateOrderDto>pactDoc)
        {
            if (pactDoc == null)
            {
                return BadRequest();
            }

            if (!_northwindRepository.CustomerExists(customerid))
            {
                return NotFound();
            }

            var orderforCustomerFromRepo = _northwindRepository.GetOrderForCustomer(customerid,orderid);

            if (orderforCustomerFromRepo == null)
            {
                var orderDto = new UpdateOrderDto();
                pactDoc.ApplyTo(orderDto);

                var orderToAdd = Mapper.Map<Order>(orderDto);

                _northwindRepository.AddOrderForCustomer(customerid, orderToAdd);

                if (!_northwindRepository.Save())
                {
                    throw new Exception($"Upserting order {orderid} for customer {customerid} failed to saved!.");
                }

                var orderToPatch = Mapper.Map<UpdateOrderDto>(orderforCustomerFromRepo);

                return CreatedAtRoute("GetOrderToCustomer",
                                     new { customerid = customerid, orderid = orderid },
                                     orderToPatch);

            }

            var ordertoPatch = Mapper.Map<UpdateOrderDto>(orderforCustomerFromRepo);
            pactDoc.ApplyTo(ordertoPatch);

            //add validation
            Mapper.Map(ordertoPatch, orderforCustomerFromRepo);

            _northwindRepository.UpdateOrderForCustomer(orderforCustomerFromRepo);

            if (!_northwindRepository.Save())
            {
                throw new Exception($"Patching order {orderid} for customer {customerid} failed to saved!.");
            }



            return NoContent();
        }

    }
}
