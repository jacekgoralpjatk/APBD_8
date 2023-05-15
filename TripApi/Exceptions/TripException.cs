using System.ComponentModel;
using System.Reflection;

namespace TripApi.Exceptions
{
    public class TripException : Exception
    {

        private readonly ExceptionCode _code;

        public TripException(ExceptionCode code) : base(code.GetDescription())
        {
            this._code = code;
        }

        public TripException(ExceptionCode code, string[] args) : base(string.Format(code.GetDescription(), args))
        {
            this._code = code;
        }

        public ExceptionCode Code() => _code;

    }

    public static class Extensions
    {
        public static string GetDescription(this Enum e)
        {
            var attribute =
                e.GetType()
                        .GetTypeInfo()
                        .GetMember(e.ToString())
                        .FirstOrDefault(member => member.MemberType == MemberTypes.Field)
                        ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .SingleOrDefault()
                    as DescriptionAttribute;

            return attribute?.Description ?? e.ToString();
        }
    }

    public enum ExceptionCode
    {
        [Description("Request body is not correct.")]
        REQUEST_BODY_NOT_VALID,

        [Description("Client is part of a trip and cannot be deleted.")]
        CLIENT_PART_OF_TRIP,

        [Description("Table {0} does not contain record with given id: {1}.")]
        TABLE_NOT_CONTAINS_BY_ID,

        [Description("Internal server error.")]
        DB_UPDATE_FAILED,

        [Description("Table {0} already contains {1}.")]
        TABLE_ALREADY_CONTAINS
    }
}
