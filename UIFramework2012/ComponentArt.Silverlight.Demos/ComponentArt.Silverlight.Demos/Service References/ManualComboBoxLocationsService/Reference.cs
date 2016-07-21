﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 3.0.40624.0
// 
namespace ComponentArt.Silverlight.Demos.ManualComboBoxLocationsService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="ManualComboBoxLocationsService.ManualComboBoxLocationsService")]
    public interface ManualComboBoxLocationsService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:ManualComboBoxLocationsService/GetMatchesForArgument", ReplyAction="urn:ManualComboBoxLocationsService/GetMatchesForArgumentResponse")]
        System.IAsyncResult BeginGetMatchesForArgument(string toMatch, string matchMode, int iStartIndex, int iNumItems, System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<string> EndGetMatchesForArgument(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface ManualComboBoxLocationsServiceChannel : ComponentArt.Silverlight.Demos.ManualComboBoxLocationsService.ManualComboBoxLocationsService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class GetMatchesForArgumentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetMatchesForArgumentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<string> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<string>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class ManualComboBoxLocationsServiceClient : System.ServiceModel.ClientBase<ComponentArt.Silverlight.Demos.ManualComboBoxLocationsService.ManualComboBoxLocationsService>, ComponentArt.Silverlight.Demos.ManualComboBoxLocationsService.ManualComboBoxLocationsService {
        
        private BeginOperationDelegate onBeginGetMatchesForArgumentDelegate;
        
        private EndOperationDelegate onEndGetMatchesForArgumentDelegate;
        
        private System.Threading.SendOrPostCallback onGetMatchesForArgumentCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public ManualComboBoxLocationsServiceClient() {
        }
        
        public ManualComboBoxLocationsServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ManualComboBoxLocationsServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ManualComboBoxLocationsServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ManualComboBoxLocationsServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<GetMatchesForArgumentCompletedEventArgs> GetMatchesForArgumentCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult ComponentArt.Silverlight.Demos.ManualComboBoxLocationsService.ManualComboBoxLocationsService.BeginGetMatchesForArgument(string toMatch, string matchMode, int iStartIndex, int iNumItems, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetMatchesForArgument(toMatch, matchMode, iStartIndex, iNumItems, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<string> ComponentArt.Silverlight.Demos.ManualComboBoxLocationsService.ManualComboBoxLocationsService.EndGetMatchesForArgument(System.IAsyncResult result) {
            return base.Channel.EndGetMatchesForArgument(result);
        }
        
        private System.IAsyncResult OnBeginGetMatchesForArgument(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string toMatch = ((string)(inValues[0]));
            string matchMode = ((string)(inValues[1]));
            int iStartIndex = ((int)(inValues[2]));
            int iNumItems = ((int)(inValues[3]));
            return ((ComponentArt.Silverlight.Demos.ManualComboBoxLocationsService.ManualComboBoxLocationsService)(this)).BeginGetMatchesForArgument(toMatch, matchMode, iStartIndex, iNumItems, callback, asyncState);
        }
        
        private object[] OnEndGetMatchesForArgument(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<string> retVal = ((ComponentArt.Silverlight.Demos.ManualComboBoxLocationsService.ManualComboBoxLocationsService)(this)).EndGetMatchesForArgument(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetMatchesForArgumentCompleted(object state) {
            if ((this.GetMatchesForArgumentCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetMatchesForArgumentCompleted(this, new GetMatchesForArgumentCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetMatchesForArgumentAsync(string toMatch, string matchMode, int iStartIndex, int iNumItems) {
            this.GetMatchesForArgumentAsync(toMatch, matchMode, iStartIndex, iNumItems, null);
        }
        
        public void GetMatchesForArgumentAsync(string toMatch, string matchMode, int iStartIndex, int iNumItems, object userState) {
            if ((this.onBeginGetMatchesForArgumentDelegate == null)) {
                this.onBeginGetMatchesForArgumentDelegate = new BeginOperationDelegate(this.OnBeginGetMatchesForArgument);
            }
            if ((this.onEndGetMatchesForArgumentDelegate == null)) {
                this.onEndGetMatchesForArgumentDelegate = new EndOperationDelegate(this.OnEndGetMatchesForArgument);
            }
            if ((this.onGetMatchesForArgumentCompletedDelegate == null)) {
                this.onGetMatchesForArgumentCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetMatchesForArgumentCompleted);
            }
            base.InvokeAsync(this.onBeginGetMatchesForArgumentDelegate, new object[] {
                        toMatch,
                        matchMode,
                        iStartIndex,
                        iNumItems}, this.onEndGetMatchesForArgumentDelegate, this.onGetMatchesForArgumentCompletedDelegate, userState);
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
        
        protected override ComponentArt.Silverlight.Demos.ManualComboBoxLocationsService.ManualComboBoxLocationsService CreateChannel() {
            return new ManualComboBoxLocationsServiceClientChannel(this);
        }
        
        private class ManualComboBoxLocationsServiceClientChannel : ChannelBase<ComponentArt.Silverlight.Demos.ManualComboBoxLocationsService.ManualComboBoxLocationsService>, ComponentArt.Silverlight.Demos.ManualComboBoxLocationsService.ManualComboBoxLocationsService {
            
            public ManualComboBoxLocationsServiceClientChannel(System.ServiceModel.ClientBase<ComponentArt.Silverlight.Demos.ManualComboBoxLocationsService.ManualComboBoxLocationsService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetMatchesForArgument(string toMatch, string matchMode, int iStartIndex, int iNumItems, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[4];
                _args[0] = toMatch;
                _args[1] = matchMode;
                _args[2] = iStartIndex;
                _args[3] = iNumItems;
                System.IAsyncResult _result = base.BeginInvoke("GetMatchesForArgument", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<string> EndGetMatchesForArgument(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<string> _result = ((System.Collections.ObjectModel.ObservableCollection<string>)(base.EndInvoke("GetMatchesForArgument", _args, result)));
                return _result;
            }
        }
    }
}
