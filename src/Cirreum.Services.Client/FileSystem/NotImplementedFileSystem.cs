namespace Cirreum.FileSystem;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

sealed class NotImplementedFileSystem : IFileSystem {

	void IFileSystem.CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs, bool overwrite) {
		throw new NotImplementedException();
	}

	void IFileSystem.CopyFile(string sourceFileName, string destFileName, bool overwrite) {
		throw new NotImplementedException();
	}

	void IFileSystem.DeleteChildDirectories(string rootPath) {
		throw new NotImplementedException();
	}

	void IFileSystem.DeleteDirectory(string path, bool recursive) {
		throw new NotImplementedException();
	}

	void IFileSystem.DeleteFile(string path) {
		throw new NotImplementedException();
	}

	void IFileSystem.DeleteFileWithRetry(string path) {
		throw new NotImplementedException();
	}

	bool IFileSystem.DirectoryExists(string path) {
		throw new NotImplementedException();
	}

	bool IFileSystem.EnsureDirectory(string path) {
		throw new NotImplementedException();
	}

	void IFileSystem.ExtractZipFile(string source, string destination, bool overwriteFiles) {
		throw new NotImplementedException();
	}

	bool IFileSystem.FileExists(string path) {
		throw new NotImplementedException();
	}

	string[] IFileSystem.GetFiles(string path, string searchPattern, bool includeChildDirectories) {
		throw new NotImplementedException();
	}

	void IFileSystem.MoveDirectory(string sourceDirName, string destDirName) {
		throw new NotImplementedException();
	}

	void IFileSystem.MoveFile(string sourceFileName, string destFileName, bool overwrite) {
		throw new NotImplementedException();
	}

	IEnumerable<string> IFileSystem.QueryDirectories(string[] paths, bool includeChildDirectories, string searchPattern, Func<string, bool>? predicate, int take) {
		throw new NotImplementedException();
	}

	IEnumerable<string> IFileSystem.QueryDirectories(string path, bool includeChildDirectories, string searchPattern, Func<string, bool>? predicate, int take) {
		throw new NotImplementedException();
	}

	IEnumerable<string> IFileSystem.QueryDirectories(string path, bool includeChildDirectories, IEnumerable<string> searchPatterns, Func<string, bool>? predicate, int take) {
		throw new NotImplementedException();
	}

	IEnumerable<string> IFileSystem.QueryFiles(string[] paths, bool includeChildDirectories, string searchPattern, Func<string, bool>? predicate, int take) {
		throw new NotImplementedException();
	}

	IEnumerable<string> IFileSystem.QueryFiles(string path, bool includeChildDirectories, string searchPattern, Func<string, bool>? predicate, int take) {
		throw new NotImplementedException();
	}

	IEnumerable<string> IFileSystem.QueryFiles(string path, bool includeChildDirectories, IEnumerable<string> searchPatterns, Func<string, bool>? predicate, int take) {
		throw new NotImplementedException();
	}

	string IFileSystem.ReadAllText(string path) {
		throw new NotImplementedException();
	}

	Task<string> IFileSystem.ReadAllTextAsync(string path, CancellationToken cancellationToken) {
		throw new NotImplementedException();
	}

	void IFileSystem.WriteAllText(string path, string contents) {
		throw new NotImplementedException();
	}

	Task IFileSystem.WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken) {
		throw new NotImplementedException();
	}

}