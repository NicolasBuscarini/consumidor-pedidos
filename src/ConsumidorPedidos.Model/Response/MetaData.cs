namespace ConsumidorPedidos.Model.Response
{
    /// <summary>
    /// Represents metadata for pagination, including information about the total items, items per page, current page, and total pages.
    /// </summary>
    /// <param name="totalItems">The total number of items available.</param>
    /// <param name="itemsPerPage">The number of items displayed per page.</param>
    /// <param name="currentPage">The current page number.</param>
    /// <param name="totalPages">The total number of pages available.</param>
    public class MetaData(int totalItems, int itemsPerPage, int currentPage, int totalPages)
    {
        /// <summary>
        /// Gets or sets the total number of items available.
        /// </summary>
        public int TotalItems { get; set; } = totalItems;

        /// <summary>
        /// Gets or sets the number of items displayed per page.
        /// </summary>
        public int ItemsPerPage { get; set; } = itemsPerPage;

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int CurrentPage { get; set; } = currentPage;

        /// <summary>
        /// Gets or sets the total number of pages available.
        /// </summary>
        public int TotalPages { get; set; } = totalPages;
    }
}
