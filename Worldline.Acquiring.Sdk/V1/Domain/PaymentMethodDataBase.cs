/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class PaymentMethodDataBase
    {
        /// <summary>
        /// The masked identifier of the card used in the original transaction that led to the dispute.
        /// The masked identifier typically includes the first six and last four digits of the card number, with the middle
        /// digits replaced by asterisks or other masking characters. Different masking patterns are used for card and non-card
        /// payment methods.
        /// <list type="bullet">
        ///   <item><description>Card: 717171*******1234</description></item>
        ///   <item><description>Non-Card: DE89****4567</description></item>
        ///   <item><description>Non-Card: j***@email.com</description></item>
        /// </list>
        /// </summary>
        public string MaskedIdentifier { get; set; }

        /// <summary>
        /// The card scheme used in the original transaction that led to the dispute.
        /// <p />
        /// Common values:
        /// <list type="bullet">
        ///   <item><description><c>MASTERCARD</c>	(Mastercard)</description></item>
        ///   <item><description><c>VISA</c>	(Visa)</description></item>
        ///   <item><description><c>JCB</c>	(Japan Credit Bureau)</description></item>
        ///   <item><description><c>UNION_PAY</c>	(UnionPay International)</description></item>
        ///   <item><description><c>DINERS</c>	(Diners)</description></item>
        ///   <item><description><c>EUROPEAN_PAYMENTS_INITIATIVE</c>	(European Payment Initiative (Wero))</description></item>
        ///   <item><description><c>CARTE_BANCAIRES</c>	(Cartes Bancaires)</description></item>
        ///   <item><description><c>EFTPOS</c>	(EFTPOS (Electronic Funds Transfer at Point of Sale))</description></item>
        /// </list>
        /// <p />
        /// Support for new schemes may be introduced without notice. Clients should handle unknown values gracefully.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// The card scheme brand used in the original transaction that led to the dispute.
        /// <p />
        /// Possible values are:
        /// <list type="bullet">
        ///   <item><description><c>MSI</c>	(Maestro Debit Card)</description></item>
        ///   <item><description><c>MCC</c>	(MasterCard Credit Card)</description></item>
        ///   <item><description><c>CIR</c>	(Cirrus Debit Card)</description></item>
        ///   <item><description><c>DMC</c>	(MasterCard Debit Card)</description></item>
        ///   <item><description><c>VISA</c> (VISA Credit Card)</description></item>
        ///   <item><description><c>VPAY</c> (V PAY)</description></item>
        ///   <item><description><c>PLUS</c> (PLUS)</description></item>
        ///   <item><description><c>ELEC</c> (Visa Electron)</description></item>
        ///   <item><description><c>JCB</c>	(JCB)</description></item>
        ///   <item><description><c>CUP</c>	(China Union Pay Credit Card)</description></item>
        ///   <item><description><c>DINER</c> (DINERS credit card)</description></item>
        ///   <item><description><c>EFTPOS</c> (EFTPOS, for Australia)</description></item>
        ///   <item><description><c>CB</c> (Cartes Bancaires Domestic Scheme France)</description></item>
        ///   <item><description><c>WERO</c> (European payment solution developed by EPI)</description></item>
        /// </list>
        /// </summary>
        public string SchemeBrand { get; set; }
    }
}
