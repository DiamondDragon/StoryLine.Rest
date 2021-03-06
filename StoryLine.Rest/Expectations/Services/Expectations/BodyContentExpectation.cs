using System;
using StoryLine.Exceptions;
using StoryLine.Rest.Services.Http;

namespace StoryLine.Rest.Expectations.Services.Expectations
{
    public class BodyContentExpectation : IResponseExpectation
    {
        private readonly string _expectedResponseText;
        private readonly ITextVerifier _textVerifier;

        public BodyContentExpectation(string body, ITextVerifier textVerifier)
        {
            _expectedResponseText = body ?? throw new ArgumentNullException(nameof(body));
            _textVerifier = textVerifier ?? throw new ArgumentNullException(nameof(textVerifier));
        }

        public void Validate(IResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            var actualResponseText = Config.ResponseToTextConverter.GetText(response);

            if (actualResponseText == null)
                throw new ExpectationException("Response doesn't contain any text");

            _textVerifier.Verify(_expectedResponseText, actualResponseText);
        }
    }
}