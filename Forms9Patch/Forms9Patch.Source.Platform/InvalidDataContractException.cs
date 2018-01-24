#if WINDOWS_UWP
#else
namespace System.Runtime.Serialization
{
    public class InvalidDataContractException : Exception
    {
        public InvalidDataContractException() : base() { }

        public InvalidDataContractException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public InvalidDataContractException(string msg) : base(msg) { }

        public InvalidDataContractException(string msg, Exception innterException) : base(msg, innterException) { }
    }
}
#endif
