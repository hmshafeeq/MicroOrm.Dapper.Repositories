# MicroOrm.Dapper.Repositories

This is a fork of https://github.com/phnx47/MicroOrm.Dapper.Repositories repository.
All credit goes to [Sergey Kuznetsov](https://github.com/phnx47)

## Docs

 
### Metadata attributes

[Key]
From System.ComponentModel.DataAnnotations - Use for primary key.

[Identity]
Use for identity key.

[Table]

From System.ComponentModel.DataAnnotations.Schema - By default the database table name will match the model name but it can be overridden with this.

[Column]

From System.ComponentModel.DataAnnotations.Schema - By default the column name will match the property name but it can be overridden with this.

[NotMapped] or [Ignore]

From System.ComponentModel.DataAnnotations.Schema - For “logical” properties that do not have a corresponding column and have to be ignored by the SQL Generator.

[Deleted]

For tables that implement “logical deletes” instead of physical deletes. Use this to decorate the datetime, bool or enum.

[LeftJoin]  | [InnerJoin] | [RightJoin]

Joins

[CreatedAt] |  [UpdatedAt]

Properties having any of these attributes will persist create and update timestamps accordingly. 

[SyncStatus]

Property having this attribute will be set to `null` or `0`  on each insert, update, delete. It will be set to `null` if property type is `DateTime`, else it will be set to `0`.
Note : This attribute is only useful if you are building distributed system, where when ever some record is changed in client app, we want to mark it unsynced, so it can be sent to server in next call. 
#### Disable Timestamps & Sync Status Tracking
You can disable timestamp and sync status tracking from your `DbContext`
implementation 
```
public class MySqlDbContext : DapperDbContext, IDbContext
{

        private static ILogger _logger = new Logger();

        private static readonly SqlGeneratorConfig _config = new SqlGeneratorConfig
        {
            SqlProvider = SqlProvider.MySQL,
            UseQuotationMarks = true,
            TrackTimeStamps = false,
            TrackSyncStatus = false
        };
}
```



Notes

    By default the SQL Generator is going to map the POCO name with the table name, and each public property to a column.
    If the [Deleted] is used on a certain POCO, the sentence will be an update instead of a delete.
    Supports complex primary keys.
    Supports simple Joins.

### Examples

“Users” POCO:
```
[Table("Users")]
public class User
{
    [Key, Identity]
    public int Id { get; set; }

    public string ReadOnly => "test"; // because don't have set

    public string Name { get; set; }

    public int AddressId { get; set; }

    [LeftJoin("Cars", "Id", "UserId")]
    public List<Car> Cars { get; set; }

    [LeftJoin("Addresses", "AddressId", "Id")]
    public Address Address { get; set; }

    [Deleted]
    public bool Deleted { get; set; }

    [UpdatedAt]
    public DateTime? UpdatedAt { get; set; }
}
```

“Cars” POCO:
```
[Table("Cars")]
public class Car
{
    [Key, Identity]
    public int Id { get; set; }

    public string Name { get; set; }

    public byte[] Data { get; set; }

    public int UserId { get; set; }

    [LeftJoin("Users", "UserId", "Id")]
    public User User { get; set; }
 
}
 
```
“Addresses” POCO:
```
[Table("Addresses")]
public class Address
{
   [Key]
   [Identity]
   public int Id { get; set; }

   public string Street { get; set; }

   [LeftJoin("Users", "Id", "AddressId")]
   public List<User> Users { get; set; }

   public string CityId { get; set; }
        
   [InnerJoin("Cities", "CityId", "Identifier")]
   public City City { get; set; }
}
```
Implements the DBContext for your models:

```
    public class MySqlDbContext : DapperDbContext, IDbContext
    {

        private static ILogger _logger = new Logger();

        private static readonly SqlGeneratorConfig _config = new SqlGeneratorConfig
        {
            SqlProvider = SqlProvider.MySQL,
            UseQuotationMarks = true,
            TrackTimeStamps = true,
            TrackSyncStatus = true
        };

        public ILogger Logger
        {
            get
            {
                return _logger;
            }
        }

        #region Repositeries 

        private IDapperRepository<User> _Users;
        private IDapperRepository<Car> _Cars;

        public IDapperRepository<User> Users => _Users ?? (_Users = new DapperRepository<User>(Connection, _config, _logger));
        public IDapperRepository<Car> Cars => _Cars ?? (_Cars = new DapperRepository<Car>(Connection, _config, _logger));

        #endregion

        public MySqlDbContext(string connectionString)
            : base(new MySqlConnection(connectionString))
        {
        } 

}
```
##### Usage:

```

 if(var _db =  new MySqlDbContext(connectionString))
 {
    var dateTime = DateTime.Now.AddDays(-diff);
    var user = new User { Name = "Sergey Phoenix", UpdatedAt = dateTime };
    await _db.Users.InsertAsync(user);
	......
	......
 }
 

```

##### API

You can explore in the [tests](https://github.com/hmshafeeq/MicroOrm.Dapper.Repositories/blob/master/test/MicroOrm.Dapper.Repositories.Tests/RepositoriesTests/RepositoriesTests.cs)

There are are three changes in this fork,
1. Insert method will return an  object instead of int 
2. Added an update method for the specific fields. 
```
  _db.Users.Update(x=>x.Id = 1, new { Name = "Mr. Sergey test" });
  
```
3. Added a very basic logging, to get list of executed queries
```
using (var conn = GetConnection())
{
    conn.Logger.Start();

	var users = _db.Users.FindAll();

	conn.Logger.Stop();

    // conn.Logger.Logs will contain the list of queries executed;	
}


## License

All contents of this package are licensed under the [MIT license](https://opensource.org/licenses/MIT).
