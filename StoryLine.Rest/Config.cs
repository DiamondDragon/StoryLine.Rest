using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StoryLine.Rest.Expectations.Services;
using StoryLine.Rest.Expectations.Services.Json;
using StoryLine.Rest.Expectations.Services.Text;
using StoryLine.Rest.Services;
using StoryLine.Rest.Services.Http;
using StoryLine.Rest.Services.Resources;
using StoryLine.Rest.Services.Http.Decorators;

[assembly: InternalsVisibleTo("StoryLine.Rest.Tests")]

namespace StoryLine.Rest
{
    public static class Config
    {
        private static readonly CompositeResponseLogger ResponseLogger = new CompositeResponseLogger();

        private static IResponseToTextConverter _responseToTextConverter = new ResponseToTextConverter(
            new ContentTypeProvider());
        internal static readonly ServiceRegistry ServiceRegistry = new ServiceRegistry();
        private static readonly AssemblyProvider AssemblyProvider = new AssemblyProvider();

        private static readonly IStackTraceProvider StackTraceProvider = new StackTraceProvider();

        private static IResourceContentProvider _resourceContentProvider = new ResourceContentProvider(
            AssemblyProvider,
            StackTraceProvider,
            new MethodDetailsFilter(AssemblyProvider));

        private static readonly IRestClient RestClientInstance = new RestClient(
            new HttpClientFactory(
                ServiceRegistry),
            new RequestMessageFactory(),
            new ResponseFactory()
        );
        private static readonly ResponseAugmentingDecorator ResponseAugmentingDecorator = new ResponseAugmentingDecorator(RestClientInstance);
        private static readonly ResponseRecordingDecorator ResponseRecordingDecorator = new ResponseRecordingDecorator(ResponseAugmentingDecorator, ResponseLogger);

        private static Func<JsonVerifierSettings, ITextVerifier> _jsonVerifierFactory = x => new JsonVerifier(x);
        private static Func<PlainTextVerifierSettings, ITextVerifier> _plainTextVerifierFactory = x => new PlainTextVerifier(x);

        private static Encoding _defaultEncoding = Encoding.UTF8;
        private static JsonSerializerSettings _defaultJsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        private static string[] _defaultTestCaseAttributes =
        {
            "TestAttribute",
            "TestCaseAttribute",
            "FactAttribute",
            "TheoryAttribute"
        };

        internal static Func<JsonVerifierSettings, ITextVerifier> JsonVerifierFactory
        {
            get => _jsonVerifierFactory;
            set => _jsonVerifierFactory = value ?? throw new ArgumentNullException(nameof(value));
        }

        internal static Func<PlainTextVerifierSettings, ITextVerifier> PlainTextVerifierFactory
        {
            get => _plainTextVerifierFactory;
            set => _plainTextVerifierFactory = value ?? throw new ArgumentNullException(nameof(value));
        }

        internal static IResourceContentProvider ResourceContentProvider
        {
            get => _resourceContentProvider;
            set => _resourceContentProvider = value ?? throw new ArgumentNullException(nameof(value));
        }

        internal static IResponseToTextConverter ResponseToTextConverter
        {
            get => _responseToTextConverter;
            set => _responseToTextConverter = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static JsonSerializerSettings DefaultJsonSerializerSettings
        {
            get => _defaultJsonSerializerSettings;
            set => _defaultJsonSerializerSettings = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static Encoding DefaultEncoding
        {
            get => _defaultEncoding;
            set => _defaultEncoding = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static string[] DefaultTestCaseAttributes
        {
            get => _defaultTestCaseAttributes;
            set => _defaultTestCaseAttributes = value ?? throw new ArgumentNullException(nameof(value));
        }

        internal static IRestClient RestClient => ResponseRecordingDecorator;

        public static void SetAssemblies(params Assembly[] assemblies)
        {
            AssemblyProvider.Assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
        }

        public static void AddResponseLogger(IResponseLogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            ResponseLogger.Add(logger);
        }

        public static void AddResponseAugmenter(IResponseAugmenter augmenter)
        {
            if (augmenter == null)
                throw new ArgumentNullException(nameof(augmenter));

            ResponseAugmentingDecorator.AddResponseAugmenter(augmenter);
        }

        public static void AddServiceEndpont(string service, string baseUrl)
        {
            AddServiceEndpont(service, baseUrl, TimeSpan.FromSeconds(30));
        }

        public static void AddServiceEndpont(string service, string baseUrl, TimeSpan timeout)
        {
            AddServiceEndpont(service, baseUrl, timeout, false);
        }

        public static void AddServiceEndpont(string service, string baseUrl, TimeSpan timeout, bool allowRedirect)
        {
            if (string.IsNullOrWhiteSpace(service))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(service));
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(baseUrl));

            ServiceRegistry.Add(new ServiceConfig(service, baseUrl, timeout, allowRedirect));
        }
    }
}
