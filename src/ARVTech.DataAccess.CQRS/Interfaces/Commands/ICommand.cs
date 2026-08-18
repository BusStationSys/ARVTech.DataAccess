namespace ARVTech.DataAccess.CQRS.Interfaces.Commands
{
    public interface ICommand
    {
        string CommandTextCreate();

        string CommandTextUpdate();

        string CommandTextDelete();
    }
}