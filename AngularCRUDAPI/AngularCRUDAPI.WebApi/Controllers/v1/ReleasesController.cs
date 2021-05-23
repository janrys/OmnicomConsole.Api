﻿using AngularCrudApi.Application.Exceptions;
using AngularCrudApi.Domain.Entities;
using AngularCrudApi.WebApi.Extensions;
using AngularCrudApi.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCrudApi.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ReleasesController : BaseApiController
    {
        private readonly ILogger<ReleasesController> log;

        public ReleasesController(IMediator mediator, ILogger<ReleasesController> log) : base(mediator)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        /// <summary>
        /// Get list of releases
        /// </summary>
        /// <returns>Release list</returns>
        [ProducesResponseType(typeof(IEnumerable<Release>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<Release> releases = await this.Query().Release.All();
                return this.Ok(releases);
            }
            catch (Exception exception)
            {
                string errorMessage = "Error loading releases";
                this.log.LogError(errorMessage, exception);
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Get list of releases
        /// </summary>
        /// <returns>Release list</returns>
        [ProducesResponseType(typeof(IEnumerable<Request>), StatusCodes.Status200OK)]
        [HttpGet("{releaseId:int}/requests")]
        public async Task<IActionResult> GetRequests(int releaseId)
        {
            try
            {
                IEnumerable<Request> requests = await this.Query().Release.Requests(releaseId);
                return this.Ok(requests);
            }
            catch (Exception exception)
            {
                string errorMessage = "Error loading requests";
                this.log.LogError(errorMessage, exception);
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Create new release
        /// </summary>
        /// <returns>Release list</returns>
        [ProducesResponseType(typeof(Release), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> CreateRelease(Release release)
        {
            if (release == null)
            {
                return this.BadRequest($"Parameter {nameof(release)} is mandatory.");
            }

            try
            {
                Release createdRelease = await this.Command().Release.Create(release);
                return this.Ok(createdRelease);
            }
            catch (Exception exception)
            {
                string errorMessage = "Error creating release";
                this.log.LogError(errorMessage, exception);
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Create new request
        /// </summary>
        /// <returns>Release list</returns>
        [ProducesResponseType(typeof(Request), StatusCodes.Status200OK)]
        [HttpPost("requests")]
        public async Task<IActionResult> CreateRequest(RequestModel request)
        {
            if (request == null)
            {
                return this.BadRequest($"Parameter {nameof(request)} is mandatory.");
            }

            try
            {
                Request createdRequest = await this.Command().Release.CreateRequest((Request)request);
                return this.Ok(createdRequest);
            }
            catch (Exception exception)
            {
                string errorMessage = "Error creating request";
                this.log.LogError(errorMessage, exception);
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Create new request
        /// </summary>
        /// <returns>Created request</returns>
        [ProducesResponseType(typeof(Request), StatusCodes.Status200OK)]
        [HttpPost("{releaseId:int}/requests")]
        public async Task<IActionResult> CreateRequestWithReleaseId(RequestModel request, int releaseId)
        {
            if (request == null)
            {
                return this.BadRequest($"Parameter {nameof(request)} is mandatory.");
            }

            request.ReleaseId = releaseId;
            return await this.CreateRequest(request);
        }

        /// <summary>
        /// Update request
        /// </summary>
        /// <returns>Updated request</returns>
        [ProducesResponseType(typeof(Request), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("requests")]
        public async Task<IActionResult> UpdateRequest(RequestModel request)
        {
            if (request == null)
            {
                return this.BadRequest($"Parameter {nameof(request)} is mandatory.");
            }

            Request updatedRequest = await this.Query().Release.RequestById(request.Id);

            if (updatedRequest == null)
            {
                return this.NotFound();
            }

            try
            {
                updatedRequest = await this.Command().Release.UpdateRequest((Request)request);
                return this.Ok(updatedRequest);
            }
            catch (Exception exception)
            {
                string errorMessage = "Error updating request";
                this.log.LogError(errorMessage, exception);
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Update request
        /// </summary>
        /// <returns>Updated request</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("requests/{requestId:int}")]
        public async Task<IActionResult> DeleteRequest(int requestId)
        {
            Request updatedRequest = await this.Query().Release.RequestById(requestId);

            if (updatedRequest == null)
            {
                return this.NotFound();
            }

            try
            {
                await this.Command().Release.DeleteRequest(requestId);
                return this.Ok();
            }
            catch (Exception exception)
            {
                string errorMessage = "Error deleting request";
                this.log.LogError(errorMessage, exception);
                throw new Exception(errorMessage);
            }
        }
    }
}
