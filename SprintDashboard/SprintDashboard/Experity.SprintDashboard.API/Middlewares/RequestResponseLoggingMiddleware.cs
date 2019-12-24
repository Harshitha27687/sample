using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Newtonsoft.Json;

namespace Experity.SprintDashboard.API.Middlewares
{
    // https://dev.to/mckabue/an-aspnet-core-request---response-logger-middleware-clb
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        private const int ReadChunkBufferLength = 4096;


        private readonly ILogger _logger;
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }


        public async Task Invoke(HttpContext context)
        {
            var model = new RequestProfilerModel
            {
                RequestTime = new DateTimeOffset(),
                Context = context,
                Request = await FormatRequest(context)
            };

            var originalBody = context.Response.Body;

            using (var newResponseBody = _recyclableMemoryStreamManager.GetStream())
            {
                context.Response.Body = newResponseBody;

                await _next(context);

                newResponseBody.Seek(0, SeekOrigin.Begin);
                await newResponseBody.CopyToAsync(originalBody);

                newResponseBody.Seek(0, SeekOrigin.Begin);
                model.Response = FormatResponse(context, newResponseBody);
                model.ResponseTime = new DateTimeOffset();
                LogEvent(model);
            }
        }

        private ResponseModel FormatResponse(HttpContext context, MemoryStream newResponseBody)
        {
            var request = context.Request;
            var response = context.Response;
            var responseModel = new ResponseModel
            {
                Schema = request.Scheme,
                Host = request.Host.ToString(),
                Path = request.Path,
                QueryString = request.QueryString.ToString(),
                StatusCode = response.StatusCode.ToString(),
                ResponseBody = ReadStreamInChunks(newResponseBody)
            };
            return responseModel;
        }

        private async Task<RequestModel> FormatRequest(HttpContext context)
        {
            var request = context.Request;
            var requestModel = new RequestModel
            {
                Schema = request.Scheme,
                Host = request.Host.ToString(),
                Path = request.Path,
                QueryString = request.QueryString.ToString(),
                RequestBody = await GetRequestBody(request)
            };
            return requestModel;
        }

        public async Task<string> GetRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            request.EnableRewind();
            using (var requestStream = _recyclableMemoryStreamManager.GetStream())
            {
                await request.Body.CopyToAsync(requestStream);
                request.Body.Seek(0, SeekOrigin.Begin);
                return ReadStreamInChunks(requestStream);
            }
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            string result;
            using (var textWriter = new StringWriter())
            using (var reader = new StreamReader(stream))
            {
                var readChunk = new char[ReadChunkBufferLength];
                int readChunkLength;
                //do while: is useful for the last iteration in case readChunkLength < chunkLength
                do
                {
                    readChunkLength = reader.ReadBlock(readChunk, 0, ReadChunkBufferLength);
                    textWriter.Write(readChunk, 0, readChunkLength);
                } while (readChunkLength > 0);

                result = textWriter.ToString();
            }

            return result;
        }

        public class RequestProfilerModel
        {
            public DateTimeOffset RequestTime { get; set; }
            public HttpContext Context { get; set; }
            public DateTimeOffset ResponseTime { get; set; }
            public RequestModel Request { get; set; }
            public ResponseModel Response { get; set; }
        }

        public class RequestModel
        {
            public string Schema { get; set; }
            public string Host { get; set; }
            public string Path { get; set; }
            public string QueryString { get; set; }
            public string RequestBody { get; set; }
        }


        public class ResponseModel
        {
            public string Schema { get; set; }
            public string Host { get; set; }
            public string Path { get; set; }
            public string QueryString { get; set; }
            public string StatusCode { get; set; }
            public string ResponseBody { get; set; }
        }

        private void LogEvent(RequestProfilerModel requestProfilerModel)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(requestProfilerModel.Request));
            _logger.LogInformation(JsonConvert.SerializeObject(requestProfilerModel.Response));
        }

    }
}
