using Dapper;
using Microsoft.Data.SqlClient;
using Survey.Server.Services;
using Survey.Shared.Models;
using System.Data;
using System.Text;
using System;

namespace Survey.Server.Infrastructure
{
    public class DbQuestionDataService : IQuestionDataService
    {
        private readonly IConfiguration _configuration;

        public DbQuestionDataService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<QuestionDto> GetQuestion(string id)
        {
            QuestionDto ret = null;

            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                try
                {
                    /*
                     CREATE TABLE Question (
                        Id int IDENTITY(1,1) PRIMARY KEY,
                        SurveyId int NOT NULL FOREIGN KEY REFERENCES Survey(Id),
	                    Text varchar(255) NOT NULL,
                        IsMultipleChoise char(1) NOT NULL DEFAULT 'N',
	                    Deleted char(1) NOT NULL DEFAULT 'N',
	                    UpdateCount int NOT NULL DEFAULT 0
                    ) 
                    */
                    var builder = new SqlBuilder();
                    var selectSurvey = builder.AddTemplate(@"
SELECT 
    [Id],[SurveyId],[Text],[IsMultipleChoise],[Deleted],[UpdateCount] 
FROM 
    [dbo].[Question] 
/**where**/
");

                    builder.Where("Id = @Id", new { Id = id });

                    // /**orderby**/
                    //builder.OrderBy("Id, Category ASC");

                    var row = await dbConnection.QueryFirstOrDefaultAsync(selectSurvey.RawSql, selectSurvey.Parameters);

                    if (row != null)
                    {
                        ret = new QuestionDto
                        {
                            Id = row.Id.ToString(),
                            SurveyId = row.SurveyId.ToString(),
                            Text = row.Text,
                            IsMultipleChoise = (row.IsMultipleChoise == "Y"),
                            Deleted = (row.Deleted == "Y"),
                            UpdateCount = row.UpdateCount,
                        };
                    }
                }
                catch
                {
                    throw;
                }
            };

            return ret;
        }

        public async Task<IEnumerable<QuestionDto>> ListQuestions(string surveyId)
        {
            List<QuestionDto> ret = new List<QuestionDto>();

            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                try
                {
                    var builder = new SqlBuilder();
                    var selectSurvey = builder.AddTemplate(@"
SELECT 
    [Id],[SurveyId],[Text],[IsMultipleChoise],[Deleted],[UpdateCount] 
FROM 
    [dbo].[Question]
/**where**/
/**orderby**/
");
                    builder.Where("SurveyId = @SurveyId", new { SurveyId = surveyId });

                    builder.OrderBy("SurveyId, Id ASC");

                    var rows = await dbConnection.QueryAsync(selectSurvey.RawSql, selectSurvey.Parameters);

                    foreach (var row in rows)
                    {
                        ret.Add(new QuestionDto
                        {
                            Id = row.Id.ToString(),
                            SurveyId = row.SurveyId.ToString(),
                            Text = row.Text,
                            IsMultipleChoise = (row.IsMultipleChoise == "Y"),
                            Deleted = (row.Deleted == "Y"),
                            UpdateCount = row.UpdateCount                            
                        });
                    }
                }
                catch
                {
                    throw;
                }
            };

            return ret;
        }

        public async Task<IEnumerable<AlternativeDto>> ListAlternatives(string questionId)
        {
            List<AlternativeDto> ret = new List<AlternativeDto>();

            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                try
                {
                    var builder = new SqlBuilder();
                    var selectSurvey = builder.AddTemplate(@"
SELECT 
    [Id],[SurveyId],[QuestionId],[Text],[IsCorrect],[UpdateCount] 
FROM 
    [dbo].[Alternative]
/**where**/
/**orderby**/
");
                    builder.Where("QuestionId = @QuestionId", new { QuestionId = questionId });

                    builder.OrderBy("QuestionId, Id ASC");

                    var rows = await dbConnection.QueryAsync(selectSurvey.RawSql, selectSurvey.Parameters);

                    foreach (var row in rows)
                    {
                        ret.Add(new AlternativeDto
                        {
                            Id = row.Id.ToString(),
                            SurveyId = row.SurveyId.ToString(),
                            QuestionId = row.QuestionId.ToString(),
                            Text = row.Text,
                            IsCorrect = (row.IsCorrect == "Y"),
                            UpdateCount = row.UpdateCount,
                        });
                    }
                }
                catch
                {
                    throw;
                }
            };

            return ret;
        }


        public async Task<QuestionDto> SaveQuestion(QuestionDto question)
        {
            QuestionDto ret = null;

            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                try
                {                    
                    if (string.IsNullOrWhiteSpace(question.Id))
                    {
                        //INSERT
                        const string sql = @"
DECLARE @InsertedRows AS TABLE (Id int);
INSERT INTO 
    [dbo].[Question] (SurveyId, Text, IsMultipleChoise) 
OUTPUT Inserted.Id INTO  @InsertedRows
VALUES 
    (@SurveyId, 
    @Text, 
    @IsMultipleChoise)
SELECT Id FROM @InsertedRows";

                        var serialNo = dbConnection.Query<int>(sql, new
                        {
                            SurveyId = !string.IsNullOrWhiteSpace(question.SurveyId) ? question.SurveyId : string.Empty,
                            Text = !string.IsNullOrWhiteSpace(question.Text) ? question.Text : string.Empty,
                            IsMultipleChoise = question.IsMultipleChoise ? "Y" : "N",
                        }).Single();

                        if (serialNo > 0)
                        {
                            ret = await GetQuestion(serialNo.ToString());
                        }
                    }
                    else
                    {
                        var builder = new SqlBuilder();
                        //UPDATE
                        var insertQuestion = builder.AddTemplate(@"
UPDATE 
    [dbo].[Question] 
SET 
    Text = @Text, 
    IsMultipleChoise = @IsMultipleChoise,
    UpdateCount = UpdateCount + 1
/**where**/
");
                        builder.Where("Id = @Id", new { Id = question.Id });
                        builder.Where("UpdateCount = @UpdateCount", new { UpdateCount = question.UpdateCount });

                        builder.AddParameters(new
                        {
                            Text = !string.IsNullOrWhiteSpace(question.Text) ? question.Text : string.Empty,
                            IsMultipleChoise = question.IsMultipleChoise ? "Y" : "N",                            
                        });

                        var res = await dbConnection.ExecuteAsync(insertQuestion.RawSql, insertQuestion.Parameters);

                        ret = await GetQuestion(question.Id);
                    }

                }
                catch
                {
                    throw;
                }
            };

            return ret;
        }

        private async Task<bool> SaveAlternatives(List<AlternativeDto> alts)
        {
            bool ret = false;

            foreach (var a in alts)
            {
                if (await SaveAlternative(a) == false)
                {
                    return false;
                }
            }

            return ret;
        }

        public async Task<bool> SaveAlternative(AlternativeDto alt)
        {
            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                try
                {
                    var builder = new SqlBuilder();
                    if (string.IsNullOrWhiteSpace(alt.Id))
                    {
                        //INSERT
                        var insertQuestion = builder.AddTemplate(@"
INSERT INTO 
    [dbo].[Alternative] (SurveyId, QuestionId, Text, IsCorrect) 
VALUES 
    (@SurveyId, @QuestionId, @Text, @IsCorrect)");

                        builder.AddParameters(new
                        {
                            SurveyId = !string.IsNullOrWhiteSpace(alt.SurveyId) ? alt.SurveyId : string.Empty,
                            QuestionId = !string.IsNullOrWhiteSpace(alt.QuestionId) ? alt.QuestionId : string.Empty,
                            Text = !string.IsNullOrWhiteSpace(alt.Text) ? alt.Text : string.Empty,
                            IsCorrect = alt.IsCorrect ? "Y" : "N",
                        });

                        var res = await dbConnection.ExecuteAsync(insertQuestion.RawSql, insertQuestion.Parameters);
                        return (res > 0);
                    }
                    else
                    {
                        //UPDATE
                        var insertQuestion = builder.AddTemplate(@"
UPDATE 
    [dbo].[Alternative] 
SET 
    Text = @Text, 
    IsCorrect = @IsCorrect  
/**where**/
");
                        builder.Where("Id = @Id", new { Id = alt.Id });

                        builder.AddParameters(new
                        {
                            Text = !string.IsNullOrWhiteSpace(alt.Text) ? alt.Text : string.Empty,
                            IsCorrect = alt.IsCorrect ? "Y" : "N",
                        });

                        var res = await dbConnection.ExecuteAsync(insertQuestion.RawSql, insertQuestion.Parameters);
                        return (res > 0);
                    }

                }
                catch
                {
                    throw;
                }
            };
        }

        public async Task<bool> DeleteQuestion(string id)
        {
            bool ret = false;
            try
            {
                if (await (DeleteAlternatives(id)))
                {
                    using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
                    {
                        try
                        {
                            var builder = new SqlBuilder();
                            var deleteSurvey = builder.AddTemplate(@"
DELETE FROM [dbo].[Question]
/**where**/
");
                            builder.Where("Id = @Id", new { Id = id });

                            // /**orderby**/
                            //builder.OrderBy("Id, Category ASC");

                            var res = await dbConnection.ExecuteAsync(deleteSurvey.RawSql, deleteSurvey.Parameters);

                            return (res > 0);
                        }
                        catch
                        {
                            throw;
                        }
                    };
                }
            }
            catch
            {
                throw;
            }
            return ret;
        }

        private async Task<bool> DeleteAlternatives(string questionId)
        {
            bool ret = false;

            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                try
                {
                    var builder = new SqlBuilder();
                    var deleteSurvey = builder.AddTemplate(@"
DELETE FROM [dbo].[Alternative]
/**where**/
");
                    builder.Where("QuestionId = @Id", new { Id = questionId });

                    // /**orderby**/
                    //builder.OrderBy("Id, Category ASC");

                    var res = await dbConnection.ExecuteAsync(deleteSurvey.RawSql, deleteSurvey.Parameters);

                    return (res >= 0);
                }
                catch
                {
                    throw;
                }
            };
        }

        public async Task<bool> DeleteAlternative(string altId)
        {
            bool ret = false;

            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                try
                {
                    var builder = new SqlBuilder();
                    var deleteSurvey = builder.AddTemplate(@"
DELETE FROM [dbo].[Alternative]
/**where**/
");
                    builder.Where("Id = @Id", new { Id = altId });

                    // /**orderby**/
                    //builder.OrderBy("Id, Category ASC");

                    var res = await dbConnection.ExecuteAsync(deleteSurvey.RawSql, deleteSurvey.Parameters);

                    return (res > 0);
                }
                catch
                {
                    throw;
                }
            };
        }

        
    }
}
