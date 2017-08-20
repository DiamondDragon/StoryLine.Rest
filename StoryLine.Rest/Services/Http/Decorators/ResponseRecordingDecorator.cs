using System;

namespace StoryLine.Rest.Services.Http.Decorators
{
    public class ResponseRecordingDecorator : IRestClient
    {
        private readonly IRestClient _innerClient;
        private readonly IResponseLogger _responseLogger;
        private readonly Func<bool> _responseRecordingEnabled;

        public ResponseRecordingDecorator(
            IRestClient innerClient,
            IResponseLogger responseLogger,
            Func<bool> responseRecordingEnabled)
        {
            _innerClient = innerClient ?? throw new ArgumentNullException(nameof(innerClient));
            _responseLogger = responseLogger ?? throw new ArgumentNullException(nameof(responseLogger));
            _responseRecordingEnabled = responseRecordingEnabled ?? throw new ArgumentNullException(nameof(responseRecordingEnabled));
        }

        public IResponse Send(IRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var response = _innerClient.Send(request);

            if (_responseRecordingEnabled())
                _responseLogger.Add(response);

            return response;
        }
    }
}
