namespace SIS.HTTP.Exceptions
{
    using System;
    public class InternalServerErrorException : Exception
    {
        private const string InternalServerErrorExceptionMessage = "The Server has encountered an error.";

        public InternalServerErrorException() : this(InternalServerErrorExceptionMessage)
        {

        }
        public InternalServerErrorException(string text) : base(text)
        {

        }
    }
}
