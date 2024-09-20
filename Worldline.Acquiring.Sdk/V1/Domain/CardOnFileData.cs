/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class CardOnFileData
    {
        public InitialCardOnFileData InitialCardOnFileData { get; set; }

        /// <summary>
        /// Indicate wether this is the initial Card on File transaction or not
        /// </summary>
        public bool? IsInitialTransaction { get; set; }

        public SubsequentCardOnFileData SubsequentCardOnFileData { get; set; }
    }
}
