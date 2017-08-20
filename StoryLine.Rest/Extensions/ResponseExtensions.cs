﻿using System;
using StoryLine.Rest.Services.Http;

namespace StoryLine.Rest.Extensions
{
    public static class ResponseExtensions
    {
        public static string GetText(this IResponse response, bool failOnEmptyBody = true)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            return Config.ResponseToTextConverter.GetText(response, failOnEmptyBody);
        }
    }
}
