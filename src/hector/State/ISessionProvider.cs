namespace Hector.State {
    public interface ISessionProvider {
        UserSession Session { get; set; }
    }
}