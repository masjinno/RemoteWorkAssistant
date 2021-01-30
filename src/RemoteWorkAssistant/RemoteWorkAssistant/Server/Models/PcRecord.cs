using System.ComponentModel.DataAnnotations;

namespace RemoteWorkAssistant.Server.Models
{
    public class PcRecord
    {
        /// <summary>
        /// 形式的に用意した主キー。
        /// {MailAddress}-{PcName}
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// メールアドレス。
        /// ユーザー判別。
        /// </summary>
        public string MailAddress { get; set; }

        /// <summary>
        /// PC名称。
        /// ユーザー単位でユニーク。
        /// </summary>
        public string PcName { get; set; }

        /// <summary>
        /// IPアドレス情報。
        /// Windowsコマンド`ipconfig /all`で出力された値を保持する想定。
        /// 格納する際に該当コマンドで出力されたものか判断していない。
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// レコードが更新された日時。
        /// ISO 8601形式。
        /// 例：2014-10-10T13:50:40+09:00
        /// </summary>
        public string UpdatedDateTime { get; set; }
    }
}
