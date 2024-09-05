namespace ConsumidorPedidos.Model.Response
{
    /// <summary>
    /// Represents an error response containing error code, message, and optional details about the error.
    /// </summary>
    /// <param name="code">The error code, typically corresponding to an HTTP status code.</param>
    /// <param name="message">A descriptive message explaining the error.</param>
    /// <param name="details">A list of additional details or context about the error.</param>
    public class ErrorResponse(
        int code)
    {
        /// <summary>
        /// Gets or sets the error code, typically corresponding to an HTTP status code.
        /// </summary>
        public int Code { get; set; } = code;

        /// <summary>
        /// Gets or sets a descriptive message explaining the error.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets a list of additional details or context about the error.
        /// </summary>
        public List<string>? Details { get; set; }
    }
}
