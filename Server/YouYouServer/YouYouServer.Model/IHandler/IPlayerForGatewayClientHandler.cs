namespace YouYouServer.Model.IHandler
{
    public interface IPlayerForGatewayClientHandler
    {
        public void Init(PlayerForGatewayClient playerForGameClient);

        public void Dispose();
    }
}