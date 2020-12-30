using System.ComponentModel.DataAnnotations;

namespace RemoteWorkAssistant.Server.Models
{
    public class UserRecord
    {
        /// <summary>
        /// メールアドレス。主キー。
        /// ユーザー判別。
        /// </summary>
        [Key]
        public string MailAddress { get; set; }

        /// <summary>
        /// パスワード
        /// </summary>
        public string Password { get; set; }
    }
}
