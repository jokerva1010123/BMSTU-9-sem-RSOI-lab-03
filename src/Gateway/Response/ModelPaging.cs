namespace ModelDTO.Response
{
    public class PaginationResponse<T>
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalElements { get; set; }
        public T items { get; set; }
    }
}