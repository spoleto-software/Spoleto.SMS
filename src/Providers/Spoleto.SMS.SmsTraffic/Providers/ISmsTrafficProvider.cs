namespace Spoleto.SMS.Providers.SmsTraffic
{
    /// <summary>
    /// The SmsTraffic provider.
    /// </summary>
    /// <remarks>
    /// <see href="https://www.smstraffic.ru/api"/>.
    /// </remarks>
    public interface ISmsTrafficProvider : ISmsProvider
    {
        /// <summary>
        /// Sends the SMS message.
        /// </summary>
        /// <param name="message">The SMS message to be send</param>
        /// <returns><see cref="SmsSendingResult"/> to indicate sending result.</returns>
        SmsSendingResult Send(SmsTrafficMessage message);

        /// <summary>
        /// Async sends the SMS message.
        /// </summary>
        /// <param name="message">The SMS message to be send</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns><see cref="SmsSendingResult"/> to indicate sending result.</returns>
        /// <exception cref="OperationCanceledException">Thrown when <paramref name="cancellationToken"/> is canceled.</exception>
        Task<SmsSendingResult> SendAsync(SmsTrafficMessage message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the balance.
        /// </summary>
        /// <returns>The balance information (number of SMS).</returns>
        int GetBalance();

        /// <summary>
        /// Async gets the balance.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The balance information (number of SMS).</returns>
        Task<int> GetBalanceAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the all SMS groups.
        /// </summary>
        /// <returns>The list of all SMS groups.</returns>
        GroupListInformation GetGroupListInformation();

        /// <summary>
        /// Async gets the all SMS groups.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The list of all SMS groups.</returns>
        Task<GroupListInformation> GetGroupListInformationAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the specified SMS group by the given identifier.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns>The information of the SMS group.</returns>
        GroupInformation? GetGroupInformation(string groupId);

        /// <summary>
        /// Async gets the specified SMS group by the given identifier.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The information of the SMS group.</returns>
        Task<GroupInformation?> GetGroupInformationAsync(string groupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the only one member to the specified SMS group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="memberNumber">The phone number of the new member.</param>
        /// <returns>The information of adding the member to the SMS group.</returns>
        GroupOperation AddGroupMember(string groupId, string memberNumber);

        /// <summary>
        /// Async adds the only one member to the specified SMS group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="memberNumber">The phone number of the new member.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The information of adding the member to the SMS group.</returns>
        Task<GroupOperation> AddGroupMemberAsync(string groupId, string memberNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the members to the specified SMS group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="memberNumbers">The phone numbers of the new members.</param>
        /// <returns>The information of adding the members to the SMS group.</returns>
        GroupOperation AddGroupMembers(string groupId, IEnumerable<string> memberNumbers);

        /// <summary>
        /// Async adds the members to the specified SMS group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="memberNumbers">The phone numbers of the new members.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The information of adding the members to the SMS group.</returns>
        Task<GroupOperation> AddGroupMembersAsync(string groupId, IEnumerable<string> memberNumbers, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the member from the specified SMS group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="memberNumber">The phone number of the member to remove from the group.</param>
        /// <returns>The information of removing the member from the SMS group.</returns>
        GroupOperation RemoveGroupMember(string groupId, string memberNumber);

        /// <summary>
        /// Async removes the member from the specified SMS group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="memberNumber">The phone number of the member to remove from the group.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The information of removing the member from the SMS group.</returns>
        Task<GroupOperation> RemoveGroupMemberAsync(string groupId, string memberNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the members from the specified SMS group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="memberNumbers">The phone numbers of the members to remove from the group.</param>
        /// <returns>The information of removing the members from the SMS group.</returns>
        GroupOperation RemoveGroupMembers(string groupId, IEnumerable<string> memberNumbers);

        /// <summary>
        /// Async removes the members from the specified SMS group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="memberNumbers">The phone numbers of the members to remove from the group.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>The information of removing the members from the SMS group.</returns>
        Task<GroupOperation> RemoveGroupMembersAsync(string groupId, IEnumerable<string> memberNumbers, CancellationToken cancellationToken = default);
    }
}
