using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;

namespace Code_Lab_01 {

	public class LabClass {

		/// <summary>
		/// 在CAD的命令窗口栏中输出 "Hello World!" 字符串。
		/// </summary>
		[CommandMethod("HelloWorld")]
		public void HelloWorld() {
			Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
			editor.WriteMessage("Hello World!");
		}

	}
}
