namespace YouYouServer.Model.IHandler
{
    public interface IPlayerForGatewayClientHandler
    {
        public void Init(PlayerForGatewayClient playerForGameClient);

        public void Dispose();
        
        public void CarrySendToWorldServer(ushort protoCode, byte[] buffer);
        
        public void CarrySendToGameServer(int serverId, ushort protoCode, ProtoCategory protoCategory, byte[] buffer);
    }
}