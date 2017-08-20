using System;
using System.Collections.Generic;

namespace StoryLine.Rest.Services.Http.Decorators
{
    public class ResponseAugmentingDecorator : IRestClient
    {
        private readonly IRestClient _innerClient;
        private readonly List<IResponseAugmenter> _augmenters = new List<IResponseAugmenter>();

        public ResponseAugmentingDecorator(IRestClient innerClinet)
        {
            _innerClient = innerClinet ?? throw new ArgumentNullException(nameof(innerClinet));
        }

        public void AddResponseAugmenter(IResponseAugmenter augmenter)
        {
            if (augmenter == null)
                throw new ArgumentNullException(nameof(augmenter));

            _augmenters.Add(augmenter);
        }

        public IResponse Send(IRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            foreach (var augmenter in _augmenters)
            {
                augmenter.BeforeRequestExecuted(request);
            }

            var response = _innerClient.Send(request);

            foreach (var augmenter in _augmenters)
            {
                augmenter.AfterResponseReceived(response);
            }

            return response;
        }
    }
}
