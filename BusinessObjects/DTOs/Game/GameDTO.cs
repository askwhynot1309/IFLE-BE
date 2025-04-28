using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs.Game
{
    public class CreateGameRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập tên trò chơi.")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả trò chơi.")]
        [MaxLength(500)]
        public string Description { get; set; }

        public string? VideoUrl { get; set; }

        [Required(ErrorMessage = "Vui lòng điền link tải trò chơi.")]
        public string DownloadUrl { get; set; }

        public string? ImageUrl { get; set; }

        //[Required]
        //public string Status { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục cho trò chơi.")]
        public List<string> CategoryIds { get; set; } = new();

        public CreateGameVersionRequest? Version { get; set; }
    }

    public class CreateGameVersionRequest
    {
        [Required(ErrorMessage = "Vui lòng chọn phiên bản cho trò chơi.")]
        [MaxLength(50)]
        public string Version { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày phát hành phiên bản.")]
        public DateTime ReleaseDate { get; set; }
    }

    public class UpdateGameRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập tên ")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả trò chơi.")]
        [MaxLength(500)]
        public string Description { get; set; }

        public string? VideoUrl { get; set; }

        [Required(ErrorMessage = "Vui lòng điền link tải trò chơi.")]
        public string DownloadUrl { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn status cho trò chơi.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục cho trò chơi.")]
        public List<string> CategoryIds { get; set; } = new();
    }

    public class AddGameVersionRequest
    {
        [Required(ErrorMessage = "Vui lòng chọn phiên bản cho trò chơi.")]
        [MaxLength(50)]
        public string Version { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày phát hành phiên bản.")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Vui lòng điền link tải trò chơi.")]
        public string DownloadUrl { get; set; }
    }

    public class GameVersionResponse
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Description { get; set; }
    }

    public class GameCategoryInGameResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class GameResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? VideoUrl { get; set; }
        public int PlayCount { get; set; }
        public string DownloadUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string Status { get; set; }
        public List<GameCategoryInGameResponse> Categories { get; set; } = new();
        public List<GameVersionResponse> Versions { get; set; } = new();
    }
}
