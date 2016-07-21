using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web;


namespace ComponentArt.Silverlight.Demos.Web
{
  [DataContract]
  public class CarModel
  {
    [DataMember]
    public int Id;

    [DataMember]
    public string Model;

    [DataMember]
    public int CategoryId;

    [DataMember]
    public int Year;

    [DataMember]
    public double ListPrice;

    [DataMember]
    public int UnitsInStock;

    [DataMember]
    public int UnitsOnOrder;

    [DataMember]
    public DateTime LastOrderedOn;
  }

  [DataContract]
  public class CarCategory
  {
    [DataMember]
    public int Id;

    [DataMember]
    public string Name;

    [DataMember]
    public string Description;
  }

  [DataContract]
  public class CarSale
  {
    [DataMember]
    public int Id;

    [DataMember]
    public DateTime DateOfSale;

    [DataMember]
    public string SalesPerson;

    [DataMember]
    public double Price;

    [DataMember]
    public string SerialNumber;

    [DataMember]
    public string Color;
  }

  public interface IComponentArtCarService
  {
    List<CarCategory> GetCategories();

    List<CarModel> GetModels(int? categoryId);

    List<CarSale> GetSales(int modelId);
  }
}
