using System;
using System.Collections.Generic;
using Gecko.Interop;

namespace Gecko.Cache
{
	public static class CacheService
	{
		private static ComPtr<nsICacheService> _cacheService;

		static CacheService()
		{
			_cacheService = Xpcom.GetService2<nsICacheService>(Contracts.CacheService);
		}

		public static CacheSession CreateSession(string clientID, CacheStoragePolicy storagePolicy, bool streamBased)
		{
#if false
			return new CacheSession(_cacheService.Instance.CreateSession(clientID, (int)storagePolicy, streamBased));
#else
			throw new NotImplementedException();
#endif
		}

		public static string[] Search(string deviceID, Predicate<CacheEntryInfo> predicate)
		{
			string[] ret = null;
			using (var searcher = new CacheSearcher(predicate))
			{
				_cacheService.Instance.VisitEntries(searcher);
				ret = searcher.GetResult();
			}
			return ret;
		}

		/// <summary>
		/// Returns key's of founded entries
		/// 
		/// WARNING. We can't return all CacheEntryInfo entity because after search in becomes invalid :(
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static string[] Search(Predicate<CacheEntryInfo> predicate)
		{
			string[] ret = null;
			using (var searcher = new CacheSearcher(predicate))
			{
				_cacheService.Instance.VisitEntries(searcher);
				ret = searcher.GetResult();
			}
			return ret;
		}

		/// <summary>
		/// This method evicts all entries in all devices implied by the storage policy.
		/// </summary>
		/// <param name="storagePolicy">The cache storage policy.</param>
		/// <remarks>This function may evict some items but will throw if it fails to evict everything.</remarks>
		/// <exception cref="System.Runtime.InteropServices.COMException"></exception>
		public static void Clear(CacheStoragePolicy storagePolicy)
		{	
#if false
			_cacheService.Instance.EvictEntries((int)storagePolicy);
#else
			throw new NotImplementedException();
#endif
		}

		public static readonly string MemoryCacheDevice = "memory";
		public static readonly string DiskCacheDevice = "disk";
	}

	internal sealed class CacheSearcher
		: nsICacheVisitor,IDisposable
	{
		private Predicate<CacheEntryInfo> _predicate;
		private string _deviceID;

		private List<string> _foundEntries = new List<string>();

		internal CacheSearcher(Predicate<CacheEntryInfo> predicate)
			:this(null,predicate)
		{
			
		}

		internal CacheSearcher(string deviceID, Predicate<CacheEntryInfo> predicate)
		{
			_deviceID = deviceID;
			_predicate = predicate;
		}

		~CacheSearcher()
		{
			_foundEntries.Clear();
		}

		public void Dispose()
		{
			_foundEntries.Clear();
			GC.SuppressFinalize( this );
		}

		public bool VisitDevice( string deviceID, nsICacheDeviceInfo deviceInfo )
		{
			if (string.IsNullOrEmpty(_deviceID)) return true;
			return string.Equals( _deviceID, deviceID );
		}

		public bool VisitEntry( string deviceID, nsICacheEntryInfo entryInfo )
		{
			if ( entryInfo == null ) return true;
			var entry = new CacheEntryInfo(entryInfo);
			if (_predicate(entry))
			{
				_foundEntries.Add( entry.Key );
			}
			return true;
		}

		public string[] GetResult()
		{
			return _foundEntries.ToArray();
		}
		
	}

	public enum CacheStoragePolicy
	{
		Anywhere = 0,
		InMemory = 1,
		OnDisk = 2,
		OnDiskAsFile = 3,
		Offline = 4
	}
}