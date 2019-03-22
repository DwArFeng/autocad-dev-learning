using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

namespace Code_Lab_02 {

	public class LabClass {

		/// <summary>
		/// <para>通过控制台提示用户选择一个位置（点），然后将这个位置（点）输出在控制台上。</para>
		/// <para>在执行命令中，如果用户取消，则输出"发生错误"。</para>
		/// </summary>
		[CommandMethod("PromptPoint")]
		public void PromptPoint() {
			#region 定义文档以及控制台。
			//定义文档以及控制台。
			Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Editor editor = doc.Editor;
			#endregion

			//新建一个提示对象，在入口参数中定义该提示对象在控制台中提醒用户的文本。
			PromptPointOptions options = new PromptPointOptions("请选择一个点...");
			//控制台调用新建的提示对象，并返回一个提示结果。
			PromptPointResult result = editor.GetPoint(options);

			//判断提示结果的状态，当且仅当状态为 OK 时，才继续向下执行，否则使用控制台输出报错并返回。
			if (result.Status != PromptStatus.OK) {
				editor.WriteMessage("发生错误 !");
				return;
			}

			//通过控制台输出用户拾取的点，这里采用的是 ToString() 方法，当然可以使用任何方式进行输出。
			editor.WriteMessage("你选择的点是: " + result.Value.ToString());
		}

	}
}
