using System;

namespace StoryLine.Rest.Services.Http.Decorators
{
    public class ResponseRecordingDecorator : IRestClient
    {
        private readonly IRestClient _innerClient;
        private readonly IResponseLogger _responseLogger;

        public ResponseRecordingDecorator(
            IRestClient innerClient,
            IResponseLogger responseLogger)
        {
            _innerClient = innerClient ?? throw new ArgumentNullException(nameof(innerClient));
            _responseLogger = responseLogger ?? throw new ArgumentNullException(nameof(responseLogger));
        }

        public IResponse Send(IRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var response = _innerClient.Send(request);

            _responseLogger.Add(response);

            return response;
        }
    }
}
