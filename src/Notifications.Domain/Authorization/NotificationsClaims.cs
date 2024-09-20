namespace Notifications.Domain.Authorization
{
    /// <summary>
    /// Notifications claims.
    /// </summary>
    public static class NotificationsClaims
    {
        /// <summary>
        /// Send emails claim.
        /// </summary>
        public const string EMAILS_SEND = "notifications-emails-send";

        /// <summary>
        /// Read settings claim.
        /// </summary>
        public const string SETTINGS_READ = "notifications-settings-read";

        /// <summary>
        /// Add settings claim.
        /// </summary>
        public const string SETTINGS_ADD = "notifications-settings-add";

        /// <summary>
        /// Update settings claim.
        /// </summary>
        public const string SETTINGS_UPDATE = "notifications-settings-update";

        /// <summary>
        /// Delete settings claim.
        /// </summary>
        public const string SETTINGS_DELETE = "notifications-settings-delete";

        /// <summary>
        /// Execute job policy.
        /// </summary>
        public const string JOB_EXECUTE = "notifications-job-execute";

        /// <summary>
        /// Send sms claim.
        /// </summary>
        public const string SMS_SEND = "notifications-sms-send";
    }
}

