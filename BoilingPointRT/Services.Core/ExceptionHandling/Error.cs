using System;

using BoilingPointRT.Services.Domain;

namespace BoilingPointRT.Services.ExceptionHandling
{
    [Serializable]
    public class Error : Descriptor
    {
        public static Error Internal = new Error("InternalError");
        public static Error AttemptToRetrieveRecordWithEmptyKey = new Error("AttemptToRetrieveRecordWithEmptyKey");
        public static Error RecordDoesNotExistAnymore = new Error("RecordDoesNotExistAnymore");
        public static Error RecordIsChangedByAnotherUser = new Error("RecordIsChangedByAnotherUser");
        public static Error AspNetRequestValidationFailed = new Error("AspNetRequestValidationFailed");
        public static Error CertificateReportProfileNotFound = new Error("CertificateReportProfileNotFound");
        public static Error DataExceedsMaximumLength = new Error("DataExceedsMaximumLength");
        public static Error RecordWithDuplicateUniqueKeyExists = new Error("RecordWithDuplicateUniqueKeyExists");
        public static Error RecordIsUsedByAnotherRecord = new Error("RecordIsUsedByAnotherRecord");
        public static Error ConnectionToDatabaseFailed = new Error("ConnectionToDatabaseFailed");
        public static Error DatabaseTableNotFound = new Error("DatabaseTableNotFound");

        protected Error(string code)
            : base(code)
        {
        }
    }
}