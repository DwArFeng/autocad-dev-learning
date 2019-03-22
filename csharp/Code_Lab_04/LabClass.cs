using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System.Windows.Forms;

namespace Code_Lab_04 {

	public class LabClass {

		#region 侦听器代理定义。
		private static void Database_BeginSave(object sender, DatabaseIOEventArgs e) {
			MessageBox.Show(
				"FireBeginSave: " + e.ToString() + "\n"
				+ "该侦听在文件保存的时候被触发，保存的文件名称为: " + e.FileName
			);
		}

		private static void DataBase_ObjectAppended(object sender, ObjectEventArgs e) {
			MessageBox.Show(
				"FireObjectAppended: " + e.ToString() + "\n"
				+ "该侦听在数据库对象添加之后被触发，添加的对象的类型是: " + e.DBObject.GetType().ToString()
			);
		}

		private static void Database_ObjectErased(object sender, ObjectErasedEventArgs e) {
			MessageBox.Show(
				"FireObjectErased: " + e.ToString() + "\n"
				+ "该侦听在数据库对象删除之后被触发，删除的对象的类型是: " + e.DBObject.GetType().ToString() + "\n"
				+ "是否删除: " + e.Erased.ToString()
			);
		}
		#endregion

		#region 侦听器字段定义。
		private readonly DatabaseIOEventHandler _beginSave = new DatabaseIOEventHandler(Database_BeginSave);
		private readonly ObjectEventHandler _objectAppend = new ObjectEventHandler(DataBase_ObjectAppended);
		private readonly ObjectErasedEventHandler _objectErased = new ObjectErasedEventHandler(Database_ObjectErased);
		#endregion


		[CommandMethod("RegisterDatabaseEventHandler")]
		public void RegisterDatabaseEventHandler() {
			#region 定义文档、数据库以及控制台。
			//定义文档、数据库以及控制台。
			Document document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Database database = document.Database;
			Editor editor = document.Editor;
			#endregion

			//添加 BeginSave 的侦听。
			database.BeginSave += _beginSave;
			//添加 ObjectAppended 的侦听。
			database.ObjectAppended += _objectAppend;
			//添加 ObjectErased 的侦听。
			database.ObjectErased += _objectErased;

			//向控制台输出提示文本。
			editor.WriteMessage("侦听器已经添加完毕，现在您可以尝试对数据库进行操作...");
		}

		[CommandMethod("UnregisterDatabaseEventHandler")]
		public void UnregisterDatabaseEventHandler() {
			#region 定义文档、数据库以及控制台。
			//定义文档、数据库以及控制台。
			Document document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
			Database database = document.Database;
			Editor editor = document.Editor;
			#endregion

			//移除 BeginSave 的侦听。
			database.BeginSave -= _beginSave;
			//移除 ObjectAppended 的侦听。
			database.ObjectAppended -= _objectAppend;
			//移除 ObjectErased 的侦听。
			database.ObjectErased -= _objectErased;

			//向控制台输出提示文本。
			editor.WriteMessage("侦听器已经移除完毕!");
		}

	}
}
