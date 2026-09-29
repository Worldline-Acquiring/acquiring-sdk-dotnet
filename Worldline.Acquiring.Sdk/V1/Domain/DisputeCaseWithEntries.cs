/*
 * This file was automatically generated.
 */
using System.Collections.Generic;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DisputeCaseWithEntries : DisputeCase
    {
        /// <summary>
        /// A list of entries that were done against the dispute during its lifecycle.
        /// </summary>
        public IList<DisputeEntry> Entries { get; set; }
    }
}
