using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TaskManagementSystem.Helpers
{
    public class DocumentUploadOperation : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.ApiDescription.HttpMethod.Equals("POST") && context.ApiDescription.RelativePath.Contains("upload"))
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties =
                            {
                                ["document"] = new OpenApiSchema
                                {
                                    Description = "Select file",
                                    Type = "file",
                                    Format = "binary"
                                },
                                ["requestBody"] = new OpenApiSchema
                                {
                                    Description = "Request body",
                                    Type = "string",
                                    Example = new OpenApiString("{\"NoteId\": 0,\"RequesterId\": 0}")
                                }
                            },
                                Required = new HashSet<string> { "document" , "requestBody" }
                            }
                        }
                    }
                };
            }
        }
    }
}
