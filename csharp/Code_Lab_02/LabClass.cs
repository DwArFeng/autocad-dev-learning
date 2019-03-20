using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

namespace Code_Lab_02 {

	public class LabClass {

		/// <summary>
		/// <para>通过控制台提示用户选择一个位置（点），然后将这个位置（点）输出在控制台上。</para>
		/// <para>在执行命令中，如果用户取消，则输出"发生错误"。</para>
		/// </summary>
		[CommandMethod("PromptPoint")]
		public void PromptPoint() {
			Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Editor editor = doc.Editor;

			PromptPointOptions options = new PromptPointOptions("请选择一个点...");
			PromptPointResult result = editor.GetPoint(options);

			if (result.Status != PromptStatus.OK) {
				editor.WriteMessage("发生错误 !");
				return;
			}

			editor.WriteMessage("你选择的点是: " + result.Value.ToString());
		}

	}
}
