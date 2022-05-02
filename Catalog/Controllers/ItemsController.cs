using Microsoft.AspNetCore.Mvc;
using Catalog.Repositories;
using System.Collections.Generic;
using Catalog.Entities;
using System;
using System.Linq;
using Catalog.Dtos;

namespace Catalog.Controllers 
{
   [ApiController]
   [Route("items")]
   public class ItemController: ControllerBase
   {
       private readonly IItemsRepository repository;
       public ItemController(IItemsRepository repository )
       {
          this.repository = repository;
       }

       [HttpGet]
       public IEnumerable<ItemDto> GetItems()
       {
           var items= repository.GetItems().Select(item => item.AsDto());
           return items;
       }
        [HttpGet("{id}")]
       public ActionResult<ItemDto> GetItem(Guid id) 
       {
            var item = repository.GetItem(id);
            if(item is null)
            {
                return NotFound();
            }
            return item.AsDto();
       }

        [HttpPost]
       public ActionResult<ItemDto> CreateItem(CreateItemDto itemDTo)
       {
            Item item= new()
            { 
               Id=Guid.NewGuid(),
               Name=itemDTo.Name,
               Price=itemDTo.Price,
               CreatedDate=DateTimeOffset.UtcNow

            };
            repository.CreateItem(item);
            return CreatedAtAction(nameof(GetItem),new {id=item.Id},item.AsDto());

       }
       [HttpPut("{id}")]
       public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
       {
           var existingItem= repository.GetItem(id);
           if(existingItem is null)
           {
               return NotFound();
           }
         Item updatedItem=existingItem with
         {
             Name=itemDto.Name,
             Price=itemDto.Price
         };
         repository.UpdateItem(updatedItem);
         return NoContent();


       }
       [HttpDelete("{id}")]
       public ActionResult DeleteItem(Guid id)
       {
           var existingItem= repository.GetItem(id);
           if(existingItem is null)
           {
               return NotFound();
           }
          return NoContent();
       }

   }

}