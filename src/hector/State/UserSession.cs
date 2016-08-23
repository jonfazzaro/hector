namespace Hector.State {
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class UserSession {
        public const string Key = "_hector";

        [DisplayName("URL")]
        [Required]
        public string Url { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}