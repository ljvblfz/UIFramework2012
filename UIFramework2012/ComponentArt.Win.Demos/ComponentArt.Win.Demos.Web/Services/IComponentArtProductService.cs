using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web;


namespace ComponentArt.Win.Demos.Web
{
  [DataContract]
  public class Product
  {
    [DataMember]
    public int Id;

    [DataMember]
    public string Name;

    [DataMember]
    public int CategoryId;

    [DataMember]
    public string CategoryName;

    [DataMember]
    public string QuantityPerUnit;

    [DataMember]
    public double UnitPrice;

    [DataMember]
    public int UnitsInStock;

    [DataMember]
    public bool Discontinued;
  }

  [DataContract]
  public class Category
  {
    [DataMember]
    public int Id;

    [DataMember]
    public string Name;

    [DataMember]
    public string Description;
  }

  [DataContract]
  public class Order
  {
    [DataMember]
    public int Id;

    [DataMember]
    public double Price;

    [DataMember]
    public int Quantity;

    [DataMember]
    public double Discount;
  }

  public interface IComponentArtProductService
  {
    List<Category> GetCategories();

    List<Product> GetProducts(int? categoryId);

    List<Order> GetOrders(int productId);
  }
}
