namespace Clean.Architecture.Web.Endpoints.ToDoItems
{
    public class ToDoItemResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
    }
}