namespace ConsumidorPedidos.Model.Response
{
    /// <summary>
    /// Represents a link for HATEOAS (Hypermedia as the Engine of Application State).
    /// </summary>
    /// <param name="rel">The relationship type of the link (e.g., "self", "next", "prev").</param>
    /// <param name="href">The URL for the link.</param>
    /// <param name="method">The HTTP method used to access the link (e.g., "GET", "POST").</param>
    public class LinkInfo(string rel, string href, string method)
    {
        /// <summary>
        /// Gets or sets the relationship type of the link (e.g., "self", "next", "prev").
        /// </summary>
        public string Rel { get; set; } = rel;

        /// <summary>
        /// Gets or sets the URL for the link.
        /// </summary>
        public string Href { get; set; } = href;

        /// <summary>
        /// Gets or sets the HTTP method used to access the link (e.g., "GET", "POST").
        /// </summary>
        public string Method { get; set; } = method;
    }
}
