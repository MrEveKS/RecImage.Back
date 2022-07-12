namespace RecImage.Business.Features.ConvertToPointsById;

public sealed class ConvertToPointsByIdQueryResult
{
    public List<List<int>> Cells { get; set; }
    public Dictionary<int, string> CellsColor { get; set; }
}