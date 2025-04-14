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
        var kps = new Kps(data.Notes.Max(p=>p.Tail) / 1000 + 1);
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
