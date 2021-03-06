﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using image_bclim.Properties;
using KuriimuContract;

namespace image_bclim
{
	public class BclimAdapter : IImageAdapter
	{
		private FileInfo _fileInfo = null;
		private Bitmap _bclim = null;
		private List<Bitmap> _bitmaps = null;

		#region Properties

		// Information
		public string Name => Settings.Default.PluginName;

		public string Description => "Binary CLayout Image";

		public string Extension => "*.bclim";

		public string About => "This is the BCLIM file adapter for Kukkii.";

		// Feature Support
		public bool FileHasExtendedProperties => false;

		public bool CanSave => false;

		public FileInfo FileInfo
		{
			get
			{
				return _fileInfo;
			}
			set
			{
				_fileInfo = value;
			}
		}

		#endregion

		public bool Identify(string filename)
		{
			using (var br = new BinaryReaderX(File.OpenRead(filename)))
			{
				if (br.BaseStream.Length < 40) return false;
				br.BaseStream.Seek((int)br.BaseStream.Length - 40, SeekOrigin.Begin);
				return br.ReadString(4) == "CLIM";
			}
		}

		public LoadResult Load(string filename)
		{
			LoadResult result = LoadResult.Success;

			_fileInfo = new FileInfo(filename);
			_bitmaps = null;

			if (_fileInfo.Exists)
				_bclim = BCLIM.Load(new FileStream(_fileInfo.FullName, FileMode.Open, FileAccess.Read));
			else
				result = LoadResult.FileNotFound;

			return result;
		}

		public SaveResult Save(string filename = "")
		{
			SaveResult result = SaveResult.Success;

			if (filename.Trim() != string.Empty)
				_fileInfo = new FileInfo(filename);

			try
			{
				//_msbt.Save(_fileInfo.FullName);
			}
			catch (Exception)
			{
				result = SaveResult.Failure;
			}

			return result;
		}

		// Bitmaps
		public IEnumerable<Bitmap> Bitmaps
		{
			get
			{
				if (_bitmaps == null)
				{
					_bitmaps = new List<Bitmap>();
					_bitmaps.Add(_bclim);
				}

				return _bitmaps;
			}
		}
	}
}