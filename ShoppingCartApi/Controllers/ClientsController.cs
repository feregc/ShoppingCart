using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Repositories;
using ShoppingCartApi.Services;

namespace ShoppingCartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        protected ResponseDto _response;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
            _response = new ResponseDto();
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
        {
            try
            {
                var list = await _clientService.GetAllClientsAsync();
                _response.Result = list;
                _response.DisplayMessage = "Clients List";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetClient(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);

            if (client == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Client does not exist";
                return NotFound(_response);
            }
            _response.Result = client;
            _response.DisplayMessage = "Client Information";
            return Ok(_response);
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, ClientDto clientDto)
        {
            try
            {
                var model = await _clientService.CreateUpdateClientAsync(clientDto);
                _response.Result = model;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error ";
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }

        // POST: api/Clients
        [HttpPost]
        public async Task<ActionResult<ClientDto>> PostClient(ClientDto clientDto)
        {
            try
            {
                var model = await _clientService.CreateUpdateClientAsync(clientDto);
                _response.Result = model;

                return CreatedAtAction("GetClient", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error saving register";
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                var isDeleted = await _clientService.DeleteClientAsync(id);
                if (isDeleted)
                {
                    _response.Result = isDeleted;
                    _response.DisplayMessage = "Client was deleted successfully";
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "Error deleting client";
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }
    }
}
