//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Game.Server.WebLogin {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="dandantang", ConfigurationName="WebLogin.PassPortSoap")]
    public interface PassPortSoap {
        
        // CODEGEN: Generating message contract since element name applicationname from namespace dandantang is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="dandantang/ChenckValidate", ReplyAction="*")]
        Game.Server.WebLogin.ChenckValidateResponse ChenckValidate(Game.Server.WebLogin.ChenckValidateRequest request);
        
        // CODEGEN: Generating message contract since element name applicationname from namespace dandantang is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="dandantang/Get_UserSex", ReplyAction="*")]
        Game.Server.WebLogin.Get_UserSexResponse Get_UserSex(Game.Server.WebLogin.Get_UserSexRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ChenckValidateRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ChenckValidate", Namespace="dandantang", Order=0)]
        public Game.Server.WebLogin.ChenckValidateRequestBody Body;
        
        public ChenckValidateRequest() {
        }
        
        public ChenckValidateRequest(Game.Server.WebLogin.ChenckValidateRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="dandantang")]
    public partial class ChenckValidateRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string applicationname;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string username;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string password;
        
        public ChenckValidateRequestBody() {
        }
        
        public ChenckValidateRequestBody(string applicationname, string username, string password) {
            this.applicationname = applicationname;
            this.username = username;
            this.password = password;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ChenckValidateResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ChenckValidateResponse", Namespace="dandantang", Order=0)]
        public Game.Server.WebLogin.ChenckValidateResponseBody Body;
        
        public ChenckValidateResponse() {
        }
        
        public ChenckValidateResponse(Game.Server.WebLogin.ChenckValidateResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="dandantang")]
    public partial class ChenckValidateResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ChenckValidateResult;
        
        public ChenckValidateResponseBody() {
        }
        
        public ChenckValidateResponseBody(string ChenckValidateResult) {
            this.ChenckValidateResult = ChenckValidateResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Get_UserSexRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Get_UserSex", Namespace="dandantang", Order=0)]
        public Game.Server.WebLogin.Get_UserSexRequestBody Body;
        
        public Get_UserSexRequest() {
        }
        
        public Get_UserSexRequest(Game.Server.WebLogin.Get_UserSexRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="dandantang")]
    public partial class Get_UserSexRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string applicationname;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string username;
        
        public Get_UserSexRequestBody() {
        }
        
        public Get_UserSexRequestBody(string applicationname, string username) {
            this.applicationname = applicationname;
            this.username = username;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Get_UserSexResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Get_UserSexResponse", Namespace="dandantang", Order=0)]
        public Game.Server.WebLogin.Get_UserSexResponseBody Body;
        
        public Get_UserSexResponse() {
        }
        
        public Get_UserSexResponse(Game.Server.WebLogin.Get_UserSexResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="dandantang")]
    public partial class Get_UserSexResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public System.Nullable<bool> Get_UserSexResult;
        
        public Get_UserSexResponseBody() {
        }
        
        public Get_UserSexResponseBody(System.Nullable<bool> Get_UserSexResult) {
            this.Get_UserSexResult = Get_UserSexResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface PassPortSoapChannel : Game.Server.WebLogin.PassPortSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PassPortSoapClient : System.ServiceModel.ClientBase<Game.Server.WebLogin.PassPortSoap>, Game.Server.WebLogin.PassPortSoap {
        
        public PassPortSoapClient() {
        }
        
        public PassPortSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PassPortSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PassPortSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PassPortSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Game.Server.WebLogin.ChenckValidateResponse Game.Server.WebLogin.PassPortSoap.ChenckValidate(Game.Server.WebLogin.ChenckValidateRequest request) {
            return base.Channel.ChenckValidate(request);
        }
        
        public string ChenckValidate(string applicationname, string username, string password) {
            Game.Server.WebLogin.ChenckValidateRequest inValue = new Game.Server.WebLogin.ChenckValidateRequest();
            inValue.Body = new Game.Server.WebLogin.ChenckValidateRequestBody();
            inValue.Body.applicationname = applicationname;
            inValue.Body.username = username;
            inValue.Body.password = password;
            Game.Server.WebLogin.ChenckValidateResponse retVal = ((Game.Server.WebLogin.PassPortSoap)(this)).ChenckValidate(inValue);
            return retVal.Body.ChenckValidateResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Game.Server.WebLogin.Get_UserSexResponse Game.Server.WebLogin.PassPortSoap.Get_UserSex(Game.Server.WebLogin.Get_UserSexRequest request) {
            return base.Channel.Get_UserSex(request);
        }
        
        public System.Nullable<bool> Get_UserSex(string applicationname, string username) {
            Game.Server.WebLogin.Get_UserSexRequest inValue = new Game.Server.WebLogin.Get_UserSexRequest();
            inValue.Body = new Game.Server.WebLogin.Get_UserSexRequestBody();
            inValue.Body.applicationname = applicationname;
            inValue.Body.username = username;
            Game.Server.WebLogin.Get_UserSexResponse retVal = ((Game.Server.WebLogin.PassPortSoap)(this)).Get_UserSex(inValue);
            return retVal.Body.Get_UserSexResult;
        }
    }
}
