﻿using AngularCrudApi.Application.Helpers;
using AngularCrudApi.Application.Interfaces.Repositories;
using AngularCrudApi.Application.Pipeline.Commands;
using AngularCrudApi.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AngularCrudApi.Application.Pipeline.Handlers
{
    public class RequestCreateCommandHandler : IRequestHandler<RequestCreateCommand, Request>
        , IRequestHandler<RequestUpdateCommand, Request>
        , IRequestHandler<RequestDeleteCommand, Unit>
    {
        private readonly ICodebookRepository codebookRepository;
        private readonly ILogger<RequestCreateCommandHandler> log;

        public RequestCreateCommandHandler(ICodebookRepository codebookRepository, ILogger<RequestCreateCommandHandler> log)
        {
            this.codebookRepository = codebookRepository ?? throw new ArgumentNullException(nameof(codebookRepository));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Task<Request> Handle(RequestCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return this.codebookRepository.CreateRequest(request.Request);
            }
            catch (Exception exception)
            {
                this.log.LogError($"Error creating request {request.Request.ToLogString()}", exception);
                throw;
            }
        }

        public Task<Request> Handle(RequestUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return this.codebookRepository.UpdateRequest(request.Request);
            }
            catch (Exception exception)
            {
                this.log.LogError($"Error updating request {request.Request.ToLogString()}", exception);
                throw;
            }
        }

        public async Task<Unit> Handle(RequestDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await this.codebookRepository.DeleteRequest(request.Id);
                return Unit.Value;
            }
            catch (Exception exception)
            {
                this.log.LogError($"Error deleting request {request.Id}", exception);
                throw;
            }
        }
    }
}
