namespace EArenaTournamentManager.Web.Models.Shared
{
    public class PagedData<T>
    {
        public List<T> Items { get; set; } = new();
        public PagerResponse Pager { get; set; } = new();
    }
}
