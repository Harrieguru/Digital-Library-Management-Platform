namespace eBook_manager.Models
{
    public class EbookDTO
    {
        public string ISBN {get; set;}
        public List<string> Authors { get; set;}
        public string Genre { get; set;}

        public string Summary { get; set;}

        public string Title { get; set;}
    }
}
