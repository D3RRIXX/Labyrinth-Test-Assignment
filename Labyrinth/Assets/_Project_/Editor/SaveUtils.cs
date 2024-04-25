using System.Diagnostics;
using System.IO;
using Labyrinth.Infrastructure.SaveSystem;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor
{
	public static class SaveUtils
	{
		[MenuItem("Tools/Open Save Folder")]
		public static void OpenPersistentDataPathInExplorer()
		{
			string path = Application.persistentDataPath;

			if (!Directory.Exists(path))
			{
				Debug.LogWarning($"Persistent data path does not exist: {path}");
				return;
			}

			Process.Start("explorer.exe", path);
		}

		[MenuItem("Tools/Delete Game Save")]
		private static void DeleteSave()
		{
			if (!File.Exists(SaveManager.SAVE_PATH))
				return;
			
			File.Delete(SaveManager.SAVE_PATH);
		}
	}
}