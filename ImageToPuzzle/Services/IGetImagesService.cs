using System.Collections.Generic;
using ImageToPuzzle.Models;

namespace ImageToPuzzle.Services;

public interface IGetImagesService
{
	List<ImageListItem> GetList();

	int GetRandomId();
}