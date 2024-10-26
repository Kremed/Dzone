namespace Dzone.Mobile.Services;

internal class SqliteSevice : ISqliteSevice
{
    public  ISQLiteAsyncConnection DbConnection = null!;

    public SqliteSevice()
    {
        Task.Run(async () =>
        {
            CreatConnection();
            await InitiTables();
        });
    }
    public ISQLiteAsyncConnection CreatConnection()
    {
        if (DbConnection is null)
        {
            DbConnection = new SQLiteAsyncConnection(Path.Combine(
                               FileSystem.AppDataDirectory, "DzoneLocalDB.db3"),
                               SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite);
        }

        return DbConnection;
    }

    public async Task<bool> InitiTables()
    {
        try
        {
            await DbConnection.CreateTableAsync<UserTbl>();
            await DbConnection.CreateTableAsync<CartLocalTbl>();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
