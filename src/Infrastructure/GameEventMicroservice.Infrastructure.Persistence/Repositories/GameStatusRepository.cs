using GameEventMicroservice.Application.Abstractions.Persistence.Repositories;
using GameEventMicroservice.Application.Models;
using Itmo.Dev.Platform.Persistence.Abstractions.Commands;
using Itmo.Dev.Platform.Persistence.Abstractions.Connections;
using System.Data.Common;

namespace GameEventMicroservice.Infrastructure.Persistence.Repositories;

public class GameStatusRepository : IGameStatusRepository
{
    private readonly IPersistenceConnectionProvider _connectionProvider;

    public GameStatusRepository(IPersistenceConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
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
            characterIds.Add(charactersReader.GetInt64(1));
        }

        var game = new Game(gameId, gameStatus, characterIds);
        return game;
    }

    public async Task AddAsync(Game game, CancellationToken cancellationToken)
    {
        const string sql = """
                     insert into games (game_id, game_state)
                     VALUES (@id, @state)
                     on conflict do nothing
                     """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("@id", game.Id)
            .AddParameter("@state", (int)game.Status);

        await command.ExecuteNonQueryAsync(cancellationToken);

        await connection.DisposeAsync();

        await command.DisposeAsync();

        foreach (long characterId in game.CharactersIds)
        {
            string sqlCharacter = """
                         insert into game_characters (game_id, character_id)
                         VALUES (@game_id, @character_id)
                         on conflict do nothing
                         """;
            await using IPersistenceConnection connect = await _connectionProvider.GetConnectionAsync(cancellationToken);
            await using IPersistenceCommand characterCommand = connect.CreateCommand(sqlCharacter)
                .AddParameter("@game_id", game.Id)
                .AddParameter("@character_id", characterId);
            await characterCommand.ExecuteNonQueryAsync(cancellationToken);
            await connect.DisposeAsync();
            await command.DisposeAsync();
        }
    }

    public async Task UpdateAsync(Game game, CancellationToken cancellationToken)
    {
        const string updateGameSql = """
                                     UPDATE games
                                     SET game_state = @state
                                     WHERE game_id = @id;
                                     """;

        const string deleteCharactersSql = """
                                           DELETE FROM game_characters
                                           WHERE game_id = @id;
                                           """;

        const string insertCharacterSql = """
                                          INSERT INTO game_characters (game_id, character_id)
                                          VALUES (@game_id, @character_id);
                                          """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand updateGameCommand = connection.CreateCommand(updateGameSql)
            .AddParameter("@id", game.Id)
            .AddParameter("@state", (int)game.Status);

        await updateGameCommand.ExecuteNonQueryAsync(cancellationToken);

        await using IPersistenceCommand deleteCharactersCommand = connection.CreateCommand(deleteCharactersSql)
            .AddParameter("@id", game.Id);

        await deleteCharactersCommand.ExecuteNonQueryAsync(cancellationToken);

        foreach (long characterId in game.CharactersIds)
        {
            await using IPersistenceCommand insertCharacterCommand = connection.CreateCommand(insertCharacterSql)
                .AddParameter("@game_id", game.Id)
                .AddParameter("@character_id", characterId);

            await insertCharacterCommand.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}