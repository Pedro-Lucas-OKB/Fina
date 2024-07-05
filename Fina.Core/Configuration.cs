using System.Net;

namespace Fina.Core;

public class Configuration
{
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 30;
    public const int DefaultStatusCode = (int)HttpStatusCode.OK;

    public static string BackendUrl { get; set; } = string.Empty;
    public static string FrontendUrl { get; set; } = string.Empty;
}
