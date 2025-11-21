namespace Cirreum.FileSystem;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

sealed class NotImplementedFileSystem : IFileSystem {

	public void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs, bool overwrite = false) {
		throw new NotImplementedException();
	}

	public void CopyFile(string sourceFileName, string destFileName, bool overwrite = false) {
		throw new NotImplementedException();
	}

	public void DeleteChildDirectories(string rootPath) {
		throw new NotImplementedException();
	}

	public void DeleteDirectory(string path, bool recursive) {
		throw new NotImplementedException();
	}

	public void DeleteFile(string path) {
		throw new NotImplementedException();
	}

	public void DeleteFileWithRetry(string path) {
		throw new NotImplementedException();
	}

	public bool DirectoryExists(string path) {
		throw new NotImplementedException();
	}

	public bool EnsureDirectory(string path) {
		throw new NotImplementedException();
	}

	public void ExtractSevenZFile(string source, string destination, int secondsToWait = 30) {
		throw new NotImplementedException();
	}

	public void ExtractZipFile(string source, string destination, bool overwriteFiles) {
		throw new NotImplementedException();
	}

	public bool FileExists(string path) {
		throw new NotImplementedException();
	}

	public string[] GetFiles(string path, string searchPattern, bool includeChildDirectories) {
		throw new NotImplementedException();
	}

	public void MoveDirectory(string sourceDirName, string destDirName) {
		throw new NotImplementedException();
	}

	public void MoveFile(string sourceFileName, string destFileName, bool overwrite = false) {
		throw new NotImplementedException();
	}

	public IEnumerable<string> QueryDirectories(string[] paths, bool includeChildDirectories, string searchPattern, int take = 0, Func<string, bool>? predicate = null) {
		throw new NotImplementedException();
	}

	public IEnumerable<string> QueryDirectories(string path, bool includeChildDirectories, string searchPattern, int take = 0, Func<string, bool>? predicate = null) {
		throw new NotImplementedException();
	}

	public IEnumerable<string> QueryDirectories(string path, bool includeChildDirectories, IEnumerable<string> searchPatterns, int take = 0, Func<string, bool>? predicate = null) {
		throw new NotImplementedException();
	}

	public long QueryFileCount(string directory, bool includeChildDirectories, string searchPattern, Func<string, bool>? predicate = null, FileAttributes attributesToSkip = FileAttributes.Hidden | FileAttributes.System) {
		throw new NotImplementedException();
	}

	public IEnumerable<string> QueryFiles(string[] paths, bool includeChildDirectories, string searchPattern, int take = 0, Func<string, bool>? predicate = null) {
		throw new NotImplementedException();
	}

	public IEnumerable<string> QueryFiles(string path, bool includeChildDirectories, string searchPattern, int take = 0, Func<string, bool>? predicate = null) {
		throw new NotImplementedException();
	}

	public IEnumerable<string> QueryFiles(string path, bool includeChildDirectories, IEnumerable<string> searchPatterns, int take = 0, Func<string, bool>? predicate = null) {
		throw new NotImplementedException();
	}

	public void WriteAllText(string path, string contents) {
		throw new NotImplementedException();
	}

	public Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default) {
		throw new NotImplementedException();
	}

	public string ReadAllText(string path) {
		throw new NotImplementedException();
	}

	public Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default) {
		throw new NotImplementedException();
	}
}