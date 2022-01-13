using Dapper;
using Microsoft.Data.SqlClient;
using Survey.Server.Services;
using Survey.Shared;
using System.Data;
using System.Text;
using System;

namespace Survey.Server.Infrastructure
{
    public class DbSurveyDataService : ISurveyDataService
    {
        private readonly IConfiguration _configuration;

        public DbSurveyDataService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<SurveyDto> GetSurvey(string id)
        {
            SurveyDto ret = null;

            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                try
                {
                    var builder = new SqlBuilder();
                    var selectSurvey = builder.AddTemplate(@"
SELECT 
    [Id],[Description],[Category],[Deleted],[InsertedDate],[InsertedBy],[UpdatedDate],[UpdatedBy],[UpdateCount] 
FROM 
    [dbo].[Survey] 
/**where**/
");

                    builder.Where("Id = @Id", new { Id = id });

                    // /**orderby**/
                    //builder.OrderBy("Id, Category ASC");

                    var row = await dbConnection.QueryFirstOrDefaultAsync(selectSurvey.RawSql, selectSurvey.Parameters);

                    if (row != null)
                    {
                        ret = new SurveyDto
                        {
                            Id = row.Id.ToString(),
                            Description = row.Description,
                            Category = row.Category,
                            Deleted = (row.Deleted == "Y"),
                            InsertedDate = DateTimeExtensions.StringToDateTime(row.InsertedDate, DateTimeExtensions.DateFormat.yyyyMMdd),
                            InsertedBy = row.InsertedBy,
                            UpdatedDate = DateTimeExtensions.StringToDateTime(row.UpdatedDate, DateTimeExtensions.DateFormat.yyyyMMdd),
                            UpdatedBy = row.UpdatedBy,
                            UpdateCount = row.UpdateCount,
                        };
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            };

            return ret;
        }

        public async Task<IEnumerable<SurveyDto>> ListSurveys()
        {
            List<SurveyDto> ret = new List<SurveyDto>();

            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                try
                {
                    var builder = new SqlBuilder();
                    var selectSurvey = builder.AddTemplate(@"
SELECT 
    [Id],[Description],[Category],[Deleted],[InsertedDate],[InsertedBy],[UpdatedDate],[UpdatedBy],[UpdateCount] 
FROM 
    [dbo].[Survey]");

                    // /**where**/
                    //builder.Where("Code = @Code", new { Code = deviceId.Value.DeviceCode });

                    // /**orderby**/
                    //builder.OrderBy("Id, Category ASC");

                    var rows = await dbConnection.QueryAsync(selectSurvey.RawSql, selectSurvey.Parameters);

                    foreach (var row in rows)
                    {
                        ret.Add(new SurveyDto
                        {
                            Id = row.Id.ToString(),
                            Description = row.Description,
                            Category = row.Category,
                            Deleted = (row.Deleted == "Y"),
                            InsertedDate = DateTimeExtensions.StringToDateTime(row.InsertedDate, DateTimeExtensions.DateFormat.yyyyMMdd),
                            InsertedBy = row.InsertedBy,
                            UpdatedDate = DateTimeExtensions.StringToDateTime(row.UpdatedDate, DateTimeExtensions.DateFormat.yyyyMMdd),
                            UpdatedBy = row.UpdatedBy,
                            UpdateCount = row.UpdateCount,
                        });
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            };

            return ret;
        }

        public async Task<bool> SaveSurvey(SurveyDto survey)
        {
            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                try
                {
                    var builder = new SqlBuilder();
                    if (string.IsNullOrWhiteSpace(survey.Id))
                    {
                        //INSERT
                        var insertSurvey = builder.AddTemplate(@"
INSERT INTO 
    [dbo].[Survey] (Description, Category, InsertedDate, InsertedBy) 
VALUES 
    (@Description, @Category, @InsertedDate, @InsertedBy)");
                        
                        var parameters = new
                        {
                            Description = !string.IsNullOrWhiteSpace(survey.Description) ? survey.Description : string.Empty,
                            Category = !string.IsNullOrWhiteSpace(survey.Category) ? survey.Category : string.Empty,
                            InsertedDate = survey.InsertedDate != DateTime.MinValue ? survey.InsertedDate.ToDbDateString() : string.Empty,
                            InsertedBy = !string.IsNullOrWhiteSpace(survey.InsertedBy) ? survey.InsertedBy : string.Empty,
                        };

                        var res = await dbConnection.ExecuteAsync(insertSurvey.RawSql, parameters);
                        return (res > 0);
                    }
                    else
                    {
                        //UPDATE
                        var insertSurvey = builder.AddTemplate(@"
UPDATE 
    [dbo].[Survey] 
SET 
    Description = @Description, 
    Category = @Category, 
    InsertedDate = @InsertedDate, 
    InsertedBy = @InsertedBy, 
    UpdatedDate = @UpdatedDate, 
    UpdatedBy = @UpdatedBy
/**where**/
");
                        builder.Where("Id = @Id", new { Id = survey.Id });
                        
                        var parameters = new
                        {
                            Description = !string.IsNullOrWhiteSpace(survey.Description) ? survey.Description : string.Empty,
                            Category = !string.IsNullOrWhiteSpace(survey.Category) ? survey.Category : string.Empty,
                            InsertedDate = survey.InsertedDate != DateTime.MinValue ? survey.InsertedDate.ToDbDateString() : string.Empty,
                            InsertedBy = !string.IsNullOrWhiteSpace(survey.InsertedBy) ? survey.InsertedBy : string.Empty,
                            UpdatedDate = DateTime.Now.ToDbDateString(),
                            UpdatedBy = "KBO"
                        };

                        builder.AddParameters(parameters);

                        var res = await dbConnection.ExecuteAsync(insertSurvey.RawSql, insertSurvey.Parameters);
                        return (res > 0);
                    }
                    
                }
                catch (Exception ex)
                {
                    return false;
                }
            };
        }

        public async Task<bool> DeleteSurvey(string id)
        {
            bool ret = false;

            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                try
                {
                    var builder = new SqlBuilder();
                    var deleteSurvey = builder.AddTemplate(@"
DELETE FROM [dbo].[Survey]
/**where**/
");
                    builder.Where("Id = @Id", new { Id = id });

                    // /**orderby**/
                    //builder.OrderBy("Id, Category ASC");

                    var res = await dbConnection.ExecuteAsync(deleteSurvey.RawSql, deleteSurvey.Parameters);

                    return (res > 0);
                }
                catch (Exception ex)
                {
                    return false;
                }
            };
        }

        public static SurveyDto MapSurveyDto(dynamic row)
        {
            return new SurveyDto
            {
                Id = row.Id,
                Description = row.Description,
                Category = row.Category,
                Deleted = row.Deleted,
                InsertedDate = row.InsertedDate,
                InsertedBy = row.InsertedBy,
                UpdatedDate = row.UpdatedDate,
                UpdatedBy = row.UpdatedBy,
                UpdateCount = row.UpdateCount,

            };
        }
    }
}
