namespace Domain.Services.Telemetry
{
    public static class CounterNames
    {
        public static class Mediator
        {
            /// <summary>
            /// How long did the Mediator command take to execute?
            /// </summary>
            public const string MediatorRequestsExecutionTime = "Mediator Requests Execution Time";

            /// <summary>
            /// How many Mediator requests were made?
            /// </summary>
            public const string MediatorRequests = "Mediator Requests";

            /// <summary>
            /// What was the last page number that we processed?
            /// </summary>
            public const string MediatorRequestsExceptions = "Mediator Requests Exceptions";
        }

        public static class QueryExecution
        {
            public const string ExecutionTime = nameof(ExecutionTime);
            public const string Success = nameof(Success);
            public const string Exception = nameof(Exception);
        }

        public static class CommandExecution
        {

            public const string ExecutionTime = nameof(ExecutionTime);
            public const string ValidationError = nameof(ValidationError);
            public const string Success = nameof(Success);
            public const string Exception = nameof(Exception);
        }

        public static class Authentication
        {
            public const string UserSuccessfullyAuthenticatedViaGoogle = nameof(UserSuccessfullyAuthenticatedViaGoogle);
            public const string UserFailedToAuthenticateViaGoogle = nameof(UserFailedToAuthenticateViaGoogle);

            public const string UserSuccessfullyAuthenticatedViaAuth0 = nameof(UserSuccessfullyAuthenticatedViaAuth0);
            public const string UserFailedToAuthenticateViaAuth0 = nameof(UserFailedToAuthenticateViaAuth0);

            public const string UserSuccessfullyAuthenticatedViaUserAccountSwitch = nameof(UserSuccessfullyAuthenticatedViaUserAccountSwitch);
            public const string UseFailedToAuthenticateViaUserAccountSwitch = nameof(UseFailedToAuthenticateViaUserAccountSwitch);

            public const string UserSuccessfullyAuthenticatedViaLoginToken = nameof(UserSuccessfullyAuthenticatedViaLoginToken);
            public const string UserFailedToAuthenticateViaLoginToken = nameof(UserFailedToAuthenticateViaLoginToken);

            public const string UserSuccessfullyInitiatedMfa = nameof(UserSuccessfullyInitiatedMfa);
            public const string UserFailedToInitiatedMfa = nameof(UserFailedToInitiatedMfa);

            public const string UserSuccessfullyAnsweredMfa = nameof(UserSuccessfullyAnsweredMfa);
            public const string UserFailedToAnswerMfa = nameof(UserFailedToAnswerMfa);

            public const string UserSuccessfullyRequestedPasswordReset = nameof(UserSuccessfullyRequestedPasswordReset);
            public const string UserFailedToRequestPasswordReset = nameof(UserFailedToRequestPasswordReset);

            public const string UserSuccessfullyRedeemedInvitation = nameof(UserSuccessfullyRedeemedInvitation);
            public const string UserFailedRedeemingInvitation = nameof(UserFailedRedeemingInvitation);

            public const string ApiRequestAuthenticationSuccess = nameof(ApiRequestAuthenticationSuccess);
            public const string ApiRequestAuthenticationFailure = nameof(ApiRequestAuthenticationFailure);
        }

        public static class LibraryAddAuthor
        {
            public const string AddAuthorSuccess = nameof(AddAuthorSuccess);
            public const string AddAuthorError = nameof(AddAuthorError);
        }

        public static class LibraryAddBook
        {
            public const string AddBookSuccess = nameof(AddBookSuccess);
            public const string AddBookError = nameof(AddBookError);
        }
    }
}
