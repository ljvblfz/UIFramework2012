﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 3.0.40624.0
// 
namespace ComponentArt.Silverlight.Demos.WCFCarService {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CarCategory", Namespace="http://schemas.datacontract.org/2004/07/ComponentArt.Silverlight.Demos.Web")]
    public partial class CarCategory : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string DescriptionField;
        
        private int IdField;
        
        private string NameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CarModel", Namespace="http://schemas.datacontract.org/2004/07/ComponentArt.Silverlight.Demos.Web")]
    public partial class CarModel : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int CategoryIdField;
        
        private int IdField;
        
        private System.DateTime LastOrderedOnField;
        
        private double ListPriceField;
        
        private string ModelField;
        
        private int UnitsInStockField;
        
        private int UnitsOnOrderField;
        
        private int YearField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int CategoryId {
            get {
                return this.CategoryIdField;
            }
            set {
                if ((this.CategoryIdField.Equals(value) != true)) {
                    this.CategoryIdField = value;
                    this.RaisePropertyChanged("CategoryId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime LastOrderedOn {
            get {
                return this.LastOrderedOnField;
            }
            set {
                if ((this.LastOrderedOnField.Equals(value) != true)) {
                    this.LastOrderedOnField = value;
                    this.RaisePropertyChanged("LastOrderedOn");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double ListPrice {
            get {
                return this.ListPriceField;
            }
            set {
                if ((this.ListPriceField.Equals(value) != true)) {
                    this.ListPriceField = value;
                    this.RaisePropertyChanged("ListPrice");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Model {
            get {
                return this.ModelField;
            }
            set {
                if ((object.ReferenceEquals(this.ModelField, value) != true)) {
                    this.ModelField = value;
                    this.RaisePropertyChanged("Model");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int UnitsInStock {
            get {
                return this.UnitsInStockField;
            }
            set {
                if ((this.UnitsInStockField.Equals(value) != true)) {
                    this.UnitsInStockField = value;
                    this.RaisePropertyChanged("UnitsInStock");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int UnitsOnOrder {
            get {
                return this.UnitsOnOrderField;
            }
            set {
                if ((this.UnitsOnOrderField.Equals(value) != true)) {
                    this.UnitsOnOrderField = value;
                    this.RaisePropertyChanged("UnitsOnOrder");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Year {
            get {
                return this.YearField;
            }
            set {
                if ((this.YearField.Equals(value) != true)) {
                    this.YearField = value;
                    this.RaisePropertyChanged("Year");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CarSale", Namespace="http://schemas.datacontract.org/2004/07/ComponentArt.Silverlight.Demos.Web")]
    public partial class CarSale : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string ColorField;
        
        private System.DateTime DateOfSaleField;
        
        private int IdField;
        
        private double PriceField;
        
        private string SalesPersonField;
        
        private string SerialNumberField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Color {
            get {
                return this.ColorField;
            }
            set {
                if ((object.ReferenceEquals(this.ColorField, value) != true)) {
                    this.ColorField = value;
                    this.RaisePropertyChanged("Color");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime DateOfSale {
            get {
                return this.DateOfSaleField;
            }
            set {
                if ((this.DateOfSaleField.Equals(value) != true)) {
                    this.DateOfSaleField = value;
                    this.RaisePropertyChanged("DateOfSale");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Price {
            get {
                return this.PriceField;
            }
            set {
                if ((this.PriceField.Equals(value) != true)) {
                    this.PriceField = value;
                    this.RaisePropertyChanged("Price");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SalesPerson {
            get {
                return this.SalesPersonField;
            }
            set {
                if ((object.ReferenceEquals(this.SalesPersonField, value) != true)) {
                    this.SalesPersonField = value;
                    this.RaisePropertyChanged("SalesPerson");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SerialNumber {
            get {
                return this.SerialNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.SerialNumberField, value) != true)) {
                    this.SerialNumberField = value;
                    this.RaisePropertyChanged("SerialNumber");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WCFCarService.WCFCarService")]
    public interface WCFCarService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/WCFCarService/GetCategories", ReplyAction="http://tempuri.org/WCFCarService/GetCategoriesResponse")]
        System.IAsyncResult BeginGetCategories(System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarCategory> EndGetCategories(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/WCFCarService/GetModels", ReplyAction="http://tempuri.org/WCFCarService/GetModelsResponse")]
        System.IAsyncResult BeginGetModels(System.Nullable<int> categoryId, System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarModel> EndGetModels(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/WCFCarService/GetSales", ReplyAction="http://tempuri.org/WCFCarService/GetSalesResponse")]
        System.IAsyncResult BeginGetSales(int productId, System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarSale> EndGetSales(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface WCFCarServiceChannel : ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class GetCategoriesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetCategoriesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarCategory> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarCategory>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class GetModelsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetModelsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarModel> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarModel>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class GetSalesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetSalesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarSale> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarSale>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class WCFCarServiceClient : System.ServiceModel.ClientBase<ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService>, ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService {
        
        private BeginOperationDelegate onBeginGetCategoriesDelegate;
        
        private EndOperationDelegate onEndGetCategoriesDelegate;
        
        private System.Threading.SendOrPostCallback onGetCategoriesCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetModelsDelegate;
        
        private EndOperationDelegate onEndGetModelsDelegate;
        
        private System.Threading.SendOrPostCallback onGetModelsCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetSalesDelegate;
        
        private EndOperationDelegate onEndGetSalesDelegate;
        
        private System.Threading.SendOrPostCallback onGetSalesCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public WCFCarServiceClient() {
        }
        
        public WCFCarServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WCFCarServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WCFCarServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WCFCarServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<GetCategoriesCompletedEventArgs> GetCategoriesCompleted;
        
        public event System.EventHandler<GetModelsCompletedEventArgs> GetModelsCompleted;
        
        public event System.EventHandler<GetSalesCompletedEventArgs> GetSalesCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService.BeginGetCategories(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetCategories(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarCategory> ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService.EndGetCategories(System.IAsyncResult result) {
            return base.Channel.EndGetCategories(result);
        }
        
        private System.IAsyncResult OnBeginGetCategories(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService)(this)).BeginGetCategories(callback, asyncState);
        }
        
        private object[] OnEndGetCategories(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarCategory> retVal = ((ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService)(this)).EndGetCategories(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetCategoriesCompleted(object state) {
            if ((this.GetCategoriesCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetCategoriesCompleted(this, new GetCategoriesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetCategoriesAsync() {
            this.GetCategoriesAsync(null);
        }
        
        public void GetCategoriesAsync(object userState) {
            if ((this.onBeginGetCategoriesDelegate == null)) {
                this.onBeginGetCategoriesDelegate = new BeginOperationDelegate(this.OnBeginGetCategories);
            }
            if ((this.onEndGetCategoriesDelegate == null)) {
                this.onEndGetCategoriesDelegate = new EndOperationDelegate(this.OnEndGetCategories);
            }
            if ((this.onGetCategoriesCompletedDelegate == null)) {
                this.onGetCategoriesCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetCategoriesCompleted);
            }
            base.InvokeAsync(this.onBeginGetCategoriesDelegate, null, this.onEndGetCategoriesDelegate, this.onGetCategoriesCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService.BeginGetModels(System.Nullable<int> categoryId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetModels(categoryId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarModel> ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService.EndGetModels(System.IAsyncResult result) {
            return base.Channel.EndGetModels(result);
        }
        
        private System.IAsyncResult OnBeginGetModels(object[] inValues, System.AsyncCallback callback, object asyncState) {
            System.Nullable<int> categoryId = ((System.Nullable<int>)(inValues[0]));
            return ((ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService)(this)).BeginGetModels(categoryId, callback, asyncState);
        }
        
        private object[] OnEndGetModels(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarModel> retVal = ((ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService)(this)).EndGetModels(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetModelsCompleted(object state) {
            if ((this.GetModelsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetModelsCompleted(this, new GetModelsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetModelsAsync(System.Nullable<int> categoryId) {
            this.GetModelsAsync(categoryId, null);
        }
        
        public void GetModelsAsync(System.Nullable<int> categoryId, object userState) {
            if ((this.onBeginGetModelsDelegate == null)) {
                this.onBeginGetModelsDelegate = new BeginOperationDelegate(this.OnBeginGetModels);
            }
            if ((this.onEndGetModelsDelegate == null)) {
                this.onEndGetModelsDelegate = new EndOperationDelegate(this.OnEndGetModels);
            }
            if ((this.onGetModelsCompletedDelegate == null)) {
                this.onGetModelsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetModelsCompleted);
            }
            base.InvokeAsync(this.onBeginGetModelsDelegate, new object[] {
                        categoryId}, this.onEndGetModelsDelegate, this.onGetModelsCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService.BeginGetSales(int productId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetSales(productId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarSale> ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService.EndGetSales(System.IAsyncResult result) {
            return base.Channel.EndGetSales(result);
        }
        
        private System.IAsyncResult OnBeginGetSales(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int productId = ((int)(inValues[0]));
            return ((ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService)(this)).BeginGetSales(productId, callback, asyncState);
        }
        
        private object[] OnEndGetSales(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarSale> retVal = ((ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService)(this)).EndGetSales(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetSalesCompleted(object state) {
            if ((this.GetSalesCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetSalesCompleted(this, new GetSalesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetSalesAsync(int productId) {
            this.GetSalesAsync(productId, null);
        }
        
        public void GetSalesAsync(int productId, object userState) {
            if ((this.onBeginGetSalesDelegate == null)) {
                this.onBeginGetSalesDelegate = new BeginOperationDelegate(this.OnBeginGetSales);
            }
            if ((this.onEndGetSalesDelegate == null)) {
                this.onEndGetSalesDelegate = new EndOperationDelegate(this.OnEndGetSales);
            }
            if ((this.onGetSalesCompletedDelegate == null)) {
                this.onGetSalesCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetSalesCompleted);
            }
            base.InvokeAsync(this.onBeginGetSalesDelegate, new object[] {
                        productId}, this.onEndGetSalesDelegate, this.onGetSalesCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService CreateChannel() {
            return new WCFCarServiceClientChannel(this);
        }
        
        private class WCFCarServiceClientChannel : ChannelBase<ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService>, ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService {
            
            public WCFCarServiceClientChannel(System.ServiceModel.ClientBase<ComponentArt.Silverlight.Demos.WCFCarService.WCFCarService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetCategories(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("GetCategories", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarCategory> EndGetCategories(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarCategory> _result = ((System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarCategory>)(base.EndInvoke("GetCategories", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetModels(System.Nullable<int> categoryId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = categoryId;
                System.IAsyncResult _result = base.BeginInvoke("GetModels", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarModel> EndGetModels(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarModel> _result = ((System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarModel>)(base.EndInvoke("GetModels", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetSales(int productId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = productId;
                System.IAsyncResult _result = base.BeginInvoke("GetSales", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarSale> EndGetSales(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarSale> _result = ((System.Collections.ObjectModel.ObservableCollection<ComponentArt.Silverlight.Demos.WCFCarService.CarSale>)(base.EndInvoke("GetSales", _args, result)));
                return _result;
            }
        }
    }
}
