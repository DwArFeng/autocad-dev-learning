using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

namespace Code_Lab_06 {

	public class LabClass {

		/// <summary>
		/// 输出从数据库中找到的第一个元素的信息。
		/// </summary>
		[CommandMethod("TraverseNamedDictionary")]
		public void TraverseNamedDictionary() {
			#region 定义文档、数据库以及控制台。
			//定义文档、数据库以及控制台。
			Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Database database = doc.Database;
			Editor editor = doc.Editor;
			#endregion

			//开启事务。
			Transaction transaction = database.TransactionManager.StartTransaction();
			try {
				//获取数据库中 NamedDictionary 对应的 ObjectId。
				ObjectId namedDictionaryId = database.NamedObjectsDictionaryId;
				//通过指定的 ObjectId 打开数据库的 NamedDictionary。
				DBDictionary namedDictionary = (DBDictionary)transaction.GetObject(namedDictionaryId, OpenMode.ForRead);

				//输出 NamedDictionary 中的数据数量。
				editor.WriteMessage("---------------命名对象词典---------------\r\n");
				editor.WriteMessage(String.Format("当前词典中的数据量为: {0:G}\r\n", namedDictionary.Count));

				//遍历 NamedDictionary 中的入口。
				editor.WriteMessage("---------------词典入口---------------\r\n");
				{
					int index = 0;
					foreach (DBDictionaryEntry entry in namedDictionary) {
						editor.WriteMessage(String.Format(
							"第 {0:G} 个入口，其键为: {1}, 其值为: {2}, 值的对象类型为: {3}\r\n",
							++index,
							entry.Key,
							entry.Value.ToString(),
							transaction.GetObject(entry.Value, OpenMode.ForRead).GetType().Name
						));
					}
				}

				transaction.Commit();

			} catch (Autodesk.AutoCAD.Runtime.Exception e) {
				editor.WriteMessage(e.Message);
				//如果操作过程中产生了任何异常，则中止事务。
				transaction.Abort();
			} finally {
				//在final语句块中释放事务。
				transaction.Dispose();
			}
		}

	}
}
