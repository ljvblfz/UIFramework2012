﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComponentArt.Win.Demos.MessageService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Message", Namespace="http://schemas.datacontract.org/2004/07/ComponentArt.Win.Demos.Web")]
    [System.SerializableAttribute()]
    public partial class Message : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AttachmentIconField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailIconField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FlagIconField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LargeEmailIconField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime LastPostDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PriorityIconField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int RepliesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StartedByField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SubjectField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int TotalViewsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AttachmentIcon {
            get {
                return this.AttachmentIconField;
            }
            set {
                if ((object.ReferenceEquals(this.AttachmentIconField, value) != true)) {
                    this.AttachmentIconField = value;
                    this.RaisePropertyChanged("AttachmentIcon");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailIcon {
            get {
                return this.EmailIconField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailIconField, value) != true)) {
                    this.EmailIconField = value;
                    this.RaisePropertyChanged("EmailIcon");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FlagIcon {
            get {
                return this.FlagIconField;
            }
            set {
                if ((object.ReferenceEquals(this.FlagIconField, value) != true)) {
                    this.FlagIconField = value;
                    this.RaisePropertyChanged("FlagIcon");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LargeEmailIcon {
            get {
                return this.LargeEmailIconField;
            }
            set {
                if ((object.ReferenceEquals(this.LargeEmailIconField, value) != true)) {
                    this.LargeEmailIconField = value;
                    this.RaisePropertyChanged("LargeEmailIcon");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime LastPostDate {
            get {
                return this.LastPostDateField;
            }
            set {
                if ((this.LastPostDateField.Equals(value) != true)) {
                    this.LastPostDateField = value;
                    this.RaisePropertyChanged("LastPostDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PriorityIcon {
            get {
                return this.PriorityIconField;
            }
            set {
                if ((object.ReferenceEquals(this.PriorityIconField, value) != true)) {
                    this.PriorityIconField = value;
                    this.RaisePropertyChanged("PriorityIcon");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Replies {
            get {
                return this.RepliesField;
            }
            set {
                if ((this.RepliesField.Equals(value) != true)) {
                    this.RepliesField = value;
                    this.RaisePropertyChanged("Replies");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StartedBy {
            get {
                return this.StartedByField;
            }
            set {
                if ((object.ReferenceEquals(this.StartedByField, value) != true)) {
                    this.StartedByField = value;
                    this.RaisePropertyChanged("StartedBy");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Subject {
            get {
                return this.SubjectField;
            }
            set {
                if ((object.ReferenceEquals(this.SubjectField, value) != true)) {
                    this.SubjectField = value;
                    this.RaisePropertyChanged("Subject");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TotalViews {
            get {
                return this.TotalViewsField;
            }
            set {
                if ((this.TotalViewsField.Equals(value) != true)) {
                    this.TotalViewsField = value;
                    this.RaisePropertyChanged("TotalViews");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MessageService.WCFMessageService")]
    public interface WCFMessageService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/WCFMessageService/GetRecords", ReplyAction="http://tempuri.org/WCFMessageService/GetRecordsResponse")]
        ComponentArt.Win.Demos.MessageService.Message[] GetRecords(int pageSize, int pageIndex, string sort);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/WCFMessageService/GetRecords", ReplyAction="http://tempuri.org/WCFMessageService/GetRecordsResponse")]
        System.IAsyncResult BeginGetRecords(int pageSize, int pageIndex, string sort, System.AsyncCallback callback, object asyncState);
        
        ComponentArt.Win.Demos.MessageService.Message[] EndGetRecords(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface WCFMessageServiceChannel : ComponentArt.Win.Demos.MessageService.WCFMessageService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class GetRecordsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetRecordsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public ComponentArt.Win.Demos.MessageService.Message[] Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((ComponentArt.Win.Demos.MessageService.Message[])(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class WCFMessageServiceClient : System.ServiceModel.ClientBase<ComponentArt.Win.Demos.MessageService.WCFMessageService>, ComponentArt.Win.Demos.MessageService.WCFMessageService {
        
        private BeginOperationDelegate onBeginGetRecordsDelegate;
        
        private EndOperationDelegate onEndGetRecordsDelegate;
        
        private System.Threading.SendOrPostCallback onGetRecordsCompletedDelegate;
        
        public WCFMessageServiceClient() {
        }
        
        public WCFMessageServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WCFMessageServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WCFMessageServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WCFMessageServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<GetRecordsCompletedEventArgs> GetRecordsCompleted;
        
        public ComponentArt.Win.Demos.MessageService.Message[] GetRecords(int pageSize, int pageIndex, string sort) {
            return base.Channel.GetRecords(pageSize, pageIndex, sort);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginGetRecords(int pageSize, int pageIndex, string sort, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetRecords(pageSize, pageIndex, sort, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public ComponentArt.Win.Demos.MessageService.Message[] EndGetRecords(System.IAsyncResult result) {
            return base.Channel.EndGetRecords(result);
        }
        
        private System.IAsyncResult OnBeginGetRecords(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int pageSize = ((int)(inValues[0]));
            int pageIndex = ((int)(inValues[1]));
            string sort = ((string)(inValues[2]));
            return this.BeginGetRecords(pageSize, pageIndex, sort, callback, asyncState);
        }
        
        private object[] OnEndGetRecords(System.IAsyncResult result) {
            ComponentArt.Win.Demos.MessageService.Message[] retVal = this.EndGetRecords(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetRecordsCompleted(object state) {
            if ((this.GetRecordsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetRecordsCompleted(this, new GetRecordsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetRecordsAsync(int pageSize, int pageIndex, string sort) {
            this.GetRecordsAsync(pageSize, pageIndex, sort, null);
        }
        
        public void GetRecordsAsync(int pageSize, int pageIndex, string sort, object userState) {
            if ((this.onBeginGetRecordsDelegate == null)) {
                this.onBeginGetRecordsDelegate = new BeginOperationDelegate(this.OnBeginGetRecords);
            }
            if ((this.onEndGetRecordsDelegate == null)) {
                this.onEndGetRecordsDelegate = new EndOperationDelegate(this.OnEndGetRecords);
            }
            if ((this.onGetRecordsCompletedDelegate == null)) {
                this.onGetRecordsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetRecordsCompleted);
            }
            base.InvokeAsync(this.onBeginGetRecordsDelegate, new object[] {
                        pageSize,
                        pageIndex,
                        sort}, this.onEndGetRecordsDelegate, this.onGetRecordsCompletedDelegate, userState);
        }
    }
}
