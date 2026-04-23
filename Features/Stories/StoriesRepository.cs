using Dapper;
using Microsoft.Win32;
using WriteTogether.Dapper;
using WriteTogether.Features.Ratings;

namespace WriteTogether.Features.Stories
{
    public class StoriesRepository
    {
        public readonly DbConnection _connection;
        public StoriesRepository(DbConnection connection)
        {
            _connection = connection;
        }
        /*
        public async Task<IEnumerable<StoriesModel>> GetAllStories()
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT 
                    id AS StoryId,
                    title AS Title,
                    description AS Description,
                    creator_user_id AS UserId,
                    status AS Status,
                    visibility AS Visibility,
                    image_url AS ImageUrl,
                    created_at AS CreatedAt
                FROM stories
                INNER JOIN tags
                ON 
                ;
                ";

            var result = await connection.QueryAsync<StoriesModel>(sql);

            return result;

        }

        */

        public async Task<IEnumerable<StoriesModel>> GetAllStories()
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT 
                    s.id AS StoryId,
                    s.title AS Title,
                    s.description AS Description,
                    s.image_url AS ImageUrl,
                    s.created_at AS CreatedAt,
                    s.status AS Status,
                    s.visibility AS Visibility,
                    u.username AS AuthorName,
                    STRING_AGG(c.name, ', ') AS Categories,
                    FLOOR(AVG(r.rating_value)) AS Rating
                FROM stories s
                INNER JOIN users u 
                    ON s.creator_user_id = u.id

                LEFT JOIN story_categories sc 
                    ON s.id = sc.story_id

                LEFT JOIN categories c 
                    ON sc.category_id = c.id
                
                LEFT JOIN ratings r
                    ON s.id = r.story_id
                
                GROUP BY 
                    s.id,
                    s.title,
                    s.description,
                    s.status,
                    s.image_url,
                    s.created_at,
                    u.username,
                    s.visibility
                    ";

            var result = await connection.QueryAsync<StoriesModel>(sql);

            return result;

        }

        public async Task<IEnumerable<StoriesModel>> GetStories(string? status, int? categoryId)
        {
            using var connection = _connection.CreateConnection();

                var sql = @"
                    SELECT 
                        s.id AS StoryId,
                        s.title AS Title,
                        s.description AS Description,
                        s.image_url AS ImageUrl,
                        s.created_at AS CreatedAt,
                        s.status AS Status,
                        s.visibility AS Visibility,
                        u.username AS AuthorName,
                        STRING_AGG(c.name, ', ') AS Categories,
                        STRING_AGG(CAST(c.id AS TEXT), ',') AS CategoryIds,
                        FLOOR(AVG(r.rating_value)) AS Rating
                    FROM stories s
                    INNER JOIN users u 
                        ON s.creator_user_id = u.id
                    LEFT JOIN story_categories sc 
                        ON s.id = sc.story_id
                    LEFT JOIN categories c 
                        ON sc.category_id = c.id
                    LEFT JOIN ratings r 
                        ON s.id = r.story_id
                    WHERE 1=1 ";

                        if (!string.IsNullOrEmpty(status))
                        {
                            sql += " AND s.status = @Status ";
                        }

                        if (categoryId.HasValue)
                        {
                            sql += @" AND EXISTS (
                                SELECT 1 
                                FROM story_categories sc2 
                                WHERE sc2.story_id = s.id AND sc2.category_id = @CategoryId
                                ) ";
                        }
                        sql += @"
                    GROUP BY 
                        s.id,
                        s.title,
                        s.description,
                        s.status,
                        s.image_url,
                        s.created_at,
                        u.username,
                        s.visibility
                ";

            var result = await connection.QueryAsync<StoriesModel>(sql, new { Status = status, CategoryId = categoryId });

            return result;

        }

        public async Task<bool> CreateStoryTag(int storyId, int tagId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO story_tags (story_id, tag_id)
                VALUES (@StoryId, @TagId);
                ";

            var parameters = new
            {
                StoryId = storyId,
                TagId = tagId
            };

            var result = await connection.ExecuteAsync(sql, parameters);
            return result > 0;

        }

        public async Task<int> DeleteStory(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                DELETE FROM stories
                WHERE id = @StoryId
            ";

            var result = await connection.ExecuteAsync(sql, new
            {
                StoryId = storyId
            });

            return result;
        }

        public async Task<int> DeleteTagFromStory(int storyId, int tagId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                DELETE FROM story_tags
                WHERE story_id = @StoryId AND tag_id = @TagId
            ";

            var result = await connection.ExecuteAsync(sql, new
            {
                StoryId = storyId,
                TagId = tagId
            });

            return result;
        }
        /*
        public async Task<StoriesModel?> GetStoryById(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT 
                    id AS StoryId,
                    title AS Title,
                    description AS Description,
                    creator_user_id AS UserId,
                    status AS Status,
                    visibility AS Visibility,
                    image_url AS ImageUrl,
                    created_at AS CreatedAt
                FROM stories
                WHERE id = @StoryId;
            ";

            return await connection.QueryFirstOrDefaultAsync<StoriesModel>(sql, new
            {
                StoryId = storyId
            });
        }
        */

        public async Task<StoriesModel?> GetStoryById(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                    SELECT 
                        s.id AS StoryId,
                        s.title AS Title,
                        s.description AS Description,
                        s.creator_user_id AS UserId,
                        s.status AS Status,
                        s.visibility AS Visibility,
                        s.image_url AS ImageUrl,
                        s.created_at AS CreatedAt,
                        u.username AS AuthorName,
                        STRING_AGG(c.name, ', ') AS Categories,
                        STRING_AGG(CAST(c.id AS TEXT), ',') AS CategoryIds,
                        FLOOR(AVG(r.rating_value)) AS Rating
                    FROM stories s
                    INNER JOIN users u 
                        ON s.creator_user_id = u.id

                    LEFT JOIN story_categories sc 
                        ON s.id = sc.story_id

                    LEFT JOIN categories c 
                        ON sc.category_id = c.id

                    LEFT JOIN ratings r
                        ON s.id = r.story_id

                    WHERE s.id = @StoryId

                    GROUP BY 
                        s.id,
                        s.title,
                        s.description,
                        s.creator_user_id,
                        s.status,
                        s.visibility,
                        s.image_url,
                        s.created_at,
                        u.username
                ";

            return await connection.QueryFirstOrDefaultAsync<StoriesModel>(sql, new
            {
                StoryId = storyId
            });
        }

        public async Task<StoriesModel?> GetStoryByUser(int userId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                    SELECT 
                        s.id AS StoryId,
                        s.title AS Title,
                        s.description AS Description,
                        s.creator_user_id AS UserId,
                        s.status AS Status,
                        s.visibility AS Visibility,
                        s.image_url AS ImageUrl,
                        s.created_at AS CreatedAt,
                        u.username AS AuthorName,
                        STRING_AGG(c.name, ', ') AS Categories,
                        STRING_AGG(CAST(c.id AS TEXT), ',') AS CategoryIds,
                        FLOOR(AVG(r.rating_value)) AS Rating
                    FROM stories s
                    INNER JOIN users u 
                        ON s.creator_user_id = u.id

                    LEFT JOIN story_categories sc 
                        ON s.id = sc.story_id

                    LEFT JOIN categories c 
                        ON sc.category_id = c.id

                    LEFT JOIN ratings r
                        ON s.id = r.story_id

                    WHERE s.creator_user_id = @UserId

                    GROUP BY 
                        s.id,
                        s.title,
                        s.description,
                        s.creator_user_id,
                        s.status,
                        s.visibility,
                        s.image_url,
                        s.created_at,
                        u.username
                ";

            return await connection.QueryFirstOrDefaultAsync<StoriesModel>(sql, new
            {
                UserId = userId
            });
        }

        public async Task<int> UpdateStoryImage(int storyId, string imageUrl)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                    UPDATE stories
                    SET image_url = @ImageUrl
                    WHERE id = @StoryId;
                ";

            return await connection.ExecuteAsync(sql, new
            {
                StoryId = storyId,
                ImageUrl = imageUrl
            });
        }

        public async Task<int> CreateStory(StoriesModelRequest story)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO stories (
                    title,
                    description,
                    creator_user_id,
                    status,
                    visibility,
                    image_url,
                    created_at
                )
                VALUES (
                    @Title,
                    @Description,
                    @UserId,
                    @Status,
                    @Visibility,
                    @ImageUrl,
                    NOW()
                )
                RETURNING id;
            ";

            var storyId = await connection.ExecuteScalarAsync<int>(sql, new
            {
                story.Title,
                story.Description,
                story.UserId,
                story.Status,
                story.Visibility,
                story.ImageUrl,
            });

            return storyId;
        }

        public async Task<int> UpdateStory(StoriesModel story)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                UPDATE stories
                SET 
                    title = @Title,
                    description = @Description,
                    status = @Status,
                    visibility = @Visibility,
                    image_url = @ImageUrl
                WHERE id = @StoryId;
            ";

            var result = await connection.ExecuteAsync(sql, new
            {
                story.StoryId,
                story.Title,
                story.Description,
                story.Status,
                story.Visibility,
                story.ImageUrl
            });

            return result;
        }

        public async Task<int> InsertStoryCategory(int storyId, int categoryId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO story_categories (story_id, category_id)
                VALUES (@StoryId, @CategoryId);
            ";

            return await connection.ExecuteAsync(sql, new
            {
                StoryId = storyId,
                CategoryId = categoryId
            });
        }

        public async Task<int> DeleteStoryCategories(int storyId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                DELETE FROM story_categories
                WHERE story_id = @StoryId;
            ";


            return await connection.ExecuteAsync(sql, new
            {
                StoryId = storyId
            });
        }

    }
}
