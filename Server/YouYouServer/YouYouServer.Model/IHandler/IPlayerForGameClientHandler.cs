namespace YouYouServer.Model.IHandler
{
    public interface IPlayerForGameClientHandler
    {
        public void Init(PlayerForGameClient playerForGameClient);

        public void Dispose();
    }
}