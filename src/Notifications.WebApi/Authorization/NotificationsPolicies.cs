namespace Notifications.WebApi.Authorization
{
    /// <summary>
    /// Notifications policies.
    /// </summary>
    public static class NotificationsPolicies
    {
        /// <summary>
        /// Send emails policy.
        /// </summary>
        public const string EMAILS_SEND = "emails-send";

        /// <summary>
        /// Read settings policy.
        /// </summary>
        public const string SETTINGS_READ = "settings-read";

        /// <summary>
        /// Add settings policy.
        /// </summary>
        public const string SETTINGS_ADD = "settings-add";

        /// <summary>
        /// Update settings policy.
        /// </summary>
        public const string SETTINGS_UPDATE = "settings-update";

        /// <summary>
        /// Delete settings policy.
        /// </summary>
        public const string SETTINGS_DELETE = "settings-delete";

        /// <summary>
        /// Execute job policy.
        /// </summary>
        public const string JOB_EXECUTE = "job-execute";

        /// <summary>
        /// Send sms policy.
        /// </summary>
        public const string SMS_SEND = "sms-send";
    }
}
