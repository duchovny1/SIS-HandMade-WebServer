namespace SIS.HTTP.Exceptions
{
    using System;
    public class BadRequestException : Exception
    {
        private const string BadRequestMessageDefaultMessage = "The Request was malformed or contains unsupported elements.";

        public BadRequestException() : this(BadRequestMessageDefaultMessage)
        {

        }
        public BadRequestException(string text) :base(text)
        {

        }
    }
}
