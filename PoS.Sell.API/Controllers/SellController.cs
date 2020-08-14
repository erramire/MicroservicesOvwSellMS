using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoS.Sell.API.Application.Domain;
using PoS.Sell.API.Application.DTOs;
using PoS.CC.Utilities;

namespace PoS.Sell.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class SellController : ControllerBase
    {
        private readonly ISellBusiness _sellBusiness;

        public SellController(ISellBusiness sellBusiness) {
            _sellBusiness = sellBusiness;
        }

        /// <summary>
        /// Do the checkout of a sell 
        /// </summary>
        /// <param name="checkOutSellDTO"></param>
        /// <returns></returns>
        [HttpPost("CheckOut/{correlationToken}", Name ="CheckOut")]
        public async Task<IActionResult> PostCheckOut([FromBody] CheckOutSellDTO checkOutSellDTO, string correlationToken)
        {
            Guard.ForNullObject(checkOutSellDTO, "CheckOut Sell");
            Guard.ForNullOrEmpty(checkOutSellDTO.Folio_Venta, "Folio Venta in New Sell");            

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var checkoutResult = await _sellBusiness.CheckOutSellAsync(checkOutSellDTO.Folio_Venta,correlationToken);

            
            return new ObjectResult("CheckOut of the Sell successful!");            
        }
        


        /// <summary>
        /// Adds a new item to the sell record, by its sku
        /// </summary>
        /// <param name="addSellItemDTO"></param>
        /// <returns></returns>
        [HttpPost("AddSellItem/{correlationToken}",Name = "AddSellItem")]
        public async Task<IActionResult> AddSellItem([FromBody] AddSellItemDTO addSellItemDTO, string correlationToken)
        {
            var num = 8;
            Guard.ForNullObject(addSellItemDTO, "Adding Sell Item");
            Guard.ForNullOrEmpty(addSellItemDTO.Folio_Venta, "Folio Venta in New item to Sell");
            Guard.ForNullOrEmpty(addSellItemDTO.Sku, "sku in New item to Sell");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var addItemResult = await _sellBusiness.AddSellItemAsync(addSellItemDTO.Folio_Venta, addSellItemDTO.Sku,correlationToken);


            return new ObjectResult("New Item added to the Sell!");
        }

        /// <summary>
        /// Create a new sell record
        /// </summary>
        /// <param name="createSellDTO"></param>
        /// <returns></returns>
        [HttpPost("CreateSell/{correlationToken}",Name = "CreateSell")]
        public async Task<IActionResult> CreateSell([FromBody] CreateSellDTO createSellDTO, string correlationToken)
        {
            Guard.ForNullObject(createSellDTO, "Creating a new Sell");
            Guard.ForNullOrEmpty(createSellDTO.CashdeskId, "cashdesk Id in New Sell");
            Guard.ForNullOrEmpty(createSellDTO.StoreId, "Store Id  in New Sell");
            Guard.ForNullOrEmpty(createSellDTO.CashierId, "Cashier Id  in New Sell");
            

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var addItemResult = await _sellBusiness.CreateSellAsync(createSellDTO.StoreId, createSellDTO.CashdeskId, createSellDTO.CashierId,correlationToken);


            return new ObjectResult("New sell created");
        }

    }
}
