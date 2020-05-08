using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace JokeDb
{
    public class JokeDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public JokeDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(JokeItem).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(JokeItem)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

        public Task<List<JokeItem>> GetItemsAsync()
        {
            return Database.Table<JokeItem>().ToListAsync();
        }

        public Task<List<JokeItem>> GetItemsNotDoneAsync()
        {
            return Database.QueryAsync<JokeItem>("SELECT * FROM [JokeItem]");
        }

        public Task<JokeItem> GetJokeAsync(int id)
        {
            return Database.Table<JokeItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveJokeAsync(JokeItem joke)
        {
            if (joke.ID != 0)
            {
                return Database.UpdateAsync(joke);
            }
            else
            {
                return Database.InsertAsync(joke);
            }
        }

        public Task<int> DeleteItemAsync(JokeItem joke)
        {
            return Database.DeleteAsync(joke);
        }
    }
}

