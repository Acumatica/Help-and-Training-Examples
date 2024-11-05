using System;

namespace WooCommerceTest
{
    public interface IWooEntity
    {
        DateTime? DateCreatedUT { get; set; }

        DateTime? DateModified { get; set; }

    }
}
