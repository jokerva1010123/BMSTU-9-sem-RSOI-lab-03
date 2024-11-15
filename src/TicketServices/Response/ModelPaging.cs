namespace ModelDTO.Response
{
    public class PaginationResponse<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalElements { get; set; }
        public T Items { get; set; }
    }
}