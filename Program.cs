using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

var app = WebApplication.Create();

app.Map("/{**path}", async (HttpContext context) =>
{
    var headers = context.Request.Headers;
    var method = context.Request.Method;
    object body = null;

    if (HttpMethods.IsPost(method) || HttpMethods.IsPut(method))
    {
        if (context.Request.ContentType == MediaTypeNames.Application.Json)
        {
            body = await context.Request.ReadFromJsonAsync<object>();
        }
        else
        {
            // If not JSON, read the raw body as text
            using var reader = new StreamReader(context.Request.Body);
            body = await reader.ReadToEndAsync();
        }
    }

    // Capture request data
    var requestInfo = new
    {
        Headers = headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
        Method = method,
        Body = body,
        Url = context.Request.GetDisplayUrl(), // Get full URL including host and query string
        TimeUTC = DateTime.UtcNow.ToString("o")
    };

    // Serialize to JSON
    var jsonResponse = JsonSerializer.Serialize(requestInfo, new JsonSerializerOptions { WriteIndented = true });

    // Determine response type (HTML or JSON) based on Accept header
    if (context.Request.Headers[HeaderNames.Accept].ToString().Contains(MediaTypeNames.Text.Html))
    {
        var htmlResponse = $@"
            <html>
                <body>
                    <h1>Request Info</h1>
                    <pre>{jsonResponse}</pre>
                </body>
            </html>";
        context.Response.ContentType = MediaTypeNames.Text.Html;
        await context.Response.WriteAsync(htmlResponse);
    }
    else
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(jsonResponse);
    }
});

await app.RunAsync();
