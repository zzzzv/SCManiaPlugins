using StarRatingRebirth;

namespace ManiaBeatmap;

public class Kps(int length) : List<int>(Enumerable.Repeat(0, length))
{
    public static int DisplayItems { get; set; } = 10;

    public override string ToString()
    {
        return Count <= DisplayItems ? $"[{string.Join(",", this)}]" : $"[{string.Join(",", this[..DisplayItems])}...{Count}]";
    }

    public static Kps FromBeatmap(ManiaData data, bool countTail)
    {
        var last = Math.Max(data.Notes[^1].Head, data.Notes.Max(p => p.Tail));
        var kps = new Kps(last / 1000 + 1);
        foreach (var note in data.Notes) {
            kps[note.Head / 1000]++;
            if (countTail && note.IsLong)
            {
                kps[note.Tail / 1000]++;
            }
        }
        return kps;
    }
}
