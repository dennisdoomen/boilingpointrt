namespace BoilingPointRT.Services.Commanding
{
    public interface IHandleCommand<in TCommand>
    {
        void Handle(TCommand command);
    }
}