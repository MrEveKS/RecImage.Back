using ImageToPuzzle.Models;
using System.Collections.Generic;

namespace ImageToPuzzle.Services
{
	public interface IGetImagesService
	{
		List<ImageListItem> GetList();
		int GetRandomId();
	}
}