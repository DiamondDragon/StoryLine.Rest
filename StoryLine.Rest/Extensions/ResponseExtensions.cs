using System;
using System.Text;
using StoryLine.Exceptions;
using StoryLine.Rest.Services.Http;

namespace StoryLine.Rest.Extensions
{
    public static class ResponseExtensions
    {
        public static string GetText(this IResponse response, bool failOnEmptyBody = true, bool failOnNonSuccessResponse = true)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (failOnEmptyBody && (response.Body == null || response.Body.Length == 0))
                throw new ExpectationException("Response has empty body. Details: " + GetRequestDetails(response));

            if (failOnNonSuccessResponse && (response.Status < 200 || response.Status >= 300))
                throw new ExpectationException("Response status is not successful. Details: " + GetRequestDetails(response));

            return Config.ResponseToTextConverter.GetText(response);
        }

        private static string GetRequestDetails(IResponse response)
        {
            var builder = new StringBuilder();

            builder.Append("Service=" + response.Request.Service + ", ");
            builder.Append("Method=" + response.Request.Method + ", ");
            builder.Append("Url=" + response.Request.Url + ", ");
            builder.Append("Status=" + response.Status);

            return builder.ToString();
        }
    }
}
