using Dapper;
using WriteTogether.Dapper;

namespace WriteTogether.Features.Challenges
{
    public class ChallengesRepository
    {
        private readonly DbConnection _connection;

        public ChallengesRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<ChallengesModel>> GetAllChallenges()
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT
                    id AS ChallengeId,
                    title AS Title,
                    description AS Description,
                    challenge_type AS ChallengeType,
                    target_value AS TargetValue,
                    reward_points AS RewardPoints,
                    active AS Active,
                    start_date AS StartDate,
                    end_date AS EndDate,
                    created_at AS CreatedAt
                FROM challenges
                ORDER BY created_at DESC;
            ";

            return await connection.QueryAsync<ChallengesModel>(sql);
        }

        public async Task<ChallengesModel?> GetChallengeById(int challengeId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                SELECT
                    id AS ChallengeId,
                    title AS Title,
                    description AS Description,
                    challenge_type AS ChallengeType,
                    target_value AS TargetValue,
                    reward_points AS RewardPoints,
                    active AS Active,
                    start_date AS StartDate,
                    end_date AS EndDate,
                    created_at AS CreatedAt
                FROM challenges
                WHERE id = @ChallengeId;
            ";

            return await connection.QueryFirstOrDefaultAsync<ChallengesModel>(
                sql,
                new { ChallengeId = challengeId }
            );
        }

        public async Task<int> CreateChallenge(ChallengesModelRequest challenge)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                INSERT INTO challenges (
                    title,
                    description,
                    challenge_type,
                    target_value,
                    reward_points,
                    active,
                    start_date,
                    end_date,
                    created_at
                )
                VALUES (
                    @Title,
                    @Description,
                    @ChallengeType,
                    @TargetValue,
                    @RewardPoints,
                    @Active,
                    @StartDate,
                    @EndDate,
                    NOW()
                )
                RETURNING id;
            ";

            return await connection.ExecuteScalarAsync<int>(sql, challenge);
        }

        public async Task<int> UpdateChallenge(ChallengesModel challenge)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                UPDATE challenges
                SET
                    title = @Title,
                    description = @Description,
                    challenge_type = @ChallengeType,
                    target_value = @TargetValue,
                    reward_points = @RewardPoints,
                    active = @Active,
                    start_date = @StartDate,
                    end_date = @EndDate
                WHERE id = @ChallengeId;
            ";

            return await connection.ExecuteAsync(sql, challenge);
        }

        public async Task<int> DeleteChallenge(int challengeId)
        {
            using var connection = _connection.CreateConnection();

            var sql = @"
                DELETE FROM challenges
                WHERE id = @ChallengeId;
            ";

            return await connection.ExecuteAsync(sql, new
            {
                ChallengeId = challengeId
            });
        }

        public async Task<IEnumerable<ChallengesModel>>
GetActiveChallengesByType(string type)
        {
            using var connection =
                _connection.CreateConnection();

            var sql = @"
        SELECT
            id AS ChallengeId,
            title AS Title,
            description AS Description,
            challenge_type AS ChallengeType,
            target_value AS TargetValue,
            reward_points AS RewardPoints,
            active AS Active,
            start_date AS StartDate,
            end_date AS EndDate,
            created_at AS CreatedAt

        FROM challenges

        WHERE active = true
        AND challenge_type = @Type

        AND (
            start_date IS NULL
            OR start_date <= NOW()
        )

        AND (
            end_date IS NULL
            OR end_date >= NOW()
        );
    ";

            return await connection.QueryAsync<
                ChallengesModel
            >(sql, new { Type = type });
        }
    }
}