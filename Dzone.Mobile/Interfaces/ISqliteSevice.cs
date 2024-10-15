namespace Dzone.Mobile.Interfaces;

public interface ISqliteSevice
{
    ISQLiteAsyncConnection CreatConnection();
    Task<bool> InitiTables();
}
