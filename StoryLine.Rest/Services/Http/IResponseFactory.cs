using System;
using System.Net.Http;

namespace StoryLine.Rest.Services.Http
{
    public interface IResponseFactory
    {
        IResponse Create(IRequest request, HttpResponseMessage result);
        IResponse CreateExceptionResponse(IRequest request, Exception exception);
    }
}