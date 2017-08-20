using System;
using System.Text;
using StoryLine.Exceptions;
using StoryLine.Rest.Services.Http;

namespace StoryLine.Rest.Services
{
    public class ResponseToTextConverter : IResponseToTextConverter
    {
        private readonly IContentTypeProvider _contentTypeProvider;

        public ResponseToTextConverter(IContentTypeProvider contentTypeProvider)
        {
            _contentTypeProvider = contentTypeProvider ?? throw new ArgumentNullException(nameof(contentTypeProvider));
        }

        public string GetText(IResponse response, bool failOnEmptyBody = true)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (failOnEmptyBody)
                ValidateRequest(response);

            if (response.Body == null)
                return string.Empty;

            if (response.Body.Length == 0)
                return string.Empty;

            var charSetName = _contentTypeProvider.GetCharSet(response.Headers);
            var encoding = string.IsNullOrEmpty(charSetName) ?
                Config.DefaultEncoding :
                GetEncoding(charSetName);

            try
            {
                return encoding.GetString(response.Body);
            }
            catch (Exception ex)
            {
                throw new ExpectationException("Failed to convert response into text.", ex);
            }
        }

        private static void ValidateRequest(IResponse response)
        {
            if (response.Body == null || response.Body.Length == 0)
                throw new ExpectationException("Empty body can't be used to extract text. Request details: " + GetResponseDetails(response));
        }

        private static string GetResponseDetails(IResponse response)
        {
            var builder = new StringBuilder();

            builder.Append("Service=" + response.Request.Service + ", ");
            builder.Append("Method=" + response.Request.Method + ", ");
            builder.Append("Url=" + response.Request.Url + ", ");
            builder.Append("Status=" + response.Status);

            return builder.ToString();
        }

        private static Encoding GetEncoding(string charSetName)
        {
            try
            {
                return Encoding.GetEncoding(charSetName);
            }
            catch (ArgumentException ex)
            {
                throw new ExpectationException("Unknown charset specified in response: " + charSetName, ex);
            }
        }
    }
}