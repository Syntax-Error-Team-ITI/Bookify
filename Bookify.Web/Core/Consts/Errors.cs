namespace Bookify.Web.Core.Consts
{
    public class Errors
    {
        public const string MaxLength = "Length can not be more than {1} characters!";
        public const string Dublicated = "{0} with the same name is already exists!";
        public const string DublicatedBook = "Book with the same title is already exists with the same autor!";
        public const string NotAllowedExtension= "Only .png, .jpg, .jpeg files are allowed!";
        public const string MaxSize= "File can not be more than 2 MB!";
        public const string NotAllowedFutureDate= "Date cannot be in the future!";
        public const string InvalidNationalId = "Invalid national ID!";
        public const string EmptyImage = "Please select an image!";
        public const string InvalidMobileNumber = "Invalid mobile number.";
        public const string DenySpecialCharacters = "Special characters are not allowed.";
        public const string Duplicated = "Another record with the same {0} is already exists!";
    }
}
