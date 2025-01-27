using FluentMigrator;
using Itmo.Dev.Platform.Persistence.Postgres.Migrations;

namespace GameEventMicroservice.Infrastructure.Persistence.Migrations;

[Migration(1731949850, "CreateGamesAndGameCharactersTables")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
        """
        create type game_state as enum
        (
            'Scheduled',
            'Started',
            'Finished'
        );

        create table games
        (
            game_id bigint primary key not null,
            game_state game_state not null
        );

        create table game_characters
        (
            game_id bigint not null,
            character_id bigint not null,
            primary key (game_id, character_id),
            foreign key (game_id) references games(game_id) on delete cascade
        );
        """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
        """
        drop table game_characters;
        drop table games;
        drop type game_state;
        """;
}