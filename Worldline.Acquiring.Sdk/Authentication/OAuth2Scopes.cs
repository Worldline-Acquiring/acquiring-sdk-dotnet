/*
 * This file was automatically generated.
 */
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Worldline.Acquiring.Sdk.Authentication
{
    public static class OAuth2Scopes
    {
        private static readonly IImmutableDictionary<string, IImmutableDictionary<string, IImmutableSet<string>>>
            ScopesByOperation = ImmutableDictionary.CreateRange(
                new[]
                {
                    new KeyValuePair<string, IImmutableDictionary<string, IImmutableSet<string>>>("v1",
                        ImmutableDictionary.CreateRange(
                            new[]
                            {
                                new KeyValuePair<string, IImmutableSet<string>>("processPayment",
                                    ImmutableHashSet.Create("processing_payment")),
                                new KeyValuePair<string, IImmutableSet<string>>("getPaymentStatus",
                                    ImmutableHashSet.Create("processing_payment")),
                                new KeyValuePair<string, IImmutableSet<string>>("simpleCaptureOfPayment",
                                    ImmutableHashSet.Create("processing_payment")),
                                new KeyValuePair<string, IImmutableSet<string>>("reverseAuthorization",
                                    ImmutableHashSet.Create("processing_payment")),
                                new KeyValuePair<string, IImmutableSet<string>>("incrementPayment",
                                    ImmutableHashSet.Create("processing_payment")),
                                new KeyValuePair<string, IImmutableSet<string>>("createRefund",
                                    ImmutableHashSet.Create("processing_refund")),
                                new KeyValuePair<string, IImmutableSet<string>>("processStandaloneRefund",
                                    ImmutableHashSet.Create("processing_refund")),
                                new KeyValuePair<string, IImmutableSet<string>>("getRefund",
                                    ImmutableHashSet.Create("processing_refund")),
                                new KeyValuePair<string, IImmutableSet<string>>("captureRefund",
                                    ImmutableHashSet.Create("processing_refund")),
                                new KeyValuePair<string, IImmutableSet<string>>("reverseRefundAuthorization",
                                    ImmutableHashSet.Create("processing_refund")),
                                new KeyValuePair<string, IImmutableSet<string>>("processAccountVerification",
                                    ImmutableHashSet.Create("processing_accountverification")),
                                new KeyValuePair<string, IImmutableSet<string>>("processBalanceInquiry",
                                    ImmutableHashSet.Create("processing_balanceinquiry")),
                                new KeyValuePair<string, IImmutableSet<string>>("technicalReversal",
                                    ImmutableHashSet.Create("processing_operation_reverse")),
                                new KeyValuePair<string, IImmutableSet<string>>("requestDccRate",
                                    ImmutableHashSet.Create("processing_dcc_rate")),
                                new KeyValuePair<string, IImmutableSet<string>>("ping",
                                    ImmutableHashSet.Create("services_ping"))
                            }))
                });

        private static readonly IImmutableSet<string> AllScopes = ScopesByOperation.Values
            .SelectMany(m => m.Values)
            .SelectMany(s => s)
            .ToImmutableHashSet();

        /// <summary>
        /// Returns all available scopes.
        /// </summary>
        public static IImmutableSet<string> All => AllScopes;

        /// <summary>
        /// Returns all scopes needed for all operations of the given API version.
        /// </summary>
        public static IImmutableSet<string> ForApiVersion(string apiVersion)
        {
            return !ScopesByOperation.TryGetValue(apiVersion, out var operations)
                ? ImmutableHashSet<string>.Empty
                : operations.Values.SelectMany(s => s).ToImmutableHashSet();
        }

        /// <summary>
        /// Returns all scopes needed for the given operation of the given API version.
        /// </summary>
        public static IImmutableSet<string> ForOperation(string apiVersion, string operationId)
        {
            if (!ScopesByOperation.TryGetValue(apiVersion, out var operations))
            {
                return ImmutableHashSet<string>.Empty;
            }

            return operations.TryGetValue(operationId, out var scopes) ? scopes : ImmutableHashSet<string>.Empty;
        }

        /// <summary>
        /// Returns all scopes needed for the given operations of the given API version.
        /// </summary>
        public static IImmutableSet<string> ForOperations(string apiVersion, params string[] operationIds)
        {
            if (!ScopesByOperation.TryGetValue(apiVersion, out var operations))
            {
                return ImmutableHashSet<string>.Empty;
            }

            return operations.Where(kvp => operationIds.Contains(kvp.Key))
                .SelectMany(kvp => kvp.Value)
                .ToImmutableHashSet();
        }

        /// <summary>
        /// Returns all scopes needed for the operations that pass the given filter.
        /// </summary>
        /// <param name="filter">The filter to apply. The first argument is the API version, the second is the operation id.</param>
        public static IImmutableSet<string> ForOperations(Func<string, string, bool> filter)
        {
            return ScopesByOperation.SelectMany(kvp1 =>
                    kvp1.Value.Where(kvp2 => filter(kvp1.Key, kvp2.Key))
                        .SelectMany(kvp2 => kvp2.Value)
                )
                .ToImmutableHashSet();
        }
    }
}
