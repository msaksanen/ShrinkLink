using ShrinkLinkDb.Entities;

namespace ShrinkLinkApp.Helpers
{
    public class MD5
    {
        private readonly IConfiguration _configuration;
        public MD5(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        internal string CreateMd5(string? text, int flag=0)
        {
            return Md5(text, flag);
        }
        private string Md5(string? text, int flag = 0)
        {
            if (string.IsNullOrWhiteSpace(text))
                text = string.Empty;
            if (flag == 1) 
                text += _configuration["UserSecrets:PasswordSalt"];

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.UTF8.GetBytes(text);
                var hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }
    }
}
