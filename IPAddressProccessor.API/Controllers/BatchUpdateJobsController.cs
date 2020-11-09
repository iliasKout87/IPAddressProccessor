using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IPAddressProccessor.API.Models;
using IPAddressProccessor.API.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace IPAddressProccessor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchUpdateJobsController : ControllerBase
    {
        private readonly IBatchUpdateJobsService batchUpdateJobsService;
        private readonly LinkGenerator linkGenerator;

        public BatchUpdateJobsController(
            IBatchUpdateJobsService batchUpdateJobsService,
            LinkGenerator linkGenerator
            )
        {
            this.batchUpdateJobsService = batchUpdateJobsService;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Guid>> Get(Guid id)
        {
            try
            {
                var result = await this.batchUpdateJobsService.Get(id);

                if(result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error getting batch update job. Inner exception: {ex}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Post(CreateBatchUpdateJobRequest request)
        {
            try
            {
                var result = await this.batchUpdateJobsService.Create(request);
                var location = this.linkGenerator.GetPathByAction("Get", "BatchUpdateJobs", new { id = result.Id });
                return Created(location, result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error creating batch update job. Inner exception: {ex}");
            }
        }


        [HttpGet("/{id:Guid}/progress")]
        public async Task<ActionResult<BatchUpdateJobProgressResponse>> GetProgress(Guid id)
        {
            try
            {
                var result = await this.batchUpdateJobsService.Get(id);

                if (result == null)
                {
                    return BadRequest($"No batch update job with id: {id} was found");
                }

                return Ok(new BatchUpdateJobProgressResponse() { UpdatesComplete = result.JobItems.Where(i => i.UpdateComplete == true).Count(), UpdatesTotal = result.JobItems.Count });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error getting progress batch update job. Inner exception: {ex}");
            }
        }

    }
}