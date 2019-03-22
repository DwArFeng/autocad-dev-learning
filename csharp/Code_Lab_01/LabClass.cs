using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

namespace Code_Lab_01 {

	public class LabClass {

		/// <summary>
		/// 在CAD的命令窗口栏中输出 "Hello World!" 字符串。
		/// </summary>
		[CommandMethod("HelloWorld")]
		public void HelloWorld() {
			//获取当前文档对应的控制台。
			Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
			//在控制台上输出 "Hello World!" 字符串
			editor.WriteMessage("Hello World!");
		}

	}
}
