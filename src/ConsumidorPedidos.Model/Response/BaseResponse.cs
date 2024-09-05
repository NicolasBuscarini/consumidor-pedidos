namespace ConsumidorPedidos.Model.Response
{
    /// <summary>
    /// Represents a base response for API endpoints, which includes data, links for HATEOAS, metadata for pagination, and error information.
    /// </summary>
    /// <typeparam name="T">The type of the data being returned.</typeparam>
    public class BaseResponse<T>
    {
        /// <summary>
        /// Gets or sets the data being returned in the response.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Gets or sets a list of links for HATEOAS (Hypermedia as the Engine of Application State).
        /// </summary>
        public List<LinkInfo> Links { get; set; }

        /// <summary>
        /// Gets or sets the metadata for pagination, such as total items, items per page, current page, and total pages.
        /// </summary>
        public MetaData? Meta { get; set; }

        /// <summary>
        /// Gets or sets the error information, if the response contains an error.
        /// </summary>
        public ErrorResponse? Error { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponse{T}"/> class.
        /// </summary>
        public BaseResponse() => Links = [];
    }
}
