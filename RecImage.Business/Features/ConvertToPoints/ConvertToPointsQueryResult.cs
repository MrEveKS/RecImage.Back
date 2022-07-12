namespace RecImage.Business.Features.ConvertToPoints;

public sealed class ConvertToPointsQueryResult
{
    public List<List<int>> Cells { get; set; }
    public Dictionary<int, string> CellsColor { get; set; }
}