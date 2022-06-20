﻿using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;

namespace ImageToPuzzle.Infrastructure.Providers;

internal sealed class DeflateCompressionProvider : ICompressionProvider
{
	public string EncodingName => "deflate";

	public bool SupportsFlush => true;

	public Stream CreateStream(Stream outputStream)
	{
		return new DeflateStream(outputStream, CompressionLevel.Optimal);
	}
}