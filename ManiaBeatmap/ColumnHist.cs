using Newtonsoft.Json;
using StarRatingRebirth;

namespace ManiaBeatmap;

public class ColumnHist(int cols)
{
    [JsonProperty("notes")]
    public int[] Notes { get; set; } = new int[cols];

    [JsonProperty("holds")]
    public int[] Holds { get; set; } = new int[cols];

    public override string ToString()
    {
        return $"{{notes: [{string.Join(",", Notes)}], holds: [{string.Join(",", Holds)}]}}";
    }

    public static ColumnHist FromBeatmap(ManiaData data)
    {
        var hist = new ColumnHist(data.CS);
        foreach (var note in data.Notes)
        {
            if (note.IsLong)
            {
                hist.Holds[note.Key]++;
            }
            else
            {
                hist.Notes[note.Key]++;
            }
        }
        return hist;
    }
}
