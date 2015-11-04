namespace Ubik.Web.Backoffice.Contracts
{
    internal interface IBackofficeContentProvider
    {
        IBackofficeContent Current { get; }
    }
}