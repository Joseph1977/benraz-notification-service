namespace Notifications.Domain.Messages.MessagesFilter
{
    /// <summary>
    /// Filter mode type.
    /// </summary>
    public enum FilterModeType
    {
        /// <summary>
        /// Allow only from white list.
        /// </summary>
        eWhiteList = 0,

        /// <summary>
        /// Allo only if fit a regix pattern.
        /// </summary>
        ePatern = 1,

        /// <summary>
        /// Travers ALL to specific email.
        /// </summary>
        eTraverse = 2,

        /// <summary>
        /// Do not filter, allow all.
        /// </summary>
        eAllowAll = 3,

        /// <summary>
        /// This mean No email will be passed, kind of disable feature.
        /// </summary>
        eDiableAll = 4,

        /// <summary>
        /// Allow emails only with Domain in the DomainsWhitelist from EmailFilter list, not valid for phones.
        /// </summary>
        eWhiteDomainList = 5
    }
}
