/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class AdditionalResponseData
    {
        /// <summary>
        /// Merchant advice code as returned by the scheme, usually returned upon rejection.
        /// Known possible values at the time of writing this documentation are:
        /// <list type="bullet">
        ///   <item><description><c>01</c> - New Account Information Available</description></item>
        ///   <item><description><c>02</c> - Try Again Later</description></item>
        ///   <item><description><c>03</c> - Do Not Try Again</description></item>
        ///   <item><description><c>04</c> - Token requirements not fulfilled for this token type</description></item>
        ///   <item><description><c>05</c> - Negotiated value not provided</description></item>
        ///   <item><description><c>21</c> - Payment Cancellation</description></item>
        ///   <item><description><c>22</c> - Merchant does not qualify for product code</description></item>
        ///   <item><description><c>24</c> - Retry after 1 hour</description></item>
        ///   <item><description><c>25</c> - Retry after 24 hours</description></item>
        ///   <item><description><c>26</c> - Retry after 2 days</description></item>
        ///   <item><description><c>27</c> - Retry after 4 days</description></item>
        ///   <item><description><c>28</c> - Retry after 6 days</description></item>
        ///   <item><description><c>29</c> - Retry after 8 days</description></item>
        ///   <item><description><c>30</c> - Retry after 10 days</description></item>
        ///   <item><description><c>40</c> - Consumer non-reloadable prepaid card</description></item>
        ///   <item><description><c>41</c> - Consumer single-use virtual card number</description></item>
        ///   <item><description><c>42</c> - Sanctions Scoring Service: Score Exceeds Applicable Threshold Value</description></item>
        ///   <item><description><c>43</c> - Consumer multi-use virtual card number
        /// Note: In case new values are added and returned by the schemes, they will be returned as is. We will
        /// maintain the above list on a best-effort basis.</description></item>
        /// </list>
        /// </summary>
        public string MerchantAdviceCode { get; set; }

        /// <summary>
        /// Human readable description of the merchant advice code.
        /// Note: In case the merchant advice code is unknown (unmapped), the system returns <c>Unknown</c>.
        /// </summary>
        public string MerchantAdviceCodeDescription { get; set; }
    }
}
