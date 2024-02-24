namespace DotnetAPI.Dtos
{
    public partial class UserForLoginConfirmationDto
    {
        public byte[] PassworkdHash {get;set;} = new byte[0];
        public byte[] PasswordSalt {get;set;} = new byte[0];
    }

}