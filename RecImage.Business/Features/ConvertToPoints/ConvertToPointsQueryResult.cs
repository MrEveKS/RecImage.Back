namespace RecImage.Business.Features.ConvertToPoints;

public sealed record ConvertToPointsQueryResult(List<List<int>> Cells, Dictionary<int, string> CellsColor);