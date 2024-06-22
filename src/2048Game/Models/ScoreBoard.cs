using System.Text.Json;
using System.Text.Json.Serialization;

namespace _2048Game.Models;

[JsonConverter(typeof(Converter))]
public sealed class ScoreBoard
{
    public int Score { get; private set; }
    public int BestScore { get; private set; }

    public void AddScore(int score)
    {
        if (score < 0)
        {
            throw new ArgumentException("Score should be greater than zero.");
        }

        Score += score;
        if (Score > BestScore)
        {
            BestScore = Score;
        }
    }

    public void Reset()
    {
        Score = 0;
    }

    public void ForceOverrideScore(int score)
    {
        Score = score;
        if (Score > BestScore)
        {
            BestScore = Score;
        }
    }

    private class Converter : JsonConverter<ScoreBoard>
    {
        public override ScoreBoard Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var scoreBoard = new ScoreBoard();
            while (reader.Read())
            {
                if (reader.TokenType is JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType is not JsonTokenType.PropertyName)
                {
                    continue;
                }

                var propertyName = reader.GetString();
                reader.Read();
                switch (propertyName)
                {
                    case nameof(Score):
                        scoreBoard.Score = reader.GetInt32();
                        break;
                    case nameof(BestScore):
                        scoreBoard.BestScore = reader.GetInt32();
                        break;
                }
            }

            return scoreBoard;
        }

        public override void Write(Utf8JsonWriter writer, ScoreBoard value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber(nameof(Score), value.Score);
            writer.WriteNumber(nameof(BestScore), value.BestScore);
            writer.WriteEndObject();
        }
    }
}