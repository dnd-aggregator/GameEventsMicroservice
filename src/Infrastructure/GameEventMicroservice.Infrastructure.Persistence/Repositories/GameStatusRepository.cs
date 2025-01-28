using GameEventMicroservice.Application.Abstractions.Persistence.Repositories;
using GameEventMicroservice.Application.Models;
using Itmo.Dev.Platform.Persistence.Abstractions.Commands;
using Itmo.Dev.Platform.Persistence.Abstractions.Connections;
using System.Data;
using System.Data.Common;

namespace GameEventMicroservice.Infrastructure.Persistence.Repositories;

public class GameStatusRepository : IGameStatusRepository
{
    private readonly IPersistenceConnectionProvider _connectionProvider;

    public GameStatusRepository(IPersistenceConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task AddOrUpdateAsync(IReadOnlyCollection<Game> games, CancellationToken cancellationToken)
    {
        const string sql = """
                            insert into games (game_id, game_state)
                            select * from unnest(@ids, @states)on conflict (game_id)
                            do update set game_state = excluded.game_state;
                           """;
        const string charactersSql = """
                                     insert into game_characters (game_id, character_id)
                                     select * from unnest(@gameIds, @characterIds)
                                     on conflict do nothing;
                                     """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using (IPersistenceCommand gameCommand = connection.CreateCommand(sql)
                         .AddParameter("@ids", games.Select(x => x.Id))
                         .AddParameter("@states", games.Select(x => (int)x.Status)))
        {
            await gameCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        long[] gameIdList = games.SelectMany(g => g.CharactersIds.Select(c => g.Id)).ToArray();
        long[] characterIdList = games.SelectMany(g => g.CharactersIds).ToArray();

        if (gameIdList.Length > 0)
        {
            await using IPersistenceCommand characterCommand = connection.CreateCommand(charactersSql)
                .AddParameter("@gameIds", gameIdList)
                .AddParameter("@characterIds", characterIdList);

            await characterCommand.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    public async Task<Game?> GetAsync(long id, CancellationToken cancellationToken)
    {
        const string gameSql = """
                               SELECT *
                               FROM games
                               WHERE game_id = @game_id;
                               """;

        const string charactersSql = """
                                     SELECT *
                                     FROM game_characters
                                     WHERE game_id = @game_id;
                                     """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand gameCommand = connection.CreateCommand(gameSql)
            .AddParameter("@game_id", id);

        await using DbDataReader gameReader = await gameCommand.ExecuteReaderAsync(cancellationToken);
        long gameId = 0;
        GameStatus gameStatus = GameStatus.Draft;
        while (await gameReader.ReadAsync(cancellationToken))
        {
            gameId = gameReader.GetInt64(0);
            gameStatus = (GameStatus)gameReader.GetInt32(1);
        }

        await gameReader.CloseAsync();

        await using IPersistenceCommand charactersCommand = connection.CreateCommand(charactersSql)
            .AddParameter("@game_id", id);

        var characterIds = new List<long>();
        await using DbDataReader charactersReader = await charactersCommand.ExecuteReaderAsync(cancellationToken);
        while (await charactersReader.ReadAsync(cancellationToken))
        {
            characterIds.Add(charactersReader.GetInt64(0));
        }

        var game = new Game(gameId, gameStatus, characterIds);
        return game;
    }
}