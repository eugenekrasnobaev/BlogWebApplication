namespace BlogBLL.DTO
{
    public class CreateCommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int? ParentId { get; set; }
    }
}
