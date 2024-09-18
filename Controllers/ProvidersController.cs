using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tech_Test_Backend.Models;
using Tech_Test_Backend.Services;

namespace Tech_Test_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvidersController : ControllerBase
    {
        private readonly IProvidersService _providerService;

        public ProvidersController(IProvidersService providerService)
        {
            _providerService = providerService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var providers = await _providerService.GetAllProvidersAsync(); 
            return Ok(providers);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(Guid id)
        {
            var provider = await _providerService.GetProviderByIdAsync(id);
            return Ok(provider);
        }

        [Authorize]
        [HttpPost]
        public async Task CreateAsync([FromBody] Provider provider)
        {
            try
            {
                await _providerService.CreateProviderAsync(provider);
            }
            catch
            {
                // Handle exception as necessary throw statement is TODO be refactored
                throw;
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _providerService.DeleteProviderAsync(id);
            }
            catch
            {
                // Handle exception TODO
                throw;
            }
        }

        [Authorize]
        [HttpPut]
        public async Task UpdateAsync([FromBody] Provider provider)
        {
            try
            {
                await _providerService.UpdateProviderAsync(provider.Id, provider);
            }
            catch
            {
                throw;
            }
        }
    }
}
