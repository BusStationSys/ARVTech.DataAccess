namespace ARVTech.DataAccess.CQRS.Commands
{
    public abstract class BaseCommand
    {
        public abstract string CommandTextCreate();

        public abstract string CommandTextDelete();

        public abstract string CommandTextUpdate();
    }
}