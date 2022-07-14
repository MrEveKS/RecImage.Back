namespace RecImage.Business.Features.ConvertToPointsById;

public sealed record ConvertToPointsByIdQueryResult(List<List<int>> Cells, Dictionary<int, string> CellsColor);