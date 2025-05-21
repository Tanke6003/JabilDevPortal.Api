// DTOs/Comment/CommentCreateDto.cs
namespace JabilDevPortal.Api.DTOs.Comment
{
    public class CommentCreateDto
    {
        public int    AuthorId { get; set; }  // opcional si lo extraes del token
        public string Comment  { get; set; } = null!;
    }
}
