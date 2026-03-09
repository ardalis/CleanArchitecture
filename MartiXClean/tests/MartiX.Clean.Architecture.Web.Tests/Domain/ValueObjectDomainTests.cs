using MartiX.Clean.Architecture.Web.Domain.CartAggregate;
using MartiX.Clean.Architecture.Web.Domain.GuestUserAggregate;
using MartiX.Clean.Architecture.Web.Domain.OrderAggregate;
using MartiX.Clean.Architecture.Web.Domain.ProductAggregate;

namespace MartiX.Clean.Architecture.Web.Tests;

public class ValueObjectDomainTests
{
  [Test]
  [Arguments("ProductId")]
  [Arguments("Quantity")]
  [Arguments("Price")]
  [Arguments("CartId")]
  [Arguments("GuestUserId")]
  public void From_WhenValueInvalid_Throws(string valueObjectName)
  {
    var failed = false;

    try
    {
      _ = valueObjectName switch
      {
        "ProductId" => (object)ProductId.From(-1),
        "Quantity" => Quantity.From(0),
        "Price" => Price.From(0m),
        "CartId" => CartId.From(Guid.Empty),
        "GuestUserId" => GuestUserId.From(Guid.Empty),
        _ => throw new ArgumentOutOfRangeException(nameof(valueObjectName), valueObjectName, null)
      };
    }
    catch
    {
      failed = true;
    }

    if (!failed) throw new Exception($"Expected invalid {valueObjectName} creation to fail.");
  }
}

