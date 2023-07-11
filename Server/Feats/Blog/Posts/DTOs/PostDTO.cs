namespace Server.Feats.Blog.Posts.DTOs;

public class PostDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Excerpt { get; set; }

    public string Thunbnailimage { get; set; }

    public string Content { get; set; }

    public string PublishDate { get; set; }

    public bool Published { get; set; }

    public string Author { get; set; }

    public Guid CategoryId { get; set; }
}
