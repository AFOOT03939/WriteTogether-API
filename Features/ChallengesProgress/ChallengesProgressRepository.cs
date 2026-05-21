using Dapper;
using WriteTogether.Dapper;
using WriteTogether.Features.ChallengesProgress.WriteTogether.Features.ChallengesProgress;

namespace WriteTogether.Features.ChallengesProgress
{
    public class ChallengesProgressRepository
    {
        private readonly DbConnection _connection;

        public ChallengesProgressRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<ChallengesProgressModel>>
            GetUserChallengesProgress(int userId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT
                    ucp.id AS ProgressId,
                    ucp.user_id AS UserId,
                    ucp.challenge_id AS ChallengeId,
                    ucp.current_progress AS CurrentProgress,
                    ucp.completed AS Completed,
                    ucp.completed_at AS CompletedAt,
                    ucp.created_at AS CreatedAt,

                    c.title AS ChallengeTitle,
                    c.target_value AS TargetValue,
                    c.reward_points AS RewardPoints,
                    c.challenge_type AS ChallengeType,
                    c.active AS Active

                FROM user_challenge_progress ucp

                INNER JOIN challenges c
                    ON ucp.challenge_id = c.id

                WHERE ucp.user_id = @UserId

                ORDER BY c.created_at DESC;
            ";

            return await connection.QueryAsync<ChallengesProgressModel>(
                sql,
                new { UserId = userId }
            );
        }

        public async Task<ChallengesProgressModel?>
            GetChallengeProgress(int userId, int challengeId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT
                    id AS ProgressId,
                    user_id AS UserId,
                    challenge_id AS ChallengeId,
                    current_progress AS CurrentProgress,
                    completed AS Completed,
                    completed_at AS CompletedAt,
                    created_at AS CreatedAt

                FROM user_challenge_progress

                WHERE user_id = @UserId
                AND challenge_id = @ChallengeId;
            ";

            return await connection
                .QueryFirstOrDefaultAsync<ChallengesProgressModel>(
                    sql,
                    new
                    {
                        UserId = userId,
                        ChallengeId = challengeId
                    }
                );
        }

        public async Task<int> CreateProgress(
            ChallengesProgressRequest progress
        )
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO user_challenge_progress (
                    user_id,
                    challenge_id,
                    current_progress,
                    completed,
                    created_at
                )
                VALUES (
                    @UserId,
                    @ChallengeId,
                    @CurrentProgress,
                    false,
                    NOW()
                )
                RETURNING id;
            ";

            return await connection.ExecuteScalarAsync<int>(
                sql,
                progress
            );
        }

        public async Task<int> UpdateProgress(
            int userId,
            int challengeId,
            int currentProgress,
            bool completed
        )
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                UPDATE user_challenge_progress
                SET
                    current_progress = @CurrentProgress,
                    completed = @Completed,
                    completed_at = CASE
                        WHEN @Completed = true
                        THEN NOW()
                        ELSE NULL
                    END
                WHERE user_id = @UserId
                AND challenge_id = @ChallengeId;
            ";

            return await connection.ExecuteAsync(sql, new
            {
                UserId = userId,
                ChallengeId = challengeId,
                CurrentProgress = currentProgress,
                Completed = completed
            });
        }

        public async Task<int> DeleteProgress(int progressId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                DELETE FROM user_challenge_progress
                WHERE id = @ProgressId;
            ";

            return await connection.ExecuteAsync(sql, new
            {
                ProgressId = progressId
            });
        }
    }
}