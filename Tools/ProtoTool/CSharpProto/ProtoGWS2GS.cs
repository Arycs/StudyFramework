// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Proto_GWS2GS.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace YouYou.Proto {

  /// <summary>Holder for reflection information generated from Proto_GWS2GS.proto</summary>
  public static partial class ProtoGWS2GSReflection {

    #region Descriptor
    /// <summary>File descriptor for Proto_GWS2GS.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ProtoGWS2GSReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChJQcm90b19HV1MyR1MucHJvdG8SDFlvdVlvdS5Qcm90byIrChdHV1MyR1Nf",
            "UmVnR2F0ZXdheVNlcnZlchIQCghTZXJ2ZXJJZBgBIAEoBWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::YouYou.Proto.GWS2GS_RegGatewayServer), global::YouYou.Proto.GWS2GS_RegGatewayServer.Parser, new[]{ "ServerId" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///网关服务器注册到游戏服务器
  /// </summary>
  public sealed partial class GWS2GS_RegGatewayServer : YouYou.IProto, pb::IMessage<GWS2GS_RegGatewayServer> {
    private static readonly pb::MessageParser<GWS2GS_RegGatewayServer> _parser = new pb::MessageParser<GWS2GS_RegGatewayServer>(() => new GWS2GS_RegGatewayServer());
    public ushort ProtoId => ProtoIdDefine.Proto_GWS2GS_RegGatewayServer;
    public string ProtoEnName => "GWS2GS_RegGatewayServer";
    public ProtoCategory Category => ProtoCategory.GatewayServer2GameServer;
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GWS2GS_RegGatewayServer> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::YouYou.Proto.ProtoGWS2GSReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GWS2GS_RegGatewayServer() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GWS2GS_RegGatewayServer(GWS2GS_RegGatewayServer other) : this() {
      serverId_ = other.serverId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GWS2GS_RegGatewayServer Clone() {
      return new GWS2GS_RegGatewayServer(this);
    }

    /// <summary>Field number for the "ServerId" field.</summary>
    public const int ServerIdFieldNumber = 1;
    private int serverId_;
    /// <summary>
    ///服务器编号
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ServerId {
      get { return serverId_; }
      set {
        serverId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GWS2GS_RegGatewayServer);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GWS2GS_RegGatewayServer other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ServerId != other.ServerId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ServerId != 0) hash ^= ServerId.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ServerId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ServerId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ServerId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ServerId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GWS2GS_RegGatewayServer other) {
      if (other == null) {
        return;
      }
      if (other.ServerId != 0) {
        ServerId = other.ServerId;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            ServerId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
