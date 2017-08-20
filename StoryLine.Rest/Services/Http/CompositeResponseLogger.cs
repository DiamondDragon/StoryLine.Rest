using System;
using System.Collections.Generic;

namespace StoryLine.Rest.Services.Http
{
    public class CompositeResponseLogger : IResponseLogger
    {
        private readonly List<IResponseLogger> _loggers = new List<IResponseLogger>();

        public void Add(IResponseLogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _loggers.Add(logger);
        }

        public void Add(IResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            foreach (var logger in _loggers)
            {
                logger.Add(response);
            }
        }
    }
}